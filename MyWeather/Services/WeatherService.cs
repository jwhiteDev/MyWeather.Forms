using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Xamarin.Forms;

using Newtonsoft.Json;

using MyWeather.Models;
using MyWeather.Helpers;

namespace MyWeather.Services
{
	public enum Units
	{
		Imperial,
		Metric
	}

	public static class WeatherService
	{
		#region Constant Fields
		const string _weatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid=fc9f6c524fc093759cd28d41fda89a1b";
		const string _weatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units={1}&appid=fc9f6c524fc093759cd28d41fda89a1b";
		const string _forecaseUri = "http://api.openweathermap.org/data/2.5/forecast?id={0}&units={1}&appid=fc9f6c524fc093759cd28d41fda89a1b";

		static readonly TimeSpan _httpTimeout = TimeSpan.FromSeconds(20);
		static readonly JsonSerializer _serializer = new JsonSerializer();
		#endregion

		#region Fields
		static HttpClient _client;
		#endregion

		#region Properties
		static HttpClient Client
		{
			get 
			{
				IntializeHttpClient();
				return _client;
			}
		}
		#endregion

		#region Methods
		public static async Task<WeatherRoot> GetWeather(double latitude, double longitude, Units units = Units.Imperial)
		{
			return await GetDataObjectFromAPI<WeatherRoot>(string.Format(_weatherCoordinatesUri, latitude, longitude, units.ToString().ToLower()));

		}

		public static async Task<WeatherRoot> GetWeather(string city, Units units = Units.Imperial)
		{
			return await GetDataObjectFromAPI<WeatherRoot>(string.Format(_weatherCityUri, city, units.ToString().ToLower()));

		}

		public static async Task<WeatherForecastRoot> GetForecast(int id, Units units = Units.Imperial)
		{
			return await GetDataObjectFromAPI<WeatherForecastRoot>(string.Format(_forecaseUri, id, units.ToString().ToLower()));
		}

		static async Task<T> GetDataObjectFromAPI<T>(string apiUrl)
		{
			return await Task.Run(async () =>
			{
				try
				{
					var response = await Client.GetAsync(apiUrl);
					using (var stream = await response.Content.ReadAsStreamAsync())
					using (var reader = new StreamReader(stream))
					using (var json = new JsonTextReader(reader))
					{
						if (json == null)
							return default(T);

						return _serializer.Deserialize<T>(json);
					}
				}
				catch (Exception e)
				{
					HockeyappHelpers.Report(e);
					return default(T);
				}
			});
		}

		static void IntializeHttpClient()
		{
			if (_client != null)
				return;

			if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
				_client = new HttpClient { Timeout = _httpTimeout };
			else
				_client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip })
				{
					Timeout = _httpTimeout
				};

			_client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
		}
		#endregion
	}
}

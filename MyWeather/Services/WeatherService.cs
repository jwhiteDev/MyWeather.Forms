using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

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
		const string _weatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid=fc9f6c524fc093759cd28d41fda89a1b";
		const string _weatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units={1}&appid=fc9f6c524fc093759cd28d41fda89a1b";
		const string _forecaseUri = "http://api.openweathermap.org/data/2.5/forecast?id={0}&units={1}&appid=fc9f6c524fc093759cd28d41fda89a1b";

		static readonly TimeSpan _httpTimeout = TimeSpan.FromSeconds(20);
		static readonly HttpClient _client = new HttpClient { Timeout = _httpTimeout };
		static readonly JsonSerializer _serializer = new JsonSerializer();

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
					var response = await _client.GetAsync(apiUrl);
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
	}
}

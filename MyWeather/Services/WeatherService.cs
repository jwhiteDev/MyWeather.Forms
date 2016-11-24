using MyWeather.Models;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;
using System;
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
		const string WeatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid=fc9f6c524fc093759cd28d41fda89a1b";
		const string WeatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units={1}&appid=fc9f6c524fc093759cd28d41fda89a1b";
		const string ForecaseUri = "http://api.openweathermap.org/data/2.5/forecast?id={0}&units={1}&appid=fc9f6c524fc093759cd28d41fda89a1b";

		static readonly TimeSpan HttpTimeout = TimeSpan.FromSeconds(20);
		static readonly HttpClient Client = new HttpClient { Timeout = HttpTimeout };

		public static async Task<WeatherRoot> GetWeather(double latitude, double longitude, Units units = Units.Imperial)
		{
			return await GetDataObjectFromAPI<WeatherRoot>(string.Format(WeatherCoordinatesUri, latitude, longitude, units.ToString().ToLower()));

		}

		public static async Task<WeatherRoot> GetWeather(string city, Units units = Units.Imperial)
		{
			return await GetDataObjectFromAPI<WeatherRoot>(string.Format(WeatherCityUri, city, units.ToString().ToLower()));

		}

		public static async Task<WeatherForecastRoot> GetForecast(int id, Units units = Units.Imperial)
		{
			return await GetDataObjectFromAPI<WeatherForecastRoot>(string.Format(ForecaseUri, id, units.ToString().ToLower()));
		}

		static async Task<T> GetDataObjectFromAPI<T>(string apiUrl)
		{
			return await Task.Run(async () =>
			{
				try
				{
					var json = await Client.GetStringAsync(apiUrl);

					if (string.IsNullOrWhiteSpace(json))
						return default(T);

					return DeserializeObject<T>(json);
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

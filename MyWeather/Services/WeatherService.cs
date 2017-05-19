using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Newtonsoft.Json;

using MyWeather.Models;
using MyWeather.Helpers;

namespace MyWeather.Services
{
    public enum Units { Imperial, Metric }

    public static class WeatherService
    {
        #region Constant Fields
        const string _weatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid=fc9f6c524fc093759cd28d41fda89a1b";
        const string _weatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units={1}&appid=fc9f6c524fc093759cd28d41fda89a1b";
        const string _forecaseUri = "http://api.openweathermap.org/data/2.5/forecast?id={0}&units={1}&appid=fc9f6c524fc093759cd28d41fda89a1b";

        static readonly TimeSpan _httpTimeout = TimeSpan.FromSeconds(20);
        static readonly JsonSerializer _serializer = new JsonSerializer();
        static readonly HttpClient _client = CreateHttpClient();
        #endregion

        #region Methods
        public static async Task<WeatherRoot> GetWeather(double latitude, double longitude, Units units = Units.Imperial) =>
            await GetDataObjectFromAPI<WeatherRoot>(string.Format(_weatherCoordinatesUri, latitude, longitude, units.ToString().ToLower()));

        public static async Task<WeatherRoot> GetWeather(string city, Units units = Units.Imperial) =>
            await GetDataObjectFromAPI<WeatherRoot>(string.Format(_weatherCityUri, city, units.ToString().ToLower()));

        public static async Task<WeatherForecastRoot> GetForecast(int id, Units units = Units.Imperial) =>
            await GetDataObjectFromAPI<WeatherForecastRoot>(string.Format(_forecaseUri, id, units.ToString().ToLower()));

        static async Task<T> GetDataObjectFromAPI<T>(string apiUrl)
        {
            try
            {
                using (var stream = await _client.GetStreamAsync(apiUrl).ConfigureAwait(false))
                using (var reader = new StreamReader(stream))
                using (var json = new JsonTextReader(reader))
                {
                    if (json == null)
                        return default(T);

                    return await Task.Run(() => _serializer.Deserialize<T>(json));
                }
            }
            catch (Exception e)
            {
                HockeyappHelpers.Report(e);
                return default(T);
            }
        }

        static HttpClient CreateHttpClient()
        {
            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip })
            {
                Timeout = _httpTimeout
            };

            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            return client;
        }
        #endregion
    }
}

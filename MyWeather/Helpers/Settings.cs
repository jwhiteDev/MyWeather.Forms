using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MyWeather.Helpers
{
    public static class Settings
    {
        #region Constant Fields
        const string IsImperialKey = "is_imperial";
        const string UseCityKey = "use_city";
        const string CityKey = "city";
        const string CityDefault = "Seattle,WA";

        static readonly bool IsImperialDefault = true;
        static readonly bool UseCityDefault = true;
        #endregion

        #region Properties
        public static bool IsImperial
        {
            get => AppSettings.GetValueOrDefault(IsImperialKey, IsImperialDefault);
            set => AppSettings.AddOrUpdateValue(IsImperialKey, value);
        }

        public static bool UseCity
        {
            get => AppSettings.GetValueOrDefault(UseCityKey, UseCityDefault);
            set => AppSettings.AddOrUpdateValue(UseCityKey, value);
        }

        public static string City
        {
            get => AppSettings.GetValueOrDefault(CityKey, CityDefault);
            set => AppSettings.AddOrUpdateValue(CityKey, value);
        }

		static ISettings AppSettings => CrossSettings.Current;
        #endregion

    }
}
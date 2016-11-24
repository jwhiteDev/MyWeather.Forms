using System;
using System.Threading;

using NUnit.Framework;

using Xamarin.UITest;

namespace MyWeather.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests : BaseTest
	{
		public Tests(Platform platform) : base(platform)
		{
		}

		public override void TestSetup()
		{
			base.TestSetup();
			WeatherPage.WaitForPageToLoad();
			App.Screenshot("App Launched");
		}

		[TestCase("San Francisco,CA", true)]
		[TestCase("San Francisco,CA", false)]
		[TestCase("Orlando,FL", true)]
		[TestCase("Orlando,FL", false)]
		[TestCase("New York,NY", true)]
		[TestCase("New York,NY", false)]
		[Test]
		public void GetWeatherUsingText(string location, bool toggleScreensBeforeTest)
		{
			//Arrange
			string expectedConditionCityText = ParseConditionCityFromString(location, ',');
			string actualTemperatureLabelText, actualConditionCityText;

			//Act
			ToggleScreens(toggleScreensBeforeTest);

			WeatherPage.EnterLocation(location);
			WeatherPage.TapGetWeatherButton();

			Thread.Sleep(2000);

			WeatherPage.WaitForNoActivityIndicator();

			//Assert
			actualConditionCityText = ParseConditionCityFromString(WeatherPage.GetConditionText(), ':');
			actualTemperatureLabelText = WeatherPage.GetTemperatureText();
			Assert.AreEqual(expectedConditionCityText, actualConditionCityText, "Exptected Condition City Does Not Match Actual Condition City");
			Assert.IsNotEmpty(actualTemperatureLabelText, "Temperature Text Is Empty");
		}


		[TestCase(true)]
		[TestCase(false)]
		[Test]
		public void GetWeatherUsingGPS(bool toggleScreensBeforeTest)
		{
			//Arrange
			string actualTemperatureLabelText, actualConditionLabelText;

			//Act
			ToggleScreens(toggleScreensBeforeTest);

			WeatherPage.ToggleGPSSwitch();
			WeatherPage.TapGetWeatherButton();

			Thread.Sleep(2000);

			WeatherPage.WaitForNoActivityIndicator();

			//Assert
			actualConditionLabelText = WeatherPage.GetConditionText();
			actualTemperatureLabelText = WeatherPage.GetTemperatureText();
			Assert.IsNotEmpty(actualConditionLabelText, "Condition Text Is Empty");
			Assert.IsNotEmpty(actualTemperatureLabelText, "Temperature Text Is Empty");
		}

		[Ignore("This Test Will Crash The App")]
		[Test]
		public void TapCrashButton()
		{
			//Arrange

			//Act
			WeatherPage.TapForecastTab();
			ForecastPage.TapCrashButton();

			//Assert
		}

		[Test]
		public void TapFeedbackButton()
		{
			//Arrange

			//Act
			WeatherPage.TapFeedbackButton();

			//Assert
			Assert.IsTrue(WeatherPage.IsFeedbackPageOpen());
		}

		void ToggleScreens(bool isToggleScreensEnabled)
		{
			if (!isToggleScreensEnabled)
				return;

			if (WeatherPage.IsWeatherPageVisible())
			{
				WeatherPage.TapForecastTab();
				ForecastPage.TapWeatherTab();
			}
			else
			{
				ForecastPage.TapWeatherTab();
				WeatherPage.TapForecastTab();
			}
		}

		string ParseConditionCityFromString(string location, char firstCharacterAfterCityText)
		{
			var indexOfFirstCharacterAfterCityTextInLocationString = location.IndexOf(firstCharacterAfterCityText.ToString(), StringComparison.Ordinal);

			return location.Substring(0, Math.Max(indexOfFirstCharacterAfterCityTextInLocationString, 0));
		}
	}
}

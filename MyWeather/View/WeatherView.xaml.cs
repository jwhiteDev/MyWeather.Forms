using Xamarin.Forms;

using HockeyApp;
using System;
using MyWeather.Helpers;
using MyWeather.Services;

namespace MyWeather.View
{
	public partial class WeatherView : ContentPage
	{
		public WeatherView()
		{
			InitializeComponent();

			if (Device.OS == TargetPlatform.iOS)
				Icon = new FileImageSource { File = "tab1.png" };

			var feedbackToolBar = new ToolbarItem
			{
				Icon = "Add",
				AutomationId = AutomationIdConstants.FeedbackButton
			};
			feedbackToolBar.Clicked += (sender, e) =>
			{
				DependencyService.Get<IHockeyappFeedbackService>()?.GiveFeedback();
			};
			ToolbarItems.Add(feedbackToolBar);

			InitializeAutomationIds();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			HockeyappHelpers.TrackEvent(HockeyappConstants.WeatherPageAppeared);
		}

		void InitializeAutomationIds()
		{
			TempLabel.AutomationId = AutomationIdConstants.TempLabel;
			UseGPSSwitch.AutomationId = AutomationIdConstants.UseGPSSwitch;
			LocationEntry.AutomationId = AutomationIdConstants.LocationEntry;
			ConditionLabel.AutomationId = AutomationIdConstants.ConditionLabel;
			GetWeatherButton.AutomationId = AutomationIdConstants.GetWeatherButton;
			GetWeatherActivityIndicator.AutomationId = AutomationIdConstants.GetWeatherActivityIndicator;
		}
	}
}

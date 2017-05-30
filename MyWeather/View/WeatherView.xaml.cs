using Xamarin.Forms;

using MyWeather.Shared;
using MyWeather.Helpers;
using MyWeather.ViewModels;

namespace MyWeather.View
{
	public partial class WeatherView : ContentPage
	{
		public WeatherView()
		{
			InitializeComponent();

			Icon = new FileImageSource { File = "tab1.png" };

			var viewModel = BindingContext as WeatherViewModel;
			var feedbackToolBarItem = new ToolbarItem
			{
				Icon = "Add",
				AutomationId = AutomationIdConstants.FeedbackButton
			};
			feedbackToolBarItem.SetBinding(ToolbarItem.CommandProperty, nameof(viewModel.FeedbackButtonTapped));
			ToolbarItems.Add(feedbackToolBarItem);

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

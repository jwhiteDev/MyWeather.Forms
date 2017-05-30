using Xamarin.Forms;

using MyWeather.Shared;
using MyWeather.Helpers;
using MyWeather.ViewModels;

namespace MyWeather.View
{
	public partial class ForecastView : ContentPage
	{
		public ForecastView()
		{
			InitializeComponent();

			Icon = new FileImageSource { File = "tab2.png" };

			ListViewWeather.ItemTapped += (sender, args) => ListViewWeather.SelectedItem = null;

#if DEBUG
			var viewModel = BindingContext as WeatherViewModel;
			var crashButtonToolBarItem = new ToolbarItem
			{
				Icon = "Crash",
				AutomationId = AutomationIdConstants.CrashButton
			};
			crashButtonToolBarItem.SetBinding(ToolbarItem.CommandProperty, nameof(viewModel.CrashButtonTapped));
			ToolbarItems.Add(crashButtonToolBarItem);
#endif
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			HockeyappHelpers.TrackEvent(HockeyappConstants.ForecastPageAppeared);
		}
	}
}

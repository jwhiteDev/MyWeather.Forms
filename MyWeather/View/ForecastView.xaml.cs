using System;

using Xamarin.Forms;

using MyWeather.Helpers;

namespace MyWeather.View
{
	public partial class ForecastView : ContentPage
	{
		public ForecastView()
		{
			InitializeComponent();
			if (Device.OS == TargetPlatform.iOS)
				Icon = new FileImageSource { File = "tab2.png" };
			ListViewWeather.ItemTapped += (sender, args) => ListViewWeather.SelectedItem = null;

#if DEBUG
			var crashButtonToolBarItem = new ToolbarItem
			{
				Icon = "Crash",
				AutomationId = AutomationIdConstants.CrashButton
			};
			crashButtonToolBarItem.SetBinding(ToolbarItem.CommandProperty, "CrashButtonTapped");
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

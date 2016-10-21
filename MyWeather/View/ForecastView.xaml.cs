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
			var crashButtonToolBar = new ToolbarItem
			{
				Icon = "Crash",
				AutomationId = AutomationIdConstants.CrashButton
			};
			crashButtonToolBar.Clicked += (sender, e) =>
			{
				try
				{
					HockeyappHelpers.TrackEvent(HockeyappConstants.CrashButtonTapped);
					throw new System.Exception(HockeyappConstants.CrashButtonTapped);
				}
				catch (Exception ex)
				{
					HockeyappHelpers.Report(ex);
				}
			};
			ToolbarItems.Add(crashButtonToolBar);
#endif
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			HockeyappHelpers.TrackEvent(HockeyappConstants.ForecastPageAppeared);
		}
	}
}

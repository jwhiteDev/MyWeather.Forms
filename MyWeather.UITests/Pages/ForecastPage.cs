using System;
using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MyWeather.UITests
{
	public class ForecastPage : BasePage
	{
		Query Crashbutton;

		public ForecastPage(IApp app, Platform platform) : base(app, platform, AutomationIdConstants.ForecastPageTitle)
		{
			if (OniOS)
				Crashbutton = x => x.Marked(AutomationIdConstants.CrashButton);
			else
				Crashbutton = x => x.Class("ActionMenuItemView");
		}

		public void TapCrashButton()
		{
			App.Tap(Crashbutton);
			App.Screenshot("Crash Button Tapped");
		}
	}
}

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

using HockeyApp.Android;

using MyWeather.Droid;
using MyWeather.Services;


[assembly: Xamarin.Forms.Dependency(typeof(HockeyappService_Android))]
namespace MyWeather.Droid
{
	public class HockeyappService_Android : IHockeyappService
	{
		public async Task GiveFeedback()
		{
			await Task.Run(() => FeedbackManager.ShowFeedbackActivity(Forms.Context));
		}

		public void TrackEvent(string eventName)
		{
			throw new NotImplementedException();
		}

		public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements)
		{
			throw new NotImplementedException();
		}
	}
}

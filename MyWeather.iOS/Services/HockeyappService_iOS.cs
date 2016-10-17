using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using UIKit;

using HockeyApp.iOS;

using MyWeather.iOS;
using MyWeather.Services;

[assembly: Xamarin.Forms.Dependency(typeof(HockeyappService_iOS))]
namespace MyWeather.iOS
{
	public class HockeyappService_iOS : IHockeyappService
	{
		public async Task GiveFeedback()
		{
			var feedbackManager = BITHockeyManager.SharedHockeyManager.FeedbackManager;
			var selectedButtonIndex = await ShowFeedbackAlert();

			if (selectedButtonIndex == 0)
				// Show current feedback
				feedbackManager.ShowFeedbackListView();
			else
				// Send new feedback
				feedbackManager.ShowFeedbackComposeView();
		}

		public void TrackEvent(string eventName)
		{
			throw new NotImplementedException();
		}

		public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements)
		{
			throw new NotImplementedException();
		}

		async Task<nint> ShowFeedbackAlert()
		{
			var tcs = new TaskCompletionSource<nint>();

			var alert = new UIAlertView
			{
				Title = "Give Feedback",
			};
			alert.AddButton("Review Existing Feedback");
			alert.AddButton("Submit New Feedback");

			alert.Clicked += (sender, buttonArgs) => tcs.SetResult(buttonArgs.ButtonIndex);

			UIApplication.SharedApplication.InvokeOnMainThread(() => alert.Show());

			while(alert.Visible)
			{
				await Task.Delay(100);
			}

			return tcs.Task.Result;
		}
	}
}

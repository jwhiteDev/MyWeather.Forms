using System.Threading.Tasks;

using Xamarin.Forms;

using HockeyApp.Android;

using MyWeather.Droid;


[assembly: Xamarin.Forms.Dependency(typeof(HockeyappFeedbackService_Android))]
namespace MyWeather.Droid
{
	public class HockeyappFeedbackService_Android : IHockeyappFeedbackService
	{
		public async Task GiveFeedback()
		{
			await Task.Run(() => FeedbackManager.ShowFeedbackActivity(Forms.Context));
		}
	}
}

using System.Threading.Tasks;
using System.Collections.Generic;

using MyWeather.Services;
using MyWeather.UWP.Services;

[assembly: Xamarin.Forms.Dependency(typeof(HockeyappFeedbackService_UWP))]

namespace MyWeather.UWP.Services
{
    public class HockeyappFeedbackService_UWP : IHockeyappFeedbackService
    {
        public Task GiveFeedback()
        {
			throw new Exception("Give Feedback Not Implemented in UWP");
        }
    }
}

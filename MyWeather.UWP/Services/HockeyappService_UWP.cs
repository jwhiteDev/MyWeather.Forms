using System.Threading.Tasks;
using System.Collections.Generic;

using MyWeather.Services;
using MyWeather.UWP.Services;

[assembly: Xamarin.Forms.Dependency(typeof(HockeyappService_UWP))]

namespace MyWeather.UWP.Services
{
    public class HockeyappService_UWP : IHockeyappService
    {
        public Task GiveFeedback()
        {
            return null;
        }

        public void TrackEvent(string eventName)
        {
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(eventName);
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements)
        {
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(eventName, properties, measurements);
        }
    }
}

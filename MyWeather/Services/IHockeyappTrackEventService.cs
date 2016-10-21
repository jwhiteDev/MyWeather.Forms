using System.Collections.Generic;

namespace MyWeather.Services
{
	public interface IHockeyappTrackEventService
	{
		void TrackEvent(string eventName);
		void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements);
	}
}

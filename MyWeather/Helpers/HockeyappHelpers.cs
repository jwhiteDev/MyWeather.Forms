using System;
using System.Collections.Generic;
using MyWeather.Services;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace MyWeather.Helpers
{
	public static class HockeyappHelpers
	{
		enum PathType { Windows, Linux };
		readonly static Dictionary<PathType, string> DirectorySeparator = new Dictionary<PathType, string>
		{
			{PathType.Linux, "/"},
			{PathType.Windows, @"\" }
		};

		public static void TrackEvent(string eventName)
		{
			switch (Device.OS)
			{
				case TargetPlatform.iOS:
				case TargetPlatform.Android:
					HockeyApp.MetricsManager.TrackEvent(eventName);
					break;
				case TargetPlatform.Windows:
					DependencyService.Get<IHockeyappTrackEventService>()?.TrackEvent(eventName);
					break;
			}
		}

		public static void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements)
		{
			switch (Device.OS)
			{
				case TargetPlatform.iOS:
				case TargetPlatform.Android:
					HockeyApp.MetricsManager.TrackEvent(eventName, properties, measurements);
					break;
				case TargetPlatform.Windows:
					DependencyService.Get<IHockeyappTrackEventService>()?.TrackEvent(eventName, properties, measurements);
					break;
			}
		}
		/// <summary>
		/// Reports a caught exception to Hockeyapp
		/// </summary>
		/// <param name="exception">The Exception Caught in the Try/Catch Block.</param>
		/// <param name="currentObjectType">The type of the current object, e.g. GetType().</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callerMembername">Name.</param>
		public static void Report(Exception exception, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerMembername = "")
		{
			var fileName = getFileNameWithoutFilePath(filePath);

			var errorReport = $"Error: {exception.Message}";
			errorReport += $", Line Number: {lineNumber}";
			errorReport += $", Caller Name: {callerMembername}";
			errorReport += $", File Name: {fileName}";
			TrackEvent(errorReport);
		}

		static string getFileNameWithoutFilePath(string filePath)
		{
			string fileName;
			PathType pathType;
			if (filePath.Contains("/"))
				pathType = PathType.Linux;
			else
				pathType = PathType.Windows;

			while (true)
			{
				if(!(filePath.Contains(DirectorySeparator[pathType])))
				{
					fileName = filePath;
					break;
				}

				var indexOfDictionarySeparator = filePath.IndexOf(DirectorySeparator[pathType], StringComparison.Ordinal);
				var newStringStartIndex = indexOfDictionarySeparator + 1;

				filePath = filePath.Substring(newStringStartIndex, filePath.Length - newStringStartIndex);
			}

			return fileName;

		}

	}
}

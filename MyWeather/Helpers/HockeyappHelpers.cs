using System;
using System.Collections.Generic;
using MyWeather.Services;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace MyWeather.Helpers
{
	public static class HockeyappHelpers
	{
		enum _pathType { Windows, Linux };

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
		public static void Report(Exception exception, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerMembername = "")
		{
			var fileName = GetFileNameFromFilePath(filePath);

			var errorReport = $"Error: {exception.Message}";
			errorReport += $", Line Number: {lineNumber}";
			errorReport += $", Caller Name: {callerMembername}";
			errorReport += $", File Name: {fileName}";

			TrackEvent(errorReport);
		}

		static string GetFileNameFromFilePath(string filePath)
		{
			string fileName;
			_pathType pathType;

			var directorySeparator = new Dictionary<_pathType, string>
			{
				{ _pathType.Linux, "/" },
				{ _pathType.Windows, @"\" }
			};

			pathType = filePath.Contains(directorySeparator[_pathType.Linux]) ? _pathType.Linux : _pathType.Windows;

			while (true)
			{
				if (!(filePath.Contains(directorySeparator[pathType])))
				{
					fileName = filePath;
					break;
				}

				var indexOfDictionarySeparator = filePath.IndexOf(directorySeparator[pathType], StringComparison.Ordinal);
				var newStringStartIndex = indexOfDictionarySeparator + 1;

				filePath = filePath.Substring(newStringStartIndex, filePath.Length - newStringStartIndex);
			}

			return fileName;

		}

	}
}

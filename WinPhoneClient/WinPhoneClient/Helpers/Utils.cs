using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using WinPhoneClient.JSON;
using WpWinNl.Utilities;

namespace WinPhoneClient.Helpers
{
    public static class Utils
    {
        public static Rect GetElementBounds(FrameworkElement element, FrameworkElement container)
        {
            if (element == null || container == null)
                return Rect.Empty;

            if (element.Visibility != Visibility.Visible)
                return Rect.Empty;

            return element.TransformToVisual(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static List<T> GetArrayFromJson<T>(BaseJson baseJson) where T : IBaseJsonValue, new()
        {
            if (baseJson != null && baseJson.Result)
            {
                try
                {
                    var data = baseJson.Data;
                    if (data is JsonArray)
                    {
                        var dronesArray = data as JsonArray;
                        if (dronesArray.Any())
                            return (from JsonValue json in dronesArray select new T { Json = json.GetObject() }).ToList();
                    }
                    else
                    {
                        return new List<T> { new T { Json = data as JsonObject } };
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        public static  MapControl GetMainMapControl()
        {
            if (App.MainPage != null)
            {
                var mapControls = FindVisualChildren<MapControl>(App.MainPage).ToArray();
                if (mapControls.Any())
                    return mapControls.FirstOrDefault(m => m.Name == "MainMap");
            }

            return null;
        }

        public static bool MainHubNavivateToSection(string sectionName)
        {
            var hub = GetMainHub();
            if (hub != null && !string.IsNullOrEmpty(sectionName))
            {
                var section =
                    FindVisualChildren<HubSection>(hub)
                        .FirstOrDefault(s => s.Name == sectionName);
                if (section != null)
                {
                    hub.ScrollToSection(section);
                    return true;
                }
            }

            return false;
        }
        public static Hub GetMainHub()
        {
            if (App.MainPage != null)
                return FindVisualChildren<Hub>(App.MainPage).FirstOrDefault( h => h.Name == "MainHub");

            return null;
        }

        public static int GetNowUnixTime()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static int ConvertToUnixTime(string dateTimeString)
        {
            if (!string.IsNullOrEmpty(dateTimeString))
            {
                DateTime datetime;
                if (DateTime.TryParse(dateTimeString, out datetime))
                {
                    DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    return (Int32) (datetime - sTime).TotalSeconds;
                }
            }

            return 0;
        }
    }
}

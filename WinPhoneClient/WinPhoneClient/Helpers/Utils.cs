using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
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

        public static Hub GetMainHub()
        {
            if (App.MainPage != null)
                return FindVisualChildren<Hub>(App.MainPage).FirstOrDefault( h => h.Name == "MainHub");

            return null;
        }
    }
}

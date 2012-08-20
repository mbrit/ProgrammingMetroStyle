using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client.UI
{
    internal static class FrameworkElementExtender
    {
        internal static IViewModel GetViewModel(this Window window)
        {
            if (window.Content is FrameworkElement)
                return ((FrameworkElement)window.Content).GetViewModel();
            else
                return null;
        }

        internal static IViewModel GetViewModel(this FrameworkElement element)
        {
            // walk up...
            var page = element.GetRelatedPage();
            if (page != null)
                return page.DataContext as IViewModel;
            else
                return null;
        }

        internal static Page GetRelatedPage(this FrameworkElement element)
        {
            // up...
            DependencyObject walk = element;
            while (walk != null)
            {
                if (walk is Page)
                    return (Page)walk;

                if (walk is FrameworkElement)
                    walk = ((FrameworkElement)walk).Parent;
                else
                    break;
            }

            // down...
            walk = element;
            while (walk != null)
            {
                if(walk is Page)
                    return (Page)walk;

                if (walk is ContentControl)
                    walk = ((ContentControl)walk).Content as FrameworkElement;
                else
                    break;
            }

            // nothing...
            return null;
        }

        internal static void OpenAppBarsOnPage(this FrameworkElement element, bool sticky)
        {
            // get...
            var page = element.GetRelatedPage();
            if (page == null)
                return;

            if (page.TopAppBar != null)
            {
                page.TopAppBar.IsSticky = sticky;
                page.TopAppBar.IsOpen = true;
            }
            if (page.BottomAppBar != null)
            {
                page.BottomAppBar.IsSticky = sticky;
                page.BottomAppBar.IsOpen = true;
            }
        }

        internal static void HideAppBarsOnPage(this FrameworkElement element)
        {
            var page = element.GetRelatedPage();
            if (page == null)
                return;

            if (page.TopAppBar != null)
            {
                page.TopAppBar.IsOpen = false;
                page.TopAppBar.IsSticky = false;
            }
            if (page.BottomAppBar != null)
            {
                page.BottomAppBar.IsOpen = false;
                page.BottomAppBar.IsSticky = false;
            }
        }
    }
}

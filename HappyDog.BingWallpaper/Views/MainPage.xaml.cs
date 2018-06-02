using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ImageListPage));
            var firstItem = NavView.MenuItems[0] as NavigationViewItem;
            firstItem.IsSelected = true;

            NavView.SelectionChanged += NavView_SelectionChanged;
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationViewItem item = null;
                switch (e.SourcePageType)
                {
                    case Type t when e.SourcePageType == typeof(ImageListPage):
                        item = NavView.MenuItems[0] as NavigationViewItem;
                        break;
                    case Type t when e.SourcePageType == typeof(DownloadListPage):
                        item = NavView.MenuItems[1] as NavigationViewItem;
                        break;

                }
                if (item != null)
                {
                    item.IsSelected = true;
                }
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                //ContentFrame.Navigate(typeof(AboutPage));
            }
            else
            {
                var item = args.SelectedItem as NavigationViewItem;
                switch (item.Tag)
                {
                    case "home":
                        ContentFrame.Navigate(typeof(ImageListPage));
                        break;
                    case "download":
                        ContentFrame.Navigate(typeof(DownloadListPage));
                        break;
                }
            }
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }
    }
}

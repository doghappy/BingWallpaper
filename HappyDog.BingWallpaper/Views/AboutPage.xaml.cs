using System;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            var version = Package.Current.Id.Version;
            Version = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public string Version { get; }
    }
}

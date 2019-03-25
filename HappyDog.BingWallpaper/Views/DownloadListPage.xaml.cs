using HappyDog.BingWallpaper.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.System;
using Windows.System.UserProfile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class DownloadListPage : Page, INotifyPropertyChanged
    {
        public DownloadListPage()
        {
            InitializeComponent();
            Files = new ObservableCollection<string>();
            bingService = new BingService();
        }

        readonly BingService bingService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Files { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            foreach (var file in files.OrderByDescending(f => f.DateCreated))
            {
                Files.Add(file.Path);
            }
        }

        private string selected;
        public string Selected
        {
            get => selected;
            set
            {
                if (selected != value)
                {
                    selected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
                }
            }
        }


        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Selected == null)
            {
                var srcLoader = ResourceLoader.GetForCurrentView();
                var dialog = new ContentDialog
                {
                    Title = srcLoader.GetString("TipDialogTitle"),
                    Content = srcLoader.GetString("DeleteImageDialogContent"),
                    PrimaryButtonText = srcLoader.GetString("DialogOK")
                };
                await dialog.ShowAsync();
            }
            else
            {
                string name = Path.GetFileName(Selected);
                var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(name);
                if (file != null)
                {
                    await file.DeleteAsync();
                }
                Files.Remove(Selected);
            }
        }

        private async void ShowFolder_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
        }

        private async void SetWallpaper_Click(object sender, RoutedEventArgs e)
        {
            if (Selected == null)
            {
                var srcLoader = ResourceLoader.GetForCurrentView();
                var dialog = new ContentDialog
                {
                    Title = srcLoader.GetString("TipDialogTitle"),
                    Content = srcLoader.GetString("DeleteImageDialogContent"),
                    PrimaryButtonText = srcLoader.GetString("DialogOK")
                };
                await dialog.ShowAsync();
            }
            else
            {
                await bingService.SetWallpaperAsync(Selected);
            }
        }
    }
}

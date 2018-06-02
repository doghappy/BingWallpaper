using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class DownloadListPage : Page, INotifyPropertyChanged
    {
        public DownloadListPage()
        {
            InitializeComponent();
            Files = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Files { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            foreach (var file in files)
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
                var dialog = new ContentDialog
                {
                    Title = "提示",
                    Content = "请选中要删除的图片",
                    PrimaryButtonText = "确定"
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
    }
}

using HappyDog.BingWallpaper.Common;
using HappyDog.BingWallpaper.Models;
using HappyDog.BingWallpaper.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class ImageListPage : Page, INotifyPropertyChanged
    {
        public ImageListPage()
        {
            InitializeComponent();
            IsLoading = true;
            bingServices = new BingService();
            Images = new IncrementalCollection<ImageInfo>(LoadAsync);
        }

        readonly BingService bingServices;
        public event PropertyChangedEventHandler PropertyChanged;

        public IncrementalCollection<ImageInfo> Images { get; }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }


        int loadCount = 0;
        private async Task<IEnumerable<ImageInfo>> LoadAsync()
        {
            if (loadCount < 2)
            {
                loadCount++;
                var data = await bingServices.LoadAsync();
                IsLoading = false;

                if (!executedSetTile)
                {
                    SetTile(data);
                    executedSetTile = true;
                }
                return data;
            }
            else
            {
                Images.HasMoreItems = false;
                return new List<ImageInfo>();
            }
        }

        private void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ImageDetailPage), e.ClickedItem);
        }

        bool executedSetTile;

        /*
        private void SetTile(IList<ImageInfo> imageInfos)
        {
            var tileBinding = new TileBinding
            {
                Branding = TileBranding.NameAndLogo,
                Content = new TileBindingContentPhotos()
            };

            for (int i = 0; i < imageInfos.Count; i++)
            {
                var content = tileBinding.Content as TileBindingContentPhotos;
                content.Images.Add(new TileBasicImage { Source = imageInfos[i].Url });
                if (i == 11)
                {
                    break;
                }
            }

            var tileContent = new TileContent
            {
                Visual = new TileVisual
                {
                    TileMedium = tileBinding,
                    TileWide = tileBinding,
                    TileLarge = tileBinding
                }
            };

            var tileNoti = new TileNotification(tileContent.GetXml());
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNoti);
        }
        }
         */

        private void SetTile(IList<ImageInfo> imageInfos)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            for (int i = 0; i < imageInfos.Count; i++)
            {
                var tileContent = new TileContent
                {
                    Visual = new TileVisual
                    {
                        TileMedium = new TileBinding
                        {
                            Branding = TileBranding.NameAndLogo,
                            Content = new TileBindingContentAdaptive
                            {
                                PeekImage = new TilePeekImage
                                {
                                    Source = imageInfos[i].Url
                                },
                                Children =
                                {
                                    new AdaptiveText
                                    {
                                        Text = imageInfos[i].Copyright,
                                        HintWrap = true
                                    }
                                }
                            }
                        },
                        TileWide = new TileBinding
                        {
                            Branding = TileBranding.NameAndLogo,
                            Content = new TileBindingContentAdaptive
                            {
                                PeekImage = new TilePeekImage
                                {
                                    Source = imageInfos[i].Url
                                },
                                Children =
                                {
                                    new AdaptiveText
                                    {
                                        Text = imageInfos[i].Copyright,
                                        HintWrap = true
                                    }
                                }
                            }
                        },
                        TileLarge = new TileBinding
                        {
                            Branding = TileBranding.NameAndLogo,
                            Content = new TileBindingContentAdaptive
                            {
                                PeekImage = new TilePeekImage
                                {
                                    Source = imageInfos[i].Url
                                },
                                Children =
                                {
                                    new AdaptiveText
                                    {
                                        Text = imageInfos[i].Copyright,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintWrap = true
                                    }
                                }
                            }
                        }
                    }
                };
                var tileNoti = new TileNotification(tileContent.GetXml());
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNoti);
                if (i == 11)
                {
                    break;
                }
            }

        }
    }
}

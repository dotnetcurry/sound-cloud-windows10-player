using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NowPlaying : Page
    {
        AppShell shell = Window.Current.Content as AppShell;
        public NowPlaying()
        {
            this.InitializeComponent();
            this.Loaded += NowPlaying_Loaded;
        }

        private void NowPlaying_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.nowPlaying.Count > 0)
            {
                SoundCloudTrack currentTrack = App.nowPlaying[App.nowplayingTrackId];
                LoadTrack(currentTrack);
            }

        }

        private async void LoadTrack(SoundCloudTrack currentTrack)
        {
            try
            {
                //Stop player, set new stream uri and play track
                shell.mPlayer.Stop();
                Uri streamUri = new Uri(currentTrack.stream_url + "?client_id=" + App.SoundCloudClientId);
                shell.mPlayer.Source = streamUri;
                shell.mPlayer.Play();

                //Change album art
                string albumartImage = Convert.ToString(currentTrack.artwork_url);
                if (string.IsNullOrWhiteSpace(albumartImage))
                {
                    albumartImage = @"ms-appx:///Assets\Albumart.png";

                }
                else
                {
                    albumartImage = albumartImage.Replace("-large", "-t500x500");
                }

                albumrtImage.ImageSource = new BitmapImage(new Uri(albumartImage));

                //Change Title and User name
                txtSongTitle.Text = currentTrack.title;
                txtAlbumTitle.Text = Convert.ToString(currentTrack.user.username);

            }
            catch (Exception ex)
            {
                MessageDialog showMessgae = new MessageDialog("Something went wrong. Please try again. Error Details : " + ex.Message);
                await showMessgae.ShowAsync();
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            App.nowplayingTrackId += 1;
            if (App.nowplayingTrackId >= App.nowPlaying.Count)
            {
                App.nowplayingTrackId = 0;
            }

            SoundCloudTrack currentTrack = App.nowPlaying[App.nowplayingTrackId];
            LoadTrack(currentTrack);

        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            App.nowplayingTrackId -= 1;
            if (App.nowplayingTrackId < 0)
            {
                App.nowplayingTrackId = App.nowPlaying.Count - 1;
            }

            SoundCloudTrack currentTrack = App.nowPlaying[App.nowplayingTrackId];
            LoadTrack(currentTrack);
        }

    }
}

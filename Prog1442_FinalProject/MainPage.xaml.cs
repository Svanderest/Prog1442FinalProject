using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Prog1442_FinalProject.Utils;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Prog1442_FinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        internal Round CurrentRound;
        List<Image> CardsPlayed;

        public MainPage()
        {
            this.InitializeComponent();
            CurrentRound = new Round();            
            CardsPlayed = new List<Image>
            {
                CardPlayer0,
                CardPlayer1,
                CardPlayer2,
                CardPlayer3
            };
            UpdateImages();
        }

        

        private void Hand_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentRound.Play((Card)e.ClickedItem);
                UpdateImages();
                ComputerPlay();
            }
            catch(ArgumentException ex)
            {
                ContentDialog grv = new ContentDialog
                {
                    Title = "Illegal play",
                    Content = ex.Message,
                    PrimaryButtonText = "Ok"
                };
                grv.ShowAsync();
            }
        }

        internal void UpdateImages()
        {
            Hand.ItemsSource = CurrentRound.Hands[0].OrderBy(c => c.Suit).ThenBy(c => c.Value);
            for (int i = 0; i < 4; i++)           
                CardsPlayed[i].Source = new BitmapImage(new Uri(CurrentRound.CurrentTrick.Cards[(4 + i - CurrentRound.CurrentTrick.LeadPlayer) % 4]?.ImagePath ?? "ms-appx:///Assets/cards/blank.png"));            
        }

        internal void ComputerPlay()
        {
            Random rng = new Random();
            while(CurrentRound.ActivePlayer != 0 && CurrentRound.CurrentTrick.NextIndex != 4)
            {
                if(!CurrentRound.Hands.Any(h => h.Count > 0))
                {
                    ContentDialog scores = new ContentDialog
                    {
                        Title = "Scores",
                        Content = String.Concat(CurrentRound.Scores.Select(s => s.ToString() + " ")),
                        PrimaryButtonText = "Ok",
                        SecondaryButtonText = "Cancel"
                    };
                    scores.ShowAsync();
                    CurrentRound = new Round();
                    UpdateImages();
                }
                if (!CurrentRound.CardsTaken.Any(t => t.Count > 0) && CurrentRound.ActivePlayer == CurrentRound.CurrentTrick.LeadPlayer)
                    CurrentRound.Play(new Card(Suit.Clubs, CardValue.Two));
                else
                {
                    while(true)
                    {
                        try
                        {
                            CurrentRound.Play(rng.Next());                            
                            break;
                        }
                        catch { }
                    }
                }
            }
            UpdateImages();
            if (CurrentRound.CurrentTrick.NextIndex == 4)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 2);
                timer.Tick += (sender, e) =>
                {
                    ((DispatcherTimer)sender).Stop();
                    CurrentRound.CurrentTrick = new Trick();
                    ComputerPlay();
                };
                timer.Start();
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CortanaCommands.xml"));
            await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);
            ComputerPlay();
        }
    }
}

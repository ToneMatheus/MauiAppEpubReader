using MauiAppEpubReader.Models;
using System.Diagnostics;

namespace MauiAppEpubReader
{
    public partial class MainPage : ContentPage
    {
        //int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new EpubViewModel();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            //count++;

            //if (count == 1)
            //    CounterBtn.Text = $"Clicked {count} time";
            //else
            //    CounterBtn.Text = $"Clicked {count} times";

            //SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            // Handle navigation starting
            Debug.WriteLine("Navigation started");
        }

        private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            // Handle navigation completed
            if (e.Result == WebNavigationResult.Failure)
            {
                // Handle navigation failure
                Debug.WriteLine("Navigation failed");
                DisplayAlert("Error", "Failed to load content", "OK");
            }
            else
            {
                Debug.WriteLine("Navigation completed successfully");
            }
        }

    }

}

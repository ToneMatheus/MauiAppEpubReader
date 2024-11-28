using Google.Apis.Util.Store;
using MauiAppEpubReader.Models;
using MauiAppEpubReader.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiAppEpubReader;

public partial class TextSproutPage : ContentPage
{
    private MysqlDataStore _dataStore = new MysqlDataStore();

    public TextSproutPage()
    {
        InitializeComponent();
        LoadTextSprouts();
    }

    private async void LoadTextSprouts()
    {
        var textSprouts = await _dataStore.GetAllTextSprout();
        TextSproutListView.ItemsSource = new ObservableCollection<TextSprout>(textSprouts);
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is TextSprout textSprout)
        {
            await Navigation.PushAsync(new TextSproutDetailPage(textSprout));
        }
    }
}
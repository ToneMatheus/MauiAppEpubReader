using Google.Apis.Util.Store;
using MauiAppEpubReader.Models;
using MauiAppEpubReader.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiAppEpubReader;

public partial class TextSproutPage : ContentPage
{
    private MysqlDataStore _dataStore = new MysqlDataStore();
    private ObservableCollection<TextSprout> _textSprouts;

    public TextSproutPage()
    {
        InitializeComponent();
        LoadTextSprouts();
        MessagingCenter.Subscribe<TextSproutDetailViewModel, TextSprout>(this, "DeleteTextSprout", (sender, textSprout) =>
        {
            _textSprouts.Remove(textSprout);
        });
        MessagingCenter.Subscribe<TextSproutDetailViewModel, TextSprout>(this, "EditTextSprout", (sender, updatedTextSprout) =>
        {
            var existingTextSprout = _textSprouts.FirstOrDefault(ts => ts.Id == updatedTextSprout.Id);
            if (existingTextSprout != null)
            {
                existingTextSprout.Title = updatedTextSprout.Title;
                existingTextSprout.Text = updatedTextSprout.Text;
                // Notify the UI that the item has changed
                var index = _textSprouts.IndexOf(existingTextSprout);
                _textSprouts.RemoveAt(index);
                _textSprouts.Insert(index, existingTextSprout);
            }
        });
    }

    private async void LoadTextSprouts()
    {
        var textSprouts = await _dataStore.GetAllTextSprout();
        _textSprouts = new ObservableCollection<TextSprout>(textSprouts);
        TextSproutListView.ItemsSource = _textSprouts;
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is TextSprout textSprout)
        {
            await Navigation.PushAsync(new TextSproutDetailPage(textSprout));
        }
    }
}
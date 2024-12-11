using MauiAppEpubReader.Models;

namespace MauiAppEpubReader;

public partial class TextSproutDetailPage : ContentPage
{
	public TextSproutDetailPage(TextSprout textSprout)
	{
		InitializeComponent();
        BindingContext = new TextSproutDetailViewModel(textSprout, Navigation);
        TextLabel.Text = textSprout.Text;
    }
}
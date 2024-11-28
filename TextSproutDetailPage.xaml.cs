using MauiAppEpubReader.Models;

namespace MauiAppEpubReader;

public partial class TextSproutDetailPage : ContentPage
{
	public TextSproutDetailPage(TextSprout textSprout)
	{
		InitializeComponent();
        TextLabel.Text = textSprout.Text;
    }
}
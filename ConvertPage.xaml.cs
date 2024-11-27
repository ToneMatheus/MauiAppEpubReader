using MauiAppEpubReader.Helpers;
using MauiAppEpubReader.Models.ConvertViewModel;
using MauiAppEpubReader.Models.MainViewModel;
using MauiAppEpubReader.Services;

namespace MauiAppEpubReader
{
    public partial class ConvertPage : ContentPage
    {
        public ConvertPage()
        {
            InitializeComponent();
            BindingContext = ServiceHelper.GetService<ConvertViewModel>();
        }
    }
}
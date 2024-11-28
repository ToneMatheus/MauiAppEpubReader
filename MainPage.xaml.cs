using MauiAppEpubReader.Models;
using MauiAppEpubReader.Services;
using MauiAppEpubReader.Helpers;
using System.Diagnostics;

namespace MauiAppEpubReader
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = ServiceHelper.GetService<MainViewModel>();

        }

    }

}

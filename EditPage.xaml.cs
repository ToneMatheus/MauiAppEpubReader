using MauiAppEpubReader.Helpers;
using MauiAppEpubReader.Models;
using MauiAppEpubReader.Services;

namespace MauiAppEpubReader
{
    public partial class EditPage : ContentPage
    {
        public EditPage()
        {
            InitializeComponent();
            BindingContext = ServiceHelper.GetService<EditViewModel>();
        }
    }
}
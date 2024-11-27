using MauiAppEpubReader.Models.MainViewModel;
using MauiAppEpubReader.Services;

namespace MauiAppEpubReader
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //var htmlContentService = new HtmlContentService(); // Create an instance of HtmlContentService
            //BindingContext = new MainViewModel(htmlContentService); // Pass the instance to the ViewModel
        }
    }
}

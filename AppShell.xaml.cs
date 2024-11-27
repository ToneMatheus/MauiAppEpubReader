using MauiAppEpubReader.Models.MainViewModel;

namespace MauiAppEpubReader
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}

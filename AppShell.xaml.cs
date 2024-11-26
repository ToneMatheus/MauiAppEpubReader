using MauiAppEpubReader.Models;

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

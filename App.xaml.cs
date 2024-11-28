using MauiAppEpubReader.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace MauiAppEpubReader
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}

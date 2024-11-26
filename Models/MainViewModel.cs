using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VersOne.Epub;

namespace MauiAppEpubReader.Models
{
    public class MainViewModel
    {
        public ICommand OpenFileCommand { get; }
        public ICommand CloseCommand { get; }
        private readonly EpubViewModel epubViewModel;

        public MainViewModel()
        {
            epubViewModel = new EpubViewModel();
            OpenFileCommand = new Command(async () => await OpenFileAsync());
            CloseCommand = new Command(CloseApplication);
        }

        private async Task OpenFileAsync()
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "com.apple.ibooks.epub" } },
                    { DevicePlatform.Android, new[] { "application/epub+zip" } },
                    { DevicePlatform.WinUI, new[] { ".epub" } },
                    { DevicePlatform.Tizen, new[] { "*/*" } },
                    { DevicePlatform.macOS, new[] { "com.apple.ibooks.epub" } }
                })
            });

            if (result != null)
            {
                await epubViewModel.LoadEpubFromFileAsync(result);
            }
        }

        private void CloseApplication()
        {
            MauiProgram.CloseApplication();
        }
    }
}

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
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand OpenFileCommand { get; }
        public ICommand CloseCommand { get; }

        private HtmlWebViewSource webViewSource;
        public HtmlWebViewSource WebViewSource
        {
            get => webViewSource;
            set
            {
                webViewSource = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
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
                await LoadEpubFromFileAsync(result);
            }
        }

        private void CloseApplication()
        {
            MauiProgram.CloseApplication();
        }

        public async Task LoadEpubFromFileAsync(FileResult fileResult)
        {
            var stream = await fileResult.OpenReadAsync();
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileResult.FileName);
            using (var fileStream = File.Create(filePath))
            {
                await stream.CopyToAsync(fileStream);
            }

            EpubBook epubBook = await EpubReader.ReadBookAsync(filePath);
            Dictionary<string, string> imageFilePaths = await ExtractImagesAsync(epubBook);
            string htmlContent = ExtractHtmlContent(epubBook, imageFilePaths);

            WebViewSource = new HtmlWebViewSource
            {
                Html = htmlContent
            };
        }

        private async Task<Dictionary<string, string>> ExtractImagesAsync(EpubBook epubBook)
        {
            Dictionary<string, string> imageFilePaths = new Dictionary<string, string>();

            foreach (var imageFile in epubBook.Content.Images.Local)
            {
                string imageFilePath = Path.Combine(FileSystem.CacheDirectory, Path.GetFileName(imageFile.FilePath));
                using (var imageStream = new MemoryStream(imageFile.Content))
                using (var fileStream = File.Create(imageFilePath))
                {
                    await imageStream.CopyToAsync(fileStream);
                }
                imageFilePaths[imageFile.FilePath] = imageFilePath;
            }

            return imageFilePaths;
        }

        private string ExtractHtmlContent(EpubBook epubBook, Dictionary<string, string> imageFilePaths)
        {
            foreach (EpubLocalTextContentFile textContentFile in epubBook.ReadingOrder)
            {
                string content = textContentFile.Content;

                // Match entire HTML content
                var match = System.Text.RegularExpressions.Regex.Match(content, @"<html.*?>.*?</html>", System.Text.RegularExpressions.RegexOptions.Singleline);
                if (match.Success)
                {
                    string htmlContent = match.Value;

                    // Update image paths in HTML content
                    foreach (var imageFilePath in imageFilePaths)
                    {
                        htmlContent = htmlContent.Replace(imageFilePath.Key, imageFilePath.Value);
                    }

                    return htmlContent;
                }
            }

            return "<html><body><h1>No content found</h1></body></html>";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

using MauiAppEpubReader.Services;
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
using System.Text.RegularExpressions;

namespace MauiAppEpubReader.Models.MainViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HtmlContentService _htmlContentService;
        public ICommand OpenFileCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private EpubBook epubBook;
        private int currentPageIndex = 0;

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

        private bool isFileOpen = true;
        public bool IsFileOpen
        {
            get => isFileOpen;
            set
            {
                isFileOpen = value;
                OnPropertyChanged();
            }
        }

        private String htmlContent;
        public String HtmlContent
        {
            get => htmlContent;
            set
            {
                if (htmlContent != value)
                {
                    htmlContent = value;
                    _htmlContentService.HtmlContent = value; // Sync with the service
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel(HtmlContentService htmlContentService)
        {
            _htmlContentService = htmlContentService;
            OpenFileCommand = new Command(async () => await OpenFileAsync());
            CloseCommand = new Command(CloseApplication);
            NextPageCommand = new Command(NextPage, CanNavigateNext);
            PreviousPageCommand = new Command(PreviousPage, CanNavigatePrevious);

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
                IsFileOpen = false;
            }
        }

        private void CloseApplication()
        {
            MauiProgram.CloseApplication();
        }

        private async Task LoadEpubFromFileAsync(FileResult fileResult)
        {
            var stream = await fileResult.OpenReadAsync();
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileResult.FileName);
            using (var fileStream = File.Create(filePath))
            {
                await stream.CopyToAsync(fileStream);
            }

            epubBook = await EpubReader.ReadBookAsync(filePath);
            currentPageIndex = 0; // Start at the first page
            LoadCurrentPage();
        }

        private void LoadCurrentPage()
        {
            if (epubBook?.ReadingOrder != null && epubBook.ReadingOrder.Count > 0)
            {
                try
                {
                    var currentPage = epubBook.ReadingOrder[currentPageIndex];
                    string content = currentPage.Content;

                    // Optionally embed images as Base64
                    foreach (var imageFile in epubBook.Content.Images.Local)
                    {
                        if (imageFile.Content.Length > 10 * 1024 * 1024) // Example: limit to 10MB
                        {
                            Debug.WriteLine($"Skipping large image: {imageFile.Key}");
                            continue;
                        }

                        string base64Image = Convert.ToBase64String(imageFile.Content);
                        string dataUri = $"data:image/jpeg;base64,{base64Image}";
                        content = content.Replace(imageFile.Key, dataUri);
                    }

                    // Debug content size and structure
                    Debug.WriteLine($"Loading page {currentPageIndex + 1}/{epubBook.ReadingOrder.Count}");
                    Debug.WriteLine($"Content Length: {content.Length}");

                    if (!content.Contains("<html>"))
                    {
                        content = $"<html><body>{content}</body></html>";
                    }

                    HtmlContent = ShowTextInHtml(content);


                    WebViewSource = new HtmlWebViewSource
                    {
                        Html = content
                    };

                    // Update navigation commands
                    (NextPageCommand as Command)?.ChangeCanExecute();
                    (PreviousPageCommand as Command)?.ChangeCanExecute();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading page {currentPageIndex + 1}: {ex.Message}");
                    WebViewSource = new HtmlWebViewSource
                    {
                        Html = "<html><body><h1>Error loading page</h1></body></html>"
                    };
                }
            }
        }

    

        // Add this method to the MainViewModel class
        private string StripHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        // Modify the ShowTextInHtml method to use StripHtmlTags
        private String ShowTextInHtml(string content)
        {
            HtmlContent = StripHtmlTags(content);
            return HtmlContent;
        }


        private void NextPage()
        {
            if (currentPageIndex < epubBook.ReadingOrder.Count - 1)
            {
                currentPageIndex++;
                LoadCurrentPage();
            }
            else
            {
                Debug.WriteLine("Reached the last page.");
            }
        }

        private void PreviousPage()
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                LoadCurrentPage();
            }
            else
            {
                Debug.WriteLine("Reached the first page.");
            }
        }

        private bool CanNavigateNext()
        {
            return epubBook != null && currentPageIndex < epubBook.ReadingOrder.Count - 1;
        }

        private bool CanNavigatePrevious()
        {
            return epubBook != null && currentPageIndex > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

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
        private string htmlContent;
        public string HtmlContent
        {
            get => htmlContent;
            set
            {
                htmlContent = value;
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
                var stream = await result.OpenReadAsync();
                string filePath = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                using (var fileStream = File.Create(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                EpubBook epubBook = await EpubReader.ReadBookAsync(filePath);
                Dictionary<string, string> imageFilePaths = new Dictionary<string, string>();

                // Extract images
                foreach (var imageFile in epubBook.Content.Images.Local)
                {
                    string imageFilePath = Path.Combine(FileSystem.CacheDirectory, Path.GetFileName(imageFile.FilePath));
                    using (var imageStream = new MemoryStream(imageFile.Content))
                    using (var fileStream = File.Create(imageFilePath))
                    {
                        await imageStream.CopyToAsync(fileStream);
                    }
                    imageFilePaths[imageFile.FilePath] = imageFilePath;
                    Debug.WriteLine($"Image saved: {imageFilePath}");
                }

                foreach (EpubLocalTextContentFile textContentFile in epubBook.ReadingOrder)
                {
                    string content = textContentFile.Content;
                    var match = System.Text.RegularExpressions.Regex.Match(content, @"<\?xml.*?>(.*?)</html>", System.Text.RegularExpressions.RegexOptions.Singleline);
                    if (match.Success)
                    {
                        string htmlContent = match.Groups[0].Value;

                        // Update image paths in HTML content
                        foreach (var imageFilePath in imageFilePaths)
                        {
                            htmlContent = htmlContent.Replace(imageFilePath.Key, imageFilePath.Value);
                        }

                        // Preprocess HTML content
                        htmlContent = PreprocessHtmlContent(htmlContent);

                        Debug.WriteLine(htmlContent);
                        HtmlContent = htmlContent; // Update the HtmlContent property
                        break; // Stop after the first match
                    }
                }
            }
        }

        private string PreprocessHtmlContent(string htmlContent)
        {
            // Example preprocessing steps
            // Ensure proper encoding
            htmlContent = System.Net.WebUtility.HtmlEncode(htmlContent);

            // Handle special characters
            htmlContent = htmlContent.Replace("&amp;", "&")
                                     .Replace("&lt;", "<")
                                     .Replace("&gt;", ">")
                                     .Replace("&quot;", "\"")
                                     .Replace("&#39;", "'");

            // Verify HTML structure (this is a simple example, you might need a more robust solution)
            if (!htmlContent.StartsWith("<html>"))
            {
                htmlContent = "<html>" + htmlContent;
            }
            if (!htmlContent.EndsWith("</html>"))
            {
                htmlContent += "</html>";
            }

            return htmlContent;
        }

        private void CloseApplication()
        {
            MauiProgram.CloseApplication();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

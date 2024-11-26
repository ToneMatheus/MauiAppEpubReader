using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VersOne.Epub;

namespace MauiAppEpubReader.Models
{
    public class EpubViewModel : INotifyPropertyChanged
    {
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
            HtmlContent = ExtractHtmlContent(epubBook, imageFilePaths);
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
                Debug.WriteLine($"Image saved: {imageFilePath}");
            }

            return imageFilePaths;
        }

        private string ExtractHtmlContent(EpubBook epubBook, Dictionary<string, string> imageFilePaths)
        {
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

                    HtmlContent = htmlContent; // Update the HtmlContent property
                    Debug.WriteLine(htmlContent);
                    return htmlContent; // Return the first match
                }
            }

            return string.Empty;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
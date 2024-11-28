using MauiAppEpubReader.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MauiAppEpubReader.Services
{
    internal class MysqlDataStore
    {
        private static List<TextSprout> _cachedTextSproutPages;
        private static DateTime _lastFetchTime;
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(10);

        public static List<TextSprout> TextSproutList = new List<TextSprout>();

        public async Task<List<TextSprout>> GetAllTextSprout()
        {
            if (_cachedTextSproutPages != null && (DateTime.Now - _lastFetchTime) < CacheExpiration)
            {
                return _cachedTextSproutPages;
            }

            HttpClient client = new HttpClient();
            try
            {
                string response = await client.GetStringAsync("http://10.0.2.2:5263/api/textsprout");

                Debug.WriteLine(response);

                _cachedTextSproutPages = JsonConvert.DeserializeObject<List<TextSprout>>(response);
                _lastFetchTime = DateTime.Now;

                return _cachedTextSproutPages;
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.WriteLine($"Request error: {httpRequestException.Message}");
                if (httpRequestException.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {httpRequestException.InnerException.Message}");
                }
                return new List<TextSprout>();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unexpected error: {e.Message}");
                if (e.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
                return new List<TextSprout>();
            }
        }
    }
}

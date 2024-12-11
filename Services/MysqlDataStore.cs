using MauiAppEpubReader.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace MauiAppEpubReader.Services
{
    internal class MysqlDataStore
    {
        public static List<TextSprout> TextSproutList = new List<TextSprout>();

        // GET ALL TEXTSPROUT
        public async Task<List<TextSprout>> GetAllTextSprout()
        {
            HttpClient client = new HttpClient();
            try
            {
#if ANDROID
                string response = await client.GetStringAsync("http://10.0.2.2:8000/api/textsprout");
#elif WINDOWS10_0_19041_0_OR_GREATER
                string response = await client.GetStringAsync("http://localhost:8000/api/textsprout");
#endif
                Debug.WriteLine(response);

                return JsonConvert.DeserializeObject<List<TextSprout>>(response);
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

        // ADD TEXTSPROUT
        public async Task AddTextSprout(TextSprout textSprout)
        {
            HttpClient client = new HttpClient();
            try
            {
                var json = JsonConvert.SerializeObject(textSprout);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

#if ANDROID
                var response = await client.PostAsync("http://10.0.2.2:8000/api/textsprout", content);
#elif WINDOWS10_0_19041_0_OR_GREATER
                var response = await client.PostAsync("http://localhost:8000/api/textsprout", content);
#endif
                response.EnsureSuccessStatusCode();

            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.WriteLine($"Request error: {httpRequestException.Message}");
                if (httpRequestException.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {httpRequestException.InnerException.Message}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unexpected error: {e.Message}");
                if (e.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
            }
        }

        // DELETE TEXTSPROUT
        public async Task DeleteTextSprout(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
#if ANDROID
                var response = await client.DeleteAsync($"http://10.0.2.2:8000/api/textsprout/{id}");
#elif WINDOWS10_0_19041_0_OR_GREATER
                var response = await client.DeleteAsync($"http://localhost:8000/api/textsprout/{id}");
#endif
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.WriteLine($"Request error: {httpRequestException.Message}");
                if (httpRequestException.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {httpRequestException.InnerException.Message}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unexpected error: {e.Message}");
                if (e.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
            }
        }


        // EDIT TEXTSPROUT
        public async Task EditTextSprout(int id, TextSprout textSprout)
        {
            HttpClient client = new HttpClient();
            try
            {
                var json = JsonConvert.SerializeObject(textSprout);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

#if ANDROID
                var response = await client.PutAsync($"http://10.0.2.2:8000/api/textsprout/{id}", content);
#elif WINDOWS10_0_19041_0_OR_GREATER
                var response = await client.PutAsync($"http://localhost:8000/api/textsprout/{id}", content);
#endif
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.WriteLine($"Request error: {httpRequestException.Message}");
                if (httpRequestException.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {httpRequestException.InnerException.Message}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unexpected error: {e.Message}");
                if (e.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
            }
        }

        public async Task<TextSprout> GetTextSproutById(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
#if ANDROID
        string response = await client.GetStringAsync($"http://10.0.2.2:8000/api/textsprout/{id}");
#elif WINDOWS10_0_19041_0_OR_GREATER
        string response = await client.GetStringAsync($"http://localhost:8000/api/textsprout/{id}");
#endif
                Debug.WriteLine(response);

                return JsonConvert.DeserializeObject<TextSprout>(response);
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.WriteLine($"Request error: {httpRequestException.Message}");
                if (httpRequestException.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {httpRequestException.InnerException.Message}");
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unexpected error: {e.Message}");
                if (e.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
                return null;
            }
        }
    }
}

using System.Net.Http.Json;
using System.Text.Json;

namespace ConsoleApp
{
    internal class WebApiClient : IDisposable
    {
        private HttpClient _httpClient;
        public JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions();

        public WebApiClient(string baseAddress)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.Brotli
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("br"));
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var @string = await response.Content.ReadAsStringAsync();
            return (await response.Content.ReadFromJsonAsync<T>(JsonSerializerOptions))!;
        }

        public Task<T> GetAsync<T>(string requestUri, int id)
        {
            return GetAsync<T>($"{requestUri}/{id}");
        }

        public Task<T> PostAsync<T>(string requestUri, T entity)
        {
            return PostAsync<T, T>(requestUri, entity);
        }

        public async Task<TOut> PostAsync<TIn, TOut>(string requestUri, TIn entity)
        {
            var response = await _httpClient.PostAsJsonAsync(requestUri, entity, JsonSerializerOptions);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<TOut>(JsonSerializerOptions))!;
        }

        public async Task PutAsync<T>(string requestUri, int id, T entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{requestUri}/{id}", entity, JsonSerializerOptions);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string requestUri, int id)
        {
            var response = await _httpClient.DeleteAsync($"{requestUri}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

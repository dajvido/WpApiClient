using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Newtonsoft.Json;

namespace WpApiClient.Services
{
    public class HttpApiClient
    {
        private readonly HttpClient _client;
        private readonly Uri _httpUri;
        private const string Json = "application/json";
        public HttpApiClient(Uri uri)
        {
            _httpUri = uri;
            _client = new HttpClient {BaseAddress = _httpUri};
            _client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue(Json));
        }
        public void SendTask(string json)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _httpUri)
            {
                Content = new StringContent(json,
                    Encoding.UTF8,
                    Json)
            };
            _client.SendAsync(request);
        }

        public async Task<List<Models.Task>> GetTasks()
        {
            var request = await _client.GetAsync(_httpUri);
            request.EnsureSuccessStatusCode();
            var response = await request.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Models.Task>>(response); ;
        }

        public void RemoveTask(int taskId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, _httpUri + "/" + taskId);
            _client.SendAsync(request);
        }
    }
}

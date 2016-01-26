using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Newtonsoft.Json;
using WpApiClient.Models;

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
        public bool SendTask(string json)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _httpUri)
            {
                Content = new StringContent(json,
                    Encoding.UTF8,
                    Json)
            };
            var response = _client.SendAsync(request);
            return response.Result.IsSuccessStatusCode;
        }

        public async Task<ApiResponse<List<Models.Task>>> GetTasks()
        {
            var result = new ApiResponse<List<Models.Task>>();
            var request = await _client.GetAsync(_httpUri);
            var response = await request.Content.ReadAsStringAsync();
            if (request.IsSuccessStatusCode)
            {
                result.Result = JsonConvert.DeserializeObject<List<Models.Task>>(response);
            }
            else
            {
                result.Error = JsonConvert.DeserializeObject<Error>(response);
            }
            return result;
        }

        public bool RemoveTask(int taskId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, _httpUri + "/" + taskId);
            var response = _client.SendAsync(request);
            return response.Result.IsSuccessStatusCode;
        }
    }
}

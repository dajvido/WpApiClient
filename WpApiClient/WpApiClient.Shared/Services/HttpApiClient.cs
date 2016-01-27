using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<ApiResponse<List<Models.Task>>> GetTasks(string ownerId = "")
        {
            var address = ownerId != "" ? new Uri(_httpUri + "?OwnerId=" + ownerId) : _httpUri;
            var result = new ApiResponse<List<Models.Task>>();
            var request = await _client.GetAsync(address);
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
        public bool UpdateTask(int taskId, string json)
        {
            var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

            var address = new Uri(_httpUri + "/" + taskId);

            return _client.PutAsync(address, content).ContinueWith(task => task.Result.EnsureSuccessStatusCode()).Result.IsSuccessStatusCode;
        }

        public bool RemoveTask(int taskId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, _httpUri + "/" + taskId);
            var response = _client.SendAsync(request);
            return response.Result.IsSuccessStatusCode;
        }
    }
}

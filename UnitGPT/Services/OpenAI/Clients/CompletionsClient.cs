using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnitGPT.Services.OpenAI.Models;
using UnitGPT.Services.Options;

namespace UnitGPT.Services.OpenAI.Clients
{
    internal class CompletionsClient
    {
        private string _url = "https://api.openai.com";
        private string _model = "gpt-3.5-turbo";

        private HttpClient _httpClient;

        internal CompletionsClient SetUp()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_url);
            _httpClient = httpClient;
            return this;
        }

        internal async Task<Response?> AskQuestion(string content)
        {
            SetRequestToken();
            var response = await MakeApiCallAsync(new(
                JsonConvert.SerializeObject(new RequestModel
                {
                    model = _model,
                    messages = new[] { new Message { role = "user", content = content } }
                }),
                Encoding.UTF8,
                "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Response>(stringData);
                
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorResponse>(stringData);
                throw new Exception($"{error?.error.code}:{error?.error.message}");
            }
        }

        private async Task<HttpResponseMessage> MakeApiCallAsync(StringContent requestBody)
        {
            return await _httpClient.PostAsync("v1/chat/completions", requestBody);
        }

        private void SetRequestToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OptionsService.Settings.APIKey);
        }

    }
}

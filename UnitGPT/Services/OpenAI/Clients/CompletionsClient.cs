using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnitGPT.Services.OpenAI.Models;

namespace UnitGPT.Services.OpenAI.Clients
{
    internal class CompletionsClient
    {
        private string _url = "https://api.openai.com";
        private string _model = "gpt-3.5-turbo";
        private string _token;

        private HttpClient _httpClient;

        public CompletionsClient()
        {
            _token = UnitGPTSettings.Instance.APIKey;
        }

        internal CompletionsClient SetUp()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_url);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _httpClient = httpClient;
            return this;
        }

        internal async Task<Response?> AskQuestion(string content)
        {
            using StringContent jsonContent = new(
                JsonConvert.SerializeObject(new RequestModel
                {
                    model = _model,
                    messages = new[] { new Message { role = "user", content = content } }
                }),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("v1/chat/completions", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Response>(stringData);
                
            }
            else
            {
                var stringData = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorResponse>(stringData);
                throw new Exception(error?.error.message);
            }
        }

    }
}

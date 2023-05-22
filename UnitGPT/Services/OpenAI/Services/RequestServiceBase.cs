using System.Linq;
using System.Threading.Tasks;
using UnitGPT.Services.OpenAI.Clients;
using UnitGPT.Services.OpenAI.Interface;
using UnitGPT.Services.OpenAI.Models;

namespace UnitGPT.Services.OpenAI.Services
{
    internal abstract class RequestServiceBase
    {
        private readonly CompletionsClient _openAiCompletionsClient;
        private readonly IParseResponseService _parseResponseService;
        private readonly Func<string,string> _promptBuilder;


        internal RequestServiceBase(IParseResponseService parseResponseService, Func<string,string> promptBuilder)
        {
            _promptBuilder = promptBuilder;
            _parseResponseService = parseResponseService;
            _openAiCompletionsClient = new CompletionsClient();
            _openAiCompletionsClient.SetUp();
        }

        public async Task<ResponseModel> MakeRequest(string promptData)
        {
            var response = await _openAiCompletionsClient.AskQuestion(_promptBuilder(promptData));
            return ParseResponse(response);
        }

        public ResponseModel ParseResponse(Response response)
        {
            return
                _parseResponseService.ParseCodeSectionFromResponseBody(response?.choices?.FirstOrDefault()?.message
                    .content);
        }
    }
}

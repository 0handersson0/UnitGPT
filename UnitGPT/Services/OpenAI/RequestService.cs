using System.Linq;
using System.Threading.Tasks;
using UnitGPT.Services.OpenAI.Clients;
using UnitGPT.Services.OpenAI.Models;
using UnitGPT.Services.OpenAI.PromptBuilders;
using UnitGPT.Services.OpenAI.Services;

namespace UnitGPT.Services.OpenAI
{
    internal class RequestService
    {
        private CompletionsClient _openAICompletionsClient;
        internal RequestService()
        {
            _openAICompletionsClient = new CompletionsClient();
            _openAICompletionsClient.SetUp();
        }

        internal async Task<UnitTestResponseModel> MakeRequest(string promptData)
        {
            var response = await _openAICompletionsClient.AskQuestion(PromptBuilderService.XUnitPromptBuilder(promptData));
            return ParseResponse(response);
        }

        internal UnitTestResponseModel ParseResponse(Response response)
        {
            return
                ParseResponseService.ParseCodeSectionFromResponseBody(response?.choices?.FirstOrDefault()?.message
                    .content);
        }

    }
}

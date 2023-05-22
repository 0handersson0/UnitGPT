using UnitGPT.Services.OpenAI.Clients;
using UnitGPT.Services.OpenAI.Interface;
using UnitGPT.Services.OpenAI.PromptBuilders;
using UnitGPT.Services.OpenAI.Services;

namespace UnitGPT.Services.OpenAI
{
    internal class CreateUnitTestRequestService : RequestServiceBase, IRequestService
    {
        private const string StartComment = "// Start of test";
        private const string EndComment = "// End of test";
        
        internal CreateUnitTestRequestService() 
            : base(new UnitTestParseResponseService(StartComment, EndComment), PromptBuilderService.XUnitPromptBuilder)
        { }
    }
}

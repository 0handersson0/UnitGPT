using UnitGPT.Services.OpenAI.Interface;
using UnitGPT.Services.OpenAI.PromptBuilders;

namespace UnitGPT.Services.OpenAI.Services
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

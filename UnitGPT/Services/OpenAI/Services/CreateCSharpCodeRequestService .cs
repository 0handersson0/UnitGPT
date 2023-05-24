using UnitGPT.Services.OpenAI.Interface;
using UnitGPT.Services.OpenAI.PromptBuilders;

namespace UnitGPT.Services.OpenAI.Services
{
    internal class CreateCSharpCodeRequestService : RequestServiceBase, IRequestService
    {
        private const string StartComment = "// Start of code";
        private const string EndComment = "// End of code";
        
        internal CreateCSharpCodeRequestService() 
            : base(new CSharpCodeParseResponseService(StartComment, EndComment), PromptBuilder.CSharpCodePromptBuilder)
        {
        }

    }
}

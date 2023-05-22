using UnitGPT.Services.OpenAI.Models;

namespace UnitGPT.Services.OpenAI.Interface
{
    internal interface IParseResponseService
    {
        public ResponseModel ParseCodeSectionFromResponseBody(string input);
    }
}

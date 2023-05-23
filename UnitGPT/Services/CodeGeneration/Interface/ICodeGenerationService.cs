using UnitGPT.Services.CodeGeneration.Models;
using UnitGPT.Services.OpenAI.Models;

namespace UnitGPT.Services.CodeGeneration.Interface
{
    internal interface ICodeGenerationService
    {
        public Task GenerateCodeAsync(CodeGenerationBaseModel model);
    }
}

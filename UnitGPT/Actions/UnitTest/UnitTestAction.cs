using UnitGPT.Actions.Base;
using UnitGPT.Services.CodeGeneration;
using UnitGPT.Services.CodeGeneration.Models;
using UnitGPT.Services.OpenAI.Models;
using UnitGPT.Services.OpenAI.Services;

namespace UnitGPT.Actions.UnitTest
{
    internal class UnitTestAction : BaseAction
    {
        public UnitTestAction() : base(new CodeToFileGenerationService(), new CreateUnitTestRequestService())
        {
            ExecutionSteps = 4;
            GeneratingUnitTestMsg = "generating unit test...";
            StartingProcessMsg = "starting process";
            GeneratingTestFileMsg = "generating test file...";
            ProcessDoneMsg = "process done...";
            NoxUnitProjectPathErrorMessage = "No xUnit project path, please add a path to created xUnit project under Tools/Options/UnitGPT.";
            NoApiKeyErrorMessage = "No api key to OpenAi. Please add a key under Tools/Options/UnitGPT.";
            NoSelectedCodeErrorMessage = "No selected code.";
        }

        public override async Task GenerateCodeAsync(ResponseModel response)
        {
            await CodeGenerationService.GenerateCodeAsync(new CodeToFileGenerationModel()
            {
                Code = response.Code,
                Name = response.Name
            });
        }
    }
}

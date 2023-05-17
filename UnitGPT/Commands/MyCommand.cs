using Microsoft.VisualStudio.Shell.Interop;
using System.Linq;
using UnitGPT.Services.CodeGeneration;
using UnitGPT.Services.OpenAI;
using UnitGPT.Services.Status;
using static System.Net.Mime.MediaTypeNames;

namespace UnitGPT.Commands
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {

        private string _errorMsg = string.Empty;
        private string _selectedCode = string.Empty;
        private CodeGenerationService _codeGenerationService;
        private RequestService _requestService;
        private VisualStatusService _visualStatusService;
        public MyCommand()
        {
            _codeGenerationService = new CodeGenerationService();
            _requestService = new RequestService();
            _visualStatusService = new VisualStatusService();
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await _visualStatusService.SetUp(4);

            CheckSettings();
            await SetSelectedText();

            if (_errorMsg != string.Empty || _selectedCode == String.Empty)
            {
                await ShowError(_errorMsg);
            }

            try
            {
                await _visualStatusService.UpdateStep("Starting process");
                await _visualStatusService.UpdateStep("generating code...");
                var unitTest = await _requestService.MakeRequest(_selectedCode);
                await _visualStatusService.UpdateStep("creating test class...");
                await _codeGenerationService.GenerateTestFileAsync(unitTest.ClassName, unitTest.TestCode);
                await _visualStatusService.UpdateStep("process done...");
            }
            catch(Exception ex)
            {
                await ShowError(ex.Message);
                await _visualStatusService.TearDown();
            }
            
        }

        private async Task SetSelectedText()
        {
            var currentDoc = await VS.Documents.GetActiveDocumentViewAsync();
            var selection = currentDoc?.TextView?.Selection.SelectedSpans;
            var selectedCode = selection?.FirstOrDefault().GetText();
            _selectedCode = selectedCode;
            if (selectedCode == string.Empty)
            {
                _errorMsg = "No selected code.";
            }
        }

        private async Task ShowError(string message)
        {
            await VS.MessageBox.ShowErrorAsync("UnitGPT", message);
        }

        private void CheckSettings()
        {
            if (UnitGPTSettings.Instance.XUnitTestProjectName.Length == 0)
            {
                _errorMsg = "No xUnit project path, please add a path to created xUnit project under Tools/Options/UnitGPT.";
            }
            if (UnitGPTSettings.Instance.APIKey.Length == 0)
            {
                _errorMsg = "No api key to OpenAi. Please add a key under Tools/Options/UnitGPT.";
            }
        }
    }
}

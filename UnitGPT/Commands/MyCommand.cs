using System.Collections.Generic;
using System.Linq;
using UnitGPT.Services.CodeGeneration;
using UnitGPT.Services.OpenAI;
using UnitGPT.Services.Status;
using UnitGPT.Services.Status.Strategies;

namespace UnitGPT.Commands
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {

        private string _errorMsg = string.Empty;
        private string _selectedCode = string.Empty;
        private CodeGenerationService _codeGenerationService;
        private RequestService _requestService;
        private StatusStrategyContext _statusStrategyContext;
        private string _generatingUnitTestMsg = "generating unit test...";
        private string _startingProcessMsg = "Starting process";
        private string _generatingTestFileMsg = "generating test file...";
        private string _processDoneMsg = "process done...";

        public MyCommand()
        {
            _codeGenerationService = new CodeGenerationService();
            _requestService = new RequestService();
            _statusStrategyContext = new StatusStrategyContext();
        }

        private async Task SetAndUpdateVisualStatusContextAsync(IStatusStrategy strategy, string msg)
        {
            await _statusStrategyContext.SetStrategy(strategy);
            await _statusStrategyContext.UpdateStep(msg);
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            const int steps = 4;

            var sbs = new StatusBarStrategy(steps);
            var twd = new ThreadedWaitDialogStrategy(steps);
            var tsc = new TaskStatusCenterStrategy(steps);
            await sbs.SetUpAsync();
            await twd.SetUpAsync();
            await tsc.SetUpAsync();

            CheckSettings();
            await SetSelectedText();

            if (_errorMsg != string.Empty || _selectedCode == String.Empty)
            {
                await ShowError(_errorMsg);
            }

            try
            {
                await SetAndUpdateVisualStatusContextAsync(sbs, _startingProcessMsg);
                await SetAndUpdateVisualStatusContextAsync(twd, _startingProcessMsg);
                await SetAndUpdateVisualStatusContextAsync(tsc, _startingProcessMsg);

                await SetAndUpdateVisualStatusContextAsync(sbs, _generatingUnitTestMsg);
                await SetAndUpdateVisualStatusContextAsync(twd, _generatingUnitTestMsg);
                await SetAndUpdateVisualStatusContextAsync(tsc, _generatingUnitTestMsg);
                var unitTest = await _requestService.MakeRequest(_selectedCode);

                await SetAndUpdateVisualStatusContextAsync(sbs, _generatingTestFileMsg);
                await SetAndUpdateVisualStatusContextAsync(twd, _generatingTestFileMsg);
                await SetAndUpdateVisualStatusContextAsync(tsc, _generatingTestFileMsg);
                await _codeGenerationService.GenerateTestFileAsync(unitTest.ClassName, unitTest.TestCode);

                await SetAndUpdateVisualStatusContextAsync(sbs, _processDoneMsg);
                await SetAndUpdateVisualStatusContextAsync(twd, _processDoneMsg);
                await SetAndUpdateVisualStatusContextAsync(tsc, _processDoneMsg);

            }
            catch (Exception ex)
            {
                await ShowError(ex.Message);
                await sbs.TearDown();
                await twd.TearDown();
                await tsc.TearDown();
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

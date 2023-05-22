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
        private const int _executionSteps = 4;

        private string _errorMsg = string.Empty;
        private string _selectedCode = string.Empty;
        private CodeGenerationService _codeGenerationService;
        private RequestService _requestService;
        private StatusStrategyContext _statusStrategyContext;
        private readonly List<IStatusStrategy> _statusStrategies;

        private string _generatingUnitTestMsg = "generating unit test...";
        private string _startingProcessMsg = "starting process";
        private string _generatingTestFileMsg = "generating test file...";
        private string _processDoneMsg = "process done...";
        private const string _noxUnitProjectPathErrorMessage = "No xUnit project path, please add a path to created xUnit project under Tools/Options/UnitGPT.";
        private const string _noApiKeyErrorMessage = "No api key to OpenAi. Please add a key under Tools/Options/UnitGPT.";
        private const string _noSelectedCodeErrorMessage = "No selected code.";

        public MyCommand()
        {
            _codeGenerationService = new ();
            _requestService = new ();
            _statusStrategyContext = new ();
            _statusStrategies = new();
        }

        private async Task SetAndUpdateVisualStatusContextAsync(string msg)
        {

            foreach (var statusStrategy in _statusStrategies)
            {
                await _statusStrategyContext.SetStrategy(statusStrategy);
                await _statusStrategyContext.UpdateStep(msg);
            }
        }

        private async Task SetUpStatusStrategiesAsync(params IStatusStrategy[] statusStrategies)
        {
            _statusStrategies.AddRange(statusStrategies);

            foreach (var statusStrategy in _statusStrategies)
            {
                await statusStrategy.SetUpAsync();
            }
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {

            await SetUpStatusStrategiesAsync(new StatusBarStrategy(_executionSteps), new TaskStatusCenterStrategy(_executionSteps));

            CheckSettings();
            await SetSelectedText();

            if (_errorMsg != string.Empty || _selectedCode == String.Empty)
            {
                await ShowError(_errorMsg);
            }

            try
            {
                await SetAndUpdateVisualStatusContextAsync(_startingProcessMsg);
                await SetAndUpdateVisualStatusContextAsync(_generatingUnitTestMsg);

                var unitTest = await _requestService.MakeRequest(_selectedCode);
                
                await SetAndUpdateVisualStatusContextAsync(_generatingTestFileMsg);

                await _codeGenerationService.GenerateTestFileAsync(unitTest.ClassName, unitTest.TestCode);

                await SetAndUpdateVisualStatusContextAsync(_processDoneMsg);

            }
            catch (Exception ex)
            {
                await ShowError(ex.Message);

                foreach (var statusStrategy in _statusStrategies)
                {
                    await statusStrategy.TearDown();
                }
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
                _errorMsg = _noSelectedCodeErrorMessage;
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
                _errorMsg = _noxUnitProjectPathErrorMessage;
            }
            if (UnitGPTSettings.Instance.APIKey.Length == 0)
            {
                _errorMsg = _noApiKeyErrorMessage;
            }
        }
    }
}

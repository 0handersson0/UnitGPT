using System.Collections.Generic;
using System.Linq;
using UnitGPT.Services.CodeGeneration.Interface;
using UnitGPT.Services.OpenAI.Interface;
using UnitGPT.Services.Status.Strategies;
using UnitGPT.Services.Status;

namespace UnitGPT.Actions.Base
{
    internal abstract class BaseAction
    {
        private string _errorMsg = string.Empty;
        private string _selectedCode = string.Empty;
        private readonly ICodeGenerationService _codeGenerationService;
        private readonly IRequestService _requestService;
        private readonly StatusStrategyContext _statusStrategyContext;
        private readonly List<IStatusStrategy> _statusStrategies;

        internal string GeneratingUnitTestMsg { get; set; }
        internal string StartingProcessMsg { get; set; }
        internal string GeneratingTestFileMsg { get; set; }
        internal string ProcessDoneMsg { get; set; }
        internal string NoxUnitProjectPathErrorMessage { get; set; }
        internal string NoApiKeyErrorMessage { get; set; }
        internal string NoSelectedCodeErrorMessage { get; set; }
        internal int ExecutionSteps { get; set; }

        protected BaseAction(ICodeGenerationService codeGenerationService, IRequestService requestService)
        {
            _codeGenerationService = codeGenerationService;
            _requestService = requestService;
            _statusStrategyContext = new();
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

        public async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {

            await SetUpStatusStrategiesAsync(new StatusBarStrategy(ExecutionSteps), new TaskStatusCenterStrategy(ExecutionSteps));

            CheckSettings();
            await SetSelectedText();

            if (_errorMsg != string.Empty || _selectedCode == String.Empty)
            {
                await ShowError(_errorMsg);
            }

            try
            {
                await SetAndUpdateVisualStatusContextAsync(StartingProcessMsg);
                await SetAndUpdateVisualStatusContextAsync(GeneratingUnitTestMsg);

                var response = await _requestService.MakeRequest(_selectedCode);

                await SetAndUpdateVisualStatusContextAsync(GeneratingTestFileMsg);

                await _codeGenerationService.GenerateCodeAsync(response.Name, response.Code);

                await SetAndUpdateVisualStatusContextAsync(ProcessDoneMsg);

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
                _errorMsg = NoSelectedCodeErrorMessage;
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
                _errorMsg = NoxUnitProjectPathErrorMessage;
            }
            if (UnitGPTSettings.Instance.APIKey.Length == 0)
            {
                _errorMsg = NoApiKeyErrorMessage;
            }
        }
    }
}

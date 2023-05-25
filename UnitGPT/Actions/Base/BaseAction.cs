using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitGPT.Services.CodeGeneration.Interface;
using UnitGPT.Services.OpenAI.Interface;
using UnitGPT.Services.OpenAI.Models;
using UnitGPT.Services.Status.Strategies;
using UnitGPT.Services.Status;

namespace UnitGPT.Actions.Base
{
    internal abstract class BaseAction
    {
        public ActionTypes ActionType;
        public string ErrorMsg = string.Empty;
        public string SelectedCode = string.Empty;
        public readonly ICodeGenerationService CodeGenerationService;

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
            CodeGenerationService = codeGenerationService;
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
            try
            {
                ClearErrorMsg();

                await SetUpStatusStrategiesAsync(new StatusBarStrategy(ExecutionSteps), new TaskStatusCenterStrategy(ExecutionSteps));
                
                await SetSelectedTextAsync();

                CheckSettings();
                
                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    await ShowErrorAsync(ErrorMsg);
                    return;
                }

                await SetAndUpdateVisualStatusContextAsync(StartingProcessMsg);
                await SetAndUpdateVisualStatusContextAsync(GeneratingUnitTestMsg);

                var response = await MakeRequestAsync();
                
                await SetAndUpdateVisualStatusContextAsync(GeneratingTestFileMsg);

                await GenerateCodeAsync(response);

                await SetAndUpdateVisualStatusContextAsync(ProcessDoneMsg);

            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message);

                foreach (var statusStrategy in _statusStrategies)
                {
                    await statusStrategy.TearDown();
                }
            }
        }

        public abstract Task GenerateCodeAsync(ResponseModel response);

        public virtual async Task<ResponseModel> MakeRequestAsync()
        {
            var response = await _requestService.MakeRequest(SelectedCode);

            // Try again, sometimes open ai api will return false negative
            if (response.Code == string.Empty)
            {
                return await _requestService.MakeRequest(SelectedCode);
            }

            return response;
        }

        public virtual async Task SetSelectedTextAsync()
        {
            var currentDoc = await VS.Documents.GetActiveDocumentViewAsync();
            var selection = currentDoc?.TextView?.Selection.SelectedSpans;
            var selectedCode = selection?.FirstOrDefault().GetText();
            SelectedCode = selectedCode;
            if (string.IsNullOrEmpty(selectedCode))
            {
                ErrorMsg = NoSelectedCodeErrorMessage;
            }
        }

        private void ClearErrorMsg()
        {
            ErrorMsg = string.Empty;
        }

        private async Task ShowErrorAsync(string message)
        {
            await VS.MessageBox.ShowErrorAsync("UnitGPT", message);
        }

        private void CheckSettings()
        {
            if (UnitGPTSettings.Instance.TestProjectName?.Length == 0 && ActionType == ActionTypes.Test)
            {
                ErrorMsg = NoxUnitProjectPathErrorMessage;
            }
            if (UnitGPTSettings.Instance.APIKey?.Length == 0)
            {
                ErrorMsg = NoApiKeyErrorMessage;
            }
        }
    }
}

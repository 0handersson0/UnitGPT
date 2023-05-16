using System.Linq;

namespace UnitGPT
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {

        private string _errorMsg = string.Empty;
        private string _selectedCode = string.Empty;

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            Check();
            SetSelectedText();

            if (_errorMsg != string.Empty || _selectedCode == String.Empty)
            {
                await ShowError(_errorMsg);
            }

            // Call apiService

            // Call codegenerationservice
        }

        private void SetSelectedText()
        {
            var currentDoc = VS.Documents.GetActiveDocumentViewAsync()?.Result;
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
            await VS.MessageBox.ShowWarningAsync("UnitGPT", message);
        }

        private void Check()
        {
            if (UnitGPTSettings.Instance.XUnitTestProjectPath.Length == 0)
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

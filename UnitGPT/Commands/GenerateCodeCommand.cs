using UnitGPT.Actions.Base;
using UnitGPT.Actions.Code;

namespace UnitGPT.Commands
{
    [Command(PackageIds.GenerateCodeCommand)]
    internal sealed class GenerateCodeCommand : BaseCommand<GenerateCodeCommand>
    {
        private readonly BaseAction _baseAction;
        public GenerateCodeCommand()
        {
            _baseAction = new CodeAction();
        }
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await _baseAction.ExecuteAsync(e);
        }
    }
}

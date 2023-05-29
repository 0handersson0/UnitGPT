using UnitGPT.Actions.Base;
using UnitGPT.Actions.UnitTest;

namespace UnitGPT.Commands;

    [Command(PackageIds.GenerateTestCommand)]
    internal sealed class GenerateTestCommand : BaseCommand<GenerateTestCommand>
    {
        private readonly BaseAction _baseAction;
        public GenerateTestCommand()
        {
            _baseAction = new UnitTestAction();
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await _baseAction.ExecuteAsync(e);
        }
    }


using UnitGPT.Actions.Base;
using UnitGPT.Actions.UnitTest;

namespace UnitGPT.Commands
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        private readonly BaseAction _baseAction;
        public MyCommand()
        {
            _baseAction = new UnitTestAction();
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await _baseAction.ExecuteAsync(e);
        }
    }
}

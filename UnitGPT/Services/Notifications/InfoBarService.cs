#nullable enable
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;

namespace UnitGPT.Services.Notifications;
internal class InfoBarService
{
    private InfoBar? _infoBar;

    private Action<object, InfoBarActionItemEventArgs>? _action;

    internal async Task CreateInfoBarAsync(InfoBarModel model)
    {
        _infoBar = await VS.InfoBar.CreateAsync(ToolWindowGuids80.SolutionExplorer, model);
        
        if (_infoBar != null)
        {
            _infoBar.ActionItemClicked += InfoBar_ActionItemClicked;
        }
        
    }

    internal void SetAction(Action<object, InfoBarActionItemEventArgs> action)
    {
        _action = action;
    }

    private void InfoBar_ActionItemClicked(object sender, InfoBarActionItemEventArgs e)
    {
        if (_action == null)
        {
            return;
        }

        ThreadHelper.ThrowIfNotOnUIThread();

        _action(sender, e);

    }

    internal async Task<bool> ShowInfoBarAsync()
    {
        if (_infoBar == null)
        {
            return false;

        }

        return await _infoBar.TryShowInfoBarUIAsync();
    }
}


global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging;
using UnitGPT.Services.Notifications;

namespace UnitGPT
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideOptionPage(typeof(OptionsProvider.UnitGPTSettingsOptions), "UnitGPT", "UnitGPTSettings", 0, 0, true, SupportsProfiles = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.UnitGPTString)]
    public sealed class UnitGPTPackage : ToolkitPackage
    {
        private InfoBarService _infoBarService;

        public UnitGPTPackage()
        {
            _infoBarService = new InfoBarService();
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            if (string.IsNullOrEmpty(UnitGPTSettings.Instance.APIKey))
            {
                await SetUpInfoBarAsync();
                await ShowMissingKeyWarningAsync();
            }
            
            await this.RegisterCommandsAsync();
        }

        private async Task<bool> ShowMissingKeyWarningAsync()
        {
            var infoBarStatus = await _infoBarService.ShowInfoBarAsync();
            return infoBarStatus;
        }

        private async Task SetUpInfoBarAsync()
        {
            var model = new InfoBarModel(
                    new[] {
                        new InfoBarTextSpan("Please provide an openAI api key to use the UnitGPT plugin."),
                        new InfoBarHyperlink("Get/create key here")
                    },
                    KnownMonikers.Key);
                await _infoBarService.CreateInfoBarAsync(model);
                _infoBarService.SetAction(((o, args) =>
                {
                    System.Diagnostics.Process.Start("https://platform.openai.com/account/api-keys");
                }));
        }
    }
}
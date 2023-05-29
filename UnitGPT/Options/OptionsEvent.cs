using UnitGPT.Services.Notifications;
using UnitGPT.Services.Options;

namespace UnitGPT.Options
{
    internal static class OptionsEvent
    {
        private static readonly InfoBarService BarService = new ();

        internal static void SettingsSaved(BaseOptionModel<UnitGPTSettings> options)
        {
            if (!string.IsNullOrEmpty(OptionsService.Settings.APIKey))
            {
                BarService.CloseInfoBar();
            }
            else
            {
                SetUpInfoBarAsync();

                while (!SetUpInfoBarAsync().IsCompleted)
                {
                    if (SetUpInfoBarAsync().IsFaulted || SetUpInfoBarAsync().IsCanceled)
                    {
                        break;
                    }
                }

                BarService.ShowInfoBarAsync();


                async Task SetUpInfoBarAsync()
                {
                    await BarService.CreateInfoBarAsync(OptionsInfoBarMessage.Model);
                    BarService.SetAction(((o, args) =>
                    {
                        OptionsInfoBarMessage.ClickEvent();
                    }));
                }
            }
        }
    }

}

using Microsoft.VisualStudio.Imaging;

namespace UnitGPT.Options
{
    internal static class OptionsInfoBarMessage
    {
        public static InfoBarModel Model
        {
            get
            {
                return new InfoBarModel(
                    new[]
                    {
                        new InfoBarTextSpan("Please provide an openAI api key to use the UnitGPT plugin."),
                        new InfoBarHyperlink("Get/create key here")
                    },
                    KnownMonikers.Key);
            }
        }

        public static Action ClickEvent
        {
            get
            {
                return () =>
                {
                    System.Diagnostics.Process.Start("https://platform.openai.com/account/api-keys");
                };
            }
        }
    }
}

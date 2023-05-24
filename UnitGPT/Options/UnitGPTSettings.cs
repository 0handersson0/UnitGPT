using System.ComponentModel;
using System.Runtime.InteropServices;

namespace UnitGPT
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.UnitGPTSettingsOptions), "UnitGPT", "UnitGPTSettings", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class UnitGPTSettingsOptions : BaseOptionPage<UnitGPTSettings> { }
    }

    public class UnitGPTSettings : BaseOptionModel<UnitGPTSettings>
    {
        [Category("UnitGPT")]
        [DisplayName("xUnit project name")]
        [Description("The name of the xUnit project.")]
        [DefaultValue(true)]
        public string XUnitTestProjectName { get; set; }

        [Category("UnitGPT")]
        [DisplayName("Openai api-key")]
        [Description("The openAi api key, requeried to make api calls.")]
        [DefaultValue(true)]
        public string APIKey { get; set; }

    }
}

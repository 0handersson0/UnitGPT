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
        [DisplayName("xUnit project path")]
        [Description("The path to the xUnit project.")]
        [DefaultValue(true)]
        public string XUnitTestProjectPath { get; set; }

        [Category("UnitGPT")]
        [DisplayName("openai api key")]
        [Description("The openAi api key, requeried to make api calls to generate tests.")]
        [DefaultValue(true)]
        public string APIKey { get; set; }


    }
}

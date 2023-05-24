using System.ComponentModel;
using System.Runtime.InteropServices;
using UnitGPT.Options;

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
        [DisplayName("Test project name")]
        [Description("The name of the test project.")]
        [DefaultValue(true)]
        public string XUnitTestProjectName { get; set; }

        [Category("UnitGPT")]
        [DisplayName("Openai api-key")]
        [Description("The openAi api key, requeried to make api calls.")]
        [DefaultValue(true)]
        public string APIKey { get; set; }

        [Category("UnitGPT")]
        [DisplayName("Test framework")]
        [Description("Select framework from the list.")]
        [DefaultValue(TestFrameworkOptions.xUnit)]
        [TypeConverter(typeof(EnumConverter))]
        public TestFrameworkOptions TestFrameworkOptions { get; set; } = TestFrameworkOptions.xUnit;

    }
}

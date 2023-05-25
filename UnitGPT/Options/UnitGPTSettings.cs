using System.ComponentModel;
using System.Runtime.InteropServices;
using UnitGPT.Options;
using UnitGPT.Options.Converter;

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
        [DisplayName("OpenAI api-key")]
        [Description("The openAi api key, required to make api calls.")]
        [DefaultValue(true)]
        public string APIKey { get; set; }

        [Category("UnitGPT")]
        [DisplayName("Test project")]
        [Description("Target for the generated tests")]
        [TypeConverter(typeof(ProjectNameListConverter))]
        public string TestProjectName { get; set; }

        [Category("UnitGPT")]
        [DisplayName("Test framework")]
        [Description("The type of the generated tests")]
        [DefaultValue(TestFrameworkOptions.xUnit)]
        [TypeConverter(typeof(EnumConverter))]
        public TestFrameworkOptions TestFrameworkOptions { get; set; } = TestFrameworkOptions.xUnit;
    }

}

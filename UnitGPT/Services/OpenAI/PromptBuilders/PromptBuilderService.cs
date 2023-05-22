namespace UnitGPT.Services.OpenAI.PromptBuilders
{
    internal class PromptBuilderService
    {
        internal PromptBuilderService() { }

        internal static string XUnitPromptBuilder(string code)
        {
            return $"Generate a xUnit test class with tests for the following c# code: ${code}. Place a comment that states 'Start of test' at the start of the generated class and place a comment that states 'End of test' at the end of the generated class";
        }

        internal static string CSharpCodePromptBuilder(string description)
        {
            return $"Write c# code based on the following description: ${description}";
        }
    }
}

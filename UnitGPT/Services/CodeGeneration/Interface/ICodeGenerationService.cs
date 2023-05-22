namespace UnitGPT.Services.CodeGeneration.Interface
{
    internal interface ICodeGenerationService
    {
        public Task GenerateCodeAsync(string name, string testCode);
    }
}

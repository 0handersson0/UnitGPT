using System.IO;
using System.Threading.Tasks;
using UnitGPT.Services.CodeGeneration.Interface;

namespace UnitGPT.Services.CodeGeneration
{
    internal class CodeToFileGenerationService : ICodeGenerationService
    {
        private string _testProjectName;

        internal CodeToFileGenerationService()
        {
            _testProjectName = UnitGPTSettings.Instance.XUnitTestProjectName;
        }

        public async Task GenerateCodeAsync(string name, string testCode)
        {
            var project = await GetProjectFromNameAsync();
            var path = GetDirectoryFromPath(project?.FullPath);
            File.WriteAllText($"{path}/{name}.cs", testCode);
        }

        private async Task<Project> GetProjectFromNameAsync()
        {
            return await VS.Solutions.FindProjectsAsync(_testProjectName);
        }

        private string GetDirectoryFromPath(string path)
        {
            return Path.GetDirectoryName(path);
        }

    }
}

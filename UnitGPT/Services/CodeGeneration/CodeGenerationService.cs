using System.IO;
using System.Threading.Tasks;

namespace UnitGPT.Services.CodeGeneration
{
    internal class CodeGenerationService
    {
        private string _testProjectName;

        internal CodeGenerationService()
        {
            _testProjectName = UnitGPTSettings.Instance.XUnitTestProjectName;
        }

        internal async Task GenerateTestFileAsync(string name, string testCode)
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

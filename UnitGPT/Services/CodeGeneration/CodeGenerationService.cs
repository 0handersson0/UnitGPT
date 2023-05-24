using System.IO;
using System.Threading.Tasks;
using UnitGPT.Services.CodeGeneration.Interface;
using UnitGPT.Services.CodeGeneration.Models;

namespace UnitGPT.Services.CodeGeneration;

internal class CodeToFileGenerationService : ICodeGenerationService
{
    private string _testProjectName;

    public async Task GenerateCodeAsync(CodeGenerationBaseModel model)
    {
        _testProjectName = UnitGPTSettings.Instance.XUnitTestProjectName;
        var codeToFileGenerationModel = model as CodeToFileGenerationModel;
        var project = await GetProjectFromNameAsync();
        if (project != null)
        {
            var path = GetDirectoryFromPath(project?.FullPath);
            File.WriteAllText($"{path}/{codeToFileGenerationModel?.Name}.cs", codeToFileGenerationModel?.Code);
        }
        else
        {
            throw new NullReferenceException($"Cant find project with name {_testProjectName}");
        }
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


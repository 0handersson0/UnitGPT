using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnitGPT.Services.Options;

internal class OptionsService
{
    private static readonly string OptionFileName = "unitgpt.json";
    private static List<SolutionItem> _files = new();
    internal static UnitGPTSettings Settings;

    internal static async Task LoadSettingsAsync()
    {
        try
        {
            if (!await SettingsFileExistAsync())
            {
                Settings = UnitGPTSettings.Instance;
            }
            else
            {
                var filePath = GetSolutionItem()?.FullPath;
                var fileContent = File.ReadAllText(filePath);
                Settings = JsonConvert.DeserializeObject<UnitGPTSettings>(fileContent);
            }
        }
        catch
        {
            Settings = UnitGPTSettings.Instance;
        }
    }

    private static SolutionItem GetSolutionItem()
    {
        return _files?.FirstOrDefault(x => x.Text.ToLower() == OptionFileName.ToLower());
    }

    private static async Task<bool> SettingsFileExistAsync()
    {
        _files.Clear();
        var activeFile = await VS.Solutions.GetActiveItemAsync();
        var project = GetProject(activeFile);
        if (project == null) return false;
        foreach (var projectChild in project.Children)
        {
            _files.Add(projectChild);
        }

        return GetSolutionItem() != null;
    }

    private static SolutionItem GetProject(SolutionItem solutionItem)
    {
        return solutionItem.Type == SolutionItemType.Project ? solutionItem : GetProject(solutionItem.Parent);
    }
}


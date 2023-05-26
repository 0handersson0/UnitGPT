using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitGPT.Services.CodeGeneration.Interface;
using UnitGPT.Services.CodeGeneration.Models;

namespace UnitGPT.Services.CodeGeneration;

internal class CodeToDocumentViewGenerationService : ICodeGenerationService
{

    public async Task GenerateCodeAsync(CodeGenerationBaseModel model)
    {
        var docView = await VS.Documents.GetActiveDocumentViewAsync();

        if (docView?.TextView == null || model is not CodeToDocumentGenerationModel codeToDocumentGenerationModel)
        {
            return;
        }

        var currentUsings = GetAllUsings(docView.TextBuffer.CurrentSnapshot.GetText());
        var (codeWithoutUsing, listOfUsing) = RemoveUsingFromGeneratedClass(codeToDocumentGenerationModel.Code);
        docView.TextBuffer?.Insert(codeToDocumentGenerationModel.Position, Environment.NewLine + codeWithoutUsing);

        foreach (var u in listOfUsing.Where(x => !currentUsings.Contains(x)))
        {
            docView.TextBuffer?.Insert(0, Environment.NewLine + u);
        }

        await VS.Commands.ExecuteAsync("Edit.FormatDocument");
    }

    public List<string> GetAllUsings(string input)
    {
        var result = new List<string>();
        var lines = input.Split('\n');

        foreach (string line in lines)
        {
            if (line.Contains("class") || line.Contains("namespace")) break;
            if (!line.Trim().StartsWith("using")) continue;
            var parts = line.Trim().Split(' ');
            var usingStatement = parts[0] + " " + parts[1];
            result.Add(usingStatement);
        }

        return result;
    }


    private Tuple<string, List<string>> RemoveUsingFromGeneratedClass(string generatedCode)
    {
        var usingList = new List<string>();
        var codeBuilder = new StringBuilder(generatedCode);
        var usingIndex = 0;
        var classIndex = generatedCode.IndexOf("class");
        while ((usingIndex = codeBuilder.ToString().IndexOf("using")) >= 0)
        {
            var semicolonIndex = codeBuilder.ToString().IndexOf(";", usingIndex);
            var usingStatement = codeBuilder.ToString().Substring(usingIndex, semicolonIndex - usingIndex + 1);

            if (usingIndex >= classIndex) break;

            usingList.Add(usingStatement);
            codeBuilder.Remove(usingIndex, semicolonIndex - usingIndex + 1);

        }

        var codeWithoutUsings = codeBuilder.ToString().Trim();

        return Tuple.Create(codeWithoutUsings, usingList);
    }

}


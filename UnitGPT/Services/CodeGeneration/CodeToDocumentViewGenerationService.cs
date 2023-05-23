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

        docView.TextBuffer?.Insert(codeToDocumentGenerationModel.Position, Environment.NewLine + codeToDocumentGenerationModel.Code);

        await VS.Commands.ExecuteAsync("Edit.FormatDocument");
    }
}


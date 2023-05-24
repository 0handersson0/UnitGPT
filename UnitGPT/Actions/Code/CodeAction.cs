using System.Linq;
using System.Threading.Tasks;
using UnitGPT.Actions.Base;
using UnitGPT.Helpers;
using UnitGPT.Helpers.Models;
using UnitGPT.Services.CodeGeneration;
using UnitGPT.Services.CodeGeneration.Models;
using UnitGPT.Services.OpenAI.Models;
using UnitGPT.Services.OpenAI.Services;

namespace UnitGPT.Actions.Code;

internal class CodeAction : BaseAction
{
    private int? _position = 0;

    public CodeAction() : base(new CodeToDocumentViewGenerationService(), new CreateCSharpCodeRequestService())
    {
        ActionType = ActionTypes.Code;
        ExecutionSteps = 4;
        GeneratingUnitTestMsg = "generating code...";
        StartingProcessMsg = "starting process";
        GeneratingTestFileMsg = "inserting code...";
        ProcessDoneMsg = "process done...";
        NoxUnitProjectPathErrorMessage = "No xUnit project path, please add a path to created xUnit project under Tools/Options/UnitGPT.";
        NoApiKeyErrorMessage = "No api key to OpenAi. Please add a key under Tools/Options/UnitGPT.";
        NoSelectedCodeErrorMessage = "No code description found. Make sure format is: // ### {code description} ### ";
    }

    public override async Task GenerateCodeAsync(ResponseModel response)
    {
        await CodeGenerationService.GenerateCodeAsync(new CodeToDocumentGenerationModel()
        {
            Code = response.Code,
            Position = _position ?? 0
        });
    }

    public override async Task SetSelectedTextAsync()
    {
        var doc = await GetActiveDocAsync();
        var code = GetCode(doc);
        var codeBlocks = GetCodeBlocks(code);
        SelectedCode = codeBlocks.FirstOrDefault()?.Comment;
        _position = codeBlocks.FirstOrDefault()?.LineNumber;

        if (string.IsNullOrEmpty(SelectedCode))
        {
            ErrorMsg = NoSelectedCodeErrorMessage;
        }
        
    }
    private async Task<DocumentView> GetActiveDocAsync()
    {
        return await VS.Documents.GetActiveDocumentViewAsync();
    }

    private static string GetCode(DocumentView doc)
    {
        return doc.TextBuffer?.CurrentSnapshot.GetText();
    }

    private CommentBlock[] GetCodeBlocks(string code)
    {
        return CodeSectionHelper.ExtractComments(code);
    }
}

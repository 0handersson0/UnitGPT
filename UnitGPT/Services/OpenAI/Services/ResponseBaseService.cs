using UnitGPT.Helpers;
using UnitGPT.Services.OpenAI.Interface;
using UnitGPT.Services.OpenAI.Models;

namespace UnitGPT.Services.OpenAI.Services
{
    abstract class ResponseBaseService : IParseResponseService
    {
        private readonly string[] _blackListWithReplacements = { "csharp", "```" }; 
        private string _startComment;
        private string _endComment;
        private string _defaultname;
        private Func<string, string> _extract;

        public ResponseBaseService(string startComment, string endComment, string defaultname, Func<string, string> extract)
        {
            _startComment = startComment;
            _endComment = endComment;
            _defaultname = defaultname;
            _extract = extract;
        }

        public ResponseModel ParseCodeSectionFromResponseBody(string input)
        {
            var codeSnippet = TextHelpers.ExtractCode(input, _startComment, _endComment);
            codeSnippet = TextHelpers.CleanTextFromWords(new Tuple<string, string[]>(codeSnippet, _blackListWithReplacements));
            var methodName = _extract(codeSnippet);
            return new ResponseModel()
            {
                Name = SetDefaultNameIfEmpty(methodName),
                Code = codeSnippet
            };
        }

        private string SetDefaultNameIfEmpty(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                className = $"{_defaultname}_{DateTime.Now}"
                    .Replace(' ', '_')
                    .Replace(':', '_');
            }
            return className;
        }
    }
}

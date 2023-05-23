using System.Text.RegularExpressions;

namespace UnitGPT.Helpers
{
    internal class TextHelpers
    {
        internal static string ExtractClassName(string codeSnippet)
        {
            var pattern = @"(?<=class\s+)\w+";
            var match = Regex.Match(codeSnippet, pattern);

            return match.Success ? match.Value : string.Empty;
        }

        internal static string ExtractMethodName(string code)
        {
            var pattern = @"(\w+)\s+(\w+)\s*\(";
            var match = Regex.Match(code, pattern);

            return match.Success ? match.Groups[2].Value : string.Empty;
        }

        internal static string ExtractCode(string input, string startComment, string endComment)
        {
            var startIndex = input.IndexOf(startComment);
            var endIndex = input.IndexOf(endComment);

            if (startIndex < 0 || endIndex < 0 || endIndex <= startIndex) return string.Empty;

            var codeStartIndex = startIndex + startComment.Length;
            var codeLength = endIndex - codeStartIndex;
            var extractedCode = input.Substring(codeStartIndex, codeLength).Trim();

            return extractedCode;

        }

        public static string[] ExtractComments(string code)
        {
            string pattern = @"// ###(.*?)###";
            MatchCollection matches = Regex.Matches(code, pattern, RegexOptions.Singleline);

            string[] extractedComments = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                extractedComments[i] = matches[i].Groups[1].Value.Trim();
            }

            return extractedComments;
        }

        internal static string CleanTextFromWord(string dirty, string soap)
        {
            var clean = dirty.Replace(soap, "");
            return clean;
        }
    }
}


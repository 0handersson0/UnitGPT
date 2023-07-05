using System.Collections.Generic;
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
            var pattern = @"// ###(.*?)###";
            var matches = Regex.Matches(code, pattern, RegexOptions.Singleline);

            var extractedComments = new string[matches.Count];
            for (var i = 0; i < matches.Count; i++)
            {
                extractedComments[i] = matches[i].Groups[1].Value.Trim();
            }

            return extractedComments;
        }

        internal static string CleanTextFromWords(Tuple<string, string[]> dirty)
        {
            var (word, stainAndSoaps) = dirty;
            foreach (var sas in stainAndSoaps)
            {
                var stainAndSoap = sas.Split(',');
                var stain = stainAndSoap[0];
                var soap = stainAndSoap.Length > 1 ? stainAndSoap[1] : "";
                word = word.Replace(stain, soap);
            }
            return word;
        }
    }
}


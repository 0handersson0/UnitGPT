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

        internal static string ExtractCode(string input)
        {
            const string startComment = "// Start of test";
            const string endComment = "// End of test";

            int startIndex = input.IndexOf(startComment);
            int endIndex = input.IndexOf(endComment);

            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                int codeStartIndex = startIndex + startComment.Length;
                int codeLength = endIndex - codeStartIndex;
                string extractedCode = input.Substring(codeStartIndex, codeLength).Trim();

                return extractedCode;
            }

            // Return an empty string if the comments are not found in the input
            return string.Empty;
        }

        internal static string CleanTextFromWord(string dirty, string soap)
        {
            var clean = dirty.Replace(soap, "");
            return clean;
        }
    }
}


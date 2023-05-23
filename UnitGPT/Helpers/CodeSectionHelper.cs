using System.Text.RegularExpressions;
using UnitGPT.Helpers.Models;

namespace UnitGPT.Helpers;

internal class CodeSectionHelper
{
    public static CommentBlock[] ExtractComments(string code)
    {
        try
        {
            const string pattern = @"// ###(.*?)###";
            var matches = Regex.Matches(code, pattern, RegexOptions.Singleline);

            var extractedComments = new CommentBlock[matches.Count];

            for (var i = 0; i < matches.Count; i++)
            {
                var position = matches[i].Index + pattern.Replace("(.*?)", matches[i].Groups[1].Value).Length;
                var comment = matches[i].Groups[1].Value.Trim();

                extractedComments[i] = new CommentBlock(position, comment);
            }

            return extractedComments;
        }
        catch (Exception e)
        {
            throw new Exception("Error selecting code description");
        }
    }
}

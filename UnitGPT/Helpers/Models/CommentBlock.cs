namespace UnitGPT.Helpers.Models;
public class CommentBlock
{
    public int LineNumber { get; }
    public string Comment { get; }

    public CommentBlock(int lineNumber, string comment)
    {
        LineNumber = lineNumber;
        Comment = comment;
    }
}

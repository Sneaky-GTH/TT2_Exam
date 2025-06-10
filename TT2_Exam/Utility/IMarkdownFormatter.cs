using System.Text.RegularExpressions;
using Ganss.Xss;

namespace TT2_Exam.Utility;

public interface IMarkdownFormatter
{
    string Format(string markdownText);
}

public partial class MarkdownFormatter : IMarkdownFormatter
{
    private static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer();
    
    public string Format(string markdownText)
    {
        if (string.IsNullOrEmpty(markdownText)) return string.Empty;
        
        var returnText = Sanitizer.Sanitize(markdownText);
        
        returnText = BoldRegex().Replace(returnText, "<strong>$1</strong>");
        
        returnText = ItalicRegex().Replace(returnText, "<em>$1</em>");
        
        returnText = returnText.Replace("\n", "<br>");

        return returnText;
    }
    
    [GeneratedRegex(@"\*\*(.+?)\*\*")]
    private static partial Regex BoldRegex();
    [GeneratedRegex(@"\*(.+?)\*")]
    private static partial Regex ItalicRegex();
}
using System;
using System.Text.RegularExpressions;
using Markdig;

public class MarkdownConverter
{
    public static string ConvertMarkdownToHtml(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;

        string pattern = @"^##\s*(\w+)\s*\n(.*?)(?=\n##\s*\w+|\z)";

        MatchCollection matches = Regex.Matches(markdown, pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        string fullHtml = "";

        foreach (Match match in matches)
        {
            string language = match.Groups[1].Value.Trim();
            string content = match.Groups[2].Value.Trim();

            if (!string.IsNullOrEmpty(content))
            {
                string htmlContent = Markdown.ToHtml(content).Trim();

                // Encode only '&' but keep '<' and '>'
                htmlContent = htmlContent.Replace("&", "&amp;");

                // Wrap in <div lang="xx">
                fullHtml += $"<div lang=\"{language.ToLower()}\">{htmlContent}</div>\n";
            }
        }

        if (string.IsNullOrEmpty(fullHtml))
            fullHtml = "<div lang=\"en\"><p>No release notes available.</p></div>";

        return fullHtml;
    }
}

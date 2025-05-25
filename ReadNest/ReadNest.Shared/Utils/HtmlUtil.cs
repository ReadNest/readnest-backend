using System.Text.RegularExpressions;

namespace ReadNest.Shared.Utils
{
    public static class HtmlUtil
    {
        public static string StripHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = Regex.Replace(input, "<(script|style)[^>]*>.*?</\\1>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            input = Regex.Replace(input, "<.*?>", " ", RegexOptions.Singleline);

            input = System.Net.WebUtility.HtmlDecode(input);

            return Regex.Replace(input, @"\s+", " ").Trim();
        }

        public static string NormalizeTextWithoutHtml(string text) => StringUtil.RemoveDiacritics(text?.ToLowerInvariant() ?? string.Empty);

        public static string NormalizeDescription(string text) => StringUtil.RemoveDiacritics(StripHtml(text ?? string.Empty).ToLowerInvariant());
    }
}

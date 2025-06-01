﻿using System.Globalization;
using System.Text;

namespace ReadNest.Shared.Utils
{
    public static class StringUtil
    {
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    if (c == 'Đ') stringBuilder.Append('D');
                    else if (c == 'đ') stringBuilder.Append('d');
                    else stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string NormalizeKeyword(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return string.Empty;

            var trimmed = keyword.Trim().ToLowerInvariant();
            return RemoveDiacritics(trimmed);
        }
    }
}

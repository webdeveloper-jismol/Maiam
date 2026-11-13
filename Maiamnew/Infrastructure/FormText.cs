namespace Maiamnew.Infrastructure
{
    /// <summary>
    /// Cleans text submitted through the website's forms before it is validated and emailed.
    /// Control characters are the concern: a line break in a field that becomes an email header
    /// (a name used as a Reply-To display name, for example) is a header-injection vector, and
    /// input made only of whitespace should fail the Required checks rather than arrive empty.
    /// </summary>
    internal static class FormText
    {
        /// <summary>Trims and replaces every control character, line breaks included, with a space.</summary>
        internal static string? SingleLine(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var cleaned = new string(value.Select(c => char.IsControl(c) ? ' ' : c).ToArray()).Trim();
            return cleaned.Length == 0 ? null : cleaned;
        }

        /// <summary>Keeps line breaks and tabs, drops other control characters, trims.</summary>
        internal static string MultiLine(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var unified = value.Replace("\r\n", "\n").Replace('\r', '\n');
            return new string(unified.Where(c => c is '\n' or '\t' || !char.IsControl(c)).ToArray()).Trim();
        }
    }
}

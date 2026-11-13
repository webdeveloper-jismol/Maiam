using System.Globalization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;

namespace Maiamnew.Infrastructure
{
    /// <summary>
    /// Shared spam check for the public forms. Each rendered form carries a signed
    /// timestamp; a submission that arrives implausibly fast, or with an absent or
    /// tampered token, is treated as automated. Used alongside the honeypot field,
    /// the anti-forgery token and the per-IP rate limit.
    /// </summary>
    public sealed class FormGuard(IDataProtectionProvider protection)
    {
        private static readonly TimeSpan MinimumFillTime = TimeSpan.FromSeconds(2);

        private readonly IDataProtector _protector = protection.CreateProtector("Maiamnew.Forms.FormToken");

        /// <summary>A fresh token to render in a hidden field.</summary>
        public string NewToken() =>
            _protector.Protect(DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture));

        /// <summary>True when the token is valid and enough time passed for a person to fill the form.</summary>
        public bool FilledByPerson(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var rendered = new DateTime(long.Parse(_protector.Unprotect(token), CultureInfo.InvariantCulture), DateTimeKind.Utc);
                return DateTime.UtcNow - rendered >= MinimumFillTime;
            }
            catch (Exception ex) when (ex is CryptographicException or FormatException or ArgumentOutOfRangeException or OverflowException)
            {
                return false;
            }
        }
    }
}

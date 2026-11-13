using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;

namespace Maiamnew.Services
{
    /// <summary>
    /// Builds the HTML and plain-text bodies for the website's enquiry emails (Register Now and
    /// Contact Us).
    /// <para>
    /// Email-client-safe HTML: nested tables for layout, inline styles only, bgcolor attributes
    /// alongside background-color, no div/flex/grid/positioning for layout, and a fixed 600px
    /// wrapper for Outlook desktop (which ignores max-width). Every submitted value is
    /// HTML-encoded, so nothing a visitor types can become markup or script in the email.
    /// </para>
    /// </summary>
    internal static class EnquiryEmailTemplate
    {
        private const string Font = "font-family:Arial,Helvetica,sans-serif;";

        // MAIAM brand colours, matching wwwroot/css/common.css
        private const string Navy = "#14283F";        // --primary-navy
        private const string NavyMuted = "#A9BBD0";   // readable on navy
        private const string Orange = "#F28C28";      // --accent-orange
        private const string LinkBlue = "#1769AA";    // --automotive-blue
        private const string TextBody = "#1D2935";    // --text-primary
        private const string TextMuted = "#56636F";   // --text-secondary
        private const string Border = "#E3E8ED";
        private const string PageBg = "#F4F4F4";
        private const string PanelBg = "#F4F6F8";
        private const string FooterBg = "#FAF7F2";

        /// <summary>One label/value row of the detail table.</summary>
        /// <param name="Label">Row heading, e.g. "Phone".</param>
        /// <param name="Value">Raw (unencoded) value as the visitor typed it.</param>
        /// <param name="IsEmail">Render the value as a mailto link.</param>
        /// <param name="MultiLine">Keep the visitor's line breaks (used for Address).</param>
        internal readonly record struct Field(string Label, string? Value, bool IsEmail = false, bool MultiLine = false);

        /// <summary>Formats the submission time the way the email shows it, e.g. "Thursday, 24 September 2026, 11:03 AM IST".</summary>
        internal static string SubmittedOn() =>
            DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(5.5))
                .ToString("dddd, dd MMMM yyyy, hh:mm tt", CultureInfo.InvariantCulture) + " IST";

        private static string Display(string? value, string fallback = "Not provided") =>
            string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

        private static string Encode(string value) => HtmlEncoder.Default.Encode(value);

        /// <summary>Encodes each line separately so that &lt;br /&gt; is the only markup from the visitor.</summary>
        private static string EncodeLines(string value) =>
            string.Join("<br />", value.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n').Select(Encode));

        /// <summary>Plain-text alternative, for clients that do not display HTML.</summary>
        internal static string Text(string heading, string formName, IEnumerable<Field> fields, string message, string submittedOn)
        {
            var body = new StringBuilder()
                .AppendLine(heading)
                .AppendLine($"MAIAM Website - {formName}")
                .AppendLine();

            foreach (var field in fields)
            {
                body.AppendLine($"{field.Label}: {Display(field.Value)}").AppendLine();
            }

            return body
                .AppendLine("Message:")
                .AppendLine(Display(message, "-"))
                .AppendLine()
                .AppendLine("----------------------------------------")
                .AppendLine($"Submitted on {submittedOn} via the {formName} on the MAIAM website.")
                .AppendLine("Reply to this email to answer the visitor directly.")
                .ToString();
        }

        internal static string Html(string heading, string formName, IEnumerable<Field> fields, string message, string submittedOn)
        {
            var rows = new StringBuilder();
            foreach (var field in fields)
            {
                var shown = Display(field.Value);
                string valueHtml;
                if (field.IsEmail && !string.IsNullOrWhiteSpace(field.Value))
                {
                    var address = Encode(shown);
                    valueHtml = $"""<a href="mailto:{address}" style="color:{LinkBlue};text-decoration:underline;">{address}</a>""";
                }
                else
                {
                    valueHtml = field.MultiLine ? EncodeLines(shown) : Encode(shown);
                }

                rows.Append($"""
                    <tr>
                    <td width="35%" valign="top" style="width:35%;padding:12px 12px 12px 0;border-bottom:1px solid {Border};{Font}font-size:14px;line-height:20px;font-weight:bold;color:{Navy};mso-line-height-rule:exactly;">{Encode(field.Label)}</td>
                    <td valign="top" style="padding:12px 0;border-bottom:1px solid {Border};{Font}font-size:14px;line-height:20px;color:{TextBody};mso-line-height-rule:exactly;word-wrap:break-word;word-break:break-word;">{valueHtml}</td>
                    </tr>
                    """);
            }

            var preheader = Encode(heading);
            var messageHtml = EncodeLines(Display(message, "-"));

            return $"""
                <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
                <html xmlns="http://www.w3.org/1999/xhtml" xmlns:o="urn:schemas-microsoft-com:office:office" lang="en">
                <head>
                <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                <meta http-equiv="X-UA-Compatible" content="IE=edge" />
                <meta name="x-apple-disable-message-reformatting" />
                <meta name="color-scheme" content="light" />
                <meta name="supported-color-schemes" content="light" />
                <title>{preheader}</title>
                <!--[if gte mso 9]><xml><o:OfficeDocumentSettings><o:AllowPNG/><o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml><![endif]-->
                </head>
                <body style="margin:0;padding:0;width:100%;background-color:{PageBg};-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;">
                <div style="display:none;font-size:1px;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;color:{PageBg};">{preheader} via the {Encode(formName)}</div>
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" bgcolor="{PageBg}" style="width:100%;background-color:{PageBg};border-collapse:collapse;mso-table-lspace:0pt;mso-table-rspace:0pt;">
                <tr>
                <td align="center" valign="top" style="padding:24px 12px;">
                <!--[if mso]><table role="presentation" width="600" align="center" cellpadding="0" cellspacing="0" border="0"><tr><td><![endif]-->
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" bgcolor="#ffffff" style="width:100%;max-width:600px;background-color:#ffffff;border:1px solid #dddddd;border-collapse:collapse;mso-table-lspace:0pt;mso-table-rspace:0pt;">
                <tr>
                <td bgcolor="{Navy}" style="background-color:{Navy};border-bottom:4px solid {Orange};padding:24px 28px;{Font}">
                <p style="margin:0;{Font}font-size:20px;line-height:26px;font-weight:bold;color:#ffffff;mso-line-height-rule:exactly;">{Encode(heading)}</p>
                <p style="margin:4px 0 0 0;{Font}font-size:13px;line-height:18px;color:{NavyMuted};mso-line-height-rule:exactly;">MAIAM Website &middot; {Encode(formName)}</p>
                </td>
                </tr>
                <tr>
                <td style="padding:8px 28px 0 28px;">
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;mso-table-lspace:0pt;mso-table-rspace:0pt;">
                {rows}
                <tr>
                <td width="35%" valign="top" style="width:35%;padding:12px 12px 12px 0;border-bottom:1px solid {Border};{Font}font-size:14px;line-height:20px;font-weight:bold;color:{Navy};mso-line-height-rule:exactly;">Submitted On</td>
                <td valign="top" style="padding:12px 0;border-bottom:1px solid {Border};{Font}font-size:14px;line-height:20px;color:{TextBody};mso-line-height-rule:exactly;">{Encode(submittedOn)}</td>
                </tr>
                </table>
                </td>
                </tr>
                <tr>
                <td style="padding:24px 28px 28px 28px;">
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;mso-table-lspace:0pt;mso-table-rspace:0pt;">
                <tr>
                <td style="padding:0 0 8px 0;{Font}font-size:14px;line-height:20px;font-weight:bold;color:{Navy};mso-line-height-rule:exactly;">Message</td>
                </tr>
                <tr>
                <td bgcolor="{PanelBg}" style="background-color:{PanelBg};border-left:3px solid {Orange};padding:14px 16px;{Font}font-size:14px;line-height:22px;color:{TextBody};mso-line-height-rule:exactly;word-wrap:break-word;word-break:break-word;">{messageHtml}</td>
                </tr>
                </table>
                </td>
                </tr>
                <tr>
                <td bgcolor="{FooterBg}" style="background-color:{FooterBg};border-top:1px solid {Border};padding:16px 28px;{Font}font-size:12px;line-height:18px;color:{TextMuted};mso-line-height-rule:exactly;">
                Submitted on {Encode(submittedOn)} via the {Encode(formName)} on the MAIAM website.<br />
                Reply to this email to answer the visitor directly.
                </td>
                </tr>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
                </td>
                </tr>
                </table>
                </body>
                </html>
                """;
        }
    }
}

# Register Now form – email setup

The Register Now form (`/register-now/`) emails each enquiry to MAIAM. Until SMTP settings are
provided, the form validates input but shows an honest "could not be sent – please call or email"
message. It never reports success unless the mail server accepted the message.

## Settings

Configuration section `Email` (read by `Services/EmailSettings`):

| Key | Example | Notes |
|-----|---------|-------|
| `Email:Host` | `smtp.office365.com` | SMTP server of the sending mailbox |
| `Email:Port` | `587` | Default 587 |
| `Email:EnableSsl` | `true` | Default true (STARTTLS) |
| `Email:UserName` | `website@manappuramfoundation.org` | Mailbox used to send |
| `Email:Password` | *(secret)* | **Never commit this** |
| `Email:FromAddress` | `website@manappuramfoundation.org` | Usually the same mailbox |
| `Email:FromName` | `MAIAM Website` | Optional |
| `Email:ToAddress` | `maiam@manappuramfoundation.org` | Default; where enquiries are delivered |

### Development (Visual Studio / dotnet run)

Use user secrets so the password stays out of the project folder:

```bash
dotnet user-secrets init --project Maiamnew
dotnet user-secrets set "Email:Host" "smtp.example.com" --project Maiamnew
dotnet user-secrets set "Email:UserName" "website@example.com" --project Maiamnew
dotnet user-secrets set "Email:Password" "<password>" --project Maiamnew
dotnet user-secrets set "Email:FromAddress" "website@example.com" --project Maiamnew
```

To test without a mail server, set `Email:PickupDirectory` to a local folder: messages are written
there as `.eml` files instead of being sent.

### Production (IIS)

Set environment variables on the IIS application pool / site (double underscore = section separator):

```text
Email__Host=smtp.example.com
Email__Port=587
Email__UserName=website@example.com
Email__Password=<password>
Email__FromAddress=website@example.com
```

Alternatively add an `Email` section to `appsettings.Production.json` on the server only (not in
source control). Do not put the password in the repository `appsettings.json`.

## Spam and abuse protection

* Anti-forgery token on the form.
* Hidden honeypot field and a signed minimum fill time (2 seconds).
* Rate limit: 10 submissions per 10 minutes per IP address (HTTP 429 with a friendly page).
* Input length limits and validation on both client and server; the email is plain text.

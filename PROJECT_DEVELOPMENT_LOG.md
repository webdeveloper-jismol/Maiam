# Project Development Log

## 2026-08-12

### Task: Branded 404 / Error Page

**Objective**
Fix a finding from a full-site regression pass: navigating to a non-existent URL (e.g. `/NonExistentPage123`, or `/Courses` with no sub-path) fell through to the browser's own generic error page instead of a branded page, because the app had no status-code handling middleware for non-exception 404s.

**Completed Work**
- Added `app.UseStatusCodePagesWithReExecute("/Error/{0}")` to `Program.cs`, re-executing the pipeline against a dedicated error route while preserving the real HTTP status code (confirmed `/NonExistentPage123` still returns `404`, not `200`).
- Added `Controllers/ErrorController.cs` with a single `Index(int statusCode)` action at `Error/{statusCode:int}`.
- Added `Views/Error/Index.cshtml` — reuses the existing `_Layout`/header/footer and the same `.page-hero`/`.section`/`.btn-maiam-primary` classes already used across the site, so it matches the design system with no new CSS. Shows "Page Not Found" for 404, a generic "Something Went Wrong" for any other status code, and a "Back to Home" button.
- Verified during a full 13-page regression sweep (triggered by an external test report) that this was the only real issue found — a separately reported "14%/28+" Placements figure was investigated and confirmed to be a false alarm: the stat counters animate from 0 to their target (100%/200+) over ~1.4s, and 14/100 and 28/200 are both exactly the same 14% progress fraction, meaning the report's tooling read the DOM mid-animation rather than finding a real data bug. No change was needed there.

**Files Created**
- `Maiam/Controllers/ErrorController.cs`
- `Maiam/Views/Error/Index.cshtml`

**Files Modified**
- `Maiam/Program.cs` — added `UseStatusCodePagesWithReExecute`

**Testing Performed**
- `dotnet build` — 0 errors, 0 warnings
- `curl` status-code check: `/NonExistentPage123` → 404 (branded page), `/Courses` (no sub-path) → 404, all 13 real routes still → 200
- Visual check at mobile width (375px) — header, page-hero, button all match the rest of the site

**Status**
Completed.

**Known Issues / Limitations**
None.

**Manual Steps**
None — takes effect automatically on next run.

**Rollback**
- Remove the `UseStatusCodePagesWithReExecute` line from `Program.cs`, delete `Controllers/ErrorController.cs` and `Views/Error/Index.cshtml`.

## 2026-08-11

### Task: Brevo Transactional Email Integration for Contact Form

**Objective**
Wire the existing Contact form to actually send enquiries by email via the Brevo Transactional Email API, without redesigning the page or touching unrelated code.

**Completed Work**
- Reused the existing `ContactViewModel` as-is (already had FullName/Phone/Email/InterestedCourse/Message with matching validation) — no duplicate model created.
- Added `Models/EmailSettings.cs` (options class: BrevoApiKey, SenderEmail, SenderName, RecipientEmail).
- Added `Services/IEmailService.cs` and `Services/BrevoEmailService.cs` — posts to `https://api.brevo.com/v3/smtp/email` via the already-registered `IHttpClientFactory`, HTML-encodes all user input before building the email body, sets `replyTo` to the visitor's own email/name (so replies go to them) while the actual sender stays the Brevo-verified address, and never throws past its own boundary — logs the real error server-side via `ILogger` and returns `false` on any failure.
- Registered `EmailSettings` binding and `IEmailService` in `Program.cs`.
- Updated `ContactController` to a Post/Redirect/Get flow: on success, sets `TempData["SuccessMessage"]` and redirects to the GET action (prevents duplicate sends on refresh); on failure, shows a generic "try again later" message via `ViewData["ErrorMessage"]` while preserving the user's entered data; invalid submissions still return the existing view with field-level validation errors untouched.
- Updated `Views/Contact/Index.cshtml` minimally — swapped the old placeholder `ViewData["SubmitSuccess"]` flag for real `TempData["SuccessMessage"]`/`ViewData["ErrorMessage"]` checks, added one new `.alert-error` block reusing the existing `.alert-success` visual pattern. No layout, header, footer, navbar, colors, fonts, or spacing changes.
- Added a small `.alert-error` rule to `wwwroot/css/pages/contact.css` (same shape as the existing `.alert-success`, just the red/primary palette).
- Ran `dotnet user-secrets init` for the `Maiam` project (no secrets stored yet — that's a manual step for the project owner, see below).
- Left `EmailSettings:BrevoApiKey` out of `appsettings.json` entirely; `SenderEmail` is blank in `appsettings.json` (only `SenderName` and the non-secret `RecipientEmail` are committed) — real values are supplied via User Secrets locally / environment variables or a secret store in production.

**Files Created**
- `Maiam/Models/EmailSettings.cs`
- `Maiam/Services/IEmailService.cs`
- `Maiam/Services/BrevoEmailService.cs`

**Files Modified**
- `Maiam/Program.cs` — registered `EmailSettings` options + `IEmailService`
- `Maiam/appsettings.json` — added non-secret `EmailSettings` keys
- `Maiam/Controllers/ContactController.cs` — PRG pattern, calls `IEmailService`
- `Maiam/Views/Contact/Index.cshtml` — success/error message wiring only
- `Maiam/wwwroot/css/pages/contact.css` — added `.alert-error`
- `Maiam/Maiam.csproj` — `UserSecretsId` added by `dotnet user-secrets init` (not a secret itself)

**NuGet Packages**
None added — used the framework's built-in `HttpClient`/`System.Net.Http.Json` and `System.Net.WebUtility`, per the "no MailKit/SendGrid unless already required" restriction.

**SQL / Database Changes**
None. `Maiam_DAL` was not touched.

**Testing Performed**
- `dotnet build` — 0 errors, 0 warnings
- Submitted the form with no Brevo credentials configured: confirmed no crash, server-side log captured the real diagnostic (`BrevoEmailService` logged the missing-config error), the visitor saw only the generic "Unable to send your enquiry..." message, and all entered field values were preserved (no retyping needed)
- Submitted the form empty: confirmed all five field-level validation messages still appear via existing unobtrusive validation, unchanged from before
- Verified the Contact page renders identically to before this change at mobile width (375px) — header, footer, colors, spacing all untouched
- Did **not** test an actual successful Brevo send end-to-end, since that requires a real verified sender + API key that only the project owner can provide (see Manual Steps)

**Status**
Completed for everything that doesn't require real Brevo credentials. Awaiting the project owner to supply a verified sender email and API key to confirm live email delivery.

**Known Issues / Limitations**
- Untested: actual email delivery to `mafoundteamit@gmail.com` (needs real credentials — see Manual Steps)
- The Brevo sender email must be a verified sender/domain in the Brevo account, or Brevo will reject the send even with a valid API key

**Recommendations**
- None beyond the manual steps below.

**Manual Steps**
Run from the `Maiam` project folder:
```powershell
cd Maiam
dotnet user-secrets set "EmailSettings:BrevoApiKey" "YOUR_BREVO_API_KEY"
dotnet user-secrets set "EmailSettings:SenderEmail" "YOUR_BREVO_VERIFIED_SENDER_EMAIL"
```
Then run the app and submit the Contact form with real data to confirm the email arrives at `mafoundteamit@gmail.com` with subject `Enquiry`.

**Rollback**
- Revert `Program.cs`, `Controllers/ContactController.cs`, `Views/Contact/Index.cshtml`, `appsettings.json`, and `wwwroot/css/pages/contact.css` to their prior versions, and delete `Models/EmailSettings.cs` and `Services/IEmailService.cs`/`BrevoEmailService.cs`.

### Task: Convert Readdy Reference Website (bfsysw.readdy.co) to ASP.NET Core MVC

**Objective**
Rebuild the complete MAIAM (Manappuram Institute of Automotive Skills) marketing website, currently live as a Readdy.ai/React demo at https://bfsysw.readdy.co/, as a native ASP.NET Core MVC application inside the existing `Maiam` project, with no React/Readdy runtime dependency.

**Completed Work**
- Inspected the existing solution: `Maiam` (ASP.NET Core MVC, .NET 8) + `Maiam_DAL` (EF Core 8 / SQL Server class library, no entities defined). Confirmed `Maiam` was an unmodified `dotnet new mvc` template and did not need a new project.
- Built a full reference inventory of the live site: header/nav (Home, About▾, Courses▾, Placements, Faculties, Facilities, Gallery, Contact), all 13 pages/routes, section-by-section content, footer, forms, and interactive elements (hero carousel, dropdowns, tabs, counters, testimonial carousel, gallery grid, mobile menu).
- Extracted the design system from computed styles: brand red `#C41E3A`, navy `#0F172A`/`#020617`, slate text scale, `Outfit` (headings) + `DM Sans` (body) fonts, pill buttons, card radius/shadow, breakpoints. Encoded as CSS variables in `site.css`.
- Self-hosted `Outfit` and `DM Sans` as local variable-font `.woff2` files (no external Google Fonts request at runtime).
- Downloaded and organized all reference images (logo, hero slides, about image, gallery photos) from the client's own live site (maiam.co.in) into `wwwroot/images/...` by section.
- Rebuilt the shared layout: `_Layout.cshtml`, `_Header.cshtml` (desktop nav + dropdowns), `_MobileMenu.cshtml` (offcanvas mobile nav with accordion submenus), `_Footer.cshtml`, `_IconSvg.cshtml` (shared inline-SVG icon partial, avoids an icon-font dependency).
- Implemented all 13 pages in menu order: Home, About MAIAM, Vision & Mission, Objectives, Testimonials, Certifications, Auto Body Repair course, Advanced Welding course, Placements, Faculties, Facilities, Gallery, Contact.
- Implemented interactive behavior in vanilla JS (`site.js` + per-page scripts): sticky/scroll header state, desktop dropdown menus, mobile offcanvas menu with accordions, hero carousel with autoplay/arrows, testimonial carousel with dots, animated stat counters (IntersectionObserver), scroll-reveal, course-detail tabs.
- Implemented the Contact form with server-side model validation (`[Required]`, `[EmailAddress]`, `[Phone]`), client-side jQuery unobtrusive validation, anti-forgery token, and a success-state banner. **No email/database integration is wired up** — submissions validate and display a success message but are not sent or persisted anywhere (by design, pending explicit approval).
- Fixed a real contrast bug found during testing: the header nav was rendering dark-navy text by default, invisible against the dark hero/page-hero background on every page until the user scrolled. Reworked `site.css` so the header defaults to white text (correct for the dark hero underneath) and switches to navy text only once `.is-scrolled` adds the white background.
- Fixed a form UX bug: after a successful Contact submission, `ModelState` retained the posted values so the tag helpers kept showing old input even though a fresh `ContactViewModel` was returned. Fixed with `ModelState.Clear()` before returning the reset view.
- Removed the now-orphaned `_Layout.cshtml.css` (default template CSS-isolation file, superseded by the new `site.css` design system).

**Pages / Routes Implemented**
- `/` (Home) — `Views/Home/Index.cshtml`
- `/About/AboutMaiam`, `/About/VisionMission`, `/About/Objectives`, `/About/Testimonials`, `/About/Certifications`
- `/Courses/AutoBodyRepair`, `/Courses/AdvancedWelding` (shared `Views/Courses/CourseDetail.cshtml`)
- `/Placements`, `/Faculties`, `/Facilities`, `/Gallery`, `/Contact`

**Files Created**
- Controllers: `AboutController.cs`, `CoursesController.cs`, `PlacementsController.cs`, `FacultiesController.cs`, `FacilitiesController.cs`, `GalleryController.cs`, `ContactController.cs`
- ViewModels: `HomeViewModel.cs`, `AboutViewModels.cs`, `CourseDetailViewModel.cs`, `PlacementsViewModel.cs`, `FacultiesViewModel.cs`, `FacilitiesViewModel.cs`, `GalleryViewModel.cs`, `ContactViewModel.cs`
- Views: `Views/Home/Index.cshtml`, `Views/About/*.cshtml` (5), `Views/Courses/CourseDetail.cshtml`, `Views/Placements/Index.cshtml`, `Views/Faculties/Index.cshtml`, `Views/Facilities/Index.cshtml`, `Views/Gallery/Index.cshtml`, `Views/Contact/Index.cshtml`
- Shared partials: `Views/Shared/_Header.cshtml`, `_MobileMenu.cshtml`, `_Footer.cshtml`, `_IconSvg.cshtml`
- CSS: `wwwroot/css/site.css` (rewritten), `wwwroot/css/pages/{home,about,courses,placements,faculties,facilities,gallery,contact}.css`
- JS: `wwwroot/js/site.js` (rewritten), `wwwroot/js/pages/{home,courses}.js`
- Fonts: `wwwroot/lib/fonts/{outfit,dm-sans}.woff2`, `fonts.css`
- Images: `wwwroot/images/{shared,home/hero,home/about,gallery}/...` (18 images downloaded from maiam.co.in)
- `.claude/launch.json` (dev-server preview config, used only for local testing in this workflow)

**Files Modified**
- `Views/Shared/_Layout.cshtml` — full rebuild (header/footer partials, fonts, meta description, Styles/Scripts sections)
- `Controllers/HomeController.cs` — Home view model wiring
- `Views/_ViewImports.cshtml` — added `Maiam.Models.ViewModels` using
- `wwwroot/css/site.css`, `wwwroot/js/site.js` — rewritten from template defaults

**Files Removed**
- `Views/Shared/_Layout.cshtml.css` — default template CSS-isolation file, orphaned after the layout rebuild (its selectors—`navbar-brand`, `nav-pills`, old blue theme—no longer apply anywhere)

**SQL / Dependencies / Configuration**
- Dependencies: Outfit + DM Sans fonts self-hosted as static `.woff2` files under `wwwroot/lib/fonts/` (no NuGet/npm package added, no external runtime request)
- Configuration: None (`appsettings.json`, `launchSettings.json`, connection strings untouched)
- Database: None. `Maiam_DAL` (`MaiamDbContext`, `Data_Layer`) was inspected but not modified — no entities exist yet and this UI rebuild did not require any

**Testing Performed**
- `dotnet build` on the full solution after every page — 0 errors, 0 warnings throughout
- All 13 routes verified returning HTTP 200 via direct request
- Visual comparison against the live reference for every page at desktop (1440px) width
- Mobile (375px) visual check on Home and About MAIAM, including the offcanvas mobile menu and hamburger toggle
- Tablet (768px) horizontal-overflow check on Home, Auto Body Repair course, and Contact — no overflow on any
- Verified interactive behavior: hero carousel autoplay + arrows, About/Courses desktop dropdowns, mobile menu accordions, course-detail tab switching (Overview/Curriculum/Career Path or Batches), animated stat counters, Contact form client-side validation (empty submit) and successful submit with field reset
- Checked browser console for errors (none) and network requests for all images (all 200 OK)

**Status**
Completed. All 13 pages from the reference menu are implemented, built, and passed visual + functional review.

**Known Issues / Limitations**
- Contact form does not send email or persist submissions to a database — this was an explicit scope decision (flagged during planning) pending your approval; currently UI/validation-only.
- Faculty photos use a generic person-icon avatar rather than real staff photos, since no faculty headshots were available on the reference or the live maiam.co.in site.
- The exact mobile off-canvas menu open/close animation on the Readdy reference could not be pixel-compared frame-by-frame (the demo's mobile menu didn't reliably open under automated clicks); the rebuilt mobile menu follows the same visual language (dark full-screen panel, accordion submenus) verified against the mobile hero and general nav structure.
- `Views/Home/Privacy.cshtml` (default template leftover) still exists and is routable at `/Home/Privacy` but is unlinked from navigation — left untouched as an unrelated, harmless artifact.

**Recommendations**
- Decide whether the Contact form should send email (needs SMTP configuration in `appsettings.json` — configuration change, requires approval) and/or persist to `Maiam_DAL` (needs a new EF Core entity + migration — database change, requires approval).
- Consider sourcing real faculty photographs to replace the icon avatars.
- Optimize/compress the larger downloaded images (e.g. `slide-6.png` at ~2MB, `gallery-8.jpg` at ~760KB) before production deployment.

**Manual Steps**
- Run the app locally (`dotnet run --project Maiam/Maiam.csproj`) and click through the live site once more against https://bfsysw.readdy.co/ to confirm final sign-off.
- If satisfied, remove the temporary `.claude/launch.json` dev-preview config if not wanted going forward (harmless to keep).

**Rollback**
- Revert the files listed under "Files Created" / "Files Modified" above, and restore `Views/Shared/_Layout.cshtml.css` from version control if the CSS-isolation file removal needs to be undone.

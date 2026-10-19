# Project Development Log

## 2026-10-14

### Task: Project Base Setup (modeled on Mps)

**Objective**
Set up the Maiamnew ASP.NET Core MVC solution with the same structure, framework and core configuration as the reference project `E:\Asp.net websites\Basic website ASP.Net MVC Core\Mps`.

**Completed Work**
- Created the `Maiamnew.slnx` solution with two projects, mirroring Mps:
  - `Maiamnew` — ASP.NET Core MVC web project (net8.0, nullable and implicit usings enabled).
  - `Maiamnew_DAL` — class library for data access (net8.0) with EF Core 8.0.0 (`EntityFrameworkCore`, `SqlServer`, `Tools`), same as `MPS_DAL`.
- `Program.cs` matches Mps: registers `MaiamnewDbContext` (SQL Server), `Data_Layer` (scoped), `HttpClient`, MVC, exception handler/HSTS, HTTPS redirection, static files, routing, authorization, default route.
- Copied the default views, layout, `site.css`, `site.js`, favicon and `wwwroot/lib` (Bootstrap, jQuery, jQuery Validation) from Mps.
- Renamed every `Mps` / `MPS_DAL` / `MPSDbContext` reference to `Maiamnew` / `Maiamnew_DAL` / `MaiamnewDbContext`.
- Gave `launchSettings.json` its own ports so Maiamnew and Mps can run at the same time.

**Affected Pages / Features**
- Home (`/`), Privacy (`/Home/Privacy`), Error (`/Home/Error`), shared layout.

**Files Created**
- `Maiamnew.slnx`
- `Maiamnew/Maiamnew.csproj`, `Program.cs`, `appsettings.json`, `appsettings.Development.json`, `Properties/launchSettings.json`
- `Maiamnew/Controllers/HomeController.cs`, `Models/ErrorViewModel.cs`
- `Maiamnew/Views/**` (`_ViewImports`, `_ViewStart`, `Home/Index`, `Home/Privacy`, `Shared/_Layout` + `.css`, `Shared/Error`, `Shared/_ValidationScriptsPartial`)
- `Maiamnew/wwwroot/**` (`css/site.css`, `js/site.js`, `favicon.ico`, `lib/*`)
- `Maiamnew_DAL/Maiamnew_DAL.csproj`, `DAL/Data_Layer.cs`, `DAL/MaiamnewDbContext.cs`
- `.claude/launch.json` (local preview config for Claude Code)

**Files Modified**
- None (new project)

**SQL / Dependencies / Configuration**
- NuGet: EF Core 8.0.0 packages in `Maiamnew_DAL` (same versions as `MPS_DAL`).
- `appsettings.json`: connection string `ConnectionStrings:Maiamnew` added with **placeholder values**. The Mps credentials were deliberately not copied.
- No SQL scripts.

**Testing Performed**
- `dotnet build Maiamnew.slnx`: succeeded, 0 warnings, 0 errors.
- Ran the `http` profile on http://localhost:5174. Home and Privacy pages render with Bootstrap styling and Maiamnew branding, and the browser console shows no errors.

**Status**
Completed (base setup). No database access has been exercised yet.

**Known Issues / Remaining Work**
- Connection string must be filled in before any DAL/database code is used.
- Site design, menu structure and content pages are not started. They need a design reference (Readdy.ai demo, screenshots, or existing site).

**Recommendations**
- Keep the real connection string out of `appsettings.json` (use User Secrets in development, environment variables or server config in production).
- Initialise a Git repository and add a standard .NET `.gitignore` before starting page work.

**Manual Steps**
- Replace the placeholders in `Maiamnew/appsettings.json` → `ConnectionStrings:Maiamnew`.
- Open `E:\Maiamnew\Maiamnew.slnx` in Visual Studio 2022 (17.13+) or later.

**Rollback**
- Delete the `E:\Maiamnew` contents. The Mps reference project was not modified.

### Task: WordPress analysis, media migration and shared core (MAIAM redesign)

**Objective**
Migrate https://maiam.co.in/ into the Maiamnew project with a completely redesigned premium UI, preserving original content, URLs and photographs.

**Completed Work**
- Crawled the live site (menu, page/post sitemaps, every page): 14 menu pages, 6 unlinked published pages, 8 WordPress placeholders. Inventory: `docs/MIGRATION_INVENTORY.md`.
- Downloaded 149 original images, 2 videos and the ABR qualification-pack PDF; reviewed every image; separated MAIAM photographs from stock/third-party graphics and outdated posters.
- Image pipeline (scratchpad Node tool using `sharp`, not a project dependency): responsive WebP renditions, favicons, Open Graph JPEGs, generated `Infrastructure/ImageCatalog.Generated.cs`. 467 MB → 23 MB. Source map: `docs/IMAGE_SOURCES.md`.
- Self-hosted Sora and Inter variable fonts (OFL).
- Shared core: `SiteInfo`, `SiteNavigation` (original menu order/labels), `PageMeta` + `_SeoMeta` (title, description, canonical, robots, Open Graph, Twitter, JSON-LD), `StructuredData` (EducationalOrganization, WebSite, BreadcrumbList, Course), `<maiam-img>` tag helper (srcset, sizes, width/height, lazy loading), `LegacyUrlMiddleware` (301s for retired URLs + trailing-slash canonicalisation), `SecurityHeadersMiddleware` (CSP, nosniff, frame, referrer, permissions), response compression, static-file caching, rate limiter policy for forms, `/sitemap.xml`, `robots.txt`, custom 404 and 500 pages.
- Shared views: `_Layout`, `_Header` (sticky header, keyboard-accessible dropdowns incl. nested Certifications, mobile drawer with focus trap), `_Footer`, `_Breadcrumb`, `_PageHero`, `_EnquiryCTA`, `_VideoEmbed` (click-to-load youtube-nocookie), `_IconSprite`; `common.css` (design tokens, components) and `common.js` (navigation, reveal, lightbox, video embeds).
- Removed template-only files: `Views/Home/Privacy.cshtml`, `Views/Shared/_Layout.cshtml.css`, `wwwroot/css/site.css`, `wwwroot/js/site.js`.

**Files Created**
- `Infrastructure/*.cs`, `Services/SiteContent.cs`, `Models/SiteContentModels.cs`, `ViewModels/PageHeroModel.cs`, `Controllers/SeoController.cs`
- `Views/Shared/_Header|_Footer|_Breadcrumb|_PageHero|_EnquiryCTA|_VideoEmbed|_IconSprite|_SeoMeta.cshtml`, `Views/Home/NotFound.cshtml`
- `wwwroot/css/common.css`, `error.css`; `wwwroot/js/common.js`, `error.js`; `wwwroot/robots.txt`
- `wwwroot/assets/images/**`, `assets/videos/*.mp4`, `assets/documents/*.pdf`, `assets/fonts/*.woff2`, `wwwroot/favicon.ico` (replaced)
- `docs/MIGRATION_INVENTORY.md`, `docs/IMAGE_SOURCES.md`, `.claude/launch.json`

**Files Modified**
- `Program.cs` (routing options, compression, rate limiter, middleware, caching), `Controllers/HomeController.cs`, `Views/_ViewImports.cshtml`, `Views/Shared/_Layout.cshtml`, `Views/Shared/Error.cshtml`, `Views/Shared/_ValidationScriptsPartial.cshtml`

**SQL / Dependencies / Configuration**
- No NuGet/npm packages added to the project. `appsettings.json`, `launchSettings.json` and `web.config` not changed.

**Status**
Completed.

### Task: Stage 1 – Homepage

**Objective**
Premium redesigned home page using original MAIAM content and photographs.

**Completed Work**
- `Views/Home/Index.cshtml`, `wwwroot/css/index.css`, `wwwroot/js/index.js`.
- Sections: hero (original tagline as H1, original intro, certification chips), 6 original highlights, About MAIAM, Courses Offered (2 menu courses), Practical Training photo grid, Why Choose MAIAM (verified benefits only), Scholarships (original 10% BPL / 5% APL backward, ABR only, with "confirm availability" note), Facilities, Faculty, Placements & industry visits, Students' Feedback slider (5 original testimonials, verbatim), Discover MAIAM (2 original videos with poster frames), Gallery preview with lightbox, Enquiry CTA, Managed by Manappuram Foundation.

**Testing Performed**
- `dotnet build`: 0 warnings, 0 errors.
- Automated DevTools audit at 1920/1440/1366/1024/768/425/375/320 px: no horizontal overflow, no broken images, no missing alt, no duplicate IDs, no heading-level skips, no console errors, no failed requests.
- Visual review of full-page screenshots at 1440, 768 and 375 px. Fixed: header CTA wrapping, testimonial card width, facility icons squashed, gallery grid gap, footer email break, CTA buttons wrapping, mobile drawer containing-block bug (navbar `backdrop-filter` caused 706 px page width on phones).
- Interaction: mobile drawer open/close, nested submenu accordion, Escape handling and focus return; desktop dropdown + nested flyout (hover and click).
- Lighthouse 13.5 (local dev server): desktop 100/100/100/100; mobile accessibility 100, best practices 100, SEO 100, performance 92 / 88 / 95 over three runs (LCP 2.6–2.7 s, CLS 0).

```text
MAIAM MENU COMPLETION REPORT
Menu Name: Home
Pages Completed: Home (/)
Files Created: Index.cshtml, index.css, index.js
Original Content: Complete
Original MAIAM Images: Verified
External Images: None used
UI/UX: Verified
Responsive: Verified (320–1920 px)
SEO: Verified (title, description, canonical, OG, EducationalOrganization + WebSite JSON-LD)
Functional Testing: Pass
Build: Pass
Regression: Pass
Known Issues: Items needing owner verification are listed in docs/MIGRATION_INVENTORY.md §5
Final Status: PASS
Next Menu: About Us
```

### Task: Stage 2 – About Us menu

**Objective**
Rebuild every About Us submenu page (and the unlinked Director's Message page) at its original URL with the new design.

**Completed Work**
- `AboutController` (Objectives, AboutMaiam, VisionAndMission, Testimonials, DirectorsMessage) and `CertificationsController` (Nsdc, AffiliationCertificate), attribute-routed to the original WordPress paths.
- Objectives: all 8 original objective statements (light grammar only), grouped into 3 themes with a sticky "On this page" index (`objectives.js` highlights the section in view).
- About MAIAM: original About text, key facts, the retired `/who-we-are/` content merged as `#who-we-are` (`about-maiam.js` highlights it for visitors redirected from the old URL), Director's message teaser.
- Vision and Mission: official statements reproduced verbatim; the three mission sentences shown as three pillars.
- Testimonials: the 5 original YouTube testimonial films (click-to-load, privacy-enhanced), the self-hosted student film and the 5 written testimonials; `testimonials.js` keeps one film playing at a time.
- Director's Message: original message from Dr. Sivadas Aniyan T.S (light grammar only).
- NSDC and ASDC/NCVET certificate pages: certificate images with lightbox and full-size view, certificate details exactly as printed, copy-number button (shared in `common.js`), ASDC background text from the original course page.
- Breadcrumb JSON-LD skips menu groupings without a page; Razor now emits UTF-8 text instead of numeric entities.

**Files Created**
- `Controllers/AboutController.cs`, `Controllers/CertificationsController.cs`
- `Views/About/Objectives|AboutMaiam|VisionAndMission|Testimonials|DirectorsMessage.cshtml`, `Views/Certifications/Nsdc|AffiliationCertificate.cshtml`
- `wwwroot/css/objectives|about-maiam|vision-and-mission|testimonials|directors-message|nsdc|certificate-of-affiliation.css`
- `wwwroot/js/objectives|about-maiam|vision-and-mission|testimonials|directors-message|nsdc|certificate-of-affiliation.js`

**Files Modified**
- `wwwroot/css/common.css` (certificate components, target sizes), `wwwroot/js/common.js` (copy buttons), `Infrastructure/StructuredData.cs`, `Program.cs` (UTF-8 encoder)
- `Views/Home/Index.cshtml` – repaired 12 characters corrupted by a PowerShell bulk edit (verified clean)

**Testing Performed**
- Build: 0 warnings, 0 errors.
- DevTools audit of all 7 pages at 1440/1024/768/375/320 px: no overflow, broken images, missing alt, duplicate IDs, heading skips, console errors or failed requests; tap targets ≥ 24 px.
- Lightbox (open, caption, single-image controls, focus, Escape, scroll lock), copy button visibility, `/who-we-are/` → `/about-maiam/#who-we-are` 301, trailing-slash 301.
- Lighthouse mobile: Objectives 97, Testimonials 97, NSDC 99 performance; 100 accessibility/best practices/SEO.
- Regression: Home re-audited at 1440/375 – pass.

```text
MAIAM MENU COMPLETION REPORT
Menu Name: About Us
Pages Completed: Objectives, About MAIAM, Vision and Mission, Testimonials, Certifications › NSDC,
                 Certifications › ASDC, NCVET Affiliation Certificate (+ unlinked Director's Message)
Files Created: 7 × .cshtml, 7 × .css, 7 × .js (listed above)
Original Content: Complete (Who We Are merged into About MAIAM)
Original MAIAM Images: Verified
External Images: None used (stock vision/mission images from the old site replaced with MAIAM photos)
UI/UX: Verified
Responsive: Verified (320–1440 px)
SEO: Verified (unique titles/descriptions ≤ 160 chars, canonical, OG, BreadcrumbList)
Functional Testing: Pass
Build: Pass
Regression: Pass
Known Issues: NSDC certificate valid only to 25-Oct-2026; Director details to confirm (see inventory §5)
Final Status: PASS
Next Menu: Courses
```

### Task: Stage 3 – Courses menu

**Objective**
Rebuild both menu courses and the two unlinked course pages at their original URLs.

**Completed Work**
- `CoursesController`: Automotive Body Repair Technician at the original `/courses/`, Advanced Welding Technician, Two Wheeler Repair Technician and Welding Technician (the last two published but not in the original menu – kept out of the menu).
- Shared course components: facts strip, practical photo grid, module list, `_RelatedCourses` partial, Course JSON-LD per page.
- Automotive Body Repair: all original facts (ASDC/NSDC/NCVET, 8 + 4 months, SSLC/Plus Two, min. age 18, max 25 per batch, placement assistance, Level 4 model curriculum), course-structure timeline, qualification-pack PDF (old WordPress PDF URL 301-redirected), MAIAM workshop photos with lightbox, ASDC text, scholarships with verification note, course film + 3 YouTube course videos (`automotive-body-repair.js` keeps one playing at a time).
- Advanced Welding: original facts and the 3 module descriptions verbatim; processes from the original page and home page; MAIAM welding photos replace the stock images used on the old page.
- Two Wheeler Repair and Welding Technician (1 month): original content verbatim, with a "confirm batch availability" notice; no stock photography.

**Content edits to note (meaning preserved, flagged for review)**
- "trainers will be given placement in reputed firms" → "trainees are supported with placement in reputed firms" (typo fix; avoids promising guaranteed jobs).
- "By 2030, all the vehicles in the world will be electrified … Those who study this course will find jobs anywhere in the world" → "vehicles are moving towards electrification … body repair skills are valued around the world" (avoids an unverifiable prediction and a job guarantee).

**Files Created**
- `Controllers/CoursesController.cs`, `Views/Shared/_RelatedCourses.cshtml`
- `Views/Courses/AutomotiveBodyRepair|AdvancedWelding|TwoWheelerRepair|WeldingTechnician.cshtml`
- `wwwroot/css/automotive-body-repair|advanced-welding|two-wheeler-repair|welding-technician.css`
- `wwwroot/js/automotive-body-repair|advanced-welding|two-wheeler-repair|welding-technician.js`

**Files Modified**
- `wwwroot/css/common.css` (course facts, related courses, course gallery; `.fact` markup made valid `dt`/`dd`)

**Testing Performed**
- Build: 0 warnings, 0 errors.
- DevTools audit of all 4 pages at 1440/1024/768/375/320 px: no overflow, broken images, missing alt, duplicate IDs, heading skips, console errors or failed requests.
- Visual review at 1440 px; fixed full-width single related-course card and the course-video layout.
- Lighthouse mobile: ABR 96, Advanced Welding 95, Two Wheeler 100 performance; 100 accessibility/best practices/SEO.

```text
MAIAM MENU COMPLETION REPORT
Menu Name: Courses
Pages Completed: Automotive Body Repair Technician (/courses/), Advanced Welding Technician
                 (+ unlinked Two Wheeler Repair Technician and Welding Technician pages)
Files Created: 4 × .cshtml, 4 × .css, 4 × .js, _RelatedCourses partial
Original Content: Complete (2 softened statements listed above)
Original MAIAM Images: Verified (stock welding/two-wheeler images from the old site not used)
External Images: None used
UI/UX: Verified
Responsive: Verified (320–1440 px)
SEO: Verified (unique titles/descriptions, canonical, OG, BreadcrumbList + Course JSON-LD)
Functional Testing: Pass (lightbox, videos, PDF link, legacy PDF redirect)
Build: Pass
Regression: Pass
Known Issues: Availability of Two Wheeler and 1-month Welding courses to be confirmed
Final Status: PASS
Next Menu: Placements
```

### Task: Stage 4 – Placements

**Objective**
Rebuild `/our-placements/` with original placement content, separating placement assistance, industry visits, campus interviews and announced placements.

**Completed Work**
- `PlacementsController` + `Views/Placements/OurPlacements.cshtml`, `our-placements.css`, `our-placements.js`; new shared `_ImageTile` partial (`ImageTileModel`).
- Sections: placement support (assistance / dealership internships / campus interviews & visits) with an explicit "not a job guarantee" note; employers named on MAIAM's own placement banners and testimonials (Nippon Toyota, Gokulam Motors, Sai Service, Kia Motors, Hyson Motors, Annamalais Toyota, CAI Mahindra, PPS Mahindra) – no counts or rates; latest placement posters/banners; campus interview by Annamalai's Toyota (original heading); Mahindra Factory Visit – Coimbatore (original heading); Toyota plant visit; placed-student testimonial; archive of all earlier placement banners from the old home slider. Posters/banners shown whole (masonry columns, no cropping of names) with lightbox.
- Fix (all pages): a null `alt` passed through partials was bound as `""`, making page-hero images and lightbox tiles decorative. `_PageHero` and `_ImageTile` now pass the catalog alt text explicitly. Audit script extended with an unnamed-link check.

**Testing Performed**
- Build: 0 warnings, 0 errors.
- DevTools audit at 1440/1024/768/375/320 px: pass. Regression audit of all 13 completed pages at 1440 px with the new link-name check: pass.
- Lighthouse mobile: performance 96, accessibility 100 (was 96 before the alt fix), best practices 100, SEO 100.

```text
MAIAM MENU COMPLETION REPORT
Menu Name: Placements
Pages Completed: Our Placements (/our-placements/)
Files Created: OurPlacements.cshtml, our-placements.css, our-placements.js (+ _ImageTile partial)
Original Content: Complete
Original MAIAM Images: Verified (all placement photos, posters and banners)
External Images: None used
UI/UX: Verified
Responsive: Verified (320–1440 px)
SEO: Verified
Functional Testing: Pass (lightbox groups)
Build: Pass
Regression: Pass (13 pages)
Known Issues: None
Final Status: PASS
Next Menu: Faculties
```

### Task: Stage 5 – Faculties

**Completed Work**
- `FacultiesController` + `Views/Faculties/Faculties.cshtml`, `faculties.css`, `faculties.js`.
- The 3 original profiles (Mr. Girish Kumar, Mr. Ramesh Babu, Mr. Kishore) with original photos, designations, experience and achievements exactly as published; training-approach summary using only original facts (TOT certified trainers, roles and employers named on the original page).
- Fix (site-wide): buttons may wrap again (a global `nowrap` caused a 29 px overflow at 320 px); `nowrap` kept only for the header CTA, CTA band and hero buttons.

**Testing Performed**
- Build pass; DevTools audit at 1440/1024/768/375/320 px pass (after the button fix); regression at 320/375 px on Home, Courses, Placements, NSDC pass.
- Lighthouse mobile: performance 97, accessibility 100, best practices 100, SEO 100.

```text
MAIAM MENU COMPLETION REPORT
Menu Name: Faculties
Pages Completed: Faculties (/faculties/)
Files Created: Faculties.cshtml, faculties.css, faculties.js
Original Content: Complete
Original MAIAM Images: Verified (original faculty photographs, no stock portraits)
External Images: None used
UI/UX: Verified | Responsive: Verified | SEO: Verified
Functional Testing: Pass | Build: Pass | Regression: Pass
Known Issues: "25 years experienced trainers" (home) vs 20–21 years here – see inventory §5
Final Status: PASS
Next Menu: Our Facilities
```

### Task: Stage 6 – Our Facilities

**Completed Work**
- `FacilitiesController` + `Views/Facilities/OurFacilities.cshtml`, `our-facilities.css`, `our-facilities.js`.
- All original items: highlights (100% Placement – All India based dealerships, TOT Certified Trainers, BPL/APL Scholarship) plus "Fully mechanized practical lab" from the home page; Computer Lab, Hostel Facility, Transportation, Soft Skills Empowerment (original descriptions); Smart Classroom, Eco Friendly Campus, Disabled Friendly Campus and Restroom, Air Conditioned Classroom (titles only, as on the original).
- Real MAIAM photos are used only where they show the facility (workshop, computer lab, classrooms, campus). Hostel/transport have no photos and are shown as icon cards; smart/AC classroom are not paired with a specific photo.
- Photo tour (15 photos) with a category filter (`our-facilities.js`, `aria-pressed`, live status) that also limits the lightbox to visible photos.

**Testing Performed**
- Build pass; DevTools audit at 1440/1024/768/375/320 px pass; filter + lightbox interaction verified (Campus → 3 photos, lightbox "1 / 3", All → 15).
- Lighthouse mobile: performance 96, accessibility 100, best practices 100, SEO 100.

```text
MAIAM MENU COMPLETION REPORT
Menu Name: Our Facilities
Pages Completed: Our Facilities (/our-facilities/)
Files Created: OurFacilities.cshtml, our-facilities.css, our-facilities.js
Original Content: Complete
Original MAIAM Images: Verified (no stock classrooms/workshops)
External Images: None used
UI/UX: Verified | Responsive: Verified | SEO: Verified
Functional Testing: Pass | Build: Pass | Regression: Pass
Known Issues: Facility availability (hostel, transport) to be confirmed as current
Final Status: PASS
Next Menu: Gallery
```

### Task: Stage 7 – Gallery

**Completed Work**
- `GalleryController` + `Views/Gallery/Gallery.cshtml`, `gallery.css`, `gallery.js`.
- All original albums in original order with original titles: Appointment Letter Distribution Ceremony, Toyota Plant Visit, Mahindra Factory Visit – Coimbatore (2 photos moved here from "Drug Awareness Section", see inventory §5), Drug Awareness Session, Onam Celebrations 2024, Renovated Office Building, Workshop & Classroom Inauguration; the untitled remainder of the old gallery grouped as Commencement of New Batch, Practical Training & Campus, Festive Celebrations, Students & Faculty (78 photos in total, excluding stock graphics and outdated admission posters).
- Sticky album navigation with counts and current-album state; per-album lightbox groups with captions; large albums collapse to 8 photos with "Show all" (all photos visible without JS); album sections use `content-visibility` for fast rendering.
- Videos section (`#videos`) with the 2 original gallery videos and the 3 original Video Gallery videos (old `/video-gallery/` 301 → `/gallery/#videos`; `/gallery1-4/` → `/gallery/`). Instagram follow block replaces the plugin feed.

**Testing Performed**
- Build pass; DevTools audit at 1440/1024/768/375/320 px pass; redirects verified; "Show all" (8 → 25, focus moves to photo 9) and 10 lightbox groups verified.
- Lighthouse mobile: performance 98, accessibility 100, best practices 100, SEO 100.

```text
MAIAM MENU COMPLETION REPORT
Menu Name: Gallery
Pages Completed: Gallery (/gallery/) incl. merged Video Gallery
Files Created: Gallery.cshtml, gallery.css, gallery.js
Original Content: Complete (all albums, all MAIAM photos, all 5 gallery/video-gallery videos)
Original MAIAM Images: Verified (no stock images)
External Images: None used
UI/UX: Verified | Responsive: Verified | SEO: Verified
Functional Testing: Pass | Build: Pass | Regression: Pass
Known Issues: Album regrouping of 2 photos to be confirmed (inventory §5)
Final Status: PASS
Next Menu: Contact Us
```

### Task: Stage 8a – Contact Us

**Completed Work**
- `ContactController` + `Views/Contact/Contact.cshtml`, `contact.css`, `contact.js`.
- Original contact details (address with pin code, both phone numbers as click-to-call, email) plus WhatsApp, copy-email button, "Get directions" (Google Maps directions to the original map coordinates), Managed-by block with Manappuram Foundation contact details, EducationalOrganization JSON-LD.
- Map: the original Google Maps embed loads only when the visitor presses "Show map" (`contact.js`); without JavaScript an "Open in Google Maps" link is shown. Keeps third-party requests off the page until requested.

**Testing Performed**
- Build pass; DevTools audit at 1440/1024/768/375/320 px pass (one intentionally decorative map placeholder photo); map load interaction verified (iframe inserted with title, focus moved); server log clean.
- Lighthouse mobile: performance 92, accessibility 100, best practices 100, SEO 100.

```text
MAIAM MENU COMPLETION REPORT
Menu Name: Contact Us
Pages Completed: Contact Us (/contact-us/)
Files Created: Contact.cshtml, contact.css, contact.js
Original Content: Complete (address, phones, email, map location)
Original MAIAM Images: Verified
External Images: None used
UI/UX: Verified | Responsive: Verified | SEO: Verified
Functional Testing: Pass | Build: Pass | Regression: Pass
Known Issues: Contact details to be re-confirmed by MAIAM before go-live
Final Status: PASS
Next: Register Now (enquiry form – storage method needs owner decision)
```

### Task: Stage 8b – Register Now (enquiry form, email delivery)

**Decision**
Owner chose email delivery via SMTP (2026-10-14). No database changes; no SQL scripts.

**Completed Work**
- `RegisterController` (GET/POST `/register-now/`), `Models/RegisterEnquiry.cs`, `Services/EnquiryEmailSender.cs` (`EmailSettings`, `IEnquiryEmailSender`, `SmtpEnquiryEmailSender` using built-in `System.Net.Mail` – no new package), `Views/Register/RegisterNow.cshtml`, `register-now.css`, `register-now.js`, `docs/EMAIL_SETUP.md`.
- Fields: the 5 original fields (name, address, educational qualification, contact number, age) + optional email, course of interest, comments (original page: "Comments or questions are welcome").
- Security: anti-forgery token, honeypot, signed minimum fill time (Data Protection), rate limit 10 POSTs / 10 min / IP with a friendly 429 page, server + client validation, length limits, plain-text email with single-line subject (no header injection), Reply-To only when a valid email is given, PRG redirect with one-time success message.
- Honest behaviour: success is shown only after the mail server accepts the message; otherwise the form keeps the entries and shows call/WhatsApp/email alternatives.
- Performance/accessibility: jQuery validation loads on first interaction with the form; `aria-invalid` + `aria-describedby` on invalid fields; focus moves to the first error or the result message.
- `Program.cs`: `Email` settings binding and sender registration; rate limit raised to 10/10 min. `HomeController`/`Error.cshtml`: friendly 429 message.

**Configuration (owner action required)**
- Add SMTP settings (`Email:Host`, `UserName`, `Password`, `FromAddress`, …) via user secrets or IIS environment variables – see `docs/EMAIL_SETUP.md`. `appsettings.json` was not modified.

**Testing Performed**
- Without SMTP: valid submission → "could not be sent" message, entries kept (no false success); invalid input → 3 server-side field errors; honeypot → rejected; submitted within 2 s → rejected; missing anti-forgery token → 400; 11th POST within 10 min → 429 page.
- With a pickup-directory test instance (port 5199, stopped afterwards): valid submission → 302 to `/register-now/#enquiry-status`, success message shown once, `.eml` generated with correct From/To/Reply-To/Subject/body.
- Browser: client-side validation loads on interaction and flags 4 fields; `aria-invalid`/`aria-describedby` and focus verified; server-side fallback verified when scripts are not loaded.
- DevTools audit at 1440/1024/768/375/320 px pass. Lighthouse mobile: performance 95–96 (78 before lazy-loading validation), accessibility 100, best practices 100, SEO 100.
- Found and fixed during testing: the form initially rendered **without** an anti-forgery token (hand-written `action` attribute); now generated by `asp-controller`/`asp-action`.

```text
MAIAM MENU COMPLETION REPORT
Menu Name: Register Now (header CTA "Enquire Now")
Pages Completed: Register Now (/register-now/)
Files Created: RegisterNow.cshtml, register-now.css, register-now.js (+ controller, model, email service)
Original Content: Complete (original fields and intro text)
Original MAIAM Images: n/a | External Images: None used
UI/UX: Verified | Responsive: Verified | SEO: Verified
Functional Testing: Pass | Build: Pass | Regression: Pass
Known Issues: Email delivery inactive until SMTP settings are configured (by design)
Final Status: PASS (pending SMTP configuration)
Next: Final migration audit
```

## 2026-10-19

### Task: Fix – blank pages when debugging in Visual Studio

**Problem**
Pages (reported on `/about-maiam/`) showed as a blank white page at `https://localhost:7091` when run from Visual Studio. The server returned HTTP 200 with the full page.

**Root cause**
In Development, Visual Studio's Browser Link and Hot Reload append `<script>` tags to every HTML response. The site compressed responses (Brotli) before that injection, so the browser received a compressed stream with uncompressed text appended, failed to decode it and rendered nothing. Confirmed by capturing an Edge-style request: body ended in the plain-text Browser Link / `aspnetcore-browser-refresh.js` scripts and Brotli decoding failed. Not visible in earlier tests because those ran outside Visual Studio.

**Fix**
- `Program.cs`: `UseResponseCompression()` only outside Development (production/IIS still compressed).
- `Infrastructure/SecurityHeadersMiddleware.cs`: in Development only, CSP allows `http://localhost:*` scripts and `http/ws/wss://localhost:*` connections for Browser Link / Hot Reload. Production CSP unchanged.
- `.claude/launch.json`: Claude's preview server moved to port 5180 so it no longer occupies Visual Studio's ports 5174/7091 (an earlier session left VS paused at `app.Run()` because 5174 was in use).

**Testing Performed**
- Separate Release build run in Development (port 5180): `/about-maiam/` served uncompressed (32,901 bytes), Browser-Link-style script appended → still valid HTML with correct title.
- Same build in Production (port 5299): Brotli-compressed (9,011 bytes), strict CSP.
- Visual Studio's own running session was not touched; it needs a restart to pick up the change.

**Manual Steps**
- In Visual Studio: stop debugging (Shift+F5) and start again (F5), then reload the page.

**Rollback**
- Revert the two code changes above (re-enables compression in Development).

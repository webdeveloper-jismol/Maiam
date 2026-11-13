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

### Task: Publish to GitHub

**Objective**
Push the project to https://github.com/webdeveloper-jismol/Maiam (no pull, per owner instruction).

**Completed Work**
- Initialised a git repository in `E:\Maiamnew` (none existed) and created `.gitignore`: build output (`bin/`, `obj/`, `publish/`), `.vs/`, logs, certificates and **`appsettings.json`**.
- Added `Maiamnew/appsettings.Example.json` with placeholder connection string and Email settings, as the template to copy.
- Commit `59a684d` – 570 files, 90 MB.
- Added remote `origin` and force-updated `main` (owner chose force-overwrite over a separate branch).

**Security**
- The repo is **public** and `Maiamnew/appsettings.json` held a working SQL Server login (`sa`). It is git-ignored and was **not** published – verified: the file returns HTTP 404 on the GitHub API, `appsettings.Example.json` returns 200. The local file was left untouched so Visual Studio keeps working.
- `git grep` over all tracked files found no credentials.

**Protecting the previous work**
- `main` previously held an earlier, unrelated MAIAM build (`Maiam/`, `Maiam_DAL/`, last commit `ebc7f75`, 13 Aug 2026). Before the force-push, those commits were preserved as remote branch **`backup-main-2026-08-13`** (`ebc7f75`). The force-push used `--force-with-lease` so it would have aborted if the remote had moved.

**Result**
- `main` = `59a684d` (local and remote match). Remote branches: `main`, `backup-main-2026-08-13`.
- `Maiamnew/wwwroot/assets/videos/maiam-brand-film.mp4` is 48.3 MB – under GitHub's 100 MB limit but large; consider Git LFS or external hosting if the repo grows.

**Manual Steps**
- On any new clone: copy `Maiamnew/appsettings.Example.json` to `Maiamnew/appsettings.json` and fill in the real connection string.
- Delete `backup-main-2026-08-13` once you are satisfied the old build is no longer needed.

**Rollback**
- To restore the previous `main`: `git push --force origin backup-main-2026-08-13:main`.

## 2026-11-11

### Task: Remove the second phone number (9072623103)

**Objective**
Owner requested the second contact number (+91 90726 23103) be removed from the site.

**Completed Work**
- `Infrastructure/SiteInfo.cs`: deleted the `Phone2Display` / `Phone2Tel` constants, so the number cannot reappear anywhere.
- `Views/Shared/_Footer.cshtml`: Contact Us block now lists one number.
- `Views/Contact/Contact.cshtml`: removed from the "Contact Number" card and from the page meta description (reworded, 155 chars).
- `Views/Register/RegisterNow.cshtml`: removed from the "Prefer to talk?" sidebar and from the "could not be sent" fallback message.
- `docs/MIGRATION_INVENTORY.md`: shared-content row updated to record the removal.
- Not changed: Manappuram Foundation's toll-free number (1800 123 6866) in the "Managed By" blocks, and the WhatsApp link, which uses the 75948 10851 number.

**Testing Performed**
- Compiled to a separate output folder (the normal build was blocked by the running Visual Studio session): 0 warnings, 0 errors.
- Crawled all 18 pages on a test instance: **0 occurrences** of `9072623103` / `90726 23103`. Remaining `tel:` links are `+917594810851` and the Foundation's `18001236866` only.
- Visual check at 1280 px (contact cards, footer, Register Now sidebar) and 375 px (no horizontal overflow) — no gaps or broken layout from the removed lines.

**Note**
The number still appears inside some original MAIAM artwork (placement banners, posters, video overlays), which are photographs and cannot be edited in code. Replacement artwork would have to come from MAIAM.

**Rollback**
- Restore the two constants in `SiteInfo.cs` and the removed markup lines in the four views.

### Task: Reword the enquiry CTA and add the second number back to it

**Objective**
Owner request: change the CTA heading "Interested in a MAIAM course?" to "Start Your Automotive Career with MAIAM", and show +91 90726 23103 in that band.

**Completed Work**
- `Views/Shared/_EnquiryCTA.cshtml`: new heading; added a fourth action button for +91 90726 23103 (click-to-call), alongside Enquire Now, +91 75948 10851 and WhatsApp.
- `Infrastructure/SiteInfo.cs`: restored the `Phone2Display` / `Phone2Tel` constants (removed earlier the same day) so the number has a single source.
- `wwwroot/css/common.css`: with four actions the old `flex-wrap: nowrap` rule produced a ragged three-row staircase at desktop width; the actions are now an even 2 x 2 grid (equal 224 px buttons) from 1024 px up, and still stack naturally below that.

**Scope note**
The second number is currently shown **only in this CTA band**. The footer, Contact Us card and Register Now sidebar still list +91 75948 10851 alone, per the earlier removal. Awaiting owner confirmation on whether to restore it there too.

**Testing Performed**
- Compiled to a separate output folder: 0 warnings, 0 errors.
- All 17 pages carrying the CTA band show the new heading and the number; 0 pages still show the old heading. (`/register-now/` has no CTA band by design.)
- Layout verified at 1440 px (2 x 2 block, no overflow), 768 px and 375 px (buttons stack, no horizontal overflow).

### Task: Highlight the two closing statements on About MAIAM

**Objective**
Owner request: emphasise the closing sentence of each of the two About MAIAM sections.

**Completed Work**
- New shared `.statement` component in `wwwroot/css/common.css`: orange left rule, soft orange-tinted panel, heading font at 600 weight in navy, scaling 16–18.4 px. Written to override the muted `.prose p` colour, with a variant for dark sections so it can be reused elsewhere.
- `Views/About/AboutMaiam.cshtml`: applied to "At MAIAM, our commitment goes beyond training…" (Introduction) and "Our mission extends beyond imparting technical skills…" (Empowering Aspirations). Wording unchanged.

**Testing Performed**
- Build: 0 warnings, 0 errors. Page renders exactly 2 highlighted statements.
- Verified at 1440 px (both sections, white and grey backgrounds) and 375 px — no horizontal overflow, panel sits within the content column.

**Note**
`.statement` is reusable; the same class can mark key sentences on other pages if wanted.

### Task: CTA band – keep only the newly added number

**Objective**
Owner request: in the enquiry CTA band, drop +91 75948 10851 and keep +91 90726 23103.

**Completed Work**
- `Views/Shared/_EnquiryCTA.cshtml`: removed the +91 75948 10851 button. The band is now Enquire Now · +91 90726 23103 · WhatsApp.
- `wwwroot/css/common.css`: reverted the actions block from the 2 x 2 grid (added for four buttons) back to the single-row flex layout, which suits three.

**Scope note**
This covers the CTA band only. +91 75948 10851 is still shown in the top bar, footer, Contact Us card, Register Now sidebar and its fallback message, and is the number behind the WhatsApp link. Awaiting owner confirmation on whether to swap it site-wide.

**Testing Performed**
- Build: 0 warnings, 0 errors. Band renders 3 buttons on one row at 1440 px, stacked at 375 px, no overflow at either.

### Task: Remove "Level 4" from the website

**Objective**
Owner request: remove every reference to "Level 4" from the site (reported first, then approved).

**Completed Work — 21 text occurrences across 6 files**
- `Views/Courses/AutomotiveBodyRepair.cshtml` (11): hero label, "Become an Automotive Body Repair Technician", two body sentences, Qualification Pack heading, PDF link, video section heading/caption/aria-label/download text/intro, page description and Course JSON-LD.
- `Views/About/Objectives.cshtml` (2): body sentence now "…first training centre in Kerala to start Automotive Body Repair training."; page description.
- `Views/Home/Index.cshtml` (3): video caption, aria-label, download link.
- `Views/Certifications/AffiliationCertificate.cshtml` (1): "Model curriculum: Automotive Body Technician."
- `Services/SiteContent.cs` (3): the three YouTube titles (shown on Gallery and Courses).
- `Infrastructure/LegacyUrlMiddleware.cs`: redirect target updated.

**Document renamed**
- `qp-automotive-body-repair-technician-level-4.pdf` → `qp-automotive-body-repair-technician.pdf` (via `git mv`).
- Two redirects now point at the new name: the original WordPress URL and the previous path on this site (which was already live on the test server).

**Docs updated**: `docs/IMAGE_SOURCES.md`, `docs/MIGRATION_INVENTORY.md`. Earlier dated entries in this log are left as the historical record.

**Testing Performed**
- Build: 0 warnings, 0 errors.
- Crawled all 18 pages: **0 occurrences** of "Level 4" / "level-4" in rendered HTML (was 41 across 5 pages).
- PDF: new path 200; both old paths 301 → new path.
- Meta descriptions still within length (Courses 160, Objectives 156). Visual check of the Courses hero, qualification-pack card and video section at 1440 px.

**Still showing "Level 4" — cannot be changed in code**
- The course video overlay ("AUTO BODY TECHNICIAN LEVEL 4") on Home and Courses, including its poster image. The opening frame (~1 s, MAIAM logo on white) is badge-free and could serve as a poster, but the badge returns once the video plays.
- The three YouTube thumbnails and the video titles on the Manappuram Foundation channel.
- The qualification-pack PDF itself is the Level 4 document.
New media from MAIAM is required to clear these.

### Task: Restructure Eligibility Criteria and Career Progression (Courses page)

**Problem**
Newly added copy on `/courses/` put both blocks inside a single `<p>`, with literal "•" characters and line breaks. HTML collapses those, so it rendered as one long run-on paragraph with the two headings buried in the text.

**Completed Work**
- Removed the run-on paragraph from the "About the course" prose.
- Added a dedicated section, "Who can join, and where it leads" (eyebrow: Admission & careers), between About and Course structure, as two cards:
  - **Eligibility Criteria** – the two requirements as a tick list (`.check-list`).
  - **Career Progression** – lead-in sentence, the five workplace types as chips, then the closing sentence below a divider.
- Wording is unchanged; only the presentation differs. The list of workplaces was split out of the sentence into separate items so it can be scanned.
- New `.abr-pathway` styles in `automotive-body-repair.css`; the career card is the wider column (1 : 1.35) since it carries more content.

**Testing Performed**
- Build: 0 warnings, 0 errors. Old run-on paragraph no longer present; new section renders all 7 items.
- Verified at 1440 px (two cards side by side) and 375 px (stacked, chips wrap) — no horizontal overflow at either.

### Task: Remove the Computer Lab facility

**Objective**
Owner request: remove Computer Lab and its photograph from the site.

**Completed Work** — removed site-wide so the facility is not advertised anywhere
- `Views/Facilities/OurFacilities.cshtml`: deleted the Computer Lab feature card and its photo; section heading "Classrooms and computer lab" → "Classrooms"; removed the computer-lab photo from the photo tour (15 → 14 photos); page description reworded.
- Section relaid out: the classroom photograph is now the left/feature element with the Smart Classroom and Air Conditioned Classroom cards stacked beside it (`our-facilities.css`: side column single-column with equal rows, compact cards vertically centred, photo 16/10).
- `Services/SiteContent.cs`: removed the Computer Lab entry from `Facilities` (this also drops it from the Home facilities list, which now shows Hostel Facility, Transportation, Soft Skills Empowerment, Smart Classroom) and the computer-lab image from the "Practical Training & Campus" gallery album.
- `Views/Home/Index.cshtml`: the facilities section photo was the computer lab — replaced with `facilities/classroom`.
- `Views/About/DirectorsMessage.cshtml`: "Workshop, classrooms, computer lab, hostel and transport." → "Workshop, classrooms, hostel and transport."

**Not removed**
- The image files `wwwroot/assets/images/facilities/computer-lab-*.webp` and their generated `ImageCatalog` entry remain in the project but are no longer referenced by any page, so nothing is served to visitors. Left in place so the facility can be restored easily; say the word to delete them.

**Testing Performed**
- Build: 0 warnings, 0 errors.
- Crawled all 18 pages: **0** occurrences of "computer lab".
- Verified the rebuilt Classrooms section at 1440 px and 375 px (stacks correctly, no horizontal overflow) and the Home facilities section with its new photo. The classroom photo is lazy-loaded; confirmed it loads at both widths.

### Task: Remove Soft Skills Empowerment and the hostel description

**Objective**
Owner request: remove the "Soft Skills Empowerment" facility, and the line "Free accommodation for MAIAM students." under Hostel Facility.

**Completed Work**
- `Services/SiteContent.cs`: deleted the Soft Skills Empowerment entry; Hostel Facility now has no description (title only, like Smart Classroom and the campus items).
- `Views/Facilities/OurFacilities.cshtml`: Student support section now has two cards — Hostel Facility (title only) and Transportation (description kept). New `.support-grid` caps the row at ~820 px so two cards do not stretch across the full width.
- `Views/Home/Index.cshtml`: the facilities list renders the description only when there is one, so title-only items no longer emit an empty `<p>`. The list (first four facilities) is now Hostel Facility, Transportation, Smart Classroom, Eco Friendly Campus.

**Testing Performed**
- Build: 0 warnings, 0 errors.
- "Soft Skills" and "Free accommodation" return 0 matches across the site; 0 empty `<p></p>` on the home page.
- Verified the Student support section and the Home facilities list at 1440 px and 375 px — no horizontal overflow.

### Task: Centre the Student support cards

**Completed Work**
- `wwwroot/css/our-facilities.css`: added `margin-inline: auto` to `.support-grid`, so the two remaining cards sit centred in the section instead of hugging the left edge.

**Testing Performed**
- Build pass. At 1440 px the card row has equal 242 px gaps either side. At 375 px the grid is full width (343 px inside the gutters) with no horizontal overflow, so the centring has no effect on phones.

**Note**
Only the cards are centred; the "Support beyond the classroom" heading stays left-aligned to match every other section heading on the site.

---

## 2026-11-13 – Contact Us message form

### Task: Add a Contact Us form (modelled on the Mageet school site) delivering to mafoundteamit@gmail.com

**Objective**
Owner request: give the Contact page the same kind of contact form the Mageet site has, with
messages emailed to `mafoundteamit@gmail.com`. The MAIAM Contact page previously showed contact
details and a map only.

**Completed Work**

*Backend*
- `Models/ContactMessage.cs` (new): Name, Email, Phone (all required), Course (optional), Message
  (5–1000 chars), plus the `Website` honeypot and signed `FormToken`. Validation mirrors the
  Register Now model — Malayalam-safe name regex, bare-address email regex, Indian mobile regex.
- `Infrastructure/FormGuard.cs` (new): the spam guard (DataProtection-signed render timestamp,
  2-second minimum fill time) extracted from `RegisterController` so both forms share one copy.
  Registered as a singleton in `Program.cs`; `RegisterController` now uses it instead of its own
  private implementation.
- `Controllers/ContactController.cs`: GET issues a fresh form token; POST is `[ValidateAntiForgeryToken]`
  + `[EnableRateLimiting("forms")]`, normalises input, re-validates, checks honeypot and fill time,
  and uses the PRG pattern (`TempData["ContactSent"]` → `/contact-us/#message-status`). It never
  reports success unless the mail server accepted the message.
- `Services/EnquiryEmailSender.cs`: added `EmailSettings.ContactToAddress` (default
  `mafoundteamit@gmail.com`) and a `SendAsync(ContactMessage, …)` overload. The message is sent
  **from** the site mailbox with **Reply-To** set to the visitor, so it passes SPF/DKIM while Reply
  still reaches the visitor. Subject is forced to a single line; the body is plain text.

*Front end*
- `Views/Contact/Contact.cshtml`: new "Send us a message" section between the contact cards and the
  map — an intro column (phone / WhatsApp / email, and a pointer to Register Now for admissions)
  beside the form card. The map section moved to `section--light` so the two alternate.
- `wwwroot/css/common.css`: promoted the generic form rules that previously lived only in
  `register-now.css` — `.form-status`, `.form-optional`, `.form-actions`, `.form-card`, and the
  disabled-while-submitting button state — so both forms share them.
- `wwwroot/css/contact.css`: `.contact-form-layout` (sticky intro + form, two columns from 1024 px,
  stacked below) and `.contact-form-intro__list`.
- `wwwroot/js/forms.js` (new, replaces `register-now.js`): the same behaviour, generalised to every
  `[data-form]` on a page — lazy-loaded jQuery validation on first interaction, character counter
  (driven by `aria-describedby` instead of a hard-coded `#Message`), double-submit protection with
  a "Sending…" state, `aria-invalid` syncing, and focus moved to the result message via
  `data-form-status`. `register-now.js` was deleted; both pages now load `forms.js`.

*Configuration and docs*
- `appsettings.Example.json`: added `Email:ContactToAddress`.
- `docs/EMAIL_SETUP.md`: rewritten to cover both forms, with a Gmail App Password section. **The
  password is not in the repository** — it must be set through user secrets (development) or
  environment variables (IIS). Note recorded there that configuring SMTP enables both forms at once.

**Testing Performed**
Run on `http://localhost:5180` with `Email:PickupDirectory` set through user secrets (messages
written as `.eml` files instead of sent); the setting was removed again after testing.
- Build: 0 warnings, 0 errors.
- Valid submission → redirect to `/contact-us/#message-status`, success message shown, and the
  `.eml` produced has `To: mafoundteamit@gmail.com`, `Reply-To: "Test Visitor" <…>`, single-line
  subject and the full plain-text body.
- Empty submission → server returns the form with four field errors and sends nothing.
- POST without an anti-forgery token → **HTTP 400**.
- POST faster than the 2-second minimum → rejected ("could not be processed"), nothing sent.
- POST with the honeypot filled → rejected, nothing sent.
- Rate limit: repeated posts return **HTTP 429** with the branded "Too many attempts" page.
- Register Now re-tested after the `FormGuard` / `forms.js` refactor: valid enquiry still delivers
  to `maiam@manappuramfoundation.org`; card styling, character counter and submit state unchanged.
- Layout checked at 1440 px and 375 px on both pages: no horizontal overflow, no console errors.

**Not Done / Owner Action Required**
- The Gmail App Password has to be set by the owner (it must never be committed):
  `dotnet user-secrets set "Email:Password" "<app password>" --project Maiamnew`, along with
  `Email:Host`, `Email:Port`, `Email:UserName` and `Email:FromAddress`.
- Until those settings exist, both forms validate input and show the honest "could not be sent —
  please call or email" message.

### Fix: email settings needed an app restart to take effect

**Problem reported**
"Contact form mail arrives but the Register Now enquiry form does not work."

**Diagnosis**
Neither form was broken. Submitting both through the running instance returned their success
messages, and an SMTP check against `smtp.gmail.com:587` authenticated successfully (`235 Accepted`),
so mail was being accepted by Gmail in both cases.

The real cause was stale configuration: the running process started at 10:55:49 while
`appsettings.json` was last edited at 11:11:54. `SmtpEnquiryEmailSender` took
`IOptions<EmailSettings>`, which is resolved once and cached for the life of the process, so edits
to `Email:ToAddress` / `Email:ContactToAddress` after start-up were ignored and each form kept
mailing the address configured earlier.

**Completed Work**
- `Services/EnquiryEmailSender.cs`: `SmtpEnquiryEmailSender` now takes `IOptionsSnapshot<EmailSettings>`
  instead of `IOptions<EmailSettings>`. The service is registered scoped, so the settings are re-read
  once per request and changes to the `Email` section apply on the next submission without a restart.
- `docs/EMAIL_SETUP.md`: noted that the two destination keys are separate — `ToAddress` is Register
  Now, `ContactToAddress` is Contact Us.

**Testing Performed**
- Compile check (`dotnet build -t:Compile`): 0 warnings, 0 errors. A full build could not write to
  `bin` because the owner's own instance held `Maiamnew.exe`; the running app must be restarted once
  for this change to take effect.
- Live submissions through the owner's running instance: Contact Us and Register Now both returned
  their success messages, confirming Gmail accepted both messages.

### Task: HTML enquiry emails (matching the Mageet School layout) + Register Now destination

**Objective**
Owner request: send Register Now enquiries to `jizmol29@gmail.com`, and format both forms' emails
like the Mageet School website's enquiry mail (dark header band, label/value detail table, Message
panel, footer note) instead of plain text.

**Completed Work**
- `Services/EnquiryEmailTemplate.cs` (new): builds the HTML and plain-text bodies. Email-client-safe
  HTML — nested tables, inline styles only, `bgcolor` alongside `background-color`, no flex/grid, a
  fixed 600px wrapper for Outlook desktop, and a hidden preheader. Styled in MAIAM's own colours:
  navy `#14283F` header with a 4px `#F28C28` rule under it, navy labels, `#1769AA` email links, and
  an orange-bordered message panel. Every submitted value is HTML-encoded; `Address` keeps its line
  breaks via `<br />` only.
- `Services/EnquiryEmailSender.cs`: both `BuildMessage` overloads now go through a shared
  `NewMessage` helper that attaches **two** alternate views — `text/plain` first, `text/html` second
  — so clients that cannot render HTML still show a readable enquiry. Subjects are now
  "New Website Enquiry – {name}" (Contact Us) and "New Course Enquiry – {name}" (Register Now).
  The old plain-text-only `BuildBody` methods were removed.
- `appsettings.json` (local, git-ignored): `Email:ToAddress` → `jizmol29@gmail.com`.

**Testing Performed**
- Compile check (`dotnet build -t:Compile`): 0 warnings, 0 errors.
- Rendered both emails from the real template code in a throwaway console host and inspected the
  output: header band, detail rows, Submitted On row, message panel and footer all present and
  well-formed.
- Escaping: fed `<script>alert(1)</script>` as a name and `<img src=x onerror=alert(2)>` as the
  message. Output contains **0** raw `<script` and **0** raw `<img` — both render as visible text.
- Plain-text alternative checked by eye; reads correctly with the same fields.
- Note: previewing the email through the site itself shows it unstyled, because the site's CSP is
  `style-src 'self'` and the browser drops inline `style` attributes. Mail clients do not apply CSP,
  so this affects the preview only, not the delivered email.

**Not Done / Owner Action Required**
- The running instance must be restarted to pick up the new template (a full build could not write
  to `bin` while the owner's app held `Maiamnew.exe`).

### Task: Validation audit of both forms, and two fixes to Register Now

**Objective**
Owner question: do both forms have proper validation? Audited every field on both forms against the
real model metadata, and checked what client-side rules each page emits.

**Audit result**
Contact Us was sound. Register Now had two genuine gaps, both now fixed:

1. **Permissive email validation.** `RegisterEnquiry.Email` used `[EmailAddress]`, which accepted
   `Name <a@b.com>` and `a@b c`. That value becomes the email's `Reply-To`, so the first would have
   smuggled a visitor-controlled display name into the header, and the second would have thrown
   `FormatException` when the address was parsed — caught by the sender, so the enquiry would have
   been silently lost behind a generic "could not be sent". Replaced with the same strict
   bare-address regex the Contact form already used. Blank is still accepted (the field is optional).
2. **No input normalisation.** `ContactController` cleaned control characters and whitespace-only
   input before validating; `RegisterController` did not. Register Now now does the same.

**Completed Work**
- `Infrastructure/FormText.cs` (new): `SingleLine` / `MultiLine` cleaners, moved out of
  `ContactController` so both controllers share one copy rather than duplicating it.
- `Controllers/RegisterController.cs`: added `Normalise(model)` + `ModelState.Clear()` +
  `TryValidateModel(model)` at the top of the POST, mirroring `ContactController`.
- `Controllers/ContactController.cs`: now calls the shared `FormText` helpers.
- `Models/RegisterEnquiry.cs`: `Email` now uses the strict bare-address regex instead of
  `[EmailAddress]`, with an explicit 150-character message.

**Testing Performed**
- Compile check: 0 warnings, 0 errors.
- Ran a 42-case validation matrix through the real `DataAnnotations` metadata for both models
  (empty, whitespace-only, under/over length, script tags, Malayalam names, apostrophes and hyphens,
  non-Indian and malformed phone numbers, out-of-range ages, malformed emails). All behaved as
  intended; the only ACCEPTs outside the baselines were the deliberately permissive free-text
  fields, `Course` and `Qualification`.
- Re-ran the email cases after the fix: blank and `a@b.com` accepted; `a@b c`, `Name <a@b.com>`,
  `a@b.com,c@d.com`, `a@b` and 151 characters all rejected.
- Confirmed from the served HTML that both pages emit unobtrusive client-side rules
  (Contact: 4 required / 6 regex / 10 length; Register: 5 required / 4 regex / 14 length / 3 range)
  plus the anti-forgery token and the lazy validation-script tag.

**Known, accepted**
- `Course` (both forms) and `Qualification` (Register) accept any text within their length limits.
  `Course` is a `<select>` and `Qualification` an open input with a datalist, so a crafted POST can
  put arbitrary text there. It is only emailed, and is HTML-encoded on the way out, so the impact is
  limited to a nonsense value in the enquiry mail.

---

## 2026-11-13 – New MAIAM photographs

### Task: Add 11 supplied MAIAM photographs to the Gallery, Auto Body course, Advanced Welding course and Home page

**Objective**
Owner supplied 11 original MAIAM photographs and asked for them in the Gallery, with the best of
them used on the Auto Body course page and the Home page. On a follow-up question the owner also
approved refreshing the Advanced Welding course page with the new welding shots.

**Approval obtained**
This machine had no WebP encoder (no sharp, no ImageMagick; Windows' own imaging stack cannot write
WebP). Per the project rule on packages, the owner was asked and chose a **throwaway install outside
the repository** — `sharp` was installed into a temporary folder, used once, and deleted. Nothing
was added to the project, to git, or to any package manifest.

**Completed Work**
- Processed all 11 JPEGs (2000×1331, 1331×2000, 1920×1080, 1126×2000) into the site's existing
  responsive WebP format at quality 72 — **44 files, 1.87 MB total**, in line with the existing
  images (480 px ≈ 11–32 KB, 1600 px ≈ 59–155 KB).
- `Infrastructure/ImageCatalog.Generated.cs`: 11 new entries, each with hand-written alt text
  describing only what is visible in the photograph. Catalog is now 120 entries.
- `Services/SiteContent.cs`: the 10 training photographs added to the top of the
  "Practical Training & Campus" album (24 → 34 photos); the celebration photograph added to
  "Festive Celebrations" (8 → 9).
- `Views/Home/Index.cshtml`: hero secondary tile → `advanced-welding/student-mask-welding`;
  three of the five training-grid tiles replaced with new photographs (MIG practice, sheet-metal
  marking, bumper heat repair) with matching captions; gallery preview now leads with the new
  celebration photo and the new classroom session.
- `Views/Courses/AutomotiveBodyRepair.cshtml`: hero → `bumper-heat-repair`; the 450 px
  `dent-repair-equipment` image (the lowest-resolution asset on the page) replaced in the media
  frame by `sheet-metal-marking`; practical strip rebuilt from the new photographs. Also dropped
  `quarter-panel-replacement`, which has promotional text burnt into the image, in favour of a clean
  photograph.
- `Views/Courses/AdvancedWelding.cshtml`: hero → `booth-welding-practice`; tall column image →
  `student-mask-welding`; practical strip now leads with the two new MIG practice photographs.
- `docs/IMAGE_SOURCES.md`: new section recording all 11, their renditions and alt text, and noting
  that the original JPEGs are deliberately **not** kept in the repository.

**Testing Performed**
Because the owner's instance held `bin`, a Release build was published to a scratch folder and run
on port 5181 for verification.
- Build: 0 warnings, 0 errors (compile + Razor generation).
- Catalog audit: 120 entries, **0 missing rendition files**; every literal image key used in any
  `.cshtml` resolves to a registered entry (**0 unregistered**).
- Decoded a sample of the output with sharp to confirm valid WebP at the catalogued dimensions, and
  confirmed they are served as `image/webp` with HTTP 200.
- Home, Auto Body course, Advanced Welding course and Gallery, at **1280 px and 375 px**, after
  scrolling to force every lazy image to load: **0 broken images, 0 horizontal overflow** on all
  four pages at both widths.
- Gallery album counts verified in the rendered page: Practical Training & Campus 34, Festive
  Celebrations 9; gallery page now renders 97 images.

**Note**
The owner's running instance must be restarted to see these changes — Razor views are compiled into
the assembly, so the already-running build still serves the previous pages.

### Task: Remove the Director's Message

**Objective**
Owner request: remove the Director's Message — both the standalone page and the teaser on About MAIAM.

**Completed Work**
- Deleted `Views/About/DirectorsMessage.cshtml`, `wwwroot/css/directors-message.css` and
  `wwwroot/js/directors-message.js`. Checked first that nothing else used their classes
  (`.director`, `.director-link`) — they were used only by that page.
- `Controllers/AboutController.cs`: removed the `[Route("directors-message")]` action.
- `Views/About/AboutMaiam.cshtml`: removed the Director's message teaser section.
- `wwwroot/css/about-maiam.css`: removed the now-orphaned `.director-teaser*` rules (40 lines).
- `Infrastructure/SiteNavigation.cs`: removed `/directors-message/` from `IndexablePaths`, so it no
  longer appears in sitemap.xml.
- `Infrastructure/LegacyUrlMiddleware.cs`: added `/directors-message/` → `/about-maiam/` as a **301**.
  The page was an original WordPress URL, so redirecting rather than 404-ing preserves any existing
  inbound links and search results.
- `docs/MIGRATION_INVENTORY.md`: the row for `/directors-message/` now records the removal, and the
  "confirm the current director" verification item is struck through as no longer applicable.

**Testing Performed**
Verified on a Release build published to a scratch folder (the owner's instance held `bin`).
- Build: 0 warnings, 0 errors.
- Reference sweep across `.cs`, `.cshtml`, `.css`, `.js`, `.json`: the only remaining mentions are
  the intentional redirect entry and an explanatory comment.
- `/directors-message/` → **301** to `/about-maiam/`; `/directors-message` (no trailing slash) also
  **301**; following the redirect ends at **200**.
- `sitemap.xml`: **0** directors-message entries, 17 URLs total.
- `/css/directors-message.css` and `/js/directors-message.js` → **404** (assets gone).
- About MAIAM page: **0** occurrences of "Director" in the body text, **0** links to the old URL,
  0 broken images, 0 horizontal overflow, stylesheets loading normally. The layout now flows
  straight from the photo pair into the "Explore MAIAM" section with no gap.

**Note**
The portrait `about/director-sivadas-aniyan` (400×400) is now unused but was deliberately **left in
place** — both the catalog entry and the image file — so the section can be restored without
reprocessing. Remove it if the decision is permanent.

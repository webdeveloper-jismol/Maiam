# MAIAM – WordPress → ASP.NET Core MVC Migration Inventory

Source: https://maiam.co.in/ (WordPress, crawled 2026-10-14 from the live menu, `page-sitemap.xml`, `post-sitemap.xml` and every page's content).
Target: `E:\Maiamnew` (ASP.NET Core 8 MVC). Original URLs are preserved; retired URLs are 301-redirected (`Infrastructure/LegacyUrlMiddleware.cs`).

Status values: **Complete** · **Complete** · **Blocked** (needs owner input) · **Redirected** · **Excluded** (WordPress placeholder).

## 1. Original menu (order and labels preserved)

| # | Menu | Submenu | Original URL | New URL | Status |
|---|------|---------|--------------|---------|--------|
| 1 | Home | – | `/` | `/` | Complete |
| 2 | About Us (`#`) | Objectives | `/objectives/` | `/objectives/` | Complete |
| 2 | About Us | About MAIAM | `/about-maiam/` | `/about-maiam/` | Complete |
| 2 | About Us | Vision and Mission | `/vision-and-mission/` | `/vision-and-mission/` | Complete |
| 2 | About Us | Testimonials | `/testimonials/` | `/testimonials/` | Complete |
| 2 | About Us › Certifications (`#`) | NSDC | `/nsdc/` | `/nsdc/` | Complete |
| 2 | About Us › Certifications | ASDC, NCVET Affiliation Certificate | `/certificate-of-affiliation/` | `/certificate-of-affiliation/` | Complete |
| 3 | Courses (`#`) | Automotive Body Repair Technician | `/courses/` | `/courses/` | Complete |
| 3 | Courses | Advanced Welding Technician | `/advanced-welding-technician/` | `/advanced-welding-technician/` | Complete |
| 4 | Placements | – | `/our-placements/` | `/our-placements/` | Complete |
| 5 | Faculties | – | `/faculties/` | `/faculties/` | Complete |
| 6 | Our Facilities | – | `/our-facilities/` | `/our-facilities/` | Complete |
| 7 | Gallery | – | `/gallery/` | `/gallery/` | Complete |
| 8 | Contact Us | – | `/contact-us/` | `/contact-us/` | Complete |
| – | Header button "Register Now" | – | `/register-now/` | `/register-now/` | Complete (email delivery needs SMTP settings – docs/EMAIL_SETUP.md) |

## 2. Published pages not in the menu

| Original URL | Title | Content | Decision |
|--------------|-------|---------|----------|
| `/two-wheeler-repair-technician-course/` | Two Wheeler Repair Technician | 4-month course details | Rebuilt at the same URL, kept out of the menu (as on the original). Current availability must be confirmed. |
| `/welding-technician/` | Welding Technician | 1-month course, 5 welding modules | Rebuilt at the same URL, kept out of the menu. Overlaps the 3-month Advanced Welding course: confirm whether it is still offered. |
| `/directors-message/` | Directors Message | Message from Dr. Sivadas Aniyan T.S, Director | Rebuilt at the same URL; linked from About MAIAM. Confirm the message and director details are current. |
| `/who-we-are/` | Who We Are | Older "about" text | Content merged into About MAIAM (`#who-we-are`); URL 301 → `/about-maiam/#who-we-are`. |
| `/video-gallery/` | Video Gallery | 3 YouTube videos | Videos merged into Gallery (`#videos`); URL 301 → `/gallery/#videos`. |
| `/certifications/` | Certifications | Empty page | 301 → `/nsdc/`. |
| `/accreditation-certificate/` | Accreditation Certificate | Empty page | 301 → `/certificate-of-affiliation/`. |
| `/mandatory-public-disclosure/` | Mandatory Public Disclosure | **Empty page** | **Blocked** – no content to migrate. Returns 404 until MAIAM supplies the disclosure content. |
| `/home-2/` | HOME_old | Superseded home page (contains an outdated "50% scholarship only for girls" offer) | Excluded; 301 → `/`. |
| `/sample-page/`, `/sample-page-2/`, `/hello-world/`, `/hello-world-2/`, `/gallery1/`–`/gallery4/`, `/category/uncategorized/` | WordPress placeholders | Default WordPress text | Excluded; 301 → `/` (gallery posts → `/gallery/`). |

## 3. Shared content

| Item | Source | Migrated to |
|------|--------|-------------|
| Phone +91 7594810851, +91 90726 23103 | Header, footer, contact page | `SiteInfo` (header, footer, contact, CTA) |
| Email maiam@manappuramfoundation.org | Header, footer, contact page | `SiteInfo` |
| Address: Manappuram Agro Farms Painoor, Valapad, Thrissur, Pin 680567 | Footer, contact page | `SiteInfo` |
| Managed by: Manappuram Foundation (CSR Body of Manappuram Finance Ltd.), Ummaih Complex, Chandapady, Near Police Station Valapad, Call 1800 123 6866, crmhead@manappuramfoundation.org | Footer | Footer "Managed By" + home band |
| Social: Facebook `manappuramauto.org`, YouTube channel `UCy2WeIo9l8cL2EiHKbLxMMQ`, Instagram `maiam_institute`, X `MaAutomotive`, WhatsApp 917594810851 | Home, footer | Footer, CTA |
| Foundation social: Facebook, Instagram `manappuram_foundation`, X `theMAFDN`, YouTube | Footer | Footer |
| Google Maps embed (place "Manappuram institute of automotive skills (MAIAM)", 10.400595, 76.126489) | Home, contact | Contact page |
| Footer quick links (About Us, Courses, Gallery, Contact Us, Faculties, Our Vision) and Our Course list | Footer | Footer |
| Instagram feed (Smash Balloon plugin) | Home, gallery | Replaced by "Follow on Instagram" links (a live feed needs an Instagram API token). |

## 4. Media

* **149 original images** downloaded from `wp-content/uploads`, converted to responsive WebP (480/720/960/1600 or 640/960/1200/1920 widths) – 467 MB → 23 MB. Catalog: `Infrastructure/ImageCatalog.Generated.cs` (key, size, alt text). Each key maps back to its WordPress file (see `docs/IMAGE_SOURCES.md`).
* **Videos:** 2 self-hosted MP4s (`assets/videos/`), 10 YouTube videos (5 testimonials, 2 gallery, 3 video gallery) embedded with privacy-enhanced, click-to-load players.
* **Document:** `QP-Auto-Body-Repair-Technician-level-4.pdf` → `assets/documents/qp-automotive-body-repair-technician-level-4.pdf` (old URL 301-redirected).

### Images not migrated as MAIAM photographs

| Original file(s) | Reason |
|------------------|--------|
| `2024/03/*welding*`, `2024/03/cutting-metal-with-plasma-*` (welding pages) | Stock photographs (licence unknown) – replaced with original MAIAM welding photos. |
| `2024/07/young-mechanic-*`, `2024/07/Untitled-design-54/55.png` (two-wheeler page) | Stock photographs (licence unknown) – not used. |
| `2022/05/vision-scaled-1.jpg`, `mission-scaled-1.jpg` | Stock photographs – replaced with MAIAM photos. |
| `2022/05/00000.jpg`, `11.jpg` (chassis diagram), `Car-aerodynamics-*.jpg`, `Volvos-SPA-chassis-*.jpg`, `55.png` | Third-party graphics/diagrams in the old gallery, not MAIAM photographs. |
| `2022/05/4c5402f5-*.jpg`, `4d64e53c-*.jpg`, `WhatsApp-Image-2022-03-09-at-10.51.55-AM-4.jpeg` | Old "Admission started (2022-23 batch)" posters – outdated; publishing them would imply current admissions. |
| `2022/02/3.png` (ASDC Accreditation Certificate banner) | Shows "Valid upto: 30th March 2024" – **expired**. Not published; awaiting the renewed certificate. |
| `2022/02/gallery1-3.jpg`, `2024/04/1.jpg`, `2.jpg`, `maiam-1.png`, `2022/05/about-maiam-trial.png`, `dent-repair-1.jpg` | Low-resolution duplicates of photos migrated at higher resolution. |
| `2022/06/reg.png`, `ICONS1.jpg`, `ICON3.jpg`, `placemnt.jpg`, `scholarship.jpg`, `11.png`, `ic1.png`, `Untitled-design-7.png`, `ADHD_PDFfile_document.png` | Decorative icons/buttons – replaced by SVG icons and real buttons. |

## 5. Items reported for verification (not silently changed)

1. **NSDC Certificate of Partnership** (No. NSDC/2025-26/10166) shows *Valid Upto: 25-Oct-2026* – 11 days after this migration. Confirm renewal.
2. **ASDC accreditation** – the old home slider's Accreditation Certificate expired 30 Mar 2024; the ASDC *Certificate of Affiliation* (No. ASC/SDC/32/008/08) is valid to 16 May 2027. Confirm the "ASDC-accredited" wording used on the home page.
3. **"25 years experienced trainers"** (home) vs. 21 / 20 / 20 years on the Faculties page.
4. **"100% placement support / assistance"** – original wording kept; confirm it is still offered as stated.
5. **Scholarships** (10% BPL, 5% APL backward, ABR course only) – shown with a "confirm current availability" note.
6. **Instagram handle** – home page uses `maiam_institute` (active feed); footer used `maiam288`. The new site uses `maiam_institute`.
7. **Testimonial photo** – "Arjun" testimonial uses an image file named `ARUN-O-S`.
8. **Gallery grouping** – two images under "Drug Awareness Section" show a Mahindra bodyshop visit and a Toyota facility visit; they are grouped with those visits in the new gallery.
9. **Course page statements** – "Indian Automobile Industry is expected to employ 36 million people by 2026" and "By 2030, all the vehicles in the world will be electrified" are dated/unverifiable claims kept from the original.
10. **Two Wheeler Repair** and **Welding Technician** (1 month) courses are not in the original menu – confirm whether they are offered.
11. **Director's Message** – confirm Dr. Sivadas Aniyan T.S is the current director.

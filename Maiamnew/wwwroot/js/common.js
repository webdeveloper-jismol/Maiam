/* =========================================================================
   MAIAM – common.js
   Shared behaviour for every page: header state, mobile navigation,
   dropdown menus, scroll reveal, image lightbox and YouTube embeds.
   Page-specific behaviour lives in each page's own script.
   ========================================================================= */
(function () {
    "use strict";

    var desktopQuery = window.matchMedia("(min-width: 1200px)");
    var reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)");

    /* ---------- Header shadow on scroll ---------- */
    function initHeader() {
        var header = document.querySelector("[data-site-header]");
        if (!header) return;
        var update = function () {
            header.classList.toggle("is-scrolled", window.scrollY > 8);
        };
        // Initial check waits for the load event so it never forces an early layout.
        window.addEventListener("load", update, { once: true });
        window.addEventListener("scroll", update, { passive: true });
    }

    /* ---------- Dropdown menus ---------- */
    function closeDropdowns(scope, except) {
        (scope || document).querySelectorAll(".nav-toggle[aria-expanded='true']").forEach(function (toggle) {
            if (toggle !== except && !(except && toggle.parentElement.contains(except))) {
                toggle.setAttribute("aria-expanded", "false");
            }
        });
    }

    function initDropdowns() {
        var nav = document.querySelector("[data-primary-nav]");
        if (!nav) return;

        nav.querySelectorAll(".nav-toggle").forEach(function (toggle) {
            toggle.addEventListener("click", function () {
                var open = toggle.getAttribute("aria-expanded") === "true";
                // On desktop only one branch stays open; nested toggles keep their parent open.
                if (desktopQuery.matches) {
                    closeDropdowns(nav, toggle);
                }
                toggle.setAttribute("aria-expanded", open ? "false" : "true");
            });
        });

        // Escape closes the innermost open dropdown and returns focus to its toggle.
        nav.addEventListener("keydown", function (event) {
            if (event.key !== "Escape") return;
            var openToggles = nav.querySelectorAll(".nav-toggle[aria-expanded='true']");
            if (!openToggles.length) return;
            var last = openToggles[openToggles.length - 1];
            last.setAttribute("aria-expanded", "false");
            last.focus();
            event.stopPropagation();
        });

        // Click outside closes desktop dropdowns.
        document.addEventListener("click", function (event) {
            if (desktopQuery.matches && !nav.contains(event.target)) {
                closeDropdowns(nav);
            }
        });

        // Leaving a dropdown with the keyboard closes it on desktop.
        nav.addEventListener("focusout", function (event) {
            if (!desktopQuery.matches) return;
            var item = event.target.closest(".has-dropdown");
            if (item && !item.contains(event.relatedTarget)) {
                var toggle = item.querySelector(":scope > .nav-toggle");
                if (toggle) toggle.setAttribute("aria-expanded", "false");
            }
        });
    }

    /* ---------- Mobile navigation drawer ---------- */
    function initMobileNav() {
        var nav = document.querySelector("[data-primary-nav]");
        var openButton = document.querySelector("[data-nav-open]");
        var backdrop = document.querySelector(".nav-backdrop");
        if (!nav || !openButton) return;

        function open() {
            document.body.classList.add("nav-open");
            openButton.setAttribute("aria-expanded", "true");
            if (backdrop) backdrop.hidden = false;
            var first = nav.querySelector(".nav-close");
            if (first) first.focus();
        }

        function close(returnFocus) {
            if (!document.body.classList.contains("nav-open")) return;
            document.body.classList.remove("nav-open");
            openButton.setAttribute("aria-expanded", "false");
            if (backdrop) backdrop.hidden = true;
            if (returnFocus) openButton.focus();
        }

        openButton.addEventListener("click", open);
        document.querySelectorAll("[data-nav-close]").forEach(function (el) {
            el.addEventListener("click", function () { close(true); });
        });

        document.addEventListener("keydown", function (event) {
            if (event.key === "Escape" && document.body.classList.contains("nav-open")) {
                close(true);
            }
            // Keep keyboard focus inside the open drawer.
            if (event.key === "Tab" && document.body.classList.contains("nav-open") && !desktopQuery.matches) {
                var focusable = Array.prototype.filter.call(
                    nav.querySelectorAll("a[href], button:not([disabled])"),
                    function (el) { return el.offsetParent !== null; });
                if (!focusable.length) return;
                var first = focusable[0];
                var last = focusable[focusable.length - 1];
                if (event.shiftKey && document.activeElement === first) {
                    event.preventDefault();
                    last.focus();
                } else if (!event.shiftKey && document.activeElement === last) {
                    event.preventDefault();
                    first.focus();
                }
            }
        });

        // Switching to the desktop layout resets the drawer.
        desktopQuery.addEventListener("change", function (event) {
            if (event.matches) close(false);
            closeDropdowns(nav);
        });
    }

    /* ---------- Scroll reveal ---------- */
    function initReveal() {
        var items = document.querySelectorAll("[data-reveal]");
        if (!items.length || reducedMotion.matches || !("IntersectionObserver" in window)) return;

        // The first notification for each item tells us whether it starts on screen.
        // Items already visible are left alone; items below the fold (not yet painted
        // on screen) are hidden and revealed when scrolled into view. This avoids a
        // forced layout on load and never hides content the visitor can already see.
        var seen = new WeakSet();
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                var el = entry.target;
                if (!seen.has(el)) {
                    seen.add(el);
                    if (entry.boundingClientRect.top < window.innerHeight) {
                        observer.unobserve(el);
                    } else {
                        el.classList.add("reveal-pending");
                    }
                    return;
                }
                if (entry.isIntersecting) {
                    el.classList.add("reveal-in");
                    el.classList.remove("reveal-pending");
                    observer.unobserve(el);
                }
            });
        }, { rootMargin: "0px 0px -8% 0px", threshold: 0.08 });

        items.forEach(function (item) { observer.observe(item); });
    }

    /* ---------- Lightbox ----------
       Links with data-lightbox="group" open their href in an accessible dialog.
       Without JavaScript the link simply opens the full-size image. */
    function initLightbox() {
        if (typeof HTMLDialogElement === "undefined") return;
        var api = null;

        // The viewer is built on first use, keeping page start-up work minimal.
        document.addEventListener("click", function (event) {
            var link = event.target.closest && event.target.closest("a[data-lightbox]");
            if (!link || event.defaultPrevented || event.button !== 0 || event.ctrlKey || event.metaKey || event.shiftKey) return;
            event.preventDefault();
            if (!api) api = createLightbox();
            api.open(link);
        });
    }

    function createLightbox() {
        var dialog = document.createElement("dialog");
        dialog.className = "lightbox";
        dialog.setAttribute("aria-label", "Image viewer");
        dialog.innerHTML =
            '<div class="lightbox__stage">' +
            '  <div class="lightbox__bar"><span class="lightbox__count" aria-live="polite"></span>' +
            '    <button type="button" class="lightbox__btn" data-lb-close aria-label="Close image viewer"><svg class="icon" aria-hidden="true"><use href="#i-close"></use></svg></button></div>' +
            '  <figure class="lightbox__figure">' +
            '    <img alt="" />' +
            '    <button type="button" class="lightbox__btn lightbox__nav lightbox__nav--prev" data-lb-prev aria-label="Previous image"><svg class="icon" aria-hidden="true"><use href="#i-chevron-left"></use></svg></button>' +
            '    <button type="button" class="lightbox__btn lightbox__nav lightbox__nav--next" data-lb-next aria-label="Next image"><svg class="icon" aria-hidden="true"><use href="#i-chevron-right"></use></svg></button>' +
            '  </figure>' +
            '  <p class="lightbox__caption"></p>' +
            '</div>';
        document.body.appendChild(dialog);

        var img = dialog.querySelector("img");
        var caption = dialog.querySelector(".lightbox__caption");
        var count = dialog.querySelector(".lightbox__count");
        var prevBtn = dialog.querySelector("[data-lb-prev]");
        var nextBtn = dialog.querySelector("[data-lb-next]");
        var group = [];
        var index = 0;
        var opener = null;

        function show(i) {
            index = (i + group.length) % group.length;
            var link = group[index];
            var thumb = link.querySelector("img");
            img.src = link.getAttribute("href");
            img.alt = thumb ? thumb.alt : "";
            caption.textContent = link.getAttribute("data-caption") || (thumb ? thumb.alt : "");
            count.textContent = group.length > 1 ? (index + 1) + " / " + group.length : "";
            prevBtn.hidden = nextBtn.hidden = group.length < 2;
        }

        function open(link) {
            var name = link.getAttribute("data-lightbox");
            group = Array.prototype.filter.call(document.querySelectorAll("a[data-lightbox]"), function (a) {
                return a.getAttribute("data-lightbox") === name;
            });
            opener = link;
            show(group.indexOf(link));
            dialog.showModal();
            document.body.classList.add("scroll-locked");
        }

        dialog.addEventListener("close", function () {
            document.body.classList.remove("scroll-locked");
            img.removeAttribute("src");
            if (opener) opener.focus();
        });

        dialog.querySelector("[data-lb-close]").addEventListener("click", function () { dialog.close(); });
        prevBtn.addEventListener("click", function () { show(index - 1); });
        nextBtn.addEventListener("click", function () { show(index + 1); });

        dialog.addEventListener("keydown", function (event) {
            if (event.key === "ArrowLeft") show(index - 1);
            if (event.key === "ArrowRight") show(index + 1);
        });

        // Click on the dark backdrop (outside the image) closes the viewer.
        dialog.addEventListener("click", function (event) {
            if (event.target === dialog || event.target.classList.contains("lightbox__figure")) dialog.close();
        });

        // Swipe left/right on touch screens.
        var startX = null;
        dialog.addEventListener("touchstart", function (event) { startX = event.touches[0].clientX; }, { passive: true });
        dialog.addEventListener("touchend", function (event) {
            if (startX === null) return;
            var dx = event.changedTouches[0].clientX - startX;
            if (Math.abs(dx) > 50) show(index + (dx < 0 ? 1 : -1));
            startX = null;
        }, { passive: true });

        return { open: open };
    }

    /* ---------- YouTube embeds (privacy-enhanced, loaded on demand) ---------- */
    function initYouTube() {
        document.querySelectorAll("a[data-youtube]").forEach(function (link) {
            link.addEventListener("click", function (event) {
                event.preventDefault();
                var id = link.getAttribute("data-youtube");
                var frame = document.createElement("div");
                frame.className = "video-embed__frame";
                var iframe = document.createElement("iframe");
                iframe.src = "https://www.youtube-nocookie.com/embed/" + encodeURIComponent(id) + "?autoplay=1&rel=0";
                iframe.title = link.getAttribute("data-title") || "YouTube video";
                iframe.allow = "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture";
                iframe.allowFullscreen = true;
                frame.appendChild(iframe);
                link.replaceWith(frame);
                iframe.focus();
            });
        });
    }

    /* ---------- Copy-to-clipboard buttons ----------
       <button data-copy="#selector" hidden>Copy</button> copies the target's text.
       Buttons stay hidden when the Clipboard API is unavailable. */
    function initCopyButtons() {
        if (!navigator.clipboard) return;
        document.querySelectorAll("button[data-copy]").forEach(function (button) {
            var target = document.querySelector(button.getAttribute("data-copy"));
            if (!target) return;
            var label = button.textContent;
            var status = document.querySelector("[data-copy-status]");
            button.hidden = false;
            button.addEventListener("click", function () {
                navigator.clipboard.writeText(target.textContent.trim()).then(function () {
                    button.textContent = "Copied";
                    if (status) status.textContent = "Copied to clipboard.";
                    setTimeout(function () { button.textContent = label; }, 2000);
                });
            });
        });
    }

    function init() {
        initHeader();
        initDropdowns();
        initMobileNav();
        initReveal();
        initLightbox();
        initYouTube();
        initCopyButtons();
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", init);
    } else {
        init();
    }
})();

/* MAIAM – gallery.js
   1. Large albums show their first photos with a "Show all" button
      (without JavaScript every photo is shown).
   2. The sticky album navigation highlights the album currently in view.
   The lightbox itself is shared and provided by common.js. */
(function () {
    "use strict";

    /* ---------- Collapsible albums ---------- */
    document.querySelectorAll("[data-album]").forEach(function (album) {
        var grid = album.querySelector("[data-album-grid]");
        var button = album.querySelector("[data-album-more]");
        var limit = grid ? parseInt(grid.getAttribute("data-preview"), 10) : 0;
        if (!grid || !button || !limit) return;

        var items = grid.querySelectorAll("li");
        items.forEach(function (item, index) {
            if (index >= limit) item.hidden = true;
        });
        button.hidden = false;
        button.setAttribute("aria-expanded", "false");

        button.addEventListener("click", function () {
            items.forEach(function (item) { item.hidden = false; });
            button.setAttribute("aria-expanded", "true");
            button.hidden = true;
            // Move focus to the first newly shown photo.
            var next = items[limit] && items[limit].querySelector("a");
            if (next) next.focus();
        });
    });

    /* ---------- Album navigation state ---------- */
    var nav = document.querySelector("[data-album-nav]");
    if (!nav || !("IntersectionObserver" in window)) return;

    var links = nav.querySelectorAll("a[href^='#']");
    var current = null;

    function setCurrent(id) {
        if (id === current) return;
        current = id;
        links.forEach(function (link) {
            var active = link.getAttribute("href") === "#" + id;
            if (active) {
                link.setAttribute("aria-current", "true");
                // keep the active chip visible in the horizontally scrolling bar
                var list = link.closest("ul");
                var left = link.offsetLeft - list.clientWidth / 2 + link.clientWidth / 2;
                list.scrollTo({ left: left, behavior: "smooth" });
            } else {
                link.removeAttribute("aria-current");
            }
        });
    }

    var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) setCurrent(entry.target.id);
        });
    }, { rootMargin: "-30% 0px -60% 0px" });

    document.querySelectorAll("[data-album], #videos").forEach(function (section) {
        observer.observe(section);
    });
})();

/* MAIAM – our-facilities.js
   Category filter for the facility photo tour (All / Workshop / Learning spaces /
   Campus). Without JavaScript the filter stays hidden and every photo is shown.
   The lightbox only steps through the photos that are currently visible. */
(function () {
    "use strict";

    var filter = document.querySelector("[data-facility-filter]");
    var gallery = document.querySelector("[data-facility-gallery]");
    var status = document.querySelector("[data-facility-status]");
    if (!filter || !gallery) return;

    var buttons = filter.querySelectorAll("button[data-filter]");
    var items = gallery.querySelectorAll("li[data-category]");
    filter.hidden = false;

    function apply(category) {
        var shown = 0;
        items.forEach(function (item) {
            var match = category === "all" || item.getAttribute("data-category") === category;
            item.hidden = !match;
            // Hidden photos leave the lightbox group so next/previous skips them.
            var link = item.querySelector("a[data-lightbox], a[data-lightbox-off]");
            if (link) {
                if (match) {
                    link.setAttribute("data-lightbox", "facilities");
                    link.removeAttribute("data-lightbox-off");
                } else {
                    link.setAttribute("data-lightbox-off", "facilities");
                    link.removeAttribute("data-lightbox");
                }
            }
            if (match) shown++;
        });
        buttons.forEach(function (button) {
            button.setAttribute("aria-pressed", button.getAttribute("data-filter") === category ? "true" : "false");
        });
        if (status) status.textContent = shown + " photos shown";
    }

    buttons.forEach(function (button) {
        button.addEventListener("click", function () {
            apply(button.getAttribute("data-filter"));
        });
    });
})();

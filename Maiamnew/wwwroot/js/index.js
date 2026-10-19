/* =========================================================================
   MAIAM – index.js (Home page)
   Testimonial slider controls: scroll-snap track with previous/next buttons.
   The track stays fully usable by touch, trackpad and keyboard without JS.
   Button states come from an IntersectionObserver on the first and last cards,
   so nothing is measured during page start-up.
   ========================================================================= */
(function () {
    "use strict";

    var track = document.querySelector("[data-slider-track]");
    var prev = document.querySelector("[data-slider-prev]");
    var next = document.querySelector("[data-slider-next]");
    if (!track || !prev || !next) return;

    var items = track.querySelectorAll("li");
    if (!items.length) return;

    function scrollByCard(direction) {
        var gap = parseFloat(getComputedStyle(track).columnGap) || 0;
        var step = items[0].getBoundingClientRect().width + gap;
        track.scrollBy({ left: direction * step, behavior: "smooth" });
    }

    prev.addEventListener("click", function () { scrollByCard(-1); });
    next.addEventListener("click", function () { scrollByCard(1); });

    track.addEventListener("keydown", function (event) {
        if (event.key === "ArrowRight" || event.key === "ArrowLeft") {
            event.preventDefault();
            scrollByCard(event.key === "ArrowRight" ? 1 : -1);
        }
    });

    if (!("IntersectionObserver" in window)) return;

    var first = items[0];
    var last = items[items.length - 1];
    var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            var fullyVisible = entry.intersectionRatio > 0.95;
            if (entry.target === first) prev.disabled = fullyVisible;
            if (entry.target === last) next.disabled = fullyVisible;
        });
    }, { root: track, threshold: [0, 0.95, 1] });

    observer.observe(first);
    observer.observe(last);
})();

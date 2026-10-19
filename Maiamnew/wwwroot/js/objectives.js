/* MAIAM – objectives.js
   Highlights the "On this page" link for the objectives group currently in view. */
(function () {
    "use strict";

    var index = document.querySelector("[data-objectives-index]");
    var groups = document.querySelectorAll("[data-objective-group]");
    if (!index || !groups.length || !("IntersectionObserver" in window)) return;

    var links = index.querySelectorAll("a");

    function setCurrent(id) {
        links.forEach(function (link) {
            if (link.getAttribute("href") === "#" + id) {
                link.setAttribute("aria-current", "true");
            } else {
                link.removeAttribute("aria-current");
            }
        });
    }

    var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) setCurrent(entry.target.id);
        });
    }, { rootMargin: "-35% 0px -55% 0px" });

    groups.forEach(function (group) { observer.observe(group); });
})();

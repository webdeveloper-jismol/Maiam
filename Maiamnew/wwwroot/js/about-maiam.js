/* MAIAM – about-maiam.js
   Visitors redirected from the retired /who-we-are/ page land on #who-we-are;
   briefly highlight that section so they can see where the content moved. */
(function () {
    "use strict";

    if (window.location.hash !== "#who-we-are") return;

    var section = document.getElementById("who-we-are");
    if (!section || window.matchMedia("(prefers-reduced-motion: reduce)").matches) return;

    section.classList.add("is-highlighted");
    section.addEventListener("animationend", function () {
        section.classList.remove("is-highlighted");
    }, { once: true });
})();

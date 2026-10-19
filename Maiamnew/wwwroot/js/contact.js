/* MAIAM – contact.js
   Loads the Google Maps embed only when the visitor asks for it (faster page,
   no third-party requests until needed). Without JavaScript the overlay keeps
   its "Open in Google Maps" link. */
(function () {
    "use strict";

    var map = document.querySelector("[data-map]");
    if (!map) return;

    var button = map.querySelector("[data-map-load]");
    var fallback = map.querySelector("[data-map-fallback]");
    if (!button) return;

    button.hidden = false;
    if (fallback) fallback.hidden = true;

    button.addEventListener("click", function () {
        var iframe = document.createElement("iframe");
        iframe.src = map.getAttribute("data-map-src");
        iframe.title = "Map showing Manappuram Institute of Automotive Skills (MAIAM), Valapad, Thrissur";
        iframe.loading = "lazy";
        iframe.referrerPolicy = "no-referrer-when-downgrade";
        iframe.allowFullscreen = true;
        map.replaceChildren(iframe);
        iframe.focus();
    });
})();

/* MAIAM – automotive-body-repair.js
   Keeps one course film playing at a time: playing the self-hosted film stops
   an open YouTube film, and opening a YouTube film pauses the self-hosted one. */
(function () {
    "use strict";

    var film = document.querySelector(".abr-videos video");
    if (!film) return;

    function stopYouTube() {
        document.querySelectorAll(".abr-videos .video-embed__frame iframe").forEach(function (iframe) {
            iframe.src = iframe.src.replace("autoplay=1", "autoplay=0");
        });
    }

    film.addEventListener("play", stopYouTube);

    document.addEventListener("click", function (event) {
        var trigger = event.target.closest && event.target.closest(".abr-videos a[data-youtube]");
        if (!trigger) return;
        if (!film.paused) film.pause();
        stopYouTube();
    }, true);
})();

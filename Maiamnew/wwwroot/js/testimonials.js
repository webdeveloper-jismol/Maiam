/* MAIAM – testimonials.js
   Only one testimonial film plays at a time: starting the self-hosted video
   stops any YouTube film that is playing, and opening a YouTube film pauses
   the self-hosted video. */
(function () {
    "use strict";

    var nativeVideo = document.querySelector(".native-video video");

    function stopYouTube() {
        document.querySelectorAll(".video-embed__frame iframe").forEach(function (iframe) {
            // Reloading without autoplay stops playback while keeping the player in place.
            iframe.src = iframe.src.replace("autoplay=1", "autoplay=0");
        });
    }

    if (nativeVideo) {
        nativeVideo.addEventListener("play", stopYouTube);
    }

    document.addEventListener("click", function (event) {
        var trigger = event.target.closest && event.target.closest("a[data-youtube]");
        if (!trigger) return;
        if (nativeVideo && !nativeVideo.paused) nativeVideo.pause();
        stopYouTube();
    }, true);
})();

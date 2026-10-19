/* MAIAM – error.js: offers a "Go back" button when the visitor arrived from another page. */
(function () {
    "use strict";

    var back = document.querySelector("[data-history-back]");
    if (!back || window.history.length < 2 || !document.referrer) return;

    back.hidden = false;
    back.addEventListener("click", function () {
        window.history.back();
    });
})();

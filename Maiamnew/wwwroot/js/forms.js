/* MAIAM – forms.js
   Shared behaviour for the site's enquiry forms (Register Now, Contact Us).
   Any form marked with data-form gets:
   - Client-side validation (jQuery Validation Unobtrusive) loaded on first
     interaction with the form, keeping the initial page load light.
   - A character counter for any field described by a [data-char-count] hint.
   - Double-submit protection with a "Sending…" button state.
   - Validation state exposed to assistive technology, and focus management.
   The server always validates the submission (DataAnnotations) as well. */
(function () {
    "use strict";

    var forms = document.querySelectorAll("[data-form]");
    if (!forms.length) return;

    // Validation scripts are listed on this script tag so they can be loaded lazily.
    var self = document.currentScript || document.querySelector("script[data-validation-scripts]");
    var scripts = self ? (self.getAttribute("data-validation-scripts") || "").split(" ").filter(Boolean) : [];
    var validationLoader = null;

    function loadScript(src) {
        return new Promise(function (resolve, reject) {
            var el = document.createElement("script");
            el.src = src;
            el.onload = resolve;
            el.onerror = reject;
            document.body.appendChild(el);
        });
    }

    // Load the validation bundle once, however many forms are on the page.
    function loadValidation() {
        if (!validationLoader) {
            validationLoader = scripts.reduce(function (chain, src) {
                return chain.then(function () { return loadScript(src); });
            }, Promise.resolve());
        }
        return validationLoader;
    }

    function enhance(form) {
        var validationRequested = false;

        function enableClientValidation() {
            if (validationRequested || !scripts.length) return;
            validationRequested = true;
            loadValidation().then(function () {
                var $ = window.jQuery;
                if ($ && $.validator && $.validator.unobtrusive) {
                    $.validator.unobtrusive.parse(form);
                }
            }).catch(function () {
                // Client-side validation is optional; the server still validates.
            });
        }

        form.addEventListener("focusin", enableClientValidation, { once: true });
        form.addEventListener("pointerdown", enableClientValidation, { once: true });

        // Character counters: the hint carries data-char-count, and the field it
        // belongs to points at the hint through aria-describedby.
        form.querySelectorAll("[data-char-count]").forEach(function (counter) {
            if (!counter.id) return;
            var field = form.querySelector("[aria-describedby~='" + counter.id + "']");
            if (!field) return;
            var max = parseInt(counter.getAttribute("data-char-count"), 10);
            if (!max) return;
            var update = function () {
                counter.textContent = (max - field.value.length) + " characters remaining";
            };
            field.addEventListener("input", update);
            update();
        });

        // Double-submit protection (respects client-side validation when present)
        var button = form.querySelector("[data-submit]");
        form.addEventListener("submit", function (event) {
            var $ = window.jQuery;
            if ($ && $(form).valid && !$(form).valid()) return;
            if (form.getAttribute("data-submitting") === "true") {
                event.preventDefault();
                return;
            }
            form.setAttribute("data-submitting", "true");
            if (button) {
                button.disabled = true;
                button.firstChild.textContent = "Sending… ";
            }
        });

        // Accessibility: expose validation state (client or server) to assistive technology.
        // Each invalid field gets aria-invalid and is linked to its error message.
        function syncValidationState() {
            form.querySelectorAll("[data-valmsg-for]").forEach(function (message) {
                var field = form.querySelector("[name='" + message.getAttribute("data-valmsg-for") + "']");
                if (!field) return;
                if (!message.id) message.id = field.id + "-error";
                var describedBy = (field.getAttribute("aria-describedby") || "").split(" ").filter(Boolean);
                var hasError = field.classList.contains("input-validation-error") && message.textContent.trim() !== "";
                if (hasError) {
                    field.setAttribute("aria-invalid", "true");
                    if (describedBy.indexOf(message.id) === -1) describedBy.push(message.id);
                } else {
                    field.removeAttribute("aria-invalid");
                    describedBy = describedBy.filter(function (id) { return id !== message.id; });
                }
                if (describedBy.length) field.setAttribute("aria-describedby", describedBy.join(" "));
                else field.removeAttribute("aria-describedby");
            });
        }

        ["submit", "focusout", "input", "change"].forEach(function (type) {
            form.addEventListener(type, function () { setTimeout(syncValidationState, 0); });
        });
        syncValidationState();

        // Move focus to the first invalid field after a failed client-side check.
        form.addEventListener("submit", function () {
            setTimeout(function () {
                var firstInvalid = form.querySelector("[aria-invalid='true']");
                if (firstInvalid && form.getAttribute("data-submitting") !== "true") firstInvalid.focus();
            }, 0);
        });

        // After a server response, bring the result message into focus.
        var statusSelector = form.getAttribute("data-form-status");
        var status = statusSelector ? document.querySelector(statusSelector) : null;
        if (status && status.children.length) {
            status.focus();
        }
    }

    forms.forEach(enhance);
})();

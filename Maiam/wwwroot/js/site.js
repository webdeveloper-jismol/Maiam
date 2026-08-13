// Site header, mobile navigation, and shared UI behavior
(function () {
    var header = document.getElementById('siteHeader');
    var mobileMenu = document.getElementById('mobileMenu');
    var mobileToggle = document.getElementById('mobileMenuToggle');
    var mobileClose = document.getElementById('mobileMenuClose');

    // Header background swap on scroll
    function updateHeaderScrollState() {
        if (!header) return;
        if (window.scrollY > 12) {
            header.classList.add('is-scrolled');
        } else {
            header.classList.remove('is-scrolled');
        }
    }
    updateHeaderScrollState();
    window.addEventListener('scroll', updateHeaderScrollState, { passive: true });

    // Desktop dropdowns (About / Courses) — click to toggle, keyboard accessible
    document.querySelectorAll('.site-header__dropdown-toggle').forEach(function (toggle) {
        toggle.addEventListener('click', function (e) {
            e.stopPropagation();
            var li = toggle.closest('li');
            var wasOpen = li.classList.contains('is-open');
            document.querySelectorAll('.site-header__nav > li.is-open').forEach(function (openLi) {
                openLi.classList.remove('is-open');
            });
            if (!wasOpen) {
                li.classList.add('is-open');
                toggle.setAttribute('aria-expanded', 'true');
            } else {
                toggle.setAttribute('aria-expanded', 'false');
            }
        });
    });
    document.addEventListener('click', function () {
        document.querySelectorAll('.site-header__nav > li.is-open').forEach(function (li) {
            li.classList.remove('is-open');
        });
    });

    // Mobile menu open/close
    function openMobileMenu() {
        if (!mobileMenu) return;
        mobileMenu.classList.add('is-open');
        document.body.style.overflow = 'hidden';
        mobileToggle && mobileToggle.setAttribute('aria-expanded', 'true');
    }
    function closeMobileMenu() {
        if (!mobileMenu) return;
        mobileMenu.classList.remove('is-open');
        document.body.style.overflow = '';
        mobileToggle && mobileToggle.setAttribute('aria-expanded', 'false');
    }
    mobileToggle && mobileToggle.addEventListener('click', openMobileMenu);
    mobileClose && mobileClose.addEventListener('click', closeMobileMenu);

    // Mobile submenu accordions
    document.querySelectorAll('.mobile-menu__group-toggle').forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            var targetId = toggle.getAttribute('data-target');
            var submenu = document.getElementById(targetId);
            if (!submenu) return;
            var isOpen = submenu.classList.contains('is-open');
            submenu.classList.toggle('is-open', !isOpen);
            toggle.setAttribute('aria-expanded', String(!isOpen));
        });
    });

    // Close mobile menu when a nav link is clicked
    mobileMenu && mobileMenu.querySelectorAll('a').forEach(function (link) {
        link.addEventListener('click', closeMobileMenu);
    });

    // Animated stat counters (elements with [data-counter-target])
    var counters = document.querySelectorAll('[data-counter-target]');
    if (counters.length && 'IntersectionObserver' in window) {
        var counterObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (!entry.isIntersecting) return;
                animateCounter(entry.target);
                counterObserver.unobserve(entry.target);
            });
        }, { threshold: 0.4 });
        counters.forEach(function (el) { counterObserver.observe(el); });
    }

    function animateCounter(el) {
        var target = parseInt(el.getAttribute('data-counter-target'), 10) || 0;
        var suffix = el.getAttribute('data-counter-suffix') || '';
        var duration = 1400;
        var startTime = null;

        function step(timestamp) {
            if (!startTime) startTime = timestamp;
            var progress = Math.min((timestamp - startTime) / duration, 1);
            var value = Math.floor(progress * target);
            el.textContent = value + suffix;
            if (progress < 1) {
                requestAnimationFrame(step);
            } else {
                el.textContent = target + suffix;
            }
        }
        requestAnimationFrame(step);
    }

    // Scroll-reveal for elements with [data-reveal]
    var revealEls = document.querySelectorAll('[data-reveal]');
    if (revealEls.length) {
        if ('IntersectionObserver' in window) {
            var revealObserver = new IntersectionObserver(function (entries) {
                entries.forEach(function (entry) {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('is-revealed');
                        revealObserver.unobserve(entry.target);
                    }
                });
            }, { threshold: 0.15 });
            revealEls.forEach(function (el) { revealObserver.observe(el); });
        } else {
            // No IntersectionObserver support: reveal immediately rather than leaving elements at opacity:0 forever.
            revealEls.forEach(function (el) { el.classList.add('is-revealed'); });
        }

        // Safety net: an element whose own bounding box is much taller than the viewport (e.g. a very
        // long content section) may never satisfy the observer's visibility threshold. Force-reveal
        // anything still hidden after a short delay so content can never get stuck invisible.
        setTimeout(function () {
            document.querySelectorAll('[data-reveal]:not(.is-revealed)').forEach(function (el) {
                el.classList.add('is-revealed');
            });
        }, 1500);
    }
})();

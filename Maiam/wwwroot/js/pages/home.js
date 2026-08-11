// Home page: hero carousel and testimonial carousel
(function () {
    // Hero carousel
    var hero = document.getElementById('homeHero');
    if (hero) {
        var slides = hero.querySelectorAll('.hero__slide');
        var counter = document.getElementById('heroCounterCurrent');
        var prevBtn = document.getElementById('heroPrev');
        var nextBtn = document.getElementById('heroNext');
        var current = 0;
        var autoplayMs = parseInt(hero.getAttribute('data-autoplay'), 10) || 5000;
        var timer = null;

        function showSlide(index) {
            slides[current].classList.remove('is-active');
            current = (index + slides.length) % slides.length;
            slides[current].classList.add('is-active');
            if (counter) counter.textContent = String(current + 1).padStart(2, '0');
        }

        function next() { showSlide(current + 1); }
        function prev() { showSlide(current - 1); }

        function restartAutoplay() {
            if (timer) clearInterval(timer);
            timer = setInterval(next, autoplayMs);
        }

        nextBtn && nextBtn.addEventListener('click', function () { next(); restartAutoplay(); });
        prevBtn && prevBtn.addEventListener('click', function () { prev(); restartAutoplay(); });

        if (slides.length > 1) {
            restartAutoplay();
        }
    }

    // Testimonial carousel
    var track = document.querySelector('.testimonial-carousel__track');
    if (track) {
        var tSlides = track.querySelectorAll('.testimonial-carousel__slide');
        var dots = document.querySelectorAll('.testimonial-carousel__dot');
        var tCurrent = 0;
        var tTimer = null;

        function showTestimonial(index) {
            tSlides[tCurrent].classList.remove('is-active');
            dots[tCurrent] && dots[tCurrent].classList.remove('is-active');
            tCurrent = (index + tSlides.length) % tSlides.length;
            tSlides[tCurrent].classList.add('is-active');
            dots[tCurrent] && dots[tCurrent].classList.add('is-active');
        }

        dots.forEach(function (dot, i) {
            dot.addEventListener('click', function () {
                showTestimonial(i);
                restartTestimonialAutoplay();
            });
        });

        function restartTestimonialAutoplay() {
            if (tTimer) clearInterval(tTimer);
            tTimer = setInterval(function () { showTestimonial(tCurrent + 1); }, 6000);
        }

        if (tSlides.length > 1) {
            restartTestimonialAutoplay();
        }
    }
})();

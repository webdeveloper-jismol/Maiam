// MAIAM Gallery lightbox: click any image to preview, navigate with prev/next, close on Esc/backdrop
(function () {
    var triggers = Array.from(document.querySelectorAll('.gallery-section__item'));
    if (!triggers.length) return;

    var lightbox = document.getElementById('galleryLightbox');
    var lightboxImage = document.getElementById('lightboxImage');
    var closeBtn = document.getElementById('lightboxClose');
    var prevBtn = document.getElementById('lightboxPrev');
    var nextBtn = document.getElementById('lightboxNext');

    var items = triggers.map(function (el) {
        return { src: el.getAttribute('data-lightbox-src'), alt: el.getAttribute('data-lightbox-alt') };
    });
    var currentIndex = 0;

    function show(index) {
        currentIndex = (index + items.length) % items.length;
        var item = items[currentIndex];
        lightboxImage.src = item.src;
        lightboxImage.alt = item.alt;
    }

    function open(index) {
        show(index);
        lightbox.classList.add('is-open');
        document.body.style.overflow = 'hidden';
    }

    function close() {
        lightbox.classList.remove('is-open');
        document.body.style.overflow = '';
        lightboxImage.src = '';
    }

    triggers.forEach(function (el, index) {
        el.addEventListener('click', function () { open(index); });
    });

    closeBtn.addEventListener('click', close);
    prevBtn.addEventListener('click', function () { show(currentIndex - 1); });
    nextBtn.addEventListener('click', function () { show(currentIndex + 1); });

    // Close when clicking the dark backdrop (not the image or arrows)
    lightbox.addEventListener('click', function (e) {
        if (e.target === lightbox) close();
    });

    document.addEventListener('keydown', function (e) {
        if (!lightbox.classList.contains('is-open')) return;
        if (e.key === 'Escape') close();
        if (e.key === 'ArrowLeft') show(currentIndex - 1);
        if (e.key === 'ArrowRight') show(currentIndex + 1);
    });
})();

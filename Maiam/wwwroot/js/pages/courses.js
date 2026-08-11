// Course detail tabs: Overview / Curriculum / Career Path or Batches
(function () {
    var tabButtons = document.querySelectorAll('.course-tabs__btn');
    if (!tabButtons.length) return;

    tabButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            var target = btn.getAttribute('data-tab-target');

            tabButtons.forEach(function (b) {
                b.classList.remove('is-active');
                b.setAttribute('aria-selected', 'false');
            });
            btn.classList.add('is-active');
            btn.setAttribute('aria-selected', 'true');

            document.querySelectorAll('.course-tab-panel').forEach(function (panel) {
                panel.classList.remove('is-active');
            });
            var panel = document.getElementById('tab-' + target);
            if (panel) panel.classList.add('is-active');
        });
    });
})();

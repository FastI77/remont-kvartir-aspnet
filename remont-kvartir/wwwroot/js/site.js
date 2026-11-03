document.addEventListener('DOMContentLoaded', function () {

    // Мобильное меню (бургер)
    const burgerBtn = document.getElementById('burgerBtn');
    const mainNav = document.getElementById('mainNav');

    if (burgerBtn && mainNav) {
        burgerBtn.addEventListener('click', function () {
            mainNav.classList.toggle('active');
        });
    }

    // Слайдер на главной
    let slideIndex = 0;
    let slideInterval;

    window.showSlides = function () {
        const slides = document.querySelectorAll(".slide");
        const dots = document.querySelectorAll(".dot");

        if (slides.length === 0) return;

        slides.forEach(slide => slide.classList.remove("active"));
        dots.forEach(dot => dot.classList.remove("active"));

        slideIndex++;
        if (slideIndex > slides.length) { slideIndex = 1; }

        slides[slideIndex - 1].classList.add("active");
        dots[slideIndex - 1].classList.add("active");

        slideInterval = setTimeout(showSlides, 5000);
    }

    window.currentSlide = function (n) {
        clearTimeout(slideInterval);
        slideIndex = n;
        const slides = document.querySelectorAll(".slide");
        const dots = document.querySelectorAll(".dot");

        slides.forEach(slide => slide.classList.remove("active"));
        dots.forEach(dot => dot.classList.remove("active"));

        slides[n].classList.add("active");
        dots[n].classList.add("active");

        slideInterval = setTimeout(showSlides, 5000);
    }

    if (document.querySelector('.slide')) {
        slideInterval = setTimeout(showSlides, 5000);
    }
});
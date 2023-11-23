window.initializeSwiper = () => {
    var grid = document.querySelector('.products');
    var masonry = new Masonry(grid, {
        itemSelector: '.product-item',
        columnWidth: '.product-item',
        percentPosition: true
    });
};
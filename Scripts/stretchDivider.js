window.addEventListener('load', stretchDecorativeDivider);
window.addEventListener('resize', stretchDecorativeDivider);

function stretchDecorativeDivider() {
    const leftCol = document.querySelector('.vendor-left');
    const middleCol = document.querySelector('.vendor-middle');
    const rightCol = document.querySelector('.vendor-right');
    const divider = document.querySelector('.decorative-divider');

    if (leftCol && middleCol && rightCol && divider) {
        const leftHeight = leftCol.offsetHeight;
        const middleHeight = middleCol.offsetHeight;
        const rightHeight = rightCol.offsetHeight;

        const maxHeight = Math.max(leftHeight, middleHeight, rightHeight);
        divider.style.height = `${maxHeight}px`;
    }
}
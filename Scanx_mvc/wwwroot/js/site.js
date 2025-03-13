
// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    let progress = document.querySelector(".progress");
    let uploaded = 4; // Example
    let total = 5; // Example
    progress.style.width = (uploaded / total) * 100 + "%";
});

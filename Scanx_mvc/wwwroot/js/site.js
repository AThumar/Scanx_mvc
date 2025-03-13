function formatText(command) {
    document.execCommand(command, false, null);
}

// Add event listeners to buttons
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("boldBtn").addEventListener("click", function () {
        formatText("bold");
    });

    document.getElementById("italicBtn").addEventListener("click", function () {
        formatText("italic");
    });

    document.getElementById("underlineBtn").addEventListener("click", function () {
        formatText("underline");
    });
});

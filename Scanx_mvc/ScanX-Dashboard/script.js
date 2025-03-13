$(document).ready(function () {
    let uploadedFiles = [];

    // Open modal on Upload PDF button click
    $("#uploadBtn").click(function () {
        $("#uploadModal").fadeIn();
    });

    // Close modal
    $("#closeModal").click(function () {
        $("#uploadModal").fadeOut();
    });

    // Upload file
    $("#uploadFile").click(function () {
        if (uploadedFiles.length >= 5) {
            alert("Upgrade to upload more than 5 PDFs.");
            return;
        }

        let fileName = $("#fileName").val().trim();
        if (fileName === "") {
            alert("Please enter a file name.");
            return;
        }

        uploadedFiles.push(fileName);
        updatePDFList();

        $("#uploadModal").fadeOut();
        $("#fileName").val(""); // Clear input
    });

    // Update UI with uploaded PDFs
    function updatePDFList() {
        let pdfList = $("#pdfList");
        pdfList.html(""); // Clear previous entries

        uploadedFiles.forEach((file, index) => {
            let pdfCard = `<div class="pdf-card">
                <img src="pdf-icon.png" width="50">
                <p>${file}</p>
            </div>`;
            pdfList.append(pdfCard);
        });

        $("#uploadedCount").text(uploadedFiles.length);
        $("#uploadProgress").css("width", (uploadedFiles.length / 5) * 100 + "%");
    }
});


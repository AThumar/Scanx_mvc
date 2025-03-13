$(document).ready(function () {
    $("#uploadBtn").click(function () {
        var formData = new FormData();
        formData.append("file", $("#pdfUpload")[0].files[0]);

        $.ajax({
            url: '/api/pdf/upload',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $("#fileName").text(response.fileName);
                    $("#pdfContent").val(response.text);
                    getAiAnswer(response.text, response.fileName);
                } else {
                    alert(response.message);
                }
            }
        });
    });

    function getAiAnswer(text, fileName) {
        $.ajax({
            url: '/api/pdf/ai-answer',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ text: text, fileName: fileName }),
            success: function (response) {
                $("#aiOutput").val(response.answer);
            }
        });
    }
});

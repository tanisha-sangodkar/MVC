$(document).ready(function () {
    $(".addToCartBtn").click(function () {
        var itemId = $(this).data("item-id");
        var form = $("#addToCartForm_" + itemId);
        var messageDiv = $("#message_" + itemId);

        // Simulate the form submission (replace this with actual form submission logic)
        form.submit();

        // Display a success message
        messageDiv.html("Adding to Cart!");
        setTimeout(function () {
            messageDiv.html("");
        }, 3000);
    });

   
    setTimeout(function () {
        document.getElementById('error').style.display = 'none';
    }, 3000);


});
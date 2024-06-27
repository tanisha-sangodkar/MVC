$(document).ready(function () {

    // Get the selected quantity value
    var quantityElement = $('#Quantity');

    

    quantityElement.change(function () {
 
        // Assuming you have a price per unit (you can replace this with your actual price variable)
        var pricePerUnit = parseFloat($('#Price').val());
        var quantity = $('#Quantity').val();
        // Calculate the total amount
        var totalAmount = quantity * pricePerUnit;

        // Update the Total Amount textbox
        $('#TotalAmount').val(totalAmount.toFixed(2)); // Assuming you want to round to 2 decimal places
    });
    if (quantityElement.val() == 0) {
        quantityElement.val("1");
    }
    else {
        quantityElement.trigger("change");
    }

    quantityElement.trigger("change");
});
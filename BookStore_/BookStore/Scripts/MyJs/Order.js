$(document).ready(function () {
     $('.delivery-status-dropdown').change(function () {
         var orderId = $(this).data('orderid');
            var newStatus = $(this).val();

            // Update the hidden fields with the selected status and order ID
            $(`#DeliveryStatus_${orderId}`).val(newStatus);
            $(`#Id_${orderId}`).val(orderId);

            // Submit the form
            $(`#deliveryStatusForm_${orderId}`).submit();
        });
});

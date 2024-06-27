function preventBack() {
    window.history.forward();
}

setTimeout("preventBack()", 0);
window.onunload = function () { null };


$(document).ready(function () {
    calculateGrandTotal();
});


function updateTotalAmount(itemId) {
    var trRowElement = document.getElementById(itemId);
    console.log('Row Element', trRowElement);

    var priceElement = trRowElement.querySelector('#price');
    var priceValue = parseFloat(priceElement.innerText); 

    var quantityElement = trRowElement.querySelector('#item_Quantity');
    var quantityValue = parseFloat(quantityElement.value); 

    var calculatedTotalAmount = quantityValue * priceValue;

    var TotalAmount=trRowElement.querySelector('#item_TotalAmount')
    TotalAmount.value = calculatedTotalAmount;

    calculateGrandTotal();
    }


function calculateGrandTotal() {
    var bookDetails = document.getElementsByClassName('book-detail');
    var count = bookDetails.length;
    var calculatedGrandTotalAmount=0;
    var grandTotalAmount = document.getElementById('grand-total');

    for (var i = 0; i < count; i++) {
        console.log(bookDetails[i])
        console.log(bookDetails[i].querySelector('#item_TotalAmount').value)
        calculatedGrandTotalAmount +=parseFloat(bookDetails[i].querySelector('#item_TotalAmount').value); 
    }
    grandTotalAmount.innerText = calculatedGrandTotalAmount;

}


function increment(itemId) {
    var trRowElement = document.getElementById(itemId);
    console.log('Row Element', trRowElement);

    var quantityElement = trRowElement.querySelector('#item_Quantity');
    var quantityValue = parseFloat(quantityElement.value); 

   
    quantityValue += 1;
    quantityElement.value = quantityValue;
    updateTotalAmount(itemId) 
    calculateGrandTotal();
}


function decrement(itemId) {
    var trRowElement = document.getElementById(itemId);
    console.log('Row Element', trRowElement);

    var decrement = trRowElement.querySelector('#decrement'); 

    var quantityElement = trRowElement.querySelector('#item_Quantity');
    var quantityValue = parseFloat(quantityElement.value);
    quantityValue -= 1;
   
    if (quantityValue >=1) {
        decrement.style.display = "visible";
        quantityElement.value = quantityValue;
        updateTotalAmount(itemId)
        calculateGrandTotal();
    }
   
   
}


//quantityValue -= 1;
//if (quantityValue >= 1) {
//    decrement.style.visibility = "visible";
//    quantityElement.value = quantityValue;
//    updateTotalAmount(itemId)
//    calculateGrandTotal();
//}
//if (quantityValue == 1) {
//    /*decrement.style.display = "none";*/
//    decrement.style.visibility = "hidden";
//}













     //var price = $('#' + itemId + ' .cart-td[data-price]').data('price');
        //var quantity = $('#' + itemId + ' .cart-td-q input').val();
        //var totalAmount = price * quantity;
        //console.log('Price: ' + price);
        //console.log('Quantity: ' + quantity);
        //// Set the total amount in the corresponding column
        //$('#' + itemId + ' .cart-td-ta input').val(totalAmount);
        //console.log('Total Amount: ' + totalAmount);
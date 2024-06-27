$(document).ready(function () {
    //validating firstname for minimum 4 characters and text only
    $('#BookName').on('change', function () {
        var name = $(this).val();
        var isValid = /^[a-zA-Z !@#$%^&*()']+$/.test(name) && name.length >= 4;

        if (!isValid) {
            $('#BookNameErrorMessage').text('Minimum 2 characters long and contain only alphabetic characters.');
        } else {
            $('#BookNameErrorMessage').text('');
        }
    });

    $('#Author').on('change', function () {
        var name = $(this).val();
        var isValid = /^[a-zA-Z ]+$/.test(name) && name.length >= 4;

        if (!isValid) {
            $('#AuthorErrorMessage').text('Minimum 4 characters long and contain only alphabetic characters.');
        } else {
            $('#AuthorErrorMessage').text('');
        }
    });


    $('#Genre').on('change', function () {
        var name = $(this).val();
        var isValid = /^[a-zA-Z ]+$/.test(name) && name.length >= 4;

        if (!isValid) {
            $('#GenreErrorMessage').text('Minimum 4 characters long and contain only alphabetic characters.');
        } else {
            $('#GenreErrorMessage').text('');
        }
    });

    $('#Description').on('change', function () {
        var name = $(this).val();
        var isValid = /^[a-zA-Z ]+$/.test(name) && name.length >= 4;

        if (!isValid) {
            $('#DescriptionErrorMessage').text('Minimum 4 characters long and contain only alphabetic characters.');
        } else {
            $('#DescriptionErrorMessage').text('');
        }
    });

    $('#Price').on('blur', function () {
        var price = $(this).val();
        var isValid = /^\d*$/.test(price);

        if (!isValid) {
            $('#PriceErrorMessage').text('Invalid price.Enter number.');
        } else {
            $('#PriceErrorMessage').text('');
        }
    });

    $('#Quantity').on('blur', function () {
        var quantity = $(this).val();
        var isValid = /^\d*$/.test(quantity);

        if (!isValid) {
            $('#QuantityErrorMessage').text('Invalid quantity.Enter number.');
        } else {
            $('#QuantityErrorMessage').text('');
        }
    });

    $('#Pages').on('blur', function () {
        var pages = $(this).val();
        var isValid = /^\d*$/.test(pages);

        if (!isValid) {
            $('#PagesErrorMessage').text('Invalid pages.Enter number.');
        } else {
            $('#PagesErrorMessage').text('');
        }
    });

    $('#Language').on('change', function () {
        var name = $(this).val();
        var isValid = /^[a-zA-Z ]+$/.test(name) && name.length >= 4;

        if (!isValid) {
            $('#LanguageErrorMessage').text('Minimum 4 characters long and contain only alphabetic characters.');
        } else {
            $('#LanguageErrorMessage').text('');
        }
    });

    $('#PublicationYear').on('blur', function () {
        var year = $(this).val();
        var isValid = /^\d{4}$/.test(year);

        if (!isValid) {
            $('#PublicationYearErrorMessage').text('Invalid year.');
        } else {
            $('#PublicationYearErrorMessage').text('');
        }
    });
});



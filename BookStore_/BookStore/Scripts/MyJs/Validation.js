$(document).ready(function () {
    

    var stateSelect = $("#State");
    var citySelect = $("#City");
    var userSelectedCity = $("#cityValue").data("city");
    console.log(userSelectedCity)
   
    //validating firstname for minimum 4 characters and text only
    $('#FirstName').on('change', function () {
        var name = $(this).val();
        var isValid = /^[a-zA-Z]*$/.test(name) && name.length >= 4;

        if (!isValid) {
            $('#firstNameErrorMessage').text('Minimum 4 characters long and contain only alphabetic characters.');
        } else {
            $('#firstNameErrorMessage').text('');
        }
    });


    //validating lastname for minimum 4 characters and text only
    $('#LastName').on('change', function () {
        var name = $(this).val();
        var isValid = /^[a-zA-Z]*$/.test(name) && name.length >= 4;

        if (!isValid) {
            $('#lastNameErrorMessage').text('Minimum 4 characters long and contain only alphabetic characters.');
        } else {
            $('#lastNameErrorMessage').text('');
        }
    });

    //validate email
    $('#EmailId').on('change', function () {
        var email = $(this).val();
        var isValid = validateEmail(email);

        if (!isValid) {
            $('#emailErrorMessage').text('Invalid email address.');
        } else {
            $('#emailErrorMessage').text('');
        }
    });

    function validateEmail(email) {
        // Regular expression for a simple email validation
        var regex = /^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        return regex.test(email);
    }
    $('#UserName').on('change', function () {
        var name = $(this).val();
        var isValid = /^[a-zA-Z]*$/.test(name) && name.length >= 4;

      
        if (!isValid) {
            $('#UserNameErrorMessage').text('Minimum 4 characters long and contain only alphabetic characters.');
        }       
        else {
            $('#UserNameErrorMessage').text('');
            checkEmailUsername(name);
        }
       
    });

    function checkEmailUsername(name) {
        var email = $('#EmailId').val();

        // Extract the part before '@' from the email
        var emailPrefix = email.split('@')[0];

        if (name !== emailPrefix) {
            $('#UserNameErrorMessage').text('Username should match email before @.');
        } else {
            $('#UserNameErrorMessage').text('');
        }

    }

    // Auto-calculate age based on Date of Birth
    $('#DateOfBirth').on('change', function () {
        var dob = new Date($(this).val());

        var selectedDate = $(this).val();

        if (!selectedDate) {
            $('#dateErrorMessage').text('Date of birth is required.');
        } else {
            $('#dateErrorMessage').text('');
        }


        var age = calculateAge(dob);
        $('#Age').val(age);

        // Validate age and display error message
        if (age < 18) {
            $('#ageErrorMessage').text('You must be at least 18 years old.');
        } else {
            $('#ageErrorMessage').text('');
        }
    });

    function calculateAge(dateOfBirth) {
        var currentDate = new Date();
        var age = currentDate.getFullYear() - dateOfBirth.getFullYear();
        var monthDiff = currentDate.getMonth() - dateOfBirth.getMonth();

        if (monthDiff < 0 || (monthDiff === 0 && currentDate.getDate() < dateOfBirth.getDate())) {
            age--;
        }

        return age;
    }

    //validate password
    $('#Password').on('blur', function () {
        var password = $(this).val();
        var isValid = validatePassword(password);

        if (!isValid) {
            $('#passwordErrorMessage').text('Invalid password.');
        } else {
            $('#passwordErrorMessage').text('');
        }
    });

    // ^ ((?=.*? [A - Z])(?=.*? [a - z])(?=.*? [0 - 9]) | (?=.*? [A - Z])(?=.*? [a - z])(?=.*? [^ a - zA - Z0 - 9]) | (?=.*? [A - Z])(?=.*? [0 - 9])(?=.*? [^ a - zA - Z0 - 9]) | (?=.*? [a - z])(?=.*? [0 - 9])(?=.*? [^ a - zA - Z0 - 9])).{ 8,} $
    function validatePassword(password) {
        // Regular expression for password validation (customize as needed)
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=]).*$/;
        return regex.test(password);
    }

    //validate confirm password
    $('#ConfirmPassword').on('blur', function () {
        var password = $('#Password').val();
        var confirmPassword = $(this).val();
        var isValid = password === confirmPassword;//checking if valid

        if (!isValid) {
            $('#confirmPasswordErrorMessage').text('Passwords do not match.');
        } else {
            $('#confirmPasswordErrorMessage').text('');
        }
    });

    //validating phone number
    $('#PhoneNumber').on('blur', function () {
        var phoneNumber = $(this).val();
        var isValid = /^\d{10}$/.test(phoneNumber);

        if (!isValid) {
            $('#phoneNumberErrorMessage').text('Invalid phone number.Enter a 10-digit number.');
        } else {
            $('#phoneNumberErrorMessage').text('');
        }
    });

    //validating image
    $('#imageInput').on('change', function () {
        var fileName = $(this).val();
        var isValid = fileName !== '';

        if (!isValid) {
            $('#fileErrorMessage').text('File is required.');
        } else {
            $('#fileErrorMessage').text('');
        }
    });
    //$("#myForm").submit(function (e) {
    //    // Reset previous error message
    //    $("#fileErrorMessage").text("");

    //    // Check if a file is selected
    //    var fileName = $("#imageInput").val();
    //    if (fileName === "") {
    //        $("#fileErrorMessage").text("Please select a file.");
    //        e.preventDefault(); // Prevent form submission
    //    }
    //});
    //suggesstion
    $("#Suggesstion").on('change', function () {
        var textareaValue = $(this).val();

        // Check if there is at least one sentence
        if (textareaValue === null || textareaValue.length < 4) {
            $('#suggesstionErrorMessage').text("Please enter at least one sentence in the suggestion box.");
        } else {
            $('#suggesstionErrorMessage').text('');
        }
    });

    /*state*/
    console.log("State Element Found");

    stateSelect.change(function () {
       
        var selectedState = stateSelect.val();
       
        citySelect.empty();
        addCityOption(citySelect, "Select city", "none");


        if (selectedState === "Goa") {
            addCityOption(citySelect, "Panjim", "Panjim");
            addCityOption(citySelect, "Margao", "Margao");
            addCityOption(citySelect, "Mapusa", "Mapusa");
        }
        else if (selectedState === "Maharastra") {
            addCityOption(citySelect, "Mumbai", "Mumbai");
            addCityOption(citySelect, "Thane", "Thane");
            addCityOption(citySelect, "Andheri", "Andheri");
        }
        else if (selectedState === "Karnataka") {
            addCityOption(citySelect, "Bangalore", "Bangalore");
            addCityOption(citySelect, "Belgaum", "Belgaum");
            addCityOption(citySelect, "Hubli", "Hubli");
        }
        else {
            addCityOption(citySelect, "Ahmedabad", "Ahmedabad");
            addCityOption(citySelect, "Gandhinagar", "Gandhinagar");
            addCityOption(citySelect, "Vadodara", "Vadodara");
        }

        citySelect.prop("selectedIndex", 0);
        citySelect.val("none");
/*        stateSelect.trigger("change");*/

        if (userSelectedCity && citySelect.find('option[value="' + userSelectedCity + '"]').length > 0) {
            citySelect.val(userSelectedCity);
        } else {
            citySelect.prop("selectedIndex", 0);
            citySelect.val("none");
        }
    });

    stateSelect.change();
    function getModelCity() {
        // Example: Assuming your model has a property named 'city'
        return yourModel.city;
    }
    function addCityOption(selectElement, cityName, cityValue) {
        selectElement.append($("<option>", {
            value: cityValue,
            text: cityName
        }));
    }
   



    

});
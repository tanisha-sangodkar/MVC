using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class CustomerDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Invalid first name")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Invalid last name")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date of Birth is required.")]
        [CustomValidation(typeof(CustomerDetails), "ValidateDateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }

        [Required(ErrorMessage = "Email id is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Invalid password ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]                   //specifies that the input should be treated as a password, hiding the input characters.
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10,}$", ErrorMessage = "Enter 10-digit phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string UserName { get; set; }

        public string ImageData { get; set; }

   
        public static ValidationResult ValidateDateOfBirth(DateTime dob)            //checks if user has selected the future date
        {
            if (dob > DateTime.Now)
            {
                return new ValidationResult("Date of Birth cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
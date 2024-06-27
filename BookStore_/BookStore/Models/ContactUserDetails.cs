using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class ContactUserDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Invalid first name")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email id is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Suggesstion is required.")]
        public string Suggesstion { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10,}$", ErrorMessage = "Enter 10-digit phone number.")]
        public string PhoneNumber { get; set; }
    }
}
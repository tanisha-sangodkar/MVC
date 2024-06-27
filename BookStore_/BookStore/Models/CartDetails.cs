using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class CartDetails
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Invalid first name")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Invalid first name")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string LastName { get; set; }

        public int BookId { get; set; }

        [Required(ErrorMessage = "Book name is required.")]
        [RegularExpression(@"^[a-zA-Z !@#$%^&*()']+$", ErrorMessage = "Invalid book name")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter minimum 2 characters.")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Book Price is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numeric characters.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Book Price is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numeric characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Book Quantity is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numeric characters.")]
        public int Quantity { get; set; }

        public int TotalAmount { get; set; }
        public string DeliveryStatus { get; set; }

        public string UserName { get; set; }
        public string BookImage { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
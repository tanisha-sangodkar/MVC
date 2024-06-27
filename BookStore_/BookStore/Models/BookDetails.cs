using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class BookDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book name is required.")]
        [RegularExpression(@"^[a-zA-Z !@#$%^&*()']+$", ErrorMessage = "Invalid book name")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter minimum 2 characters.")]
        public string BookName{ get; set; }

        [Required(ErrorMessage = "Book Author is required.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Invalid book author")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Book Genre is required.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Invalid book genre")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Book Description is required.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Invalid book description")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Book Price is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numeric characters.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Book Quantity is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numeric characters.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Book Pages is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numeric characters.")]
        public int Pages { get; set; }

        [Required(ErrorMessage = "Book Language is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Invalid book language")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "Enter minimum 4 characters.")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Book Quantity is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numeric characters.")]
        public int PublicationYear { get; set; }
        public string BookImage { get; set; }

        public int TotalAmount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int BookId { get; set; }

        [Required(ErrorMessage = "Book Quantity is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numeric characters.")]
        public int Quantity { get; set; }

    }
}
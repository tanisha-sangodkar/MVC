using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace BookStore.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int  Customer_Id { get; set; }   
        public string FirstName { get; set;}
        public string LastName { get; set;}

        public int BookId { get; set; }
        public string BookName { get; set;}
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int Pages { get; set; }

        public string Username { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set;}
        public int TotalAmount { get; set; }
        public string ImageData { get; set; }

        public string BookImage { get; set; }
        public DateTime orderDate { get; set; }
    }
}
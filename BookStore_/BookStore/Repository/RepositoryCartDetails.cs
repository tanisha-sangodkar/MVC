using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Diagnostics;
using WebGrease.Css.Ast.Selectors;
using System.Drawing;
using Microsoft.Ajax.Utilities;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using BookStore.Helper;

namespace BookStore.Repository
{
    public class RepositoryCartDetails
    {
        /// <summary>
        /// adding cart details in cart table by mapping it to the customerid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookDetails"></param>
        /// <returns></returns>
        public bool AddCartDetails(int customerId, BookDetails bookDetails)
        {
            try
            {
                bool insert_count = false;
                bool exist_count = false;
                bool update_count = false;
                CustomerDetails customerdetails = new CustomerDetails();

                exist_count = CheckBookExist(customerId, bookDetails.Id);       //checks if the book exist in cart 
                if (exist_count)
                {

                    update_count = UpdateCartBookQuantity(customerId, bookDetails);     //if exist increase the quantity
                    if (update_count)      //checking if the quantity is updated
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                else
                {
                    insert_count = InsertIntoCart(customerId, bookDetails);
                    if (insert_count)      //checking if the book is inserted n cart table
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// getting list of books added in cart by that customer
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<CartDetails> GetCartDetails(string username)
        {
            CustomerDetails customerDetail = new CustomerDetails();
            List<CartDetails> cartDetails = new List<CartDetails>();
            try
            {
                var Cart = new List<Cart>();

                var bookdetail = new BookDetails();

                customerDetail = GetCustomerId(username);           //getting user details
                Cart = GetCartByCustomerId(customerDetail);         //getting cart books
                foreach (var cartValue in Cart)
                {

                    bookdetail = GetBookDetailByBookId(cartValue.BookId);
                    CartDetails cartdetail = new CartDetails();
                    cartdetail.Id = cartValue.Id;
                    cartdetail.CustomerId = cartValue.CustomerId;
                    cartdetail.BookId = cartValue.BookId;
                    cartdetail.BookName = bookdetail.BookName;
                    cartdetail.Quantity = cartValue.Quantity;
                    cartdetail.Price = bookdetail.Price;
                    cartdetail.BookImage = bookdetail.BookImage;
                    cartdetail.TotalAmount = cartdetail.Quantity * cartdetail.Price;
                    cartDetails.Add(cartdetail);
                }
                return cartDetails;

            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return cartDetails;
            }
        }


        /// <summary>
        /// insert a book detail into cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookDetails"></param>
        /// <returns></returns>
        public bool InsertIntoCart(int customerId, BookDetails bookDetails)
        {
            try
            {
                int insert_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPAddCartDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@BookId", bookDetails.Id);
                        cmd.Parameters.AddWithValue("@Quantity", bookDetails.Quantity);
                        insert_count = cmd.ExecuteNonQuery();
                    }
                }
                if (insert_count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// checking if the book already exist in the cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public bool CheckBookExist(int customerId, int bookId)
        {
            try
            {
                int exist_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("USPBookExistInCart", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@customer_id", customerId);
                        cmd.Parameters.AddWithValue("@book_id", bookId);
                        exist_count = (int)cmd.ExecuteScalar();
                    }
                }
                if (exist_count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// updates the quantity of the book if book exit in cart
        /// </summary>
        public bool UpdateCartBookQuantity(int customerId, BookDetails bookDetails)
        {
            try
            {
                int insert_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPUpdateCartBookQuantity", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BookId", bookDetails.Id);
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);

                        bookDetails.Quantity++;
                        cmd.Parameters.AddWithValue("@quantity", bookDetails.Quantity);
                        insert_count = cmd.ExecuteNonQuery();
                    }
                }
                if (insert_count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// fetching id and username by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public CustomerDetails GetCustomerId(string username)
        {
            CustomerDetails customer = new CustomerDetails();
            try
            {

                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPGetCustomerId", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customer.Id = (int)reader["Id"];
                                customer.UserName = reader["UserName"].ToString();

                            }
                        }
                    }
                    con.Close();
                }
                return customer;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return customer;
            }
        }


        /// <summary>
        /// fetching cart data by customer id
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public List<Cart> GetCartByCustomerId(CustomerDetails customer)
        {
            List<Cart> cartDetail = new List<Cart>();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPSelectCartDetailsOfUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", customer.Id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cart cart = new Cart();
                                cart.Id = (int)reader["Id"];
                                cart.BookId = (int)reader["BookId"];
                                cart.Quantity = (int)reader["Quantity"];
                                cart.CustomerId = (int)reader["CustomerId"];
                                cartDetail.Add(cart);
                            }

                        }
                        con.Close();
                    }
                }
                return cartDetail;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return cartDetail;
            }
        }


        /// <summary>
        /// fecthing book detail based on bookid
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public BookDetails GetBookDetailByBookId(int bookId)
        {
            BookDetails bookDetail = new BookDetails();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("USPSelectBookDetailsByBookId", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BookId", bookId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bookDetail.Id = (int)reader["Id"];
                                bookDetail.BookName = reader["BookName"].ToString();
                                bookDetail.Price = (int)reader["Price"];
                                bookDetail.BookImage = reader["BookImage"].ToString();
                            }
                        }
                        con.Close();
                    }
                }
                return bookDetail;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return bookDetail;
            }
        }

        /// <summary>
        /// deleting from the cart when the user clicks on buy
        /// </summary>
        /// <param name="cartid"></param>
        /// <returns></returns>
        public bool deleteCartDetails(int bookid, int customerid)
        {
            int delete_count = 0;
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPDeleteCart", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@bookid", bookid);
                        cmd.Parameters.AddWithValue("@customerid", customerid);
                        delete_count = cmd.ExecuteNonQuery();
                    }
                }
                if (delete_count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// get cart detail based on customerid and bookid
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public OrderDetails CartDetails(int bookid, int customerid)
        {
            OrderDetails orderDetails = new OrderDetails();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPGetCartDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@bookid", bookid);
                        cmd.Parameters.AddWithValue("@customerid", customerid);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderDetails.BookId = (int)reader["BookId"];
                                orderDetails.Quantity = (int)reader["Quantity"];
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("USPSelectBookDetailsByBookId", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BookId", orderDetails.BookId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderDetails.BookId = (int)reader["Id"];
                                orderDetails.BookName = reader["BookName"].ToString();
                                orderDetails.Price = (int)reader["Price"];
                                orderDetails.ImageData = reader["BookImage"].ToString();
                                orderDetails.BookImage = orderDetails.ImageData;
                            }
                        }
                    }

                    return orderDetails;
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return orderDetails;
            }
        }


        /// <summary>
        /// inserting order detail on sql
        /// </summary>
        /// <param name="orderdetails"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool AllCartBooks(List<CartDetails> orderdetails, string username)
        {
            try
            {
                int count = 0;
                CustomerDetails customerDetails = new CustomerDetails();
                bool delete_result = false;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    customerDetails = GetCustomerDetailByUsername(username);
                    foreach (var order in orderdetails)
                    {
                        order.CustomerId = customerDetails.Id;
                        order.FirstName = customerDetails.FirstName;
                        order.LastName = customerDetails.LastName;
                        order.PhoneNumber = customerDetails.PhoneNumber;


                        // Create a SqlCommand object to call the stored procedure
                        RepositoryBookDetails repositoryBookDetails = new RepositoryBookDetails();

                        var bookQuantity = repositoryBookDetails.SearchEditBook(order.BookId).Quantity;
                        if (bookQuantity > order.Quantity)
                        {
                            bool result = AddOrder(order);
                            if (result == true)
                            {
                                var calculatedQuantity = bookQuantity - order.Quantity;
                               RepositoryBookDetails updateQuantity = new RepositoryBookDetails();
                                updateQuantity.UpdateQuantity(order.BookId, calculatedQuantity);
                                delete_result = deleteCartDetailsByCustomerId(order.CustomerId);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// get customer detail based on id
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
                
            public CustomerDetails GetCustomerDetailByUsername(string username)
             {
            CustomerDetails customer = new CustomerDetails();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    customer = GetCustomerId(username);
                    using (SqlCommand cmd = new SqlCommand("USPGetCustomerDetails", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", customer.Id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customer.FirstName = reader["FirstName"].ToString(); ;
                                customer.LastName = reader["LastName"].ToString();
                                customer.DateOfBirth = (DateTime)reader["DateOfBirth"];
                                customer.Age = (int)reader["Age"];
                                customer.EmailId = reader["EmailId"].ToString();
                                customer.PhoneNumber = reader["PhoneNumber"].ToString();
                                customer.Gender = reader["Gender"].ToString();
                                customer.State = reader["State"].ToString();
                                customer.City = reader["City"].ToString();
                                customer.ImageData = reader["ImageData"].ToString();
                            }
                        }
                    }

                }
                return customer;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return customer;
            }
        }

        /// <summary>
        /// insert order details
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool AddOrder(CartDetails orderdetail)
        {
            try
            {
                int count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("USPAddOrderDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", orderdetail.CustomerId);
                        cmd.Parameters.AddWithValue("@FirstName", orderdetail.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", orderdetail.LastName);
                        cmd.Parameters.AddWithValue("@BookId", orderdetail.BookId);

                        cmd.Parameters.AddWithValue("@BookName", orderdetail.BookName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", orderdetail.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Quantity", orderdetail.Quantity);
                        cmd.Parameters.AddWithValue("@Price", orderdetail.Price);

                        orderdetail.TotalAmount = (int)orderdetail.Price * (int)orderdetail.Quantity;
                        cmd.Parameters.AddWithValue("@TotalAmount", orderdetail.TotalAmount);

                        count = cmd.ExecuteNonQuery();

                    }
                }

                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// delete cart details based on customer id
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public bool deleteCartDetailsByCustomerId(int customerid)
        {
            try
            {
                int delete_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPDeleteCartByCustomerId", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@customerid", customerid);
                        delete_count = cmd.ExecuteNonQuery();
                    }
                }
                if (delete_count >= 1)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// fetching quntity from cart table based on customerid and bookid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookdetail"></param>
        /// <returns></returns>
        public int GetQuantityByCustomerCart(int customerId, BookDetails bookdetail)
        {
            try
            {
                int quantity = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SPGetQuantityByCustomerCart", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@customerid", customerId);
                        cmd.Parameters.AddWithValue("@bookid", bookdetail.Id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                quantity = (int)reader["quantity"];
                            }
                        }
                    }
                }
                if (quantity == null)
                {
                    quantity = 0;
                }
                return quantity;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return 0;
            }
        }



        /// <summary>
        /// fetching quantity based on cart id
        /// </summary>
        /// <param name="cartid"></param>
        /// <returns></returns>
        public Cart GetCartById(int cartid)
        {
            Cart cart = new Cart();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPGetCartById", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", cartid);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cart.Id = (int)reader["id"];
                                cart.BookId = (int)reader["BookId"];
                                cart.Quantity = (int)reader["Quantity"];
                                cart.CustomerId = (int)reader["CustomerId"];

                            }
                        }
                    }
                }
                return cart;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return cart;
            }
        }



        /// <summary>
        /// updates book quantity in cart table based on cartid
        /// </summary>
        /// <param name="cartid"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool UpdateQuantityById(int cartid, int quantity)
        {
            try
            {
                int count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPUpdateQuantityByCartId", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", cartid);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        count = cmd.ExecuteNonQuery();

                    }
                }
                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

    }
}
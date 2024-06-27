using BookStore.Helper;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using static System.Collections.Specialized.BitVector32;

namespace BookStore.Repository
{
    public class RepositoryOrderDetails
    {
        /// <summary>
        /// adding order detail in order table
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="bookdetails"></param>
        /// <returns></returns>
        public bool AddOrder(string UserName, OrderDetails bookdetails)
        {
            try
            {

                bool count = false;
                int insert_count = 0;

                CustomerDetails customerdetails = new CustomerDetails();

                // Create a SqlCommand object to call the stored procedure
                customerdetails = GetCustomerDetail(UserName);
                count = InsertOrderDetail(customerdetails, bookdetails);

                if (count)      //checking if the user is inserted
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
        /// getting user details from table
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public CustomerDetails GetCustomerDetail(string UserName)
        {
            CustomerDetails customerdetails = new CustomerDetails();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();


                    using (SqlCommand cmd = new SqlCommand("USPSearch", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", UserName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerdetails.Id = (int)reader["Id"];
                                customerdetails.FirstName = reader["FirstName"].ToString();
                                customerdetails.LastName = reader["LastName"].ToString();
                                customerdetails.PhoneNumber = reader["PhoneNumber"].ToString();
                            }
                        }
                    }

                }
                return customerdetails;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return customerdetails;
            }
        }

        /// <summary>
        /// add order detail in order table
        /// </summary>
        /// <param name="customerdetails"></param>
        /// <param name="bookdetails"></param>
        /// <returns></returns>
        public bool InsertOrderDetail(CustomerDetails customerdetails, OrderDetails bookdetails)
        {
            try
            {
                int insert_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("USPAddOrderDetails", con))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        //Add parameters for the stored procedure
                        command.Parameters.AddWithValue("@CustomerId", customerdetails.Id);
                        command.Parameters.AddWithValue("@FirstName", customerdetails.FirstName);
                        command.Parameters.AddWithValue("@LastName", customerdetails.LastName);
                        command.Parameters.AddWithValue("@BookId", bookdetails.BookId);
                        command.Parameters.AddWithValue("@BookName", bookdetails.BookName);
                        command.Parameters.AddWithValue("@PhoneNumber", customerdetails.PhoneNumber);
                        command.Parameters.AddWithValue("@Quantity", bookdetails.Quantity);
                        command.Parameters.AddWithValue("@Price", bookdetails.Price);
                        command.Parameters.AddWithValue("@TotalAmount", bookdetails.TotalAmount); // Pass the Base64-encoded image data

                        // Execute the stored procedure
                        insert_count = command.ExecuteNonQuery();
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
        /// get order detail from order table based on id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<CartDetails> GetOrderdetails(int Id)
        {
            List<CartDetails> cartdetails = new List<CartDetails>();
            try
            {
                int insert_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPFetchOrderDetails", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", Id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CartDetails cartdetail = new CartDetails
                                {
                                    Id = (int)reader["id"],
                                    CustomerId = (int)reader["CustomerId"],
                                    FirstName = reader["FirstName"].ToString(),
                                    BookId = (int)reader["BookId"],
                                    BookName = reader["BookName"].ToString(),
                                    Quantity = (int)reader["Quantity"],
                                    Price = (int)reader["Price"],
                                    TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                                    OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                    DeliveryStatus = reader["DeliveryStatus"].ToString()
                                };
                                cartdetail.TotalAmount = cartdetail.Price * cartdetail.Quantity;
                                cartdetails.Add(cartdetail);
                            }

                        }
                    }
                }
                return cartdetails;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return cartdetails;
            }
        }


        /// <summary>
        /// get whole order detail table for admin page
        /// </summary>
        /// <returns></returns>
        public List<CartDetails> GetOrderdetails()
        {
            List<CartDetails> cartdetails = new List<CartDetails>();
            try
            {
                int insert_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPGetOrderDetails", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CartDetails cartdetail = new CartDetails
                                {
                                    Id = (int)reader["Id"],
                                    CustomerId = (int)reader["CustomerId"],
                                    FirstName = reader["FirstName"].ToString(),
                                    BookId = (int)reader["BookId"],
                                    BookName = reader["BookName"].ToString(),
                                    Quantity = (int)reader["Quantity"],
                                    Price = (int)reader["Price"],
                                    TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                                    DeliveryStatus = reader["DeliveryStatus"].ToString(),
                                    OrderDate = (DateTime)reader["OrderDate"]
                                };
                                cartdetail.TotalAmount = cartdetail.Price * cartdetail.Quantity;
                                cartdetails.Add(cartdetail);
                            }

                        }
                    }
                }
                return cartdetails;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return cartdetails;
            }
        }


        /// <summary>
        /// get order detail based on username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<CartDetails> GetOrderdetails(string username)
        {
            List<CartDetails> cartdetails = new List<CartDetails>();
            try
            {

                CustomerDetails customer = new CustomerDetails();
                int insert_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    customer = GetCustomerDetail(username);
                    cartdetails = GetOrderdetails(customer.Id);
                }
                return cartdetails;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return cartdetails;
            }
        }


        /// <summary>
        /// update the delivery status when admin changes
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>

        public bool UpdateOrderDeliveryStatus(CartDetails orderDetails)
        {
            try
            {
                int insert_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPGetUpdateOrderDelStatus", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@id", orderDetails.Id);
                        cmd.Parameters.AddWithValue("@deliverystatus", orderDetails.DeliveryStatus);
                        insert_count = cmd.ExecuteNonQuery();
                    }
                }
                if (insert_count >= 1)      //checking if the user is inserted
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
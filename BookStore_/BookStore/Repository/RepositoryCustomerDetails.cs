using BookStore.Helper;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace BookStore.Repository
{
    public class RepositoryCustomerDetails
    {
        /// <summary>
        /// fetching all the customer detail from the table and returning it to admin page
        /// </summary>
        /// <returns></returns>
        public List<CustomerDetails> GetAllCustomer()
        {
            List<CustomerDetails> customerDetail = new List<CustomerDetails>();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("USPDisplayCustomerDetails", con))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database columns to the User model properties
                                CustomerDetails customerDetails = new CustomerDetails
                                {
                                    Id = (int)reader["Id"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    DateOfBirth = (DateTime)reader["DateOfBirth"],
                                    Age = (int)reader["Age"],
                                    EmailId = reader["EmailId"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    State = reader["State"].ToString(),
                                    City = reader["City"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    ImageData = reader["ImageData"].ToString()
                                };


                                customerDetail.Add(customerDetails);
                            }
                        }
                    }

                }
                return customerDetail;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return customerDetail;
            }
        }

        /// <summary>
        /// fetching single customer detail based on username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public CustomerDetails GetCustomerDetails(string username)
        {
            CustomerDetails customerDetail = new CustomerDetails();
            CustomerDetails customerinfo = new CustomerDetails();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("USPGetCustomerId", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database columns to the User model properties

                                customerinfo.Id = (int)reader["Id"];
                                customerinfo.UserName = reader["UserName"].ToString();

                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("USPGetEditCustomerDetail", con))
                    {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", customerinfo.Id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database columns to the User model properties
                                CustomerDetails customerDetails = new CustomerDetails
                                {
                                    Id = (int)reader["id"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    DateOfBirth = (DateTime)reader["DateOfBirth"],
                                    Age = (int)reader["Age"],
                                    EmailId = reader["EmailId"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    State = reader["State"].ToString(),
                                    City = reader["City"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    ImageData = reader["ImageData"].ToString()
                                };
                                customerDetail = customerDetails;
                            }
                        }
                    }

                }
                return customerDetail;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return customerDetail;
            }
        }


        /// <summary>
        /// gets customer detail rom the database based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerDetails GetCustomerDetails(int id)
        {
            CustomerDetails customerDetail = new CustomerDetails();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {

                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("USPGetEditCustomerDetail", con))
                    {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database columns to the User model properties
                                CustomerDetails customerDetails = new CustomerDetails
                                {
                                    Id = (int)reader["id"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    DateOfBirth = (DateTime)reader["DateOfBirth"],
                                    Age = (int)reader["Age"],
                                    EmailId = reader["EmailId"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    State = reader["State"].ToString(),
                                    City = reader["City"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    ImageData = reader["ImageData"].ToString()
                                };
                                customerDetail = customerDetails;
                            }
                        }
                    }

                }
                return customerDetail;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return customerDetail;
            }
        }


        /// <summary>
        /// edits the customer data in database
        /// </summary>
        /// <param name="EditUserInfo"></param>
        /// <param name="imageInput"></param>
        /// <returns>true when edits.false when doent edit</returns>
        public bool EditUser(CustomerDetails EditUserInfo, HttpPostedFileBase imageInput)
        {
            //update user logic
            int insert_count = 0;
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPUpdateCustomerDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", EditUserInfo.Id);
                        cmd.Parameters.AddWithValue("@FirstName", EditUserInfo.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", EditUserInfo.LastName);
                        cmd.Parameters.AddWithValue("@DateOfBirth", EditUserInfo.DateOfBirth);
                        cmd.Parameters.AddWithValue("@Age", EditUserInfo.Age);
                        cmd.Parameters.AddWithValue("@EmailId", EditUserInfo.EmailId);
                        cmd.Parameters.AddWithValue("@PhoneNumber", EditUserInfo.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Gender", EditUserInfo.Gender);
                        cmd.Parameters.AddWithValue("@State", EditUserInfo.State);
                        cmd.Parameters.AddWithValue("@City", EditUserInfo.City);
                        cmd.Parameters.AddWithValue("@UserName", EditUserInfo.UserName);

                        string imageBase64 = null;

                        if (imageInput != null && imageInput.ContentLength > 0)
                        {
                            using (var binaryReader = new BinaryReader(imageInput.InputStream))
                            {
                                byte[] imageData = binaryReader.ReadBytes(imageInput.ContentLength);
                                imageBase64 = Convert.ToBase64String(imageData);
                            }
                        }
                        EditUserInfo.ImageData = imageBase64;
                        cmd.Parameters.AddWithValue("@ImageData", EditUserInfo.ImageData);

                        insert_count = cmd.ExecuteNonQuery();

                    }
                }
                // Execute the stored procedure
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


        /// <summary>
        /// deletes customer from table
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool deleteCustomer(int Id)
        {
            try
            {
                int delete_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPDeleteCustomer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Id);
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
        /// changes user password in database
        /// </summary>
        /// <param name="userPassword"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ChangeUserPassword(ChangePassword userPassword, string username)
        {
            try
            {
                int update_count = 0;
                string encrytPassword = EncryptPassword(userPassword.Password);
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPUserChangePassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@password", encrytPassword);
                        cmd.Parameters.AddWithValue("@username", username);
                        update_count = cmd.ExecuteNonQuery();

                    }
                }

                // Execute the stored procedure
                if (update_count >= 1)      //checking if the user is inserted
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
        /// encrypts user password before adding database
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string EncryptPassword(string password)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
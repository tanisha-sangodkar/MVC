using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Web;
using BookStore.Helper;
using System.Web.Mvc;

namespace BookStore.Repository
{
 
    public class RepositoryRegisterUser
    { 
        /// <summary>
        /// registers user data
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="imageInput"></param>
        /// <returns></returns>
        public bool AddUser(CustomerDetails userInfo, HttpPostedFileBase imageInput)
        {
            int insert_count = 0;
            string imageBase64 = null;
            try {
                if (imageInput != null && imageInput.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(imageInput.InputStream))
                    {
                        byte[] imageData = binaryReader.ReadBytes(imageInput.ContentLength);
                        imageBase64 = Convert.ToBase64String(imageData);
                    }
                    // Set the Base64-encoded image data in the model
                    userInfo.ImageData = imageBase64;
                    
                    string encrytPassword = EncryptPassword(userInfo.Password);
                    RepositoryDuplicateCheck userDuplicate = new RepositoryDuplicateCheck();
                    string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        if (userDuplicate.DuplicateCheck(userInfo) == true)
                        {
                            // Create a SqlCommand object to call the stored procedure
                            using (SqlCommand command = new SqlCommand("USPRegisterUserDetails", con))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;

                                //Add parameters for the stored procedure

                                command.Parameters.AddWithValue("@FirstName", userInfo.FirstName);
                                command.Parameters.AddWithValue("@LastName", userInfo.LastName);
                                command.Parameters.AddWithValue("@DateOfBirth", userInfo.DateOfBirth);
                                command.Parameters.AddWithValue("@Age", userInfo.Age);
                                command.Parameters.AddWithValue("@EmailId", userInfo.EmailId);
                                command.Parameters.AddWithValue("@Password", encrytPassword);
                                command.Parameters.AddWithValue("@PhoneNumber", userInfo.PhoneNumber);
                                command.Parameters.AddWithValue("@Gender", userInfo.Gender);
                                command.Parameters.AddWithValue("@State", userInfo.State);
                                command.Parameters.AddWithValue("@City", userInfo.City);
                                command.Parameters.AddWithValue("@UserName", userInfo.UserName);
                                command.Parameters.AddWithValue("@ImageData", userInfo.ImageData); // Pass the Base64-encoded image data

                                // Execute the stored procedure
                                insert_count = command.ExecuteNonQuery();

                            }
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
        /// create a log file
        /// </summary>
        /// <param name="ex"></param>
        private string EncryptPassword(string password)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }    
    }
}
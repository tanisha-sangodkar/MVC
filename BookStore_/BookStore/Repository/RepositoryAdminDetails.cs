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

namespace BookStore.Repository
{
    public class RepositoryAdminDetails
    {
        /// <summary>
        /// registers a new admin
        /// </summary>
        /// <param name="AdminInfo"></param>
        /// <param name="adminImage"></param>
        /// <returns></returns>
        public bool AddAdmin(AdminDetails AdminInfo, HttpPostedFileBase adminImage)
        {
            int insert_count = 0;
            string imageBase64 = null;

            try
            {
                if (adminImage != null && adminImage.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(adminImage.InputStream))
                    {
                        // Set the Base64-encoded image data in the model
                        byte[] imageData = binaryReader.ReadBytes(adminImage.ContentLength);
                        imageBase64 = Convert.ToBase64String(imageData);
                    }
                   
                    AdminInfo.ImageData = imageBase64;

                    string encrytPassword = EncryptPassword(AdminInfo.Password);
                    RepositoryDuplicateCheck adminDuplicate = new RepositoryDuplicateCheck();
                    string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        if (adminDuplicate.DuplicateAdminCheck(AdminInfo) == true)
                        {
                            // Create a SqlCommand object to call the stored procedure
                            using (SqlCommand command = new SqlCommand("USPRegisterAdminDetails", con))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;

                                //Add parameters for the stored procedure

                                command.Parameters.AddWithValue("@FirstName", AdminInfo.FirstName);
                                command.Parameters.AddWithValue("@LastName", AdminInfo.LastName);
                                command.Parameters.AddWithValue("@DateOfBirth", AdminInfo.DateOfBirth);
                                command.Parameters.AddWithValue("@Age", AdminInfo.Age);
                                command.Parameters.AddWithValue("@EmailId", AdminInfo.EmailId);
                                command.Parameters.AddWithValue("@Password", encrytPassword);
                                command.Parameters.AddWithValue("@PhoneNumber", AdminInfo.PhoneNumber);
                                command.Parameters.AddWithValue("@Gender", AdminInfo.Gender);
                                command.Parameters.AddWithValue("@State", AdminInfo.State);
                                command.Parameters.AddWithValue("@City", AdminInfo.City);
                                command.Parameters.AddWithValue("@UserName", AdminInfo.UserName);
                                command.Parameters.AddWithValue("@ImageData", AdminInfo.ImageData); // Pass the Base64-encoded image data

                                // Execute the stored procedure
                                insert_count = command.ExecuteNonQuery();

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
        /// changes admin password
        /// </summary>
        /// <param name="userPassword"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ChangeAdminPassword(ChangePassword userPassword, string username)
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
                    using (SqlCommand cmd = new SqlCommand("USPAdminChangePassword", con))
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
        /// encrypts the password before adding it in database
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string EncryptPassword(string password)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}

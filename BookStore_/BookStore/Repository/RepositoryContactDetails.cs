using BookStore.Helper;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace BookStore.Repository
{
    /// <summary>
    /// inert user added contact us details
    /// </summary>
    public class RepositoryContactDetails
    {
        /// <summary>
        /// adds details from the contact us page into the database
        /// </summary>
        /// <param name="contactUser"></param>
        /// <returns></returns>
        public bool AddContactDetails(ContactUserDetails contactUser)
        {
            int insert_count = 0;
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand command = new SqlCommand("USPContactDetails", con))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Add parameters for the stored procedure

                        command.Parameters.AddWithValue("@FirstName", contactUser.FirstName);
                        command.Parameters.AddWithValue("@EmailId", contactUser.EmailId);
                        command.Parameters.AddWithValue("@Suggesstion", contactUser.Suggesstion);
                        command.Parameters.AddWithValue("@PhoneNumber", contactUser.PhoneNumber);


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
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// displaying the contct etail on admin side contact page
        /// </summary>
        /// <returns></returns>
        public List<ContactUserDetails> DisplayContactDetails()
        {
            List<ContactUserDetails> contactDetail = new List<ContactUserDetails>();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("USPContactusDetails", con))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database columns to the User model properties
                                ContactUserDetails contactDetails = new ContactUserDetails
                                {
                                    Id = (int)reader["Id"],
                                    FirstName = reader["FirstName"].ToString(),
                                    EmailId = reader["EmailId"].ToString(),
                                    Suggesstion = reader["Suggesstion"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                };


                                contactDetail.Add(contactDetails);
                            }
                        }
                    }

                }
                return contactDetail;
            }

            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return contactDetail;
            }
        }


    }
}
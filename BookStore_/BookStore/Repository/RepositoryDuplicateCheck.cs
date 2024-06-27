using BookStore.Helper;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace BookStore.Repository
{
    public class RepositoryDuplicateCheck
    {
        /// <summary>
        /// checking for duplicate email,username and password for user
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <returns></returns>
        public bool DuplicateCheck(CustomerDetails customerDetails)
        {
            try { 
            int duplicate_count = 0;
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("USPDuplicateEmailUsernamePhoneNumber", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    //Add parameters for the stored procedure

                    command.Parameters.AddWithValue("@EmailId", customerDetails.EmailId);
                    command.Parameters.AddWithValue("@UserName", customerDetails.UserName);
                    command.Parameters.AddWithValue("@PhoneNumber", customerDetails.PhoneNumber);

                    duplicate_count = (int)command.ExecuteScalar();
                }
            }
            if (duplicate_count ==0)      //checking if the user is presented
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
        /// checking for duplicate email,username and password for admin
        /// </summary>
        /// <param name="adminDetails"></param>
        /// <returns></returns>
        public bool DuplicateAdminCheck(AdminDetails adminDetails)
        {
            try { 
            int duplicate_count = 0;
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("USPAdDuplicateEmailUsernamePhoneNumber", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    //Add parameters for the stored procedure

                    command.Parameters.AddWithValue("@EmailId", adminDetails.EmailId);
                    command.Parameters.AddWithValue("@UserName", adminDetails.UserName);
                    command.Parameters.AddWithValue("@PhoneNumber", adminDetails.PhoneNumber);

                    duplicate_count = (int)command.ExecuteScalar();
                }
            }
            if (duplicate_count == 0)      //checking if the user is present
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
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
    public class RepositoryLogin
    {
        /// <summary>
        /// User and admin login page
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool LoginUser(LoginUserDetails user)
        {
            try
            {
                CustomerDetails customer = new CustomerDetails();
                int user_result = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPSearchLoginUserCount", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", user.UserName);
                        user_result = (int)cmd.ExecuteScalar();
                    }

                }
                if (user_result >= 1)
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("USPSearchLoginUser", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", user.UserName);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    customer.UserName = reader["Username"].ToString();
                                    customer.EmailId = reader["EmailId"].ToString();
                                    customer.Password = reader["Password"].ToString();
                                }
                            }
                            string decryptPassword = DecryptPassword(customer.Password);

                            if (decryptPassword == user.Password)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
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
        /// checks whether admin exists or no
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if password matches.false if doesnt match</returns>
        public bool LoginAdmin(LoginUserDetails user)
        {
            try
            {
                AdminDetails admin = new AdminDetails();
                int admin_result = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USPSearchLoginAdminCount", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", user.UserName);
                        admin_result = (int)cmd.ExecuteScalar();
                    }

                }
                if (admin_result >= 1)
                {

                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("USPSearchLoginAdmin", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", user.UserName);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    admin.UserName = reader["Username"].ToString();
                                    admin.EmailId = reader["EmailId"].ToString();
                                    admin.Password = reader["Password"].ToString();
                                }
                            }
                            //string decryptPassword = DecryptPassword(admin.Password);

                            if (admin.Password == user.Password)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
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
        /// decrypts password 
        /// </summary>
        /// <param name="UserPassword"></param>
        /// <returns></returns>
        private string DecryptPassword(string UserPassword)
        {
            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(UserPassword));
        }
    }
}
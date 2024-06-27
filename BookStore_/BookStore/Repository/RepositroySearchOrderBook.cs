using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using BookStore.Helper;

namespace BookStore.Repository
{
    public class RepositroySearchOrderBook
    {
        /// <summary>
        /// searching the order book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookDetails SearchOrderBook(int id)
        {
            BookDetails SearchBook = new BookDetails();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPSearchBook", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SearchBook.Id = (int)reader["Id"];
                                SearchBook.BookName = reader["BookName"].ToString();
                                SearchBook.Author = reader["Author"].ToString();
                                SearchBook.Genre = reader["Genre"].ToString();
                                SearchBook.Description = reader["Description"].ToString();
                                SearchBook.Price = (int)reader["Price"];
                                SearchBook.Pages = (int)reader["Pages"];
                                SearchBook.Language = reader["Language"].ToString();
                                SearchBook.PublicationYear = (int)reader["PublicationYear"];
                                SearchBook.BookImage = reader["BookImage"].ToString();
                            }
                        }
                    }
                }
                //displaying details in view
                return SearchBook;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return SearchBook;
            }
        }

        /// <summary>
        /// searching the book quntity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SearchQuantity(int id)
        {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                int quantity = 0;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPSelectBookQuantity", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = (int)reader["Id"];
                                quantity = (int)reader["Quantity"];

                            }
                        }
                    }
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
    }
}
using BookStore.Helper;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace BookStore.Repository
{
    public class RepositoryBookDetails
    {
        /// <summary>
        /// adding book details in book table
        /// </summary>
        /// <param name="bookdetails"></param>
        /// <param name="bookImage"></param>
        /// <returns></returns>
        public bool AddBook(BookDetails bookdetails, HttpPostedFileBase bookImage)
        {
            try
            {
                int insert_count = 0;
                string imageBase64 = null;
                if (bookImage != null && bookImage.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(bookImage.InputStream))
                    {
                        byte[] imageData = binaryReader.ReadBytes(bookImage.ContentLength);
                        imageBase64 = Convert.ToBase64String(imageData);
                    }
                    // Set the Base64-encoded image data in the model
                    bookdetails.BookImage = imageBase64;

                    string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();

                        // Create a SqlCommand object to call the stored procedure
                        using (SqlCommand command = new SqlCommand("USPAddBookDetails", con))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            //Add parameters for the stored procedure
                            command.Parameters.AddWithValue("@BookName", bookdetails.BookName);
                            command.Parameters.AddWithValue("@Author", bookdetails.Author);
                            command.Parameters.AddWithValue("@Genre", bookdetails.Genre);
                            command.Parameters.AddWithValue("@Description", bookdetails.Description);
                            command.Parameters.AddWithValue("@Price", bookdetails.Price);
                            command.Parameters.AddWithValue("@Quantity", bookdetails.Quantity);
                            command.Parameters.AddWithValue("@Pages", bookdetails.Pages);
                            command.Parameters.AddWithValue("@Language", bookdetails.Language);
                            command.Parameters.AddWithValue("@PublicationYear", bookdetails.PublicationYear);
                            command.Parameters.AddWithValue("@BookImage", bookdetails.BookImage); // Pass the Base64-encoded image data

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

            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// retriveing all the books from the table
        /// </summary>
        /// <returns>list of books to the view</returns>
        public List<BookDetails> DisplayBookDetails()
        {
            List<BookDetails> bookDetail = new List<BookDetails>();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("USPDisplayBookDetails", con))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database columns to the User model properties
                                BookDetails bookDetails = new BookDetails
                                {
                                    Id = (int)reader["Id"],
                                    BookName = reader["BookName"].ToString(),
                                    Author = reader["Author"].ToString(),
                                    Genre = reader["Genre"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (int)reader["Price"],
                                    Quantity = (int)reader["Quantity"],
                                    Pages = (int)reader["Pages"],
                                    Language = reader["Language"].ToString(),
                                    PublicationYear = (int)reader["PublicationYear"],
                                    BookImage = reader["BookImage"].ToString()
                                };
                                bookDetail.Add(bookDetails);
                            }
                        }
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
        /// Retrive the book that is to be edited from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookDetails SearchEditBook(int id)
        {
            //retrieveing book details from database to be edited         
            BookDetails SearchEditBook = new BookDetails();
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
                                //retrives the book details to be edited
                                SearchEditBook.Id = (int)reader["Id"];
                                SearchEditBook.BookName = reader["BookName"].ToString();
                                SearchEditBook.Author = reader["Author"].ToString();
                                SearchEditBook.Genre = reader["Genre"].ToString();
                                SearchEditBook.Description = reader["Description"].ToString();
                                SearchEditBook.Price = (int)reader["Price"];
                                SearchEditBook.Quantity = (int)reader["Quantity"];
                                SearchEditBook.Pages = (int)reader["Pages"];
                                SearchEditBook.Language = reader["Language"].ToString();
                                SearchEditBook.PublicationYear = (int)reader["PublicationYear"];
                                SearchEditBook.BookImage = reader["BookImage"].ToString();
                            }
                        }
                    }
                }
                //displaying details in view
                return SearchEditBook;
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return SearchEditBook;
            }
        }

        /// <summary>
        /// Update the book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateBook(BookDetails bookDetails, HttpPostedFileBase bookImage)
        {
            try
            {
                //update user logic
                int insert_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPEditBook", con))
                    {
                        //inserted the edited detils of the book
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", bookDetails.Id);
                        cmd.Parameters.AddWithValue("@BookName", bookDetails.BookName);
                        cmd.Parameters.AddWithValue("@Author", bookDetails.Author);
                        cmd.Parameters.AddWithValue("@Genre", bookDetails.Genre);
                        cmd.Parameters.AddWithValue("@Description", bookDetails.Description);
                        cmd.Parameters.AddWithValue("@Price", bookDetails.Price);
                        cmd.Parameters.AddWithValue("@Quantity", bookDetails.Quantity);
                        cmd.Parameters.AddWithValue("@Pages", bookDetails.Pages);
                        cmd.Parameters.AddWithValue("@Language", bookDetails.Language);
                        cmd.Parameters.AddWithValue("@PublicationYear", bookDetails.PublicationYear);

                        string imageBase64 = null;

                        if (bookImage != null && bookImage.ContentLength > 0)
                        {
                            using (var binaryReader = new BinaryReader(bookImage.InputStream))
                            {
                                byte[] imageData = binaryReader.ReadBytes(bookImage.ContentLength);
                                imageBase64 = Convert.ToBase64String(imageData);
                            }
                        }
                        bookDetails.BookImage = imageBase64;
                        cmd.Parameters.AddWithValue("@BookImage", bookDetails.BookImage);

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
        /// delete book from database based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteBook(int id)
        {
            try
            {
                //delete book logic
                int delete_count = 0;

                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("DeleteBook", con))
                    {
                        //deleted book based on id
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
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
        /// retrive book from database based on what is entered in search bar
        /// </summary>
        /// <param name="bookname"></param>
        /// <returns></returns>
        public List<BookDetails> SearchBookByName(string bookname)
        {
            List<BookDetails> bookDetail = new List<BookDetails>();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("SearchBookName", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@bookname", bookname);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookDetails bookdetail = new BookDetails
                                {
                                    Id = (int)reader["Id"],
                                    BookName = reader["BookName"].ToString(),
                                    Author = reader["Author"].ToString(),
                                    Genre = reader["Genre"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (int)reader["Price"],
                                    Quantity = (int)reader["Quantity"],
                                    Pages = (int)reader["Pages"],
                                    Language = reader["Language"].ToString(),
                                    PublicationYear = (int)reader["PublicationYear"],
                                    BookImage = reader["BookImage"].ToString()
                                };
                                bookDetail.Add(bookdetail);
                            }

                        }
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
        /// updating book quantity when the user purchases the book
        /// </summary>
        public bool UpdateQuantity(int id, int quantity)
        {
            try
            {
                int update_count = 0;
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    // Create a SqlCommand object to call the stored procedure
                    using (SqlCommand cmd = new SqlCommand("USPUpdateBookQuantity", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        update_count = cmd.ExecuteNonQuery();

                    }
                }
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

    }
}


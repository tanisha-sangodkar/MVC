using BookStore.Helper;
using BookStore.Models;
using BookStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        /// <summary>
        /// Dashboard or the homepage of the admin
        /// </summary>
        /// <returns>view with options</returns>
        public ActionResult AdminHomePage()
        {
            return View();
        }

        /// <summary>
        /// Book stock details
        /// </summary>
        /// <returns>view to edit or delete the book along with other details</returns>
        public ActionResult BookInventory()
        {
            List<BookDetails> allBooks = new List<BookDetails>();
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    //display logic
                    RepositoryBookDetails bookInformation = new RepositoryBookDetails();

                    // calling function in repository for displaying users
                    allBooks = bookInformation.DisplayBookDetails();
                }//store the fetched database details
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return View(allBooks);
        }
        /// <summary>
        /// Book insert view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddNewBook()
        {
            return View();
        }

        /// <summary>
        /// inserts the user data
        /// </summary>
        /// <param name="bookdetails"></param>
        /// <param name="bookImage"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewBook(BookDetails bookdetails, HttpPostedFileBase bookImage)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Registration logic 
                        RepositoryBookDetails bookDetail = new RepositoryBookDetails();

                        //calling function in repository to insert user
                        if (bookDetail.AddBook(bookdetails, bookImage))
                        {
                            //if successfully inserted
                            ViewBag.SuccessMessage = "Registration successful!";
                            ModelState.Clear();
                        }
                    }
                    else
                    {
                        // If model validation fails or no image was uploaded, return to the registration page
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return View();
        }

        /// <summary>
        /// Book to be edited is retrived from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditBook(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    RepositoryBookDetails searchBook = new RepositoryBookDetails();
                    BookDetails editBook = new BookDetails();
                    // calling function in repository to retireve update user
                    editBook = searchBook.SearchEditBook(id);                                 //store the fetched database details
                    return View(editBook);
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditBook(BookDetails bookdetails, HttpPostedFileBase bookImage)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    RepositoryBookDetails editBook = new RepositoryBookDetails();
                    // calling function in repository to retireve update user
                    if (editBook.UpdateBook(bookdetails, bookImage))
                    {
                        //if successfully inserted
                        ViewBag.SuccessMessage = "Book Updated successful!";
                        ModelState.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return View();
        }

        /// <summary>
        /// customer registered details wl be available
        /// </summary>
        /// <returns>table with customer details</returns>
        [HttpGet]
        public ActionResult CustomerDetails()
        {
            List<CustomerDetails> allCustomer = new List<CustomerDetails>();
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {

                    //display logic
                    RepositoryCustomerDetails customerInformation = new RepositoryCustomerDetails();

                    // calling function in repository for displaying users
                    allCustomer = customerInformation.GetAllCustomer();                              //store the fetched database details             
                    return View(allCustomer);
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return View(allCustomer);
        }

        /// <summary>
        /// contains order details
        /// </summary>
        /// <returns>table with order information</returns>
        [HttpGet]
        public ActionResult OrderDetails()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    List<CartDetails> orderDetails = new List<CartDetails>();
                    RepositoryOrderDetails orderDetail = new RepositoryOrderDetails();
                    orderDetails = orderDetail.GetOrderdetails();
                    return View(orderDetails);
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public ActionResult OrderDetails(CartDetails orderDetails)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {

                    RepositoryOrderDetails orderDetail = new RepositoryOrderDetails();
                    if (orderDetail.UpdateOrderDeliveryStatus(orderDetails))
                    {
                        return RedirectToAction("OrderDetails", "Admin");
                    }
                    return RedirectToAction("OrderDetails", "Admin");
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return RedirectToAction("OrderDetails", "Admin");
            }
        }

        /// <summary>
        /// Registrating new admin
        /// </summary>
        /// <returns>view for adding new admin</returns>
        [HttpGet]
        public ActionResult AddAdmin()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }

        /// <summary>
        /// inserts admin detail in table
        /// </summary>
        /// <param name="adminDetails"></param>
        /// <param name="adminImage"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAdmin(AdminDetails adminDetails, HttpPostedFileBase adminImage)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {

                    if (ModelState.IsValid)
                    {
                        //calculating the age difference 
                        TimeSpan ageDifference = DateTime.Now - adminDetails.DateOfBirth;
                        int age = (int)(ageDifference.TotalDays / 365);
                        adminDetails.Age = age;

                        //Registration logic 
                        RepositoryAdminDetails admin = new RepositoryAdminDetails();

                        //calling function in repository to insert user
                        if (admin.AddAdmin(adminDetails, adminImage))
                        {
                            //if successfully inserted
                            ViewBag.SuccessMessage = "Registration successful!";
                            ModelState.Clear();
                        }
                        else
                        {
                            // If model validation fails or no image was uploaded, return to the registration page
                            ViewBag.FailureMessage = "Registration unsuccessful!";
                            ModelState.Clear();
                            return View();
                        }
                    }
                    else
                    {
                        // If model validation fails or no image was uploaded, return to the registration page
                        ViewBag.FailureMessage = "Registration unsuccessful!";
                        ModelState.Clear();
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
            return View();
        }

        /// <summary>
        /// Admin will be able to change password
        /// </summary>
        /// <returns>view to change password</returns>
        [HttpGet]
        public ActionResult AdminChangePassword()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public ActionResult AdminChangePassword(ChangePassword userPassword)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    string username = (string)Session["UserName"];
                    RepositoryAdminDetails adminChangePassword = new RepositoryAdminDetails();
                    if (adminChangePassword.ChangeAdminPassword(userPassword, username))
                    {
                        ViewBag.successfulmessage = "password changed successfully";
                    }
                    else
                    {
                        ViewBag.failuremessage = "password not changed successfully";
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }

        /// <summary>
        /// all infomation from the contact table will be displayed
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContactDetails()
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    List<ContactUserDetails> allContactDetail = new List<ContactUserDetails>();
                    //display logic
                    RepositoryContactDetails contactInformation = new RepositoryContactDetails();

                    // calling function in repository for displaying users
                    allContactDetail = contactInformation.DisplayContactDetails();                              //store the fetched database details

                    return View(allContactDetail);
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeleteBook(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    //delete logic
                    RepositoryBookDetails deleteBook = new RepositoryBookDetails();

                    deleteBook.DeleteBook(id);//store the fetched database details

                    return RedirectToAction("BookInventory", "Admin");
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }

        /// <summary>
        /// admin registration page will reset
        /// </summary>
        /// <returns>home page</returns>
        public ActionResult AdminReset()
        {
            try
            {
                ModelState.Clear();
                return View(); // Redirect to the home page
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
            return View();
        }


        /// <summary>
        /// deletes user from table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteCustomer(int id)
        {
            try
            {
                RepositoryCustomerDetails deleteCustomer = new RepositoryCustomerDetails();
                if (deleteCustomer.deleteCustomer(id))
                {
                    return RedirectToAction("CustomerDetails", "Admin");
                }
                else
                {
                    ViewBag.message = "Deletion Unsuccessfully";
                    return RedirectToAction("CustomerDetails", "Admin");
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return RedirectToAction("CustomerDetails", "Admin");
            }
        }


    }
}

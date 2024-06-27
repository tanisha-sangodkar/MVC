using BookStore.Models;
using BookStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using BookStore.Helper;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Home page with login,contact us,register and about us is available
        /// </summary>
        /// <returns></returns>
        public ActionResult MainPage()
        {
            return View();
        }

        /// <summary>
        /// User login view is available
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// user login code where first it will cheeck it user than for admin and if not than diplays user noy found message
        /// </summary>
        /// <param name="user"></param>
        /// <returns>If User:retuns uer home page
        /// if admin:return admin home page</returns>

        [HttpPost]
        public ActionResult Login(LoginUserDetails user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RepositoryLogin loginUser = new RepositoryLogin();
                    RepositoryLogin loginAdmin = new RepositoryLogin();

                    // calling function in repository for logic user
                    if (loginUser.LoginUser(user))
                    {
                        Session["UserName"] = user.UserName;
                        return RedirectToAction("CustomerHomePage", "Customer");
                    }
                    else if (loginAdmin.LoginAdmin(user))
                    {                    
                        Session["UserName"] = user.UserName;
                        return RedirectToAction("AdminHomePage", "Admin");
                    }
                    else
                    {
                        ViewBag.message = "User not found";
                    }
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
            }
            return View();
        }

        /// <summary>
        /// User registration
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(CustomerDetails userInfo, HttpPostedFileBase imageInput)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //calculating the age difference 
                    TimeSpan ageDifference = DateTime.Now - userInfo.DateOfBirth;
                    int age = (int)(ageDifference.TotalDays / 365);
                    userInfo.Age = age;

                    //Registration logic 
                    RepositoryRegisterUser user = new RepositoryRegisterUser();

                    //calling function in repository to insert user
                    if (user.AddUser(userInfo, imageInput))
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
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return View();
        }

        /// <summary>
        /// contact us form will be displayed
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactUserDetails contactUser)
        {
            try
            {
                RepositoryContactDetails ContactDetails = new RepositoryContactDetails();
                if (ContactDetails.AddContactDetails(contactUser))
                {
                    //if successfully inserted
                    ViewBag.SuccessMessage = "Sent successfull!";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.FailureMessage = "Sent unsuccessfull!";
                    ModelState.Clear();
                    return View();
                }
                return View();
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
        /// infrmation about the book store is available
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// session will get clear 
        /// </summary>
        /// <returns>to home page</returns>
        public ActionResult UserLogout()
        {
            try
            {
                Session.Clear(); // Clear user session data
                Session.RemoveAll();
                return RedirectToAction("MainPage", "Home"); // Redirect to the home page
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
        /// user registration page will reset
        /// </summary>
        /// <returns>home page</returns>
        [HttpPost]
        public ActionResult UserReset()
        {
            try
            {
                ModelState.Clear();
                return View("Registration", "Home"); // Redirect to the home page
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return View("Registration", "Home");
        }
        /// <summary>
        /// ession will get clear
        /// </summary>
        /// <returns>home page</returns>
        public ActionResult AdminLogout()
        {
            try
            {
                Session.Clear(); // Clear admin session data
                Session.RemoveAll();
                return RedirectToAction("MainPage", "Home"); // Redirect to the home page
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return View();
        }



    }
}
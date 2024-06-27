using BookStore.Models;
using BookStore.Repository;
using Microsoft.Ajax.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using BookStore.Helper;

namespace BookStore.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        /// <summary>
        /// Main Page of the customer side
        /// </summary>
        /// <returns>main page view where user can buy books</returns>
        [HttpGet]
        public ActionResult CustomerHomePage(string errorMessage = "")
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    List<BookDetails> allBooks = new List<BookDetails>();
                    //display logic
                    RepositoryBookDetails bookInformation = new RepositoryBookDetails();
                    ViewBag.ErrorMessage = errorMessage;
                    // calling function in repository for displaying users
                    allBooks = bookInformation.DisplayBookDetails();                              //store the fetched database details               
                    return View(allBooks);
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
        /// search the bookname from the searchbar
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchBook(string searchQuery)
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
                    var bookDetails = searchBook.SearchBookByName(searchQuery);

                    return View("CustomerHomePage", bookDetails);

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
        /// My cart page: books added in cart will be visible
        /// </summary>
        /// <returns>view with book quatity,price,can do buy now </returns>

        [HttpGet]
        public ActionResult CustomerCart()
        {

            try
            {
                if (ModelState.IsValid)
                {
                    RepositoryCartDetails getCartDetails = new RepositoryCartDetails();
                    List<CartDetails> Cartdetails = new List<CartDetails>();
                    if (Session["UserName"] == null)
                    {
                        return RedirectToAction("MainPage", "Home");
                    }
                    else
                    {
                        string username = (string)Session["UserName"];
                        Cartdetails = getCartDetails.GetCartDetails(username);
                        return View(Cartdetails);
                    }
                }
                else
                {
                    ViewBag.message = "Invalid Quantity.Numeric inputs only ";
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
        /// pass the book details in cart
        /// </summary>
        /// <param name="bookdetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerCart(BookDetails bookdetails)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    RepositoryCartDetails repositoryCartDetails = new RepositoryCartDetails();
                    RepositoryOrderDetails searchUser = new RepositoryOrderDetails();

                    string userName = (string)Session["UserName"];
                    var customerId = searchUser.GetCustomerDetail(userName).Id;          //getting user details

                    var cartQuantity = repositoryCartDetails.GetQuantityByCustomerCart(customerId, bookdetails);
                    if (bookdetails.Quantity >= (cartQuantity + 1))
                    {
                        bookdetails.Quantity = 1;       //setting quantity as 1
                        var totalAmount = (int)bookdetails.Price * (int)bookdetails.Quantity;   //caluculating total amount basd on quntity
                        bookdetails.TotalAmount = totalAmount;

                        RepositoryCartDetails cartDetails = new RepositoryCartDetails();

                        if (cartDetails.AddCartDetails(customerId, bookdetails))
                        {
                            return RedirectToAction("CustomerHomePage", "Customer");
                        }

                    }
                    else
                    {
                        string errormessage = $"Insufficient Quantity of {bookdetails.BookName}";
                        return RedirectToAction("CustomerHomePage", "Customer", new { errorMessage = errormessage });
                    }
                }
                return RedirectToAction("CustomerHomePage", "Customer");
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return RedirectToAction("CustomerHomePage", "Customer");
            }

        }


        /// <summary>
        /// when user selects buy all from the cart page
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerBuyAll(List<CartDetails> orderDetails)
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
                    RepositoryCartDetails orderDetail = new RepositoryCartDetails();
                    if (orderDetail.AllCartBooks(orderDetails, username))
                    {
                        return RedirectToAction("CustomerOrderDetail", "Customer");
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return RedirectToAction("CustomerCart", "Customer");
        }

        /// <summary>
        /// display customer detials
        /// </summary>
        /// <returns>view consisting customer details</returns>
        [HttpGet]
        public ActionResult CustomerProfile()
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
                    RepositoryCustomerDetails customerInformation = new RepositoryCustomerDetails();
                    CustomerDetails customer = new CustomerDetails();
                    // calling function in repository for displaying users            
                    customer = customerInformation.GetCustomerDetails(username);
                    return View(customer);
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
        /// display customer detials
        /// </summary>
        /// <returns>view consisting customer details</returns>
        [HttpGet]
        public ActionResult CustomerEdit(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    RepositoryCustomerDetails customerInformation = new RepositoryCustomerDetails();
                    CustomerDetails customer = new CustomerDetails();
                    // calling function in repository for displaying users            
                    customer = customerInformation.GetCustomerDetails(id);
                    return View(customer);
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
        /// after edit the details it will return to this page
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <param name="imageInput"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerEdit(CustomerDetails customerDetails, HttpPostedFileBase imageInput)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    RepositoryCustomerDetails customerInformation = new RepositoryCustomerDetails();

                    // calling function in repository for updating user details            
                    if (customerInformation.EditUser(customerDetails, imageInput))
                    {
                        Session["UserName"] = customerDetails.UserName;
                        ModelState.Clear();

                        ViewBag.SuccessMessage = "Updated Successfully";
                    }
                    else
                    {
                        ModelState.Clear();
                        ViewBag.FailureMessage = "Updation Unsuccessfully";
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
        /// displays book details on the order detail page
        /// </summary>
        /// <param name="cartid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CustomerOrder(int bookid, int customerid)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {

                    OrderDetails orderDetails = new OrderDetails();
                    RepositoryCartDetails searchbookdetails = new RepositoryCartDetails();
                    orderDetails = searchbookdetails.CartDetails(bookid, customerid);
                    // calling function in repository to retireve update user
                    //SearchBook = searchBook.SearchOrderBook(id);
                    return View(orderDetails);
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
        /// updates quantity
        /// </summary>
        /// <param name="count"></param>
        /// <param name="cartid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateQuantity(int count,int cartid)
        {
            try
            {
                RepositoryCartDetails repositoryCartDetails = new RepositoryCartDetails();
                
               var quantity =repositoryCartDetails.GetCartById(cartid).Quantity;
                quantity = count + quantity;
                if (quantity > 0)
                {
                    repositoryCartDetails.UpdateQuantityById(cartid, quantity);
                    return RedirectToAction("CustomerCart", "Customer");
                }
                else
                {
                    return RedirectToAction("CustomerCart", "Customer");
                }
                       
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return RedirectToAction("CustomerCart", "Customer");
            }
        }

        /// <summary>
        /// Orders that customer have ordered
        /// </summary>
        /// <param name="bookdetails"></param>
        /// <returns>Orders that customer have ordered will visible</returns>
        [HttpPost]
        public ActionResult CustomerOrder(OrderDetails bookdetails)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    RepositroySearchOrderBook compareQuantity = new RepositroySearchOrderBook();
                    int recieved_Quantity = 0;
                    recieved_Quantity = compareQuantity.SearchQuantity(bookdetails.BookId);
                    if (recieved_Quantity >= bookdetails.Quantity)
                    {
                        var totalAmount = (int)bookdetails.Price * (int)bookdetails.Quantity;
                        bookdetails.TotalAmount = totalAmount;
                        string username = (string)Session["Username"];


                        RepositoryOrderDetails addOrderDetails = new RepositoryOrderDetails();
                        if (addOrderDetails.AddOrder(username, bookdetails))
                        {
                            RepositoryBookDetails bookQuanity = new RepositoryBookDetails();       //calculating quantity
                            int calculatedQuantity = recieved_Quantity - bookdetails.Quantity;

                            bookQuanity.UpdateQuantity(bookdetails.BookId, calculatedQuantity);                 //Sendng quantity to update

                            RepositoryOrderDetails customerId = new RepositoryOrderDetails();
                            CustomerDetails customer = customerId.GetCustomerDetail(username);

                            RepositoryCartDetails deleteCustomer = new RepositoryCartDetails();
                            deleteCustomer.deleteCartDetails(bookdetails.BookId, customer.Id);
                            return RedirectToAction("CustomerOrderDetail", "Customer", new { username = customer.UserName });
                        }
                        else
                        {
                            return View(bookdetails);
                        }
                    }
                    else
                    {

                        ViewBag.Message = "Quantity Insufficient";
                        return View(bookdetails);
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
        /// display the buying details of the book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult CustomerBuy(int id)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    RepositroySearchOrderBook searchBook = new RepositroySearchOrderBook();
                    BookDetails SearchBook = new BookDetails();
                    // calling function in repository to retireve update user
                    SearchBook = searchBook.SearchOrderBook(id);
                    return View(SearchBook);

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
        /// adds order in order table aftr pressing buy button
        /// </summary>
        /// <param name="bookdetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerBuy(OrderDetails bookdetails)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("MainPage", "Home");
                }
                else
                {
                    bookdetails.BookId = bookdetails.Id;
                    RepositroySearchOrderBook compareQuantity = new RepositroySearchOrderBook();
                    int recieved_Quantity = 0;
                    recieved_Quantity = compareQuantity.SearchQuantity(bookdetails.Id);
                    if (recieved_Quantity >= bookdetails.Quantity)
                    {
                        var totalAmount = (int)bookdetails.Price * (int)bookdetails.Quantity;
                        bookdetails.TotalAmount = totalAmount;
                        string username = (string)Session["Username"];


                        RepositoryOrderDetails addOrderDetails = new RepositoryOrderDetails();
                        if (addOrderDetails.AddOrder(username, bookdetails))
                        {
                            RepositoryBookDetails bookQuanity = new RepositoryBookDetails();       //calculating quantity
                            int calculatedQuantity = recieved_Quantity - bookdetails.Quantity;

                            bookQuanity.UpdateQuantity(bookdetails.Id, calculatedQuantity);                 //Sendng quantity to update

                            RepositoryOrderDetails customerId = new RepositoryOrderDetails();
                            CustomerDetails customer = customerId.GetCustomerDetail(username);

                            RepositoryCartDetails deleteCustomer = new RepositoryCartDetails();
                            deleteCustomer.deleteCartDetails(bookdetails.Id, customer.Id);
                            return RedirectToAction("CustomerOrderDetail", "Customer"); /*, new { id = customer.Id }*/
                        }
                        else
                        {
                            return View();
                        }
                    }              
                    else
                    {
                        ViewBag.Message = "Quantity available is less than seleted";
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
            return View(bookdetails);
        }


        /// <summary>
        /// gets order details from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CustomerOrderDetail()
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
                    List<CartDetails> orderdetails = new List<CartDetails>();
                    RepositoryOrderDetails orderDetails = new RepositoryOrderDetails();
                    orderdetails = orderDetails.GetOrderdetails(username);
                    return View(orderdetails);
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
        /// user will be able to change password
        /// </summary>
        /// <returns>chng password view</returns>
        [HttpGet]
        public ActionResult CustomerPasswordChange()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerPasswordChange(ChangePassword userPassword)
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
                    RepositoryCustomerDetails userChangePassword = new RepositoryCustomerDetails();
                    if (userChangePassword.ChangeUserPassword(userPassword, username))
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
        /// removing item from the cart
        /// </summary>
        /// <param name="cartid"></param>
        /// <returns>chng password view</returns>
        [HttpPost]
        public ActionResult DeleteCartItem(int bookid, int customerid)
        {
            try
            {
                RepositoryCartDetails deleteItem = new RepositoryCartDetails();
                deleteItem.deleteCartDetails(bookid, customerid);
                return RedirectToAction("CustomerCart", "Customer");
            }
            catch (Exception ex)
            {
                LogExceptions logExceptions = new LogExceptions();
                logExceptions.LogException(ex);
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }
    }
}
/*
 * LoginController.cs - Controller class responsible for managing traffic from the view to service classes and vice versa in regards to user login. 
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using CLC.Services.Business;
using System.Linq;
using System.Web.Mvc;

namespace CLC.Controllers
{
    public class LoginController : Controller
    {
        // Index method is the default when the user visits this controller.
        [HttpGet]
        public ActionResult Index()
        {
            return View("Login");
        }

        // Login button was clicked.
        [HttpPost]
        public ActionResult doLogin(LoginRequest loginRequest)
        {
            // Create an instance of the LoginResponse object.
            LoginResponse response;

            if (ModelState.IsValid)
            {
                // Create an instance of the SecurityService class.
                SecurityService ss = new SecurityService();

                // Call the Authnticate method within the SecurityService class and save the return in the response variable.
                response = ss.Authenticate(loginRequest);

                if (response.Success)
                {
                    // Load user model 
                    UserService userService = new UserService();
                    var user = userService.loadUser(loginRequest);
                    Session["user"] = user;
                    return View("LoginPassed", loginRequest);
                }
            }
            else
            {
                string errors = string.Join("<br/> ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                response = new LoginResponse(false, errors);
            }
            return View("LoginFailed", response);
        }
    }
}
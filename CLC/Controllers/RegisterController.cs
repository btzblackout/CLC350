using CLC.Models;
using CLC.Services.Business;
/*
 * RegisterController.cs - Controller class responsible for managing traffic from the view to service classes and vice versa in regards to user registration. 
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using System.Linq;
using System.Web.Mvc;

namespace CLC.Controllers
{
    public class RegisterController : Controller
    {
        // Index method is the default when the user visits this controller.
        [HttpGet]
        public ActionResult Index()
        {
            return View("Register");
        }

        // Return back to the login screen.
        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }

        // Register the user.
        [HttpPost]
        public ActionResult doRegister(RegisterRequest registerRequest)
        {
            // Create an instance of the RegisterSecurityService and RegisterResponse classes.
            RegisterSecurityService ss = new RegisterSecurityService();
            RegisterResponse response;

            if (ModelState.IsValid)
            {
                // Call the Authenticate method in the RegisterSecurityService class and save the return in the response variable.
                response = ss.Authenticate(registerRequest);

                // If the registration was a success.
                if (response.Success)
                {
                    return View("RegisterPassed", registerRequest);
                }
            }
            else
            {
                string errors = string.Join("<br/> ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));
                response = new RegisterResponse(false, errors);
            }
            return View("RegisterFailed", response);

        }

    }
}
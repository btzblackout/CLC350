/*
 * RegisterSecurityService.cs - Service class responsible for authenticating user registration as well as calling the RegisterSecurityDAO.
 * Authors - Martin, Ryan, and Raymond.
 * Date - 06/25/2023
 */
using CLC.Models;
using CLC.Services.Data;

namespace CLC.Services.Business
{
    public class RegisterSecurityService
    {
        // Authenticate the user.
        public RegisterResponse Authenticate(RegisterRequest registerRequest)
        {
            // Create an instance of the RegisterResponse object for returning.
            RegisterResponse response = new RegisterResponse();

            // Set Success to false by default.
            response.Success = false;

            // Create an instance of the RegisterSecurityDAO.
            RegisterSecurityDAO dataService = new RegisterSecurityDAO();

            // Check to see if the user exists.
            if (dataService.userExists(registerRequest))
            {
                response.Message = "Username already exists.";
            }
            // Check to see if the email exists.
            else if (dataService.emailExists(registerRequest))
            {
                response.Message = "Email already in use.";
            }
            // If neither already exist then create the user
            else if (dataService.createUser(registerRequest))
            {
                response.Success = true;
            }

            // Return the response variable.
            return response;

        }

    }
}
/*
 * SecurityService.cs - Service class responsible for authenticating user login, as well as calling the SecurityDAO.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using CLC.Services.Data;

namespace CLC.Services.Business
{
    public class SecurityService
    {
        // Authenticate whether the user exists.
        public LoginResponse Authenticate(LoginRequest loginRequest)
        {
            // Create an instance of the LoginResponse object for return.
            LoginResponse response = new LoginResponse();

            // Set Success to false by default.
            response.Success = false;

            // Create an instance of the SecurityDAO.
            SecurityDAO dataService = new SecurityDAO();

            // Check if the user is valid.
            if (dataService.validUser(loginRequest))
            {
                response.Success = true;
            }
            else
            {
                response.Message = "Invalid username or password.";
            }

            // Return the response variable.
            return response;

        }

    }
}
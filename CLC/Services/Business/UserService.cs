/*
 * UserService.cs - Service class responsible for handling user login and user session, as well as calling the UserDAO.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using CLC.Services.Data;
using System;
using System.Web.Mvc;

namespace CLC.Services.Business
{
    public class UserService
    {

        // Logs in the user.
        public Boolean loggedIn(Controller c)
        {
            return c.Session["user"] != null;
        }

        // Loads the user.
        public User loadUser(LoginRequest loginRequest)
        {
            // Create an instance of the UserDAO.
            UserDAO userDAO = new UserDAO();

            // Call the findUser method in the UserDAO and return the result.
            return userDAO.findUser(loginRequest);
        }



    }
}
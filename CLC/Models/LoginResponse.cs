﻿/*
 * LoginResponse.cs - LoginResponse model class.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using System;

namespace CLC.Models
{
    public class LoginResponse
    {
        // Declare the variables.
        private Boolean success;
        private String message;

        // Create the constructors.
        public LoginResponse()
        {
        }
        public LoginResponse(bool passed, string message)
        {
            this.success = passed;
            this.message = message;
        }

        // Create the public getters and setters.
        public bool Success { get => success; set => success = value; }
        public string Message { get => message; set => message = value; }

    }
}
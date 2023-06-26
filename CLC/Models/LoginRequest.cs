/*
 * LoginRequest.cs - LoginRequest model class.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using System.ComponentModel.DataAnnotations;

namespace CLC.Models
{
    public class LoginRequest
    {
        // Declare the variables.
        private string username;
        private string password;

        // Create the constructors.
        public LoginRequest()
        {
        }
        public LoginRequest(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        // Create the public getters and setters
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string Username { get => username; set => username = value; }


        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        [DataType(DataType.Password)]
        public string Password { get => password; set => password = value; }

    }
}
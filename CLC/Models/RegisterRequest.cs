/*
 * RegisterRequest.cs - RegisterRequest model class.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using System.ComponentModel.DataAnnotations;

namespace CLC.Models
{
    public class RegisterRequest
    {
        // Declare the variables.
        private string username;
        private string password;
        private string email;
        private string firstName;
        private string lastName;
        private string sex;
        private int? age;
        private string state;

        // Create the constructors.
        public RegisterRequest()
        {
            this.username = "";
            this.password = "";
            this.email = "";
            this.firstName = "";
            this.lastName = "";
            this.sex = "";
            this.age = 0;
            this.state = "";
        }
        public RegisterRequest(string username, string password, string email, string firstName, string lastName, string sex, int age, string state)
        {
            this.username = username;
            this.password = password;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.sex = sex;
            this.age = age;
            this.state = state;
        }

        // Create the public getters and setters.
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string Username { get => username; set => username = value; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        [DataType(DataType.Password)]
        public string Password { get => password; set => password = value; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get => email; set => email = value; }

        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string FirstName { get => firstName; set => firstName = value; }

        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string LastName { get => lastName; set => lastName = value; }

        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string Sex { get => sex; set => sex = value; }

        [RegularExpression(@"^[0-9]+$")]
        public int? Age { get => age; set => age = value; }

        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string State { get => state; set => state = value; }

    }
}
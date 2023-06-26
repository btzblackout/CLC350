/*
 * Error.cs - Error model class.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
namespace CLC.Models
{
    public class Error
    {
        // Declare the variable.
        private string content;

        // Create the constructor.
        public Error(string content)
        {
            this.content = content;
        }

        // Public getter and setter.
        public string Content { get => content; set => content = value; }

    }
}
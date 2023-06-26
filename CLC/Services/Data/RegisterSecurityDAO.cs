/*
 * RegisterSecurityDAO.cs - Data Access Object (DAO) class responsible for providing user registration security and saving new users into the DB.
 * Authors - Martin, Ryan, and Raymond.
 * Date - 06/25/2023
 */
using System.Data.SqlClient;
using System.Data;
using CLC.Models;

namespace CLC.Services.Data
{
    public class RegisterSecurityDAO
    {
        // Database connection string.
        string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // Check if the username already exists.
        public bool userExists(RegisterRequest registerRequest)
        {
            // Create a boolean variable for returning.
            bool result = false;

            try
            {
                // String to SELECT * FROM dbo.users based on provided username.
                string query = "SELECT * FROM dbo.users WHERE USERNAME=@Username";

                // Standard db connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = registerRequest.Username;

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // If reader.HasRows == true then the user exists.
                    if (reader.HasRows)
                        result = true;
                    else
                        result = false;

                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            // Return the result variable.
            return result;
        }

        // Check if the email already exists.
        public bool emailExists(RegisterRequest registerRequest)
        {
            // Create a boolean variable for returning.
            bool result = false;

            try
            {
                // String to SELECT * FROM dbo.users based on the provided email.
                string query = "SELECT * FROM dbo.users WHERE EMAIL=@Email";

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = registerRequest.Email;

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // If reader.HasRows == true then the email exists.
                    if (reader.HasRows)
                        result = true;
                    else
                        result = false;

                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            // Return the result variable.
            return result;
        }

        // Create a new user in the DB.
        public bool createUser(RegisterRequest registerRequest)
        {
            // Create a boolean variable for returning.
            bool result = false;

            try
            {
                // Strintg to INSERT INTO dbo.users based on provided user information.
                string query = "INSERT INTO dbo.users (USERNAME, PASSWORD, EMAIL, FIRSTNAME, LASTNAME, SEX, AGE, STATE) " +
                    "VALUES (@Username, @Password, @Email, @FirstName, @LastName, @Sex, @Age, @State)";

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 20).Value = registerRequest.Username;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 20).Value = registerRequest.Password;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = registerRequest.Email;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 20).Value = registerRequest.FirstName ?? "N/A";
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 20).Value = registerRequest.LastName ?? "N/A";
                    cmd.Parameters.Add("@Sex", SqlDbType.VarChar, 20).Value = registerRequest.Sex ?? "N/A";
                    cmd.Parameters.Add("@Age", SqlDbType.Int, 11).Value = registerRequest.Age ?? 0;
                    cmd.Parameters.Add("@State", SqlDbType.VarChar, 20).Value = registerRequest.State ?? "N/A";

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    int rows = cmd.ExecuteNonQuery();

                    // If rows == 1 (rows affected) the user was successfully added.
                    if (rows == 1)
                        result = true;
                    else
                        result = false;

                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            // Return the results variable.
            return result;
        }
    }
}
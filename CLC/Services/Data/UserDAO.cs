/*
 * UserDAO.cs - Data Access Object (DAO) class responsible for retrieving users from the DB for logging in.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using System;
using System.Data.SqlClient;
using System.Data;


namespace CLC.Services.Data
{
    public class UserDAO
    {

        // Database connection string.
        string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // Return a single user.
        public User findUser(LoginRequest loginRequest)
        {
            // Create a User object for returning.
            User user = null;

            try
            {
                // String to SELECT * FROM dbo.users based on the provided user information.
                string query = "SELECT * FROM dbo.users WHERE USERNAME=@Username AND PASSWORD=@Password";

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = loginRequest.Username;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = loginRequest.Password;

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // While there are records to read.
                    while (reader.Read())
                    {
                        // Store database values in variables.
                        int ID = int.Parse(reader["ID"].ToString());
                        String username = reader["USERNAME"].ToString();
                        String password = reader["PASSWORD"].ToString();
                        String email = reader["EMAIL"].ToString();
                        String firstname = reader["FIRSTNAME"].ToString();
                        String lastname = reader["LASTNAME"].ToString();
                        String sex = reader["SEX"].ToString();
                        int age = int.Parse(reader["AGE"].ToString());
                        String state = reader["STATE"].ToString();

                        // Use variables to create a new User object and save it to user.
                        user = new User(ID, username, password, email, firstname, lastname, sex, age, state);                     
                    }

                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Return the user variable.
            return user;
        }
    }
}
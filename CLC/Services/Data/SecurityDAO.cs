/*
 * SecurityDAO.cs - Data Access Object (DAO) class responsible for providing user login security.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CLC.Services.Data
{
    public class SecurityDAO
    {
        // Database connection string.
        string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // Check if the user exists.
        public Boolean validUser(LoginRequest loginRequest)
        {
            // Create a boolean variable for returning.
            bool result = false;

            try
            {
                // String to SELECT * FROM dbo.users
                string query = "SELECT * FROM dbo.users WHERE USERNAME=@Username AND PASSWORD=@Password";

                // Standard db connection.
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

    }
}
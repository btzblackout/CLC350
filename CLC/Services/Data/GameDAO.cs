/*
 * GameDAO.cs - Data Access Object (DAO) class responsible for accessing and manipulating the data storage for everything involving the game itself.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;

namespace CLC.Services.Data.Game
{
    public class GameDAO
    {
        // Database connection string.
        string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /* Grid CRUD operations. */
        
        // Create grid.
        public void createGrid(Grid grid)
        {
            // gridID variable to hold query execution response. Default to -1.
            int gridID = -1;
            
            try
            {
                // String to INSERT into dbo.grids.
                string query = "INSERT INTO dbo.grids (ROWS, COLS, USERID, GAMEOVER) " +
                    "VALUES (@Rows, @Cols, @User_ID, @GameOver) SELECT SCOPE_IDENTITY()";

                //Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@Rows", SqlDbType.Int, 11).Value = grid.Rows;
                    cmd.Parameters.Add("@Cols", SqlDbType.Int, 11).Value = grid.Cols;
                    cmd.Parameters.Add("@User_ID", SqlDbType.Int, 11).Value = grid.Userid;
                    cmd.Parameters.Add("@GameOver", SqlDbType.Bit).Value = grid.GameOver;

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    gridID = Convert.ToInt32(cmd.ExecuteScalar());

                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }

            try
            {
                // String to INSERT into dbo.cells.
                string query = "INSERT INTO dbo.cells (X, Y, BOMB, VISITED, LIVENEIGHBORS, GRIDID, ISFLAGGED) " +
                    "VALUES (@x, @y, @bomb, @visited, @live, @grid, @isFlagged)";

                // For loops for traversing the grid.
                for (int y = 0; y < grid.Rows; y++)
                {
                    for (int x = 0; x < grid.Cols; x++)
                    {
                        //Standard DB connection.
                        using (SqlConnection cn = new SqlConnection(conn))
                        using (SqlCommand cmd = new SqlCommand(query, cn))
                        {
                            // Define @ variables for query.
                            cmd.Parameters.Add("@x", SqlDbType.Int, 11).Value = grid.Cells[x, y].X;
                            cmd.Parameters.Add("@y", SqlDbType.Int, 11).Value = grid.Cells[x, y].Y;
                            cmd.Parameters.Add("@bomb", SqlDbType.Bit).Value = grid.Cells[x, y].Bomb;
                            cmd.Parameters.Add("@visited", SqlDbType.Bit).Value = grid.Cells[x, y].Visited;
                            cmd.Parameters.Add("@live", SqlDbType.Int, 11).Value = grid.Cells[x, y].LiveNeighbors;
                            cmd.Parameters.Add("@grid", SqlDbType.Int, 11).Value = gridID;
                            cmd.Parameters.Add("@isFlagged", SqlDbType.Bit).Value = grid.Cells[x, y].IsFlagged;

                            // Open the connection.
                            cn.Open();

                            // Execute the DB command.
                            int rows = cmd.ExecuteNonQuery();

                            // Close the connection.
                            cn.Close();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
        
        // Find grid.
        public Grid findGrid(User user)
        {
            // g variable of type Grid for returning, defaulted to null.
            Grid g = null;
            try
            {
                // String to SELECT * FROM dbo.grids based on user ID.
                string query = "SELECT * FROM dbo.grids WHERE USERID=@id";

                //Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@id", SqlDbType.Int, 11).Value = user.Id;
                    
                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    // While there are records to read.
                    while (reader.Read())
                    {
                        // Store database values in variables.
                        int ID = int.Parse(reader["ID"].ToString());
                        int rows = int.Parse(reader["ROWS"].ToString());
                        int cols = int.Parse(reader["COLS"].ToString());
                        int USER_ID = int.Parse(reader["USERID"].ToString());
                        Boolean GAMEOVER = Boolean.Parse(reader["GAMEOVER"].ToString());
                    
                        // Use variables to create new Grid object, save to g.
                        g = new Grid(ID, rows, cols, USER_ID, GAMEOVER);

                        // Create new Cell[] for g.
                        g.Cells = new Cell[cols, rows];
                    }
                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Execute if g is not null
            if (g != null)
            {
                try
                {
                    // String to SELECT * FROM dbo.cells based on grid ID.
                    string query = "SELECT * FROM dbo.cells WHERE GRIDID=@id";

                    // Standard DB connection.
                    using (SqlConnection cn = new SqlConnection(conn))
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        // Define @ variables for query.
                        cmd.Parameters.Add("@id", SqlDbType.Int, 11).Value = g.Id;

                        // Open the connection.
                        cn.Open();

                        // Execute the DB command.
                        SqlDataReader reader = cmd.ExecuteReader();

                        // While there are records to read.
                        while (reader.Read())
                        {
                            // Store DB values in variables.
                            int ID = int.Parse(reader["ID"].ToString());
                            int x = int.Parse(reader["X"].ToString());
                            int y = int.Parse(reader["Y"].ToString());
                            Boolean bomb = Boolean.Parse(reader["BOMB"].ToString());
                            Boolean visited = Boolean.Parse(reader["VISITED"].ToString());
                            int live = int.Parse(reader["LIVENEIGHBORS"].ToString());
                            int gridId = int.Parse(reader["GRIDID"].ToString());
                            Boolean isFlagged = Boolean.Parse(reader["ISFLAGGED"].ToString());

                            // Create a new Cell object and set values
                            Cell c = new Cell(x, y);
                            c.Id = ID;
                            c.Bomb = bomb;
                            c.Visited = visited;
                            c.LiveNeighbors = live;
                            g.Cells[x, y] = c;
                            c.IsFlagged = isFlagged;

                        }
                        // Close the connection.
                        cn.Close();
                    }
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
            // Return the Grid object g.
            return g;
        }

        // Update grid.
        public void updateGrid(Grid grid)
        {
            try
            {
                // String to UPDATE dbo.grids based on provided grid.
                string query = "UPDATE dbo.grids SET ROWS = @Rows, COLS = @Cols, USERID = @User_ID, GAMEOVER = @GameOver WHERE ID=@id";

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@Rows", SqlDbType.Int, 11).Value = grid.Rows;
                    cmd.Parameters.Add("@Cols", SqlDbType.Int, 11).Value = grid.Cols;
                    cmd.Parameters.Add("@User_ID", SqlDbType.Int, 11).Value = grid.Userid;
                    cmd.Parameters.Add("@GameOver", SqlDbType.Bit).Value = grid.GameOver;
                    cmd.Parameters.Add("@id", SqlDbType.Int, 11).Value = grid.Id;

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    cmd.ExecuteNonQuery();

                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }

            try
            {
                // String to UPDATE dbo.cells based on provided grid's Cells.
                string query = "UPDATE dbo.cells SET X = @x, Y = @y, BOMB = @bomb, VISITED = @visited, LIVENEIGHBORS = @live, " +
                    "GRIDID = @grid, ISFLAGGED = @isFlagged WHERE ID=@id";

                // For loops for traversing the grid.
                for (int y = 0; y < grid.Rows; y++)
                {
                    for (int x = 0; x < grid.Cols; x++)
                    {
                        // Standard DB connection.
                        using (SqlConnection cn = new SqlConnection(conn))
                        using (SqlCommand cmd = new SqlCommand(query, cn))
                        {
                            // Define @ variables for query.
                            cmd.Parameters.Add("@x", SqlDbType.Int, 11).Value = grid.Cells[x, y].X;
                            cmd.Parameters.Add("@y", SqlDbType.Int, 11).Value = grid.Cells[x, y].Y;
                            cmd.Parameters.Add("@bomb", SqlDbType.Bit).Value = grid.Cells[x, y].Bomb;
                            cmd.Parameters.Add("@visited", SqlDbType.Bit).Value = grid.Cells[x, y].Visited;
                            cmd.Parameters.Add("@live", SqlDbType.Int, 11).Value = grid.Cells[x, y].LiveNeighbors;
                            cmd.Parameters.Add("@grid", SqlDbType.Int, 11).Value = grid.Id;
                            cmd.Parameters.Add("@id", SqlDbType.Int, 11).Value = grid.Cells[x, y].Id;
                            cmd.Parameters.Add("@isFlagged", SqlDbType.Bit).Value = grid.Cells[x, y].IsFlagged;

                            // Open the connection.
                            cn.Open();

                            // Execute the DB command.
                            int rows = cmd.ExecuteNonQuery();

                            // Close the connection.
                            cn.Close();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        // Delete grid.
        public void deleteGrid(User user)
        {
            try
            {
                // String to DELETE from dbo.grids based on the provided user ID.
                string query = "DELETE FROM dbo.grids WHERE USERID=@Id ";

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.AddWithValue("@Id", user.Id);

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    cmd.ExecuteNonQuery();

                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /* Saved Game CRUD operations. */

        // Save game.
        public void saveGame(string serializedGame, int userId)
        {
            try
            {
                // String to INSERT INTO dbo.SavedGames based on the provided user ID, current date, and serialized game.
                string query = "INSERT INTO dbo.SavedGames (UserId, Date, SavedGame) VALUES (@userId, @date, @savedGame)";

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@savedGame", SqlDbType.VarChar, 500).Value = serializedGame;

                    // Open the connection.
                    cn.Open();
                    
                    // Execute the DB command.
                    cmd.ExecuteNonQuery();

                    // Close the connection.
                    cn.Close();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        // Load all saved games.
        public List<SavedGame> loadUserGames(User user)
        {
            //Create the list of SavedGame objects to return.
            List<SavedGame> savedGames = new List<SavedGame>();

            try
            {
                // String to SELECT * FROM dbo.SavedGames based on the provided user ID.
                string query = "SELECT * FROM dbo.SavedGames WHERE UserId = @userId";

                // Create a placeholder SavedGame object.
                SavedGame currentSavedGame;

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = user.Id;

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Create an XmlSerializer object for deserialization.
                    XmlSerializer deserializer = new XmlSerializer(typeof(SavedGame));

                    // While there are records to read.
                    while (reader.Read())
                    {
                        // Deserialize the XML.
                        using (TextReader tr = new StringReader((string)reader[3]))
                        {
                            currentSavedGame = (SavedGame)deserializer.Deserialize(tr);
                        }

                        // Re-set the Id of the savedgame.
                        currentSavedGame.id = (int)reader[0];
                        savedGames.Add(currentSavedGame);
                    }
                    // Close the connection.
                    cn.Close();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            // Return the list of SavedGame objects.
            return savedGames;
        }

        // Play saved game.
        public void playSavedGame(int userID, int gridID, int rows, int cols)
        {
            try
            {
                // String to UPDATE dbo.grids based on provided userID, rows, cols, and gridID.
                string query = "UPDATE dbo.grids SET ID=@id, ROWS = @Rows, COLS = @Cols WHERE USERID=@User_ID";

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@Rows", SqlDbType.Int, 11).Value = rows;
                    cmd.Parameters.Add("@Cols", SqlDbType.Int, 11).Value = cols;
                    cmd.Parameters.Add("@User_ID", SqlDbType.Int, 11).Value = userID;
                    cmd.Parameters.Add("@id", SqlDbType.Int, 11).Value = gridID;

                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    cmd.ExecuteNonQuery();

                    // Close the connection.
                    cn.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        // Delete saved game.
        public void deleteSavedGame(int Id)
        {
            // String to DELETE from dbo.SavedGames based on provided Id.
            string query = "DELETE FROM dbo.SavedGames WHERE Id = @Id";

            // Standard DB connection.
            using (SqlConnection cn = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                // Define @ variables for query.
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;

                // Open the connection.
                cn.Open();

                // Execute the DB command.
                cmd.ExecuteNonQuery();

                // Close the connection.
                cn.Close();
            }
        }

        /* Saved Game API operations. */

        // Return all saved games.
        public List<SavedGame> AllGames()
        {
            // Create the list of games to return.
            List<SavedGame> savedGames = new List<SavedGame>();

            try
            {
                // String to SELECT * FROM dbo.SavedGames.
                string query = "SELECT * FROM dbo.SavedGames";

                // Create a placeholder SavedGame object.
                SavedGame currentSavedGame;

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Create an XmlSerializer object for deserialization.
                    XmlSerializer deserializer = new XmlSerializer(typeof(SavedGame));

                    // While there are records to read.
                    while (reader.Read())
                    {
                        // Deserialize the XML.
                        using (TextReader tr = new StringReader((string)reader[3]))
                        {
                            currentSavedGame = (SavedGame)deserializer.Deserialize(tr);
                        }
                        // Re-set the Id of the savedgame.
                        currentSavedGame.id = (int)reader[0];

                        // Add to the savedGames list.
                        savedGames.Add(currentSavedGame);
                     }
                    // Close the connection.
                    cn.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            // Return the list of SavedGame objects.
            return savedGames;
        }

        // Return a single saved game by Id.
        public SavedGame GetGameById(int id)
        {

            // Create a savedGame object for returning.
            SavedGame savedGame = new SavedGame();

            try
            {
                // String to SELECT * FROM dbo.SavedGames based on the Id provided.
                string query = "SELECT * FROM dbo.SavedGames WHERE Id = @gameId";

                // Standard DB connection.
                using (SqlConnection cn = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Define @ variables for query.
                    cmd.Parameters.Add("@gameId", SqlDbType.Int).Value = id;
                    
                    // Open the connection.
                    cn.Open();

                    // Execute the DB command.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Create an XmlSerializer object for deserialization.
                    XmlSerializer deserializer = new XmlSerializer(typeof(SavedGame));

                    // While there are records to read.
                    while (reader.Read())
                    {
                        // Deserialize the XML.
                        using (TextReader tr = new StringReader((string)reader[3]))
                        {
                            savedGame = (SavedGame)deserializer.Deserialize(tr);
                        }

                        // Re-set the Id of the savedgame.
                        savedGame.id = (int)reader[0];
                    }
                    // Close the connection.
                    cn.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            // Return the SavedGame object.
            return savedGame;
        }

        // Delete a single saved game by Id.
        public List<SavedGame> DeleteGameById(int id)
        {
            // Call the deleteSavedGame method to delete the game from the DB.
            deleteSavedGame(id);

            // Send the remaining games back.
            return AllGames();
        }
    }
}
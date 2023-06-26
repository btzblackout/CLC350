/*
 * GameService.cs - Service class responsible for managing the state of various game aspects as well as calls to the GameDAO.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using CLC.Services.Data.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace CLC.Services.Business.Game
{
    public class GameService
    {
        // Play a saved game.
        public void playSavedGame(int userID, int gridID, int rows, int cols)
        {
            // Create an instance of the GameDAO.
            GameDAO gameDAO = new GameDAO();

            // Call the playSavedGame method within GameDAO.
            gameDAO.playSavedGame(userID, gridID, rows, cols);
        }

        // Delete a saved game.
        public void deleteSavedGame(int gameId)
        {
            // Create an instance of GameDAO.
            GameDAO gameDAO = new GameDAO();

            // Call the deleteSavedGame method within GameDAO.
            gameDAO.deleteSavedGame(gameId);
        }

        // Save a game.
        public void saveGame(Controller c)
        {
            // Grab the user.
            User user = (User)c.Session["user"];
            
            // Create an instance of the GameDAO.
            GameDAO gameDAO = new GameDAO();
            
            // Grab the grid.
            Grid grid = gameDAO.findGrid(user);
            
            // Create an empty SavedGame object.
            SavedGame gameToSave = new SavedGame();

            // Populate the gameToSave.
            gameToSave.userId = user.Id;
            gameToSave.gridId = grid.Id;
            gameToSave.rows = grid.Rows;
            gameToSave.cols = grid.Cols;
            gameToSave.date = DateTime.Now;

            // Create the serialized string variable.
            string serializedGame = string.Empty;

            // Create an XmlSerializer object for serialization.
            XmlSerializer serializer = new XmlSerializer(gameToSave.GetType());
            
            // Use the XmlSerializer to serialize the object to a string.
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, gameToSave);
                serializedGame = sw.ToString();
            }

            // Call the method saveGame in GameDAO and pass the appropriate parameters.
            gameDAO.saveGame(serializedGame, user.Id); 
        }

        // Load all saved games.
        public List<SavedGame> loadGames(Controller c)
        {
            // Get the current user.
            User user = (User)c.Session["user"];

            // Create instance of GameDAO
            GameDAO gameDAO = new GameDAO();

            // Call the loadUserGames method, passing the appropriate parameters and saving the return to the savedGames list.
            List<SavedGame> savedGames = gameDAO.loadUserGames(user);

            // Return the savedGames list.
            return savedGames;
        }

        // Find a grid.
        public Grid findGrid(Controller c)
        {
            // Get the current user.
            User user = (User)c.Session["user"];

            // Create an instance of the GameDAO.
            GameDAO gameDAO = new GameDAO();

            // Return the result of calling the GameDAO method findGrid.
            return gameDAO.findGrid(user);

        }

        // Remove a grid.
        public void removeGrid(Controller c)
        {
            // Get the current user.
            User user = (User)c.Session["user"];

            // Create an instance of the GameDAO.
            GameDAO gameDAO = new GameDAO();

            // Call the deleteGrid method in the GameDAO.
            gameDAO.deleteGrid(user);

        }

        // Activate a cell.
        public void activateCell(Grid g, int X, int Y)
        {

            // this will make every cell that has been click on as active and show its value, will them push updated cells and grid to DB
            // Create an instance of the GameDAO
            GameDAO gameDAO = new GameDAO();

            // Set the current cell's visited state to true.
            g.Cells[X, Y].Visited = true;

            // Check to see if the cell is a bomb.
            if (g.Cells[X, Y].Bomb)
            {
                // Reveals whole grid.
                for (int y = 0; y < g.Rows; y++)
                {
                    for (int x = 0; x < g.Cols; x++)
                    {
                        g.Cells[x, y].Visited = true;
                    }
                }
                // Set GameOver for the game to true.
                g.GameOver = true;

                System.Diagnostics.Debug.WriteLine("Hit bomb at: " + X + ", " + Y);
            }
            else
            {
                // Check if LiveNeighbors == 0.
                if (g.Cells[X, Y].LiveNeighbors == 0)

                    // Reveal surrounding cells.
                    revealSurroundingCells(g, g.Cells[X, Y].X, g.Cells[X, Y].Y);

                // Check if game has been won.
                if (gameWon(g))
                {
                    // Reveals whole grid.
                    for (int y = 0; y < g.Rows; y++)
                    {
                        for (int x = 0; x < g.Cols; x++)
                        {
                            g.Cells[x, y].Visited = true;
                        }
                    }

                    // Set GameOver for the game to true.
                    g.GameOver = true;
                }
            }
            // Call the updateGrid method within GameDAO.
            gameDAO.updateGrid(g);
        }

        // Activate a flag for a cell.
        public void activateFlag(Grid g, int X, int Y)
        {

            // Toggle the value of IsFlagged
            // TODO: toggle not working properly, toggle will work in view but then revert back to inital state? leaving as it for now but once cell is flagged it can't 
            // be unflagged, however, flag also now acts as proper cell and can still be  clicked to reveil bomb or cell value as normal
            // g.Cells[X, Y].IsFlagged = !g.Cells[X, Y].IsFlagged;

            // Create an instance of GameDAO.
            GameDAO gameDAO = new GameDAO();

            // Set the cell's flagged value to true.
            g.Cells[X, Y].IsFlagged = true;

            // Call the updateGrid method within GameDAO.
            gameDAO.updateGrid(g);

            


        }

        // Deactivate a flag for a cell.
        public void deactivateFlag(Grid g, int X, int Y)
        {

            // Toggle the value of IsFlagged
            // TODO: toggle not working properly, toggle will work in view but then revert back to inital state? leaving as it for now but once cell is flagged it cant 
            // be unflagged, however, flag also now acts as proper cell and can still be clicked to reveil bomb or cell value as normal
            // g.Cells[X, Y].IsFlagged = !g.Cells[X, Y].IsFlagged;

            // Create an instance of GameDAO.
            GameDAO gameDAO = new GameDAO();

            // Set the cell's flagged value to false.
            g.Cells[X, Y].IsFlagged = false;

            // Call the updateGrid method within GameDAO.
            gameDAO.updateGrid(g);
        }

        // Check if the game has been won.
        private Boolean gameWon(Grid g)
        {
            // loops through every cell and checks
            // if there's still an unvisited cell that
            // isn't a bomb.
            for (int y = 0; y < g.Rows; y++)
            {
                for (int x = 0; x < g.Cols; x++)
                {
                    if (!g.Cells[x, y].Visited && !g.Cells[x, y].Bomb)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Reveal the surrounding cells.
        private void revealSurroundingCells(Grid g, int x, int y)
        {
            // Call ReveaNextCell on all cells around the current cell.
            RevealNextCell(g, x - 1, y - 1);
            RevealNextCell(g, x - 1, y);
            RevealNextCell(g, x - 1, y + 1);
            RevealNextCell(g, x + 1, y);
            RevealNextCell(g, x, y - 1);
            RevealNextCell(g, x, y + 1);
            RevealNextCell(g, x + 1, y - 1);
            RevealNextCell(g, x + 1, y + 1);
        }

        // Reveal the given cell.
        private void RevealNextCell(Grid g, int x, int y)
        {
            // Ensure the cell is on the grid.
            if (!(x >= 0 && x < g.Cols && y >= 0 && y < g.Rows)) return;

            // Check if the cell has already been visited.
            if (g.Cells[x, y].Visited) return;

            // Check if LiveNeighbors == 0.
            if (g.Cells[x, y].LiveNeighbors == 0)
            {
                g.Cells[x, y].Visited = true;
                revealSurroundingCells(g, x, y);
            }
            
            // Ensure the current cell is not a bomb.
            else if (!g.Cells[x, y].Bomb)
            {
                g.Cells[x, y].Visited = true;
            }

        }

        // Create a grid.
        public Grid createGrid(Controller c, int width, int height)
        {
            // Grab the current user.
            User user = (User)c.Session["user"];

            // Create a new grid object.
            Grid grid = new Grid(-1, width, height, user.Id, false);

            // Create a new Cell matrix.
            Cell[,] cells = new Cell[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[x, y] = new Cell(x, y);
                }
            }

            // Create a new instance of the Random object.
            Random rand = new Random();

            // Randomly distribute the bombs.
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (rand.Next(0, 100) <= 10)
                    {
                        cells[x, y].Bomb = true;
                        cells[x, y].LiveNeighbors = 9;
                        for (int neighborX = -1; neighborX <= 1; neighborX++)
                        {
                            for (int neighborY = -1; neighborY <= 1; neighborY++)
                            {
                                if (neighborX == 0 && neighborY == 0)
                                {

                                }
                                else if (x + neighborX >= 0 && x + neighborX < width && y + neighborY >= 0 && y + neighborY < height)
                                {
                                    cells[x + neighborX, y + neighborY].LiveNeighbors++;
                                }

                            }
                        }

                    }
                }
            }
            // Set the matrix of Cell objects to grid.Cells.
            grid.Cells = cells;

            // Create an instance of the GameDAO.
            GameDAO gameDAO = new GameDAO();

            // Call the createGrid method in the GameDAO.
            gameDAO.createGrid(grid);

            // Return the grid object.
            return grid;
        }
        
    }
}
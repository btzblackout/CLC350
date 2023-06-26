/*
 * GameController.cs - Controller class responsible for managing traffic from the view to service classes and vice versa in regards to the game.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using CLC.Services.Business;
using CLC.Services.Business.Game;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CLC.Controllers
{
    public class GameController : Controller
    {
        // Index method is the default when the user visits this controller.
        // GET: Game
        public ActionResult Index()
        {
            // Create an instance of the UserService class.
            UserService userService = new UserService();

            if (userService.loggedIn(this))
            { 
                GameService gameService = new GameService();

                Grid g = gameService.findGrid(this);

                if (g != null)
                {
                    // todo

                }
                else
                {
                    // Generate a grid for user
                    g = gameService.createGrid(this, 10, 10);
                }

                // Return game board view with grid model
                return View("Game", g);
            }

            else
            {
                // User not logged in
                Error e = new Error("You must be logged in to access this page.");

                return View("Error", e);
            }
        }
        
        // Method to activate a cell.
        [HttpPost]
        public ActionResult activateCell(string id, string x, string y)
        { 
            // Create an instnace of the UserService class.
            UserService userService = new UserService();

            // Check if user is logged in.
            if (userService.loggedIn(this))
            {
                // Update cell 
                GameService gameService = new GameService();

                // Load user grid.
                Grid g = gameService.findGrid(this);

                // Activate cell.
                gameService.activateCell(g, int.Parse(x), int.Parse(y));

                // Return same view.
                return PartialView("GameBoard", g);
            }
            else
            {
                
                Error e = new Error("You must be logged in to access this page.");

                return View("Error", e);
            }
        }

        // Method to activate a flag.
        [HttpPost]
        public ActionResult activateFlag(string id, string x, string y)
        {
            // Create an instance of the userService object.
            UserService userService = new UserService();

            // Check if user is logged in.
            if (userService.loggedIn(this))
            {
                // Update cell. 
                GameService gameService = new GameService();

                // Load user grid.
                Grid g = gameService.findGrid(this);

                // Activate cell.
                gameService.activateFlag(g, int.Parse(x), int.Parse(y));

                // Return same view.
                return PartialView("GameBoard", g);
            }
            else
            {

                Error e = new Error("You must be logged in to access this page.");

                return View("Error", e);
            }
        }

        // Method to deactivate flag.
        [HttpPost]
        public ActionResult deactivateFlag(string id, string x, string y)
        {
            // Create an instance of the UserService class.
            UserService userService = new UserService();

            // Check if user is logged in.
            if (userService.loggedIn(this))
            {
                // Update cell.
                GameService gameService = new GameService();

                // Load user grid. 
                Grid g = gameService.findGrid(this);

                // Activate cell.
                gameService.deactivateFlag(g, int.Parse(x), int.Parse(y));

                // Return same view.
                return PartialView("GameBoard", g);
            }
            else
            {

                Error e = new Error("You must be logged in to access this page.");

                return View("Error", e);
            }
        }

        // Method to reset the grid.
        [HttpGet]
        public ActionResult resetGrid()
        {
            // Create an instance of the GameService object.
            GameService gameService = new GameService();

            // Call the removeGrid method in the GameService class.
            gameService.removeGrid(this);

            // Returns view
            return Index();
        }

        // Save button was clicked.
        [HttpPost]
        public ActionResult onSaveButton()
        {
            // Create an instance of the GameService
            GameService service = new GameService();

            // Call the saveGame and removeGrid methods in the GameService class.
            service.saveGame(this);
            service.removeGrid(this);
            return Index();
        }

        // Load button was clicked.
        [HttpGet]
        public ActionResult onLoadButton()
        {
            // Create an instance of the GameService
            GameService service = new GameService();

            //Create a list of SavedGame objects
            List<SavedGame> savedGames;

            //Call the loadGames method in the GameService and save the return in the savedGames variable.
            savedGames = service.loadGames(this);
            return View(savedGames);
        }

        // Delete a saved game.
        [HttpPost]
        public ActionResult deleteSavedGame(int id)
        {
            // Create an instance of the GameService.
            GameService gameService = new GameService();

            // Call the deleteSavedGame method in the GameService.
            gameService.deleteSavedGame(id);

            // Redirect to the desired action or view.
            return RedirectToAction("Index");
        }

        // Play a specific saved game.
        public ActionResult PlaySavedGame(int userID, int gridID, int rows, int cols)
        {
            // Create an instance of the GameService.
            GameService gameService = new GameService();

            // Call the playSavedGame method in the GameService.
            gameService.playSavedGame(userID, gridID, rows, cols);

            //Call the findGrid method in the GameService and save the return in a variable of type Grid called g.
            Grid g = gameService.findGrid(this);

            // Return game board view with grid model.
            return View("Game", g);
        }
    }
}
/*
 * GameApiController.cs - Controller class responsible for recieving and routing API calls for saved games.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using CLC.Models;
using CLC.Services.Data.Game;
using System.Collections.Generic;
using System.Web.Http;


namespace CLC.Controllers
{
    // Create a routeprefix for the API controls.
    [RoutePrefix("api")]
    public class GameApiController : ApiController
    {
        // Create an instance of the GameDAO.
        private GameDAO _dao;
        
        // Create the constructor.
        public GameApiController()
        {
            // Initialize the GameDAO object.
            _dao = new GameDAO();
        }

        // Get all the saved games.
        [Route("showSavedGames")]
        public IEnumerable<SavedGame> Getallgames()
        {
            return _dao.AllGames();
        }     
        
        // Get the saved game by the game Id.
        [Route("showSavedGames/{gameid:int}")]
        public SavedGame GetSavedGameById(int gameid)
        {
            return _dao.GetGameById(gameid);
        }

        // Deletes a game from the DAO
        [Route("deleteOneGame/{gameid:int}")]
        [HttpDelete]
        public IEnumerable<SavedGame> DeleteGameById(int gameid)
        {
            return _dao.DeleteGameById(gameid);
        }

    }
}
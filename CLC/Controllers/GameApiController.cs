using CLC.Models;
using CLC.Services.Business.Game;
using CLC.Services.Data.Game;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;


namespace CLC.Controllers
{
    [RoutePrefix("api")]
    public class GameApiController : ApiController
    {
        // Game Service and Save Game DAO
        private GameDAO _dao;
        private GameService _gameService = new GameService();

        // Intiate a new DAO
        public GameApiController()
        {
            _dao = new GameDAO();
        }

        //Get all the saved games
        [Route("showSavedGames")]
        public IEnumerable<SavedGame> Getallgames()
        {
            return _dao.AllGames();
        }     
        
        //Get the saved game by the game Id.
        [Route("showSavedGames/{gameid:int}")]
        public SavedGame GetSavedGameById(int gameid)
        {
            return _dao.GetGameById(gameid);
        }

        //Deletes a game from the DAO
        [Route("deleteOneGame/{gameid:int}")]
        [HttpDelete]
        public IEnumerable<SavedGame> DeleteGameById(int gameid)
        {
            return _dao.DeleteGameById(gameid);
        }

    }
}
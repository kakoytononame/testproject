using Microsoft.AspNetCore.Mvc;
using testproject.Entities;
using testproject.Models;

namespace testproject.Controllers
{
    public class GameController:ControllerBase
    {
        private readonly GameService _gameService;
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }
        [HttpGet("games")]
        public async Task<IActionResult> GetGames()
        {
            return Ok(await _gameService.GetGamesAsync());
        }
        [HttpGet("creategame")]
        public async Task<IActionResult> CreateGameAsinc(int ownerId)
        {
            return Ok(await _gameService.CreateGameAsinc(ownerId));
        }
        [HttpGet("joingame")]
        
        public async Task<IActionResult> JoinGameAsinc(int secondPlayerId,int gameId)
        {
            return Ok(await _gameService.JoinGameAsinc(secondPlayerId,gameId));
        }
        [HttpGet("dostep")]
        public async Task<IActionResult> DoStepAsinc(int gameId,int index,int value)
        {
            return Ok(await _gameService.DoStepAsinc(gameId,index,value));
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MyBGList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BoardGamesController : ControllerBase
    {
        private readonly ILogger<BoardGamesController> _logger;

        public BoardGamesController(ILogger<BoardGamesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetBoardGames")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public IEnumerable<BoardGame> Get()
        {
            return new[] {
                new BoardGame() {
                    Id = 1,
                    Name = "Axis & Allies",
                    Publisher = "Milton Bradley",
                    Year = 1981,
                    MinPlayers = Random.Shared.Next(4),
                    MaxPlayers = Random.Shared.Next(5, 9)
                },
                new BoardGame() {
                    Id = 2,
                    Name = "Citadel",
                    Publisher = "Hans im Glück",
                    Year = 2000,
                    MinPlayers = Random.Shared.Next(4),
                    MaxPlayers = Random.Shared.Next(5, 9)
                },
                new BoardGame() {
                    Id = 3,
                    Name = "Terraforming Mars",
                    Publisher = "FryxGames",
                    Year = 2016,
                    MinPlayers = Random.Shared.Next(4),
                    MaxPlayers = Random.Shared.Next(5, 9)
                }
            };
        }
    }
}
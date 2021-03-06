using Microsoft.AspNetCore.Mvc;

using MyBGList.DTO;

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
        public RestDTO<BoardGame[]> Get()
        {
            return new RestDTO<BoardGame[]>() 
            {
                Data = new BoardGame[] {
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
                },

                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null, "BoardGames", null, Request.Scheme)!,
                        "self",
                        "GET")
                }
            };
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using MinesweeperBackend.Controllers.GameControllerSchemas;

namespace MinesweeperBackend.Controllers {
    [ApiController]
    [Route("api/[controller]/[action]/{params?}")]
    public class GameController : ControllerBase {

        private BadRequestObjectResult Error(string message) {
            return BadRequest(new { error = message });
        }

        [HttpPost]
        public IActionResult New(NewGameRequest model) {
            GameState.StartNewGame(model.Width, model.Height, model.Mines_count);

            return Ok(GameState.CurrentGame);
        }

        [HttpPost]
        public IActionResult Turn(GameTurnRequest model) {
            if (GameState.CurrentGame == null) return Error("Игра еще не начата. Сначала начните новую игру.");
            if (GameState.CurrentGame.Game_id != model.Game_id) return Error("Неверный идентификатор игры.");
            if (GameState.CurrentGame.Completed) return Error("Игра уже окончена. Начните новую игру.");


            bool canTurn = GameState.CurrentGame.Turn(model.Col, model.Row);
            if (!canTurn) return Error("Нельзя открыть эту ячейку.");

            return Ok(GameState.CurrentGame);
        } 
    }
}

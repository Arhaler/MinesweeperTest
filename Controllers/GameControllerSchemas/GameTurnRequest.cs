namespace MinesweeperBackend.Controllers.GameControllerSchemas {
    public class GameTurnRequest {
        public Guid Game_id { get; set; }
        public int Col { get; set; }
        public int Row { get; set; }
    }
}

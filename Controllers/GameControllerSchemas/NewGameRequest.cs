﻿namespace MinesweeperBackend.Controllers.GameControllerSchemas {
    public class NewGameRequest {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mines_count { get; set; }
    }
}

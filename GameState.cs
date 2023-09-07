using MinesweeperBackend.Models;

namespace MinesweeperBackend {
    public static class GameState {
        public static int GamesPlayed { get; private set; }
        public static Game? CurrentGame { get; private set; }

        public static void StartNewGame(int cols, int rows, int mines) {
            CurrentGame = new(cols, rows, mines);
        }
    }
}

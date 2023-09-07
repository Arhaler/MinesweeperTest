using System.Drawing;
using System.Reflection;

namespace MinesweeperBackend.Models {
    public class Game {
        public Guid Game_id { get; private set; }

        public bool Completed { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Mines_count { get; private set; }
        public string[,] Field { get; private set; }

        
        private readonly int minWidth = 2;
        private readonly int minHeight = 2;
        private readonly int maxWidth = 30;
        private readonly int maxHeight = 30;
        private readonly int minMines = 2;
        private readonly int maxMines;
        private readonly string[,] minesField;

        private int openedCells;

        public Game(int cols, int rows, int mines) {
            Game_id = Guid.NewGuid();

            Width = Math.Max(minWidth, Math.Min(maxWidth, cols));
            Height = Math.Max(minHeight, Math.Min(maxHeight, rows));
            maxMines = Width * Height - 1;
            Mines_count = Math.Max(minMines, Math.Min(maxMines, mines));

            Random random = new();

            Field = new string[Height, Width];
            minesField = new string[Height, Width];

            for (int c = 0; c < Width; c++) {
                for (int r = 0; r < Height; r++) {
                    Field[r, c] = " ";
                }
            }

            
            int generatedMines = 0;
            while (generatedMines < Mines_count) {
                int col = random.Next(0, Width);
                int row = random.Next(0, Height);

                if (minesField[row, col] != "X") {
                    minesField[row, col] = "X";
                    generatedMines++;
                } 
            }
        }


        /// <summary>
        /// Открывает соседние ячейки около ячейки находящейся по указанным координатам и указывает количество мин вокруг нее
        /// </summary>
        /// <param name="col">Колонка исходной ячейки</param>
        /// <param name="row">Ряд исходной ячейки</param>
        /// <param name="win">Если true, то все ячейки с минами будут изменены на M</param>
        private void OpenNeighbors(int col, int row) {
            openedCells++;
            int minesNeighbors = 0;

            List<int[]> neighBors = new();

            // проверяем ячейки по соседству
            for (int c = -1; c <= 1; c ++) {
                for (int r = -1; r <= 1; r ++) {
                    if (r == 0 && c == 0) continue;
                    if (row + r < 0 || col + c < 0) continue;
                    if (row + r > Field.GetLength(0) - 1 || col + c > Field.GetLength(1) - 1) continue;

                    neighBors.Add(new int[] { row + r, col + c });

                    // увеличиваем счетчик мин вокруг для исходной ячейки, если ячейка является миной
                    if (minesField[row + r, col + c] == "X") {
                        minesNeighbors++;
                    }
                }
            }
            Field[row, col] = minesNeighbors.ToString();

            // если для открытия больше нет ячеек без мин, то открываем оставшееся поле и выходим из метода
            if (Height * Width - openedCells == Mines_count) {
                OpenField(true);
                return;
            }

            // если мин среди соседей не найдено, то рекурсивно проверяем еще закрытые соседние ячейки
            if (minesNeighbors == 0) {
                for (int i = 0; i < neighBors.Count; i++) {
                    if (Field[neighBors[i][0], neighBors[i][1]] == " ") {
                        OpenNeighbors(neighBors[i][1], neighBors[i][0]);
                    }
                }
            }
        }


        /// <summary>
        /// Открывает еще закрытые ячейки, если игра выиграна, то мины меняются на M
        /// </summary>
        private void OpenField(bool win) {
            Completed = true;

            for (int col = 0; col < Width; col++) {
                for (int row = 0; row < Height; row++) {
                    if (Field[row, col] == " ") {
                        int minesNeighbors = 0;

                        // проверяем ячейки по соседству
                        for (int c = -1; c <= 1; c++) {
                            for (int r = -1; r <= 1; r++) {
                                if (r == 0 && c == 0) continue;
                                if (row + r < 0 ||col + c < 0) continue;
                                if (row + r > Field.GetLength(0) - 1 || col + c > Field.GetLength(1) - 1) continue;

                                // увеличиваем счетчик мин вокруг для исходной ячейки, если ячейка является миной
                                if (minesField[row + r, col + c] == "X") {
                                    minesNeighbors++;
                                }
                            }
                        }

                        // открываем ячейку с миной и меняем их на M, если игра была выиграна
                        if (win && minesField[row, col] == "X") {
                            Field[row, col] = "M";
                        }
                        // открываем ячейку с миной, если игра была проиграна
                        if (!win && minesField[row, col] == "X") {
                            Field[row, col] = "X";
                        }
                    } 
                }
            }
        }


        /// <summary>
        /// Если возможно открыть указанную ячейку, то открывает ее и соседние ячейки, если выбранная ячейка не являлась миной и игра не была окончена
        /// </summary>
        /// <param name="col">Колонка ячейки</param>
        /// <param name="row">Ряд ячейки</param>
        /// <returns>True, если ячейка была открыта, иначе False</returns>
        public bool Turn(int col, int row) {
            bool canTurn = CanTurn(col, row);

            if (canTurn) {
                if (minesField[row, col] == "X") {
                    OpenField(false);
                } else {
                    OpenNeighbors(col, row);
                }
            }

            return canTurn;
        }


        /// <summary>
        /// Проверяет возможность открытия ячейки по указанным координатам
        /// </summary>
        /// <param name="col">Колонка ячейки</param>
        /// <param name="row">Ряд ячейки</param>
        /// <returns>True, если возможно открыть ячейку по указаннам координатам, иначе False</returns>
        public bool CanTurn(int col, int row) {
            return  Width > col && col >= 0 && Height > row && row >= 0 && Field[row, col] == " ";
        }
    }
}

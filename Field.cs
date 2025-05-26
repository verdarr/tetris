using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class Field
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private int[,] grid;
        public Figure CurrentFigure { get; private set; }
        private Figure nextFigure;
        private Random random;

        public Field(int width, int height)
        {
            Width = width;
            Height = height;
            grid = new int[height, width];
            random = new Random();
            nextFigure = GenerateRandomFigure();
            SpawnFigure();
        }

        private Figure GenerateRandomFigure()
        {
            Array values = Enum.GetValues(typeof(TetrominoType));
            TetrominoType randomShape = (TetrominoType)values.GetValue(random.Next(values.Length));
            return new Figure(randomShape, new Point(Width / 2 - 1, 0));
        }

        public void SpawnFigure()
        {
            CurrentFigure = nextFigure;
            nextFigure = GenerateRandomFigure();
            CurrentFigure.Position = new Point(Width / 2 - 1, 0);
        }

        public bool CheckCollision()
        {
            int[,] figureMatrix = CurrentFigure.GetCurrentMatrix();

            for (int i = 0; i < figureMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < figureMatrix.GetLength(1); j++)
                {
                    if (figureMatrix[i, j] == 1)
                    {
                        int x = CurrentFigure.Position.X + j;
                        int y = CurrentFigure.Position.Y + i;

                        if (x < 0 || x >= Width || y >= Height)
                            return true;

                        if (y >= 0 && grid[y, x] == 1)
                            return true;
                    }
                }
            }

            return false;
        }

        public void MergeFigure()
        {
            int[,] figureMatrix = CurrentFigure.GetCurrentMatrix();

            for (int i = 0; i < figureMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < figureMatrix.GetLength(1); j++)
                {
                    if (figureMatrix[i, j] == 1)
                    {
                        int x = CurrentFigure.Position.X + j;
                        int y = CurrentFigure.Position.Y + i;

                        if (y >= 0)
                            grid[y, x] = 1;
                    }
                }
            }
        }

        public int ClearFullLines()
        {
            int linesCleared = 0;

            for (int y = Height - 1; y >= 0; y--)
            {
                bool lineFull = true;

                for (int x = 0; x < Width; x++)
                {
                    if (grid[y, x] == 0)
                    {
                        lineFull = false;
                        break;
                    }
                }

                if (lineFull)
                {
                    linesCleared++;

                    for (int ny = y; ny > 0; ny--)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            grid[ny, x] = grid[ny - 1, x];
                        }
                    }

                    for (int x = 0; x < Width; x++)
                    {
                        grid[0, x] = 0;
                    }

                    y++;
                }
            }

            return linesCleared;
        }

        public Figure GetNextFigure()
        {
            return nextFigure;
        }

        public int[,] GetGrid()
        {
            return (int[,])grid.Clone();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class Game
    {
        public Field Field { get; private set; }
        public int Score { get; private set; }
        public int Level { get; private set; }
        public bool IsGameOver { get; private set; }
        private IScoreManager scoreManager;
        private int linesCleared;
        private int linesToNextLevel;

        public Game(int width, int height, IScoreManager scoreManager)
        {
            Field = new Field(width, height);
            this.scoreManager = scoreManager;
            StartGame();
        }

        public void StartGame()
        {
            Score = 0;
            Level = 1;
            linesCleared = 0;
            linesToNextLevel = 5;
            IsGameOver = false;
            Field = new Field(Field.Width, Field.Height);
        }

        public void EndGame()
        {
            IsGameOver = true;
        }

        public void Update()
        {
            if (IsGameOver) return;

            Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X, Field.CurrentFigure.Position.Y + 1);

            if (Field.CheckCollision())
            {
                Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X, Field.CurrentFigure.Position.Y - 1);
                Field.MergeFigure();

                int lines = Field.ClearFullLines();
                if (lines > 0)
                {
                    AddScore(lines);
                    linesCleared += lines;

                    if (linesCleared >= linesToNextLevel)
                    {
                        IncreaseLevel();
                        linesToNextLevel += 5;
                    }
                }

                Field.SpawnFigure();

                if (Field.CheckCollision())
                {
                    EndGame();
                }
            }
        }

        private void AddScore(int lines)
        {
            switch (lines)
            {
                case 1: Score += 100 * Level; break;
                case 2: Score += 300 * Level; break;
                case 3: Score += 500 * Level; break;
                case 4: Score += 800 * Level; break;
            }
        }

        public void IncreaseLevel()
        {
            Level++;
        }

        public void MoveFigureLeft()
        {
            if (IsGameOver) return;

            Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X - 1, Field.CurrentFigure.Position.Y);

            if (Field.CheckCollision())
            {
                Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X + 1, Field.CurrentFigure.Position.Y);
            }
        }

        public void MoveFigureRight()
        {
            if (IsGameOver) return;

            Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X + 1, Field.CurrentFigure.Position.Y);

            if (Field.CheckCollision())
            {
                Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X - 1, Field.CurrentFigure.Position.Y);
            }
        }

        public void RotateFigure()
        {
            if (IsGameOver) return;

            Field.CurrentFigure.Rotate();

            if (Field.CheckCollision())
            {
                // Попробуем сдвинуть фигуру, если после поворота она выходит за границы
                for (int i = 0; i < 3; i++)
                {
                    Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X - 1, Field.CurrentFigure.Position.Y);
                    if (!Field.CheckCollision()) return;
                }

                Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X + 3, Field.CurrentFigure.Position.Y);
                if (Field.CheckCollision())
                {
                    Field.CurrentFigure.Rotate();
                    Field.CurrentFigure.Rotate();
                    Field.CurrentFigure.Rotate();
                }
            }
        }

        public void DropFigure()
        {
            if (IsGameOver) return;

            while (!Field.CheckCollision())
            {
                Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X, Field.CurrentFigure.Position.Y + 1);
            }

            Field.CurrentFigure.Position = new Point(Field.CurrentFigure.Position.X, Field.CurrentFigure.Position.Y - 1);
            Update();
        }

        public void SaveHighScore(string playerName)
        {
            scoreManager.SaveScore(playerName, Score);
        }
    }
}

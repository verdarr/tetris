using Timer = System.Windows.Forms.Timer;

namespace WinFormsApp1
{
    public class TetrisForm : Form
    {
        private Game game = null!;
        private Timer gameTimer = null!;
        private ScoreManager scoreManager = null!;
        private const int CellSize = 30;
        private const int FieldWidth = 10;
        private const int FieldHeight = 20;
        private const int InfoPanelWidth = 150;
        private bool isPaused = false;

        public TetrisForm()
        {
            InitializeGame();
            InitializeForm();
            InitializeTimer();
            this.DoubleBuffered = true;
        }

        private void InitializeGame()
        {
            scoreManager = new ScoreManager();
            game = new Game(FieldWidth, FieldHeight, scoreManager);
        }

        private void InitializeForm()
        {
            this.Text = "Тетрис";
            this.ClientSize = new Size(FieldWidth * CellSize + InfoPanelWidth, FieldHeight * CellSize);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Paint += TetrisForm_Paint;
            this.KeyDown += TetrisForm_KeyDown;
        }

        private void InitializeTimer()
        {
            gameTimer = new Timer();
            gameTimer.Interval = 1000 / 60; // Начальная скорость
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!isPaused && !game.IsGameOver)
            {
                game.Update();
                UpdateGameSpeed();
                this.Invalidate();
            }
        }

        private void UpdateGameSpeed()
        {
            // Уменьшаем интервал таймера с увеличением уровня
            gameTimer.Interval = Math.Max(50, 500 - (game.Level * 50));
        }

        private void TetrisForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawField(g);
            DrawCurrentFigure(g);
            DrawNextFigure(g);
            DrawInfo(g);

            if (game.IsGameOver)
            {
                DrawGameOver(g);
            }
            else if (isPaused)
            {
                DrawPause(g);
            }
        }

        private void DrawField(Graphics g)
        {
            // Рисуем сетку
            for (int x = 0; x < FieldWidth; x++)
            {
                for (int y = 0; y < FieldHeight; y++)
                {
                    Rectangle rect = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);
                    g.DrawRectangle(Pens.Gray, rect);
                }
            }

            // Рисуем заполненные клетки
            int[,] grid = game.Field.GetGrid();
            for (int y = 0; y < FieldHeight; y++)
            {
                for (int x = 0; x < FieldWidth; x++)
                {
                    if (grid[y, x] == 1)
                    {
                        Rectangle rect = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);
                        g.FillRectangle(Brushes.Black, rect);
                        g.DrawRectangle(Pens.Gray, rect);
                    }
                }
            }
        }

        private void DrawCurrentFigure(Graphics g)
        {
            if (game.Field.CurrentFigure == null) return;

            int[,] figureMatrix = game.Field.CurrentFigure.GetCurrentMatrix();
            Color color = game.Field.CurrentFigure.Color;

            for (int i = 0; i < figureMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < figureMatrix.GetLength(1); j++)
                {
                    if (figureMatrix[i, j] == 1)
                    {
                        int x = game.Field.CurrentFigure.Position.X + j;
                        int y = game.Field.CurrentFigure.Position.Y + i;

                        if (y >= 0)
                        {
                            Rectangle rect = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);
                            g.FillRectangle(new SolidBrush(color), rect);
                            g.DrawRectangle(Pens.Gray, rect);
                        }
                    }
                }
            }
        }

        private void DrawNextFigure(Graphics g)
        {
            Figure nextFigure = game.Field.GetNextFigure();
            if (nextFigure == null) return;

            int[,] figureMatrix = nextFigure.GetCurrentMatrix();
            Color color = nextFigure.Color;
            int startX = FieldWidth * CellSize + 20;
            int startY = 100;

            g.DrawString("Следующая:", this.Font, Brushes.Black, startX, startY - 30);

            for (int i = 0; i < figureMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < figureMatrix.GetLength(1); j++)
                {
                    if (figureMatrix[i, j] == 1)
                    {
                        Rectangle rect = new Rectangle(startX + j * CellSize, startY + i * CellSize, CellSize, CellSize);
                        g.FillRectangle(new SolidBrush(color), rect);
                        g.DrawRectangle(Pens.Gray, rect);
                    }
                }
            }
        }

        private void DrawInfo(Graphics g)
        {
            int startX = FieldWidth * CellSize + 20;
            int startY = 20;

            g.DrawString($"Очки: {game.Score}", this.Font, Brushes.Black, startX, startY);
            g.DrawString($"Уровень: {game.Level}", this.Font, Brushes.Black, startX, startY + 20);

            if (game.IsGameOver)
            {
                g.DrawString("Игра окончена", this.Font, Brushes.Red, startX, startY + 200);
                g.DrawString("Нажмите N для новой игры", this.Font, Brushes.Black, startX, startY + 220);
            }
            else if (isPaused)
            {
                g.DrawString("Пауза", this.Font, Brushes.Red, startX, startY + 200);
                g.DrawString("Нажмите P для продолжения", this.Font, Brushes.Black, startX, startY + 220);
            }
            else
            {
                g.DrawString("Управление:", this.Font, Brushes.Black, startX, startY + 200);
                g.DrawString("← → - двигать", this.Font, Brushes.Black, startX, startY + 220);
                g.DrawString("↑ - повернуть", this.Font, Brushes.Black, startX, startY + 240);
                g.DrawString("↓ - ускорить", this.Font, Brushes.Black, startX, startY + 260);
                g.DrawString("Пробел - сбросить", this.Font, Brushes.Black, startX, startY + 280);
                g.DrawString("P - пауза", this.Font, Brushes.Black, startX, startY + 300);
                g.DrawString("N - новая игра", this.Font, Brushes.Black, startX, startY + 320);
            }
        }

        private void DrawGameOver(Graphics g)
        {
            string gameOverText = "Игра окончена!";
            SizeF textSize = g.MeasureString(gameOverText, this.Font);
            float x = (FieldWidth * CellSize - textSize.Width) / 2;
            float y = (FieldHeight * CellSize - textSize.Height) / 2;

            g.FillRectangle(Brushes.White, x - 10, y - 10, textSize.Width + 20, textSize.Height + 20);
            g.DrawRectangle(Pens.Black, x - 10, y - 10, textSize.Width + 20, textSize.Height + 20);
            g.DrawString(gameOverText, this.Font, Brushes.Red, x, y);
        }

        private void DrawPause(Graphics g)
        {
            string pauseText = "Пауза";
            SizeF textSize = g.MeasureString(pauseText, this.Font);
            float x = (FieldWidth * CellSize - textSize.Width) / 2;
            float y = (FieldHeight * CellSize - textSize.Height) / 2;

            g.FillRectangle(Brushes.White, x - 10, y - 10, textSize.Width + 20, textSize.Height + 20);
            g.DrawRectangle(Pens.Black, x - 10, y - 10, textSize.Width + 20, textSize.Height + 20);
            g.DrawString(pauseText, this.Font, Brushes.Blue, x, y);
        }

        private void TetrisForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (game.IsGameOver)
            {
                if (e.KeyCode == Keys.N)
                {
                    game.StartGame();
                    gameTimer.Start();
                    this.Invalidate();
                }
                return;
            }

            if (e.KeyCode == Keys.P)
            {
                isPaused = !isPaused;
                this.Invalidate();
                return;
            }

            if (isPaused) return;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    game.MoveFigureLeft();
                    break;
                case Keys.Right:
                    game.MoveFigureRight();
                    break;
                case Keys.Down:
                    game.Update();
                    break;
                case Keys.Up:
                    game.RotateFigure();
                    break;
                case Keys.Space:
                    game.DropFigure();
                    break;
                case Keys.N:
                    game.StartGame();
                    break;
            }

            this.Invalidate();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (!game.IsGameOver && MessageBox.Show("Вы хотите сохранить результат?", "Тетрис",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var inputForm = new InputNameForm())
                {
                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        game.SaveHighScore(inputForm.PlayerName);
                    }
                }
            }
        }

    }
}
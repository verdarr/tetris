using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WinFormsApp1
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator System.Drawing.Point(Point v)
        {
            throw new NotImplementedException();
        }
    }

    public enum TetrominoType { I, J, L, O, S, T, Z }

    public class Figure
    {
        public TetrominoType Shape { get; private set; }
        public Point Position { get; set; }
        public Color Color { get; private set; }
        private int[,] matrix;

        public Figure(TetrominoType shape, Point position)
        {
            Shape = shape;
            Position = position;
            Color = GenerateColor(shape);
            matrix = GetMatrix(shape);
        }

        private int[,] GetMatrix(TetrominoType type)
        {
            switch (type)
            {
                case TetrominoType.I:
                    return new int[4, 4] { { 0, 0, 0, 0 }, { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
                case TetrominoType.J:
                    return new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
                case TetrominoType.L:
                    return new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
                case TetrominoType.O:
                    return new int[2, 2] { { 1, 1 }, { 1, 1 } };
                case TetrominoType.S:
                    return new int[3, 3] { { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } };
                case TetrominoType.T:
                    return new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
                case TetrominoType.Z:
                    return new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
                default:
                    return new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            }
        }

        private Color GenerateColor(TetrominoType type)
        {
            switch (type)
            {
                case TetrominoType.I: return Color.Cyan;
                case TetrominoType.J: return Color.Blue;
                case TetrominoType.L: return Color.Orange;
                case TetrominoType.O: return Color.Yellow;
                case TetrominoType.S: return Color.Green;
                case TetrominoType.T: return Color.Purple;
                case TetrominoType.Z: return Color.Red;
                default: return Color.White;
            }
        }

        public void Rotate()
        {
            int[,] newMatrix = new int[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    newMatrix[j, matrix.GetLength(0) - 1 - i] = matrix[i, j];
                }
            }

            matrix = newMatrix;
        }

        public int[,] GetCurrentMatrix()
        {
            return (int[,])matrix.Clone();
        }
    }
}

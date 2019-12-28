using System;

namespace NeuralNetworks
{
    public class Matrix : ICloneable
    {
        #region Operators
        public static Matrix operator +(Matrix matrix, float value)
        {
            matrix = new Matrix(matrix);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    matrix[row, col] += value;
                }
            }

            return matrix;
        }

        public static Matrix operator -(Matrix matrix, float value) => matrix +- value;

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (!EqualSize(a, b))
            {
                throw new ArithmeticException("Matrices a and b must have the same size for summation");
            }

            var matrix = new Matrix(a);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    matrix[row, col] += b[row, col];
                }
            }

            return matrix;
        }

        public static Matrix operator -(Matrix a, Matrix b) => a +- b;

        public static Matrix operator -(Matrix matrix)
        {
            matrix = new Matrix(matrix);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    matrix[row, col] *= -1;
                }
            }

            return matrix;
        }

        public static Matrix operator *(Matrix matrix, float value)
        {
            matrix = new Matrix(matrix);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    matrix[row, col] *= value;
                }
            }

            return matrix;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (!EqualSize(a, b))
            {
                throw new ArithmeticException("Matrices a and b must have the same size for multiplication");
            }

            var matrix = new Matrix(a);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    matrix[row, col] *= b[row, col];
                }
            }

            return matrix;
        }
        #endregion
        #region Static Methods
        public static Matrix GetRandomMatrix(int row, int col, int a = -1, int b = 1)
        {
            var matrix = new Matrix(row, col);
            var random = new Random();
            // Random value is in range [a, b]
            matrix.Map((x) => (float)(random.NextDouble() * (b - a) + a));
            return matrix;
        }

        public static Matrix MatMult(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows)
            {
                throw new ArithmeticException("");
            }

            var matrix = new Matrix(a.Rows, b.Columns);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    for (int i = 0; i < a.Columns; i++)
                    {
                        matrix[row, col] += a[row, i] * b[i, col];
                    }
                }
            }

            return matrix;
        }

        public static Matrix Transpose(Matrix matrix)
        {
            var mat = new Matrix(matrix.Columns, matrix.Rows);

            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    mat[col, row] = matrix[row, col];
                }
            }

            return mat;
        }

        public static Matrix Map(Matrix matrix, Func<float, float> func)
        {
            matrix = new Matrix(matrix);
            matrix.Map(func);
            return matrix;
        }

        public static bool EqualSize(Matrix a, Matrix b) => a.Rows == b.Rows && a.Columns == b.Columns;
        #endregion
        #region Fields
        public float this[int row, int col] { get => values[row, col]; set => values[row, col] = value; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        private float[,] values;
        #endregion
        #region Ctors
        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            values = new float[Rows, Columns];
        }

        public Matrix(float[,] values) : this(values.GetLength(0), values.GetLength(1))
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    this.values[row, col] = values[row, col];
                }
            }
        }

        public Matrix(Matrix matrix) : this(matrix.values) { }
        #endregion
        #region Methods
        public float[,] ToArray() => new Matrix(this).values;

        public void Map(Func<float, float> func)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    values[row,col] = func(values[row,col]);
                }
            }
        }

        public object Clone() => new Matrix(this);
        #endregion
    }
}

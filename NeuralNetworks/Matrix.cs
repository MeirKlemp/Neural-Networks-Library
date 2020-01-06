using System;

namespace NeuralNetworks
{
    public class Matrix : ICloneable
    {
        #region Operators
        public static Matrix operator +(Matrix matrix, double value)
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

        public static Matrix operator -(Matrix matrix, double value) => matrix +- value;

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

        public static Matrix operator *(Matrix matrix, double value)
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
        private static Random random = new Random();
        /// <summary>
        /// Sets the random's seed.
        /// </summary>
        /// <param name="seed">The seed to set into the random.</param>
        public static void SetRandomSeed(int seed)
        {
            random = new Random(seed);
        }

        /// <summary>
        /// Creates a matrix with a random values.
        /// </summary>
        /// <param name="row">The new matrix's amount of rows.</param>
        /// <param name="col">The new matrix's amount of columns.</param>
        /// <param name="minValue">The inclusive minimum value of the random values.</param>
        /// <param name="maxValue">The exlusive maximum value of the random values.</param>
        /// <returns>The new matrix with the random values.</returns>
        public static Matrix GetRandomMatrix(int row, int col, int minValue = -1, int maxValue = 1)
        {
            var matrix = new Matrix(row, col);
            // Random value is in range [a, b)
            matrix.Map((x) => random.NextDouble() * (maxValue - minValue) + minValue);
            return matrix;
        }

        /// <summary>
        /// Calculates the matrix that is being created through the matrix multiplication of two given matrices.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns>The calculated matrix from the matrix multiiplication from the two given matrices.</returns>
        public static Matrix MatMult(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows)
            {
                throw new ArithmeticException("For matrix multiplication matrix a's amount of columns must be equals to matrix b's amount of rows.");
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

        /// <summary>
        /// Transposes a given matrix.
        /// </summary>
        /// <param name="matrix">The matrix to transpose.</param>
        /// <returns>The transposed matrix.</returns>
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

        /// <summary>
        /// Creates a clone of the given matrix, then runs the map method on it.
        /// </summary>
        /// <param name="matrix">The matrix to clone.</param>
        /// <param name="func">The function for the map.</param>
        /// <returns>The mapped matrix.</returns>
        public static Matrix Map(Matrix matrix, Func<double, double> func)
        {
            matrix = new Matrix(matrix);
            matrix.Map(func);
            return matrix;
        }

        public static bool EqualSize(Matrix a, Matrix b) => a.Rows == b.Rows && a.Columns == b.Columns;
        #endregion
        #region Fields
        /// <summary>
        /// An implementation of [] operators for the class in order to access into the matrix's values easly.
        /// </summary>
        /// <param name="row">The rows's index.</param>
        /// <param name="col">The column's index.</param>
        /// <returns></returns>
        public double this[int row, int col] { get => values[row, col]; set => values[row, col] = value; }
        /// <summary>
        /// The amount of the matrix's rows.
        /// </summary>
        public int Rows { get; private set; }
        /// <summary>
        /// The amount of the matrix's columns.
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        /// The matrix's values.
        /// </summary>
        private double[,] values;
        #endregion
        #region Ctors
        /// <summary>
        /// A constructor for creating an empty matrix with a specific size.
        /// </summary>
        /// <param name="rows">The new matrix's rows.</param>
        /// <param name="columns">The new matrix's columns.</param>
        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            values = new double[Rows, Columns];
        }

        /// <summary>
        /// A constructor for creating a matrix filled with a 2d array's size and values.
        /// </summary>
        /// <param name="values">The 2d array.</param>
        public Matrix(double[,] values) : this(values.GetLength(0), values.GetLength(1))
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    this.values[row, col] = values[row, col];
                }
            }
        }

        /// <summary>
        /// A constructor to clone a given matrix.
        /// </summary>
        /// <param name="matrix">The matrix to clone.</param>
        public Matrix(Matrix matrix) : this(matrix.values) { }
        #endregion
        #region Methods
        /// <summary>
        /// Runs every element of the matrix through a function.
        /// </summary>
        /// <param name="func">A function that gests a double and returns a double.</param>
        public void Map(Func<double, double> func)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    values[row,col] = func(values[row,col]);
                }
            }
        }

        /// <summary>
        /// The implementation of the ICloneable's Clone function.
        /// </summary>
        /// <returns>A cloned matrix object.</returns>
        public object Clone() => new Matrix(this);

        /// <summary>
        /// Converts the matrix into a 2d array.
        /// </summary>
        /// <returns>The 2d array of the matrix.</returns>
        public double[,] ToArray() => new Matrix(this).values;

        /// <summary>
        /// Convets the matrix into a representing string.
        /// </summary>
        /// <returns>The representing string.</returns>
        public override string ToString()
        {
            var str = "";

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    str += values[row, col];

                    if (col < Columns - 1)
                    {
                        str += ", ";
                    }
                }
                str += "\n ";
            }
            
            return str;
        }
        #endregion
    }
}

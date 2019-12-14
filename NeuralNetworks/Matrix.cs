using System;
using System.Collections.Generic;
using System.Text;

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
                    matrix[row][col] += value;
                }
            }

            return matrix;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (!EqualSize(a, b))
            {
                throw new ArithmeticException("Matrices a and b must have the same size");
            }

            var matrix = new Matrix(a);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    matrix[row][col] += b[row][col];
                }
            }

            return matrix;
        }
        #endregion
        #region Static Methods
        public static Matrix Transpose(Matrix matrix)
        {
            var mat = new Matrix(matrix.Columns, matrix.Rows);

            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    mat[col][row] = matrix[row][col];
                }
            }

            return mat;
        }

        public static Matrix Map(Matrix matrix, Func<double, double> func)
        {
            matrix = new Matrix(matrix);
            matrix.Map(func);
            return matrix;
        }

        public static bool EqualSize(Matrix a, Matrix b) => a.Rows == b.Rows && a.Columns == b.Columns;
        #endregion
        #region Fields
        public double[] this[int index] => values[index];
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        private double[][] values;
        #endregion
        #region Ctors
        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            values = new double[Rows][];
            for (int row = 0; row < Rows; row++)
            {
                values[row] = new double[Columns];
            }
        }

        public Matrix(double[][] values) : this(values.GetLength(0), values.GetLength(1))
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    this.values[row][col] = values[row][col];
                }
            }
        }

        public Matrix(Matrix matrix) : this(matrix.values) { }
        #endregion
        #region Methods
        public void Map(Func<double, double> func)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    values[row][col] = func(values[row][col]);
                }
            }
        }

        public object Clone()
        {
            return new Matrix(this);
        }
        #endregion
    }
}

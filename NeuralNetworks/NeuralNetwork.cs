using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworks
{
    /// <summary>
    /// This class represents a Neural Network.
    /// </summary>
    public class NeuralNetwork : ICloneable
    { 
        #region Fields
        /// <summary>
        /// The change rate of the weights when learning.
        /// </summary>
        public double LearningRate { get; set; }
        /// <summary>
        /// The layers of the neural networks. The output layers' length is in the last layer's NextSize property.
        /// </summary>
        public Layer[] Layers { get; set; }
        #endregion
        #region Ctors
        /// <summary>
        /// A ctor to create a network with given layers.
        /// </summary>
        /// <param name="layer">The network must contain at least 2 layers.</param>
        /// <param name="layers">Allows to add hidden layers</param>
        public NeuralNetwork(Layer layer, params Layer[] layers)
        {
            Layers = new Layer[layers.Length + 1];

            Layers[0] = new Layer(layer);
            for (int i = 1; i < Layers.Length; ++i)
            {
                Layers[i] = new Layer(layers[i - 1]);

                if (Layers[i].Size != Layers[i - 1].NextSize)
                {
                    throw new Exception("Layer's NextSize must be equals to the next layer's Size.");
                }
            }

            LearningRate = .1;
        }

        /// <summary>
        /// A ctor that takes an array of layers.
        /// </summary>
        /// <param name="layers">Array of layers</param>
        public NeuralNetwork(Layer[] layers)
        {
            Layers = new Layer[layers.Length];

            for (int i = 0; i < Layers.Length; ++i)
            {
                Layers[i] = new Layer(layers[i]);

                if (i > 0 && Layers[i].Size != Layers[i - 1].NextSize)
                {
                    throw new Exception("Layer's NextSize must be equals to the next layer's Size.");
                }
            }

            LearningRate = .1;
        }

        /// <summary>
        /// A ctor to clone a given network.
        /// </summary>
        /// <param name="net">The given network.</param>
        public NeuralNetwork(NeuralNetwork net) : this(net.Layers) { }
        #endregion
        #region Methods
        /// <summary>
        /// Calculates the output from a given input.
        /// </summary>
        /// <param name="input">The input for the networks.</param>
        /// <returns>The calculated output.</returns>
        public double[] GetPrediction(double[] input)
        {
            if (input.Length == Layers[0].Size)
            {
                var result = GetWeightsValues(ArrayToMatrix(input));

                return MatrixToArray(result[result.Count - 1]);
            }

            return null;
        }

        /// <summary>
        /// Fixes the weights and biases with the back propagation algorithm.
        /// </summary>
        /// <param name="input">The input for the network.</param>
        /// <param name="answer">The expected output.</param>
        public void Train(double[] input, double[] answer)
        {
            var result = GetWeightsValues(ArrayToMatrix(input));
            var error = ArrayToMatrix(answer) - result[result.Count - 1];

            for (int i = Layers.Length - 1; i >= 0; --i)
            {
                var gradient = Matrix.Map(result[i + 1], Layers[i].DerivationFunction) * error * LearningRate;
                var delta = Matrix.MatMult(gradient, Matrix.Transpose(result[i]));

                Layers[i].Weights += delta;
                Layers[i].Bias += gradient;

                error = Matrix.MatMult(Matrix.Transpose(Layers[i].Weights), error);
            }
        }

        /// <summary>
        /// Gets an input matrix and calclates the ouput matrix.
        /// </summary>
        /// <param name="input">The input matrix.</param>
        /// <returns>The output matrix.</returns>
        private List<Matrix> GetWeightsValues(Matrix input)
        {
            var result = new List<Matrix>(Layers.Length + 1) { input };
            var values = input;

            foreach (var layer in Layers)
            {
                values = Matrix.MatMult(layer.Weights, values) + layer.Bias;
                values.Map(layer.ActivationFunction);
                result.Add(values);
            }

            return result;
        }

        /// <summary>
        /// Transforms a given 1 dimensional array to matrix with 1 row.
        /// </summary>
        /// <param name="array">A 1 dimensional array to transform</param>
        /// <returns>The matrix with the array values.</returns>
        private Matrix ArrayToMatrix(double[] array)
        {
            var matArray = new double[array.Length, 1];
            for (int i = 0; i < array.Length; ++i)
            {
                matArray[i, 0] = array[i];
            }

            return new Matrix(matArray);
        }

        /// <summary>
        /// Transforms a given matrix (vector) with 1 row to 1 dimensional array.
        /// </summary>
        /// <param name="matrix">A metrix with 1 row (vector)</param>
        /// <returns>The aray with the matrix values.</returns>
        private double[] MatrixToArray(Matrix matrix)
        {
            var array = new double[matrix.Columns];

            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = matrix[i, 0];
            }

            return array;
        }

        /// <summary>
        /// Clones the neural networks
        /// </summary>
        /// <returns>An object of NeuralNetwork.</returns>
        public object Clone() => new NeuralNetwork(this);

        /// <summary>
        /// Parses a string into the weights.
        /// </summary>
        /// <param name="neurons">The string to parse.</param>
        public void SetNeuronsFromString(string neurons)
        {
            var values = neurons.Trim().Split('\n');

            int count = 0;
            foreach (var layer in Layers)
            {
                for (int row = 0; row < layer.Weights.Rows; ++row)
                {
                    for (int col = 0; col < layer.Weights.Columns; ++col)
                    {
                        ++count;
                    }
                }

                for (int row = 0; row < layer.Bias.Rows; ++row)
                {
                    for (int col = 0; col < layer.Bias.Columns; ++col)
                    {
                        ++count;
                    }
                }
            }

            if (values.Length != count)
            {
                throw new Exception("The number of values ​​that have been given is not equal to the number of neurons this network has.");
            }

            foreach (var layer in Layers)
            {
                for (int row = 0; row < layer.Weights.Rows; ++row)
                {
                    for (int col = 0; col < layer.Weights.Columns; ++col)
                    {
                        layer.Weights[row, col] = double.Parse(values[values.Length - count--]);
                    }
                }

                for (int row = 0; row < layer.Bias.Rows; ++row)
                {
                    for (int col = 0; col < layer.Bias.Columns; ++col)
                    {
                        layer.Bias[row, col] = double.Parse(values[values.Length - count--]);
                    }
                }
            }
        }

        /// <summary>
        /// Converts the data of the weights into a string.
        /// </summary>
        /// <returns>The string with the values of the weights.</returns>
        public override string ToString()
        {
            var result = "";

            foreach (var layer in Layers)
            {
                for (int row = 0; row < layer.Weights.Rows; ++row)
                {
                    for (int col = 0; col < layer.Weights.Columns; ++col)
                    {
                        result += layer.Weights[row, col] + "\n";
                    }
                }

                for (int row = 0; row < layer.Bias.Rows; ++row)
                {
                    for (int col = 0; col < layer.Bias.Columns; ++col)
                    {
                        result += layer.Bias[row, col];
                    }
                }
            }

            return result;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworks
{
    public class NeuralNetwork
    {
        public double LearningRate { get; set; }
        private Layer[] Layers { get; set; }

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

        public double[] GetPrediction(double[] input)
        {
            if (input.Length == Layers[0].Size)
            {
                var result = GetWeightsValues(ArrayToMatrix(input));

                return MatrixToArray(result[result.Count - 1]);
            }

            return null;
        }

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

        private Matrix ArrayToMatrix(double[] array)
        {
            var matArray = new double[array.Length, 1];
            for (int i = 0; i < array.Length; i++)
            {
                matArray[i, 0] = array[i];
            }

            return new Matrix(matArray);
        }

        private double[] MatrixToArray(Matrix matrix)
        {
            var array = new double[matrix.Columns];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = matrix[i, 0];
            }

            return array;
        }
    }
}

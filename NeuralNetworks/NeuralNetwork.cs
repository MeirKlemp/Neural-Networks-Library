using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworks
{
    public class NeuralNetwork
    {
        public float LearningRate { get; set; }
        private Layer[] Layers { get; set; }

        public NeuralNetwork(params Layer[] layers)
        {
            if (layers[layers.Length - 1].NextSize > 0)
            {
                Layers = new Layer[layers.Length + 1];
                Layers[Layers.Length - 1] = new Layer(layers[layers.Length - 1].NextSize);
            }
            else
            {
                Layers = new Layer[layers.Length];
            }

            for (int i = 0; i < layers.Length; ++i)
            {
                Layers[i] = (Layer)layers[i].Clone();
            }

            LearningRate = .1f;
        }

        public float[] GetPrediction(float[] input)
        {
            return null;
        }

        public void Train(float[] input, float answer)
        {

        }

        private Matrix GetWeightsValues(Matrix input)
        {
            return null;
        }
    }
}

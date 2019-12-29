using System;

namespace NeuralNetworks
{
    /// <summary>
    /// This class represent a layer in the neural networks.
    /// </summary>
    public class Layer : ICloneable
    {
        #region Fields
        /// <summary>
        /// The size of the layer.
        /// </summary>
        public int Size { get; private set; }
        /// <summary>
        /// The size of the next layer.
        /// </summary>
        public int NextSize { get; private set; }
        /// <summary>
        /// Activation function of this layer in the neural networks.
        /// </summary>
        public Func<double, double> ActivationFunction { get; private set; }
        /// <summary>
        /// Derivation function of the activation function of this layer, for learning.
        /// </summary>
        public Func<double, double> DerivationFunction { get; private set; }
        /// <summary>
        /// The weights connected between this layer and the next layer.
        /// </summary>
        public Matrix Weights { get; set; }
        /// <summary>
        /// The bias of the layer.
        /// </summary>
        public Matrix Bias { get; set; }
        #endregion
        #region Ctors
        /// <summary>
        /// The ctor of this calss.
        /// </summary>
        /// <param name="size">The size of the layer.</param>
        /// <param name="nextSize">The size of the next layer.</param>
        /// <param name="activation">Activation function of this layer</param>
        /// <param name="derivation">Derivation function of the activation function</param>
        public Layer(int size, int nextSize, Func<double, double> activation, Func<double, double> derivation)
        {
            Size = size;
            ActivationFunction = activation;
            DerivationFunction = derivation;
            NextSize = nextSize;

            if (NextSize > 0)
            {
                Weights = Matrix.GetRandomMatrix(NextSize, Size);
                Bias = Matrix.GetRandomMatrix(NextSize, 1);
            }
        }

        /// <summary>
        /// Ctor for cloning.
        /// </summary>
        /// <param name="clone"></param>
        public Layer(Layer clone)
        {
            Size = clone.Size;
            ActivationFunction = (Func<double, double>)clone.ActivationFunction.Clone();
            DerivationFunction = (Func<double, double>)clone.DerivationFunction.Clone();
            NextSize = clone.NextSize;

            if (NextSize > 0)
            {
                Weights = (Matrix)clone.Weights.Clone();
                Bias = (Matrix)clone.Bias.Clone();
            }
        }
        #endregion
        #region Methods
        public object Clone() => new Layer(this);
        #endregion
    }
}

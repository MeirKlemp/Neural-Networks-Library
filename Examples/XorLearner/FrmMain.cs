using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuralNetworks;
namespace XorLearner
{
    public partial class FrmMain : Form
    {
        public int SquaresResolusion { get; set; } = 10;
        public int SquaresRows => pbCanvas.Height / SquaresResolusion;
        public int SquaresCols => pbCanvas.Width / SquaresResolusion;
        public Random Random { get; set; } = new Random();

        public List<double>[,] Data { get; set; } =
        {
            { new List<double> { 0, 0 }, new List<double> { 0 } },
            { new List<double> { 0, 1 }, new List<double> { 1 } },
            { new List<double> { 1, 0 }, new List<double> { 1 } },
            { new List<double> { 1, 1 }, new List<double> { 0 } }
        };

        public NeuralNetwork Brain { get; set; }

        public FrmMain() => InitializeComponent();

        private void FrmMain_Load(object sender, EventArgs e)
        {
            NewBrain(this, EventArgs.Empty);
        }

        private void Update(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                var dataNum = Random.Next(4);
                Brain.Train(Data[dataNum, 0].ToArray(), Data[dataNum, 1].ToArray());
            }

            pbCanvas.Image = Draw();
        }

        private Image Draw()
        {
            var img = new Bitmap(pbCanvas.Width, pbCanvas.Height);

            using (var gfx = Graphics.FromImage(img))
            {
                for (int row = 0; row < SquaresRows; row++)
                {
                    for (int col = 0; col < SquaresCols; col++)
                    {
                        var val1 = (double)row / SquaresRows;
                        var val2 = (double)col / SquaresCols;
                        var output = Brain.GetPrediction(new double[] { val1, val2 })[0];

                        var color = (int)(255 * output);
                        gfx.FillRectangle(new SolidBrush(Color.FromArgb(color, color, color)), row * SquaresResolusion, col * SquaresResolusion, SquaresResolusion, SquaresResolusion);
                    }
                }
            }

            return img;
        }

        private void NewBrain(object sender, EventArgs e)
        {
            var sigmoid = new Func<double, double>((x) => 1 / (1 + Math.Exp(-x)));
            var dsigmoid = new Func<double, double>((y) => y * (1 - y));

            // 2 input, 2 hidden, 1 output.
            Brain = new NeuralNetwork(
                new Layer(2, 2, sigmoid, dsigmoid),
                new Layer(2, 1, sigmoid, dsigmoid)
            );

            pbCanvas.Image = Draw();
        }

        private void Start(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void Stop(object sender, EventArgs e)
        {
            timer.Stop();
        }
    }
}

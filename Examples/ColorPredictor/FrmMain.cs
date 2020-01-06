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

namespace ColorPredictor
{
    public partial class FrmMain : Form
    {
        public NeuralNetwork Brain { get; set; }
        public Dictionary<Color, int> Data { get; set; }

        private int correct;
        private double computerColor;
        private Color backColor;
        private Random random;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Data = new Dictionary<Color, int>();
            random = new Random();

            var sigmoid = new Func<double, double>((x) => 1 / (1 + Math.Exp(-x)));
            var dsigmoid = new Func<double, double>((y) => y * (1 - y));

            // 3 input, 2 hidden, 1 output.
            Brain = new NeuralNetwork(
                new Layer(3, 2, sigmoid, dsigmoid),
                new Layer(2, 1, sigmoid, dsigmoid)
            );

            NewBackColor();
            timer.Start();
        }

        private void Update(object sender, EventArgs e)
        {
            pbCanvas.Image = Draw();
        }

        private Image Draw()
        {
            if (pbCanvas.Width * pbCanvas.Height != 0)
            {
                var img = new Bitmap(pbCanvas.Width, pbCanvas.Height);

                using (var gfx = Graphics.FromImage(img))
                {
                    Color comChosen;
                    var comCircleSize = Math.Min(pbCanvas.Width, pbCanvas.Height) / 8;
                    var comCircle = new Rectangle(pbCanvas.Width / 4 - comCircleSize / 2, pbCanvas.Height * 2 / 3 - comCircleSize / 2, comCircleSize, comCircleSize);

                    if (computerColor > .5)
                    {
                        comChosen = Color.White;
                        comCircle.X += pbCanvas.Width / 2;
                    }
                    else
                    {
                        comChosen = Color.Black;
                    }

                    gfx.FillRectangle(new SolidBrush(backColor), pbCanvas.Bounds);

                    gfx.DrawLine(new Pen(Color.White, 3), pbCanvas.Width / 2, 0, pbCanvas.Width / 2, (int)(pbCanvas.Height * computerColor));
                    gfx.DrawLine(new Pen(Color.Black, 3), pbCanvas.Width / 2, (int)(pbCanvas.Height * computerColor), pbCanvas.Width / 2, pbCanvas.Height);
                    gfx.FillEllipse(new SolidBrush(comChosen), comCircle);

                    var sizeBlack = gfx.MeasureString("Black", new Font(Font.FontFamily, comCircleSize));
                    var sizeWhite = gfx.MeasureString("White", new Font(Font.FontFamily, comCircleSize));
                    gfx.DrawString("Black", new Font(Font.FontFamily, comCircleSize), Brushes.Black, new PointF(pbCanvas.Width / 4 - sizeBlack.Width / 2, pbCanvas.Height / 2 - sizeBlack.Height / 2));
                    gfx.DrawString("White", new Font(Font.FontFamily, comCircleSize), Brushes.White, new PointF(pbCanvas.Width * .75f - sizeWhite.Width / 2, pbCanvas.Height / 2 - sizeWhite.Height / 2));

                    if (Data.Count > 0)
                    {
                        var correctness = string.Format("Correct choice: {0:0.00}%", correct / (float)Data.Count * 100);
                        gfx.DrawString(correctness, new Font(Font.FontFamily, comCircleSize / 4), new SolidBrush(comChosen), new PointF());
                    }
                }

                return img;
            }

            return null;
        }

        private void NewBackColor()
        {
            backColor = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            computerColor = Brain.GetPrediction(new double[] { backColor.R / 255.0, backColor.G / 255.0, backColor.B / 255.0 })[0];
        }

        private void Learn()
        {
            for (int i = 0; i < 1000; i++)
            {
                int rand = random.Next(Data.Keys.Count);
                var color = Data.Keys.ElementAt(rand);
                var value = Data[color];
                Brain.Train(new double[] { color.R / 255.0, color.G / 255.0, color.B / 255.0 }, new double[] { value });
            }
        }

        private void pbCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            var chosen = e.X > pbCanvas.Width / 2 ? 1 : 0;

            if (computerColor < .5f && chosen < .5f || computerColor > .5f && chosen > .5f)
            {
                correct++;
            }

            if (!Data.ContainsKey(backColor))
            {
                Data.Add(backColor, chosen);
            }
            else
            {
                Data[BackColor] = chosen;
            }

            Learn();
            NewBackColor();
        }
    }
}

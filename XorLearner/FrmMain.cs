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
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            var a = new Matrix(2, 2);
            var b = new Matrix(2, 2);

            a[0, 0] = 1;
            a[0, 1] = 2;
            a[1, 0] = 3;
            a[1, 1] = 4;

            b[0, 0] = 1;
            b[0, 1] = 2;
            b[1, 0] = 3;
            b[1, 1] = 4;

            var mat = Matrix.MatMult(a, b);

            label1.Text = string.Format("{0} {1}\n{2} {3}", mat[0, 0], mat[0, 1], mat[1, 0], mat[1, 1]);
        }
    }
}

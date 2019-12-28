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
            var mat = Matrix.GetRandomMatrix(2, 2, 12, 33);
            label1.Text = string.Format("{0} {1}\n{2} {3}", mat[0, 0], mat[0, 1], mat[1, 0], mat[1, 1]);
        }
    }
}

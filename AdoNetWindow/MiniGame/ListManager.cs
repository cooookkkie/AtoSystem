using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.MiniGame
{
    public partial class ListManager : Form
    {
        public ListManager()
        {
            InitializeComponent();
        }

        private void btnBlcokBreaker_Click(object sender, EventArgs e)
        {
            BlockBreaker bb = new BlockBreaker();
            bb.Show();
        }

        private void btnTetris_Click(object sender, EventArgs e)
        {
            Tetris.Tetris tetris = new Tetris.Tetris();
            tetris.Show();
        }

        private void btnAvoidStar_Click(object sender, EventArgs e)
        {
            AvoidStar.AvoidStar avoidStar = new AvoidStar.AvoidStar();
            avoidStar.Show();
        }
    }
}

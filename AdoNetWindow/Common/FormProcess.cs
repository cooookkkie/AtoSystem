using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Common
{
    public partial class FormProcess : Form
    {
        int max;
        public FormProcess(int maxValue)
        {
            InitializeComponent();
            max = maxValue;
        }

        private void FormProcess_Load(object sender, EventArgs e)
        {
            progProcess.Maximum = max;
        }
        //진행율 표시 노출 매서트
        internal void SetProgress(int i)
        {
            double curRate = ((double)i / (double)max) * 100;

            progProcess.Value = i;
            lbProcessRate.Text = curRate.ToString("#,##0.00") + "%";
            Update();
        }

        internal int GetProgress()
        {
            return progProcess.Value;
        }
    }
}

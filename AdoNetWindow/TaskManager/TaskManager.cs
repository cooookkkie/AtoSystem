using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.TaskManager
{
    public partial class TaskManager : Form
    {
        UsersModel um;
        public TaskManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TaskDetailManager tdm = new TaskDetailManager(um);
            tdm.ShowDialog();
        }

        private void dgvTask_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

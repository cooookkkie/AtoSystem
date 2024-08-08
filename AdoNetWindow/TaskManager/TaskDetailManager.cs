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
    public partial class TaskDetailManager : Form
    {
        UsersModel um;
        public TaskDetailManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }
    }
}

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

namespace AdoNetWindow.Product
{
    public partial class IncomeList : Form
    {
        UsersModel um;
        public IncomeList(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void IncomeList_Load(object sender, EventArgs e)
        {
            dgvProduct.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvProduct.Columns["is_proceed"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvProduct.Columns["reason"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

        }

        private void dgvProduct_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void dgvProduct_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void dgvProduct_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {

        }

        
    }
}

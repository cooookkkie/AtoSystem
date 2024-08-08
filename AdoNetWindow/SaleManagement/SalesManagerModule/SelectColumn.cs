using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    public partial class SelectColumn : Form
    {
        string select_column_name = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();

        public SelectColumn(DataTable columnDt)
        {
            InitializeComponent();

            for(int i = 0; i < columnDt.Columns.Count; i++)
            {
                int n = dgvFax.Rows.Add();
                dgvFax.Rows[n].Cells["column_name"].Value  = columnDt.Columns[i].ColumnName;
            }
        }

        public string GetColumn()
        {
            this.ShowDialog();

            return this.select_column_name;
        }

        #region Datagridview event
        private void dgvFax_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                select_column_name = dgvFax.Rows[e.RowIndex].Cells["column_name"].Value.ToString();
                this.Dispose();
            }
        }

        private void dgvFax_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFax.CurrentRow != null)
            {
                foreach (DataGridViewRow row in dgvFax.Rows)
                    row.Cells["chk"].Value = false;

                dgvFax.Rows[dgvFax.CurrentRow.Index].Cells["chk"].Value = true;
                dgvFax.EndEdit();
            }
        }

        private void dgvFax_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvFax.Columns[e.ColumnIndex].Name.Equals("chk"))
                {
                    bool isChecked;
                    if (dgvFax.Rows[e.RowIndex].Cells["chk"].Value == null || !bool.TryParse(dgvFax.Rows[e.RowIndex].Cells["chk"].Value.ToString(), out isChecked))
                        isChecked = false;


                    if (isChecked)
                        dgvFax.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.Red;
                    else
                        dgvFax.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.White;
                }
            }
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            select_column_name = null;
            this.Dispose();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (dgvFax.CurrentRow == null)
            {
                messageBox.Show(this, "선택한 컬럼이 없습니다");
                return;
            }
            else
            {
                select_column_name = dgvFax.CurrentRow.Cells["column_name"].Value.ToString();
                this.Dispose();
            }
        }
        #endregion

        #region key event
        private void SelectColumn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode) 
                {
                    case Keys.Q:
                        btnSelect.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
        #endregion
    }
}

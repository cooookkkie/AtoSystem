using AdoNetWindow.Model;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Pending
{
    public partial class PendingView : Form
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        UsersModel um;
        string id;
        string type;
        public PendingView(UsersModel uModel, string custom_id, string sType)
        {
            InitializeComponent();
            um = uModel;
            id = custom_id;
            type = sType;
        }

        private void PendingView_Load(object sender, EventArgs e)
        {
            SetDgvHeaderSetting();
            GetData();
        }

        #region Method
        private void GetData()
        {
            //Call Unpending Data
            DataGridView dgv = dgvProduct;
            List<AllCustomsModel> model = customsRepository.GetAllTypePending(type, id);
            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    int n = dgv.Rows.Add();
                    
                    dgv.Rows[n].Cells["shipper"].Value = model[i].shipper;
                    dgv.Rows[n].Cells["product"].Value = model[i].product;
                    dgv.Rows[n].Cells["origin"].Value = model[i].origin;
                    dgv.Rows[n].Cells["sizes"].Value = model[i].sizes;

                    if (!string.IsNullOrEmpty(model[i].cost_unit))
                    {
                        dgv.Rows[n].Cells["box_weight"].Value = model[i].cost_unit;
                    }
                    else
                    {
                        dgv.Rows[n].Cells["box_weight"].Value = model[i].box_weight;
                    }
                    

                    dgv.Rows[n].Cells["etd"].Value = model[i].etd;
                    dgv.Rows[n].Cells["eta"].Value = model[i].eta;
                    dgv.Rows[n].Cells["warehousing_date"].Value = model[i].warehousing_date;
                    dgv.Rows[n].Cells["pending_date"].Value = model[i].pending_check;
                    dgv.Rows[n].Cells["contract_qty"].Value = Convert.ToDouble(model[i].quantity_on_paper).ToString("#,##0");
                    dgv.Rows[n].Cells["warehouse_qty"].Value = Convert.ToDouble(model[i].qty).ToString("#,##0");
                    lbManager.Text = "담당자 : " + model[i].manager;



                    if (dgv.Rows[n].Cells["box_weight"].Value == null || dgv.Rows[n].Cells["box_weight"].Value == "")
                    {
                        dgv.Rows[n].Cells["box_weight"].Value = "0";
                    }
                    if (dgv.Rows[n].Cells["contract_qty"].Value == null || dgv.Rows[n].Cells["contract_qty"].Value == "")
                    {
                        dgv.Rows[n].Cells["contract_qty"].Value = "0";
                    }
                    if (dgv.Rows[n].Cells["warehouse_qty"].Value == null || dgv.Rows[n].Cells["warehouse_qty"].Value == "")
                    {
                        dgv.Rows[n].Cells["warehouse_qty"].Value = "0";
                    }
                }
            }
            Calculate();
        }
        private void Calculate()
        {
            double box_weight = 0;
            double contract_qty = 0;
            double warehouse_qty = 0;
            if (dgvProduct.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    box_weight += Convert.ToDouble(row.Cells["box_weight"].Value) * Convert.ToDouble(row.Cells["contract_qty"].Value);
                    contract_qty += Convert.ToDouble(row.Cells["contract_qty"].Value);
                    warehouse_qty += Convert.ToDouble(row.Cells["warehouse_qty"].Value);
                }
            }
            txtTotalBoxWeight.Text = box_weight.ToString("#,##0.00");
            txtTotalContractQty.Text = contract_qty.ToString("#,##0");
            txtTotalWarehouseQty.Text = warehouse_qty.ToString("#,##0");
        }
        private void SetDgvHeaderSetting()
        {
            DataGridView dgv = dgvProduct;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            //Disable Sorting for DataGridView
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //색상
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["box_weight"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 255);
            dgv.Columns["box_weight"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["contract_qty"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["contract_qty"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["warehouse_qty"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["warehouse_qty"].HeaderCell.Style.ForeColor = Color.White;
        }
        #endregion

        #region Key event
        private void PendingView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        #endregion
    }
}

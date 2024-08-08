using AdoNetWindow.Model;
using Repositories.SalesPartner;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    public partial class CompanyManager : Form
    {
        ISalesRepository salesRepository = new SalesRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        DataGridViewCell cell = null;
        DailyBusiness db = null;
        UsersModel um;
        Libs.MessageBox messageBox = new Libs.MessageBox();

        public CompanyManager(UsersModel um, DailyBusiness db, DataGridViewCell cell)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;
            this.cell = cell;

            if (cell.Value == null)
                txtCompany.Text = "";
            else
                txtCompany.Text = cell.Value.ToString();

            btnSearching.PerformClick();
        }

        public CompanyManager(UsersModel um, DailyBusiness db)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;
        }

        #region Button
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvCompany.CurrentRow != null && dgvCompany.CurrentRow.Index >= 0)
            {
                int row_index = dgvCompany.CurrentRow.Index;

                if (db != null && cell != null)
                    db.SetCompany(dgvCompany.Rows[row_index].Cells["company_code"].Value.ToString(), cell);
                else if (db != null)
                    db.GetSalesProduct(dgvCompany.Rows[row_index].Cells["company_code"].Value.ToString());
            }
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            dgvCompany.Rows.Clear();
            //데이터 출력====================================================================================
            //매출처 정보
           DataTable companyDt = salesPartnerRepository.GetSalesPartner("", txtCompany.Text, false, "", "", "", "", true, false, false, false, false, false, false, false, ""
                                                                        , txtDistribution.Text, txtHandlingItem.Text, txtRemark2.Text);


            //씨오버 거래처 
            DataTable seaoverDt = salesRepository.GetSaleCompany(txtCompany.Text, false, "", "", "", "", "", "", true, true);

            //씨오버 거래정보 + 아토전산 거래처
            var seaoverTemp = from p in seaoverDt.AsEnumerable()
                              join t in companyDt.AsEnumerable()
                              on p.Field<string>("거래처코드") equals t.Field<string>("seaover_company_code")
                              into outer
                              from t in outer.DefaultIfEmpty()
                              select new
                              {
                                  id = (t == null) ? 0 : t.Field<Int32>("id"),
                                  company = p.Field<string>("거래처명"),
                                  seaover_company_code = p.Field<string>("거래처코드"),
                                  distribution = (t == null) ? "" : t.Field<string>("distribution"),
                                  handling_item = (t == null) ? "" : t.Field<string>("handling_item"),
                                  remark2 = (t == null) ? "" : t.Field<string>("remark2"),
                                  isHide = (t == null) ? "false" : t.Field<string>("isHide"),
                                  ato_manager = (t == null) ? p.Field<string>("매출자") : t.Field<string>("ato_manager"),
                                  isDelete = (t == null) ? "FALSE" : t.Field<string>("isDelete")
                              };
            DataTable seaoverDt1 = ConvertListToDatatable(seaoverTemp);

            if (seaoverDt1 != null &&seaoverDt1.Rows.Count > 0)
            {
                for (int i = 0; i < seaoverDt1.Rows.Count; i++)
                {
                    bool isOutput = false;

                    if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                    {
                        string[] distribution = txtDistribution.Text.Trim().Split(' ');
                        for (int j = 0; j < distribution.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(distribution[j].Trim()) && seaoverDt1.Rows[i]["distribution"].ToString().Contains(distribution[j]))
                            {
                                isOutput = true;
                                break;
                            }
                        }
                    }
                    else
                        isOutput = true;


                    if (isOutput && !string.IsNullOrEmpty(txtHandlingItem.Text.Trim()))
                    {
                        isOutput = false;
                        string[] handling_item = txtHandlingItem.Text.Trim().Split(' ');
                        for (int j = 0; j < handling_item.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(handling_item[j].Trim()) && seaoverDt1.Rows[i]["handling_item"].ToString().Contains(handling_item[j]))
                            {
                                isOutput = true;
                                break;
                            }
                        }
                    }

                    if (isOutput && !string.IsNullOrEmpty(txtRemark2.Text.Trim()))
                    {
                        isOutput = false;
                        string[] remark2 = txtRemark2.Text.Trim().Split(' ');
                        for (int j = 0; j < remark2.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(remark2[j].Trim()) && seaoverDt1.Rows[i]["remark2"].ToString().Contains(remark2[j]))
                            {
                                isOutput = true;
                                break;
                            }
                        }
                    }
                    //출력
                    if (isOutput)
                    {
                        int n = dgvCompany.Rows.Add();

                        dgvCompany.Rows[n].Cells["id"].Value = seaoverDt1.Rows[i]["id"].ToString();
                        dgvCompany.Rows[n].Cells["company_code"].Value = seaoverDt1.Rows[i]["seaover_company_code"].ToString();
                        dgvCompany.Rows[n].Cells["company"].Value = seaoverDt1.Rows[i]["company"].ToString();
                        dgvCompany.Rows[n].Cells["distribution"].Value = seaoverDt1.Rows[i]["distribution"].ToString();
                        dgvCompany.Rows[n].Cells["handling_item"].Value = seaoverDt1.Rows[i]["handling_item"].ToString();
                        dgvCompany.Rows[n].Cells["remark2"].Value = seaoverDt1.Rows[i]["remark2"].ToString();
                    }
                }
            }
            if (dgvCompany.Rows.Count > 0)
                dgvCompany.Focus();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void txtCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearching.PerformClick();
        }
        #endregion

        #region Method
        public static DataTable ConvertListToDatatable(IEnumerable<dynamic> v)
        {
            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();

            DataTable table = new DataTable();
            foreach (var info in infos)
            {
                Type propType = info.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                else
                    table.Columns.Add(info.Name, info.PropertyType);
            }

            DataRow row;

            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;
                }
                table.Rows.Add(row);
            }
            table.AcceptChanges();
            return table;
        }
        #endregion

        #region Key event
        Libs.Tools.Common common = new Libs.Tools.Common();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control tb = common.FindFocusedControl(this);
            if (tb != null && tb.Name != null && (tb.Name == "txtCompany" || tb.Name == "txtHandlingItem" || tb.Name == "txtDistribution" || tb.Name == "txtRemark2") && dgvCompany.Rows.Count > 0)
            {
                switch (keyData)
                {
                    case Keys.Down:
                        dgvCompany.Focus();
                        int idx = 0;
                        return true;
                    default:
                        return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        private void CompanyManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        btnSelect.PerformClick();
                        break;
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = String.Empty;
                        txtDistribution.Text = String.Empty;
                        txtHandlingItem.Text = String.Empty;
                        txtRemark2.Text = String.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                }
            }
        }

        #endregion

        #region Datagridview event
        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnSelect.PerformClick();
        }
        #endregion

        
    }
}

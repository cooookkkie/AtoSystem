using AdoNetWindow.Model;
using Repositories.ContractRecommendation;
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
    public partial class RecommendationUnit : UserControl
    {
        IContractRecommendationRepository contractRecommendationRepository = new ContractRecommendationRepository();
        UsersModel um;
        DataRow[] dt = null;
        public RecommendationUnit(UsersModel uModel, string product, string origin, DataRow[] dRow = null)
        {
            InitializeComponent();
            um = uModel;
            txtProduct.Text = product;
            txtOrigin.Text = origin;
            dt = dRow;
        }

        private void RecommendationUnit_Load(object sender, EventArgs e)
        {
            dgvMakePeriod.Rows.Add();
            dgvMakePeriod.ClearSelection();
            dgvMakePeriod.ReadOnly = true;
            dgvMakePeriod.Enabled = false;

            dgvContractPeriod.Rows.Add();
            dgvContractPeriod.ClearSelection();
            dgvContractPeriod.ReadOnly = true;
            dgvContractPeriod.Enabled = false;
            dgvContractPeriod.DefaultCellStyle.SelectionBackColor = Color.DarkGreen;
            GetData();
        }

        #region Method
        private void UpdateSetting()
        {
            dgvMakePeriod.ReadOnly = false;
            dgvMakePeriod.Enabled = true;
            dgvContractPeriod.ReadOnly = false;
            dgvContractPeriod.Enabled = true;

            GetSetting();

            btnUpdate.Text = "등록(A)";
            btnUpdate.ForeColor = Color.Blue;
        }

        private void InsertData()
        {
            if (MessageBox.Show(this,"[" + txtProduct.Text +  " / " + txtOrigin.Text + "] 의 내역을 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();
            //Delete
            sql = contractRecommendationRepository.DeleteRecommend(txtProduct.Text, txtOrigin.Text);
            sqlList.Add(sql);
            //Insert
            for (int i = 0; i < 12; i++)
            {
                if (dgvMakePeriod.Rows[0].Cells[i].Selected)
                {
                    ContractRecommendationModel model = new ContractRecommendationModel();
                    model.product = txtProduct.Text;
                    model.origin = txtOrigin.Text;
                    model.division = "조업시기";
                    model.month = i + 1;
                    model.remark = txtRemark.Text;
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    sql = contractRecommendationRepository.InsertRecommend(model);
                    sqlList.Add(sql);
                }
            }

            for (int i = 0; i < 12; i++)
            {
                if (dgvContractPeriod.Rows[0].Cells[i].Selected)
                {
                    ContractRecommendationModel model = new ContractRecommendationModel();
                    model.product = txtProduct.Text;
                    model.origin = txtOrigin.Text;
                    model.division = "계약시기";
                    model.month = i + 1;
                    model.remark = txtRemark.Text;
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    sql = contractRecommendationRepository.InsertRecommend(model);
                    sqlList.Add(sql);
                }
            }
            //Execute
            if (sqlList.Count > 0)
            {
                int results = contractRecommendationRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
                }
                else
                {
                    dgvMakePeriod.ClearSelection();
                    dgvMakePeriod.ReadOnly = true;
                    dgvMakePeriod.Enabled = false;

                    dgvContractPeriod.ClearSelection();
                    dgvContractPeriod.ReadOnly = true;
                    dgvContractPeriod.Enabled = false;
                    GetData();
                    btnUpdate.Text = "수정";
                    btnUpdate.ForeColor = Color.Black;
                }
            }
        }
        private void GetData()
        {
            //초기화
            foreach (DataGridViewCell cell in dgvMakePeriod.Rows[0].Cells)
            {
                cell.Style.BackColor = Color.White;
            }
            foreach (DataGridViewCell cell in dgvContractPeriod.Rows[0].Cells)
            {
                cell.Style.BackColor = Color.White;
            }

            if (dt != null && dt.Length > 0)
            {
                for (int i = 0; i < dt.Length; i++)
                {
                    DataGridView dgv;
                    Color color;
                    if (dt[i]["division"].ToString() == "조업시기")
                    {
                        dgv = dgvMakePeriod;
                        color = Color.DarkBlue;
                    }
                    else
                    {
                        dgv = dgvContractPeriod;
                        color = Color.DarkGreen;
                    }

                    string month = dt[i]["month"].ToString();
                    dgv.Rows[0].Cells[Convert.ToInt16(month) - 1].Style.BackColor = color;
                    txtRemark.Text = dt[i]["remark"].ToString();
                }
            }
        }
        private void GetSetting()
        {
            //초기화
            foreach (DataGridViewCell cell in dgvMakePeriod.Rows[0].Cells)
            {
                cell.Style.BackColor = Color.White;
            }
            foreach (DataGridViewCell cell in dgvContractPeriod.Rows[0].Cells)
            {
                cell.Style.BackColor = Color.White;
            }
            //배경색 대신 Select변경
            DataTable recommendDt = contractRecommendationRepository.GetRecommendAsOne(txtProduct.Text, txtOrigin.Text);
            if (recommendDt.Rows.Count > 0)
            {
                for (int i = 0; i < recommendDt.Rows.Count; i++)
                {
                    DataGridView dgv;
                    if (recommendDt.Rows[i]["division"].ToString() == "조업시기")
                    {
                        dgv = dgvMakePeriod;
                    }
                    else
                    {
                        dgv = dgvContractPeriod;
                    }

                    dgv.Rows[0].Cells[Convert.ToInt16(recommendDt.Rows[i]["month"].ToString()) - 1].Selected = true;
                }
            }
        }
        #endregion

        #region Key event
        private void dgvMakePeriod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        if (btnUpdate.Text == "수정")
                        {
                            UpdateSetting();
                        }
                        break;
                    case Keys.A:
                        if (btnUpdate.Text == "등록(A)")
                        {
                            InsertData();
                        }
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "수정")
            {
                UpdateSetting();
            }
            else if (btnUpdate.Text == "등록(A)")
            {
                InsertData();
            }
        }
        #endregion

        #region Datagridview event
        private void dgvMakePeriod_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgvMakePeriod.Rows[e.RowIndex].Cells[e.ColumnIndex];
                ContractRecommendationModel model = new ContractRecommendationModel();
                List<StringBuilder> sqlList = new List<StringBuilder>();
                model.product = txtProduct.Text;
                model.origin = txtOrigin.Text;
                model.division = "조업시기";
                model.month = e.ColumnIndex + 1;
                model.remark = txtRemark.Text;
                model.edit_user = um.user_name;
                model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                //등록
                if (cell.Style.BackColor == Color.White)
                {
                    cell.Style.BackColor = Color.DarkBlue;
                    StringBuilder sql = contractRecommendationRepository.InsertRecommend(model);
                    sqlList.Add(sql);
                }
                //삭제
                else
                {
                    cell.Style.BackColor = Color.White;
                    StringBuilder sql = contractRecommendationRepository.DeleteRecommendAsOne(model);
                    sqlList.Add(sql);
                }
                //Execute
                if (contractRecommendationRepository.UpdateTran(sqlList) == -1)
                {
                    MessageBox.Show(this, "수정시 에러가 발생하였습니다.");
                }
            }
        }

        private void dgvContractPeriod_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgvContractPeriod.Rows[e.RowIndex].Cells[e.ColumnIndex];
                ContractRecommendationModel model = new ContractRecommendationModel();
                List<StringBuilder> sqlList = new List<StringBuilder>();
                model.product = txtProduct.Text;
                model.origin = txtOrigin.Text;
                model.division = "계약시기";
                model.month = e.ColumnIndex + 1;
                model.remark = txtRemark.Text;
                model.edit_user = um.user_name;
                model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                //등록
                if (cell.Style.BackColor == Color.White)
                {
                    cell.Style.BackColor = Color.DarkBlue;
                    StringBuilder sql = contractRecommendationRepository.InsertRecommend(model);
                    sqlList.Add(sql);
                }
                //삭제
                else
                {
                    cell.Style.BackColor = Color.White;
                    StringBuilder sql = contractRecommendationRepository.DeleteRecommendAsOne(model);
                    sqlList.Add(sql);
                }
                //Execute
                if (contractRecommendationRepository.UpdateTran(sqlList) == -1)
                    MessageBox.Show(this,"수정시 에러가 발생하였습니다.");
            }
        }
        #endregion
    }
}

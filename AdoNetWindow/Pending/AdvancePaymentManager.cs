using AdoNetWindow.Model;
using Repositories;
using Repositories.Pending;
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
    public partial class AdvancePaymentManager : Form
    {
        IPaymentRepository paymentRepository = new PaymentRepository();
        ICommonRepository commonRepository = new CommonRepository();
        CalendarModule.calendar cd;
        int id;
        int paymenId;
        string paymentDate;
        UsersModel um; 
        public AdvancePaymentManager(CalendarModule.calendar cal, UsersModel uModel, int custom_id, int payment_id, string paymentDate)
        {
            InitializeComponent();
            cd = cal;
            um = uModel;
            id = custom_id;
            paymenId = payment_id;
            this.paymentDate = paymentDate;
        }
        private void AdvancePaymentManager_Load(object sender, EventArgs e)
        {
            SetData();
        }

        #region Method
        private void SetData()
        {
            DataTable pendingDt = paymentRepository.GetPaymentAsOne(id.ToString());
            if (pendingDt.Rows.Count > 0)
            {
                DataRow[] dr = pendingDt.Select($"payment_date = '{paymentDate}'");
                if(dr.Length > 0)
                {
                    txtAtono.Text = dr[0]["ato_no"].ToString();
                    txtBlno.Text = dr[0]["bl_no"].ToString();
                    txtLcno.Text = dr[0]["lc_payment_date"].ToString();
                    txtUsance.Text = dr[0]["usance_type"].ToString();
                    txtAgency.Text = dr[0]["agency_type"].ToString();
                    txtDivision.Text = dr[0]["division"].ToString();
                    txtManager.Text = dr[0]["manager"].ToString();

                    txtPaymentAmount.Text = Convert.ToDouble(dr[0]["payment_amount"].ToString()).ToString("#,##0.00");
                    txtPaymentDate.Text = Convert.ToDateTime(dr[0]["payment_date"].ToString()).ToString("yyyy-MM-dd");
                    cbPaymentCurrency.Text = dr[0]["payment_currency"].ToString();
                    cbPaymentDateStatus.Text = dr[0]["payment_date_status"].ToString();
                    txtRemark.Text = dr[0]["remark"].ToString();
                    txtPaymentBank.Text = dr[0]["payment_bank"].ToString();
                    for (int i = 0; i < dr.Length; i++)
                    {
                        int n = dgvProduct.Rows.Add();
                        dgvProduct.Rows[n].Cells["product"].Value = dr[i]["product"].ToString();
                        dgvProduct.Rows[n].Cells["origin"].Value = dr[i]["origin"].ToString();
                        dgvProduct.Rows[n].Cells["sizes"].Value = dr[i]["sizes"].ToString();
                        dgvProduct.Rows[n].Cells["unit"].Value = dr[i]["box_weight"].ToString();
                        dgvProduct.Rows[n].Cells["qty"].Value = dr[i]["quantity_on_paper"].ToString();
                    }
                }
            }
        }
        #endregion


        #region Buttton Click
        private void btnComfirm_Click(object sender, EventArgs e)
        {
            int result = paymentRepository.UpdateStatus(paymenId.ToString(), "확정", paymentDate);
            if (result == -1)
            {
                MessageBox.Show(this, "수정중에 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                cbPaymentDateStatus.Text = "확정";
                cd.displayDays(cd.year, cd.month);
            }
        }

        private void btnDeadline_Click(object sender, EventArgs e)
        {
            int result = paymentRepository.UpdateStatus(paymenId.ToString(), "확정(마감)", paymentDate);
            if (result == -1)
            {
                MessageBox.Show(this, "수정중에 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                cbPaymentDateStatus.Text = "확정(마감)";
                cd.displayDays(cd.year, cd.month);
            }
        }

        private void btnPayComplete_Click(object sender, EventArgs e)
        {
            int result = paymentRepository.UpdateStatus(paymenId.ToString(), "결제완료", paymentDate);
            if (result == -1)
            {
                MessageBox.Show(this, "수정중에 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                cbPaymentDateStatus.Text = "결제완료";
                cd.displayDays(cd.year, cd.month);
            }
        }

        private void btnUpadate_Click(object sender, EventArgs e)
        {
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql;
            //유효성검사
            if (string.IsNullOrEmpty(cbPaymentDateStatus.Text))
            {
                MessageBox.Show(this, "결제상태를 선택해주세요.");
                this.Activate();
                return;
            }
            DateTime pay_date;
            if (!DateTime.TryParse(txtPaymentDate.Text, out pay_date))
            {
                MessageBox.Show(this, "결제일자를 입력해주세요.");
                this.Activate();
                return;
            }
            double pay_amount;
            if (!double.TryParse(txtPaymentAmount.Text, out pay_amount))
            {
                MessageBox.Show(this, "결제금액을 입력해주세요.");
                this.Activate();
                return;
            }
            else if (pay_amount == 0)
            {
                MessageBox.Show(this, "결제금액을 입력해주세요.");
                this.Activate();
                return;
            }
            //등록
            PaymentModel model = new PaymentModel();
            model.id = paymenId;
            model.division = "pending";
            model.contract_id = id;
            model.payment_date_status = cbPaymentDateStatus.Text;
            model.payment_currency = cbPaymentCurrency.Text;
            model.payment_amount = pay_amount;
            model.payment_date = pay_date.ToString("yyyy-MM-dd");
            model.remark = txtRemark.Text;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
            model.edit_user = um.user_name;

            //Delete
            sql = paymentRepository.DeleteSql(model);
            sqlList.Add(sql);
            //Insert
            sql = paymentRepository.InsertSql(model);
            sqlList.Add(sql);
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                cd.displayDays(cd.year, cd.month);
            }
            this.BringToFront();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "선택하신 결제내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            PaymentModel model = new PaymentModel();
            model.id = paymenId;
            //Delete
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = paymentRepository.DeleteSql(model);
            sqlList.Add(sql);
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                this.Dispose();
                cd.displayDays(cd.year, cd.month);
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (um.auth_level > 2)
            {
                UnPendingManager view = new UnPendingManager(cd, um, "", id);
                view.StartPosition = FormStartPosition.CenterParent;
                view.Show();
            }
            else
            {
                PendingView view = new PendingView(um, id.ToString(), "");
                view.StartPosition = FormStartPosition.CenterParent;
                view.Show();
            }
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        private void AdvancePaymentManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnUpadate.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.W:
                        btnDetail.PerformClick();
                        break;
                }
            }
        }
    }
}

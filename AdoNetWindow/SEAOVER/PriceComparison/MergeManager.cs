using AdoNetWindow.DashboardForSales;
using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class MergeManager : Form
    {
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        PriceComparison pc = null;
        DetailDashBoardForSales ddfs = null;
        public MergeManager(UsersModel um, List<DataGridViewRow> row, PriceComparison pc)
        {
            InitializeComponent();
            this.um = um;
            this.pc = pc;

            for (int i = 0; i < row.Count; i++)
            {
                int n = dgvProduct.Rows.Add();
                dgvProduct.Rows[n].Cells["product"].Value = row[i].Cells["product"].Value;
                dgvProduct.Rows[n].Cells["origin"].Value = row[i].Cells["origin"].Value;
                dgvProduct.Rows[n].Cells["sizes"].Value = row[i].Cells["sizes"].Value;
                dgvProduct.Rows[n].Cells["unit"].Value = row[i].Cells["unit"].Value;
                dgvProduct.Rows[n].Cells["price_unit"].Value = row[i].Cells["price_unit"].Value;
                dgvProduct.Rows[n].Cells["unit_count"].Value = row[i].Cells["unit_count"].Value;
                dgvProduct.Rows[n].Cells["package"].Value = row[i].Cells["bundle_count"].Value;
                dgvProduct.Rows[n].Cells["seaover_unit"].Value = row[i].Cells["seaover_unit"].Value;
            }
        }

        public MergeManager(UsersModel um, List<DataGridViewRow> row, DetailDashBoardForSales ddfs)
        {
            InitializeComponent();
            this.um = um;
            this.ddfs = ddfs;
            this.cbRegistration.Checked = true;
            for (int i = 0; i < row.Count; i++)
            {
                int n = dgvProduct.Rows.Add();
                dgvProduct.Rows[n].Cells["product"].Value = row[i].Cells["product"].Value;
                dgvProduct.Rows[n].Cells["origin"].Value = row[i].Cells["origin"].Value;
                dgvProduct.Rows[n].Cells["sizes"].Value = row[i].Cells["sizes"].Value;
                dgvProduct.Rows[n].Cells["unit"].Value = row[i].Cells["unit"].Value;
                dgvProduct.Rows[n].Cells["price_unit"].Value = row[i].Cells["price_unit"].Value;
                dgvProduct.Rows[n].Cells["unit_count"].Value = row[i].Cells["unit_count"].Value;
                dgvProduct.Rows[n].Cells["package"].Value = row[i].Cells["bundle_count"].Value;
                dgvProduct.Rows[n].Cells["seaover_unit"].Value = row[i].Cells["seaover_unit"].Value;
            }
        }

        #region Datagridview event
        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvProduct.EndEdit();
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    row.Cells["chk"].Value = false;
                dgvProduct.Rows[e.RowIndex].Cells["chk"].Value = true;

                //대표품목 선택시
                txtProduct.Text = dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString();
                txtOrigin.Text = dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString();
                txtSizes.Text = dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString();
                txtUnit.Text = dgvProduct.Rows[e.RowIndex].Cells["unit"].Value.ToString();
                txtPriceUnit.Text = dgvProduct.Rows[e.RowIndex].Cells["price_unit"].Value.ToString();
                txtUnitCount.Text = dgvProduct.Rows[e.RowIndex].Cells["unit_count"].Value.ToString();
                txtPackage.Text = dgvProduct.Rows[e.RowIndex].Cells["package"].Value.ToString();
                txtSeaoverUnit.Text = dgvProduct.Rows[e.RowIndex].Cells["seaover_unit"].Value.ToString();
            }
        }

        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "chk")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    if (isChecked)
                        dgvProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Beige;
                    else
                        dgvProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
        #endregion

        #region Button
        private void btnInsert_Click(object sender, EventArgs e)
        {
            //권한확인
            if (cbRegistration.Checked)
            {
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_add"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }
            }

            bool isChecked = false;
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value))
                {
                    isChecked = true;
                    break;
                }
            }
            //Msg
            if (MessageBox.Show(this, "선택하신 품목으로 병합하시겠습니까?"
                + "\n\n 우측하단에 '기본값 설정' 체크박스를 선택하였을 경우만 데이터가 등록되어 계속 병합되고, 설정하지 않은 경우 일회성으로 병합됩니다.", "YesOrNo"
                    , MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            int main_id = 9999;
            string merge_product = "";
            if (cbRegistration.Checked)
            {
                StringBuilder sql = new StringBuilder();
                List<StringBuilder> sqlList = new List<StringBuilder>();
                ProductGroupModel model = new ProductGroupModel();
                main_id = commonRepository.GetNextId("t_product_group", "main_id");
                int sub_id = 1;
                model.main_id = main_id;
                model.product = txtProduct.Text;
                model.origin = txtOrigin.Text;
                model.sizes = txtSizes.Text;
                model.unit = txtUnit.Text;
                model.price_unit = txtPriceUnit.Text;
                model.unit_count = txtUnitCount.Text;
                model.seaover_unit = txtSeaoverUnit.Text;

                //merge_product 
                merge_product = txtProduct.Text
                                    + "^" + txtOrigin.Text
                                    + "^" + txtSizes.Text
                                    + "^" + txtUnit.Text
                                    + "^" + txtPriceUnit.Text
                                    + "^" + txtUnitCount.Text
                                    + "^" + txtSeaoverUnit.Text;

                //기존 데이터 삭제
                sql = productGroupRepository.DeleteProductGroup(model);
                sqlList.Add(sql);

                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    model.sub_id = sub_id;
                    model.item_product = row.Cells["product"].Value.ToString();
                    model.item_origin = row.Cells["origin"].Value.ToString();
                    model.item_sizes = row.Cells["sizes"].Value.ToString();
                    model.item_unit = row.Cells["unit"].Value.ToString();
                    model.item_price_unit = row.Cells["price_unit"].Value.ToString();
                    model.item_unit_count = row.Cells["unit_count"].Value.ToString();
                    model.item_seaover_unit = row.Cells["seaover_unit"].Value.ToString();

                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    //데이터 추가
                    sql = productGroupRepository.InsertProductGroup(model);
                    sqlList.Add(sql);
                    sub_id++;

                    //merge_product 
                    string product_code = model.item_product
                                + "^" + model.item_origin
                                + "^" + model.item_sizes
                                + "^" + model.item_unit
                                + "^" + model.item_price_unit
                                + "^" + model.item_unit_count
                                + "^" + model.item_seaover_unit;
                    if (!merge_product.Contains(product_code))
                        merge_product += "\n" + product_code;
                }

                //Execute
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    MessageBox.Show(this, "수정중 에러가 발생하였습니다.");
                    this.Activate();
                    return;
                }

            }

            //반영하기
            if (pc != null)
                pc.SetMergeProduct(this.dgvProduct, main_id);

            if (ddfs != null)
                ddfs.SetMergeProduct(this.dgvProduct, main_id, merge_product);

            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion
    }
}

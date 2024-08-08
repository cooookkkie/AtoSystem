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

namespace AdoNetWindow.SEAOVER.TwoLine
{
    public partial class ProductUnitManger : Form
    {
        IFormChangedDataRepository formChangedDataRepository = new FormChangedDataRepository();
        ProductUnit pu;
        string category;
        SeaoverPriceModel model;
        public ProductUnitManger(ProductUnit productUnit, string contents, SeaoverPriceModel spm, string category)
        {
            InitializeComponent();
            pu = productUnit;
            model = spm;
            this.category = category;
            txtContents.Text = contents.Trim();
        }

        private void ProductUnitManger_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }


        private void ContentsSave()
        {
            if(pu != null)
                pu.ContentsUpdate(txtContents.Text.Trim());
            this.Dispose();
        }
        private void AllSetting()
        {
            pu.ContentsUpdate(txtContents.Text.Trim());
            if (MessageBox.Show(this, "설정할 경우 모든 품목서의 공통내역이 설정값으로 변경됩니다. 설정하시겠습니까? ", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FormChangedDataModel fcdm = new FormChangedDataModel();
                fcdm.column_name = category;
                int id = formChangedDataRepository.GetNextId();
                fcdm.id = id;

                switch (category)
                {
                    case "category":
                        fcdm.column_code = model.category_code;
                        fcdm.origin_text = model.category;
                        break;
                    case "product":
                        fcdm.column_code = model.product_code;
                        fcdm.origin_text = model.product;
                        break;
                    case "origin":
                        fcdm.column_code = model.origin_code;
                        fcdm.origin_text = model.origin;
                        break;
                    case "sizes":
                        fcdm.column_code = model.product_code + "^" + model.origin_code + "^" + model.sizes_code;
                        fcdm.origin_text = model.sizes;
                        break;
                    case "sizes1":
                        fcdm.column_code = model.product_code + "^" + model.origin_code + "^" + model.sizes_code + "^" + 1;
                        fcdm.origin_text = model.sizes1;
                        break;
                    case "sizes2":
                        fcdm.column_code = model.product_code + "^" + model.origin_code + "^" + model.sizes_code + "^" + 2;
                        fcdm.origin_text = model.sizes2;
                        break;
                    case "cost_unit":
                        fcdm.column_code = model.product_code + "^" + model.origin_code + "^" + model.sizes_code + "^" + model.unit + "^" + model.price_unit + "^" + model.cost_unit;
                        fcdm.origin_text = model.cost_unit;
                        break;
                    case "weight":
                        fcdm.column_code = model.product_code + "^" + model.origin_code + "^" + model.sizes_code + "^" + model.unit + "^" + model.price_unit + "^" + model.unit_count;
                        fcdm.origin_text = model.weight;
                        break;
                    default:
                        return;
                        break;
                }
                fcdm.changed_text = txtContents.Text.Trim();

                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = new StringBuilder();
                sql = formChangedDataRepository.DeleteSql(fcdm);
                sqlList.Add(sql);
                sql = formChangedDataRepository.InsertSql(fcdm);
                sqlList.Add(sql);
                int results = formChangedDataRepository.UpdateCustomTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ContentsSave();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnAllSetting_Click(object sender, EventArgs e)
        {
            AllSetting();
        }

        private void ProductUnitManger_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        ContentsSave();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.Z:
                        AllSetting();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        this.Dispose();
                        break;
                }
            }
        }

        
    }
}

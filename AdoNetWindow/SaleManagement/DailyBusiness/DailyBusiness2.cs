using AdoNetWindow.Model;
using Libs.Datagrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    public partial class DailyBusiness2 : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        public DailyBusiness2(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void DailyBusiness2_Load(object sender, EventArgs e)
        {
            UserDatagrid udg = new UserDatagrid(um);

            string[] colName = new string[13];
            colName[0] = "input_date";
            colName[1] = "company";
            colName[2] = "origin";
            colName[3] = "product";
            colName[4] = "sizes";
            colName[5] = "unit";
            colName[6] = "qty";
            colName[7] = "sale_price";
            colName[8] = "sale_amount";
            colName[9] = "warehouse";
            colName[10] = "purchase_company";
            colName[11] = "purchase_price";
            colName[12] = "remark";
            /*colName[13] = "freight";
            colName[14] = "inquire";*/
            
            string[] colText = new string[13];
            colText[0] = "입력일자";
            colText[1] = "거래처";
            colText[2] = "원산지";
            colText[3] = "품명";
            colText[4] = "규격";
            colText[5] = "단위";
            colText[6] = "수량";
            colText[7] = "매출단가";
            colText[8] = "매출금액";
            colText[9] = "창고";
            colText[10] = "매입처";
            colText[11] = "매입금액";
            colText[12] = "비고";
            /*colText[13] = "화물";
            colText[14] = "문의내역";*/

            udg.SetColumn(colText, colName);
            elementHost1.Child = udg;

            System.Windows.Controls.DataGrid dg = udg.datagrid();
        }
    }
}

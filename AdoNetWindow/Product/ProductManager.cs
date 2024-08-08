using AdoNetWindow.Common;
using AdoNetWindow.Model;
using AdoNetWindow.OverseaManufacturingBusiness;
using AdoNetWindow.PurchaseManager;
using AdoNetWindow.SEAOVER.PriceComparison;
using Repositories;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace AdoNetWindow.Product
{
    public partial class ProductManager : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        UsersModel um;
        string[] productInfo;
        List<string[]> productInfoList;
        bool isSelectMode = false;
        bool isMultiSelect = false;
        PurchaseManager.PurchaseUnitPriceInfo pupi = null;
        Config.GroupManager gm = null;
        Pending.UnpendingAddManager uam  = null;
        PurchaseManager.GraphManager gpm = null;
        PurchaseManager.CostAccounting ca = null;
        Pending.ConvertProductToSeaover cpts = null;
        DashboardForTrade.PurchaseDashboard pdb = null;
        DashboardForTrade.PurchaseDashboard2 pdb2 = null;
        public ProductManager(UsersModel uModel)
        {
            InitializeComponent();
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            um = uModel;
        }
        public ProductManager(UsersModel uModel, Pending.ConvertProductToSeaover convertProductToSeaover)
        {
            InitializeComponent();
            um = uModel;
            cpts = convertProductToSeaover;
            if (isMultiSelect)
                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.Columns["chk"].Visible = false;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnInsert.Visible = false;
            txtProduct.Focus();

        }
        public ProductManager(UsersModel uModel, DashboardForTrade.PurchaseDashboard pdb)
        {
            InitializeComponent();
            um = uModel;
            this.pdb = pdb;
            if (isMultiSelect)
                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.Columns["chk"].Visible = false;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnInsert.Visible = false;
            txtProduct.Focus();

        }
        public ProductManager(UsersModel uModel, DashboardForTrade.PurchaseDashboard2 pdb2)
        {
            InitializeComponent();
            um = uModel;
            this.pdb2 = pdb2;
            if (isMultiSelect)
                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.Columns["chk"].Visible = false;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnInsert.Visible = false;
            txtProduct.Focus();

        }
        public ProductManager(UsersModel uModel, PurchaseManager.CostAccounting cAccount)
        {
            InitializeComponent();
            um = uModel;
            ca = cAccount;
            if (isMultiSelect)
                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.Columns["chk"].Visible = false;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnInsert.Visible = false;
            txtProduct.Focus();

        }
        public ProductManager(UsersModel uModel,PurchaseManager.PurchaseUnitPriceInfo pp, bool isMultiSelect = false)
        {
            InitializeComponent();
            um = uModel;
            pupi = pp;
            if (isMultiSelect)
                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.Columns["chk"].Visible = false;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnInsert.Visible = false;
            txtProduct.Focus();

        }
        public ProductManager(UsersModel uModel, Pending.UnpendingAddManager uaManager, bool isMultiSelect = false)
        {
            InitializeComponent();
            um = uModel;
            uam = uaManager;
            if (isMultiSelect)
                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.Columns["chk"].Visible = false;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnInsert.Visible = false;
            txtProduct.Focus();
        }
        public ProductManager(UsersModel uModel, PurchaseManager.GraphManager graph, bool isMultiSelect = false)
        {
            InitializeComponent();
            um = uModel;
            gpm = graph;
            if (isMultiSelect)
                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.Columns["chk"].Visible = false;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnInsert.Visible = false;
            txtProduct.Focus();
        }
        public ProductManager(UsersModel uModel, Config.GroupManager gManager, bool isMultiSelect = false)
        {
            InitializeComponent();
            um = uModel;
            gm = gManager;
            if (isMultiSelect)
                /*this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);*/

            dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnInsert.Visible = false;
            txtProduct.Focus();
        }

        private void ProductManager_Load(object sender, EventArgs e)
        {
            CallProcedure();
            SetDgvStyleSetting();
            //GetData();
            this.ActiveControl = txtProduct;
        }

        #region Method

        public string[] GetProduct(string product, string origin, string sizes, string unit)
        {
            dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            isSelectMode = true;
            isMultiSelect = false;
            btnSelect.Visible = true;

            dgvProduct.Columns["unit"].Visible = false;

            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            
            CallProcedure();
            txtProduct.Text = product;
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtUnit.Text = unit;
            SetDgvStyleSetting();
            //GetData();
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            try
            {
                if (this.ShowDialog() == DialogResult.OK)
                {        
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this,ex.Message);
                this.Activate();
            }

            return productInfo;
        }
        public List<string[]> GetProductList(string product, string origin, string sizes, string unit)
        {
            dgvProduct.Columns["chk"].Visible = false;

            dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            isSelectMode = true;
            isMultiSelect = true;
            btnSelect.Visible = true;

            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            CallProcedure();
            txtProduct.Text = product;
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtUnit.Text = unit;
            SetDgvStyleSetting();
            //GetData();
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            try
            {
                if (this.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
                this.Activate();
            }

            return productInfoList;
        }

        public DataTable JoinTwoTables(DataTable dt1, DataTable dt2)
        {
            DataTable dtResult = new DataTable();
            var dt1Columns = new List<string>();
            foreach (DataColumn column in dt1.Columns)
            {
                dt1Columns.Add(column.ColumnName);
                dtResult.Columns.Add(column.ColumnName);
            }

            var dt2Columns = new List<string>();
            foreach (DataColumn column in dt2.Columns)
            {
                if (!dt1Columns.Contains(column.ColumnName))
                {
                    dt2Columns.Add(column.ColumnName);
                    dtResult.Columns.Add(column.ColumnName);
                }
            }

            return dtResult;
        }

        private DataTable SetTable()
        {
            DataTable dt = new DataTable();

            DataColumn col0 = new DataColumn();
            col0.DataType = System.Type.GetType("System.String");
            col0.AllowDBNull = false;
            col0.ColumnName = "group";
            col0.Caption = "그룹";
            col0.DefaultValue = "";
            dt.Columns.Add(col0);

            DataColumn col1 = new DataColumn();
            col1.DataType = System.Type.GetType("System.String");
            col1.AllowDBNull = false;
            col1.ColumnName = "product";
            col1.Caption = "품명";
            col1.DefaultValue = "";
            dt.Columns.Add(col1);

            DataColumn col2 = new DataColumn();
            col2.DataType = System.Type.GetType("System.String");
            col2.AllowDBNull = false;
            col2.ColumnName = "origin";
            col2.Caption = "원산지";
            col2.DefaultValue = "";
            dt.Columns.Add(col2);

            DataColumn col3 = new DataColumn();
            col3.DataType = System.Type.GetType("System.String");
            col3.AllowDBNull = false;
            col3.ColumnName = "sizes";
            col3.Caption = "규격";
            col3.DefaultValue = "";
            dt.Columns.Add(col3);

            DataColumn col33 = new DataColumn();
            col33.DataType = System.Type.GetType("System.Double");
            col33.AllowDBNull = false;
            col33.ColumnName = "sizes2";
            col33.Caption = "규격2";
            col33.DefaultValue = 01;
            dt.Columns.Add(col33);

            DataColumn col4 = new DataColumn();
            col4.DataType = System.Type.GetType("System.Double");
            col4.AllowDBNull = false;
            col4.ColumnName = "unit";
            col4.Caption = "단위";
            col4.DefaultValue = 0;
            dt.Columns.Add(col4);

            DataColumn col5 = new DataColumn();
            col5.DataType = System.Type.GetType("System.Double");
            col5.AllowDBNull = false;
            col5.ColumnName = "cost_unit";
            col5.Caption = "트레이";
            col5.DefaultValue = 0;
            dt.Columns.Add(col5);

            DataColumn col6 = new DataColumn();
            col6.DataType = System.Type.GetType("System.Double");
            col6.AllowDBNull = false;
            col6.ColumnName = "incidental_expense";
            col6.Caption = "부대비용";
            col6.DefaultValue = 0;
            dt.Columns.Add(col6);

            DataColumn col7 = new DataColumn();
            col7.DataType = System.Type.GetType("System.Double");
            col7.AllowDBNull = false;
            col7.ColumnName = "tax";
            col7.Caption = "과세";
            col7.DefaultValue = 0;
            dt.Columns.Add(col7);

            DataColumn col8 = new DataColumn();
            col8.DataType = System.Type.GetType("System.Double");
            col8.AllowDBNull = false;
            col8.ColumnName = "custom";
            col8.Caption = "관세";
            col8.DefaultValue = 0;
            dt.Columns.Add(col8);

            DataColumn col81 = new DataColumn();
            col81.DataType = System.Type.GetType("System.Double");
            col81.AllowDBNull = false;
            col81.ColumnName = "production_days";
            col81.Caption = "생산일";
            col81.DefaultValue = 0;
            dt.Columns.Add(col81);

            DataColumn col82 = new DataColumn();
            col82.DataType = System.Type.GetType("System.Double");
            col82.AllowDBNull = false;
            col82.ColumnName = "purchase_margin";
            col82.Caption = "수입마진";
            col82.DefaultValue = 0;
            dt.Columns.Add(col82);

            DataColumn col9 = new DataColumn();
            col9.DataType = System.Type.GetType("System.String");
            col9.AllowDBNull = false;
            col9.ColumnName = "manager";
            col9.Caption = "담당자";
            col9.DefaultValue = "";
            dt.Columns.Add(col9);

            DataColumn col10 = new DataColumn();
            col10.DataType = System.Type.GetType("System.String");
            col10.AllowDBNull = false;
            col10.ColumnName = "isExist";
            col10.Caption = "등록여부";
            col10.DefaultValue = "";
            dt.Columns.Add(col10);

            DataColumn col11 = new DataColumn();
            col11.DataType = System.Type.GetType("System.Boolean");
            col11.AllowDBNull = false;
            col11.ColumnName = "weight_calculate";
            col11.Caption = "중량";
            col11.DefaultValue = false;
            dt.Columns.Add(col11);

            DataColumn col12 = new DataColumn();
            col12.DataType = System.Type.GetType("System.Boolean");
            col12.AllowDBNull = false;
            col12.ColumnName = "tray_calculate";
            col12.Caption = "트레이";
            col12.DefaultValue = false;
            dt.Columns.Add(col12);


            DataColumn col13 = new DataColumn();
            col13.DataType = System.Type.GetType("System.Boolean");
            col13.AllowDBNull = false;
            col13.ColumnName = "isMonth";
            col13.Caption = "isMonth";
            col13.DefaultValue = false;
            dt.Columns.Add(col13);

            DataColumn col14 = new DataColumn();
            col14.DataType = System.Type.GetType("System.String");
            col14.AllowDBNull = false;
            col14.ColumnName = "show_sttdate";
            col14.Caption = "show_sttdate";
            col14.DefaultValue = false;
            dt.Columns.Add(col14);

            DataColumn col15 = new DataColumn();
            col15.DataType = System.Type.GetType("System.String");
            col15.AllowDBNull = false;
            col15.ColumnName = "show_enddate";
            col15.Caption = "show_enddate";
            col15.DefaultValue = false;
            dt.Columns.Add(col15);

            DataColumn col16 = new DataColumn();
            col16.DataType = System.Type.GetType("System.String");
            col16.AllowDBNull = false;
            col16.ColumnName = "hide_date";
            col16.Caption = "hide_date";
            col16.DefaultValue = false;
            dt.Columns.Add(col16);

            DataColumn col17 = new DataColumn();
            col17.DataType = System.Type.GetType("System.Double");
            col17.AllowDBNull = false;
            col17.ColumnName = "base_around_month";
            col17.Caption = "base_around_month";
            col17.DefaultValue = false;
            dt.Columns.Add(col17);

            DataColumn col18 = new DataColumn();
            col18.DataType = System.Type.GetType("System.Double");
            col18.AllowDBNull = false;
            col18.ColumnName = "duplicate_cnt";
            col18.Caption = "duplicate_cnt";
            col18.DefaultValue = 0;
            dt.Columns.Add(col18);

            DataColumn col19 = new DataColumn();
            col19.DataType = System.Type.GetType("System.String");
            col19.AllowDBNull = false;
            col19.ColumnName = "remark";
            col19.Caption = "비고";
            col19.DefaultValue = "";
            dt.Columns.Add(col19);

            return dt;
        }
            private void GetData()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            dgvProduct.Rows.Clear();
            DataTable productDt = SetTable();
            DataTable seaoverDt = seaoverRepository.GetProductTable2(txtProduct.Text, cbisExactly.Checked, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            DataTable otherCostDt = productOtherCostRepository.GetProduct(txtProduct.Text, cbisExactly.Checked, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text, txtGroup.Text);
            //SEAOVER품목
            if (cbSeaover.Checked && seaoverDt.Rows.Count > 0)
            {
                for (int i = 0; i < seaoverDt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(txtManager.Text) && string.IsNullOrEmpty(txtGroup.Text))
                    {
                        DataRow row = productDt.NewRow();
                        row["product"] = seaoverDt.Rows[i]["품명"].ToString();
                        row["origin"] = seaoverDt.Rows[i]["원산지"].ToString();
                        row["sizes"] = seaoverDt.Rows[i]["규격"].ToString();
                        double d;
                        if(double.TryParse(seaoverDt.Rows[i]["규격3"].ToString(), out d))
                            row["sizes2"] = seaoverDt.Rows[i]["규격3"].ToString();

                        row["unit"] = seaoverDt.Rows[i]["SEAOVER단위"].ToString();
                        row["cost_unit"] = seaoverDt.Rows[i]["트레이"].ToString();
                        row["weight_calculate"] = true;
                        row["isExist"] = "SEA";

                        //품목 추가등록정보
                        DataRow[] arrRows = null;
                        arrRows = otherCostDt.Select($"product = '{seaoverDt.Rows[i]["품명"].ToString()}' " +
                                                     $"AND origin = '{seaoverDt.Rows[i]["원산지"].ToString()}'" +
                                                     $"AND sizes = '{seaoverDt.Rows[i]["규격"].ToString()}'" +
                                                     $"AND unit = '{seaoverDt.Rows[i]["SEAOVER단위"].ToString()}'");
                        //출력
                        if (arrRows.Length > 0)
                        {
                            row["group"] = arrRows[0]["group_name"].ToString();
                            //row["cost_unit"] = Convert.ToDouble(arrRows[0]["cost_unit"].ToString());
                            row["custom"] = Convert.ToDouble(arrRows[0]["custom"].ToString());
                            row["incidental_expense"] = Convert.ToDouble(arrRows[0]["incidental_expense"].ToString());
                            row["tax"] = Convert.ToDouble(arrRows[0]["tax"].ToString());
                            row["production_days"] = Convert.ToDouble(arrRows[0]["production_days"].ToString());
                            row["purchase_margin"] = Convert.ToDouble(arrRows[0]["purchase_margin"].ToString());
                            row["cost_unit"] = Convert.ToDouble(arrRows[0]["cost_unit"].ToString());
                            row["manager"] = arrRows[0]["manager"].ToString();

                            row["weight_calculate"] = Convert.ToBoolean(arrRows[0]["weight_calculate"].ToString());
                            row["tray_calculate"] = Convert.ToBoolean(arrRows[0]["tray_calculate"].ToString());

                            bool weight_calculate = Convert.ToBoolean(arrRows[0]["weight_calculate"].ToString());
                            bool tray_calculate = Convert.ToBoolean(arrRows[0]["tray_calculate"].ToString());
                            if (weight_calculate == tray_calculate)
                                row["weight_calculate"] = true;
                            else
                            {
                                row["weight_calculate"] = weight_calculate;
                                row["tray_calculate"] = tray_calculate;
                            }


                            row["isMonth"] = arrRows[0]["isMonth"].ToString();
                            row["show_sttdate"] = arrRows[0]["show_sttdate"].ToString();
                            row["show_enddate"] = arrRows[0]["show_enddate"].ToString();
                            row["hide_date"] = arrRows[0]["hide_date"].ToString();
                            row["base_around_month"] = arrRows[0]["base_around_month"].ToString();
                            row["duplicate_cnt"] = arrRows[0]["cnt"].ToString();
                            row["remark"] = arrRows[0]["remark"].ToString();
                        }
                        else
                            row["weight_calculate"] = true;
                        productDt.Rows.Add(row);
                    }
                    else
                    { 
                        //품목 추가등록정보
                        DataRow[] arrRows = null;
                        arrRows = otherCostDt.Select($"product = '{seaoverDt.Rows[i]["품명"].ToString()}' " +
                                                     $"AND origin = '{seaoverDt.Rows[i]["원산지"].ToString()}'" +
                                                     $"AND sizes = '{seaoverDt.Rows[i]["규격"].ToString()}'" +
                                                     $"AND unit = '{seaoverDt.Rows[i]["SEAOVER단위"].ToString()}'");
                        //출력
                        if (arrRows.Length > 0)
                        {
                            DataRow row = productDt.NewRow();
                            
                            row["product"] = seaoverDt.Rows[i]["품명"].ToString();
                            row["origin"] = seaoverDt.Rows[i]["원산지"].ToString();
                            row["sizes"] = seaoverDt.Rows[i]["규격"].ToString();
                            double d;
                            if (double.TryParse(seaoverDt.Rows[i]["규격3"].ToString(), out d))
                                row["sizes2"] = seaoverDt.Rows[i]["규격3"].ToString();
                            row["unit"] = seaoverDt.Rows[i]["SEAOVER단위"].ToString();
                            row["cost_unit"] = seaoverDt.Rows[i]["트레이"].ToString();
                            row["isExist"] = "SEA";

                            row["group"] = arrRows[0]["group_name"].ToString();
                            //row["cost_unit"] = Convert.ToDouble(arrRows[0]["cost_unit"].ToString());
                            row["custom"] = Convert.ToDouble(arrRows[0]["custom"].ToString());
                            row["incidental_expense"] = Convert.ToDouble(arrRows[0]["incidental_expense"].ToString());
                            row["tax"] = Convert.ToDouble(arrRows[0]["tax"].ToString());
                            row["production_days"] = Convert.ToDouble(arrRows[0]["production_days"].ToString());
                            row["purchase_margin"] = Convert.ToDouble(arrRows[0]["purchase_margin"].ToString());
                            row["manager"] = arrRows[0]["manager"].ToString();
                            bool weight_calculate = Convert.ToBoolean(arrRows[0]["weight_calculate"].ToString());
                            bool tray_calculate = Convert.ToBoolean(arrRows[0]["tray_calculate"].ToString());
                            if (weight_calculate == tray_calculate)
                                row["weight_calculate"] = true;
                            else
                            {
                                row["weight_calculate"] = weight_calculate;
                                row["tray_calculate"] = tray_calculate;
                            }

                            row["isMonth"] = arrRows[0]["isMonth"].ToString();
                            row["show_sttdate"] = arrRows[0]["show_sttdate"].ToString();
                            row["show_enddate"] = arrRows[0]["show_enddate"].ToString();
                            row["hide_date"] = arrRows[0]["hide_date"].ToString();
                            row["base_around_month"] = arrRows[0]["base_around_month"].ToString();
                            row["duplicate_cnt"] = arrRows[0]["cnt"].ToString();
                            row["remark"] = arrRows[0]["remark"].ToString();

                            productDt.Rows.Add(row);
                        }
                    }
                }
            }
            //임의품목
            if (cbUnregistered.Checked && otherCostDt.Rows.Count > 0)
            {
                for (int i = 0; i < otherCostDt.Rows.Count; i++)
                {
                    DataRow row = productDt.NewRow();

                    //품목 추가등록정보
                    DataRow[] arrRows = null;
                    arrRows = seaoverDt.Select($"품명 = '{otherCostDt.Rows[i]["product"].ToString()}' " +
                                                 $"AND 원산지 = '{otherCostDt.Rows[i]["origin"].ToString()}'" +
                                                 $"AND 규격 = '{otherCostDt.Rows[i]["sizes"].ToString()}'" +
                                                 $"AND SEAOVER단위 = '{otherCostDt.Rows[i]["unit"].ToString()}'");
                    //출력
                    if (arrRows.Length == 0)
                    {
                        row["group"] = otherCostDt.Rows[i]["group_name"].ToString();
                        row["product"] = otherCostDt.Rows[i]["product"].ToString();
                        row["origin"] = otherCostDt.Rows[i]["origin"].ToString();
                        row["sizes"] = otherCostDt.Rows[i]["sizes"].ToString();

                        string tmp;
                        string sizes = otherCostDt.Rows[i]["sizes"].ToString();
                        if (sizes.Contains("/"))
                        {
                            tmp = sizes.Substring(0, sizes.IndexOf("/"));
                        }
                        else if (sizes.Contains("미"))
                        {
                            tmp = sizes.Substring(0, sizes.IndexOf("미"));
                        }
                        else if (sizes.Contains("UP"))
                        {
                            tmp = sizes.Substring(0, sizes.IndexOf("UP"));
                        }
                        else if (sizes.Contains("kg"))
                        {
                            tmp = sizes.Substring(0, sizes.IndexOf("kg"));
                        }
                        else if (sizes.Contains("L"))
                        {
                            tmp = sizes.Substring(0, sizes.IndexOf("L"));
                        }
                        else
                        {
                            tmp = "0";
                        }

                        double d;
                        if (double.TryParse(tmp, out d))
                            row["sizes2"] = d;


                        row["unit"] = otherCostDt.Rows[i]["unit"].ToString();
                        row["cost_unit"] = Convert.ToDouble(otherCostDt.Rows[i]["cost_unit"].ToString());
                        row["custom"] = Convert.ToDouble(otherCostDt.Rows[i]["custom"].ToString());
                        row["incidental_expense"] = Convert.ToDouble(otherCostDt.Rows[i]["incidental_expense"].ToString());
                        row["tax"] = Convert.ToDouble(otherCostDt.Rows[i]["tax"].ToString());
                        row["production_days"] = Convert.ToDouble(otherCostDt.Rows[i]["production_days"].ToString());
                        row["purchase_margin"] = Convert.ToDouble(otherCostDt.Rows[i]["purchase_margin"].ToString());
                        row["manager"] = otherCostDt.Rows[i]["manager"].ToString();

                        bool weight_calculate = Convert.ToBoolean(otherCostDt.Rows[i]["weight_calculate"].ToString());
                        bool tray_calculate = Convert.ToBoolean(otherCostDt.Rows[i]["tray_calculate"].ToString());
                        if (weight_calculate == tray_calculate)
                            row["weight_calculate"] = true;
                        else
                        {
                            row["weight_calculate"] = weight_calculate;
                            row["tray_calculate"] = tray_calculate;
                        }

                        row["isMonth"] = otherCostDt.Rows[i]["isMonth"].ToString();
                        row["show_sttdate"] = otherCostDt.Rows[i]["show_sttdate"].ToString();
                        row["show_enddate"] = otherCostDt.Rows[i]["show_enddate"].ToString();
                        row["hide_date"] = otherCostDt.Rows[i]["hide_date"].ToString();
                        row["base_around_month"] = otherCostDt.Rows[i]["base_around_month"].ToString();
                        row["duplicate_cnt"] = otherCostDt.Rows[i]["cnt"].ToString();
                        row["remark"] = otherCostDt.Rows[i]["remark"].ToString();
                        productDt.Rows.Add(row);
                    }
                }
            }
            //Output===================================================================================================
            if (productDt.Rows.Count > 0)
            {
                //정렬
                DataView dv = new DataView(productDt);
                dv.Sort = "product, origin, sizes2, sizes, unit";
                productDt = dv.ToTable();
                //출력
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["group"].Value = productDt.Rows[i]["group"].ToString();
                    dgvProduct.Rows[n].Cells["product"].Value = productDt.Rows[i]["product"].ToString();
                    dgvProduct.Rows[n].Cells["product_origin"].Value = productDt.Rows[i]["product"].ToString();
                    dgvProduct.Rows[n].Cells["origin"].Value = productDt.Rows[i]["origin"].ToString();
                    dgvProduct.Rows[n].Cells["origin_origin"].Value = productDt.Rows[i]["origin"].ToString();
                    dgvProduct.Rows[n].Cells["sizes"].Value = productDt.Rows[i]["sizes"].ToString();
                    dgvProduct.Rows[n].Cells["sizes_origin"].Value = productDt.Rows[i]["sizes"].ToString();
                    dgvProduct.Rows[n].Cells["sizes2"].Value = productDt.Rows[i]["sizes2"].ToString();
                    dgvProduct.Rows[n].Cells["unit"].Value = productDt.Rows[i]["unit"].ToString();
                    dgvProduct.Rows[n].Cells["unit_origin"].Value = productDt.Rows[i]["unit"].ToString();
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = productDt.Rows[i]["cost_unit"].ToString();
                    dgvProduct.Rows[n].Cells["cost_unit_origin"].Value = productDt.Rows[i]["cost_unit"].ToString();
                    dgvProduct.Rows[n].Cells["custom"].Value = productDt.Rows[i]["custom"].ToString();
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = productDt.Rows[i]["incidental_expense"].ToString();
                    dgvProduct.Rows[n].Cells["tax"].Value = productDt.Rows[i]["tax"].ToString();
                    dgvProduct.Rows[n].Cells["production_days"].Value = productDt.Rows[i]["production_days"].ToString();
                    dgvProduct.Rows[n].Cells["purchase_margin"].Value = productDt.Rows[i]["purchase_margin"].ToString();
                    dgvProduct.Rows[n].Cells["manager"].Value = productDt.Rows[i]["manager"].ToString();
                    dgvProduct.Rows[n].Cells["weight_calculate"].Value = productDt.Rows[i]["weight_calculate"];
                    dgvProduct.Rows[n].Cells["tray_calculate"].Value = productDt.Rows[i]["tray_calculate"];


                    dgvProduct.Rows[n].Cells["isMonth"].Value = productDt.Rows[i]["isMonth"];
                    dgvProduct.Rows[n].Cells["show_sttdate"].Value = productDt.Rows[i]["show_sttdate"];
                    dgvProduct.Rows[n].Cells["show_enddate"].Value = productDt.Rows[i]["show_enddate"];
                    dgvProduct.Rows[n].Cells["hide_date"].Value = productDt.Rows[i]["hide_date"];
                    dgvProduct.Rows[n].Cells["base_around_month"].Value = productDt.Rows[i]["base_around_month"];
                    dgvProduct.Rows[n].Cells["duplicate_cnt"].Value = productDt.Rows[i]["duplicate_cnt"];
                    dgvProduct.Rows[n].Cells["isExist"].Value = productDt.Rows[i]["isExist"].ToString();
                    dgvProduct.Rows[n].Cells["remark"].Value = productDt.Rows[i]["remark"].ToString();
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private void SetDgvStyleSetting()
        {
            DataGridView dgv = dgvProduct;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            Color colGreen = Color.FromArgb(153, 204, 0);
            dgv.Columns["cost_unit"].HeaderCell.Style.BackColor = colGreen;
            dgv.Columns["custom"].HeaderCell.Style.BackColor = colGreen;
            dgv.Columns["tax"].HeaderCell.Style.BackColor = colGreen;
            dgv.Columns["incidental_expense"].HeaderCell.Style.BackColor = colGreen;
            dgv.Columns["production_days"].HeaderCell.Style.BackColor = colGreen;
            dgv.Columns["purchase_margin"].HeaderCell.Style.BackColor = colGreen;
            dgv.Columns["remark"].HeaderCell.Style.BackColor = colGreen;
            dgv.Columns["manager"].HeaderCell.Style.BackColor = colGreen;

            dgv.Columns["cost_unit"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["custom"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["tax"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["incidental_expense"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["production_days"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["purchase_margin"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["remark"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["manager"].HeaderCell.Style.ForeColor = Color.Black;

            Color colLightGreen = Color.FromArgb(246, 250, 244);
            dgv.Columns["cost_unit"].DefaultCellStyle.BackColor = colLightGreen;
            dgv.Columns["custom"].DefaultCellStyle.BackColor = colLightGreen;
            dgv.Columns["tax"].DefaultCellStyle.BackColor = colLightGreen;
            dgv.Columns["incidental_expense"].DefaultCellStyle.BackColor = colLightGreen;
            dgv.Columns["production_days"].DefaultCellStyle.BackColor = colLightGreen;
            dgv.Columns["purchase_margin"].DefaultCellStyle.BackColor = colLightGreen;
            dgv.Columns["manager"].DefaultCellStyle.BackColor = colLightGreen;

            
            dgv.Columns["weight_calculate"].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns["tray_calculate"].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns["weight_calculate"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["tray_calculate"].HeaderCell.Style.ForeColor = Color.Black;

            Color col = Color.FromArgb(221, 235, 247);
            dgv.Columns["isExist"].HeaderCell.Style.BackColor = col;
            dgv.Columns["isExist"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["isExist"].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns["isExist"].DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dgv.Columns["isExist"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["isExist"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["duplicate_cnt"].HeaderCell.Style.BackColor = col;
            dgv.Columns["duplicate_cnt"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["duplicate_cnt"].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns["duplicate_cnt"].DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dgv.Columns["duplicate_cnt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["duplicate_cnt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            /*dgv.Columns["isExist"].HeaderCell.Style.Font = new Font("나눔고딕", 6, FontStyle.Regular);
            dgv.Columns["isExist"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Regular);*/
            dgv.Columns["sizes2"].Visible = false;
            dgv.Columns["unit"].Visible = true;

        }
        //SEAOVER 프로시져 호출하기
        private void CallProcedure()
        {
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string seaover_id = um.seaover_id;
            //업체별시세현황 스토어프로시져 호출
            if (seaoverRepository.CallStoredProcedure(seaover_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this, "호출 내용이 없음");
                this.Activate();
                return;
            }
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void UpdateProduct()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "품목관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            dgvProduct.EndEdit();
            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this,"수정할 내역이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                int cnt = 0;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                        cnt += 1;
                }
                if (cnt == 0)
                {
                    MessageBox.Show(this,"수정할 내역이 없습니다.");
                    this.Activate();
                    return;
                }
            }

            //등록
            if (MessageBox.Show(this,"품목정보를 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                bool allUpdate = false;
                if (MessageBox.Show(this,"매입내역(오퍼), 계약내역(팬딩)도 함께 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    allUpdate = true;

                //Sql Execute
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql;
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        foreach(DataGridViewCell cell in dgvProduct.Rows[i].Cells)
                            if(cell.Value == null)
                                cell.Value = string.Empty;

                        //Delete model
                        ProductOtherCostModel delete_model = new ProductOtherCostModel();
                        delete_model.group_name = dgvProduct.Rows[i].Cells["group"].Value.ToString();
                        delete_model.product = dgvProduct.Rows[i].Cells["product_origin"].Value.ToString();
                        delete_model.origin = dgvProduct.Rows[i].Cells["origin_origin"].Value.ToString();
                        delete_model.sizes = dgvProduct.Rows[i].Cells["sizes_origin"].Value.ToString();
                        delete_model.unit = dgvProduct.Rows[i].Cells["unit_origin"].Value.ToString();

                        //Insert model
                        ProductOtherCostModel model = new ProductOtherCostModel();
                        model.group_name = dgvProduct.Rows[i].Cells["group"].Value.ToString();
                        model.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                        model.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                        model.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                        model.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();

                        double cost_unit;
                        if (dgvProduct.Rows[i].Cells["cost_unit"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString(), out cost_unit))
                            cost_unit = 0;
                        model.cost_unit = cost_unit.ToString();
                        double custom;
                        if (dgvProduct.Rows[i].Cells["custom"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["custom"].Value.ToString(), out custom))
                            custom = 0;
                        model.custom = custom;
                        double incidental_expense;
                        if (dgvProduct.Rows[i].Cells["incidental_expense"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                            incidental_expense = 0;
                        model.incidental_expense = incidental_expense;
                        double tax;
                        if (dgvProduct.Rows[i].Cells["tax"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["tax"].Value.ToString(), out tax))
                            tax = 0;
                        model.tax = tax;
                        double production_days;
                        if (dgvProduct.Rows[i].Cells["production_days"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["production_days"].Value.ToString(), out production_days))
                            production_days = 0;
                        model.production_days = production_days;

                        double purchase_margin;
                        if (dgvProduct.Rows[i].Cells["purchase_margin"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                            purchase_margin = 0;
                        model.purchase_margin = purchase_margin;

                        model.manager = dgvProduct.Rows[i].Cells["manager"].Value.ToString();
                        
                        bool weight_calculate;
                        if (dgvProduct.Rows[i].Cells["weight_calculate"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                            weight_calculate = false;
                        bool tray_calculate;
                        if (dgvProduct.Rows[i].Cells["tray_calculate"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["tray_calculate"].Value.ToString(), out tray_calculate))
                            tray_calculate = false;
                        if (weight_calculate == tray_calculate)
                        {
                            weight_calculate = true;
                            tray_calculate = true;
                        }
                        model.weight_calculate = weight_calculate;
                        model.tray_calculate = tray_calculate;
                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        model.edit_user = um.user_name;


                        bool isMonth;
                        if (dgvProduct.Rows[i].Cells["isMonth"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["isMonth"].Value.ToString(), out isMonth))
                            isMonth = false;
                        model.isMonth = isMonth;

                        DateTime show_sttdate;
                        if (dgvProduct.Rows[i].Cells["show_sttdate"].Value == null || !DateTime.TryParse(dgvProduct.Rows[i].Cells["show_sttdate"].Value.ToString(), out show_sttdate))
                            model.show_sttdate = "NULL";
                        else
                            model.show_sttdate = "'" + show_sttdate.ToString("yyyy-MM-dd") + "'";

                        DateTime show_enddate;
                        if (dgvProduct.Rows[i].Cells["show_enddate"].Value == null || !DateTime.TryParse(dgvProduct.Rows[i].Cells["show_enddate"].Value.ToString(), out show_enddate))
                            model.show_enddate = "NULL";
                        else
                            model.show_enddate = "'" + show_enddate.ToString("yyyy-MM-dd") + "'";

                        DateTime hide_date;
                        if (dgvProduct.Rows[i].Cells["hide_date"].Value == null || !DateTime.TryParse(dgvProduct.Rows[i].Cells["hide_date"].Value.ToString(), out hide_date))
                            model.hide_date = "NULL";
                        else
                            model.hide_date = "'" + hide_date.ToString("yyyy-MM-dd") + "'";

                        double base_around_month;
                        if (dgvProduct.Rows[i].Cells["base_around_month"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["base_around_month"].Value.ToString(), out base_around_month))
                            base_around_month = 0;
                        model.base_around_month = base_around_month;

                        model.remark = dgvProduct.Rows[i].Cells["remark"].Value.ToString();

                        //Delete
                        sql = productOtherCostRepository.DeleteProduct(delete_model);
                        sqlList.Add(sql);
                        //Insert
                        sql = productOtherCostRepository.InsertProduct(model);
                        sqlList.Add(sql);


                        //오퍼, 팬딩내역 수정
                        if (allUpdate)
                        {
                            //오퍼내역 수정
                            sql = productOtherCostRepository.UpdateOffer(
                                dgvProduct.Rows[i].Cells["product_origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["origin_origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["sizes_origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["unit_origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["product"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["sizes"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["unit"].Value.ToString()
                                , weight_calculate);
                            sqlList.Add(sql);
                            //팬딩내역 수정
                            sql = productOtherCostRepository.UpdatePendding(
                                dgvProduct.Rows[i].Cells["product_origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["origin_origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["sizes_origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["unit_origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["product"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["origin"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["sizes"].Value.ToString()
                                , dgvProduct.Rows[i].Cells["unit"].Value.ToString()
                                , weight_calculate);
                            sqlList.Add(sql);
                        }
                    }
                }
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                //Transaction execute
                if (sqlList.Count > 0)
                {
                    //Execute
                    int result = productOtherCostRepository.UpdateTrans(sqlList);
                    if (result == -1)
                    {
                        MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        GetData();
                        //MessageBox.Show(this,"등록완료");
                    }
                }
            }
        }
        private void DeleteProduct()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "품목관리", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this,"삭제할 내역이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                int cnt = 0;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                        cnt += 1;
                }
                if (cnt == 0)
                {
                    MessageBox.Show(this,"삭제할 내역이 없습니다.");
                    this.Activate();
                    return;
                }
            }
            //삭제
            if (MessageBox.Show(this,"선택한 내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Sql Execute
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql;

                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        //model
                        ProductOtherCostModel model = new ProductOtherCostModel();
                        model.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                        model.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                        model.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                        model.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();

                        //Delete
                        sql = productOtherCostRepository.DeleteProduct(model);
                        sqlList.Add(sql);
                    }
                }
                //Transaction execute
                if (sqlList.Count > 0)
                {
                    //Execute
                    int result = productOtherCostRepository.UpdateTrans(sqlList);
                    if (result == -1)
                    {
                        MessageBox.Show(this,"삭제 중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        GetData();
                        MessageBox.Show(this,"삭제완료");
                        this.Activate();
                    }
                }
            }
        }
        #endregion

        #region Key event
        /*protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            if (tb.Name != "dgvProduct")
            {
                switch (keyData)
                {
                    case Keys.Left:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Right:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                    case Keys.Up:
                        if (tb.Name == "dgvProduct")
                        {
                            int idx = 0;

                            if (dgvProduct.SelectedCells.Count > 0)
                            {
                                idx = dgvProduct.SelectedCells[0].RowIndex - 1;
                            }
                            else if (dgvProduct.SelectedRows.Count > 0)
                            {
                                idx = dgvProduct.SelectedRows[0].Index - 1;
                            }

                            if (idx >= 0)
                            {
                                dgvProduct.ClearSelection();
                                dgvProduct.Rows[idx].Selected = true;
                            }
                        }
                        else
                        {
                            tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        }
                        return true;
                    case Keys.Down:
                        if (tb.Name == "dgvProduct")
                        {
                            int idx = 0;

                            if (dgvProduct.SelectedCells.Count > 0)
                            {
                                idx = dgvProduct.SelectedCells[0].RowIndex + 1;
                            }
                            else if (dgvProduct.SelectedRows.Count > 0)
                            {
                                idx = dgvProduct.SelectedRows[0].Index + 1;
                            }

                            if (idx < dgvProduct.Rows.Count)
                            {
                                dgvProduct.ClearSelection();
                                dgvProduct.Rows[idx].Selected = true;
                            }
                        }
                        else
                        {
                            tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        }
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }*/
        private void ProductManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        if (!isSelectMode)
                            UpdateProduct();
                        break;
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.S:
                        if (isSelectMode)
                        {
                            btnSelect.PerformClick();
                        }
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtGroup.Text = String.Empty;
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        cbSeaover.Checked = !cbSeaover.Checked;
                        break;
                    case Keys.F2:
                        cbUnregistered.Checked = !cbUnregistered.Checked;
                        break;
                }
            }
        }
        private void txtCheckNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(46) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))
                e.Handled = true;
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (isSelectMode)
            { 
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            if (dgvProduct.Rows.Count > 0 && dgvProduct.SelectedRows.Count > 0)
                            {
                                if (isMultiSelect)
                                { 
                                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[dgvProduct.SelectedRows[0].Index].Cells[0].Value);
                                    dgvProduct.Rows[dgvProduct.SelectedRows[0].Index].Cells[0].Value = !isChecked;
                                }
                                else
                                {
                                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[dgvProduct.SelectedRows[0].Index].Cells[0].Value);
                                    int idx = dgvProduct.SelectedRows[0].Index;
                                    dgvProduct.ClearSelection();
                                    dgvProduct.Rows[idx].Cells[0].Value = true;
                                    btnSelect.PerformClick();
                                }
                            }
                            break;
                    }
                }
            }
        }
        private void dgvProduct_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            string name = dgvProduct.CurrentCell.OwningColumn.Name;

            if (name == "custom" || name == "tax" || name == "incidental_expense")
            {
                e.Control.KeyPress += new KeyPressEventHandler(txtCheckNumeric_KeyPress);
            }
            else
            {
                e.Control.KeyPress -= new KeyPressEventHandler(txtCheckNumeric_KeyPress);
            }
        }
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 1)
            { 
                string name = dgvProduct.CurrentCell.OwningColumn.Name;
                dgvProduct.Rows[e.RowIndex].Cells["chk"].Value = true;
            }
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvProduct.EndEdit();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "weight_calculate")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value);
                    dgvProduct.Rows[e.RowIndex].Cells["tray_calculate"].Value = !isChecked;
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "tray_calculate")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["tray_calculate"].Value);
                    dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value = !isChecked;
                }
            }
        }

        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //매입단가 일괄등록
            if (e.RowIndex >= 0)
            { 
                if (pupi != null || gpm != null || uam != null || ca != null || cpts != null || pdb != null || pdb2 != null)
                {
                    if (dgvProduct.Rows.Count > 0)
                    {
                        productInfoList = new List<string[]>();
                        productInfo = new string[14];
                        productInfo[0] = dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString();
                        productInfo[1] = dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString();
                        productInfo[2] = dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString();
                        productInfo[3] = dgvProduct.Rows[e.RowIndex].Cells["unit"].Value.ToString();
                        productInfo[4] = dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value.ToString();

                        productInfo[5] = dgvProduct.Rows[e.RowIndex].Cells["tax"].Value.ToString();
                        productInfo[6] = dgvProduct.Rows[e.RowIndex].Cells["custom"].Value.ToString();
                        productInfo[7] = dgvProduct.Rows[e.RowIndex].Cells["incidental_expense"].Value.ToString();
                        productInfo[8] = dgvProduct.Rows[e.RowIndex].Cells["manager"].Value.ToString();

                        productInfo[9] = "0";                //오퍼가
                        productInfo[10] = "0";               //아소트
                        productInfo[11] = "임의 거래처";     //거래처


                        bool weight_calculate = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value);
                        bool tray_calculate = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["tray_calculate"].Value);

                        if (weight_calculate == tray_calculate)
                        {
                            weight_calculate = true;
                            tray_calculate = false;
                        }

                        productInfo[12] = weight_calculate.ToString();
                        productInfo[13] = tray_calculate.ToString();

                        productInfoList.Add(productInfo);
                    }
                    if(pupi != null)
                        pupi.AddProduct(productInfoList);
                    else if(gpm != null)
                        gpm.AddProduct(productInfoList);
                    else if (uam != null)
                        uam.AddProduct(productInfoList);
                    else if (ca != null)
                        ca.AddProduct2(productInfoList);
                    else if (cpts != null)
                        cpts.AddProduct2(productInfoList);
                    else if (pdb != null)
                        pdb.InputProduct(productInfoList);
                    else if (pdb2 != null)
                        pdb2.InputProduct(productInfoList);

                }
                //그룹관리
                else if (gm != null)
                {
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        string item_code = dgvProduct.SelectedRows[0].Cells["product"].Value.ToString()
                            + "^" + dgvProduct.SelectedRows[0].Cells["origin"].Value.ToString()
                            + "^" + dgvProduct.SelectedRows[0].Cells["sizes"].Value.ToString()
                            + "^" + dgvProduct.SelectedRows[0].Cells["unit"].Value.ToString()
                            + "^" + dgvProduct.SelectedRows[0].Cells["unit"].Value.ToString()
                            + "^" + dgvProduct.SelectedRows[0].Cells["cost_unit"].Value.ToString();

                        gm.AddItem(item_code);
                    }
                }
            
                else
                {
                    dgvProduct.EndEdit();
                    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    {
                        if (isSelectMode)
                        {
                            productInfo = new string[12];
                            productInfo[0] = dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString();
                            productInfo[1] = dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString();
                            productInfo[2] = dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString();
                            productInfo[3] = dgvProduct.Rows[e.RowIndex].Cells["unit"].Value.ToString();
                            if (dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value != null)
                                productInfo[4] = dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value.ToString();
                            else
                                productInfo[4] = "0";
                            productInfo[5] = "0";
                            productInfo[6] = "0";
                            productInfo[7] = "임의 거래처";

                            bool weight_calculate = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value);
                            bool tray_calculate = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["tray_calculate"].Value);
                            if (weight_calculate == tray_calculate)
                            {
                                weight_calculate = true;
                                tray_calculate = false;
                            }

                            productInfo[8] = weight_calculate.ToString();
                            productInfo[9] = tray_calculate.ToString();


                            productInfoList = new List<string[]>();
                            productInfoList.Add(productInfo);
                            this.Dispose();
                        }
                    }
                }
            }
        }
        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (dgvProduct.SelectedRows.Count <= 1)
                    {
                        dgvProduct.ClearSelection();
                        dgvProduct.Rows[e.RowIndex].Selected = true;
                    }
                    else if (dgvProduct.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                        {
                            dgvProduct.Rows[cell.RowIndex].Selected = true;
                        }
                    }
                }
                else
                {
                    if (isSelectMode)
                    {
                        if (!isMultiSelect)
                        {
                            dgvProduct.ClearSelection();
                            foreach (DataGridViewRow row in dgvProduct.Rows)
                            {
                                row.Cells[0].Value = false;
                            }
                            dgvProduct.Rows[e.RowIndex].Cells[0].Value = true;
                        }
                    }
                }
            }
        }
        #endregion

        #region Button event
        private void btnDeleteDuplicate_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "품목관리", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();

            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                DataGridViewRow row = dgvProduct.Rows[i];
                int cnt;
                if (row.Cells["duplicate_cnt"].Value == null || !int.TryParse(row.Cells["duplicate_cnt"].Value.ToString(), out cnt))
                    cnt = 0;

                if (cnt > 1)
                {
                    //model
                    ProductOtherCostModel model = new ProductOtherCostModel();
                    model.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                    model.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                    model.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                    model.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();

                    //Delete
                    sql = productOtherCostRepository.DeleteProduct(model);
                    sqlList.Add(sql);

                    //Insert
                    ProductOtherCostModel addModel = new ProductOtherCostModel();
                    addModel.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                    addModel.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                    addModel.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                    addModel.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                    addModel.cost_unit = dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString();

                    double tax;
                    if (dgvProduct.Rows[i].Cells["tax"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["tax"].Value.ToString(), out tax))
                        tax = 0;
                    addModel.tax = tax;
                    double custom;
                    if (dgvProduct.Rows[i].Cells["custom"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["custom"].Value.ToString(), out custom))
                        custom = 0;
                    addModel.custom = custom;
                    double incidental_expense;
                    if (dgvProduct.Rows[i].Cells["incidental_expense"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                        incidental_expense = 0;
                    addModel.incidental_expense = incidental_expense;

                    bool weight_calculate;
                    if (dgvProduct.Rows[i].Cells["weight_calculate"].Value == null || !Boolean.TryParse(dgvProduct.Rows[i].Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                        weight_calculate = true;
                    addModel.weight_calculate = weight_calculate;
                    addModel.tray_calculate = !weight_calculate;

                    addModel.manager = dgvProduct.Rows[i].Cells["manager"].Value.ToString();
                    addModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    double production_days;
                    if (dgvProduct.Rows[i].Cells["production_days"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["production_days"].Value.ToString(), out production_days))
                        production_days = 0;
                    addModel.production_days = production_days;

                    bool isMonth;
                    if (dgvProduct.Rows[i].Cells["isMonth"].Value == null || !Boolean.TryParse(dgvProduct.Rows[i].Cells["isMonth"].Value.ToString(), out isMonth))
                        isMonth = false;
                    addModel.isMonth = isMonth;

                    DateTime show_sttdate;
                    if (dgvProduct.Rows[i].Cells["show_sttdate"].Value != null && DateTime.TryParse(dgvProduct.Rows[i].Cells["show_sttdate"].Value.ToString(), out show_sttdate))
                        addModel.show_sttdate = show_sttdate.ToString("yyyy-MM-dd");

                    DateTime show_enddate;
                    if (dgvProduct.Rows[i].Cells["show_enddate"].Value != null && DateTime.TryParse(dgvProduct.Rows[i].Cells["show_enddate"].Value.ToString(), out show_enddate))
                        addModel.show_enddate = show_enddate.ToString("yyyy-MM-dd");

                    DateTime hide_date;
                    if (dgvProduct.Rows[i].Cells["hide_date"].Value != null && DateTime.TryParse(dgvProduct.Rows[i].Cells["hide_date"].Value.ToString(), out hide_date))
                        addModel.hide_date = hide_date.ToString("yyyy-MM-dd");

                    double purchase_margin;
                    if (dgvProduct.Rows[i].Cells["purchase_margin"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                        purchase_margin = 0;
                    addModel.purchase_margin = purchase_margin;

                    double base_around_month;
                    if (dgvProduct.Rows[i].Cells["base_around_month"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["base_around_month"].Value.ToString(), out base_around_month))
                        base_around_month = 0;
                    addModel.base_around_month = base_around_month;

                    sql = productOtherCostRepository.InsertProduct(addModel);
                    sqlList.Add(sql);
                }
            }
            if (sqlList.Count > 0)
            {
                if (productOtherCostRepository.UpdateTrans(sqlList) == -1)
                {
                    MessageBox.Show(this, "수정중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    GetData();
                    MessageBox.Show(this, "완료");
                    this.Activate();
                }

            }
        }
        private void btnIncomList_Click(object sender, EventArgs e)
        {
            IncomeList il = new IncomeList(um);
            il.Owner = this;
            il.Show();
        }
        private void btnInsert_Click_1(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "품목관리", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            AddProduct ap = new AddProduct(um);
            ap.ShowDialog();
            GetData();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            UpdateProduct();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteProduct();
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            //매입단가 일괄등록
            if (pupi != null || gpm != null || uam != null || ca != null || cpts != null || pdb != null || pdb2 != null)
            {
                if (dgvProduct.Rows.Count > 0)
                {
                    productInfoList = new List<string[]>();
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        bool isChecked = dgvProduct.Rows[i].Selected;
                        if (isChecked)
                        {
                            productInfo = new string[15];
                            productInfo[0] = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                            productInfo[1] = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                            productInfo[2] = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                            productInfo[3] = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                            productInfo[4] = dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString();
                            productInfo[5] = dgvProduct.Rows[i].Cells["tax"].Value.ToString();
                            productInfo[6] = dgvProduct.Rows[i].Cells["custom"].Value.ToString();
                            productInfo[7] = dgvProduct.Rows[i].Cells["incidental_expense"].Value.ToString();
                            productInfo[8] = dgvProduct.Rows[i].Cells["manager"].Value.ToString();

                            productInfo[9] = "0";                //오퍼가
                            productInfo[10] = "0";               //아소트
                            productInfo[11] = "임의 거래처";     //거래처


                            bool weight_calculate = Convert.ToBoolean(dgvProduct.Rows[i].Cells["weight_calculate"].Value);
                            bool tray_calculate = Convert.ToBoolean(dgvProduct.Rows[i].Cells["tray_calculate"].Value);

                            if (weight_calculate == tray_calculate)
                            {
                                weight_calculate = true;
                                tray_calculate = false;
                            }
                            productInfo[12] = weight_calculate.ToString();
                            productInfo[13] = tray_calculate.ToString();
                            productInfo[14] = dgvProduct.Rows[i].Cells["purchase_margin"].Value.ToString();

                            productInfoList.Add(productInfo);
                        }
                    }
                }
                if (pupi != null)
                    pupi.AddProduct(productInfoList);
                else if (gpm != null)
                    gpm.AddProduct(productInfoList);
                else if (uam != null)
                    uam.AddProduct(productInfoList);
                else if (ca != null)
                    ca.AddProduct2(productInfoList);
                else if (cpts != null)
                    cpts.AddProduct2(productInfoList);
                else if (pdb != null)
                    pdb.InputProduct(productInfoList);
                else if (pdb2 != null)
                    pdb2.InputProduct(productInfoList);
            }
            //그룹관리
            else if (gm != null)
            {
                if (dgvProduct.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        if (dgvProduct.Rows[i].Selected)
                        { 
                            string item_code = dgvProduct.Rows[i].Cells["product"].Value.ToString()
                                + "^" + dgvProduct.Rows[i].Cells["origin"].Value.ToString()
                                + "^" + dgvProduct.Rows[i].Cells["sizes"].Value.ToString()
                                + "^" + dgvProduct.Rows[i].Cells["unit"].Value.ToString()
                                + "^" + dgvProduct.Rows[i].Cells["unit"].Value.ToString()
                                + "^" + dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString();

                            gm.AddItem(item_code);
                        }
                    }
                }
            }
            else
            {
                dgvProduct.EndEdit();
                if (dgvProduct.Rows.Count > 0)
                {
                    if (isSelectMode)
                    {
                        //단일선택
                        if (!isMultiSelect)
                        {
                            for (int i = 0; i < dgvProduct.Rows.Count; i++)
                            {
                                bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells[0].Selected);
                                if (isChecked)
                                {
                                    productInfo = new string[12];
                                    productInfo[0] = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                                    productInfo[1] = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                                    productInfo[2] = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                                    productInfo[3] = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                                    productInfo[4] = dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString();
                                    productInfo[5] = "0";
                                    productInfo[6] = "0";
                                    productInfo[7] = "임의 거래처";

                                    productInfo[8] = dgvProduct.Rows[i].Cells["custom"].Value.ToString();
                                    productInfo[9] = dgvProduct.Rows[i].Cells["tax"].Value.ToString();
                                    productInfo[10] = dgvProduct.Rows[i].Cells["incidental_expense"].Value.ToString();
                                    productInfo[11] = dgvProduct.Rows[i].Cells["purchase_margin"].Value.ToString();

                                    break;
                                }
                            }
                        }
                        //단중선택
                        else
                        {
                            productInfoList = new List<string[]>();
                            for (int i = 0; i < dgvProduct.Rows.Count; i++)
                            {
                                bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells[0].Selected);
                                if (isChecked)
                                {
                                    productInfo = new string[12];
                                    productInfo[0] = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                                    productInfo[1] = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                                    productInfo[2] = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                                    productInfo[3] = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                                    productInfo[4] = dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString();
                                    productInfo[5] = "0";
                                    productInfo[6] = "0";
                                    productInfo[7] = "임의 거래처";
                                    productInfo[8] = dgvProduct.Rows[i].Cells["custom"].Value.ToString();
                                    productInfo[9] = dgvProduct.Rows[i].Cells["tax"].Value.ToString();
                                    productInfo[10] = dgvProduct.Rows[i].Cells["incidental_expense"].Value.ToString();
                                    productInfo[11] = dgvProduct.Rows[i].Cells["purchase_margin"].Value.ToString();

                                    productInfoList.Add(productInfo);
                                }
                            }
                        }
                        this.Dispose();
                    }
                }
            }
        }
        #endregion

        #region 우클릭 메뉴
        private void dgvProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();

                    m.Items.Add("선택/해제");
                    m.Items.Add("규격추가");
                    m.Items.Add("박스디자인 폴더");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvProduct, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch
            {

            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (dgvProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvProduct.SelectedRows[0];
                    if (dr.Index < 0)
                    {
                        return;
                    }

                    int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                    /*PendingInfo p;*/

                    switch (e.ClickedItem.Text)
                    {
                        case "선택/해제":

                            bool isChecked = !Convert.ToBoolean(dgvProduct.SelectedRows[0].Cells["chk"].Value);
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                row.Cells["chk"].Value = isChecked;
                            }
                            dgvProduct.EndEdit();
                            break;
                        case "규격추가":
                            {
                                ProductOtherCostModel pModel = new ProductOtherCostModel();
                                pModel.group_name = dr.Cells["group"].Value.ToString();
                                pModel.product = dr.Cells["product"].Value.ToString();
                                pModel.origin = dr.Cells["origin"].Value.ToString();
                                pModel.sizes = dr.Cells["sizes"].Value.ToString();
                                pModel.unit = dr.Cells["unit"].Value.ToString();
                                pModel.cost_unit = dr.Cells["cost_unit"].Value.ToString();

                                if(dr.Cells["custom"].Value != null && double.TryParse(dr.Cells["custom"].Value.ToString(), out double custom))
                                    pModel.custom = custom;

                                if (dr.Cells["tax"].Value != null && double.TryParse(dr.Cells["tax"].Value.ToString(), out double tax))
                                    pModel.custom = tax;

                                if (dr.Cells["incidental_expense"].Value != null && double.TryParse(dr.Cells["incidental_expense"].Value.ToString(), out double incidental_expense))
                                    pModel.custom = incidental_expense;

                                if (dr.Cells["production_days"].Value != null && double.TryParse(dr.Cells["production_days"].Value.ToString(), out double production_days))
                                    pModel.custom = production_days;

                                if (dr.Cells["purchase_margin"].Value != null && double.TryParse(dr.Cells["purchase_margin"].Value.ToString(), out double purchase_margin))
                                    pModel.custom = purchase_margin;

                                bool isWeight;
                                if (dr.Cells["weight_calculate"].Value == null || !bool.TryParse(dr.Cells["weight_calculate"].Value.ToString(), out isWeight))
                                    isWeight = true;
                                pModel.weight_calculate = isWeight;

                                pModel.manager = dr.Cells["manager"].Value.ToString();

                                AddProduct ap = new AddProduct(um, pModel);
                                ap.ShowDialog();
                            }
                            break;
                        case "박스디자인 폴더":

                            if (dr.Cells["isExist"].Value != null && dr.Cells["isExist"].Value.ToString() == "SEA")
                            {
                                CreateBoxDesignFolderManager cbdfm = new CreateBoxDesignFolderManager(dr.Cells["product"].Value.ToString()
                                                                                                    , dr.Cells["origin"].Value.ToString()
                                                                                                    , dr.Cells["manager"].Value.ToString());
                                cbdfm.Owner = this;
                                cbdfm.ShowDialog();
                            }
                            else
                                messageBox.Show(this, "SEAOVER에 등록된 품목만 폴더를 생성할 수 있습니다!");
                            


                            break;
                    }
                }
                catch 
                {
                }
            }
        }

        

        #endregion

        #region Datagridview 멀티헤더

        private void dgvProduct_Paint(object sender, PaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            string[] strHeaders = { "품목정보", "추가정보", "계산방식", "기타"};
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            // 품목정보
            {
                Rectangle r1 = gv.GetCellDisplayRectangle(0, -1, false);
                int width1 = gv.GetCellDisplayRectangle(1, -1, false).Width;
                int width2 = gv.GetCellDisplayRectangle(2, -1, false).Width;
                int width3 = gv.GetCellDisplayRectangle(3, -1, false).Width;
                int width4 = gv.GetCellDisplayRectangle(4, -1, false).Width;
                int width5 = gv.GetCellDisplayRectangle(5, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 + width2 + width3 + width4 + width5 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(43, 94, 170);
                e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                e.Graphics.DrawString(strHeaders[0], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            // 입력정보
            {
                Rectangle r1 = gv.GetCellDisplayRectangle(6, -1, false);
                int width1 = gv.GetCellDisplayRectangle(7, -1, false).Width;
                int width2 = gv.GetCellDisplayRectangle(8, -1, false).Width;
                int width3 = gv.GetCellDisplayRectangle(9, -1, false).Width;
                int width4 = gv.GetCellDisplayRectangle(10, -1, false).Width;
                int width5 = gv.GetCellDisplayRectangle(11, -1, false).Width;
                int width6 = gv.GetCellDisplayRectangle(12, -1, false).Width;
                int width7 = gv.GetCellDisplayRectangle(13, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 + width2 + width3 + width4 + width5 + width6 + width7 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colGreen = Color.FromArgb(153, 204, 0);
                e.Graphics.FillRectangle(new SolidBrush(colGreen), r1);
                e.Graphics.DrawString(strHeaders[1], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(Color.Black), r1, format);
            }
            // 계산방식
            {
                Rectangle r1 = gv.GetCellDisplayRectangle(14, -1, false);
                int width1 = gv.GetCellDisplayRectangle(15, -1, false).Width;
                //int width2 = gv.GetCellDisplayRectangle(15, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color col = Color.Beige;
                e.Graphics.FillRectangle(new SolidBrush(col), r1);
                e.Graphics.DrawString(strHeaders[2], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(Color.Black), r1, format);
            }
        }
        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r = e.CellBounds;
                r.Y += e.CellBounds.Height / 2;
                r.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r, true);
                e.PaintContent(r);
                e.Handled = true;
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                string name = dgvProduct.CurrentCell.OwningColumn.Name;
                bool isCheck = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["chk"].Value);
                if (isCheck)
                {
                    dgvProduct.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.Red;
                }
                else
                {
                    dgvProduct.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.LightGray;
                }
            }
        }

        private void dgvProduct_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void dgvProduct_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AdoNetWindow.API
{
    public partial class HACCPCompany : Form
    {
        const string targetUrl = "http://apis.data.go.kr/B553748/FoodCompanyListService/getFoodCompanyList";
        const string serviceKey = "1ZMlHLxV3Y%2F6641V7AhY3uuks7lXai3KvDY0m2KVYRtjL3IBO91QuDJuquPJuuWARBSa68Di423WLkLeSS4jdA%3D%3D";
        DataTable newDt;

        delegate void GetAllData();

        public HACCPCompany()
        {
            InitializeComponent();
        }
        private void HACCPCompany_Load(object sender, EventArgs e)
        {
            GetData();
        }

        #region Method
        private void SetData(int sttIdx, int endIdx)
        {
            for (int i = sttIdx; i <= endIdx; i++)
            {
                string result = getApi(i);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlNodeList list = xml.GetElementsByTagName("item");
                //Convert to datatable
                ListViewItem itm;
                foreach (XmlNode nd in list)
                {
                    if (nd.InnerText.Length > 0)
                    {
                        DataRow dr = newDt.NewRow();

                        dr["appointno"] = nd["appointno"].InnerText;                //인증번호
                        dr["licenseno"] = nd["licenseno"].InnerText;                //인허가번호
                        dr["company"] = nd["company"].InnerText;                    //회사명
                        dr["businessnm"] = nd["businessnm"].InnerText;              //업종
                        dr["species"] = nd["species"].InnerText;                    //품목유형
                        dr["ceoname"] = nd["ceoname"].InnerText;                    //대표자명
                        dr["sido"] = nd["sido"].InnerText;                          //시도
                        dr["sgg"] = nd["sgg"].InnerText;                            //시군구
                        dr["issuedate"] = nd["issuedate"].InnerText;                //인증시작일
                        dr["issueenddate"] = nd["issueenddate"].InnerText;          //인증종료일

                        newDt.Rows.Add(dr);
                    }
                }
            }
        }


        private void GetApiData()
        {
            SetTable();
            //Get Data (Xml)
            string result = getApi(1);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            XmlNodeList list = xml.GetElementsByTagName("body");
            int totalCount = 0;
            foreach (XmlNode nd in list)
            {
                totalCount = Convert.ToInt32(nd["totalCount"].InnerText.ToString()) / 100;
                break;
            }
            //데이터 불러오기
            int data_count = 0;
            if (totalCount > 0)
            {
                for (int i = 1; i <= totalCount; i++)
                {
                    result = getApi(i);
                    xml = new XmlDocument();
                    xml.LoadXml(result);
                    list = xml.GetElementsByTagName("item");
                    //Convert to datatable
                    ListViewItem itm;
                    foreach (XmlNode nd in list)
                    {
                        if (nd.InnerText.Length > 0)
                        {
                            DataRow dr = newDt.NewRow();

                            dr["appointno"] = nd["appointno"].InnerText;                //인증번호
                            dr["licenseno"] = nd["licenseno"].InnerText;                //인허가번호
                            dr["company"] = nd["company"].InnerText;                    //회사명
                            dr["businessnm"] = nd["businessnm"].InnerText;              //업종
                            dr["species"] = nd["species"].InnerText;                    //품목유형
                            dr["ceoname"] = nd["ceoname"].InnerText;                    //대표자명
                            dr["sido"] = nd["sido"].InnerText;                          //시도
                            dr["sgg"] = nd["sgg"].InnerText;                            //시군구
                            dr["issuedate"] = nd["issuedate"].InnerText;                //인증시작일
                            dr["issueenddate"] = nd["issueenddate"].InnerText;          //인증종료일

                            newDt.Rows.Add(dr);
                            data_count++;
                        }
                    }
                }
            }
            txtDataCount.Text = data_count.ToString();
        }

        private void GetData()
        {
            if (newDt == null ||newDt.Rows.Count == 0)
            {
                MessageBox.Show(this, "호출한 데이터가 없습니다. 'API 데이터 불러오기' 버튼을 통해 먼저 데이터를 불러와주시기 바랍니다.");
                return;
            }

            string company = txtCompany.Text;
            string business_type = txtBusinessType.Text;

            DataTable dt = newDt.Clone();
            if (!string.IsNullOrEmpty(company.Trim()) || !string.IsNullOrEmpty(business_type.Trim()))
            {
                string whr = "1=1";

                if (!string.IsNullOrEmpty(company.Trim()))
                    whr += $" AND company LIKE '%{company.Trim()}%'";
                if (!string.IsNullOrEmpty(business_type.Trim()))
                    whr += $" AND businessnm LIKE '%{business_type.Trim()}%'";

                DataRow[] dr = dt.Select(whr);
                dt = dr.CopyToDataTable();

                dgvCompany.DataSource = dt;
                ColumnNameSetting();
            }


            /*int page;
            if (!int.TryParse(txtPageNo.Text, out page))
            {
                MessageBox.Show(this,"페이지 값은 숫자만 입력할 수 있습니다.");
                return;
            }

            SetTable();
            //Get Data (Xml)
            string result = getApi(page);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            XmlNodeList list = xml.GetElementsByTagName("body");
            int totalCount = 0;
            foreach (XmlNode nd in list)
            {
                totalCount = Convert.ToInt32(nd["totalCount"].InnerText.ToString());
            }

            xml = new XmlDocument();
            xml.LoadXml(result);
            list = xml.GetElementsByTagName("item");
            //Convert to datatable
            ListViewItem itm;
            foreach (XmlNode nd in list)
            {
                if (nd.InnerText.Length > 0)
                {
                    DataRow dr = newDt.NewRow();

                    dr["appointno"] = nd["appointno"].InnerText;                //인증번호
                    dr["licenseno"] = nd["licenseno"].InnerText;                //인허가번호
                    dr["company"] = nd["company"].InnerText;                    //회사명
                    dr["businessnm"] = nd["businessnm"].InnerText;              //업종
                    dr["species"] = nd["species"].InnerText;                    //품목유형
                    dr["ceoname"] = nd["ceoname"].InnerText;                    //대표자명
                    dr["sido"] = nd["sido"].InnerText;                          //시도
                    dr["sgg"] = nd["sgg"].InnerText;                            //시군구
                    dr["issuedate"] = nd["issuedate"].InnerText;                //인증시작일
                    dr["issueenddate"] = nd["issueenddate"].InnerText;          //인증종료일

                    newDt.Rows.Add(dr);
                }
            }
            dgvCompany.DataSource = newDt;
            ColumnNameSetting();*/
        }
        private static string getApi(int pageNo)
        {
            if (pageNo < 1) pageNo = 1;

            string result = string.Empty;
            try
            {
                WebClient client = new WebClient();
                string url = string.Format(@"{0}?serviceKey={1}&numOfRows={2}&pageNo={3}&returntype={4}", targetUrl, serviceKey, 100, pageNo, "xml");

                client.Encoding = Encoding.UTF8;

                using (Stream data = client.OpenRead(url))
                {
                    using (StreamReader rd = new StreamReader(data))
                    {
                        string s = rd.ReadToEnd();
                        result = s;

                        rd.Close();
                        data.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }

        private void ColumnNameSetting()
        {
            if (dgvCompany.Columns.Count > 0)
            {
                for (int i = 0; i < dgvCompany.Columns.Count; i++)
                {
                    dgvCompany.Columns[i].HeaderText = newDt.Columns[i].Caption;
                }
            }
        }
        private void SetTable()
        {
            newDt = new DataTable();

            DataColumn col01 = new DataColumn();
            col01.DataType = System.Type.GetType("System.String");
            col01.AllowDBNull = false;
            col01.ColumnName = "appointno";
            col01.Caption = "인증번호";
            col01.DefaultValue = "";
            newDt.Columns.Add(col01);

            DataColumn col02 = new DataColumn();
            col02.DataType = System.Type.GetType("System.String");
            col02.AllowDBNull = false;
            col02.ColumnName = "licenseno";
            col02.Caption = "인허가번호";
            col02.DefaultValue = "";
            newDt.Columns.Add(col02);

            DataColumn col03 = new DataColumn();
            col03.DataType = System.Type.GetType("System.String");
            col03.AllowDBNull = false;
            col03.ColumnName = "company";
            col03.Caption = "회사명";
            col03.DefaultValue = "";
            newDt.Columns.Add(col03);

            DataColumn col04 = new DataColumn();
            col04.DataType = System.Type.GetType("System.String");
            col04.AllowDBNull = false;
            col04.ColumnName = "businessnm";
            col04.Caption = "업종";
            col04.DefaultValue = "";
            newDt.Columns.Add(col04);

            DataColumn col05 = new DataColumn();
            col05.DataType = System.Type.GetType("System.String");
            col05.AllowDBNull = false;
            col05.ColumnName = "species";
            col05.Caption = "품목유형";
            col05.DefaultValue = "";
            newDt.Columns.Add(col05);

            DataColumn col06 = new DataColumn();
            col06.DataType = System.Type.GetType("System.String");
            col06.AllowDBNull = false;
            col06.ColumnName = "ceoname";
            col06.Caption = "대표자명";
            col06.DefaultValue = "";
            newDt.Columns.Add(col06);

            DataColumn col07 = new DataColumn();
            col07.DataType = System.Type.GetType("System.String");
            col07.AllowDBNull = false;
            col07.ColumnName = "sido";
            col07.Caption = "시도";
            col07.DefaultValue = "";
            newDt.Columns.Add(col07);

            DataColumn col08 = new DataColumn();
            col08.DataType = System.Type.GetType("System.String");
            col08.AllowDBNull = false;
            col08.ColumnName = "sgg";
            col08.Caption = "시군구";
            col08.DefaultValue = "";
            newDt.Columns.Add(col08);

            DataColumn col09 = new DataColumn();
            col09.DataType = System.Type.GetType("System.String");
            col09.AllowDBNull = false;
            col09.ColumnName = "issuedate";
            col09.Caption = "인증시작일";
            col09.DefaultValue = "";
            newDt.Columns.Add(col09);

            DataColumn col10 = new DataColumn();
            col10.DataType = System.Type.GetType("System.String");
            col10.AllowDBNull = false;
            col10.ColumnName = "issueenddate";
            col10.Caption = "인증종료일";
            col10.DefaultValue = "";
            newDt.Columns.Add(col10);
        }
        #endregion

        #region Key event
        private void txtPageNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void HACCPCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }

        #endregion

        #region Button
        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "데이터 호출시 약간의 시간이 소요됩니다. 호출하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                GetApiData();
        }
        private void btnLeft_Click(object sender, EventArgs e)
        {
            int page;
            if (!int.TryParse(txtPageNo.Text, out page))
                page = 1;

            if (page > 1)
                page--;
            txtPageNo.Text = page.ToString();
            GetData();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            int page;
            if (!int.TryParse(txtPageNo.Text, out page))
                page = 1;

            page++;
            txtPageNo.Text = page.ToString();
            GetData();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        #endregion

        
    }
}

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
    public partial class ImportProductPermitInfo : Form
    {
        const string targetUrl = "http://apis.data.go.kr/1471000/ImportFoodPrmisnInfoService202110";
        const string serviceKey = "1ZMlHLxV3Y%2F6641V7AhY3uuks7lXai3KvDY0m2KVYRtjL3IBO91QuDJuquPJuuWARBSa68Di423WLkLeSS4jdA%3D%3D";
        DataTable newDt;
        public ImportProductPermitInfo()
        {
            InitializeComponent();
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


        private void GetData()
        {
            int page;
            if (!int.TryParse(txtPageNo.Text, out page))
            {
                MessageBox.Show(this, "페이지 값은 숫자만 입력할 수 있습니다.");
                return;
            }
            SetTable();
            //Get Data (Xml)
            string result = getApi(page);
            if (!string.IsNullOrEmpty(result))
            { 
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

                        //dr["ACCEPT_DATE"] = nd["ACCEPT_DATE"].InnerText;                               //인증번호
                        dr["NOTICE_GOODS_NAME"] = nd["NOTICE_GOODS_NAME"].InnerText;                                   //인허가번호
                        dr["GOODS_ENG_NAME"] = nd["GOODS_ENG_NAME"].InnerText;                               //회사명
                        dr["GOODS_KOR_NAME"] = nd["GOODS_KOR_NAME"].InnerText;                               //업종
                        dr["MANUFAC_COUNTRY_NAME"] = nd["MANUFAC_COUNTRY_NAME"].InnerText;                           //품목유형
                        dr["EXPORT_COUNTRY_NAME"] = nd["EXPORT_COUNTRY_NAME"].InnerText;                         //대표자명
                        dr["FLOW_FROM_DATE"] = nd["FLOW_FROM_DATE"].InnerText;                         //시도
                        dr["FROM_TO_DATE"] = nd["FROM_TO_DATE"].InnerText;                          //시군구
                        dr["NOTICE_MANUFAC_NAME"] = nd["NOTICE_MANUFAC_NAME"].InnerText;                                         //인증시작일
                        dr["FAIL_REASON_DETAIL"] = nd["FAIL_REASON_DETAIL"].InnerText;                             //인증종료일

                        newDt.Rows.Add(dr);
                    }
                }
                dgvCompany.DataSource = newDt;
                ColumnNameSetting();
            }
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
        private string getApi(int pageNo)
        {
            if (pageNo < 1) pageNo = 1;

            string result = string.Empty;
            try
            {
                WebClient client = new WebClient();
                /*string url = string.Format(@"{0}?ServiceKey={1}&numOfRows={2}&pageNo={3}&type={4}", targetUrl, serviceKey, 100, pageNo, "xml");*/
                string url = "http://apis.data.go.kr/1471000/ImportFoodPrmisnInfoService202110/getInspctImproptList"; // URL
                url += "?ServiceKey=" + serviceKey; // Service Key
                /*url += "&goods_eng_name=MUSHROOMEXTRACT";
                url += "&goods_kor_name=표고버섯추출물";*/
                url += "&pageNo=" + pageNo;
                url += "&numOfRows=100";
                url += "&type=xml";
                if (!string.IsNullOrEmpty(txtProductEng.Text))
                    url = url + string.Format(@"&goods_eng_name={0}", txtProductEng.Text);
                if (!string.IsNullOrEmpty(txtProductKor.Text))
                    url = url + string.Format(@"&goods_kor_name={0}", txtProductKor.Text);



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
                MessageBox.Show(this,e.Message);
            }
            return result;
        }

        private void SetTable()
        {
            newDt = new DataTable();

            DataColumn col01 = new DataColumn();
            col01.DataType = System.Type.GetType("System.String");
            col01.AllowDBNull = false;
            col01.ColumnName = "ACCEPT_DATE";
            col01.Caption = "처리일자";
            col01.DefaultValue = "";
            newDt.Columns.Add(col01);

            DataColumn col02 = new DataColumn();
            col02.DataType = System.Type.GetType("System.String");
            col02.AllowDBNull = false;
            col02.ColumnName = "NOTICE_GOODS_NAME";
            col02.Caption = "품목명";
            col02.DefaultValue = "";
            newDt.Columns.Add(col02);

            DataColumn col03 = new DataColumn();
            col03.DataType = System.Type.GetType("System.String");
            col03.AllowDBNull = false;
            col03.ColumnName = "GOODS_ENG_NAME";
            col03.Caption = "제품명(영문)";
            col03.DefaultValue = "";
            newDt.Columns.Add(col03);

            DataColumn col04 = new DataColumn();
            col04.DataType = System.Type.GetType("System.String");
            col04.AllowDBNull = false;
            col04.ColumnName = "GOODS_KOR_NAME";
            col04.Caption = "제품명(한글)";
            col04.DefaultValue = "";
            newDt.Columns.Add(col04);

            DataColumn col05 = new DataColumn();
            col05.DataType = System.Type.GetType("System.String");
            col05.AllowDBNull = false;
            col05.ColumnName = "MANUFAC_COUNTRY_NAME";
            col05.Caption = "제조(생산)국가";
            col05.DefaultValue = "";
            newDt.Columns.Add(col05);

            DataColumn col06 = new DataColumn();
            col06.DataType = System.Type.GetType("System.String");
            col06.AllowDBNull = false;
            col06.ColumnName = "EXPORT_COUNTRY_NAME";
            col06.Caption = "수출국가";
            col06.DefaultValue = "";
            newDt.Columns.Add(col06);

            DataColumn col07 = new DataColumn();
            col07.DataType = System.Type.GetType("System.String");
            col07.AllowDBNull = false;
            col07.ColumnName = "FLOW_FROM_DATE";
            col07.Caption = "유통유효개시일자";
            col07.DefaultValue = "";
            newDt.Columns.Add(col07);

            DataColumn col08 = new DataColumn();
            col08.DataType = System.Type.GetType("System.String");
            col08.AllowDBNull = false;
            col08.ColumnName = "FROM_TO_DATE";
            col08.Caption = "유통유효종료일자";
            col08.DefaultValue = "";
            newDt.Columns.Add(col08);

            DataColumn col09 = new DataColumn();
            col09.DataType = System.Type.GetType("System.String");
            col09.AllowDBNull = false;
            col09.ColumnName = "NOTICE_MANUFAC_NAME";
            col09.Caption = "제조수출회사명";
            col09.DefaultValue = "";
            newDt.Columns.Add(col09);

            DataColumn col10 = new DataColumn();
            col10.DataType = System.Type.GetType("System.String");
            col10.AllowDBNull = false;
            col10.ColumnName = "FAIL_REASON_DETAIL";
            col10.Caption = "부적합상세내역";
            col10.DefaultValue = "";
            newDt.Columns.Add(col10);
/*
            DataColumn col11 = new DataColumn();
            col11.DataType = System.Type.GetType("System.String");
            col11.AllowDBNull = false;
            col11.ColumnName = "PAST_EXPRT_ENTP_NO";
            col11.Caption = "수출업체번호(구)";
            col11.DefaultValue = "";
            newDt.Columns.Add(col11);

            DataColumn col12 = new DataColumn();
            col12.DataType = System.Type.GetType("System.String");
            col12.AllowDBNull = false;
            col12.ColumnName = "REG_DT";
            col12.Caption = "등록일시";
            col12.DefaultValue = "";
            newDt.Columns.Add(col12);

            DataColumn col13 = new DataColumn();
            col13.DataType = System.Type.GetType("System.String");
            col13.AllowDBNull = false;
            col13.ColumnName = "USE_YN";
            col13.Caption = "사용여부";
            col13.DefaultValue = "";
            newDt.Columns.Add(col13);*/


        }
        #endregion

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

        private void txtPageNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ImportProductPermitInfo_KeyDown(object sender, KeyEventArgs e)
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
                    case Keys.N:
                        txtProductEng.Text = string.Empty;
                        txtProductKor.Text = string.Empty;
                        txtProductKor.Focus();
                        break;
                    case Keys.M:
                        txtProductKor.Focus();
                        break;
                }

            }
        }
    }
}

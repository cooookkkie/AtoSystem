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

namespace AdoNetWindow
{
    public partial class ImportExport : Form
    {
        const string targetUrl = "http://apis.data.go.kr/1192000/select0070List/getselect0070List";
        const string serviceKey = "%2BSPXqSxPP9bwz%2Fgv5Dc7LCFMneAJ%2Fj%2FINtS%2ByapPN4QfMH7W81e%2Fli4cYtVZCItRwp4kFNM7wRRFPCWYabjJig%3D%3D";
        public ImportExport()
        {
            InitializeComponent();
            txtPageNo.Text = "1";
            this.KeyPreview = true;
            listView.Columns.Add("기준년월", 70);
            listView.Columns.Add("품목코드", 100);
            /*listView.Columns.Add("수출입구분코드", 100);*/
            listView.Columns.Add("수출입품목명", 500);
            listView.Columns.Add("수출입구분명", 100);
            /*listView.Columns.Add("대분류코드", 100);
            listView.Columns.Add("중분류코드", 100);
            listView.Columns.Add("소분류코드", 100);
            listView.Columns.Add("세분류코드", 100);
            listView.Columns.Add("수출입품목순번", 100);*/
            listView.Columns.Add("수출입중량", 100);
            listView.Columns.Add("미화금액", 100);


        }
        private void ImportExport_Load(object sender, EventArgs e)
        {
            this.txtBaseMonth.Maximum = 12;
            this.txtBaseMonth.Minimum = 1;
            this.txtBaseYear.Maximum = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            this.txtBaseYear.Minimum = 1950;
            this.txtBaseYear.Value = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
        }

        private void setListview()
        {
            string baseDt = txtBaseYear.Value.ToString("0000") + txtBaseMonth.Value.ToString("00");
            string division = cbDivision.Text;
            if (division == "전체")
            {
                division = "";
            }
            string product = txtProduct.Text;
            int pageNo = Convert.ToInt32(txtPageNo.Text);

            string result = getApi(baseDt, division, product, pageNo);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            XmlNodeList list = xml.GetElementsByTagName("item");


            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;
            listView.Items.Clear();

            int idx = 0;
            ListViewItem itm;
            foreach (XmlNode nd in list)
            {
                if (nd.InnerText.Length > 0)
                {
                    idx++;
                    string[] arr = new string[6];

                    arr[0] = nd["stdYymm"].InnerText;
                    arr[1] = nd["mprcExipitmCode"].InnerText;
                    /*arr[2] = nd["imxprtSeCode"].InnerText;*/
                    arr[2] = nd["mprcExipitmNm"].InnerText;
                    arr[3] = nd["imxprtSeNm"].InnerText;
                    /*arr[5] = nd["mprcExipitmLclasCode"].InnerText;
                    arr[6] = nd["mprcExipitmMlsfcCode"].InnerText;
                    arr[7] = nd["mprcExipitmSclasCode"].InnerText;
                    arr[8] = nd["mprcExipitmDtlclfcCode"].InnerText;
                    arr[9] = nd["mprcExipitmSn"].InnerText;*/
                    arr[4] = nd["imxprtWt"].InnerText;
                    arr[4] = Convert.ToDouble(arr[4]).ToString("#,##0.00");
                    arr[5] = nd["imxprtDollarAmount"].InnerText;
                    arr[5] = Convert.ToDouble(arr[5]).ToString("#,##0");

                    itm = new ListViewItem(arr);
                    listView.Items.Add(itm);
                }
            }



            XmlNodeList title = xml.GetElementsByTagName("header");
            
            foreach (XmlNode nd in title)
            {
                if (nd.InnerText.Length > 0)
                {
                    double quotient = Convert.ToInt32(nd["totalCount"].InnerText) / 100;
                    double remainder = Convert.ToInt32(nd["totalCount"].InnerText) % 100;
                    double pageCnt;

                    if (remainder > 0)
                    {
                        pageCnt = System.Math.Truncate(quotient) + 1;
                    }
                    else
                    {
                        pageCnt = System.Math.Truncate(quotient);
                    }
                    
                    lbTotalCount.Text = " / " + pageCnt.ToString();
                }
            }
        }


        private static string getApi(string baseDt, string division, string product, int pageNo)
        {
            if (pageNo < 1) pageNo = 1;

            string result = string.Empty;
            try
            {
                WebClient client = new WebClient();
                string url = string.Format(@"{0}?serviceKey={1}&numOfRows={2}&pageNo={3}&type={4}&baseDt={5}", targetUrl, serviceKey, 100, pageNo, "xml", baseDt);

                if (!string.IsNullOrEmpty(division))
                    url = url + string.Format(@"&imxprtSeNm={0}", division);
                if (!string.IsNullOrEmpty(product))
                    url = url + string.Format(@"&mprcExipitmNm={0}", product);


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

        private void txtbaseDt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtPageNo.Text = "1";
                setListview();
            }
        }

        private void cbDivision_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtPageNo.Text = "1";
                setListview();
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtPageNo.Text = "1";
                setListview();
            }
        }
        private void txtBaseYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtPageNo.Text = "1";
                setListview();
            }
        }

        private void txtBaseMonth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtPageNo.Text = "1";
                setListview();
            }
        }

        private void cbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPageNo.Text = "1";
            setListview();
        }

        private void txtPageNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                setListview();
            }
        }


        private void btnLeft_Click(object sender, EventArgs e)
        {
            int currentPange = Convert.ToInt32(txtPageNo.Text);
            if (currentPange <= 1)
            {
                currentPange = 1;
            }
            else
            {
                currentPange--;
            }
            txtPageNo.Text = currentPange.ToString();
            setListview();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            int currentPange = Convert.ToInt32(txtPageNo.Text);
            if (currentPange >= 1)
            {
                currentPange++;
            }
            else
            {
                currentPange = 1;
            }
            txtPageNo.Text = currentPange.ToString();
            setListview();
        }
    }
}

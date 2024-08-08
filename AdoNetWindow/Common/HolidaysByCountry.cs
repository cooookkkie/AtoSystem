using HtmlAgilityPack;
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
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace AdoNetWindow.Common
{
    public partial class HolidaysByCountry : Form
    {
        Dictionary<string, string> countryDic;
        public HolidaysByCountry()
        {
            InitializeComponent();
        }

        private void HolidaysByCountry_Load(object sender, EventArgs e)
        {
            for (int i = DateTime.Now.Year - 10; i <= DateTime.Now.Year + 10; i++)
            { 
                cbYear.Items.Add(i.ToString());
            }
            cbYear.Text = DateTime.Now.Year.ToString();
            cbMonth.Text = DateTime.Now.Month.ToString();
            GetCountryCode();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private string GetCountryCode(string country)
        {
            string code = countryDic[country];
            return code;
        }

        #region Button


        private void GetCountryCode()
        {
            string uri = "https://timesles.com/ko/holidays/months/" + DateTime.Now.ToString("yyyy-MM") + "/china-41/";  // 사이트 주소
            try
            {

                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(uri);
                //HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'h-table-line')");
                //HtmlNode bodyNode = htmlDoc.DocumentNode.SelectNodes("body");

                string dd = htmlDoc.DocumentNode.OuterHtml;
                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//select[@id='selectCountry']");
                
                if (nodes.Count > 0)
                {
                    HtmlNode firstNode = nodes.First();
                    HtmlNodeCollection node = firstNode.SelectNodes("option");
                    if (node.Count > 0)
                    {
                        countryDic = new Dictionary<string, string>();
                        for (int i = 1; i < node.Count; i++)
                        {
                            countryDic.Add(node[i].InnerText, node[i].Attributes["value"].Value);
                            cbCountry.Items.Add(node[i].InnerText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbCountry.Text))
            {
                dgvHoliday.Rows.Clear();
                int year;
                if (!int.TryParse(cbYear.Text, out year))
                    return;
                int month;
                if (!int.TryParse(cbMonth.Text, out month))
                    return;

                DateTime dt = new DateTime(year, month, 1);
                string sDt = dt.ToString("yyyy-MM");
                string code = GetCountryCode(cbCountry.Text);
                if (code == null || string.IsNullOrEmpty(code))
                {
                    MessageBox.Show(this, "등록된 코드가 없습니다.");
                    this.Activate();
                    return;
                }
                string uri = "https://timesles.com/ko/holidays/months/" + sDt+ "/" + code + "/";  // 사이트 주소
                try
                {

                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument htmlDoc = web.Load(uri);
                    //HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'h-table-line')");
                    //HtmlNode bodyNode = htmlDoc.DocumentNode.SelectNodes("body");

                    string dd = htmlDoc.DocumentNode.OuterHtml;
                    HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='h-table-line']");
                    HtmlNode firstNode = nodes.First();
                    if (nodes.Count > 1)
                    {
                        for (int i = 1; i < nodes.Count; i++)
                        {
                            int n = dgvHoliday.Rows.Add();
                            DataGridViewRow row = dgvHoliday.Rows[n];
                            HtmlNodeCollection hc = nodes[i].SelectNodes("div");
                            for (int j = 0; j < hc.Count; j++)
                            {
                                row.Cells[j].Value = hc[j].InnerText;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {

                }

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void HolidaysByCountry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
        #endregion
    }
}

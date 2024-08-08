using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace AdoNetWindow.Product
{
    public partial class CreateBoxDesignFolderManager : Form
    {
        Libs.ftpCommon ftp = new Libs.ftpCommon();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public CreateBoxDesignFolderManager(string product, string origin, string manager)
        {
            InitializeComponent();
            txtProduct.Text = product; 
            txtOrigin.Text = origin;
            cbManager.Text = manager;
        }

        #region Method
        private void MakeBoxDsignDocumentFolder(string folder_path, string root_path = "Solution/Document")
        {
            //기본 아토번호 폴더 생성     
            if (!ftp.CheckDirectory(folder_path, true, root_path))
            {
                MessageBox.Show(this, "서류폴더 생성중 에러가 발생하였습니다.");
                this.Activate();
            }

            

        }
        #endregion


        #region Button
        private void btnInsert_Click(object sender, EventArgs e)
        {


            string product = txtProduct.Text;
            string origin = txtOrigin.Text;
            string manager = cbManager.Text;

            if (string.IsNullOrEmpty(product))
                messageBox.Show(this, "품명이 없습니다.");
            if (string.IsNullOrEmpty(origin))
                messageBox.Show(this, "원산지이 없습니다.");
            if (string.IsNullOrEmpty(manager))
                messageBox.Show(this, "담당자가 없습니다.");

            product = product.Replace(@"\", "");
            product = product.Replace(@"/", "");
            product = product.Replace(@":", "");
            product = product.Replace(@"*", "");
            product = product.Replace(@"?", "");
            product = product.Replace(@"<", "");
            product = product.Replace(@">", "");
            product = product.Replace(@".", ",");

            origin = origin.Replace(@"\", "");
            origin = origin.Replace(@"/", "");
            origin = origin.Replace(@":", "");
            origin = origin.Replace(@"*", "");
            origin = origin.Replace(@"?", "");
            origin = origin.Replace(@"<", "");
            origin = origin.Replace(@">", "");
            origin = origin.Replace(@".", ",");

            string folder_path = manager + "/" + product + "/" + origin;
            MakeBoxDsignDocumentFolder(folder_path, @"ATO/아토무역/무역/무역1/ㄴ.수입자료/박스디자인");
            string errMsg;
            ftp.StartTradeBoxDegisnFolder(manager, product, origin, out errMsg);
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        private void CreateBoxDesignFolderManager_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Alt) 
            {
                switch(e.KeyCode)
                {
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
    }
}

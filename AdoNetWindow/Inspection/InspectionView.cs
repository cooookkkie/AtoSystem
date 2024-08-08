using AdoNetWindow.Model;
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

namespace AdoNetWindow.Inspection
{
    public partial class InspectionView : Form
    {
        UsersModel um;
        string[] files_path;
        string selelct_file;
        public InspectionView(UsersModel uModel, string[] files, string selelct_file)
        {
            InitializeComponent();
            um = uModel;
            files_path = files;
            this.selelct_file = selelct_file;
        }

        private void InspectionView_Load(object sender, EventArgs e)
        {
            if (files_path.Length > 0)
            {
                int cb_idx = 0;
                for (int i = 0; i < files_path.Length; i++)
                {
                    cbFiles.Items.Add(Path.GetFileName(files_path[i]));
                    if (selelct_file == files_path[i])
                        cb_idx = i;
                }
                cbFiles.SelectedIndex = cb_idx;
            }
            
        }
        Libs.ftpCommon ftp = new Libs.ftpCommon();
        private void cbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _url = files_path[cbFiles.SelectedIndex];
            _url = _url.Replace(@"\", "/");
            this.pbInspection.Image = ftp.GetUrlImage(_url);
            this.pbInspection.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        

        private void btnPre_Click(object sender, EventArgs e)
        {
            int idx = cbFiles.SelectedIndex;
            if (idx == 0)
                cbFiles.SelectedIndex = cbFiles.Items.Count - 1;
            else
                cbFiles.SelectedIndex = idx - 1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int idx = cbFiles.SelectedIndex;
            if (idx == cbFiles.Items.Count - 1)
                cbFiles.SelectedIndex = 0;
            else
                cbFiles.SelectedIndex = idx + 1;
        }
    }
}

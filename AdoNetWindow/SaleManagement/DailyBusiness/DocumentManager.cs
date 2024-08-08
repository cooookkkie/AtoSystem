using AdoNetWindow.Model;
using Repositories.SalesPartner;
using ScottPlot.Drawing.Colormaps;
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

namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    public partial class DocumentManager : Form
    {
        IDailyBusinessRepository dailyBusinessRepository = new DailyBusinessRepository();
        UsersModel um;
        DailyBusiness db = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public DocumentManager(UsersModel um, DailyBusiness db)
        {
            InitializeComponent();
            this.um = um;   
            this.db = db;
        }
        private void DocumentManager_Load(object sender, EventArgs e)
        {
            btnSeaching.PerformClick();
        }

        #region Button
        private void btnServerBackup_Click(object sender, EventArgs e)
        {
            db.ServerBackup();
            btnSeaching.PerformClick();
            messageBox.Show(this, "생성완료");
            this.Activate();
        }
        private void btnNesDocument_Click(object sender, EventArgs e)
        {
            db.NewDocument();
            db.Activate();
        }
        Libs.Tools.LoginCookie cookie = new Libs.Tools.LoginCookie();
        private void btnSeaching_Click(object sender, EventArgs e)
        {
            if (tcMain.SelectedTab.Name == "tabPc")
            {
                string _path = @"C:\Cookies\TEMP\DAILYBUSINESS";
                List<string> jsonList = cookie.GetFolderList(_path);
                List<string> txtList = cookie.GetTempList(_path);

                DataTable backupDt = new DataTable();
                backupDt.Columns.Add("document_id", typeof(int));
                backupDt.Columns.Add("update_date", typeof(string));
                backupDt.Columns.Add("update_time", typeof(string));
                backupDt.Columns.Add("save_path", typeof(string));


                foreach (string jsonFolder in jsonList)
                {
                    string temp = jsonFolder;
                    string path = Path.GetDirectoryName(temp);
                    string folder = Path.GetFileName(path);
                    string file_name = Path.GetFileName(temp);


                    DataRow dr = backupDt.NewRow();
                    dr["document_id"] = 1;
                    dr["update_date"] = folder.Substring(0, 4) + "-" + folder.Substring(4, 2) + "-" + folder.Substring(6, 2);
                    dr["update_time"] = file_name.Replace(".txt", "").Substring(0, 2) + ":" + file_name.Replace(".txt", "").Substring(2, 2);
                    dr["save_path"] = temp;
                    backupDt.Rows.Add(dr);
                }

                foreach (string txtFolder in txtList)
                {
                    string temp = txtFolder;
                    string path = Path.GetDirectoryName(temp);
                    string folder = Path.GetFileName(path);
                    string file_name = Path.GetFileName(temp);

                    if (file_name != "DAILYBUSINESS.txt")
                    {
                        DataRow dr = backupDt.NewRow();
                        dr["document_id"] = 2;
                        dr["update_date"] = folder.Substring(0, 4) + "-" + folder.Substring(4, 2) + "-" + folder.Substring(6, 2);
                        dr["update_time"] = file_name.Replace(".txt", "").Substring(0, 2) + ":" + file_name.Replace(".txt", "").Substring(2, 2);
                        dr["save_path"] = temp;
                        backupDt.Rows.Add(dr);
                    }
                }

                DataView dv = new DataView(backupDt);
                dv.Sort = "update_date DESC, update_time DESC";
                backupDt = dv.ToTable();
                backupDt.AcceptChanges();

                dgvPcDocument.Rows.Clear();

                //최근에 저장한 문서
                int n = dgvPcDocument.Rows.Add();
                dgvPcDocument.Rows[n].Cells["document_id"].Value = 0;
                dgvPcDocument.Rows[n].Cells["update_date"].Value = "최근에 저장된 문서";
                //백업파일
                for (int i = 0; i < backupDt.Rows.Count; i++)
                {
                    n = dgvPcDocument.Rows.Add();
                    dgvPcDocument.Rows[n].Cells["document_id"].Value = backupDt.Rows[i]["document_id"].ToString();
                    dgvPcDocument.Rows[n].Cells["update_date"].Value = backupDt.Rows[i]["update_date"].ToString();
                    dgvPcDocument.Rows[n].Cells["update_time"].Value = backupDt.Rows[i]["update_time"].ToString();
                    dgvPcDocument.Rows[n].Cells["save_path"].Value = backupDt.Rows[i]["save_path"].ToString();
                }
            }
            else if (tcMain.SelectedTab.Name == "tabServer")
            {
                //TempList
                dgvServerDocument.Rows.Clear();
                DataTable tempDt = dailyBusinessRepository.GetTempList(um.user_id, 1);
                for (int i = 0; i < tempDt.Rows.Count; i++)
                {
                    DateTime dt;
                    if (!DateTime.TryParse(tempDt.Rows[i]["updatetime"].ToString(), out dt))
                        dt = DateTime.Now;

                    int n = dgvServerDocument.Rows.Add();
                    dgvServerDocument.Rows[n].Cells["id"].Value = tempDt.Rows[i]["id"].ToString();
                    dgvServerDocument.Rows[n].Cells["update_date2"].Value = dt.ToString("yyyy-MM-dd");
                    dgvServerDocument.Rows[n].Cells["update_time2"].Value = dt.ToString("HH:mm");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnCurrentDocument_Click(object sender, EventArgs e)
        {
            if (tcMain.SelectedTab.Name == "tabPc")
            {
                if (dgvPcDocument.Rows.Count > 0)
                {
                    db.GetTxtData();
                    db.Activate();
                }
            }
            else
            {
                if (dgvServerDocument.Rows.Count > 0)
                {
                    int id;
                    if (int.TryParse(dgvServerDocument.Rows[0].Cells["id"].Value.ToString(), out id))
                    {
                        db.GetServerBackupData(id, 1);
                        db.Activate();
                    }
                    else
                    {
                        messageBox.Show(this,"저장내역을 찾을 수 없습니다.");
                        this.Activate();
                    }
                }

            }
        }
        #endregion

        #region Key event
        private void DocumentManager_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSeaching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }

            }
        }
        #endregion

        #region TabMenu event
        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSeaching.PerformClick();
        }
        #endregion


        #region Datagridview event
        private void dgvPcDocumnet_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 1)
            {
                if (db != null)
                {
                    if (dgvPcDocument.Rows[e.RowIndex].Cells["document_id"].Value.ToString() == "1")
                    {
                        string folder_name = dgvPcDocument.Rows[e.RowIndex].Cells["update_date"].Value.ToString().Replace("-", "")
                                    + @"\" + dgvPcDocument.Rows[e.RowIndex].Cells["update_time"].Value.ToString().Replace(":", "");

                        db.GetJsonData(folder_name);
                    }
                    else
                    {
                        string file_name = dgvPcDocument.Rows[e.RowIndex].Cells["update_date"].Value.ToString().Replace("-", "")
                                    + "|" + dgvPcDocument.Rows[e.RowIndex].Cells["update_time"].Value.ToString().Replace(":", "");

                        db.GetTxtData(file_name + ".txt");
                        db.Activate();
                    }
                }
            }
            else
            {
                db.GetTxtData();
                db.Activate();
            }

        }
        private void dgvServerDocument_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (db != null)
                {
                    int id;
                    if (int.TryParse(dgvServerDocument.Rows[e.RowIndex].Cells["id"].Value.ToString(), out id))
                    {
                        db.GetServerBackupData(id, 1);
                        db.Activate();
                    }
                    else
                    {
                        messageBox.Show(this,"저장내역을 찾을 수 없습니다.");
                        this.Activate();
                    }
                }
            }
        }


        #endregion

        
    }
}

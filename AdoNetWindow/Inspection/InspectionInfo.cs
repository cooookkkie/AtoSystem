using AdoNetWindow.Model;
using Repositories;
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

namespace AdoNetWindow.Inspection
{
    public partial class InspectionInfo : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        CalendarModule.calendar cd;
        UsersModel um;
        InspectionModel im;
        IinspectionRepository inspectionRepository = new InspectionRepository();
        ICommonRepository commonRepository = new CommonRepository();
        Libs.ftpCommon ftp = new Libs.ftpCommon();

        int id, sub_id;

        public InspectionInfo(CalendarModule.calendar cal, UsersModel umodel, int id, int sub_id)
        {
            InitializeComponent();
            cd = cal;
            um = umodel;
            this.dgvAttachment.AllowDrop = true;
            this.MaximizeBox = false;
            this.id = id;
            this.sub_id = sub_id;
        }

        private void InspectionInfo_Load(object sender, EventArgs e)
        {
            GetData();
        }

        #region Mthod
        private DateTime SetInspection(DateTime warehousing_date)
        {
            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt;
            DateTime tempDt1 = new DateTime(warehousing_date.Year, warehousing_date.Month, 1);
            DateTime tempDt2 = new DateTime(warehousing_date.Year, warehousing_date.Month, 1).AddDays(-1);
            common.getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            common.getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
            //주말일경우 날짜 수정
            
            tempDt = warehousing_date;
        retry:
            //주말일 경우 수정
            if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                tempDt = tempDt.AddDays(2);
            else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                tempDt = tempDt.AddDays(1);

            //법정공휴일일 경우
            if (tempDt.Month == tempDt1.Month)
            {
                foreach (int n in no1)
                {
                    if (n == tempDt.Day)
                    {
                        tempDt = tempDt.AddDays(1);
                        goto retry;
                    }
                }
            }
            else
            {
                foreach (int n in no2)
                {
                    if (n == tempDt.Day)
                    {
                        tempDt = tempDt.AddDays(1);
                        goto retry;
                    }
                }
            }

            warehousing_date = tempDt;
            return warehousing_date;
        }
        private bool SetNetDrive()
        {
            string folder_path = lbContractYear.Text + "/" + ftp.ReplaceName(lbAtono.Text) + "/" + ftp.ReplaceName(lbProdcut.Text) + "/" + ftp.ReplaceName(lbOrigin.Text) + "/" + ftp.ReplaceName(lbSizes.Text);
            try
            {
                if (ftp.CheckDirectory(folder_path, true))
                {
                    DirectoryInfo di = new DirectoryInfo("O:");
                    //DirectoryInfo.Exists로 폴더 존재유무 확인
                    if (!di.Exists)
                    {
                        string userid, pwd;
                        FTPmanager.FtpLoginManager ftpLogin = new FTPmanager.FtpLoginManager(um, out userid, out pwd);
                        if (!(userid == null || pwd == null))
                        {
                            string errMsg;
                            if (!ftp.StartDirectory(folder_path, out errMsg, userid, pwd))
                            {
                                MessageBox.Show(this,errMsg);
                                this.Activate();
                                return false;
                            }
                            else
                            {
                                System.Diagnostics.Process.Start("explorer.exe", @"O:\" + folder_path.Replace("/", @"\"));
                                GetAttachment();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        
                        //System.Diagnostics.Process.Start("explorer.exe", @"O:\" + folder_path.Replace("/", @"\"));
                        GetAttachment();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
                this.Activate();
                return true;
            }
            return true;
        }
        private void AttachmentCounting()
        {
            if (dgvAttachment.RowCount > 0)
            {
                int cnt = 1;
                for (int i = 0; i < dgvAttachment.Rows.Count; i++)
                {
                    dgvAttachment.Rows[i].Cells["cnt"].Value = cnt;
                    cnt++;
                }
            }
        }
        private void GetAttachment()
        { 
            dgvAttachment.Rows.Clear();
            string product_folder_path = lbContractYear.Text + "/" + ftp.ReplaceName(lbAtono.Text) + "/" + ftp.ReplaceName(lbProdcut.Text) + "/" + ftp.ReplaceName(lbOrigin.Text) + "/" + ftp.ReplaceName(lbSizes.Text);
            string[] files = ftp.GetFileList(product_folder_path);
            int cnt = 1;
            for (int i = 0; i < files.Length; i++)
            {
                int n = dgvAttachment.Rows.Add();
                DataGridViewRow row = dgvAttachment.Rows[n];

                row.Cells["cnt"].Value = cnt;
                row.Cells["file_name"].Value = Path.GetFileName(files[i]);
                row.Cells["file_path"].Value = product_folder_path + "/" + Path.GetFileName(files[i]);
                cnt++;
            }
        }

        private void GetData()
        {
            DataTable inspectionDt = inspectionRepository.GetInspectinoList("","", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", id, sub_id);
            if (inspectionDt.Rows.Count > 0)
            {
                lbContractYear.Text = inspectionDt.Rows[0]["contract_year"].ToString();
                lbAtono.Text = inspectionDt.Rows[0]["ato_no"].ToString();
                lbBlno.Text = inspectionDt.Rows[0]["bl_no"].ToString();
                lbContractno.Text = inspectionDt.Rows[0]["contract_no"].ToString();
                lbEtd.Text = inspectionDt.Rows[0]["etd"].ToString();
                lbEta.Text = inspectionDt.Rows[0]["eta"].ToString();

                DateTime warehousing_date;
                if (DateTime.TryParse(inspectionDt.Rows[0]["warehousing_date"].ToString(), out warehousing_date))
                {
                    warehousing_date = SetInspection(warehousing_date);
                    txtWarehousingDate.Text = warehousing_date.ToString("yyyy-MM-dd");
                }
                lbProdcut.Text = inspectionDt.Rows[0]["product"].ToString();
                lbOrigin.Text = inspectionDt.Rows[0]["origin"].ToString();
                lbSizes.Text = inspectionDt.Rows[0]["sizes"].ToString();
                lbBoxWeight.Text = inspectionDt.Rows[0]["box_weight"].ToString();
                txtWarehouse.Text = inspectionDt.Rows[0]["warehouse"].ToString();

                double qty;
                if (double.TryParse(inspectionDt.Rows[0]["qty"].ToString(), out qty))
                    lbQty.Text = qty.ToString("#,##0");

                DateTime dt;
                if (DateTime.TryParse(inspectionDt.Rows[0]["inspection_date"].ToString(), out dt))
                {
                    txtInspectionDate.Text = dt.ToString("yyyy-MM-dd");
                    txtInspectionResults.Text = inspectionDt.Rows[0]["inspection_remark"].ToString();
                    cbInspectionStatus.Text = inspectionDt.Rows[0]["inspection_status"].ToString();
                    txtInspectionManager.Text = inspectionDt.Rows[0]["inspection_manager"].ToString();
                }
                else
                {
                    txtInspectionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtInspectionManager.Text = um.user_name;
                    cbInspectionStatus.Text = "대기";
                }
                //서류폴더
                SetNetDrive();
            }
        }
        #endregion

        #region Drag & Drop
        private void dgvAttachment_DragDrop(object sender, DragEventArgs e)
        {
            int cnt = 1;
            if (dgvAttachment.Rows.Count > 0)
                cnt = Convert.ToInt32(dgvAttachment.Rows[dgvAttachment.Rows.Count - 1].Cells["cnt"].Value.ToString()) + 1;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string product_folder_path = lbContractYear.Text + "/" + ftp.ReplaceName(lbAtono.Text) + "/" + ftp.ReplaceName(lbProdcut.Text) + "/" + ftp.ReplaceName(lbOrigin.Text) + "/" + ftp.ReplaceName(lbSizes.Text);
            foreach (string file in files)
            {
                int n = dgvAttachment.Rows.Add();
                DataGridViewRow row = dgvAttachment.Rows[n];

                row.Cells["cnt"].Value = cnt;
                row.Cells["file_name"].Value = Path.GetFileName(file);
                row.Cells["file_path"].Value = file;
                cnt++;

                //파일업로드
                if (!ftp.UploadFiles(product_folder_path, file))
                {
                    row.Cells["is_registration"].Value = "실패";
                }
                else
                {
                    row.Cells["file_path"].Value = product_folder_path + "/" + Path.GetFileName(file);
                    row.Cells["is_registration"].Value = "성공";
                }
            }
        }
        private void dgvAttachment_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        #endregion

        #region Button
        private void btnWarehousingDate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
                txtWarehousingDate.Text = sdate;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            InspectionInfoModel model = new InspectionInfoModel();

            model.id = id;
            model.sub_id = sub_id;
            model.inspection_cnt = 1;

            DateTime inspection_date;
            if (!DateTime.TryParse(txtInspectionDate.Text, out inspection_date))
            {
                MessageBox.Show(this, "검품일자를 확인해주세요.");
                this.Activate();
                return;
            }
            model.inspection_date = inspection_date.ToString("yyyy-MM-dd");
            model.status = cbInspectionStatus.Text;
            model.remark = txtInspectionResults.Text;
            model.manager = txtInspectionManager.Text;
            model.edit_user = um.user_name;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            List<StringBuilder> sqlList = new List<StringBuilder>();
            //전데이터 삭제
            StringBuilder sql = inspectionRepository.DeleteInspection(model);
            sqlList.Add(sql);
            //새데이터 등록
            sql = inspectionRepository.InsertInspection(model);
            sqlList.Add(sql);
            //팬딩내역 수정
            sql = commonRepository.UpdateData("t_customs", $"warehousing_date = '{txtWarehousingDate.Text}', warehouse = '{txtWarehouse.Text}'", $"id = {id} AND sub_id = {sub_id}");
            sqlList.Add(sql);

            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
        }

        private void btnFolderStart_Click(object sender, EventArgs e)
        {
            string folder_path = lbContractYear.Text + "/" + ftp.ReplaceName(lbAtono.Text) + "/" + ftp.ReplaceName(lbProdcut.Text) + "/" + ftp.ReplaceName(lbOrigin.Text) + "/" + ftp.ReplaceName(lbSizes.Text);
            try
            {
                if (ftp.CheckDirectory(folder_path, true))
                {
                    DirectoryInfo di = new DirectoryInfo("O:");
                    //DirectoryInfo.Exists로 폴더 존재유무 확인
                    if (!di.Exists)
                    {
                        string userid, pwd;
                        FTPmanager.FtpLoginManager ftpLogin = new FTPmanager.FtpLoginManager(um, out userid, out pwd);
                        if (!(userid == null || pwd == null))
                        {
                            string errMsg;
                            if (!ftp.StartDirectory(folder_path, out errMsg, userid, pwd))
                            {
                                MessageBox.Show(this, errMsg);
                                this.Activate();
                            }
                            else
                            {
                                System.Diagnostics.Process.Start("explorer.exe", @"O:\" + folder_path.Replace("/", @"\"));
                            }
                        }
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(@"O:\" + folder_path.Replace("/", @"\"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
                this.Activate();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnInspectionDate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
                txtInspectionDate.Text = sdate;
        }
        #endregion

        #region Datagridview event
        private void dgvAttachment_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string _url = @"O:\" + dgvAttachment.Rows[e.RowIndex].Cells["file_path"].Value.ToString();
                _url = _url.Replace("/", @"\");
                this.pbInspection.Image = ftp.GetUrlImage(_url);
                this.pbInspection.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void dgvAttachment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvAttachment.Columns[e.ColumnIndex].Name == "is_delete")
                {
                    string product_folder_path = lbContractYear.Text
                                            + "/" + ftp.ReplaceName(lbAtono.Text)
                                            + "/" + ftp.ReplaceName(lbProdcut.Text)
                                            + "/" + ftp.ReplaceName(lbOrigin.Text)
                                            + "/" + ftp.ReplaceName(lbSizes.Text)
                                            + "/" + dgvAttachment.Rows[e.RowIndex].Cells["file_name"].Value.ToString();

                    if (MessageBox.Show(this, "선택하신 이미지 파일을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (!ftp.DeleteFTPFile(product_folder_path))
                        {
                            MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                        else
                        {
                            AttachmentCounting();
                            dgvAttachment.Rows.Remove(dgvAttachment.Rows[e.RowIndex]);
                        }
                    }
                }
            }
        }
        #endregion
    }
}

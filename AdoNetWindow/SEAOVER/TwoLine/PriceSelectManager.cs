using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.TwoLine
{
    public partial class PriceSelectManager : Form
    {
        int price = 0;
        private System.Windows.Threading.DispatcherTimer timer;
        private bool isVisible;
        //Dictionary<int, string> dic = new Dictionary<int, string>();
        string[] select = new string[2];
        string model;
        public PriceSelectManager()
        {
            InitializeComponent();
        }

        public string[] Manager(DataTable dt, Point p)
        {
            this.Width = 525;
            model = "_2LineForm";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["매출단가"] = Convert.ToInt32(dt.Rows[i]["매출단가"]);
                }
            }
            dgv.DataSource = dt;

            this.dgv.AllowUserToAddRows = false;
            this.dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv.Columns[0].Resizable = DataGridViewTriState.False;

            this.dgv.Columns["원산지코드"].Visible = false;
            this.dgv.Columns["품목코드"].Visible = false;
            this.dgv.Columns["규격코드"].Visible = false;
            /*this.dgv.Columns["규격2"].Visible = false;
            this.dgv.Columns["규격3"].Visible = false;
            this.dgv.Columns["규격4"].Visible = false;*/

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (this.dgv.Columns[i].Name == "대분류")
                {
                    dgv.Columns[i].HeaderCell.Style.BackColor = Color.Red;
                    dgv.Columns[i].HeaderCell.Style.ForeColor = Color.Yellow;
                    //dgv.Columns["category"].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
                    dgv.Columns[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 153);
                    dgv.Columns[i].Width = 100;
                    this.dgv.Columns[i].Visible = false;
                }
                else if (this.dgv.Columns[i].Name == "품명")
                {
                    dgv.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
                    dgv.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                    dgv.Columns[i].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                    dgv.Columns[i].Width = 100;
                    this.dgv.Columns[i].Visible = false;
                }
                else if (this.dgv.Columns[i].Name == "원산지")
                {
                    dgv.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
                    dgv.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                    dgv.Columns[i].Width = 100;
                    this.dgv.Columns[i].Visible = false;
                }
                else if (this.dgv.Columns[i].Name == "규격")
                {
                    dgv.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
                    dgv.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                    dgv.Columns[i].Width = 100;
                    this.dgv.Columns[i].Visible = false;
                }
            }

            dgv.Columns["btnSelect"].HeaderCell.Style.BackColor = Color.Red;            

            dgv.Columns["단위"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
            dgv.Columns["단위"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["단위"].Width = 40;

            dgv.Columns["가격단위"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
            dgv.Columns["가격단위"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["가격단위"].Width = 40;

            dgv.Columns["단위수량"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
            dgv.Columns["단위수량"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["단위수량"].Width = 40;

            dgv.Columns["SEAOVER단위"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
            dgv.Columns["SEAOVER단위"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["SEAOVER단위"].Width = 40;

            dgv.Columns["매출단가"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["매출단가"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["매출단가"].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgv.Columns["매출단가"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgv.Columns["매출단가"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["매출단가"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["매출단가"].Width = 80;

            dgv.Columns["단가수정일"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["단가수정일"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["단가수정일"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["단가수정일"].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgv.Columns["단가수정일"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);

            dgv.Columns["담당자1"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
            dgv.Columns["담당자1"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["담당자1"].Width = 60;

            dgv.Columns["구분"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); 
            dgv.Columns["구분"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["구분"].Width = 40;


            this.StartPosition = FormStartPosition.Manual;
            this.Location = p;
            this.ShowDialog();

            return select;
        }

        public string[] Manager2(DataTable dt, Point p)
        {
            model = "SimpleHandlingFormManager";
            this.Width = 700;
            this.Height = 500;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["매출단가"] = Convert.ToInt32(dt.Rows[i]["매출단가"]);
                }
            }
            dgv.DataSource = dt;

            this.dgv.AllowUserToAddRows = false;
            this.dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv.Columns[0].Resizable = DataGridViewTriState.False;

            dgv.Columns["품명"].Visible = false;
            dgv.Columns["원산지"].Visible = false;
            dgv.Columns["규격"].Visible = false;
            dgv.Columns["단위"].Visible = false;

            dgv.Columns["btnSelect"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["재고수"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["재고수"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["재고수"].Width = 50;

            dgv.Columns["예약수"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["예약수"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["예약수"].Width = 50;

            dgv.Columns["실재고"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["실재고"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["실재고"].Width = 50;

            dgv.Columns["매출단가"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["매출단가"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["매출단가"].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgv.Columns["매출단가"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgv.Columns["매출단가"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["매출단가"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["매출단가"].Width = 80;

            dgv.Columns["창고"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["창고"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["창고"].Width = 200;

            dgv.Columns["적요"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["적요"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["적요"].Width = 200;
              
            lbDirection.Location = new Point(670, -2);
            lbDirection.Text = "👆";

            this.StartPosition = FormStartPosition.Manual;
            this.Location = p;
            this.ShowDialog();          

            return select;
        }

        private void PriceSelectManager_Load(object sender, EventArgs e)
        {
            //헤더 디자인
            this.dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            timer_start();
        }

        private void dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentCell is DataGridViewCheckBoxCell) 
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit); 
            }

        }

        private void dgv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            StringFormat drawFormat = new StringFormat(); 
            //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            using (Brush brush = new SolidBrush(Color.Red))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, brush, e.RowBounds.Location.X + 35, e.RowBounds.Location.Y + 4, drawFormat);
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows.Count > 1 && e.ColumnIndex == 0)
            {
                if (model == "_2LineForm")
                {
                    select = new string[3];
                    select[0] = dgv.Rows[e.RowIndex].Cells["매출단가"].Value.ToString();
                    select[1] = dgv.Rows[e.RowIndex].Cells["단가수정일"].Value.ToString();
                    select[2] = dgv.Rows[e.RowIndex].Cells["담당자1"].Value.ToString();
                }
                else if (model == "SimpleHandlingFormManager")
                {
                    select = new string[6];
                    select[0] = dgv.Rows[e.RowIndex].Cells["품명"].Value.ToString();
                    select[1] = dgv.Rows[e.RowIndex].Cells["원산지"].Value.ToString();
                    select[2] = dgv.Rows[e.RowIndex].Cells["규격"].Value.ToString();
                    select[3] = dgv.Rows[e.RowIndex].Cells["단위"].Value.ToString();
                    select[4] = dgv.Rows[e.RowIndex].Cells["매출단가"].Value.ToString();
                    select[5] = dgv.Rows[e.RowIndex].Cells["창고"].Value.ToString();
                }
                    

                //price = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["매출단가"].Value);
                this.Dispose();
            }
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 12)
            {
                double intNum;


                if (double.TryParse(e.Value.ToString(), out intNum))
                    e.Value = string.Format("{0:#,###}", intNum);
                else
                    e.Value = e.Value;


            }
        }

        #region 로딩 효과
        private void timer_start()
        {
            this.timer = new System.Windows.Threading.DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            this.timer.Tick += timer_Tick;
            this.timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (isVisible)
            {
                lbDirection.Visible = true;
            }
            else
            {
                lbDirection.Visible = false;
            }
            isVisible = !isVisible;
        }
        #endregion

        private void PriceSelectManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgv.Rows.Count > 0 && dgv.SelectedRows.Count > 0)
                {
                    if (model == "_2LineForm")
                    {
                        select[0] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["매출단가"].Value.ToString();
                        select[1] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["단가수정일"].Value.ToString();
                        select[2] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["담당자1"].Value.ToString();
                    }
                    else if (model == "SimpleHandlingFormManager")
                    {
                        select = new string[6];
                        select[0] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["품명"].Value.ToString();
                        select[1] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["원산지"].Value.ToString();
                        select[2] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["규격"].Value.ToString();
                        select[3] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["단위"].Value.ToString();
                        select[4] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["매출단가"].Value.ToString();
                        select[5] = dgv.Rows[dgv.SelectedRows[0].Index].Cells["창고"].Value.ToString();
                    }

                    //price = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["매출단가"].Value);
                    this.Dispose();
                }
            }
        }
    }
}


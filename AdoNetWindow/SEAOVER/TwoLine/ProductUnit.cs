using AdoNetWindow.Model;
using AdoNetWindow.SEAOVER._2Line;
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
    public partial class ProductUnit : UserControl
    {
        public enum UnitType
        {
            Category,
            Product,
            Origin,
            Weight,
            sizes,
            price,


            sizes1,
            sizes2,
            costUnit,
            trayPrice,
            boxPrice,
            isTax,
            remark

        }
        UnitType ut;
        SeaoverPriceModel model;
        int height;
        _2LineForm form = null;
        SEAOVER.OneLine.OneLineForm oneform = null;
        int type;
        bool accent;
        int accent2;
        public ProductUnit(SEAOVER.OneLine.OneLineForm onelineform, UnitType unitType, SeaoverPriceModel seaoverPriceModel, int sHeight, int form_type, int accent2 = 0)
        {
            InitializeComponent();
            ut = unitType;
            model = seaoverPriceModel;
            height = sHeight;
            oneform = onelineform;
            type = form_type;
            this.accent2 = accent2;
        }
        public ProductUnit(_2LineForm _2form, UnitType unitType, SeaoverPriceModel seaoverPriceModel, int sHeight, bool isAccent, int accent2 = 0)
        {
            InitializeComponent();
            ut = unitType;
            model = seaoverPriceModel;
            height = sHeight;
            form = _2form;
            type = 2;
            accent = isAccent;
            this.accent2 = accent2;
        }

        public ProductUnit(_2LineForm _2form, UnitType unitType, SeaoverPriceModel seaoverPriceModel, int sHeight, int form_type, int accent2 = 0)
        {
            InitializeComponent();
            ut = unitType;
            model = seaoverPriceModel;
            height = sHeight;
            form = _2form;
            type = form_type;
            this.accent2 = accent2;
        }

        private void ProductUnit_Load(object sender, EventArgs e)
        {
            this.Height = height;
            lbContents.Width = this.Width;
            lbContents.Height = this.Height;
            //txtbox
            txtUpdate.Location = new Point(0, 0);
            txtUpdate.Width = this.Width;
            txtUpdate.Height = this.Height - 1;

            if (ut == UnitType.Category)
            {
                this.BorderStyle = System.Windows.Forms.BorderStyle.None;
                if (type == 2)
                {
                    this.Width = 31;
                    this.lbContents.Width = 31;
                }
                else if (form != null)
                {
                    this.Width = 62;
                    this.lbContents.Width = 62;
                }
                else
                {
                    this.Width = 30;
                    this.lbContents.Width = 30;
                }
                this.lbContents.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                this.lbContents.BackColor = Color.Beige;
                this.lbContents.Text = "";
                string str = model.category.ToString();

                string val = model.category.ToString();
                val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                int valLen = val.Length;
                string category;
                //5행이하, 6글자 이상
                if (valLen > 3 & height <= 14 * 5)
                {
                    int stt_idx = 0;
                    string tempStr = "";
                    for (int k = 0; k < valLen; k++)
                    {
                        if ((k + 1) % 2 == 0)
                        {
                            if ((stt_idx + 2) == valLen)
                            {
                                tempStr += val.Substring(stt_idx, 2);
                            }
                            else
                            {
                                tempStr += val.Substring(stt_idx, 2) + "\r\n";
                            }
                            stt_idx += 2;
                        }
                    }
                    category = tempStr;
                }
                //5행이하, 6글자 이상
                else if (valLen > 1 & height <= 14 * 2)
                {
                    int stt_idx = 0;
                    string tempStr = "";
                    for (int k = 0; k < valLen; k++)
                    {
                        if ((k + 1) % 2 == 0)
                        {
                            if ((stt_idx + 2) == valLen)
                            {
                                tempStr += val.Substring(stt_idx, 2);
                            }
                            else
                            {
                                tempStr += val.Substring(stt_idx, 2) + "\r\n";
                            }
                            stt_idx += 2;
                        }
                    }
                    category = tempStr;
                }
                //6행이상, 6글자 이상
                else
                {
                    int stt_idx = 0;
                    string tempStr = "";
                    for (int k = 0; k < valLen; k++)
                    {
                        if ((stt_idx + 1) == valLen)
                        {
                            tempStr += val.Substring(stt_idx, 1);
                        }
                        else
                        {
                            tempStr += val.Substring(stt_idx, 1) + "\r\n";
                        }

                        stt_idx += 1;
                    }
                    category = tempStr;
                }
                /*int indexStart = 0;
                int indexEnd = 0;
                int iSplit = 1;
                int totalLength = str.Length;
                int forCount = totalLength / iSplit;

                for (int i = 0; i < forCount + 1; i++)
                {
                    if (totalLength < indexStart + iSplit)
                    {
                        indexEnd = totalLength - indexStart;
                    }
                    else
                    {
                        indexEnd = str.Substring(indexStart, iSplit).LastIndexOf("") + 1;
                    }

                    this.lbContents.Text += str.Substring(indexStart, indexEnd) + "\r\n";

                    indexStart += indexEnd;
                }
                if (this.lbContents.Text.Length > 4)
                { 
                    this.lbContents.Text = this.lbContents.Text.Substring(0, this.lbContents.Text.Length - 4);
                }*/
                this.lbContents.Text = category;
                this.lbContents.Font = new Font(this.lbContents.Font.Name, this.lbContents.Font.Size, FontStyle.Bold);
            }
            else if (ut == UnitType.Product)
            {
                if (type == 2)
                {
                    this.Width = 82;
                    this.lbContents.Width = 82;
                }
                else if (form != null)
                {
                    this.Width = 164;
                    this.lbContents.Width = 164;
                }
                else
                {
                    this.Width = 135;
                    this.lbContents.Width = 135;
                }
                this.lbContents.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                this.lbContents.Text = model.product.ToString();
                if (accent)
                {
                    this.lbContents.Text = "★" + this.lbContents.Text;
                }
            }
            else if (ut == UnitType.Origin)
            {
                if (type == 2)
                {
                    this.Width = 36;
                    this.lbContents.Width = 36;
                }
                else if (form != null)
                {
                    this.Width = 72;
                    this.lbContents.Width = 72;
                }
                else
                {
                    this.Width = 53;
                    this.lbContents.Width = 53;
                }
                this.lbContents.Font = new Font("나눔고딕", 7);

                if (model.origin.ToString().Length >= 4)
                {
                    this.lbContents.Text = "";
                    string str = model.origin.ToString();

                    int indexStart = 0;
                    int indexEnd = 0;
                    int iSplit = 2;
                    int totalLength = str.Length;
                    int forCount = totalLength / iSplit;

                    for (int i = 0; i < forCount + 1; i++)
                    {
                        if (totalLength < indexStart + iSplit)
                        {
                            indexEnd = totalLength - indexStart;
                        }
                        else
                        {
                            indexEnd = str.Substring(indexStart, iSplit).LastIndexOf("") + 1;
                        }

                        this.lbContents.Text += str.Substring(indexStart, indexEnd) + "\r\n";

                        indexStart += indexEnd;
                    }
                    this.lbContents.Text = this.lbContents.Text.Substring(0, this.lbContents.Text.Length - 4);
                }
                else
                {
                    this.lbContents.Text = model.origin.ToString();
                }
            }
            else if (ut == UnitType.Weight)
            {
                if (type == 2)
                {
                    this.Width = 70;
                    this.lbContents.Width = 70;
                }
                else if (form != null)
                {
                    this.Width = 140;
                    this.lbContents.Width = 140;
                }
                else
                {
                    this.Width = 49;
                    this.lbContents.Width = 49;
                }

                this.lbContents.Font = new Font("나눔고딕", 7);
                this.lbContents.Text = model.weight.ToString();
            }
            else if (ut == UnitType.sizes)
            {
                if (type == 2)
                {
                    this.Width = 64;
                    this.lbContents.Width = 64;
                }
                else
                {
                    this.Width = 128;
                    this.lbContents.Width = 128;
                }
                this.lbContents.Font = new Font("나눔고딕", 7);
                this.lbContents.Text = model.sizes.ToString();
            }
            else if (ut == UnitType.sizes1)
            {
                this.Width = 103;
                this.lbContents.Width = 103;
                this.lbContents.Font = new Font("나눔고딕", 7);
                this.lbContents.Text = model.sizes1.ToString();
            }
            else if (ut == UnitType.sizes2)
            {
                this.Width = 77;
                this.lbContents.Width = 77;
                this.lbContents.Font = new Font("나눔고딕", 7);
                this.lbContents.Text = model.sizes2.ToString();
            }
            else if (ut == UnitType.isTax)
            {
                this.Width = 43;
                this.lbContents.Width = 43;
                this.lbContents.Font = new Font("나눔고딕", 7);
                this.lbContents.Text = model.is_tax.ToString();
            }
            else if (ut == UnitType.remark)
            {
                this.Width = 96;
                this.lbContents.Width = 96;
                this.lbContents.Font = new Font("나눔고딕", 7);
                this.lbContents.Text = model.remark.ToString();
            }
            else if (ut == UnitType.price)
            {
                if (type == 2)
                {
                    this.Width = 69;
                    this.lbContents.Width = 69;
                }
                else if (form != null)
                {
                    this.Width = 138;
                    this.lbContents.Width = 138;
                }
                else
                {
                    this.Width = 89;
                    this.lbContents.Width = 89;
                }
                this.lbContents.Font = new Font("나눔고딕", 7, FontStyle.Bold);
                this.lbContents.TextAlign = ContentAlignment.MiddleRight;

                if (model.sales_price == 0)
                    this.lbContents.Text = "-";
                else if (model.sales_price == -1)
                    this.lbContents.Text = "-";
                else if (model.sales_price == -2)
                    this.lbContents.Text = "문의";
                else if (model.sales_price == -3)
                    this.lbContents.Text = "★통관예정★";
                else
                {
                    //말풍선
                    if (!string.IsNullOrEmpty(model.edit_date))
                    {
                        DateTime nowDate = DateTime.Now;
                        DateTime editDate = Convert.ToDateTime(model.edit_date);
                        if ((nowDate - editDate).Days > 61)
                        {
                            this.lbContents.ForeColor = Color.Red;
                            this.lbContents.MouseHover += MouseHover_Event;
                        }
                    }
                    //출력
                    if (model.price_unit == "팩")
                        this.lbContents.Text = model.sales_price.ToString("#,##0") + " /p";
                    else if (model.price_unit == "kg" || model.price_unit == "Kg" || model.price_unit == "KG")
                        this.lbContents.Text = model.sales_price.ToString("#,##0") + " /k";
                    else
                        this.lbContents.Text = model.sales_price.ToString("#,##0");
                }

                //두 품목서 공통품목 강조
                if (accent2 < 0)
                    this.lbContents.ForeColor = Color.LightGray;
                else
                    this.lbContents.ForeColor = Color.Black;

            }
            else if (ut == UnitType.costUnit)
            {
                this.Width = 45;
                this.lbContents.Width = 45;
                this.lbContents.Font = new Font("나눔고딕", 7, FontStyle.Bold);
                this.lbContents.Text = model.cost_unit;
            }
            else if (ut == UnitType.trayPrice)
            {
                this.Width = 71;
                this.lbContents.Width = 71;
                this.lbContents.Font = new Font("나눔고딕", 7, FontStyle.Bold);
                this.lbContents.TextAlign = ContentAlignment.MiddleRight;
                this.lbContents.Text = model.tray_price;
            }
            else if (ut == UnitType.boxPrice)
            {
                this.Width = 70;
                this.lbContents.Width = 70;
                this.lbContents.Font = new Font("나눔고딕", 7, FontStyle.Bold);
                this.lbContents.TextAlign = ContentAlignment.MiddleRight;

                double price = model.sales_price;
                if (price == 0)
                    this.lbContents.Text = "-";
                else if (price == -1)
                    this.lbContents.Text = "-";
                else if (price == -2)
                    this.lbContents.Text = "문의";
                else if (price == -3)
                    this.lbContents.Text = "★통관예정★";
                else
                {
                    this.lbContents.Text = price.ToString("#,##0");
                    //말풍선
                    if (!string.IsNullOrEmpty(model.edit_date))
                    {
                        DateTime nowDate = DateTime.Now;
                        DateTime editDate = Convert.ToDateTime(model.edit_date);
                        if ((nowDate - editDate).Days > 61)
                        {
                            this.lbContents.ForeColor = Color.Red;
                            this.lbContents.MouseHover += MouseHover_Event;
                        }
                    }

                }

            }
            this.lbContents.Font = AutoFontSize(this.lbContents, this.lbContents.Text);

            int cnt = 0;
        retry:
            this.lbContents.Font = new Font("나눔고딕", this.lbContents.Font.Size - cnt);
        
            if (lbContents.Height > this.height && this.lbContents.Font.Size - cnt > 3)
            {
                cnt += 1;
                goto retry;
            }

            //this.txtUpdate.Font = this.lbContents.Font;


            //가격강조
            if (accent && ut == UnitType.price)
            {
                this.lbContents.Text = "★" + this.lbContents.Text;
            }

            this.lbContents.Text = this.lbContents.Text.Trim();

            
        }




        //수정반영
        public void ContentsUpdate(string contents)
        {
            lbContents.Text = contents;
            lbContents.Font = AutoFontSize(lbContents, lbContents.Text);
            int cnt = 0;
        retry:
            this.lbContents.Font = new Font("나눔고딕", this.lbContents.Font.Size - cnt);

            if (lbContents.Height > this.height && this.lbContents.Font.Size - cnt > 3)
            {
                cnt += 1;
                goto retry;
            }
            if(form != null)
                form.UpdateCommit(model, ut, contents);
            else if(oneform != null)
                oneform.UpdateCommit(model, ut, contents);
        }

        private void txtUpdate_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Modifiers == Keys.Control)
            {

                if (e.KeyCode == Keys.Enter)
                {
                    lbContents.Text = txtUpdate.Text;
                    txtUpdate.Visible = false;
                    txtUpdate.Select(txtUpdate.Text.Length, 0);
                    txtUpdate.ScrollToCaret();

                    lbContents.Font = AutoFontSize(lbContents, lbContents.Text);  
                    int cnt = 0;
                retry:
                    this.lbContents.Font = new Font("나눔고딕", this.lbContents.Font.Size - cnt);

                    if (lbContents.Height > this.height && this.lbContents.Font.Size - cnt > 3)
                    {
                        cnt += 1;
                        goto retry;
                    }
                    if(form != null)
                        form.UpdateCommit(model, ut, txtUpdate.Text);

                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        txtUpdate.Text = "";
                        txtUpdate.Visible = false;
                        break;
                }
            }            
        }
        //내역수정
        private void lbContents_DoubleClick(object sender, EventArgs e)
        {
            /*txtUpdate.Visible = true;

            if (txtUpdate.Height < 40)
            {
                txtUpdate.Height = 40;
            }

            txtUpdate.Text = lbContents.Text;
            txtUpdate.Focus();*/

            string category = "";
            switch (ut)
            {
                case UnitType.Category:
                    category = "category";
                    break;
                case UnitType.Product:
                    category = "product";
                    break;
                case UnitType.Origin:
                    category = "origin";
                    break;
                case UnitType.Weight:
                    category = "weight";
                    break;
                case UnitType.sizes:
                    category = "sizes";
                    break;
                case UnitType.price:
                    category = "price";
                    break;

                case UnitType.sizes1:
                    category = "sizes1";
                    break;
                case UnitType.sizes2:
                    category = "sizes2";
                    break;
                case UnitType.costUnit:
                    category = "cost_unit";
                    break;
                case UnitType.trayPrice:
                    category = "tray_price";
                    break;
                case UnitType.boxPrice:
                    category = "box_price";
                    break;
                case UnitType.isTax:
                    category = "is_tax";
                    break;
                case UnitType.remark:
                    category = "remark";
                    break;
            }
            ProductUnitManger pum = new ProductUnitManger(this, lbContents.Text, model, category);
            pum.StartPosition = FormStartPosition.Manual;

            int cX = Cursor.Position.X;
            int cY = Cursor.Position.Y;
            if (cY > 656)
            {
                cY = 656;
            }
            Point mousePoint = new Point(cX, cY); //마우스가 클릭된 위치
            pum.Location = mousePoint;
            pum.ShowDialog();
        }

        //자동 폰트 사이즈 정렬
        public Font AutoFontSize(Label label, String text)
        {
            Font ft;
            Graphics gp;
            SizeF sz;
            Single Faktor, FaktorX, FaktorY;

            gp = label.CreateGraphics();
            sz = gp.MeasureString(text, label.Font);

            gp.Dispose();

            FaktorX = (label.Width) / sz.Width;
            FaktorY = (label.Height) / sz.Height;

            if (FaktorX > FaktorY)
            {
                Faktor = FaktorY;
            }
            else
            {
                Faktor = FaktorX;
            }

            ft = label.Font;
            float f = ft.SizeInPoints * (Faktor) - 1;


            if (ut == UnitType.Category || ut == UnitType.Product)
            {
                if (f > 15)
                {
                    f = 8;
                }
                else if (f < 5)
                {
                    f = 5;
                }
            }
            else
            {
                if (f > 8)
                {
                    f = 8;
                }
                else if (f < 5)
                {
                    f = 5;
                }
            }


            return new Font(ft.Name, f);
        }
        /// <summary>

        /// 마우스 Hover 이벤트 핸들러

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        public void MouseHover_Event(object sender, EventArgs e)

        {
            //Tooltip 객체 생성
            ToolTip ttip = new ToolTip();
            ttip.ToolTipTitle = Convert.ToDateTime(model.edit_date).ToString("yyyy-MM-dd");
            ttip.ShowAlways = true;
            ttip.IsBalloon = true;

            ttip.SetToolTip(this.lbContents, model.manager1);

        }


        #region 우클릭 메뉴
        private void lbContents_MouseDown(object sender, MouseEventArgs e)
        {
            if (ut == UnitType.Category)
            {
                try
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("잘라내기");
                        m.Items.Add("붙혀넣기");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        Point mousePoint = new Point(e.X, e.Y); //마우스가 클릭된 위치
                        m.Show(this, mousePoint);
                        //Selection
                        /*PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;*/
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (ut == UnitType.Category)
            {
                switch (e.ClickedItem.Text)
                {
                        case "잘라내기":
                            if (form != null)
                                form.copyUnit(model.category);
                            break;
                        case "붙혀넣기":
                            if (form != null)
                                form.pasteUnit(model.category);
                            break;

                        default:
                            break;
                }
                
            }
        }


        #endregion

        
    }
}

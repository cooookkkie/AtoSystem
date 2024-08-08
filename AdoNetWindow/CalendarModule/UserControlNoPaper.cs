using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.CalendarModule
{
    public partial class UserControlNoPaper : UserControl
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        calendar cd;
        UsersModel um;
        public UserControlNoPaper(calendar calendar, UsersModel uModel, string id, DataRow row)
        {
            InitializeComponent();

            cd = calendar;
            um = uModel;

            lbId.Text = id;
            lb.Text = row["ato_no"].ToString() + " (ETA : " + row["eta"].ToString() + ")";
        }

        private void lb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int id;
            if (int.TryParse(lbId.Text, out id))
            {
                UnPendingManager upm = new UnPendingManager(cd, um, "", id);
                upm.StartPosition = FormStartPosition.CenterParent;
                upm.ShowDialog();
            }
        }

        private void lb_MouseUp(object sender, MouseEventArgs e)
        { 
            if(e.Button == MouseButtons.Right)
            { 
                try
                {
                    DateTime dt;
                    DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                    if (um.auth_level >= 50)
                    {
                        this.BorderStyle = BorderStyle.Fixed3D;
                        ContextMenuStrip m = new ContextMenuStrip();
                        //공통
                        m.Items.Add("O 처리");
                        m.Items.Add("X 처리");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        m.Closed += new ToolStripDropDownClosedEventHandler(m_Closed);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(lb, e.Location);
                    }
                }
                catch
                {

                }
            }
        }
        //우클릭 메뉴 Closiing event
        void m_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.BorderStyle = BorderStyle.None;
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "O 처리":
                        if (customsRepository.UpdateHOCO(Convert.ToInt32(lbId.Text), "O") == -1)
                            MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                        else
                            cd.CheckNoPaper();
                        break;
                    case "X 처리":
                        if (customsRepository.UpdateHOCO(Convert.ToInt32(lbId.Text), "X") == -1)
                            MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                        else
                            cd.CheckNoPaper();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
            }
        }
    }
}

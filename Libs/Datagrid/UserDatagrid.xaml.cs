using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Libs.Datagrid
{
    /// <summary>
    /// UserDatagrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserDatagrid : UserControl
    {
        UsersModel um;
        public UserDatagrid(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }
        public void SetColumn(string[] colText, string[] colName)
        {
            for (int i = 0; i < colText.Length; i++)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = colText[i];
                textColumn.Binding = new Binding(colName[i]);
                dg.Columns.Add(textColumn);
            }
        }

        public DataGrid datagrid()
        {
            return this.dg;
        }


    }
}

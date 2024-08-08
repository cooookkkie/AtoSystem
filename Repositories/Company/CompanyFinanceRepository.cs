using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public class CompanyFinanceRepository : ClassRoot, ICompanyFinanceRepository
    {
        public DataTable GetCompanyFinance(string company_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                      ");
            qry.Append($"\n   *                                         ");
            qry.Append($"\n FROM t_company_finance                      ");
            qry.Append($"\n WHERE 1=1                                   ");
            qry.Append($"\n   AND company_id = {company_id}             ");
            qry.Append($"\n ORDER BY year DESC                          ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public StringBuilder DeleteCompanyFinanc(string company_id, string year = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_company_finance                ");
            qry.Append($"\n WHERE 1=1               ");
            if(!string.IsNullOrEmpty(year))
                qry.Append($"\n   AND year = {year}               ");
            if (!string.IsNullOrEmpty(company_id))
                qry.Append($"\n   AND company_id = {company_id}               ");
            return qry;
        }

        public StringBuilder InsertCompanyFinanc(CompanyFinanceModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_company_finance (            ");
            qry.Append($"\n   company_id                               ");
            qry.Append($"\n , year                                     ");
            qry.Append($"\n , capital_amount                           ");
            qry.Append($"\n , debt_amount                              ");
            qry.Append($"\n , sales_amount                             ");
            qry.Append($"\n , net_income_amount                        ");
            qry.Append($"\n , edit_user                                ");
            qry.Append($"\n , updatetime                               ");
            qry.Append($"\n ) VALUES (                                 ");
            qry.Append($"\n    {model.company_id}                      ");
            qry.Append($"\n ,  {model.year}                            ");
            qry.Append($"\n ,  {model.capital_amount}                  ");
            qry.Append($"\n ,  {model.debt_amount}                     ");
            qry.Append($"\n ,  {model.sales_amount}                    ");
            qry.Append($"\n ,  {model.net_income_amount}               ");
            qry.Append($"\n , '{model.edit_user}'                      ");
            qry.Append($"\n , '{model.updatetime}'                     ");
            qry.Append($"\n )                                          ");
            return qry;
        }
    }
}

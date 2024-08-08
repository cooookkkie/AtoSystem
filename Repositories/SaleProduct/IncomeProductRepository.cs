using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SaleProduct
{
    public class IncomeProductRepository : ClassRoot, IincomeProductRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public DataTable GetIncomeProduct(string division, string product, string origin, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n select                             ");
            qry.Append($"\n *                                  ");
            qry.Append($"\n FROM atotrading.t_income_product   ");
            qry.Append($"\n WHERE 1=1                          ");
            if(!string.IsNullOrEmpty(division))
                qry.Append($"\n   {commonRepository.whereSql("division", division)}         ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("product", product)}           ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("origin", origin)}             ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {commonRepository.whereSql("manager", manager)}           ");
            qry.Append($"\n ORDER BY manager, division         ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public StringBuilder DeleteData(IncomeProductModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_income_product                        ");
            qry.Append($" WHERE manager = '{model.manager}'                   ");
            qry.Append($"   AND division = '{model.division}'                 ");

            return qry;
        }

        public StringBuilder InsertData(IncomeProductModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_income_product (                        ");
            qry.Append($"\n   division                                            ");
            qry.Append($"\n , product                                             ");
            qry.Append($"\n , origin                                              ");
            qry.Append($"\n , process                                             ");
            qry.Append($"\n , reason                                              ");
            qry.Append($"\n , remark                                              ");
            qry.Append($"\n , manager                                             ");
            qry.Append($"\n , edit_user                                           ");
            qry.Append($"\n , updatetime                                          ");
            qry.Append($"\n ) VALUES (                                            ");
            qry.Append($"\n   '{model.division  }'                                          ");
            qry.Append($"\n , '{model.product   }'                                          ");
            qry.Append($"\n , '{model.origin    }'                                          ");
            qry.Append($"\n , '{model.process   }'                                          ");
            qry.Append($"\n , '{model.reason    }'                                          ");
            qry.Append($"\n , '{model.remark    }'                                          ");
            qry.Append($"\n , '{model.manager   }'                                          ");
            qry.Append($"\n , '{model.edit_user }'                                          ");
            qry.Append($"\n , '{model.updatetime}'                                          ");
            qry.Append($"\n )                                                               ");

            string sql = qry.ToString();
            return qry;
        }
    }
}

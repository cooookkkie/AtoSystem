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
    public class ProductExcludedSalesRepository : ClassRoot, IProductExcludedSalesRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public DataTable GetExcludedSales(string sttdate, string enddate, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                          ");
            qry.Append($"\n *                               ");
            qry.Append($"\n FROM t_product_excluded_sales   ");
            qry.Append($"\n WHERE 1=1                       ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND sale_date >= '{sttdate}'   ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND sale_date <= '{enddate}'   ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND product LIKE '%{product}%'   ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND origin LIKE '%{origin}%'   ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND sizes LIKE '%{sizes}%'   ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND unit LIKE '%{unit}%'   ");
            if (!string.IsNullOrEmpty(price_unit))
                qry.Append($"\n   AND price_unit LIKE '%{price_unit}%'   ");
            if (!string.IsNullOrEmpty(unit_count))
                qry.Append($"\n   AND unit_count LIKE '%{unit_count}%'   ");
            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND seaover_unit LIKE '%{seaover_unit}%'   ");
            qry.Append($"\n ORDER BY sale_date, product, origin, sizes, unit   ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetExcludedSalesByMonth(string sttdate, string enddate, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit, string sub_product, bool isMerge)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                         ");
            qry.Append($"\n   sale_date                                                                                                    ");
            qry.Append($"\n , product                                                                                                      ");
            qry.Append($"\n , origin                                                                                                       ");
            qry.Append($"\n , sizes                                                                                                        ");
            qry.Append($"\n , unit                                                                                                         ");
            qry.Append($"\n , price_unit                                                                                                   ");
            qry.Append($"\n , unit_count                                                                                                   ");
            qry.Append($"\n , seaover_unit                                                                                                 ");
            if(string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n , SUM(sale_qty) AS sale_qty                                                    ");
            else
                qry.Append($"\n , SUM(sale_qty * seaover_unit / {seaover_unit}) AS sale_qty                                                    ");
            qry.Append($"\n FROM t_product_excluded_sales                                                                                  ");
           
            qry.Append($"\n WHERE 1=1                       ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND sale_date >= '{sttdate}'   ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND sale_date <= '{enddate}'   ");

            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND product LIKE '%{product}%'   ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   AND origin LIKE '%{origin}%'   ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n   AND sizes LIKE '%{sizes}%'   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n   AND unit LIKE '%{unit}%'   ");
                if (!string.IsNullOrEmpty(price_unit))
                    qry.Append($"\n   AND price_unit LIKE '%{price_unit}%'   ");
                if (!string.IsNullOrEmpty(unit_count))
                    qry.Append($"\n   AND unit_count LIKE '%{unit_count}%'   ");
                if (!string.IsNullOrEmpty(seaover_unit))
                    qry.Append($"\n   AND seaover_unit LIKE '%{seaover_unit}%'   ");
            }
            else if (sub_product != null && !string.IsNullOrEmpty(sub_product))
            {
                string[] products = sub_product.Trim().Split('\n');
                if (products.Length > 0)
                {
                    qry.Append($"\n   AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        if (i == 0)
                            qry.Append($"\n (");
                        else
                            qry.Append($"\n OR (");
                        string[] sub = products[i].Trim().Split('^');
                        if (product.Length > 1)
                        {
                            qry.Append($"product = '{sub[0]}' ");
                            qry.Append($"AND origin = '{sub[1]}' ");
                            qry.Append($"AND sizes = '{sub[2]}' ");
                            qry.Append($"AND seaover_unit = '{sub[6]}' ");
                            qry.Append($") ");
                        }
                    }
                    qry.Append($"\n)");
                }
            }
            qry.Append($"\n GROUP BY DATE_FORMAT(sale_date, '%Y%m'), product, origin, sizes, unit, price_unit, unit_count, seaover_unit    ");
            qry.Append($"\n ORDER BY sale_date, product, origin, sizes, unit   ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public StringBuilder DeleteExcludedSales(int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_product_excluded_sales    ");
            qry.Append($" WHERE id = {id}                   ");

            return qry;
        }
        public StringBuilder InsertExcludedSales(ProductExcludedSalesModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_product_excluded_sales (   ");
            qry.Append($"   id                                 ");
            qry.Append($" , product                            ");
            qry.Append($" , origin                             ");
            qry.Append($" , sizes                              ");
            qry.Append($" , unit                               ");
            qry.Append($" , price_unit                         ");
            qry.Append($" , unit_count                         ");
            qry.Append($" , seaover_unit                       ");
            qry.Append($" , sale_date                          ");
            qry.Append($" , sale_company                       ");
            qry.Append($" , sale_qty                           ");
            qry.Append($" , remark                             ");
            qry.Append($" ) VALUES (                           ");
            qry.Append($"    {model.id}                        ");
            qry.Append($" , '{model.product     }'             ");
            qry.Append($" , '{model.origin      }'             ");
            qry.Append($" , '{model.sizes       }'             ");
            qry.Append($" , '{model.unit        }'             ");
            qry.Append($" , '{model.price_unit  }'             ");
            qry.Append($" , '{model.unit_count  }'             ");
            qry.Append($" , '{model.seaover_unit}'             ");
            qry.Append($" , '{model.sale_date   }'             ");
            qry.Append($" , '{model.sale_company}'             ");
            qry.Append($" ,  {model.sale_qty    }              ");
            qry.Append($" , '{model.remark      }'             ");
            qry.Append($" )                                    ");

            return qry;
        }

        public DataTable GetExcludedSalesAsOne(string sttdate, string enddate, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                          ");
            qry.Append($"\n product, origin, sizes, unit, price_unit, unit_count, seaover_unit       ");
            qry.Append($"\n , SUM(sale_qty) AS sale_qty       ");
            qry.Append($"\n FROM t_product_excluded_sales   ");
            qry.Append($"\n WHERE 1=1                       ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND sale_date >= '{sttdate}'   ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND sale_date <= '{enddate}'   ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("product", product)}   ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("origin", origin)}   ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("sizes", sizes)}   ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   {commonRepository.whereSql("unit", unit)}   ");
            if (!string.IsNullOrEmpty(price_unit))
                qry.Append($"\n   {commonRepository.whereSql("price_unit", price_unit)}   ");
            if (!string.IsNullOrEmpty(unit_count))
                qry.Append($"\n   {commonRepository.whereSql("unit_count", unit_count)}   ");
            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   {commonRepository.whereSql("seaover_unit", seaover_unit)}   ");
            qry.Append($"\n GROUP BY product, origin, sizes, unit, price_unit, unit_count, seaover_unit   ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
    }
}

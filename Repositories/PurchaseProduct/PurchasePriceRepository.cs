using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PurchasePriceRepository : ClassRoot, IPurchasePriceRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public DataTable GetMultiPurchaseDashborad(string warehouse, string product, string origin, string sizes, string unit, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                        ");
            qry.Append($"\n   p1.*                                        ");
            qry.Append($"\n , p2.manager                                  ");
            qry.Append($"\n FROM (                                        ");
            qry.Append($"\n SELECT                                        ");
            qry.Append($"\n   distinct p.product                          ");
            qry.Append($"\n , p.origin                                    ");
            qry.Append($"\n , p.sizes                                     ");
            qry.Append($"\n , p.unit                                      ");
            qry.Append($"\n FROM t_purchase_price AS p                    ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                ");
            qry.Append($"\n   on p.cid = c.id                             ");
            qry.Append($"\n WHERE c.name IS NOT NULL                      ");
            if(!string.IsNullOrEmpty(warehouse.Trim()))
                qry.Append($"\n  AND c.name LIKE  '%{warehouse}%'                      ");
            //qry.Append($"\n   {commonRepository.whereSql("c.name", warehouse)}                      ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n   {commonRepository.whereSql("p.product", product)}                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n   {commonRepository.whereSql("p.origin", origin)}                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n   {commonRepository.whereSql("p.sizes", sizes)}                      ");
            if (!string.IsNullOrEmpty(unit.Trim()))
                qry.Append($"\n   {commonRepository.whereSql("p.unit", unit)}                      ");
            
            qry.Append($"\n ) AS p1                                       ");
            qry.Append($"\n LEFT OUTER JOIN t_product_other_cost AS p2    ");
            qry.Append($"\n   ON p1.product = p2.product                  ");
            qry.Append($"\n   AND p1.origin = p2.origin                   ");
            qry.Append($"\n   AND p1.sizes = p2.sizes                     ");
            qry.Append($"\n   AND p1.unit = p2.unit                       ");
            qry.Append($"\n WHERE 1=1                                     ");
            qry.Append($"\n   AND IFNULL(p1.product, '') != ''                      ");
            qry.Append($"\n   AND IFNULL(p1.origin, '') != ''                      ");
            qry.Append($"\n   AND IFNULL(p1.sizes, '') != ''                      ");
            qry.Append($"\n   AND IFNULL(p1.unit, '') != ''                      ");



            if (!string.IsNullOrEmpty(manager.Trim()))
                qry.Append($"\n   {commonRepository.whereSql("p2.manager", manager)}                      ");
            qry.Append($"\n ORDER BY p1.product, p1.origin, p1.sizes, p1.unit                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPurchaseDashborad(string sttdate, string enddate, List<string[]> productList)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                          ");
            qry.Append($"\n   p.id                                          ");
            qry.Append($"\n , p.product                                     ");
            qry.Append($"\n , p.origin                                      ");
            qry.Append($"\n , p.sizes                                       ");
            qry.Append($"\n , p.unit                                        ");
            qry.Append($"\n , p.updatetime                                  ");
            qry.Append($"\n , p.cid AS company_id                           ");
            qry.Append($"\n , c.name AS company                             ");
            qry.Append($"\n , p.purchase_price                              ");
            qry.Append($"\n FROM t_purchase_price AS p                      ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                  ");
            qry.Append($"\n   ON p.cid = c.id                               ");
            qry.Append($"\n WHERE 1=1                                       ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND p.updatetime >= '{sttdate}'                            ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND p.updatetime <= '{enddate}'                            ");
            if (productList != null && productList.Count > 0)
            {
                qry.Append($"\n   AND (                                                      ");

                string whrStr = "";

                for (int i = 0; i < productList.Count; i++)
                {
                    if(string.IsNullOrEmpty(whrStr))
                        whrStr = $"\n   (p.product = '{productList[i][0]}' AND p.origin = '{productList[i][1]}' AND p.sizes = '{productList[i][2]}' AND p.unit = '{productList[i][3]}')";
                    else
                        whrStr += $"\n   OR (p.product = '{productList[i][0]}' AND p.origin = '{productList[i][1]}' AND p.sizes = '{productList[i][2]}' AND p.unit = '{productList[i][3]}')";
                }

                qry.Append(whrStr);
                qry.Append($"\n   )                                                      ");

            }
            qry.Append($"\n ORDER BY p.product, p.origin, p.sizes, p.unit, p.updatetime DESC                                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPurchaseShipperList(string company, string product = "", string origin = "", string sizes = "", string unit = "")
        {
            company = company.Replace("'", "");
            product = product.Replace("'", "");
            origin = origin.Replace("'", "");
            sizes = sizes.Replace("'", "");
            unit = unit.Replace("'", "");

            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                          ");
            qry.Append($"\n   distinct c.name AS company                    ");
            qry.Append($"\n FROM t_purchase_price AS p                      ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                  ");
            qry.Append($"\n   ON p.cid = c.id                               ");
            qry.Append($"\n WHERE 1=1                                       ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n   {commonRepository.whereSql("c.name", company)}             ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND p.product = '{product}'               ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND p.origin = '{origin}'               ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND p.sizes = '{sizes}'               ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND p.unit = '{unit}'               ");
            qry.Append($"\n ORDER BY c.name                                 ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetPurchasePriceList(string purchase_date, string product, string origin, string sizes, string unit, string id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                           ");
            qry.Append($"\n   p.id                                                                           ");
            qry.Append($"\n , p.product                                                                      ");
            qry.Append($"\n , p.origin                                                                       ");
            qry.Append($"\n , p.sizes                                                                        ");
            qry.Append($"\n , p.unit                                                                         ");
            qry.Append($"\n , p.cost_unit                                                                    ");
            qry.Append($"\n , IFNULL(p.purchase_price, 0) AS purchase_price                                  ");
            qry.Append($"\n , IFNULL(p.fixed_tariff, 0) AS fixed_tariff                                      ");
            qry.Append($"\n , p.cost_unit                                                                    ");
            qry.Append($"\n , IF(IFNULL(p.is_private, 0) = 0, 'FALSE', 'TRUE') AS is_private                 ");
            qry.Append($"\n , IF(IFNULL(p.weight_calculate, 0) = 0, 'FALSE', 'TRUE') AS weight_calculate     ");
            qry.Append($"\n , IF(IFNULL(p.is_FOB, 0) = 0, 'FALSE', 'TRUE') AS is_FOB                         ");
            qry.Append($"\n , p.edit_user                                                                    ");
            qry.Append($"\n , p.updatetime                                                                   ");
            qry.Append($"\n , p.cid                                                                          ");
            qry.Append($"\n , c.name AS company                                                              ");
            qry.Append($"\n FROM t_purchase_price AS p                                                       ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                                                   ");
            qry.Append($"\n   ON p.cid = c.id                                                                ");
            qry.Append($"\n WHERE 1=1                                                                        ");
            if (!string.IsNullOrEmpty(purchase_date))
                qry.Append($"\n   AND p.updatetime = '{purchase_date}'                            ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND p.product = '{product}'                                ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND p.origin = '{origin}'                                  ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND p.sizes = '{sizes}'                                    ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND p.unit = '{unit}'                                      ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n   AND p.id = {id}                                            ");
            qry.Append($"\n ORDER BY p.updatetime DESC, p.purchase_price    ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPurchasePriceList(string sttdate, string enddate, string product, string origin, string sizes, string unit, string id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                          ");
            qry.Append($"\n   p.id                                          ");
            qry.Append($"\n , p.updatetime                                  ");
            qry.Append($"\n , p.cid                                         ");
            qry.Append($"\n , c.name AS company                             ");
            qry.Append($"\n , p.purchase_price                              ");
            qry.Append($"\n FROM t_purchase_price AS p                      ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                  ");
            qry.Append($"\n   ON p.cid = c.id                               ");
            qry.Append($"\n WHERE 1=1                                       ");
            if(!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND p.updatetime >= '{sttdate}'                            ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND p.updatetime <= '{enddate}'                            ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND p.product = '{product}'                                ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND p.origin = '{origin}'                                  ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND p.sizes = '{sizes}'                                    ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND p.unit = '{unit}'                                      ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n   AND p.id = {id}                                            ");
            qry.Append($"\n ORDER BY p.updatetime DESC, p.purchase_price    ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public int GetNextId()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT IF(MAX(id) IS NULL, 0, MAX(id)) + 1 ");
            qry.Append($" FROM t_purchase_price");
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return int.Parse(command.ExecuteScalar().ToString());
        }
        public DataTable GetPurchasePriceById(string product, string origin, string sizes, string unit, string updatetime, string company, string unit_price)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                                                                                                               ");
            qry.Append($"\n   c.name AS cname                                                                                                                                                                                                                                    ");
            qry.Append($"\n , p.product                                                                                                                                                                                                                                          ");
            qry.Append($"\n , p.origin                                                                                                                                                                                                                                           ");
            qry.Append($"\n , p.sizes                                                                                                                                                                                                                                            ");
            qry.Append($"\n , p.sizes2                                                                                                                                                                                                                                           ");
            qry.Append($"\n , p.unit                                                                                                                                                                                                                                             ");
            qry.Append($"\n , p.cost_unit                                                                                                                                                                                                                                        ");
            qry.Append($"\n , p.purchase_price                                                                                                                                                                                                                                   ");
            qry.Append($"\n , p.custom                                                                                                                                                                                                                                           ");
            qry.Append($"\n , p.tax                                                                                                                                                                                                                                              ");
            qry.Append($"\n , p.incidental_expense                                                                                                                                                                                                                               ");
            qry.Append($"\n , p.updatetime                                                                                                                                                                                                                                       ");
            qry.Append($"\n , p.manager                                                                                                                                                                                                                                          ");
            qry.Append($"\n , IFNULL(p.weight_calculate, 'TRUE') AS weight_calculate                                                                                                                                                                                             ");
            qry.Append($"\n , IFNULL(p.tray_calculate , 'FALSE') AS tray_calculate                                                                                                                                                                                               ");
            qry.Append($"\n FROM(                                                                                                                                                                                                                                                ");
            qry.Append($"\n	 SELECT                                                                                                                                                                                                                                              ");
            qry.Append($"\n		p.cid                                                                                                                                                                                                                                            ");
            qry.Append($"\n	  , p.product                                                                                                                                                                                                                                        ");
            qry.Append($"\n	  , p.origin                                                                                                                                                                                                                                         ");
            qry.Append($"\n	  , p.sizes                                                                                                                                                                                                                                          ");
            qry.Append($"\n	  , p.sizes2                                                                                                                                                                                                                                         ");
            qry.Append($"\n	  , p.unit                                                                                                                                                                                                                                           ");
            qry.Append($"\n	  , IFNULL(o.cost_unit, 0)  AS cost_unit                                                                                                                                                                                                             ");
            qry.Append($"\n	  , p.purchase_price                                                                                                                                                                                                                                 ");
            qry.Append($"\n	  , IFNULL(o.custom, 0) AS custom                                                                                                                                                                                                                    ");
            qry.Append($"\n	  , IFNULL(o.tax, 0) AS tax                                                                                                                                                                                                                          ");
            qry.Append($"\n	  , IFNULL(o.incidental_expense, 0) AS incidental_expense                                                                                                                                                                                            ");
            qry.Append($"\n	  , p.updatetime                                                                                                                                                                                                                                     ");
            qry.Append($"\n	  , o.manager                                                                                                                                                                                                                                        ");
            qry.Append($"\n	  , IF(o.weight_calculate = 1, 'TRUE', 'FALSE') AS weight_calculate                                                                                                                                                                                  ");
            qry.Append($"\n	  , IF(o.tray_calculate = 1, 'TRUE', 'FALSE') AS tray_calculate                                                                                                                                                                                      ");
            qry.Append($"\n	  FROM                                                                                                                                                                                                                                               ");
            qry.Append($"\n	  (                                                                                                                                                                                                                                                  ");
            qry.Append($"\n		SELECT                                                                                                                                                                                                                                           ");
            qry.Append($"\n		  cid                                                                                                                                                                                                                                            ");
            qry.Append($"\n		, product                                                                                                                                                                                                                                        ");
            qry.Append($"\n		, origin                                                                                                                                                                                                                                         ");
            qry.Append($"\n		, sizes                                                                                                                                                                                                                                          ");
            qry.Append($"\n		, CASE                                                                                                                                                                                                                                           ");
            qry.Append($"\n			WHEN INSTR(sizes, '/') > 1 THEN SUBSTRING_INDEX(sizes, '/', 1)                                                                                                                                                                               ");
            qry.Append($"\n			 WHEN INSTR(sizes, '미') > 1 THEN SUBSTRING_INDEX(sizes, '미', 1)                                                                                                                                                                              ");
            qry.Append($"\n			 WHEN INSTR(sizes, 'kg') > 1 THEN SUBSTRING_INDEX(sizes, 'kg', 1)                                                                                                                                                                            ");
            qry.Append($"\n			 WHEN INSTR(sizes, 'g') > 1 THEN SUBSTRING_INDEX(sizes, 'g', 1)                                                                                                                                                                              ");
            qry.Append($"\n			 WHEN INSTR(sizes, 'l') > 1 THEN SUBSTRING_INDEX(sizes, 'l', 1)                                                                                                                                                                              ");
            qry.Append($"\n			 WHEN INSTR(sizes, 'up') > 1 THEN SUBSTRING_INDEX(sizes, 'up', 1)                                                                                                                                                                            ");
            qry.Append($"\n			 END AS sizes2                                                                                                                                                                                                                               ");
            qry.Append($"\n		, unit                                                                                                                                                                                                                                           ");
            qry.Append($"\n		, purchase_price                                                                                                                                                                                                                                 ");
            qry.Append($"\n		, MAX(updatetime) AS updatetime                                                                                                                                                                                                                  ");
            qry.Append($"\n		FROM t_purchase_price                                                                                                                                                                                                                            ");
            qry.Append($"\n		WHERE 1=1                                                                                                                                                                                                                                        ");
            qry.Append($"\n		  AND product = '{product}'                                                                                                                                                                                                                                       ");
            qry.Append($"\n		  AND origin = '{origin}'                                                                                                                                                                                                                                       ");
            qry.Append($"\n		  AND sizes = '{sizes}'                                                                                                                                                                                                                                       ");
            qry.Append($"\n		  AND unit = '{unit}'                                                                                                                                                                                                                                       ");
            qry.Append($"\n		  AND purchase_price = {unit_price}                                                                                                                                                                                                                                       ");
            qry.Append($"\n		  AND updatetime = '{updatetime}'                                                                                                                                                                                                                                       ");
            qry.Append($"\n		GROUP BY cid, product, origin, sizes, unit, purchase_price                                                                                                                                                                                       ");
            qry.Append($"\n	  ) AS p                                                                                                                                                                                                                                             ");
            qry.Append($"\n	  LEFT OUTER JOiN  (                                                                                                                                                                                                                                 ");
            qry.Append($"\n		SELECT                                                                                                                                                                                                                                           ");
            qry.Append($"\n         product                                                                                                                                                                                                                                      ");
            qry.Append($"\n       , origin                                                                                                                                                                                                                                       ");
            qry.Append($"\n       , sizes                                                                                                                                                                                                                                        ");
            qry.Append($"\n       , cost_unit                                                                                                                                                                                                                                    ");
            qry.Append($"\n       , custom                                                                                                                                                                                                                                       ");
            qry.Append($"\n       , tax                                                                                                                                                                                                                                          ");
            qry.Append($"\n       , incidental_expense                                                                                                                                                                                                                           ");
            qry.Append($"\n       , manager                                                                                                                                                                                                                                      ");
            qry.Append($"\n       , weight_calculate                                                                                                                                                                                                                             ");
            qry.Append($"\n       , tray_calculate                                                                                                                                                                                                                               ");
            qry.Append($"\n       FROM t_product_other_cost                                                                                                                                                                                                                      ");
            qry.Append($"\n       WHERE 1=1                                                                                                                                                                                                                                      ");
            qry.Append($"\n       GROUP BY product, origin, sizes, cost_unit, custom, tax, incidental_expense, manager, weight_calculate, tray_calculate                                                                                                                         ");
            qry.Append($"\n	  ) AS o                                                                                                                                                                                                                                             ");
            qry.Append($"\n		 ON p.product = o.product                                                                                                                                                                                                                        ");
            qry.Append($"\n		AND p.origin = o.origin                                                                                                                                                                                                                          ");
            qry.Append($"\n		AND p.sizes = o.sizes                                                                                                                                                                                                                            ");
            qry.Append($"\n ) AS p                                                                                                                                                                                                                                               ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                                                                                                                                                                                                                       ");
            qry.Append($"\n   ON p.cid = c.id                                                                                                                                                                                                                                    ");
            qry.Append($"\n WHERE 1=1                                                                                                                                                                                                                                            ");
            qry.Append($"\n  GROUP BY c.name, p.product, p.origin, p.sizes, p.sizes2, p.unit  , p.cost_unit  , p.purchase_price  , p.custom  , p.tax  , p.incidental_expense  , p.updatetime , IFNULL(p.weight_calculate, 'TRUE') , IFNULL(p.tray_calculate, 'FALSE')            ");
            qry.Append($"\n  ORDER BY product, origin, sizes2 + 0, sizes, c.name                                                                                                                                                                                                 ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public PurchasePriceModel GetPurchasePriceAsOne(string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   p.id                        ");
            qry.Append($"\n , p.cid                       ");
            qry.Append($"\n , p.product                   ");
            qry.Append($"\n , p.origin                    ");
            qry.Append($"\n , p.sizes                     ");
            qry.Append($"\n , p.unit                      ");
            qry.Append($"\n , p.purchase_price            ");
            qry.Append($"\n , p.updatetime                ");
            qry.Append($"\n , p.edit_user                 ");
            qry.Append($"\n , IF(IFNULL(is_private, 0) = 0, 'false', 'true') AS is_private                 ");
            qry.Append($"\n , IF(IFNULL(weight_calculate, 0) = 0, 'false', 'true') AS weight_calculate                 ");
            qry.Append($"\n , IF(IFNULL(is_FOB, 0) = 0, 'false', 'true') AS is_FOB                 ");
            qry.Append($"\n , c.name AS company           ");
            qry.Append($"\n , IFNULL(p.fixed_tariff, 0) AS fixed_tariff           ");
            qry.Append($"\n FROM t_purchase_price AS p    ");
            qry.Append($"\n INNER JOIN t_company AS c          ");
            qry.Append($"\n   ON p.cid = c.id             ");
            qry.Append($"\n WHERE 1=1                     ");
            qry.Append($"\n   AND p.id = {id}       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetPurchasePriceAsOne(dr);
        }
        private PurchasePriceModel SetPurchasePriceAsOne(MySqlDataReader rd)
        {
            PurchasePriceModel model = null;
            while (rd.Read())
            {
                model = new PurchasePriceModel();;
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.product = rd["product"].ToString();
                model.origin = rd["origin"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.unit = rd["unit"].ToString();
                model.company = rd["company"].ToString();
                model.purchase_price = Convert.ToDouble(rd["purchase_price"].ToString());
                model.updatetime = Convert.ToDateTime(rd["updatetime"].ToString()).ToString("yyyy-MM-dd");
                model.edit_user = rd["edit_user"].ToString();
                model.is_FOB = Convert.ToBoolean(rd["is_FOB"].ToString());
                model.weight_calculate = Convert.ToBoolean(rd["weight_calculate"].ToString());
                model.is_private = Convert.ToBoolean(rd["is_private"].ToString());
                model.fixed_tariff = Convert.ToDouble(rd["fixed_tariff"].ToString());
            }
            rd.Close();
            return model;
        }

        public List<PurchasePriceModel> GetPurchasePriceForChart(string sttdate, string enddate, string product, string origin, string sizes, string unit, string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                            ");
            qry.Append($"\n   p.*                          ");
            qry.Append($"\n , c.name AS cname              ");
            qry.Append($"\n FROM t_purchase_price AS p     ");
            qry.Append($"\n INNER JOIN t_company AS c      ");
            qry.Append($"\n   ON p.cid = c.id                ");
            qry.Append($"\n WHERE 1=1                      ");
            qry.Append($"\n   AND p.updatetime >= '{sttdate}'       ");
            qry.Append($"\n   AND p.updatetime <= '{enddate}'       ");
            qry.Append($"\n   AND p.product = '{product}'           ");
            qry.Append($"\n   AND p.origin = '{origin}'             ");
            qry.Append($"\n   AND p.sizes = '{sizes}'               ");
            qry.Append($"\n   AND p.unit = '{unit}'                 ");
            if (!string.IsNullOrEmpty(company))
            { 
                qry.Append($"\n   AND c.name = '{company}'              ");
            }
            qry.Append($"\n ORDER BY p.updatetime");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetPurchasePrice(dr);
        }
        public DataTable GetPurchasePriceForChartDay(string sttdate, string enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                            ");
            qry.Append($"\n   p.id                                                      ");
            qry.Append($"\n , p.cid                                                     ");
            qry.Append($"\n , p.product                                                 ");
            qry.Append($"\n , p.origin                                                  ");
            qry.Append($"\n , p.sizes                                                   ");
            qry.Append($"\n , p.unit                                                    ");
            qry.Append($"\n , p.purchase_price / p.unit * {unit} AS purchase_price      ");
            qry.Append($"\n , IFNULL(p.fixed_tariff, 0) AS fixed_tariff                 ");
            qry.Append($"\n , p.updatetime                                              ");
            qry.Append($"\n , p.edit_user                                               ");
            qry.Append($"\n , p.is_private                                              ");
            qry.Append($"\n , c.name AS cname                                           ");
            qry.Append($"\n FROM t_purchase_price AS p     ");
            qry.Append($"\n INNER JOIN t_company AS c      ");
            qry.Append($"\n   ON p.cid = c.id                ");
            qry.Append($"\n WHERE 1=1                      ");
            qry.Append($"\n   AND p.updatetime >= '{sttdate}'       ");
            qry.Append($"\n   AND p.updatetime <= '{enddate}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND p.product = '{product}'           ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   AND p.origin = '{origin}'             ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n   AND p.sizes = '{sizes}'               ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n   AND p.unit = '{unit}'                 ");
            }
            else if(!string.IsNullOrEmpty(sub_product))
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n   AND (                 ");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");

                            qry.Append($" p.product = '{sub[0]}' ");
                            qry.Append($" AND p.origin = '{sub[1]}' ");
                            qry.Append($" AND p.sizes = '{sub[2]}' ");
                            qry.Append($" AND p.unit = '{sub[6]}' ");
                            qry.Append($") ");
                        }
                    }
                    qry.Append($") ");
                }
            }
            
            qry.Append($"\n ORDER BY p.updatetime DESC   ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCurrentPrice()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                            ");
            qry.Append($"\n p.*                                                                               ");
            qry.Append($"\n , c.name AS company                                                               ");
            qry.Append($"\n FROM(                                                                             ");
            qry.Append($"\n 	SELECT                                                                        ");
            qry.Append($"\n 	*                                                                             ");
            qry.Append($"\n 	FROM t_purchase_price AS a                                                    ");
            qry.Append($"\n 	WHERE updatetime = (                                                          ");
            qry.Append($"\n 		SELECT MAX(updatetime) FROM (                                             ");
            qry.Append($"\n 			SELECT product, origin, sizes, unit, MAX(updatetime) AS updatetime    ");
            qry.Append($"\n 			FROM t_purchase_price                                                 ");
            qry.Append($"\n 			GROUP BY product, origin, sizes, unit                                 ");
            qry.Append($"\n 		) AS b                                                                    ");
            qry.Append($"\n 		WHERE a.product = b.product                                               ");
            qry.Append($"\n 		  AND a.origin = b.origin                                                 ");
            qry.Append($"\n 		  AND a.sizes = b.sizes                                                   ");
            qry.Append($"\n 		  AND a.unit = b.unit                                                     ");
            qry.Append($"\n 	)                                                                             ");
            qry.Append($"\n ) AS p                                                                            ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                                                    ");
            qry.Append($"\n   ON p.cid = c.id                                                                 ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCurrentUpdatetime()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                     ");
            qry.Append($"\n   product                                  ");
            qry.Append($"\n , origin                                   ");
            qry.Append($"\n , sizes                                    ");
            qry.Append($"\n , unit                                     ");
            qry.Append($"\n , MAX(updatetime) AS updatetime            ");
            qry.Append($"\n FROM t_purchase_price                      ");
            qry.Append($"\n GROUP BY product, origin, sizes, unit      ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCurrentPurchasePrice(string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n    SELECT                                                                                                                                                                           ");
            qry.Append($"\n    t1.product                                                                                                                                                                       ");
            qry.Append($"\n  , t1.origin                                                                                                                                                                        ");
            qry.Append($"\n  , t1.sizes                                                                                                                                                                         ");
            qry.Append($"\n  , t1.unit                                                                                                                                                                          ");
            qry.Append($"\n  , CASE WHEN IF(t2.cost_unit IS NULL OR t2.cost_unit = '', 0, t2.cost_unit) > 0 THEN CONVERT(ROUND(t1.unit / t2.cost_unit, 3), double)                                              ");
            qry.Append($"\n 		 ELSE t1.unit END AS unit2                                                                                                                                                  ");
            qry.Append($"\n  , IF(t2.cost_unit IS NULL OR t2.cost_unit = '', 0, t2.cost_unit) AS cost_unit                                                                                                      ");
            qry.Append($"\n  , IFNULL(t2.custom, 0) AS custom                                                                                                                                                   ");
            qry.Append($"\n  , IFNULL(t2.incidental_expense, 0) AS incidental_expense                                                                                                                           ");
            qry.Append($"\n  , IFNULL(t2.tax, 0) AS tax                                                                                                                                                         ");
            qry.Append($"\n  , IFNULL(t1.purchase_price, 0) AS purchase_price                                                                                                                                   ");
            qry.Append($"\n  , IFNULL(t1.fixed_tariff, 0) AS fixed_tariff                                                                                                                                   ");
            qry.Append($"\n  , t1.updatetime                                                                                                                                                                    ");
            qry.Append($"\n  , t1.company                                                                                                                                                                       ");
            qry.Append($"\n  , t2.manager                                                                                                                                                                       ");
            qry.Append($"\n  , IF(IFNULL(t2.weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate                                                                                                      ");
            qry.Append($"\n  , IF(IFNULL(t2.tray_calculate, 0) = 1, 'TRUE', 'FALSE') AS tray_calculate                                                                                                          ");
            qry.Append($"\n  FROM (                                                                                                                                                                             ");
            qry.Append($"\n  	SELECT                                                                                                                                                                          ");
            qry.Append($"\n   	  p.cid                                                                                                                                                                         ");
            qry.Append($"\n  	, p.product                                                                                                                                                                     ");
            qry.Append($"\n  	, p.origin                                                                                                                                                                      ");
            qry.Append($"\n  	, p.sizes                                                                                                                                                                       ");
            qry.Append($"\n  	, p.unit                                                                                                                                                                        ");
            qry.Append($"\n  	, p.purchase_price                                                                                                                                                              ");
            qry.Append($"\n  	, p.updatetime                                                                                                                                                                  ");
            qry.Append($"\n  	, p.fixed_tariff                                                                                                                                                                  ");
            qry.Append($"\n   	, c.name AS company                                                                                                                                                             ");
            qry.Append($"\n   	FROM(                                                                                                                                                                           ");
            qry.Append($"\n		SELECT                                                                                                                                                                          ");
            qry.Append($"\n          t1.*                                                                                                                                                                       ");
            qry.Append($"\n        FROM (                                                                                                                                                                       ");
            qry.Append($"\n			SELECT                                                                                                                                                                      ");
            qry.Append($"\n			  t1.cid                                                                                                                                                                    ");
            qry.Append($"\n			, t1.product                                                                                                                                                                ");
            qry.Append($"\n			, t1.origin                                                                                                                                                                 ");
            qry.Append($"\n			, t1.sizes                                                                                                                                                                  ");
            qry.Append($"\n			, t1.unit                                                                                                                                                                   ");
            qry.Append($"\n			, t1.purchase_price                                                                                                                                                         ");
            qry.Append($"\n			, t1.edit_user                                                                                                                                                              ");
            qry.Append($"\n			, t1.updatetime                                                                                                                                                             ");
            qry.Append($"\n			, t1.fixed_tariff                                                                                                                                                             ");
            qry.Append($"\n			, CASE WHEN @grp1 = t1.product AND @grp2 = t1.origin AND @grp3 = t1.sizes AND @grp4 = t1.unit THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum                   ");
            qry.Append($"\n			, (@grp1:= t1.product) AS dum1                                                                                                                                              ");
            qry.Append($"\n			, (@grp2:= t1.origin) AS dum2                                                                                                                                               ");
            qry.Append($"\n			, (@grp3:= t1.sizes) as dum3                                                                                                                                                ");
            qry.Append($"\n			, (@grp4:= t1.unit) AS dum4                                                                                                                                                 ");
            qry.Append($"\n			FROM t_purchase_price AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='', @grp3:='', @grp4:='') AS r                                                                           ");
            qry.Append($"\n            WHERE t1.purchase_price > 0                                                                                                                                              ");
            qry.Append($"\n			ORDER BY t1.product, t1.origin, t1.sizes, t1.unit, t1.updatetime DESC, t1.purchase_price                                                                                                        ");
            qry.Append($"\n        ) AS t1                                                                                                                                                                      ");
            qry.Append($"\n        WHERE t1.rownum <= 10                                                                                                                                                        ");
            qry.Append($"\n          AND t1.updatetime >=                                                                                                                                                       ");
            qry.Append($"\n          (                                                                                                                                                                          ");
            qry.Append($"\n			    SELECT                                                                                                                                                                  ");
            qry.Append($"\n				  date_add(MAX(t.updatetime), INTERVAL -10 DAY)                                                                                                                         ");
            qry.Append($"\n				FROM (                                                                                                                                                                  ");
            qry.Append($"\n					SELECT                                                                                                                                                              ");
            qry.Append($"\n					  t1.cid                                                                                                                                                            ");
            qry.Append($"\n					, t1.product                                                                                                                                                        ");
            qry.Append($"\n					, t1.origin                                                                                                                                                         ");
            qry.Append($"\n					, t1.sizes                                                                                                                                                          ");
            qry.Append($"\n					, t1.unit                                                                                                                                                           ");
            qry.Append($"\n					, t1.purchase_price                                                                                                                                                 ");
            qry.Append($"\n					, t1.edit_user                                                                                                                                                      ");
            qry.Append($"\n					, t1.updatetime                                                                                                                                                     ");
            qry.Append($"\n					, CASE WHEN @grp1 = t1.product AND @grp2 = t1.origin AND @grp3 = t1.sizes AND @grp4 = t1.unit THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum           ");
            qry.Append($"\n					, (@grp1:= t1.product) AS dum1                                                                                                                                      ");
            qry.Append($"\n					, (@grp2:= t1.origin) AS dum2                                                                                                                                       ");
            qry.Append($"\n					, (@grp3:= t1.sizes) as dum3                                                                                                                                        ");
            qry.Append($"\n					, (@grp4:= t1.unit) AS dum4                                                                                                                                         ");
            qry.Append($"\n					FROM t_purchase_price AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='', @grp3:='', @grp4:='') AS r                                                                   ");
            qry.Append($"\n					WHERE t1.purchase_price > 0                                                                                                                                         ");
            qry.Append($"\n					ORDER BY t1.product, t1.origin, t1.sizes, t1.unit, t1.updatetime DESC, t1.purchase_price                                                                                                ");
            qry.Append($"\n				) AS t                                                                                                                                                                  ");
            qry.Append($"\n				WHERE t.rownum <= 10                                                                                                                                                    ");
            qry.Append($"\n                  AND t1.product = t.product                                                                                                                                         ");
            qry.Append($"\n                  AND t1.origin = t.origin                                                                                                                                           ");
            qry.Append($"\n                  AND t1.sizes = t.sizes                                                                                                                                             ");
            qry.Append($"\n                  AND t1.unit = t.unit                                                                                                                                               ");
            qry.Append($"\n          )                                                                                                                                                                          ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n 		  AND product LIKE '%{product}%'                                        ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n 		  AND origin LIKE '%{origin}%'                                        ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n 		  AND sizes LIKE '%{sizes}%'                                        ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n 		  AND unit LIKE '%{unit}%'                                        ");
            qry.Append($"\n	) AS p                                                                                                                                                                              ");
            qry.Append($"\n   	LEFT OUTER JOIN t_company AS c                                                                                                                                                  ");
            qry.Append($"\n   	ON p.cid = c.id                                                                                                                                                                 ");
            qry.Append($"\n  ) AS t1                                                                                                                                                                            ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                                                                                  ");
            qry.Append($"\n	SELECT                                                                                                                                                                              ");
            qry.Append($"\n      product                                                                                                                                                                        ");
            qry.Append($"\n    , origin                                                                                                                                                                         ");
            qry.Append($"\n    , sizes                                                                                                                                                                          ");
            qry.Append($"\n    , unit                                                                                                                                                                           ");
            qry.Append($"\n    , cost_unit                                                                                                                                                                      ");
            qry.Append($"\n    , custom                                                                                                                                                                         ");
            qry.Append($"\n    , tax                                                                                                                                                                            ");
            qry.Append($"\n    , incidental_expense                                                                                                                                                             ");
            qry.Append($"\n    , manager                                                                                                                                                                        ");
            qry.Append($"\n    , weight_calculate                                                                                                                                                               ");
            qry.Append($"\n    , tray_calculate                                                                                                                                                                 ");
            qry.Append($"\n	FROM t_product_other_cost                                                                                                                                                           ");
            qry.Append($"\n    GROUP BY product    , origin    , sizes    , unit    , cost_unit    , custom    , tax    , incidental_expense    , manager    , weight_calculate    , tray_calculate             ");
            qry.Append($"\n  ) AS t2                                                                                                                                                                            ");
            qry.Append($"\n    ON t1.product = t2.product                                                                                                                                                       ");
            qry.Append($"\n    AND t1.origin = t2.origin                                                                                                                                                        ");
            qry.Append($"\n    AND t1.sizes = t2.sizes                                                                                                                                                          ");
            qry.Append($"\n    AND t1.unit = t2.unit                                                                                                                                                            ");
            qry.Append($"\n  ORDER BY t1.product, t1.origin,t1.sizes,t1.unit,t1.purchase_price                                                                                                                  ");




            /*qry.Append($"\n  SELECT                                                                                                                                                                       ");
            qry.Append($"\n    t1.product                                                                                                                                                                 ");
            qry.Append($"\n  , t1.origin                                                                                                                                                                  ");
            qry.Append($"\n  , t1.sizes                                                                                                                                                                   ");
            qry.Append($"\n  , t1.unit                                                                                                                                                                    ");
            qry.Append($"\n  , CASE WHEN IF(t2.cost_unit IS NULL OR t2.cost_unit = '', 0, t2.cost_unit) > 0 THEN CONVERT(ROUND(t1.unit / t2.cost_unit, 3), double)                                        ");
            qry.Append($"\n 		 ELSE t1.unit END AS unit2                                                                                                                                            ");
            qry.Append($"\n  , IF(t2.cost_unit IS NULL OR t2.cost_unit = '', 0, t2.cost_unit) AS cost_unit                                                                                                ");
            qry.Append($"\n  , IFNULL(t2.custom, 0) AS custom                                                                                                                                             ");
            qry.Append($"\n  , IFNULL(t2.incidental_expense, 0) AS incidental_expense                                                                                                                     ");
            qry.Append($"\n  , IFNULL(t2.tax, 0) AS tax                                                                                                                                                   ");
            qry.Append($"\n  , IFNULL(t1.purchase_price, 0) AS purchase_price                                                                                                                             ");
            qry.Append($"\n  , t1.updatetime                                                                                                                                                              ");
            qry.Append($"\n  , t1.company                                                                                                                                                                 ");
            qry.Append($"\n  , t2.manager                                                                                                                                                                 ");
            qry.Append($"\n  , IF(IFNULL(t2.weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate                                                                                                ");
            qry.Append($"\n  , IF(IFNULL(t2.tray_calculate, 0) = 1, 'TRUE', 'FALSE') AS tray_calculate                                                                                                    ");
            qry.Append($"\n  FROM (                                                                                                                                                                       ");
            qry.Append($"\n  	SELECT                                                                                                                                                                    ");
            qry.Append($"\n   	  p.cid                                                                                                                                                                   ");
            qry.Append($"\n  	, p.product                                                                                                                                                               ");
            qry.Append($"\n  	, p.origin                                                                                                                                                                ");
            qry.Append($"\n  	, p.sizes                                                                                                                                                                 ");
            qry.Append($"\n  	, p.unit                                                                                                                                                                  ");
            qry.Append($"\n  	, p.purchase_price                                                                                                                                                        ");
            qry.Append($"\n  	, p.updatetime                                                                                                                                                            ");
            qry.Append($"\n   	, c.name AS company                                                                                                                                                       ");
            qry.Append($"\n   	FROM(                                                                                                                                                                     ");
            qry.Append($"\n  		SELECT                                                                                                                                                                ");
            qry.Append($"\n  		  t1.cid                                                                                                                                                              ");
            qry.Append($"\n  		, t1.product                                                                                                                                                          ");
            qry.Append($"\n  		, t1.origin                                                                                                                                                           ");
            qry.Append($"\n  		, t1.sizes                                                                                                                                                            ");
            qry.Append($"\n  		, t1.unit                                                                                                                                                             ");
            qry.Append($"\n  		, t1.purchase_price                                                                                                                                                   ");
            qry.Append($"\n  		, t1.edit_user                                                                                                                                                        ");
            qry.Append($"\n  		, t1.updatetime                                                                                                                                                       ");
            qry.Append($"\n  		, CASE WHEN @grp1 = t1.product AND @grp2 = t1.origin AND @grp3 = t1.sizes AND @grp4 = t1.unit THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum             ");
            qry.Append($"\n  		, (@grp1:= t1.product) AS dum1                                                                                                                                        ");
            qry.Append($"\n  		, (@grp2:= t1.origin) AS dum2                                                                                                                                         ");
            qry.Append($"\n  		, (@grp3:= t1.sizes) as dum3                                                                                                                                          ");
            qry.Append($"\n  		, (@grp4:= t1.unit) AS dum4                                                                                                                                           ");
            qry.Append($"\n  		FROM t_purchase_price AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='', @grp3:='', @grp4:='') AS r                                                                     ");
            qry.Append($"\n  		WHERE updatetime >= (                                                                                                                                                 ");
            qry.Append($"\n  			SELECT                                                                                                                                                            ");
            qry.Append($"\n  			  date_add(MAX(updatetime), INTERVAL -7 DAY)                                                                                                                      ");
            qry.Append($"\n  			FROM t_purchase_price AS t2                                                                                                                                       ");
            qry.Append($"\n  			WHERE t1.product = t2.product                                                                                                                                     ");
            qry.Append($"\n  			  AND t1.origin = t2.origin                                                                                                                                       ");
            qry.Append($"\n  			  AND t1.sizes = t2.sizes                                                                                                                                         ");
            qry.Append($"\n  			  AND t1.unit = t2.unit                                                                                                                                           ");
            qry.Append($"\n  			GROUP BY t2.product, t2.origin, t2.sizes, t2.unit                                                                                                                 ");
            qry.Append($"\n  			  )                                                                                                                                                               ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n 		  AND product LIKE '%{product}%'                                        ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n 		  AND origin LIKE '%{origin}%'                                        ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n 		  AND sizes LIKE '%{sizes}%'                                        ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n 		  AND unit LIKE '%{unit}%'                                        ");
            qry.Append($"\n  		ORDER BY t1.product, t1.origin, t1.sizes, t1.unit, t1.purchase_price                                                                                                  ");
            qry.Append($"\n   	) AS p                                                                                                                                                                    ");
            qry.Append($"\n   	LEFT OUTER JOIN t_company AS c                                                                                                                                            ");
            qry.Append($"\n   	ON p.cid = c.id                                                                                                                                                           ");
            qry.Append($"\n  ) AS t1                                                                                                                                                                      ");
            qry.Append($"\n  LEFT OUTER JOIN t_product_other_cost AS t2                                                                                                                                   ");
            qry.Append($"\n    ON t1.product = t2.product                                                                                                                                                 ");
            qry.Append($"\n    AND t1.origin = t2.origin                                                                                                                                                  ");
            qry.Append($"\n    AND t1.sizes = t2.sizes                                                                                                                                                    ");
            qry.Append($"\n    AND t1.unit = t2.unit                                                                                                                                                      ");
            qry.Append($"\n  ORDER BY t1.product, t1.origin,t1.sizes,t1.unit,t1.updatetime                                                                                                                                                      ");*/


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetPurhcasePriceAverage(string product, string origin, string sizes, string unit, string sub_product, bool isMerge)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                                      ");
            qry.Append($"\n   updatetime                                                                                                                                                                ");
            qry.Append($"\n , AVG(purchase_price) AS purchase_price                                                                                                                              ");
            qry.Append($"\n , IFNULL(AVG(fixed_tariff), 0) AS fixed_tariff                                                                                                                              ");
            qry.Append($"\n FROM (                                                                                                                                                                      ");
            qry.Append($"\n 	SELECT                                                                                                                                                                  ");
            qry.Append($"\n 	  t1.product                                                                                                                                                            ");
            qry.Append($"\n 	, t1.origin                                                                                                                                                             ");
            qry.Append($"\n 	, t1.sizes                                                                                                                                                              ");
            qry.Append($"\n 	, t1.unit                                                                                                                                                               ");
            qry.Append($"\n 	, t1.purchase_price                                                                                                                                                     ");
            qry.Append($"\n 	, t1.updatetime                                                                                                                                                         ");
            qry.Append($"\n     , t1.fixed_tariff                                                                                                                                                      ");
            qry.Append($"\n 	, CASE WHEN @grp1 = t1.product AND @grp2 = t1.origin AND @grp3 = t1.sizes AND @grp4 = t1.updatetime THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum         ");
            qry.Append($"\n 	, (@grp1:= t1.product) AS dum1                                                                                                                                          ");
            qry.Append($"\n 	, (@grp2:= t1.origin) AS dum2                                                                                                                                           ");
            qry.Append($"\n 	, (@grp3:= t1.sizes) as dum3                                                                                                                                            ");
            qry.Append($"\n 	, (@grp4:= t1.updatetime) AS dum4                                                                                                                                       ");
            qry.Append($"\n 	FROM (                                                                                                                                                                  ");
            qry.Append($"\n 		SELECT                                                                                                                                                              ");
            qry.Append($"\n           product                                                                                                                                                           ");
            qry.Append($"\n 		, origin                                                                                                                                                            ");
            qry.Append($"\n         , sizes                                                                                                                                                             ");
            if (string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n         , unit AS unit                                                                                                                                                              ");
                qry.Append($"\n         , purchase_price AS purchase_price                                                                                                                                                ");
            }
            else
            {
                qry.Append($"\n         , {unit} AS unit                                                                                                                                                              ");
                qry.Append($"\n         , purchase_price / unit * {unit} AS purchase_price                                                                                                                                                ");
            }
            qry.Append($"\n         , DATE_FORMAT(updatetime, '%Y%m') AS updatetime                                                                                                                     ");
            qry.Append($"\n         , fixed_tariff                                                                                                                                                      ");
            qry.Append($"\n         FROM t_purchase_price                                                                                                                                               ");
            qry.Append($"\n 		WHERE 1=1                                                                                                                                                           ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		  AND product = '{product}'                                                                                                                                               ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		  AND origin = '{origin}'                                                                                                                                               ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		  AND sizes = '{sizes}'                                                                                                                                          ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		  AND unit = '{unit}'                                                                                                                                          ");
            }
            else if (!string.IsNullOrEmpty(sub_product))
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            qry.Append($"\n 		  AND ( ");
                            for (int j = 0; j < sub.Length; j++)
                            {
                                if (j == 0)
                                    qry.Append($"\n( ");
                                else
                                    qry.Append($"\n OR ( ");

                                qry.Append($"product = '{sub[0]}' ");
                                qry.Append($"AND origin = '{sub[1]}' ");
                                qry.Append($"AND sizes = '{sub[2]}' ");
                                qry.Append($"AND unit = '{sub[6]}' ");
                                qry.Append($") ");

                            }
                            qry.Append($") ");
                        }
                    }
                }
            }


            qry.Append($"\n           AND purchase_price > 0                                                                                                                                            ");
            qry.Append($"\n     ) AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='', @grp3:='', @grp4:='', @grp5:='') AS r                                                                                ");
            qry.Append($"\n ) AS t1                                                                                                                                                                     ");
            qry.Append($"\n WHERE t1.rownum <= 2                                                                                                                                                        ");
            qry.Append($"\n GROUP BY updatetime DESC                                                                                                                                                        ");
            
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        
        public DataTable GetTradeManaer(string product, string origin, string sizes, string unit, string manager, string operating_period = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                       ");
            qry.Append($"\n   t1.*                                                                                                                                       ");
            qry.Append($"\n , REPLACE(t2.operating_period, '^', '')  AS operating_period                                                                                 ");
            qry.Append($"\n FROM (                                                                                                                                       ");
            qry.Append($"\n SELECT                                                                                                                                       ");
            qry.Append($"\n   t1.*                                                                                                                                       ");
            qry.Append($"\n , IFNULL(t2.delivery_days, 0) AS delivery_days                                                                                               ");
            qry.Append($"\n FROM (                                                                                                                                       ");
            qry.Append($"\n	 SELECT                                                                                                                                      ");
            qry.Append($"\n	   t2.name AS company                                                                                                                        ");
            qry.Append($"\n	 , t1.product                                                                                                                                ");
            qry.Append($"\n	 , t1.origin                                                                                                                                 ");
            qry.Append($"\n	 , t1.sizes                                                                                                                                  ");
            qry.Append($"\n	 , IF(t1.unit = '', 0, t1.unit) AS unit                                                                                                             ");
            qry.Append($"\n	 , CASE WHEN IF(t1.cost_unit IS NULL OR t1.cost_unit = '', 0, t1.cost_unit) > 0 THEN CAST(TRUNCATE(t1.unit / t1.cost_unit, 3) AS double) ELSE t1.unit END AS unit2        ");
            qry.Append($"\n	 , t1.manager                                                                                                                                ");
            qry.Append($"\n	 , t1.cost_unit                                                                                                                              ");
            qry.Append($"\n	 , t1.tax                                                                                                                                    ");
            qry.Append($"\n	 , t1.custom                                                                                                                                 ");
            qry.Append($"\n	 , t1.incidental_expense                                                                                                                     ");
            qry.Append($"\n	 , t1.purchase_price                                                                                                                         ");
            qry.Append($"\n	 , t1.fixed_tariff                                                                                                                         ");
            qry.Append($"\n	 , t1.updatetime                                                                                                                             ");

            qry.Append($"\n	 , IF(t1.weight_calculate = 1, 'TRUE', 'FALSE') AS weight_calculate                                                                          ");
            qry.Append($"\n	 , IF(t1.tray_calculate = 1, 'TRUE', 'FALSE') AS tray_calculate                                                                              ");
            qry.Append($"\n	 , t1.production_days                                                                                                                             ");
            qry.Append($"\n	 , t1.purchase_margin                                                                                                                             ");
            qry.Append($"\n	 , t1.base_around_month                                                                                                                           ");
            qry.Append($"\n	 FROM(                                                                                                                                       ");
            qry.Append($"\n		SELECT                                                                                                                                   ");
            qry.Append($"\n		  t2.cid                                                                                                                                 ");
            qry.Append($"\n		, t1.product                                                                                                                             ");
            qry.Append($"\n		, t1.origin                                                                                                                              ");
            qry.Append($"\n		, t1.sizes                                                                                                                               ");
            qry.Append($"\n		, IF(t1.unit = '', 0, t1.unit) AS unit                                                                                                   ");
            qry.Append($"\n		, t1.manager                                                                                                                             ");
            qry.Append($"\n		, IFNULL(t1.cost_unit, 0) AS cost_unit                                                                                                   ");
            qry.Append($"\n		, IFNULL(t1.tax               , 0) AS tax                                                                                                ");
            qry.Append($"\n		, IFNULL(t1.custom            , 0) AS custom                                                                                             ");
            qry.Append($"\n		, IFNULL(t1.incidental_expense, 0) AS incidental_expense                                                                                 ");
            qry.Append($"\n		, IFNULL(t2.purchase_price, 0) AS purchase_price                                                                                         ");
            qry.Append($"\n		, IFNULL(t2.fixed_tariff, 0) AS fixed_tariff                                                                                             ");
            qry.Append($"\n		, IFNULL(t1.weight_calculate, 1) AS weight_calculate                                                                                     ");
            qry.Append($"\n		, IFNULL(t1.tray_calculate, 0) AS tray_calculate                                                                                         ");
            qry.Append($"\n		, IFNULL(t1.production_days, 20) AS production_days                                                                                         ");
            qry.Append($"\n		, IFNULL(t1.purchase_margin, 5) AS purchase_margin                                                                                         ");
            qry.Append($"\n		, IFNULL(t1.base_around_month, 3) AS base_around_month                                                                                     ");
            qry.Append($"\n		, t2.updatetime                                                                                                                          ");
            qry.Append($"\n		FROM t_product_other_cost AS t1                                                                                                          ");
            qry.Append($"\n		LEFT OUTER JOIN (                                                                                                                        ");
            qry.Append($"\n			SELECT                                                                                                                               ");
            qry.Append($"\n			  cid                                                                                                                                ");
            qry.Append($"\n			, product                                                                                                                            ");
            qry.Append($"\n			, origin                                                                                                                             ");
            qry.Append($"\n			, sizes                                                                                                                              ");
            qry.Append($"\n			, unit                                                                                                                               ");
            qry.Append($"\n			, purchase_price                                                                                                                     ");
            qry.Append($"\n			, fixed_tariff                                                                                                                       ");
            qry.Append($"\n			, updatetime                                                                                                                         ");
            qry.Append($"\n			FROM t_purchase_price                                                                                                                ");
            qry.Append($"\n			WHERE updatetime IN (SELECT MAX(updatetime) FROM t_purchase_price GROUP BY product, origin, sizes, unit)                             ");
            qry.Append($"\n			GROUP BY product, origin, sizes, unit, updatetime                                                                                    ");
            qry.Append($"\n		)  AS t2                                                                                                                                 ");
            qry.Append($"\n		ON t1.product = t2.product                                                                                                               ");
            qry.Append($"\n		AND t1.origin = t2.origin                                                                                                                ");
            qry.Append($"\n		AND t1.sizes = t2.sizes                                                                                                                  ");
            qry.Append($"\n		AND t1.unit = t2.unit                                                                                                                    ");
            qry.Append($"\n		WHERE 1 = 1                                                                                                                              ");
            //qry.Append($"\n		  AND t1.manager IS NOT NULL AND t1.manager <> ''                                                                                        ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("t1.product", product)}                                         ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("t1.origin", origin)}                                          ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("t1.sizes", sizes)}                                          ");
            /*if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   {commonRepository.whereSql("t1.unit", unit)}                                         ");*/
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {commonRepository.whereSql("t1.manager", manager)}                                                             ");
            qry.Append($"\n		GROUP BY t1.product, t1.origin, t1.sizes, t1.unit, t1.manager                                                                            ");
            qry.Append($"\n	 ) AS t1                                                                                                                                     ");
            qry.Append($"\n	 LEFT OUTER JOIN t_company AS t2                                                                                                             ");
            qry.Append($"\n	   ON t1.cid = t2.id                                                                                                                         ");
            qry.Append($"\n ) AS t1                                                                                                                                      ");
            qry.Append($"\n LEFT OUTER JOIN t_country AS t2                                                                                                              ");
            qry.Append($"\n   ON t1.origin = t2.country_name                                                                                                             ");

            qry.Append($"\n ) AS t1                                                                          ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                               ");
            qry.Append($"\n    SELECT                                                                        ");
            qry.Append($"\n    product, origin, group_concat(IF(division = '조업시기' , CONCAT('^', month, '월') , null)) AS operating_period  ");
            qry.Append($"\n    FROM t_product_contract_recommendation                                        ");
            qry.Append($"\n    GROUP BY product, origin                                                      ");
            qry.Append($"\n  ) AS t2                                                                         ");
            qry.Append($"\n    ON t1.product = t2.product                                                    ");
            qry.Append($"\n    AND t1.origin = t2.origin                                                     ");
            qry.Append($"\n WHERE 1=1                                                                        ");
            qry.Append($"\n   AND (REPLACE(IFNULL(t1.unit2, 0), '.', '') REGEXP '[^0123456789]') <> 1        ");
            
            if (!string.IsNullOrEmpty(operating_period))
                qry.Append($"\n   {commonRepository.whereSql("t2.operating_period", '^' + operating_period + "월")}                                                             ");
            qry.Append($"\n ORDER BY CASE WHEN t1.updatetime IS NULL THEN 9999 ELSE t1.updatetime END                                                                    ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetTradeManaerAsOne(string product, string origin, string sizes, string unit, double exchange_rate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                               ");
            qry.Append($"\n    t1.product                                                                                                         ");
            qry.Append($"\n  , t1.origin                                                                                                          ");
            qry.Append($"\n  , t1.sizes                                                                                                           ");
            qry.Append($"\n  , t1.unit                                                                                                            ");
            qry.Append($"\n  , t1.manager                                                                                                         ");
            qry.Append($"\n  , IFNULL(t1.cost_unit, 0) AS cost_unit                                                                               ");
            qry.Append($"\n  , IFNULL(t1.tax               , 0) AS tax                                                                            ");
            qry.Append($"\n  , IFNULL(t1.custom            , 0) AS custom                                                                         ");
            qry.Append($"\n  , IFNULL(t1.incidental_expense, 0) AS incidental_expense                                                             ");
            qry.Append($"\n  , t1.manager                                                                                                         ");
            qry.Append($"\n  , IFNULL(t2.purchase_price, 0) AS purchase_price                                                                     ");
            qry.Append($"\n  , IFNULL(t2.exchange_rate, 0) AS exchange_rate                                                                       ");
            qry.Append($"\n  , t2.updatetime                                                                                                      ");
            qry.Append($"\n  FROM t_product_other_cost AS t1                                                                                      ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                    ");
            qry.Append($"\n 	SELECT                                                                                                            ");
            qry.Append($"\n       t1.*                                                                                                            ");
            qry.Append($"\n 	, t2.exchange_rate                                                                                                ");
            qry.Append($"\n     FROM (                                                                                                            ");
            qry.Append($"\n 		SELECT                                                                                                        ");
            qry.Append($"\n 		  product                                                                                                     ");
            qry.Append($"\n 		, origin                                                                                                      ");
            qry.Append($"\n 		, sizes                                                                                                       ");
            qry.Append($"\n 		, unit                                                                                                        ");
            qry.Append($"\n 		, purchase_price                                                                                              ");
            qry.Append($"\n 		, updatetime                                                                                                  ");
            qry.Append($"\n 		FROM t_purchase_price                                                                                         ");
            qry.Append($"\n 		WHERE updatetime IN (SELECT MAX(updatetime) FROM t_purchase_price GROUP BY product, origin, sizes, unit)      ");
            qry.Append($"\n 		GROUP BY product, origin, sizes, unit, updatetime                                                             ");
            qry.Append($"\n     ) AS t1                                                                                                           ");
            qry.Append($"\n     INNER JOIN t_finance AS t2                                                                                        ");
            qry.Append($"\n       ON t1.updatetime = t2.base_date                                                                                 ");
            qry.Append($"\n  )  AS t2                                                                                                             ");
            qry.Append($"\n    ON t1.product = t2.product                                                                                         ");
            qry.Append($"\n    AND t1.origin = t2.origin                                                                                          ");
            qry.Append($"\n    AND t1.sizes = t2.sizes                                                                                            ");
            qry.Append($"\n    AND t1.unit = t2.unit                                                                                              ");
            qry.Append($"\n  WHERE 1 = 1                                                                                                          ");
            qry.Append($"\n    AND t1.manager IS NOT NULL AND t1.manager <> ''                                                                    ");
            qry.Append($"\n    AND t1.product = '{product}'                                                                    ");
            qry.Append($"\n    AND t1.origin = '{origin}'                                                                    ");
            qry.Append($"\n    AND t1.sizes = '{sizes}'                                                                    ");
            qry.Append($"\n    AND t1.unit = '{unit}'                                                                    ");
            qry.Append($"\n  GROUP BY t1.product, t1.origin, t1.sizes, t1.unit, t1.manager                                                        ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        private string whereSqlExactly(string whrColumn, string whrValue)
        {
            string[] tempStr = null;
            string tempWhr = "";
            string whrStr = "";
            if (!string.IsNullOrEmpty(whrValue.Trim()))
            {
                tempStr = whrValue.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   {whrColumn} = '{tempStr[i]}' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR {whrColumn} = '{tempStr[i]}' ";
                            }
                        }
                    }
                    whrStr = $"\n	 AND ( {tempWhr} )";
                }
                else
                {
                    whrStr = $"\n	  AND {whrColumn} = '{whrValue}'";
                }
            }
            return whrStr;
        }


        public DataTable GetPurchasePriceForChartMonth(string sttdate, string enddate, string product, string origin, string sizes, string unit, string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                  ");
            qry.Append($"\n   SUM(m1) AS m1                                      ");
            qry.Append($"\n , SUM(m2) AS m2                                      ");
            qry.Append($"\n , SUM(m3) AS m3                                      ");
            qry.Append($"\n , SUM(m4) AS m4                                      ");
            qry.Append($"\n , SUM(m5) AS m5                                      ");
            qry.Append($"\n , SUM(m6) AS m6                                      ");
            qry.Append($"\n , SUM(m7) AS m7                                      ");
            qry.Append($"\n , SUM(m8) AS m8                                      ");
            qry.Append($"\n , SUM(m9) AS m9                                      ");
            qry.Append($"\n , SUM(m10) AS m10                                    ");
            qry.Append($"\n , SUM(m11) AS m11                                    ");
            qry.Append($"\n , SUM(m12) AS m12                                    ");
            qry.Append($"\n FROM(                                                ");
            qry.Append($"\n 	SELECT                                           ");
            qry.Append($"\n 	  if(m.m = 1, m.purchase_price, 0) AS m1         ");
            qry.Append($"\n 	, if(m.m = 2, m.purchase_price, 0) AS m2         ");
            qry.Append($"\n 	, if(m.m = 3, m.purchase_price, 0) AS m3         ");
            qry.Append($"\n 	, if(m.m = 4, m.purchase_price, 0) AS m4         ");
            qry.Append($"\n 	, if(m.m = 5, m.purchase_price, 0) AS m5         ");
            qry.Append($"\n 	, if(m.m = 6, m.purchase_price, 0) AS m6         ");
            qry.Append($"\n 	, if(m.m = 7, m.purchase_price, 0) AS m7         ");
            qry.Append($"\n 	, if(m.m = 8, m.purchase_price, 0) AS m8         ");
            qry.Append($"\n 	, if(m.m = 9, m.purchase_price, 0) AS m9         ");
            qry.Append($"\n 	, if(m.m = 10, m.purchase_price, 0) AS m10       ");
            qry.Append($"\n 	, if(m.m = 11, m.purchase_price, 0) AS m11       ");
            qry.Append($"\n 	, if(m.m = 12, m.purchase_price, 0) AS m12       ");
            qry.Append($"\n 	FROM(                                            ");
            qry.Append($"\n 		SELECT                                       ");
            qry.Append($"\n 		   AVG(p.purchase_price) AS purchase_price   ");
            qry.Append($"\n 		 , MONTH(p.updatetime) AS m                  ");
            qry.Append($"\n 		 FROM t_purchase_price AS p                  ");
            qry.Append($"\n 		 WHERE 1=1                                   ");
            qry.Append($"\n            AND p.updatetime >= '{sttdate}'       ");
            qry.Append($"\n            AND p.updatetime <= '{enddate}'       ");
            qry.Append($"\n            AND p.product = '{product}'           ");
            qry.Append($"\n            AND p.origin = '{origin}'             ");
            qry.Append($"\n            AND p.sizes = '{sizes}'               ");
            qry.Append($"\n            AND p.unit = '{unit}'                 ");
            qry.Append($"\n 		 GROUP BY MONTH(p.updatetime)                ");
            qry.Append($"\n 	) AS m                                           ");
            qry.Append($"\n ) AS m                                               ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCostAccounting(string sdate, string edate, string product, string origin, string sizes, string unit, string company, bool exactly, string manager, int sortType = 1, bool isExactly = false, bool isCurrent = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                      ");
            qry.Append($"\n    c.name AS cname                                                                                           ");
            qry.Append($"\n  , c.id AS cid                                                                                               ");
            qry.Append($"\n  , p.id                                                                                                      ");
            qry.Append($"\n  , p.product                                                                                                 ");
            qry.Append($"\n  , p.origin                                                                                                  ");
            qry.Append($"\n  , p.sizes                                                                                                   ");
            qry.Append($"\n  , p.sizes2                                                                                                  ");
            qry.Append($"\n  , p.unit                                                                                                    ");
            qry.Append($"\n  , p.cost_unit                                                                                               ");
            qry.Append($"\n  , p.purchase_price                                                                                          ");
            qry.Append($"\n  , p.custom                                                                                                  ");
            qry.Append($"\n  , p.tax                                                                                                     ");
            qry.Append($"\n  , p.incidental_expense                                                                                      ");
            qry.Append($"\n  , p.updatetime                                                                                              ");
            qry.Append($"\n  , p.manager                                                                                                 ");
            qry.Append($"\n  , IFNULL(p.weight_calculate, 'TRUE') AS weight_calculate                                                    ");
            qry.Append($"\n  , IFNULL(p.is_FOB , 'FALSE') AS is_FOB                                                                      ");
            qry.Append($"\n  , IFNULL(p.production_days, 20) AS production_days                                                          ");
            qry.Append($"\n  , IFNULL(p.purchase_margin, 7) AS purchase_margin                                                           ");
            qry.Append($"\n  , IFNULL(p.fixed_tariff, 0) AS fixed_tariff                                                                 ");
            qry.Append($"\n  FROM(                                                                                                       ");
            qry.Append($"\n 	 SELECT                                                                                                  ");
            qry.Append($"\n 		p.id                                                                                                 ");
            qry.Append($"\n 	  , p.cid                                                                                                ");
            qry.Append($"\n 	  , p.product                                                                                            ");
            qry.Append($"\n 	  , p.origin                                                                                             ");
            qry.Append($"\n 	  , p.sizes                                                                                              ");
            qry.Append($"\n 	  , p.sizes2                                                                                             ");
            qry.Append($"\n 	  , p.unit                                                                                               ");
            qry.Append($"\n 	  , IF(IFNULL(p.cost_unit, 0) = 0, IFNULL(o.cost_unit, 0), p.cost_unit)  AS cost_unit                                                                 ");
            qry.Append($"\n 	  , p.purchase_price                                                                                     ");
            qry.Append($"\n 	  , CAST(IFNULL(o.custom, 0) AS CHAR) AS custom                                                                        ");
            qry.Append($"\n 	  , CAST(IFNULL(o.tax, 0) AS CHAR) AS tax                                                                              ");
            qry.Append($"\n 	  , CAST(IFNULL(o.incidental_expense, 0) AS CHAR) AS incidental_expense                                                ");
            qry.Append($"\n 	  , p.updatetime                                                                                         ");
            qry.Append($"\n 	  , o.manager                                                                                            ");
            qry.Append($"\n 	  , IF(IFNULL(p.weight_calculate, IFNULL(o.weight_calculate, 0)) = 1, 'TRUE', 'FALSE') AS weight_calculate           ");
            qry.Append($"\n 	  , IF(IFNULL(p.is_FOB, 0) = 1, 'TRUE', 'FALSE') AS is_FOB                                               ");
            qry.Append($"\n 	  , o.production_days                                                                                    ");
            qry.Append($"\n 	  , o.purchase_margin                                                                                    ");
            qry.Append($"\n 	  , p.fixed_tariff                                                                                       ");
            qry.Append($"\n 	  FROM                                                                                                   ");
            qry.Append($"\n 	  (                                                                                                      ");
            qry.Append($"\n 		SELECT                                                                                               ");
            qry.Append($"\n 		  id                                                                                                 ");
            qry.Append($"\n 		, cid                                                                                                ");
            qry.Append($"\n 		, product                                                                                            ");
            qry.Append($"\n 		, origin                                                                                             ");
            qry.Append($"\n 		, sizes                                                                                              ");
            qry.Append($"\n 		, CASE                                                                                               ");
            qry.Append($"\n 			WHEN INSTR(sizes, '/') > 1 THEN SUBSTRING_INDEX(sizes, '/', 1)                                   ");
            qry.Append($"\n 			 WHEN INSTR(sizes, '미') > 1 THEN SUBSTRING_INDEX(sizes, '미', 1)                                ");
            qry.Append($"\n 			 WHEN INSTR(sizes, 'kg') > 1 THEN SUBSTRING_INDEX(sizes, 'kg', 1)                                ");
            qry.Append($"\n 			 WHEN INSTR(sizes, 'g') > 1 THEN SUBSTRING_INDEX(sizes, 'g', 1)                                  ");
            qry.Append($"\n 			 WHEN INSTR(sizes, 'l') > 1 THEN SUBSTRING_INDEX(sizes, 'l', 1)                                  ");
            qry.Append($"\n 			 WHEN INSTR(sizes, 'up') > 1 THEN SUBSTRING_INDEX(sizes, 'up', 1)                                ");
            qry.Append($"\n 			 END AS sizes2                                                                                   ");
            qry.Append($"\n 		, unit                                                                                               ");
            qry.Append($"\n 		, cost_unit                                                                                          ");
            qry.Append($"\n 		, purchase_price                                                                                     ");
            qry.Append($"\n 		, weight_calculate                                                                                   ");
            qry.Append($"\n 		, is_FOB                                                                                             ");
            qry.Append($"\n 		, MAX(updatetime) AS updatetime                                                                      ");
            qry.Append($"\n 		, fixed_tariff                                                                                       ");

            if (!isCurrent)
            {
                qry.Append($"\n 		FROM t_purchase_price                                                                                ");
                qry.Append($"\n 		WHERE 1=1                                                                                            ");
                if (!string.IsNullOrEmpty(sdate))
                    qry.Append($"\n 		  AND updatetime >= '{sdate}'                                                                     ");
                if (!string.IsNullOrEmpty(edate))
                    qry.Append($"\n 		  AND updatetime <= '{edate}'                                                                     ");


                if (isExactly)
                {
                    if (!string.IsNullOrEmpty(product.Trim()))
                        qry.Append($"\n 		  AND product = '{product}'                                                                     ");
                    if (!string.IsNullOrEmpty(origin.Trim()))
                        qry.Append($"\n 		  AND origin = '{origin}'                                                                     ");
                    if (!string.IsNullOrEmpty(sizes.Trim()))
                        qry.Append($"\n 		  AND sizes = '{sizes}'                                                                     ");
                    if (!string.IsNullOrEmpty(unit.Trim()))
                        qry.Append($"\n 		  AND unit = '{unit}'                                                                     ");
                }
                else
                {
                    if (!string.IsNullOrEmpty(product.Trim()))
                        qry.Append($"\n 		  {commonRepository.whereSql("product", product)}                                                                     ");
                    if (!string.IsNullOrEmpty(origin.Trim()))
                        qry.Append($"\n 		  {commonRepository.whereSql("origin", origin)}                                                                     ");
                    if (!string.IsNullOrEmpty(sizes.Trim()))
                        qry.Append($"\n 		  {commonRepository.whereSql("sizes", sizes)}                                                                     ");
                    if (!string.IsNullOrEmpty(unit.Trim()))
                        qry.Append($"\n 		  {commonRepository.whereSql("unit", unit)}                                                                     ");
                }
                qry.Append($"\n 		GROUP BY id, cid, product, origin, sizes, unit, purchase_price                                           ");
            }
            else
            {
                qry.Append($"\n 		FROM (                                                                                                                                                                                                                                                                                                 ");
                qry.Append($"\n 			SELECT                                                                                                                                                                                                                                                                                             ");
                qry.Append($"\n 			  t1.*                                                                                                                                                                                                                                                                                             ");
                qry.Append($"\n 			, CASE WHEN @grp1 = t1.product AND @grp2 = t1.origin AND @grp3 = t1.sizes AND @grp4 = t1.unit THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum                                                                                                                                          ");
                qry.Append($"\n 			, (@grp1:= t1.product) AS dum1                                                                                                                                                                                                                                                                     ");
                qry.Append($"\n 			, (@grp2:= t1.origin) AS dum2                                                                                                                                                                                                                                                                      ");
                qry.Append($"\n 			, (@grp3:= t1.sizes) as dum3                                                                                                                                                                                                                                                                       ");
                qry.Append($"\n 			, (@grp4:= t1.unit) AS dum4                                                                                                                                                                                                                                                                        ");
                qry.Append($"\n 			FROM t_purchase_price AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='', @grp3:='', @grp4:='') AS r                                                                                                                                                                                                  ");
                qry.Append($"\n 			WHERE t1.purchase_price > 0                                                                                                                                                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sdate))
                    qry.Append($"\n 		  AND updatetime >= '{sdate}'                                                                     ");
                if (!string.IsNullOrEmpty(edate))
                    qry.Append($"\n 		  AND updatetime <= '{edate}'                                                                     ");


                if (isExactly)
                {
                    if (!string.IsNullOrEmpty(product.Trim()))
                        qry.Append($"\n 		  AND product = '{product}'                                                                     ");
                    if (!string.IsNullOrEmpty(origin.Trim()))
                        qry.Append($"\n 		  AND origin = '{origin}'                                                                     ");
                    if (!string.IsNullOrEmpty(sizes.Trim()))
                        qry.Append($"\n 		  AND sizes = '{sizes}'                                                                     ");
                    if (!string.IsNullOrEmpty(unit.Trim()))
                        qry.Append($"\n 		  AND unit = '{unit}'                                                                     ");
                }
                else
                {
                    if (!string.IsNullOrEmpty(product.Trim()))
                        qry.Append($"\n 		  {commonRepository.whereSql("product", product)}                                                                     ");
                    if (!string.IsNullOrEmpty(origin.Trim()))
                        qry.Append($"\n 		  {commonRepository.whereSql("origin", origin)}                                                                     ");
                    if (!string.IsNullOrEmpty(sizes.Trim()))
                        qry.Append($"\n 		  {commonRepository.whereSql("sizes", sizes)}                                                                     ");
                    if (!string.IsNullOrEmpty(unit.Trim()))
                        qry.Append($"\n 		  {commonRepository.whereSql("unit", unit)}                                                                     ");
                }
                qry.Append($"\n 			ORDER BY t1.product, t1.origin, t1.sizes, t1.unit, t1.updatetime DESC, t1.purchase_price                                                                                                                                                                                                           ");
                qry.Append($"\n 		) AS t                                                                                                                                                                                                                                                                                                 ");
                qry.Append($"\n 		WHERE t.rownum = 1                                                                                                                                                                                                                                                                                     ");
            }

            qry.Append($"\n 	  ) AS p                                                                                                                      ");
            qry.Append($"\n 	  LEFT OUTER JOiN  (                                                                                                          ");
            qry.Append($"\n		   SELECT                                                                                                                     ");
            qry.Append($"\n          product                                                                                                                  ");
            qry.Append($"\n        , origin                                                                                                                   ");
            qry.Append($"\n        , sizes                                                                                                                    ");
            qry.Append($"\n        , unit                                                                                                                     ");
            qry.Append($"\n        , IF(IFNULL(cost_unit, '') = '', 0, cost_unit) AS cost_unit                                                                ");
            qry.Append($"\n        , CAST(MAX(custom) AS CHAR) AS custom                                                                                      ");
            qry.Append($"\n        , CAST(MAX(tax) AS CHAR) AS tax                                                                                            ");
            qry.Append($"\n        , CAST(MAX(incidental_expense) AS CHAR) AS incidental_expense                                                              ");
            qry.Append($"\n        , manager                                                                                                                  ");
            qry.Append($"\n        , weight_calculate                                                                                                         ");
            qry.Append($"\n        , tray_calculate                                                                                                           ");
            qry.Append($"\n        , MAX(production_days) AS production_days                                                                                  ");
            qry.Append($"\n        , MAX(purchase_margin) AS purchase_margin                                                                                  ");
            qry.Append($"\n        FROM t_product_other_cost                                                                                                  ");
            qry.Append($"\n        WHERE 1=1                                                                                                                  ");
            if (isExactly)
            {
                if (!string.IsNullOrEmpty(product.Trim()))
                    qry.Append($"\n 		  AND product = '{product}'                                                                     ");
                if (!string.IsNullOrEmpty(origin.Trim()))
                    qry.Append($"\n 		  AND origin = '{origin}'                                                                     ");
                if (!string.IsNullOrEmpty(sizes.Trim()))
                    qry.Append($"\n 		  AND sizes = '{sizes}'                                                                     ");
                if (!string.IsNullOrEmpty(unit.Trim()))
                    qry.Append($"\n 		  AND unit = '{unit}'                                                                     ");
            }
            else
            {
                if (!string.IsNullOrEmpty(product.Trim()))
                    qry.Append($"\n 		  {commonRepository.whereSql("product", product)}                                                                     ");
                if (!string.IsNullOrEmpty(origin.Trim()))
                    qry.Append($"\n 		  {commonRepository.whereSql("origin", origin)}                                                                     ");
                if (!string.IsNullOrEmpty(sizes.Trim()))
                    qry.Append($"\n 		  {commonRepository.whereSql("sizes", sizes)}                                                                     ");
                if (!string.IsNullOrEmpty(unit.Trim()))
                    qry.Append($"\n 		  {commonRepository.whereSql("unit", unit)}                                                                     ");
            }
            qry.Append($"\n        GROUP BY product, origin, sizes, unit, IF(IFNULL(cost_unit, '') = '', 0, cost_unit), manager, weight_calculate, tray_calculate     ");
            qry.Append($"\n 	  ) AS o                                                                                     ");
            qry.Append($"\n 		 ON p.product = o.product                                                                            ");
            qry.Append($"\n 		AND p.origin = o.origin                                                                              ");
            qry.Append($"\n 		AND p.sizes = o.sizes                                                                                ");
            qry.Append($"\n 		AND p.unit = o.unit                                                                                  ");
            qry.Append($"\n  ) AS p                                                                                                      ");
            qry.Append($"\n  LEFT OUTER JOIN t_company AS c                                                                              ");
            qry.Append($"\n    ON p.cid = c.id                                                                                           ");
            qry.Append($"\n  WHERE 1=1                                                                                                   ");
            if (!(company == null || string.IsNullOrEmpty(company)))
            {
                if (!exactly)
                    qry.Append($"\n	  {commonRepository.whereSql("c.name", company)}                                                                         ");
                else
                    qry.Append($"\n	  AND c.name LIKE '%{company}%'                                                                         ");
            }
            if (!(manager == null || string.IsNullOrEmpty(manager)))
                qry.Append($"\n	  {commonRepository.whereSql("p.manager", manager)}                                                                         ");

            qry.Append($"\n   GROUP BY c.name, c.id, p.id, p.product, p.origin, p.sizes, p.sizes2, p.unit  , p.cost_unit  , p.purchase_price  , p.custom  , p.tax  , p.incidental_expense  , p.updatetime , IFNULL(p.weight_calculate, 'TRUE') , IFNULL(p.production_days, 20) , IFNULL(p.purchase_margin, 7)  ");


            //정렬방식
            if (sortType == 1)
                qry.Append($"\n   ORDER BY product, origin, sizes2 + 0, sizes, c.name                                       ");
            else if (sortType == 2)
                qry.Append($"\n   ORDER BY c.name, product, origin, sizes2 + 0, sizes                                       ");
            else if (sortType == 3)
                qry.Append($"\n   ORDER BY product, origin, sizes2 + 0, sizes, purchase_price                               ");
            else if (sortType == 4)
                qry.Append($"\n   ORDER BY purchase_price, product, origin, sizes2 + 0, sizes                               ");
            else if (sortType == 5)
                qry.Append($"\n   ORDER BY product, origin, sizes2 + 0, sizes, updatetime                                   ");
            else if (sortType == 6)
                qry.Append($"\n   ORDER BY updatetime, product, origin, sizes2 + 0, sizes                                   ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCostAccounting2(string sdate, string edate, string product, string origin, string sizes, string unit, string company)
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n SELECT                                                                ");
            qry.Append($"\n   c.name AS cname                                                     ");
            qry.Append($"\n , p.product                                                           ");
            qry.Append($"\n , p.origin                                                            ");
            qry.Append($"\n , p.sizes                                                             ");
            qry.Append($"\n , p.sizes2                                                            ");
            qry.Append($"\n , p.unit                                                              ");
            qry.Append($"\n , p.cost_unit                                                         ");
            qry.Append($"\n , p.purchase_price                                                    ");
            qry.Append($"\n , p.custom                                                            ");
            qry.Append($"\n , p.tax                                                               ");
            qry.Append($"\n , p.incidental_expense                                                ");
            qry.Append($"\n , IFNULL(p.weight_calculate, 'TRUE') AS weight_calculate              ");
            qry.Append($"\n , IFNULL(p.tray_calculate , 'FALSE') AS tray_calculate                ");
            qry.Append($"\n , p.updatetime                                                        ");
            qry.Append($"\n , IF(IFNULL(p.is_FOB, 0) = 1, 'TRUE', 'FALSE') AS is_FOB              ");
            qry.Append($"\n FROM(                                                                 ");
            qry.Append($"\n 	 SELECT                                                           ");
            qry.Append($"\n 		p.cid                                                         ");
            qry.Append($"\n 	  , p.product                                                     ");
            qry.Append($"\n 	  , p.origin                                                      ");
            qry.Append($"\n 	  , p.sizes                                                       ");
            qry.Append($"\n 	  , p.sizes2                                                      ");
            qry.Append($"\n 	  , p.unit                                                        ");
            qry.Append($"\n 	  , IFNULL(o.cost_unit, '') AS cost_unit                          ");
            qry.Append($"\n 	  , p.purchase_price                                              ");
            qry.Append($"\n 	  , IFNULL(o.custom, 0) AS custom                                 ");
            qry.Append($"\n 	  , IFNULL(o.tax, 0) AS tax                                       ");
            qry.Append($"\n 	  , IFNULL(o.incidental_expense, 0) AS incidental_expense         ");
            qry.Append($"\n 	  , p.updatetime                                                  ");

            qry.Append($"\n 	  , IF(IFNULL(p.weight_calculate, o.weight_calculate) = 1, 'TRUE', 'FALSE') AS weight_calculate                ");
            qry.Append($"\n 	  , IF(IFNULL(p.weight_calculate, o.weight_calculate) = 0, 'TRUE', 'FALSE') AS tray_calculate                  ");
            qry.Append($"\n 	  , is_FOB                                                        ");
            qry.Append($"\n 	  FROM                                                            ");
            qry.Append($"\n 	  (                                                               ");
            qry.Append($"\n 		SELECT                                                        ");
            qry.Append($"\n 		  cid                                                         ");
            qry.Append($"\n 		, product                                                     ");
            qry.Append($"\n 		, origin                                                      ");
            qry.Append($"\n 		, sizes                                                       ");
            qry.Append($"\n 		, CASE                                                        ");
            qry.Append($"\n 		    WHEN INSTR(sizes, '/') > 1 THEN SUBSTRING_INDEX(sizes, '/', 1)       ");
            qry.Append($"\n             WHEN INSTR(sizes, '미') > 1 THEN SUBSTRING_INDEX(sizes, '미', 1)     ");
            qry.Append($"\n             WHEN INSTR(sizes, 'kg') > 1 THEN SUBSTRING_INDEX(sizes, 'kg', 1)     ");
            qry.Append($"\n             WHEN INSTR(sizes, 'g') > 1 THEN SUBSTRING_INDEX(sizes, 'g', 1)       ");
            qry.Append($"\n             WHEN INSTR(sizes, 'l') > 1 THEN SUBSTRING_INDEX(sizes, 'l', 1)       ");
            qry.Append($"\n             WHEN INSTR(sizes, 'up') > 1 THEN SUBSTRING_INDEX(sizes, 'up', 1)     ");
            qry.Append($"\n             END AS sizes2                                                            ");
            qry.Append($"\n 		, unit                                                        ");
            qry.Append($"\n 		, purchase_price                                              ");
            qry.Append($"\n 		, MAX(updatetime) AS updatetime                               ");
            qry.Append($"\n 		, IFNULL(weight_calculate, 1) AS weight_calculate             ");
            qry.Append($"\n 		, is_FOB                                                      ");
            qry.Append($"\n 		FROM t_purchase_price                                         ");
            qry.Append($"\n 		WHERE 1=1                                                     ");
            if (!string.IsNullOrEmpty(sdate))
                qry.Append($"\n 		  AND updatetime >= '{sdate}'                                    ");
            if (!string.IsNullOrEmpty(edate))
                qry.Append($"\n 		  AND updatetime <= '{edate}'                                    ");
            qry.Append($"\n 		  AND updatetime <> '0000-00-00 00:00:00'                                    ");
            qry.Append($"\n 	    GROUP BY cid, product, origin, sizes, unit, purchase_price, weight_calculate, is_FOB         ");
            qry.Append($"\n 	  ) AS p                                                          ");
            qry.Append($"\n 	  LEFT OUTER JOiN  t_product_other_cost AS o                      ");
            qry.Append($"\n 		 ON p.product = o.product                                     ");
            qry.Append($"\n 		AND p.origin = o.origin                                       ");
            qry.Append($"\n 		AND p.sizes = o.sizes                                         ");
            qry.Append($"\n 		AND p.unit = o.unit                                           ");
            qry.Append($"\n ) AS p                                                                ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                                        ");
            qry.Append($"\n   ON p.cid = c.id                                                     ");
            qry.Append($"\n WHERE 1=1                                                             ");
            string[] tempStr = null;
            string tempWhr = "";
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = product.Split('^');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        { 
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   p.product  = '{tempStr[i]}' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR p.product = '{tempStr[i]}' ";
                            }
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )   ");
                }
                else
                {
                    qry.Append($"\n	  AND p.product = '{product}'            ");
                }
            }
            if (!(origin == null || string.IsNullOrEmpty(origin)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = origin.Split('^');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   p.origin  = '{tempStr[i]}' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR p.origin = '{tempStr[i]}' ";
                            }
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )   ");
                }
                else
                {
                    qry.Append($"\n	  AND p.origin = '{origin}'            ");
                }
            }
            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = sizes.Split('^');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   p.sizes  = '{tempStr[i]}' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR p.sizes = '{tempStr[i]}' ";
                            }
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )   ");
                }
                else
                {
                    qry.Append($"\n	  AND p.sizes = '{sizes}'            ");
                }
            }
            if (!(unit == null || string.IsNullOrEmpty(unit)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = unit.Split('^');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   p.unit  = '{tempStr[i]}' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR p.unit = '{tempStr[i]}' ";
                            }
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )   ");
                }
                else
                {
                    qry.Append($"\n	  AND p.unit = '{unit}'            ");
                }
            }
            if (!(company == null || string.IsNullOrEmpty(company)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = company.Split('^');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   c.name  = '{tempStr[i]}' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR c.name = '{tempStr[i]}' ";
                            }
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )   ");
                }
                else
                {
                    qry.Append($"\n	  AND c.name = '{company}'            ");
                }
            }
            qry.Append($"\n ORDER BY product, origin, sizes2, sizes, unit + 0, p.purchase_price DESC                         ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCostAccounting3(string sdate, string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n SELECT                                                                ");
            qry.Append($"\n   c.name AS cname                                                     ");
            qry.Append($"\n , p.product                                                           ");
            qry.Append($"\n , p.origin                                                            ");
            qry.Append($"\n , p.sizes                                                             ");
            qry.Append($"\n , p.sizes2                                                            ");
            qry.Append($"\n , p.unit                                                              ");
            qry.Append($"\n , p.cost_unit                                                         ");
            qry.Append($"\n , p.purchase_price                                                    ");
            qry.Append($"\n , p.custom                                                            ");
            qry.Append($"\n , p.tax                                                               ");
            qry.Append($"\n , p.incidental_expense                                                ");
            qry.Append($"\n , p.updatetime                                                        ");
            qry.Append($"\n FROM(                                                                 ");
            qry.Append($"\n 	 SELECT                                                           ");
            qry.Append($"\n 		p.cid                                                         ");
            qry.Append($"\n 	  , p.product                                                     ");
            qry.Append($"\n 	  , p.origin                                                      ");
            qry.Append($"\n 	  , p.sizes                                                       ");
            qry.Append($"\n 	  , p.sizes2                                                      ");
            qry.Append($"\n 	  , p.unit                                                        ");
            qry.Append($"\n 	  , IFNULL(o.cost_unit, '') AS cost_unit                          ");
            qry.Append($"\n 	  , p.purchase_price                                              ");
            qry.Append($"\n 	  , IFNULL(o.custom, 0) AS custom                                 ");
            qry.Append($"\n 	  , IFNULL(o.tax, 0) AS tax                                       ");
            qry.Append($"\n 	  , IFNULL(o.incidental_expense, 0) AS incidental_expense         ");
            qry.Append($"\n 	  , p.updatetime                                                  ");
            qry.Append($"\n 	  FROM                                                            ");
            qry.Append($"\n 	  (                                                               ");
            qry.Append($"\n 		SELECT                                                        ");
            qry.Append($"\n 		  cid                                                         ");
            qry.Append($"\n 		, product                                                     ");
            qry.Append($"\n 		, origin                                                      ");
            qry.Append($"\n 		, sizes                                                       ");
            qry.Append($"\n 		, CASE                                                        ");
            qry.Append($"\n 		    WHEN INSTR(sizes, '/') > 1 THEN SUBSTRING_INDEX(sizes, '/', 1)       ");
            qry.Append($"\n             WHEN INSTR(sizes, '미') > 1 THEN SUBSTRING_INDEX(sizes, '미', 1)     ");
            qry.Append($"\n             WHEN INSTR(sizes, 'kg') > 1 THEN SUBSTRING_INDEX(sizes, 'kg', 1)     ");
            qry.Append($"\n             WHEN INSTR(sizes, 'g') > 1 THEN SUBSTRING_INDEX(sizes, 'g', 1)       ");
            qry.Append($"\n             WHEN INSTR(sizes, 'l') > 1 THEN SUBSTRING_INDEX(sizes, 'l', 1)       ");
            qry.Append($"\n             WHEN INSTR(sizes, 'up') > 1 THEN SUBSTRING_INDEX(sizes, 'up', 1)     ");
            qry.Append($"\n             END AS sizes2                                                            ");
            qry.Append($"\n 		, unit                                                        ");
            qry.Append($"\n 		, purchase_price                                              ");
            qry.Append($"\n 		, MAX(updatetime) AS updatetime                               ");
            qry.Append($"\n 		FROM t_purchase_price                                         ");
            qry.Append($"\n 		WHERE 1=1                                                     ");
            if (!string.IsNullOrEmpty(sdate))
            {
                qry.Append($"\n 		  AND updatetime = '{sdate}'                                    ");
            }
            qry.Append($"\n 		  AND updatetime <> '0000-00-00 00:00:00'                                    ");
            qry.Append($"\n 	    GROUP BY cid, product, origin, sizes, unit, purchase_price         ");
            qry.Append($"\n 	  ) AS p                                                          ");
            qry.Append($"\n 	  LEFT OUTER JOiN  t_product_other_cost AS o                      ");
            qry.Append($"\n 		 ON p.product = o.product                                     ");
            qry.Append($"\n 		AND p.origin = o.origin                                       ");
            qry.Append($"\n 		AND p.sizes = o.sizes                                         ");
            qry.Append($"\n 		AND p.unit = o.unit                                           ");
            qry.Append($"\n ) AS p                                                                ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c                                        ");
            qry.Append($"\n   ON p.cid = c.id                                                     ");
            qry.Append($"\n WHERE 1=1                                                             ");
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                qry.Append($"\n	  AND p.product = '{product}'            ");
            }
            if (!(origin == null || string.IsNullOrEmpty(origin)))
            {
                    qry.Append($"\n	  AND p.origin = '{origin}'            ");
            }
            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
            {
                qry.Append($"\n	  AND p.sizes = '{sizes}'            ");
            }
            if (!(unit == null || string.IsNullOrEmpty(unit)))
            {
                qry.Append($"\n	  AND p.unit = '{unit}'            ");
            }
            qry.Append($"\n ORDER BY product, origin, sizes2, sizes, unit + 0, p.purchase_price DESC                         ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPurchasePrice(string sttdate, string enddate, string product, string origin, string sizes, string unit, string manager, string sdate = "", string price = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                            ");
            qry.Append($"\n   p.*                          ");
            qry.Append($"\n , c.name AS company            ");
            qry.Append($"\n FROM t_purchase_price AS p     ");
            qry.Append($"\n LEFT OUTER JOIN t_company AS c ");
            qry.Append($"\n   ON p.cid = c.id              ");
            qry.Append($"\n WHERE 1=1                      ");
            if (!string.IsNullOrEmpty(sttdate))
            {
                qry.Append($"\n   AND p.updatetime >= '{sttdate}'                 ");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                qry.Append($"\n   AND p.updatetime <= '{enddate}'                 ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n   AND p.product LIKE '%{product}%'                 ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n   AND p.origin LIKE '%{origin}%'                 ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n   AND p.sizes LIKE '%{sizes}%'                 ");
            }
            if (!string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n   AND p.unit LIKE '%{unit}%'                 ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n   AND p.manager LIKE '%{manager}%'                 ");
            }
            if (!string.IsNullOrEmpty(sdate))
            {
                qry.Append($"\n   AND p.updatetime = '{sdate}'                 ");
            }
            if (!string.IsNullOrEmpty(price))
            {
                qry.Append($"\n   AND p.purchase_price = {price}                 ");
            }
            qry.Append($"\n ORDER BY p.updatetime");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetRankingPurchasePriceASOne(string sttdate, string enddate, string product, string origin, string sizes, string unit, string manager, double purchase_price)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                           ");
            qry.Append($"\n   DATE_FORMAT(t.updatetime, '%Y-%m-%d') AS updatetime                            ");
            qry.Append($"\n , t.purchase_price                                                               ");
            qry.Append($"\n , t.division                                                                     ");
            qry.Append($"\n FROM(                                                                            ");
            qry.Append($"\n 	SELECT                                                                       ");
            qry.Append($"\n 	  updatetime                                                                 ");
            qry.Append($"\n 	, purchase_price                                                             ");
            qry.Append($"\n     , 0 AS division                                                              ");
            qry.Append($"\n 	FROM t_purchase_price                                                        ");
            qry.Append($"\n 	WHERE 1=1                                                                    ");
            qry.Append($"\n 	  AND purchase_price > 0                             ");
            if (!string.IsNullOrEmpty(sttdate))
            {
                qry.Append($"\n   AND updatetime >= '{sttdate}'                 ");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                qry.Append($"\n   AND updatetime <= '{enddate}'                 ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n   AND product = '{product}'                 ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n   AND origin = '{origin}'                 ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n   AND sizes = '{sizes}'                 ");
            }
            if (!string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n   AND unit = '{unit}'                 ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n   AND manager = '{manager}'                 ");
            }
            qry.Append($"\n 	UNION ALL                             ");
            if (double.IsInfinity(purchase_price) || double.IsNaN(purchase_price))
                purchase_price = 0;
            qry.Append($"\n 	SELECT '{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}', {purchase_price}, 1   ");

            qry.Append($"\n    UNION ALL                             ");
            qry.Append($"\n    SELECT                                ");
            qry.Append($"\n 	  pi_date AS updatetime              ");
            qry.Append($"\n 	, unit_price AS purchase_price       ");
            qry.Append($"\n     , 0 AS division                      ");
            qry.Append($"\n 	FROM t_customs                       ");
            qry.Append($"\n 	WHERE 1=1                            ");
            qry.Append($"\n 	  AND unit_price > 0                 ");
            if (!string.IsNullOrEmpty(sttdate))
            {
                qry.Append($"\n   AND pi_date >= '{sttdate}'                 ");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                qry.Append($"\n   AND pi_date <= '{enddate}'                 ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n   AND product = '{product}'                 ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n   AND origin = '{origin}'                 ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n   AND sizes = '{sizes}'                 ");
            }
            if (!string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n   AND box_weight = '{unit}'                 ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n   AND manager = '{manager}'                 ");
            }

            qry.Append($"\n ) AS t                                                                              ");
            qry.Append($"\n GROUP BY DATE_FORMAT(t.updatetime, '%Y-%m-%d'), purchase_price, division            ");
            qry.Append($"\n ORDER BY purchase_price, division ASC                                                         ");
           


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public List<PurchasePriceModel> GetPurchasePrice(string sttdate, string enddate, string product, string origin, string sizes, string unit, string company, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                            ");
            qry.Append($"\n   p.id                        ");
            qry.Append($"\n , p.cid                       ");
            qry.Append($"\n , p.product                   ");
            qry.Append($"\n , p.origin                    ");
            qry.Append($"\n , p.sizes                     ");
            qry.Append($"\n , p.unit                      ");
            qry.Append($"\n , p.purchase_price            ");
            qry.Append($"\n , p.updatetime                ");
            qry.Append($"\n , p.edit_user                 ");
            qry.Append($"\n , IF(IFNULL(is_private, 0) = 0, 'false', 'true') AS is_private                 ");
            qry.Append($"\n , c.name AS cname             ");
            qry.Append($"\n FROM t_purchase_price AS p     ");
            qry.Append($"\n INNER JOIN t_company AS c      ");
            qry.Append($"\n   ON p.cid = c.id                ");
            qry.Append($"\n WHERE 1=1                      ");
            if (!(sttdate == null || string.IsNullOrEmpty(sttdate)))
                qry.Append($"\n   AND updatetime >= '{sttdate}'                      ");
            if (!(enddate == null || string.IsNullOrEmpty(enddate)))
                qry.Append($"\n   AND updatetime <= '{enddate}'                      ");

            string[] tempStr = null;
            string tempWhr = "";
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = product.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.product  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.product   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.product   LIKE '%{product}%'                                                                         ");
                }
            }

            if (!(origin == null || string.IsNullOrEmpty(origin)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = origin.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.origin  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.origin   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.origin   LIKE '%{origin}%'                                                                         ");
                }
            }
            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = sizes.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.sizes  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.sizes   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.sizes   LIKE '%{sizes}%'                                                                         ");
                }
            }
            if (!(unit == null || string.IsNullOrEmpty(unit)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = unit.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.unit  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.unit   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.unit   LIKE '%{unit}%'                                                                         ");
                }
            }

            if (!(company == null || string.IsNullOrEmpty(company)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = company.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   c.name  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR c.name   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND c.name   LIKE '%{company}%'                                                                         ");
                }
            }
            if (!(manager == null || string.IsNullOrEmpty(manager)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = manager.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.edit_uer  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.edit_user   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.edit_user   LIKE '%{manager}%'                                                                         ");
                }
            }

            qry.Append($"\n ORDER BY p.product, p.origin, SUBSTRING_INDEX(p.sizes,'/',1) + 0, SUBSTRING_INDEX(p.sizes,'미',1) + 0, SUBSTRING_INDEX(p.sizes,'kg',1) + 0, p.sizes, CAST(p.unit AS double), c.name, p.updatetime");


            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetPurchasePrice(dr);
        }
        public DataTable GetPurchasePrice2(string sttdate, string enddate, string product, string origin, string sizes, string unit, string company, bool isExactly, string manager, string purchase_id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                     ");
            qry.Append($"\n   p.id                                                                     ");
            qry.Append($"\n , p.cid                                                                    ");
            qry.Append($"\n , p.product                                                                ");
            qry.Append($"\n , p.origin                                                                 ");
            qry.Append($"\n , p.sizes                                                                  ");
            qry.Append($"\n , p.unit                                                                   ");
            qry.Append($"\n , IFNULL(p.cost_unit, 0) AS cost_unit                                      ");
            qry.Append($"\n , p.purchase_price                                                         ");
            qry.Append($"\n , p.updatetime                                                             ");
            qry.Append($"\n , p.edit_user                                                              ");
            qry.Append($"\n , IF(IFNULL(is_private, 0) = 0, 'FALSE', 'TRUE') AS is_private             ");
            qry.Append($"\n , IFNULL(p.custom, 0) AS custom                                            ");
            qry.Append($"\n , IFNULL(p.tax, 0) AS tax                                                  ");
            qry.Append($"\n , IFNULL(p.incidental_expense, 0) AS incidental_expense                    ");
            qry.Append($"\n , c.name AS cname                                                          ");

            qry.Append($"\n , IF(IFNULL(p.weight_calculate, p.weight_calculate2) = 1, 'TRUE', 'FALSE') AS weight_calculate           ");
            qry.Append($"\n , IF(IFNULL(p.is_FOB, 0) = 1, 'TRUE', 'FALSE') AS is_FOB                    ");

            //qry.Append($"\n , IF(p.tray_calculate   = 1, 'TRUE', 'FASE') AS tray_calculate             ");
            qry.Append($"\n , IFNULL(p.fixed_tariff, 0) AS fixed_tariff                                ");
            qry.Append($"\n FROM (                                                                     ");
            qry.Append($"\n	   SELECT                                                                  ");
            qry.Append($"\n      p.id                                                                  ");
            qry.Append($"\n	   , p.cid                                                                 ");
            qry.Append($"\n    , p.product                                                             ");
            qry.Append($"\n	   , p.origin                                                              ");
            qry.Append($"\n    , p.sizes                                                               ");
            qry.Append($"\n    , p.unit                                                                ");
            qry.Append($"\n    , IF(IFNULL(p.cost_unit, '') = '', p2.cost_unit, p.cost_unit) AS cost_unit     ");
            qry.Append($"\n    , p.purchase_price                                                      ");
            qry.Append($"\n    , p.updatetime                                                          ");
            qry.Append($"\n    , p.edit_user                                                           ");
            qry.Append($"\n    , p.is_private                                                          ");

            qry.Append($"\n    , p.weight_calculate                                                    ");
            qry.Append($"\n    , p.is_FOB                                                              ");

            qry.Append($"\n    , p2.custom                                                             ");
            qry.Append($"\n    , p2.tax                                                                ");
            qry.Append($"\n    , p2.incidental_expense                                                 ");
            qry.Append($"\n    , IFNULL(p2.weight_calculate, 1)  AS weight_calculate2                  ");
            qry.Append($"\n    , p2.tray_calculate                                                     ");
            qry.Append($"\n    , IFNULL(p.fixed_tariff, 0) AS fixed_tariff                             ");
            qry.Append($"\n	   FROM t_purchase_price AS p                                              ");
            qry.Append($"\n    LEFT OUTER JOIN t_product_other_cost AS p2                              ");
            qry.Append($"\n      ON p.product = p2.product                                             ");
            qry.Append($"\n      AND p.origin = p2.origin                                              ");
            qry.Append($"\n      AND p.sizes = p2.sizes                                                ");
            qry.Append($"\n      AND p.unit = p2.unit                                                  ");
            qry.Append($"\n    WHERE 1=1                                                               ");

            if (!(sttdate == null || string.IsNullOrEmpty(sttdate)))
                qry.Append($"\n   AND p.updatetime >= '{sttdate}'                      ");
            if (!(enddate == null || string.IsNullOrEmpty(enddate)))
                qry.Append($"\n   AND p.updatetime <= '{enddate}'                      ");

            string[] tempStr = null;
            string tempWhr = "";
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = product.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.product  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.product   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.product   LIKE '%{product}%'                                                                         ");
                }
            }
            if (!(origin == null || string.IsNullOrEmpty(origin)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = origin.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.origin  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.origin   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.origin   LIKE '%{origin}%'                                                                         ");
                }
            }
            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = sizes.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.sizes  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.sizes   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.sizes   LIKE '%{sizes}%'                                                                         ");
                }
            }
            if (!(unit == null || string.IsNullOrEmpty(unit)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = unit.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.unit  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.unit   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.unit   LIKE '%{unit}%'                                                                         ");
                }
            }
            if (!(manager == null || string.IsNullOrEmpty(manager)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = manager.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   p.edit_uer  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR p.edit_user   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND p.edit_user   LIKE '%{manager}%'                                                                         ");
                }
            }
            qry.Append($"\n ) AS p                                                                     ");
            qry.Append($"\n INNER JOIN t_company AS c                                                  ");
            qry.Append($"\n   ON p.cid = c.id                                                          ");
            qry.Append($"\n WHERE 1=1                                                                  ");
            if (!(company == null || string.IsNullOrEmpty(company)))
            {
                if (isExactly)
                    qry.Append($"\n	  AND c.name   LIKE '%{company}%'                                                                         ");
                else
                {
                    tempStr = null;
                    tempWhr = "";
                    tempStr = company.Split(' ');
                    if (tempStr.Length > 1)
                    {
                        for (int i = 0; i < tempStr.Length; i++)
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                                tempWhr = $"\n	   c.name  LIKE '%{tempStr[i]}%' ";
                            else
                                tempWhr += $"\n	   OR c.name   LIKE '%{tempStr[i]}%' ";
                        }
                        qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                    }
                    else
                        qry.Append($"\n	  AND c.name   LIKE '%{company}%'                                                                         ");
                }
            }

            if(!string.IsNullOrEmpty(purchase_id))
                qry.Append($"\n   AND p.id = {purchase_id}                                                                  ");
            qry.Append($"\n ORDER BY p.product, p.origin, SUBSTRING_INDEX(p.sizes,'/',1) + 0, SUBSTRING_INDEX(p.sizes,'미',1) + 0, SUBSTRING_INDEX(p.sizes,'kg',1) + 0, p.sizes, CAST(p.unit AS double), c.name, p.updatetime  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        private List<PurchasePriceModel> SetPurchasePrice(MySqlDataReader rd)
        {
            List<PurchasePriceModel> list = new List<PurchasePriceModel>();
            while (rd.Read())
            {
                PurchasePriceModel model = new PurchasePriceModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.product = rd["product"].ToString();
                model.origin = rd["origin"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.unit = rd["unit"].ToString();
                model.company = rd["cname"].ToString();
                model.purchase_price = Convert.ToDouble(rd["purchase_price"].ToString());
                model.updatetime = Convert.ToDateTime(rd["updatetime"].ToString()).ToString("yyyy-MM-dd");
                model.edit_user = rd["edit_user"].ToString();
                model.is_private = Convert.ToBoolean(rd["is_private"].ToString());

                list.Add(model); ;
            }
            rd.Close();
            return list;
        }

        public int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
        {

            if (sqlList.Count > 0)
            {
                dbInstance.Connection.Close();
                MySqlConnection conn = (MySqlConnection)dbInstance.Connection;
                MySqlCommand command = conn.CreateCommand();
                transaction = conn.BeginTransaction();

                try
                {
                    int susccesCnt = 0;
                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        string sql = sqlList[i].ToString();
                        command.CommandText = sqlList[i].ToString();
                        command.ExecuteNonQuery();
                        susccesCnt++;
                    }

                    if (sqlList.Count == susccesCnt)
                    {
                        transaction.Commit();
                        return susccesCnt;
                    }
                    else
                    {
                        transaction.Rollback();
                        return -1;
                    }

                }
                catch (Exception e)
                {
                    try
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                    catch (MySqlException myex)
                    {
                        if (transaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + myex.GetType() +
                                              " was encountered while attempting to roll back the transaction.");
                        }
                    }
                    Console.WriteLine(e.Message);
                    return -1;
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                Console.WriteLine("sql null");
                return -1;
            }
        }

        public StringBuilder InsertPurchasePrice(PurchasePriceModel ppm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT INTO t_purchase_price (            ");
            qry.Append($"  id                                      ");
            qry.Append($", cid                                     ");
            qry.Append($", product                                 ");
            qry.Append($", origin                                  ");
            qry.Append($", sizes                                   ");
            qry.Append($", unit                                    ");
            qry.Append($", cost_unit                               ");
            qry.Append($", purchase_price                          ");
            qry.Append($", updatetime                              ");
            qry.Append($", edit_user                               ");
            qry.Append($", is_private                              ");
            qry.Append($", fixed_tariff                            ");
            qry.Append($", is_FOB                                  ");
            qry.Append($", weight_calculate                        ");
            qry.Append($") VALUES (                                ");
            qry.Append($"   {ppm.id}                               ");
            qry.Append($",  {ppm.company}                          ");
            qry.Append($", '{ppm.product}'                         ");
            qry.Append($", '{ppm.origin}'                          ");
            qry.Append($", '{ppm.sizes}'                           ");
            qry.Append($", '{ppm.unit}'                            ");
            qry.Append($", '{ppm.cost_unit}'                       ");
            qry.Append($",  {ppm.purchase_price}                   ");
            qry.Append($", '{ppm.updatetime}'                      ");
            qry.Append($", '{ppm.edit_user}'                       ");
            qry.Append($",  {ppm.is_private}                       ");
            qry.Append($",  {ppm.fixed_tariff}                     ");
            qry.Append($",  {ppm.is_FOB}                           ");
            qry.Append($",  {ppm.weight_calculate}                 ");
            qry.Append($")                                         ");

            return qry;
        }

        public StringBuilder UpdatePurchasePrice(PurchasePriceModel ppm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE t_purchase_price SET                                       ");
            qry.Append($"  cid            =  {ppm.company       }                          ");
            qry.Append($", product        = '{ppm.product       }'                         ");
            qry.Append($", origin         = '{ppm.origin        }'                         ");
            qry.Append($", sizes          = '{ppm.sizes         }'                         ");
            qry.Append($", unit           = '{ppm.unit          }'                         ");
            qry.Append($", cost_unit      = '{ppm.cost_unit     }'                         ");
            qry.Append($", fixed_tariff   =  {ppm.fixed_tariff}                            ");
            qry.Append($", weight_calculate   =  {ppm.weight_calculate}                    ");
            qry.Append($", is_FOB             =  {ppm.is_FOB}                              ");
            qry.Append($", purchase_price =  {ppm.purchase_price}                          ");
            qry.Append($", updatetime     = '{ppm.updatetime    }'                         ");
            qry.Append($", edit_user      = '{ppm.edit_user     }'                         ");
            qry.Append($"WHERE id = {ppm.id}                                               ");
            return qry;
        }


        public StringBuilder DeletePurchasePrice(string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_purchase_price                  ");
            qry.Append($"  WHERE id = {id}                             ");

            return qry;
        }

        public StringBuilder DeletePurchasePrice2(PurchasePriceModel ppm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_purchase_price                  ");
            qry.Append($"  WHERE cid = {ppm.company}                   ");
            qry.Append($"    AND product = '{ppm.product}'             ");
            qry.Append($"    AND origin = '{ppm.origin}'               ");
            qry.Append($"    AND sizes = '{ppm.sizes}'                 ");
            qry.Append($"    AND unit = '{ppm.unit}'                   ");
            qry.Append($"    AND updatetime = '{ppm.updatetime}'       ");

            return qry;
        }

        public DataTable GetTradeManaer(string product, string origin, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                     ");
            qry.Append($"\n   product                                                                  ");
            qry.Append($"\n , origin                                                                   ");
            qry.Append($"\n , manager                                                                  ");
            qry.Append($"\n FROM t_product_other_cost                                                  ");
            qry.Append($"\n WHERE 1=1                                                                  ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("product", product)}                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("origin", origin)}                      ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {commonRepository.whereSql("manager", manager)}                      ");
            qry.Append($"\n GROUP BY product, origin, manager                                          ");
            qry.Append($"\n ORDER BY product, origin, manager                                          ");

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




using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductOtherCostRepository : ClassRoot, IProductOtherCostRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public DataTable GetProductByUser(string product, string origin, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                             ");
            qry.Append($"\n  distinct                          ");
            qry.Append($"\n   product                          ");
            qry.Append($"\n , origin                           ");
            qry.Append($"\n , manager                          ");
            qry.Append($"\n FROM t_product_other_cost          ");
            qry.Append($"\n WHERE 1=1                          ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND product LIKE '%{product}%'                                                        ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND origin LIKE '%{origin}%'                                                          ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   AND manager LIKE '%{manager}%'                                                            ");
            
            qry.Append($"\n ORDER BY manager, product, origin  ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductOnlyOne(string product, string origin, string sizes, string seaover_unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                         ");
            qry.Append($"\n   p.*                                                                                                          ");
            qry.Append($"\n , CONCAT(p.product, '^', p.origin, '^', p.sizes, '^', p.unit) AS 품목코드                                      ");
            qry.Append($"\n , pp.company                                                                                                   ");
            qry.Append($"\n FROM (                                                                                                         ");
            qry.Append($"\n 	SELECT                                                                                                     ");
            qry.Append($"\n 	  p.*                                                                                                      ");
            qry.Append($"\n 	FROM (                                                                                                     ");
            qry.Append($"\n 		SELECT                                                                                                 ");
            qry.Append($"\n 		*                                                                                                      ");
            qry.Append($"\n 		, @seq := IF(@prev_product_origin = CONCAT(product, origin, sizes, unit), @seq + 1, 1) AS sequence     ");
            qry.Append($"\n 		, @prev_product_origin := CONCAT(product, origin, sizes, unit) AS dummy                                ");
            qry.Append($"\n 		FROM t_product_other_cost                                                                              ");
            qry.Append($"\n 	WHERE 1 = 1                                                                        ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n     {commonRepository.whereSql("product", product)}                                                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n     {commonRepository.whereSql("origin", origin)}                                                      ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n     {commonRepository.whereSql("sizes", sizes)}                                                      ");
            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n     {commonRepository.whereSql("unit", seaover_unit)}                                                      ");
            qry.Append($"\n 		ORDER BY product, origin, sizes, unit, manager                                                         ");
            qry.Append($"\n 	) AS p                                                                                                     ");
            qry.Append($"\n 	WHERE sequence = 1                                                                                         ");
            qry.Append($"\n ) AS p                                                                                                         ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                                     ");
            qry.Append($"\n 	  p.product                                                                                                ");
            qry.Append($"\n 	, p.origin                                                                                                 ");
            qry.Append($"\n 	, p.sizes                                                                                                  ");
            qry.Append($"\n 	, p.unit                                                                                                   ");
            qry.Append($"\n 	, GROUP_CONCAT(company,'^') AS company                                                                     ");
            qry.Append($"\n 	FROM (                                                                                                     ");
            qry.Append($"\n 		SELECT                                                                                                 ");
            qry.Append($"\n 		  distinct p.product                                                                                   ");
            qry.Append($"\n 		, p.origin                                                                                             ");
            qry.Append($"\n 		, p.sizes                                                                                              ");
            qry.Append($"\n 		, p.unit                                                                                               ");
            qry.Append($"\n 		, c.name AS company                                                                                    ");
            qry.Append($"\n 		FROM t_purchase_price AS p                                                                             ");
            qry.Append($"\n 		LEFT OUTER JOIN t_company AS c                                                                         ");
            qry.Append($"\n 		  ON p.cid = c.id                                                                                      ");
            qry.Append($"\n 	    WHERE 1 = 1                                                                        ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n     {commonRepository.whereSql("p.product", product)}                                                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n     {commonRepository.whereSql("p.origin", origin)}                                                      ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n     {commonRepository.whereSql("p.sizes", sizes)}                                                      ");
            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n     {commonRepository.whereSql("p.unit", seaover_unit)}                                                      ");
            qry.Append($"\n 	) AS p                                                                                                     ");
            qry.Append($"\n 	GROUP BY p.product, p.origin, p.sizes, p.unit                                                              ");
            qry.Append($"\n ) AS pp                                                                                                        ");
            qry.Append($"\n   ON p.product = pp.product                                                                                    ");
            qry.Append($"\n   AND p.origin = pp.origin                                                                                     ");
            qry.Append($"\n   AND p.sizes = pp.sizes                                                                                       ");
            qry.Append($"\n   AND p.unit = pp.unit                                                                                         ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductAndPending(string product, string origin, string sizes, string seaover_unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                         ");
            qry.Append($"\n   p.*                                                                                                          ");
            qry.Append($"\n , CONCAT(p.product, '^', p.origin, '^', p.sizes, '^', p.unit) AS 품목코드                                      ");
            qry.Append($"\n , pp.company                                                                                                   ");
            qry.Append($"\n FROM (                                                                                                         ");
            qry.Append($"\n 	SELECT                                                                                                     ");
            qry.Append($"\n 	  p.*                                                                                                      ");
            qry.Append($"\n 	FROM (                                                                                                     ");
            qry.Append($"\n 		SELECT                                                                                                 ");
            qry.Append($"\n 		*                                                                                                      ");
            qry.Append($"\n 		, @seq := IF(@prev_product_origin = CONCAT(product, origin, sizes, unit), @seq + 1, 1) AS sequence     ");
            qry.Append($"\n 		, @prev_product_origin := CONCAT(product, origin, sizes, unit) AS dummy                                ");
            qry.Append($"\n 		FROM t_product_other_cost                                                                              ");
            qry.Append($"\n 	WHERE 1 = 1                                                                        ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n     {commonRepository.whereSql("product", product)}                                                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n     {commonRepository.whereSql("origin", origin)}                                                      ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n     {commonRepository.whereSql("sizes", sizes)}                                                      ");
            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n     {commonRepository.whereSql("unit", seaover_unit)}                                                      ");
            qry.Append($"\n 		ORDER BY product, origin, sizes, unit, manager                                                         ");
            qry.Append($"\n 	) AS p                                                                                                     ");
            qry.Append($"\n 	WHERE sequence = 1                                                                                         ");
            qry.Append($"\n ) AS p                                                                                                         ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                                     ");
            qry.Append($"\n 	  p.product                                                                                                ");
            qry.Append($"\n 	, p.origin                                                                                                 ");
            qry.Append($"\n 	, p.sizes                                                                                                  ");
            qry.Append($"\n 	, p.unit                                                                                                   ");
            qry.Append($"\n 	, GROUP_CONCAT(company,'^') AS company                                                                     ");
            qry.Append($"\n 	FROM (                                                                                                     ");
            qry.Append($"\n 		SELECT                                                                                                 ");
            qry.Append($"\n 		  distinct p.product                                                                                   ");
            qry.Append($"\n 		, p.origin                                                                                             ");
            qry.Append($"\n 		, p.sizes                                                                                              ");
            qry.Append($"\n 		, p.unit                                                                                               ");
            qry.Append($"\n 		, c.name AS company                                                                                    ");
            qry.Append($"\n 		FROM t_purchase_price AS p                                                                             ");
            qry.Append($"\n 		LEFT OUTER JOIN t_company AS c                                                                         ");
            qry.Append($"\n 		  ON p.cid = c.id                                                                                      ");
            qry.Append($"\n 	    WHERE 1 = 1                                                                        ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n     {commonRepository.whereSql("p.product", product)}                                                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n     {commonRepository.whereSql("p.origin", origin)}                                                      ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n     {commonRepository.whereSql("p.sizes", sizes)}                                                      ");
            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n     {commonRepository.whereSql("p.unit", seaover_unit)}                                                      ");
            qry.Append($"\n 	) AS p                                                                                                     ");
            qry.Append($"\n 	GROUP BY p.product, p.origin, p.sizes, p.unit                                                              ");
            qry.Append($"\n ) AS pp                                                                                                        ");
            qry.Append($"\n   ON p.product = pp.product                                                                                    ");
            qry.Append($"\n   AND p.origin = pp.origin                                                                                     ");
            qry.Append($"\n   AND p.sizes = pp.sizes                                                                                       ");
            qry.Append($"\n   AND p.unit = pp.unit                                                                                         ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductInfoAsOneExactly(string product, string origin, string sizes, string seaover_unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                             ");
            qry.Append($"\n   p.product                                                                        ");
            qry.Append($"\n , p.origin                                                                         ");
            qry.Append($"\n , p.sizes                                                                          ");
            qry.Append($"\n , p.unit                                                                           ");
            qry.Append($"\n , IFNULL(p.cost_unit, 0) AS cost_unit                                              ");
            qry.Append($"\n , IFNULL(p.custom, 0) AS custom                                                    ");
            qry.Append($"\n , IFNULL(p.tax, 0) AS tax                                                          ");
            qry.Append($"\n , IFNULL(p.incidental_expense, 0) AS incidental_expense                            ");
            qry.Append($"\n , IF(IFNULL(p.weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate       ");
            qry.Append($"\n , IF(IFNULL(p.tray_calculate, 0) = 1, 'TRUE', 'FALSE') AS tray_calculate           ");
            qry.Append($"\n , IFNULL(p.manager, '') AS manager                                                 ");
            qry.Append($"\n , IFNULL(p.production_days, 20) AS production_days                                 ");
            qry.Append($"\n , IFNULL(p.purchase_margin, 0) AS purchase_margin                                  ");
            qry.Append($"\n , IFNULL(c.delivery_days, 15) AS delivery_days                                     ");
            qry.Append($"\n FROM t_product_other_cost AS p                                                     ");
            qry.Append($"\n LEFT OUTER JOIN t_country AS c                                                     ");
            qry.Append($"\n   ON p.origin = c.country_name                                                     ");
            qry.Append($"\n WHERE 1 = 1                                                                        ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND p.product = '{product}'                                                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND p.origin = '{origin}'                                                      ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND p.sizes = '{sizes}'                                                      ");
            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND p.unit = '{seaover_unit}'                                                      ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductInfoAsOne(string product, string origin, string sizes, string unit, string seaover_unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                             ");
            qry.Append($"\n   p.product                                                                        ");
            qry.Append($"\n , p.origin                                                                         ");
            qry.Append($"\n , p.sizes                                                                          ");
            qry.Append($"\n , p.unit                                                                           ");
            qry.Append($"\n , IFNULL(p.cost_unit, 0) AS cost_unit                                              ");
            qry.Append($"\n , IFNULL(p.custom, 0) AS custom                                                    ");
            qry.Append($"\n , IFNULL(p.tax, 0) AS tax                                                          ");
            qry.Append($"\n , IFNULL(p.incidental_expense, 0) AS incidental_expense                            ");
            qry.Append($"\n , IF(IFNULL(p.weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate       ");
            qry.Append($"\n , IF(IFNULL(p.tray_calculate, 0) = 1, 'TRUE', 'FALSE') AS tray_calculate           ");
            qry.Append($"\n , IFNULL(p.manager, '') AS manager                                                 ");
            qry.Append($"\n , IFNULL(p.production_days, 20) AS production_days                                 ");
            qry.Append($"\n , IFNULL(p.purchase_margin, 0) AS purchase_margin                                  ");
            qry.Append($"\n , IFNULL(c.delivery_days, 15) AS delivery_days                                     ");
            qry.Append($"\n FROM t_product_other_cost AS p                                                     ");
            qry.Append($"\n LEFT OUTER JOIN t_country AS c                                                     ");
            qry.Append($"\n   ON p.origin = c.country_name                                                     ");
            qry.Append($"\n WHERE 1 = 1                                                                        ");
            if(!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("p.product", product)}                                                          ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("p.origin", origin)}                                                          ");
            
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("p.sizes", sizes)}                                                          ");

            if (!string.IsNullOrEmpty(unit) && !string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND (p.unit LIKE '%{unit}%' OR p.unit LIKE '%{seaover_unit}%'                                ");

            else if (!string.IsNullOrEmpty(unit) && string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND p.unit LIKE '%{unit}%'                                                              ");
            else if (string.IsNullOrEmpty(unit) && !string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND p.unit LIKE '%{seaover_unit}%'                                                      ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductAsOne(string product, string origin, string sizes, string unit, string seaover_unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                             ");
            qry.Append($"\n   group_name                                                                          ");
            qry.Append($"\n , product                                                                          ");
            qry.Append($"\n , origin                                                                           ");
            qry.Append($"\n , sizes                                                                            ");
            qry.Append($"\n , unit                                                                             ");
            qry.Append($"\n , IFNULL(cost_unit, 0) AS cost_unit                                                ");
            qry.Append($"\n , IFNULL(custom, 0) AS custom                                                      ");
            qry.Append($"\n , IFNULL(tax, 0) AS tax                                                            ");
            qry.Append($"\n , IFNULL(incidental_expense, 0) AS incidental_expense                              ");
            qry.Append($"\n , IF(IFNULL(weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate         ");
            qry.Append($"\n , IF(IFNULL(tray_calculate, 0) = 1, 'TRUE', 'FALSE') AS tray_calculate             ");
            qry.Append($"\n , IFNULL(manager, '') AS manager                                                   ");
            qry.Append($"\n , IFNULL(updatetime, '') AS updatetime                                             ");
            qry.Append($"\n , IFNULL(edit_user, '') AS edit_user                                               ");
            qry.Append($"\n , IFNULL(production_days, 20) AS production_days                                   ");
            qry.Append($"\n , IF(IFNULL(isMonth, 0) = 1, 'TRUE', 'FALSE') AS isMonth                           ");
            qry.Append($"\n , IFNULL(show_sttdate, '') AS show_sttdate                                               ");
            qry.Append($"\n , IFNULL(show_enddate, '') AS show_enddate                                               ");
            qry.Append($"\n , IFNULL(hide_date, '') AS hide_date                                               ");

            qry.Append($"\n , IFNULL(purchase_margin, '') AS purchase_margin                                   ");
            qry.Append($"\n , IFNULL(base_around_month, '') AS base_around_month                                   ");

            qry.Append($"\n FROM t_product_other_cost                               ");
            qry.Append($"\n WHERE product = '{product}'                             ");
            qry.Append($"\n   AND origin = '{origin}'                               ");
            qry.Append($"\n   AND sizes = '{sizes}'                                 ");
            if (!string.IsNullOrEmpty(unit) && !string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND (unit = '{unit}' OR unit = '{seaover_unit}')      ");
            else if (!string.IsNullOrEmpty(unit) && string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND unit = '{unit}'                                   ");
            else if (string.IsNullOrEmpty(unit) && !string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND unit = '{seaover_unit}'                           ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProduct(string product, bool isExactly, string origin, string sizes, string unit, string manager = "", string group_name = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                            ");
            qry.Append($"\n    p.*                                                                                                            ");
            qry.Append($"\n  , c.cnt                                                                                                          ");
            qry.Append($"\n  FROM (                                                                                                           ");
            qry.Append($"\n 	 SELECT                                                                                                       ");
            qry.Append($"\n 	   group_name                                                                                                 ");
            qry.Append($"\n 	 , product                                                                                                    ");
            qry.Append($"\n 	 , origin                                                                                                     ");
            qry.Append($"\n 	 , sizes                                                                                                      ");
            qry.Append($"\n 	 , concat('',unit * 1) AS unit                                                                                ");
            qry.Append($"\n 	 , IF(IFNULL(cost_unit           , '') = '', 0, IFNULL(cost_unit           , '')) AS cost_unit                ");
            qry.Append($"\n 	 , IF(IFNULL(custom              , '') = '', 0, IFNULL(custom              , '')) AS custom                   ");
            qry.Append($"\n 	 , IF(IFNULL(incidental_expense  , '') = '', 0, IFNULL(incidental_expense  , '')) AS incidental_expense       ");
            qry.Append($"\n 	 , IF(IFNULL(tax                 , '') = '', 0, IFNULL(tax                 , '')) AS tax                      ");
            qry.Append($"\n 	 , manager                                                                                                    ");
            qry.Append($"\n 	 , IF(weight_calculate = 1, 'TRUE', 'FALSE') AS weight_calculate                                              ");
            qry.Append($"\n 	 , IF(tray_calculate = 1, 'TRUE', 'FALSE') AS tray_calculate                                                  ");
            qry.Append($"\n 	 , IFNULL(production_days, 20) AS production_days                                                             ");
            qry.Append($"\n 	 , IFNULL(purchase_margin, 5) AS purchase_margin                                                              ");
            qry.Append($"\n 	 , IF(IFNULL(isMonth, '0') = '0', 'FALSE', 'TRUE') AS isMonth                                                 ");
            qry.Append($"\n 	 , IF(show_sttdate < '2019-01-01', NULL, show_sttdate) AS show_sttdate                                        ");
            qry.Append($"\n 	 , IF(show_enddate < '2019-01-01', NULL, show_enddate) AS show_enddate                                        ");
            qry.Append($"\n 	 , hide_date                                                                                                  ");
            qry.Append($"\n 	 , IFNULL(base_around_month, 2) AS base_around_month                                                          ");
            qry.Append($"\n 	 , IFNULL(remark, '') AS remark                                                                               ");
            qry.Append($"\n 	 FROM t_product_other_cost                                                                                    ");
            qry.Append($"\n 	 WHERE 1=1                                                                                                    ");
            if (!(group_name == null || string.IsNullOrEmpty(group_name)))
                qry.Append($"\n 	   {commonRepository.whereSql("group_name", group_name)}                                                                   ");
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                if(!isExactly)
                    qry.Append($"\n 	   {commonRepository.whereSql("product", product)}                                                                   ");
                else
                    qry.Append($"\n 	   AND product LIKE '%{product}%'                                                                   ");
            }
                
            if (!(origin == null || string.IsNullOrEmpty(origin)))
                qry.Append($"\n 	   {commonRepository.whereSql("origin", origin)}                                                                   ");
            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
                qry.Append($"\n 	   {commonRepository.whereSql("sizes", sizes)}                                                                   ");
            if (!(unit == null || string.IsNullOrEmpty(unit)))
                qry.Append($"\n 	   {commonRepository.whereSql("unit", unit)}                                                                   ");
            if (!(manager == null || string.IsNullOrEmpty(manager)))
                qry.Append($"\n 	   {commonRepository.whereSql("manager", manager)}                                                                   ");
            qry.Append($"\n  ) AS p                                                                                                           ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                ");
            qry.Append($"\n 	SELECT                                                                                                        ");
            qry.Append($"\n     product                                                                                                       ");
            qry.Append($"\n     , origin                                                                                                      ");
            qry.Append($"\n     , sizes                                                                                                       ");
            qry.Append($"\n     , concat('',unit * 1) AS unit                                                                                 ");
            qry.Append($"\n     , IF(IFNULL(cost_unit, '') = '', 0, cost_unit) AS cost_unit                                                   ");
            qry.Append($"\n     , COUNT(*) AS cnt                                                                                             ");
            qry.Append($"\n     FROM t_product_other_cost                                                                                     ");
            qry.Append($"\n 	WHERE 1=1                                                                                                    ");
            if (!(group_name == null || string.IsNullOrEmpty(group_name)))
                qry.Append($"\n 	   {commonRepository.whereSql("group_name", group_name)}                                                                   ");
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                if (!isExactly)
                    qry.Append($"\n 	   {commonRepository.whereSql("product", product)}                                                                   ");
                else
                    qry.Append($"\n 	   AND product LIKE '%{product}%'                                                                   ");
            }
            if (!(origin == null || string.IsNullOrEmpty(origin)))
                qry.Append($"\n 	   {commonRepository.whereSql("origin", origin)}                                                                   ");
            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
                qry.Append($"\n 	   {commonRepository.whereSql("sizes", sizes)}                                                                   ");
            if (!(unit == null || string.IsNullOrEmpty(unit)))
                qry.Append($"\n 	   {commonRepository.whereSql("unit", unit)}                                                                   ");
            if (!(manager == null || string.IsNullOrEmpty(manager)))
                qry.Append($"\n 	   {commonRepository.whereSql("manager", manager)}                                                                   ");
            qry.Append($"\n     GROUP BY product, origin, sizes, unit, IF(IFNULL(cost_unit, '') = '', 0, cost_unit)                           ");
            qry.Append($"\n  ) AS c                                                                                                           ");
            qry.Append($"\n    ON p.product = c.product                                                                                       ");
            qry.Append($"\n    AND p.origin = c.origin                                                                                        ");
            qry.Append($"\n    AND p.sizes = c.sizes                                                                                          ");
            qry.Append($"\n    AND p.unit = c.unit                                                                                            ");
            qry.Append($"\n    AND p.cost_unit = p.cost_unit                                                                                  ");
            qry.Append($"\n ORDER BY  product, origin, sizes, unit                                                                            ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public StringBuilder UpdateProduct(string product, string origin, string sizes, string unit, string seaover_unit, int tabType, bool isVal, string sdate, string edate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_product_other_cost SET   ");

            if (tabType == 1)
                qry.Append($"\n   isMonth = {isVal}                      ");
            else if (tabType == 2)
            {
                qry.Append($"\n     show_sttdate = '{sdate}'                    ");
                qry.Append($"\n   , show_enddate = '{edate}'                    ");
            }
            else if (tabType == 3)
                qry.Append($"\n   hide_date = '{sdate}'                    ");

            qry.Append($"\n WHERE 1=1                                                          ");
            qry.Append($"\n   AND product = '{product}'                                        ");
            qry.Append($"\n   AND origin = '{origin}'                                          ");
            qry.Append($"\n   AND sizes = '{sizes}'                                            ");
            qry.Append($"\n   AND (unit = '{unit}' OR unit  = '{seaover_unit}')                ");
            

            string sql = qry.ToString();

            return qry;
        }

        public StringBuilder UpdateOffer(string product_origin, string origin_origin, string sizes_origin, string unit_origin, string product, string origin, string sizes, string unit, bool weight_calculate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_purchase_price SET   ");
            qry.Append($"\n   product = '{product}'                                            ");
            qry.Append($"\n , origin = '{origin}'                                              ");
            qry.Append($"\n , sizes = '{sizes}'                                                ");
            qry.Append($"\n , unit = '{unit}'                                                  ");
            qry.Append($"\n , weight_calculate = {weight_calculate}                            ");
            qry.Append($"\n WHERE 1=1                                                          ");
            qry.Append($"\n   AND product = '{product_origin}'                                 ");
            qry.Append($"\n   AND origin = '{origin_origin}'                                   ");
            qry.Append($"\n   AND sizes = '{sizes_origin}'                                     ");
            qry.Append($"\n   AND unit = '{unit_origin}'                                       ");

            string sql = qry.ToString();
            return qry;
        }
        public StringBuilder UpdatePendding(string product_origin, string origin_origin, string sizes_origin, string unit_origin, string product, string origin, string sizes, string unit, bool weight_calculate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_customs SET   ");
            qry.Append($"\n   product = '{product}'                                            ");
            qry.Append($"\n , origin = '{origin}'                                              ");
            qry.Append($"\n , sizes = '{sizes}'                                                ");
            qry.Append($"\n , box_weight = '{unit}'                                            ");
            qry.Append($"\n , weight_calculate = {weight_calculate}                            ");
            qry.Append($"\n WHERE 1=1                                                          ");
            qry.Append($"\n   AND product = '{product_origin}'                                 ");
            qry.Append($"\n   AND origin = '{origin_origin}'                                   ");
            qry.Append($"\n   AND sizes = '{sizes_origin}'                                     ");
            qry.Append($"\n   AND box_weight = '{unit_origin}'                                 ");

            string sql = qry.ToString();
            return qry;
        }


        public StringBuilder InsertProduct2(ProductOtherCostModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_product_other_cost (");
            qry.Append($"\n   group_name                           ");
            qry.Append($"\n , product                         ");
            qry.Append($"\n , origin                          ");
            qry.Append($"\n , sizes                           ");
            qry.Append($"\n , unit                            ");
            qry.Append($"\n , cost_unit                       ");
            qry.Append($"\n , custom                          ");
            qry.Append($"\n , tax                             ");
            qry.Append($"\n , production_days                 ");
            qry.Append($"\n , incidental_expense              ");
            qry.Append($"\n , manager                         ");
            qry.Append($"\n , weight_calculate                ");
            qry.Append($"\n , tray_calculate                  ");
            qry.Append($"\n , updatetime                      ");
            qry.Append($"\n , edit_user                       ");
            qry.Append($"\n , isMonth                         ");
            qry.Append($"\n , show_sttdate                    ");
            qry.Append($"\n , show_enddate                    ");
            qry.Append($"\n , hide_date                       ");

            qry.Append($"\n ) VALUES (                        ");
            qry.Append($"\n   '{model.group_name}'            ");
            qry.Append($"\n , '{model.product}'               ");
            qry.Append($"\n , '{model.origin}'                ");
            qry.Append($"\n , '{model.sizes}'                 ");
            qry.Append($"\n , '{model.unit}'                  ");
            qry.Append($"\n , '{model.cost_unit}'             ");
            qry.Append($"\n , {model.custom}                  ");
            qry.Append($"\n , {model.tax}                     ");
            qry.Append($"\n , {model.production_days}         ");
            qry.Append($"\n , {model.incidental_expense}      ");
            qry.Append($"\n , '{model.manager}'               ");
            qry.Append($"\n , {model.weight_calculate}        ");
            qry.Append($"\n , {model.tray_calculate}          ");
            qry.Append($"\n , '{model.updatetime}'            ");
            qry.Append($"\n , '{model.edit_user}'             ");
            qry.Append($"\n , {model.isMonth}                 ");
            qry.Append($"\n , '{model.show_sttdate}'          ");
            qry.Append($"\n , '{model.show_enddate}'          ");
            qry.Append($"\n , '{model.hide_date}'             ");

            qry.Append($"\n )                                 ");

            string sql = qry.ToString();

            return qry;
        }


        public StringBuilder InsertProduct(ProductOtherCostModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_product_other_cost (");
            qry.Append($"\n   group_name                           ");
            qry.Append($"\n , product                         ");
            qry.Append($"\n , origin                          ");
            qry.Append($"\n , sizes                           ");
            qry.Append($"\n , unit                            ");
            qry.Append($"\n , cost_unit                       ");
            qry.Append($"\n , custom                          ");
            qry.Append($"\n , incidental_expense              ");
            qry.Append($"\n , tax                             ");
            qry.Append($"\n , weight_calculate                ");
            qry.Append($"\n , tray_calculate                  ");
            qry.Append($"\n , updatetime                      ");
            qry.Append($"\n , edit_user                       ");
            qry.Append($"\n , production_days                 ");
            qry.Append($"\n , manager                         ");
            qry.Append($"\n , isMonth                         ");
            qry.Append($"\n , show_sttdate                    ");
            qry.Append($"\n , show_enddate                    ");
            qry.Append($"\n , hide_date                       ");
            qry.Append($"\n , purchase_margin                 ");
            qry.Append($"\n , base_around_month               ");
            qry.Append($"\n , remark                          ");
            qry.Append($"\n ) VALUES (                        ");
            qry.Append($"\n   '{model.group_name        }'              ");
            qry.Append($"\n , '{model.product           }'              ");
            qry.Append($"\n , '{model.origin            }'              ");
            qry.Append($"\n , '{model.sizes             }'              ");
            qry.Append($"\n , '{model.unit              }'              ");
            qry.Append($"\n , '{model.cost_unit         }'              ");
            qry.Append($"\n , {model.custom            }              ");
            qry.Append($"\n , {model.incidental_expense}              ");
            qry.Append($"\n , {model.tax               }              ");
            qry.Append($"\n , {model.weight_calculate  }              ");
            qry.Append($"\n , {model.tray_calculate    }              ");
            qry.Append($"\n , '{model.updatetime        }'              ");
            qry.Append($"\n , '{model.edit_user         }'              ");
            qry.Append($"\n , {model.production_days   }              ");
            qry.Append($"\n , '{model.manager           }'              ");
            qry.Append($"\n , {model.isMonth           }              ");
            if(model.show_sttdate == null)
                qry.Append($"\n , null              ");
            else
                qry.Append($"\n , {model.show_sttdate      }              ");

            if (model.show_enddate == null)
                qry.Append($"\n , null              ");
            else
                qry.Append($"\n , {model.show_enddate      }              ");

            if (model.hide_date == null)
                qry.Append($"\n , null              ");
            else
                qry.Append($"\n , {model.hide_date      }              ");

            qry.Append($"\n , {model.purchase_margin   }              ");
            qry.Append($"\n , {model.base_around_month }              ");
            qry.Append($"\n , '{model.remark}'                        ");
            qry.Append($"\n )                                         ");
            string sql = qry.ToString();

            return qry;
        }

        public StringBuilder DeleteProduct(ProductOtherCostModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_product_other_cost    ");
            qry.Append($"\n WHERE 1=1                           ");
            qry.Append($"\n AND product = '{model.product}'     ");
            qry.Append($"\n AND origin = '{model.origin}'       ");
            qry.Append($"\n AND sizes = '{model.sizes}'         ");
            qry.Append($"\n AND unit  = '{model.unit}'           ");

            return qry;
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

        public StringBuilder UpdateProductPurchaseMargin(string product, string origin, string sizes, string unit, double purchase_margin)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_product_other_cost  SET  ");
            qry.Append($"\n   purchase_margin = {purchase_margin}     ");
            qry.Append($"\n WHERE product = '{product}'     ");
            qry.Append($"\n AND origin = '{origin}'       ");
            qry.Append($"\n AND sizes = '{sizes}'         ");

            string sql = qry.ToString();
            return qry;
        }
    }
}

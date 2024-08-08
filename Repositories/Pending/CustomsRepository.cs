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
    public class CustomsRepository : ClassRoot, ICustomsRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public CustomsRepository()
        { }
        public StringBuilder UpdateShipmentSchdule(int id, string shipment_date, string etd, string eta)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_customs SET                                         ");
            qry.Append($"\n   shipment_date = '{shipment_date}'                          ");
            qry.Append($"\n , etd = '{etd}'                                              ");
            qry.Append($"\n , eta = '{eta}'                                              ");
            qry.Append($"\n WHERE id = {id}                                              ");

            return qry;
        }
        public DataTable GetMissingETDPendingList(DateTime standard_date, string ato_no, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT distinct                                                ");
            qry.Append($"\n id                                                             ");
            qry.Append($"\n , contract_year                                                ");
            qry.Append($"\n , ato_no                                                       ");
            qry.Append($"\n , contract_no                                                  ");
            qry.Append($"\n , bl_no                                                        ");
            qry.Append($"\n , shipment_date                                                ");
            qry.Append($"\n , etd                                                          ");
            qry.Append($"\n , eta                                                          ");
            qry.Append($"\n , manager                                                      ");
            qry.Append($"\n FROM t_customs                                                 ");
            qry.Append($"\n WHERE IFNULL(warehousing_date , '') = ''                       ");
            qry.Append($"\n   AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'    ");
            qry.Append($"\n   AND contract_year > 2022                                     ");
            qry.Append($"\n   AND shipment_date <= '{standard_date.ToString("yyyy-MM-dd")}'");
            qry.Append($"\n   AND IFNULL(etd, '') = ''                                     ");
            qry.Append($"\n   AND IFNULL(eta, '') = ''                                     ");
            if(!string.IsNullOrEmpty(ato_no))
                qry.Append($"\n   {commonRepository.whereSql("ato_no", ato_no)}                                     ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {commonRepository.whereSql("manager", manager)}                                     ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCollateralAvailableProduct()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                            ");
            qry.Append($"\ndistinct                                                          ");
            qry.Append($"\nato_no AS ato_no                                                 ");
            qry.Append($"\nFROM t_customs AS c                                               ");
            qry.Append($"\nWHERE 1=1                                                         ");
            qry.Append($"\n  AND c.payment_date_status = '결제완료'      ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%취소%' AND c.ato_no NOT LIKE '%삭제%'      ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%DW%'                                    ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%AD%'                                    ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%SJ%'                                    ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%JD%'                                    ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%HS%'                                    ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%OD%'                                    ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%EX%'                                    ");
            qry.Append($"\n  AND c.ato_no NOT LIKE '%YD%'                                    ");
            qry.Append($"\n  AND c.product NOT LIKE '%해물가스%'                                ");
            qry.Append($"\n  AND c.product NOT LIKE '%해물까스%'                                ");
            qry.Append($"\n  AND c.product NOT LIKE '%생선가스%'                                ");
            qry.Append($"\n  AND c.product NOT LIKE '%생선까스%'                                ");
            qry.Append($"\n  AND c.product NOT LIKE '%피바지락%'                                ");
            qry.Append($"\n  AND c.product NOT LIKE '%백합%'                                   ");
            qry.Append($"\n  AND c.product NOT LIKE '%자숙콩%'                                 ");
            qry.Append($"\n  AND c.product NOT LIKE '%파인샤베트%'                               ");
            qry.Append($"\n  AND c.product NOT LIKE '%새우젓%'                                 ");
            qry.Append($"\n  AND c.product NOT LIKE '%고노와다%'                                ");
            qry.Append($"\n  AND c.product NOT LIKE '%망고스틴%'                                ");
            qry.Append($"\n  AND c.product NOT LIKE '%번데기%'                                 ");
            qry.Append($"\n  AND c.product NOT LIKE '%연어알%'                                 ");
            qry.Append($"\n  AND c.product NOT LIKE '%냉동딸기%'                                ");
            qry.Append($"\n  AND (c.cc_status = '통관' OR c.cc_status = '확정')                 ");
            qry.Append($"\n  AND c.contract_year >= 2023                                      ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetShippingStockList()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                            ");
            qry.Append($"\n product                                                                                                                           ");
            qry.Append($"\n , origin                                                                                                                          ");
            qry.Append($"\n , sizes                                                                                                                           ");
            qry.Append($"\n , box_weight                                                                                                                      ");
            qry.Append($"\n , cost_unit                                                                                                                       ");
            qry.Append($"\n , SUM(IFNULL(quantity_on_paper, 0)) AS qty                                                                                        ");
            qry.Append($"\n FROM t_customs                                                                                                                    ");
            qry.Append($"\n WHERE cc_status != '확정' AND cc_status != '통관'                                                                                     ");
            qry.Append($"\n AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                                                                             ");
            qry.Append($"\n AND IFNULL(is_shipment_qty , 1) = 1                                                                                               ");
            qry.Append($"\n AND ((bl_no IS NULL OR bl_no = '') OR bl_no IS NOT NULL AND bl_no <> '' AND (warehousing_date IS NULL OR warehousing_date = ''))  ");
            qry.Append($"\n GROUP BY product , origin , sizes , box_weight , cost_unit                                                                        ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetDuplicateAtono(string contract_year, string ato_no, bool isDelete = false, string id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                     ");
            qry.Append($"\n    *                       ");
            qry.Append($"\n	FROM t_customs AS p        ");
            qry.Append($"\n WHERE 1=1                  ");
            if (!string.IsNullOrEmpty(contract_year))
                qry.Append($"\n  AND contract_year = {contract_year}                                                                     ");

            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n  AND (                                                                                                   ");
                qry.Append($"\n    ato_no = '{ato_no}'                                                                                   ");
                qry.Append($"\n    OR (ato_no LIKE '%{ato_no}%' AND (ato_no LIKE '%취소%' OR ato_no LIKE '%삭제%'))                      ");
                qry.Append($"\n  )                                                                                                       ");

            }

            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n  AND id <> {id}                                                                     ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetDashboard(string sttdate, string enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                                              ");
            qry.Append($"\n   product                                                                                                                                                                           ");
            qry.Append($"\n , origin                                                                                                                                                                            ");
            qry.Append($"\n , sizes                                                                                                                                                                             ");
            qry.Append($"\n , unit                                                                                                                                                                              ");
            qry.Append($"\n , cost_unit                                                                                                                                                                         ");
            qry.Append($"\n , IF(weight_calculate = 1, 'TRUE', 'FALSE') AS weight_calculate                                                                                                                     ");
            if(string.IsNullOrEmpty(unit))
                qry.Append($"\n , AVG(unit_price) AS unit_price                                                                                                                                                     ");
            else
                qry.Append($"\n , AVG(unit_price / unit * {unit}) AS unit_price                                                                                                                                                     ");
            qry.Append($"\n , eta                                                                                                                                                                               ");
            qry.Append($"\n FROM (                                                                                                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                                                                                                          ");
            qry.Append($"\n 	  product                                                                                                                                                                       ");
            qry.Append($"\n 	, origin                                                                                                                                                                        ");
            qry.Append($"\n 	, sizes                                                                                                                                                                         ");
            qry.Append($"\n 	, box_weight AS unit                                                                                                                                                            ");
            qry.Append($"\n 	, cost_unit                                                                                                                                                                     ");
            qry.Append($"\n 	, IFNULL(weight_calculate, 1) AS weight_calculate                                                                                                                               ");
            qry.Append($"\n 	, unit_price                                                                                                                                                                    ");
            qry.Append($"\n 	, IFNULL(delivery_days, 15) AS delivery_days                                                                                                                                    ");
            qry.Append($"\n 	, CASE WHEN DATE_FORMAT(eta,'%Y%m') IS NULL AND DATE_FORMAT(etd,'%Y%m') IS NULL THEN DATE_ADD(DATE_FORMAT(shipment_date,'%Y%m'), INTERVAL IFNULL(delivery_days, 15) DAY)        ");
            qry.Append($"\n 		WHEN DATE_FORMAT(eta,'%Y%m') IS NULL AND DATE_FORMAT(etd,'%Y%m') IS NOT NULL THEN DATE_ADD(DATE_FORMAT(etd,'%Y%m'), INTERVAL IFNULL(delivery_days, 15) DAY)                 ");
            qry.Append($"\n 		WHEN DATE_FORMAT(eta,'%Y%m') IS NOT NULL THEN DATE_FORMAT(eta,'%Y%m')                                                                                                       ");
            qry.Append($"\n 		ELSE DATE_FORMAT(eta,'%Y%m') END AS eta                                                                                                                                     ");
            qry.Append($"\n 	FROM t_customs AS p                                                                                                                                                             ");
            qry.Append($"\n 	LEFT OUTER JOIN t_country AS c                                                                                                                                                  ");
            qry.Append($"\n 	  ON p.origin = c.country_name                                                                                                                                                  ");
            qry.Append($"\n 	WHERE CASE WHEN DATE_FORMAT(eta,'%Y%m') IS NULL AND DATE_FORMAT(etd,'%Y%m') IS NULL THEN DATE_ADD(DATE_FORMAT(shipment_date,'%Y%m'), INTERVAL IFNULL(delivery_days, 15) DAY)    ");
            qry.Append($"\n 		WHEN DATE_FORMAT(eta,'%Y%m') IS NULL AND DATE_FORMAT(etd,'%Y%m') IS NOT NULL THEN DATE_ADD(DATE_FORMAT(etd,'%Y%m'), INTERVAL IFNULL(delivery_days, 15) DAY)                 ");
            qry.Append($"\n 		WHEN DATE_FORMAT(eta,'%Y%m') IS NOT NULL THEN DATE_FORMAT(eta,'%Y%m')                                                                                                       ");
            qry.Append($"\n 		ELSE DATE_FORMAT(eta,'%Y%m') END IS NOT NULL                                                                                                                                ");
            qry.Append($"\n     AND p.ato_no NOT LIKE '%삭제%' AND p.ato_no NOT LIKE '%취소%'                                                                                                                                ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n       AND product = '{product}'                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n       AND origin = '{origin}'                                                                     ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n       AND sizes = '{sizes}'                                                                     ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n       AND box_weight = '{unit}'                                                                     ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n       AND (                                                                     ");
                    for (int i = 0; i < products.Length; i++)
                    {
                        if (i == 0)
                            qry.Append($"\n  (");
                        else
                            qry.Append($"\n  OR (");
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            qry.Append($" product = '{sub[0]}'");
                            qry.Append($" AND origin = '{sub[1]}'");
                            qry.Append($" AND sizes = '{sub[2]}'");
                            qry.Append($" AND box_weight = '{sub[6]}'");
                            qry.Append($")");
                        }
                    }
                    qry.Append($"\n  )");
                } 
            }
            qry.Append($"\n ) AS t                                                                                                                                                                              ");
            qry.Append($"\n WHERE 1=1                                                                                                                                                                           ");
            qry.Append($"\n   AND eta >= '{sttdate}'                                                                                                                                                            ");
            qry.Append($"\n   AND eta <= '{enddate}'                                                                                                                                                            ");
            qry.Append($"\n GROUP BY product, origin, sizes, unit, cost_unit, weight_calculate, unit_price                                                                                                      ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPendingList2(string stt_contract_year, string end_contract_year
                                        , string ato_no, string contract_no, string shipper, string bl_no
                                        , string product, bool isExactly, string origin, string sizes, string box_weight, string manager, string cc_status)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                          ");
            qry.Append($"\n    c.*                                                                                                           ");
            qry.Append($"\n  , IFNULL(p.custom, 0) AS custom                                                                                 ");
            qry.Append($"\n  , IFNULL(p.tax, 0) AS tax                                                                                       ");
            qry.Append($"\n  , IFNULL(p.incidental_expense, 0) AS incidental_expense                                                         ");
            qry.Append($"\n  , IFNULL(p.production_days, 0) AS production_days                                                               ");
            qry.Append($"\n  FROM (                                                                                                          ");
            qry.Append($"\n 	 SELECT                                                                                                      ");
            qry.Append($"\n 	   c.*                                                                                                       ");
            qry.Append($"\n 	 , f.before_trq                                                                                              ");
            qry.Append($"\n 	 , f.after_trq                                                                                               ");
            qry.Append($"\n 	 FROM (                                                                                                      ");
            qry.Append($"\n 		 SELECT                                                                                                  ");
            qry.Append($"\n 		   c.id                                                                                                  ");
            qry.Append($"\n 		 , c.sub_id                                                                                              ");
            qry.Append($"\n 		 , c.contract_year                                                                                       ");
            qry.Append($"\n 		 , c.ato_no                                                                                              ");
            qry.Append($"\n 		 , c.pi_date                                                                                             ");
            qry.Append($"\n 		 , c.shipper                                                                                             ");
            qry.Append($"\n 		 , c.contract_no                                                                                         ");
            qry.Append($"\n 		 , c.bl_no                                                                                               ");
            qry.Append($"\n 		 , c.warehouse                                                                                           ");
            qry.Append($"\n 		 , c.cc_status                                                                                           ");
            qry.Append($"\n 		 , c.etd                                                                                                 ");
            qry.Append($"\n 		 , c.eta                                                                                                 ");
            qry.Append($"\n 		 , c.warehousing_date                                                                                    ");
            qry.Append($"\n 		 , c.product                                                                                             ");
            qry.Append($"\n 		 , c.origin                                                                                              ");
            qry.Append($"\n 		 , c.sizes                                                                                               ");
            qry.Append($"\n 		 , IF(IFNULL(c.weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate                            ");
            qry.Append($"\n 		 , c.box_weight                                                                                          ");
            qry.Append($"\n 		 , c.unit_price                                                                                          ");
            qry.Append($"\n 		 , IF(IFNULL(c.cost_unit, 0) = '', 0, IFNULL(c.cost_unit, 0)) AS cost_unit                               ");
            qry.Append($"\n 		 , c.quantity_on_paper                                                                                   ");
            qry.Append($"\n 		 , IF(IFNULL(s.payment_ex_rate, 0) = '', 0, IFNULL(s.payment_ex_rate, 0)) AS payment_ex_rate             ");
            qry.Append($"\n 		 , IF(IFNULL(s.shipping_ex_rate, 0) = '', 0, IFNULL(s.shipping_ex_rate, 0)) AS shipping_ex_rate             ");
            qry.Append($"\n 		 FROM t_customs AS c                                                                                     ");
            qry.Append($"\n 		 LEFT OUTER JOIN t_shipping AS s                                                                         ");
            qry.Append($"\n 		   ON c.id = s.custom_id                                                                                 ");
            qry.Append($"\n 		 WHERE 1=1                                                                                               ");
            qry.Append($"\n 		   AND c.product <> '기타비용'                                                                                               ");
            if (!string.IsNullOrEmpty(stt_contract_year))
                qry.Append($"\n   AND c.contract_year >= {stt_contract_year}                                                                      ");
            if (!string.IsNullOrEmpty(end_contract_year))
                qry.Append($"\n   AND c.contract_year <= {end_contract_year}                                                                      ");
            if (!string.IsNullOrEmpty(ato_no))
                qry.Append($"\n   {commonRepository.whereSql("c.ato_no", ato_no)}                                                                      ");
            if (!string.IsNullOrEmpty(contract_no))
                qry.Append($"\n   {commonRepository.whereSql("c.contract_no", contract_no)}                                                                      ");
            if (!string.IsNullOrEmpty(shipper))
                qry.Append($"\n   {commonRepository.whereSql("c.shipper", shipper)}                                                                      ");
            if (!string.IsNullOrEmpty(bl_no))
                qry.Append($"\n   {commonRepository.whereSql("c.bl_no", bl_no)}                                                                      ");
            if (isExactly)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND c.product LIKE '%{product}%'                                                                      ");
            }
            else
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   {commonRepository.whereSql("c.product", product)}                                                                      ");
            }
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("c.origin", origin)}                                                                      ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("c.sizes", sizes)}                                                                      ");
            if (!string.IsNullOrEmpty(box_weight))
                qry.Append($"\n   {commonRepository.whereSql("c.box_weight", box_weight)}                                                                      ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {commonRepository.whereSql("c.edit_user", manager)}                                                                      ");
            if (!string.IsNullOrEmpty(cc_status) && cc_status != "전체")
                qry.Append($"\n   AND c.cc_status = '{cc_status}'                                                                     ");
            qry.Append($"\n 	 ) AS c                                                                                                      ");
            qry.Append($"\n 	 LEFT OUTER JOIN (                                                                                           ");
            qry.Append($"\n 		 SELECT                                                                                                  ");
            qry.Append($"\n 		   id                                                                                                    ");
            qry.Append($"\n 		 , sub_id                                                                                                ");
            qry.Append($"\n 		 , before_trq                                                                                            ");
            qry.Append($"\n 		 , after_trq                                                                                             ");
            qry.Append($"\n 		 FROM t_fixed_cost                                                                                       ");
            qry.Append($"\n 		 WHERE 1=1                                                                                               ");
            if (!string.IsNullOrEmpty(stt_contract_year))
                qry.Append($"\n   AND contract_year >= {stt_contract_year}                                                                      ");
            if (!string.IsNullOrEmpty(end_contract_year))
                qry.Append($"\n   AND contract_year <= {end_contract_year}                                                                      ");
            if (!string.IsNullOrEmpty(ato_no))
                qry.Append($"\n   {commonRepository.whereSql("ato_no", ato_no)}                                                                      ");
            if (!string.IsNullOrEmpty(contract_no))
                qry.Append($"\n   {commonRepository.whereSql("contract_no", contract_no)}                                                                      ");
            if (!string.IsNullOrEmpty(shipper))
                qry.Append($"\n   {commonRepository.whereSql("shipper", shipper)}                                                                      ");
            if (!string.IsNullOrEmpty(bl_no))
                qry.Append($"\n   {commonRepository.whereSql("bl_no", bl_no)}                                                                      ");
            if (isExactly)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND product LIKE '%{product}%'                                                                      ");
            }
            else
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   {commonRepository.whereSql("product", product)}                                                                      ");
            }
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("origin", origin)}                                                                      ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("sizes", sizes)}                                                                      ");
            if (!string.IsNullOrEmpty(box_weight))
                qry.Append($"\n   {commonRepository.whereSql("box_weight", box_weight)}                                                                      ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {commonRepository.whereSql("edit_user", manager)}                                                                      ");
            qry.Append($"\n 	 ) AS f                                                                                                      ");
            qry.Append($"\n 	   ON c.id = f.id                                                                                            ");
            qry.Append($"\n 	   AND c.sub_id = f.sub_id                                                                                   ");
            qry.Append($"\n 	   AND c.cc_status = '확정'                                                                                    ");
            qry.Append($"\n  ) AS c                                                                                                          ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                               ");
            qry.Append($"\n 	SELECT                                                                                                       ");
            qry.Append($"\n       product                                                                                                    ");
            qry.Append($"\n 	, origin                                                                                                     ");
            qry.Append($"\n     , sizes                                                                                                      ");
            qry.Append($"\n     , unit                                                                                                       ");
            qry.Append($"\n     , custom                                                                                                     ");
            qry.Append($"\n     , tax                                                                                                        ");
            qry.Append($"\n     , incidental_expense                                                                                         ");
            qry.Append($"\n     , production_days                                                                                            ");
            qry.Append($"\n     FROM t_product_other_cost                                                                                    ");
            qry.Append($"\n     GROUP BY product, origin, sizes, unit, custom, tax, incidental_expense, production_days                      ");
            qry.Append($"\n  ) AS p                                                                                                          ");
            qry.Append($"\n    ON c.product = p.product                                                                                      ");
            qry.Append($"\n    AND c.origin = p.origin                                                                                       ");
            qry.Append($"\n    AND c.sizes = p.sizes                                                                                         ");
            qry.Append($"\n    AND c.box_weight = p.unit                                                                                     ");
            qry.Append($"\n  ORDER BY c.contract_year, c.warehousing_date DESC, c.eta DESC, c.etd DESC, c.cc_status, c.ato_no                                                                 ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetNotSeaoverProduct()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                               ");
            qry.Append($"\n  *                                                                                                                                   ");
            qry.Append($"\n FROM t_customs                                                                                                                       ");
            qry.Append($"\n WHERE contract_year > {DateTime.Now.Year - 2}                                                                                                           ");
            //qry.Append($"\n   AND (payment_date_status <> '결제완료' OR (payment_date_status = '결제완료' AND payment_date > date_add('{DateTime.Now.ToString("yyyy-MM-dd")}', INTERVAL 3 MONTH))) ");
            qry.Append($"\n   AND ato_no NOT LIKE '%취소%'            ");
            qry.Append($"\n   AND ato_no NOT LIKE '%삭제%'            ");
            qry.Append($"\n   AND payment_date_status <> '결제완료'   ");
            qry.Append($"\n   AND product <> '기타비용'   ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable CheckHOCO(string sttdate, string enddate)
       {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                                                        ");
            qry.Append($"\n   t.*                                                                                                                                                                                         ");
            qry.Append($"\n FROM(                                                                                                                                                                                         ");
            qry.Append($"\n 	SELECT                                                                                                                                                                                    ");
            qry.Append($"\n 	  c1.id                                                                                                                                                                                   ");
            qry.Append($"\n 	, c1.ato_no                                                                                                                                                                               ");
            qry.Append($"\n 	, CASE WHEN IFNULL(c1.eta,'') <> '' THEN IFNULL(c1.eta,'')                                                                                                                                ");
            qry.Append($"\n 		   WHEN IFNULL(c1.eta,'') = '' AND IFNULL(c1.etd,'') <> '' THEN DATE_ADD(IFNULL(c1.etd,''), INTERVAL IFNULL(c2.delivery_days, 15) DAY)                                                ");
            //qry.Append($"\n 		   WHEN IFNULL(c1.eta,'') = '' AND IFNULL(c1.etd,'') = '' AND IFNULL(c1.shipment_date,'') <> '' THEN DATE_ADD(IFNULL(c1.shipment_date,''), INTERVAL IFNULL(c2.delivery_days, 15) DAY) ");
            qry.Append($"\n 	  END AS eta                                                                                                                                                                              ");
            qry.Append($"\n 	, CASE WHEN IFNULL(c1.eta,'') <> '' THEN 90                                                                                                                                               ");
            qry.Append($"\n 		   WHEN IFNULL(c1.eta,'') = '' AND IFNULL(c1.etd,'') <> '' THEN 80                                                                                                                    ");
            //qry.Append($"\n 		   WHEN IFNULL(c1.eta,'') = '' AND IFNULL(c1.etd,'') = '' AND IFNULL(c1.shipment_date,'') <> '' THEN 70                                                                               ");
            qry.Append($"\n 	  END AS score                                                                                                                                                                            ");
            qry.Append($"\n 	, c1.sanitary_certificate                                                                                                                                                                 ");
            qry.Append($"\n 	, IFNULL(c2.delivery_days, 15) AS delivery_days                                                                                                                                           ");
            qry.Append($"\n 	FROM t_customs AS c1                                                                                                                                                                      ");
            qry.Append($"\n 	LEFT OUTER JOIN t_country AS c2                                                                                                                                                           ");
            qry.Append($"\n 	  ON c1.origin = c2.country_name                                                                                                                                                          ");
            qry.Append($"\n 	WHERE c1.sub_id = 1                                                                                                                                                                       ");
            qry.Append($"\n 	GROUP BY c1.id, c1.ato_no, c1.shipment_date, c1.etd, c1.eta, c1.sanitary_certificate                                                                                                      ");
            qry.Append($"\n ) AS t                                                                                                                                                                                        ");
            qry.Append($"\n WHERE t.eta <= '{enddate}' AND t.eta >= '{sttdate}'                                                                                                                                         ");
            qry.Append($"\n   AND (sanitary_certificate IS NULL OR sanitary_certificate = '')                                                                                               ");
            qry.Append($"\n   AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                                                                                                     ");
            qry.Append($"\n ORDER BY t.eta                                                                                               ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetGuarantee(string sttdate, string enddate, string ato_no, string bl_no, string lc_no)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                                                                                                   ");
            qry.Append($"\n  *                                                                                                                                                                      ");
            qry.Append($"\nFROM (                                                                                                                                                                   ");
            qry.Append($"\n  SELECT                                                                                                                                                                 ");
            qry.Append($"\n    c.id                                                                                                                                                                 ");
            qry.Append($"\n  , c.ato_no                                                                                                                                                             ");
            qry.Append($"\n  , c.shipment_date AS guarantee_date                                                                                                                                    ");
            qry.Append($"\n  , CASE                                                                                                                                                                 ");
            qry.Append($"\n  	WHEN IFNULL(c.etd, '') = '' AND IFNULL(c.shipment_date, '') <> '' THEN IFNULL(c.shipment_date, '')                                                                  ");
            qry.Append($"\n  	WHEN IFNULL(c.etd, '') <> '' AND IFNULL(c.shipment_date, '') = '' THEN IFNULL(c.etd, '')                                                                            ");
            qry.Append($"\n  	WHEN IFNULL(c.etd, '') = '' AND IFNULL(c.shipment_date, '') = '' AND IFNULL(c.eta, '') <> '' THEN DATE_ADD(c.eta, INTERVAL -IFNULL(t.delivery_days, 15) DAY)        ");
            qry.Append($"\n  	ELSE IFNULL(c.etd, '')                                                                                                                                              ");
            qry.Append($"\n    END AS etd                                                                                                                                                           ");
            qry.Append($"\n  , CASE                                                                                                                                                                 ");
            qry.Append($"\n  	WHEN IFNULL(c.eta, '') <> '' THEN IFNULL(c.eta, '')                                                                                                                 ");
            qry.Append($"\n  	WHEN IFNULL(c.eta, '') = '' AND IFNULL(c.etd, '') <> '' THEN DATE_ADD(c.etd, INTERVAL IFNULL(t.delivery_days, 15) DAY)                                              ");
            qry.Append($"\n  	WHEN IFNULL(c.eta, '') = '' AND IFNULL(c.shipment_date, '') <> '' THEN DATE_ADD(c.shipment_date, INTERVAL IFNULL(t.delivery_days, 15) DAY)                          ");
            qry.Append($"\n    END AS eta                                                                                                                                                           ");
            qry.Append($"\n  , CASE                                                                                                                                                                 ");
            qry.Append($"\n  	WHEN IFNULL(c.eta, '') <> '' THEN 1                                                                                                                 ");
            qry.Append($"\n  	WHEN IFNULL(c.eta, '') = '' AND IFNULL(c.etd, '') <> '' THEN 2                                              ");
            qry.Append($"\n  	WHEN IFNULL(c.eta, '') = '' AND IFNULL(c.shipment_date, '') <> '' THEN 2                          ");
            qry.Append($"\n    END AS eta_mode                                                                                                                                                           ");
            qry.Append($"\n  , c.total_amount                                                                                                                                                       ");
            qry.Append($"\n  , c.usance_type                                                                                                                                                        ");
            qry.Append($"\n  FROM (                                                                                                                                                                 ");
            qry.Append($"\n  	SELECT                                                                                                                                                              ");
            qry.Append($"\n  		c.id, c.ato_no, c.etd, c.eta, c.shipment_date, SUM(c.total_amount) AS total_amount, c.usance_type                                                               ");
            qry.Append($"\n  	FROM (                                                                                                                                                              ");
            qry.Append($"\n  		SELECT                                                                                                                                                          ");
            qry.Append($"\n  		  id                                                                                                                                                            ");
            qry.Append($"\n  		, ato_no                                                                                                                                                        ");
            qry.Append($"\n  		, etd                                                                                                                                                           ");
            qry.Append($"\n  		, eta                                                                                                                                                           ");
            qry.Append($"\n  		, shipment_date                                                                                                                                                 ");
            qry.Append($"\n  		, CASE                                                                                                                                                          ");
            qry.Append($"\n  			WHEN IFNULL(cost_unit, 0) > 0 THEN unit_price * ( cost_unit * quantity_on_paper)                                                                            ");
            qry.Append($"\n  			ELSE unit_price * ( box_weight * quantity_on_paper) END AS total_amount                                                                                     ");
            qry.Append($"\n  		, usance_type                                                                                                                                                   ");
            qry.Append($"\n  		FROM t_customs                                                                                                                                                  ");
            qry.Append($"\n  		WHERE (cc_status = '확정' OR cc_status = '결제완료')                                                                                                                 ");
            qry.Append($"\n  		  AND (agency_type = 'X' OR agency_type IS NULL OR agency_type = '')                                                                                            ");
            qry.Append($"\n  	) AS c                                                                                                                                                              ");
            qry.Append($"\n  	GROUP BY c.id, c.ato_no, c.etd, c.eta, c.shipment_date, c.usance_type                                                                                               ");
            qry.Append($"\n  ) AS c                                                                                                                                                                 ");
            qry.Append($"\n  INNER JOIN (                                                                                                                                                           ");
            qry.Append($"\n  	SELECT                                                                                                                                                              ");
            qry.Append($"\n  	  c.id                                                                                                                                                              ");
            qry.Append($"\n  	, c.origin                                                                                                                                                          ");
            qry.Append($"\n  	, IFNULL(t.delivery_days, 15) AS delivery_days                                                                                                                      ");
            qry.Append($"\n  	FROM t_customs AS c                                                                                                                                                 ");
            qry.Append($"\n  	LEFT OUTER JOIN t_country AS t                                                                                                                                      ");
            qry.Append($"\n  	  ON c.origin = t.country_name                                                                                                                                      ");
            qry.Append($"\n  	WHERE c.sub_id = 1                                                                                                                                                  ");
            qry.Append($"\n  ) AS t                                                                                                                                                                 ");
            qry.Append($"\n    ON c.id = t.id                                                                                                                                                       ");
            qry.Append($"\n  WHERE 1 = 1                                                                                                                                                            ");
            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n    AND ato_no LIKE '%{ato_no}%'                                                                                                                                                  ");
            }
            if (!string.IsNullOrEmpty(bl_no))
            {
                qry.Append($"\n    AND bl_no LIKE '%{bl_no}%'                                                                                                                                                  ");
            }
            if (!string.IsNullOrEmpty(lc_no))
            {
                qry.Append($"\n    AND lc_payment_date LIKE '%{lc_no}%'                                                                                                                                                  ");
            }

            qry.Append($"\n) AS t                                                                                                                                                                   ");
            qry.Append($"\nWHERE date_add(t.eta, INTERVAL 1 DAY) IS NOT NULL                                                                                                                        ");
            qry.Append($"\n  AND t.eta >= '{sttdate}'                                                                                                                                               ");
            qry.Append($"\n  ORDER BY  t.eta                                                                                                                                                        ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetArrivalSchedule(string sttYear, string endYear, string division, string bl_no, string shipping_company
                                            , string bl_status, string ato_no, string broker, string warehouse, string agency_type
                                            , int type)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                            ");
            qry.Append($"\n  c.id                                                                           ");
            qry.Append($"\n, c.division                                                                     ");
            qry.Append($"\n, c.bl_no                                                                        ");
            qry.Append($"\n, a.forwarder                                                                    ");
            qry.Append($"\n, c.etd                                                                          ");
            qry.Append($"\n, c.eta                                                                          ");
            qry.Append($"\n, a.bl_status                                                                    ");
            qry.Append($"\n, c.product                                                                      ");
            qry.Append($"\n, c.cnt                                                        ");
            qry.Append($"\n, c.product_group                                                        ");
            qry.Append($"\n, c.ato_no                                                                       ");
            qry.Append($"\n, c.broker                                                                       ");
            qry.Append($"\n, c.warehouse                                                                    ");
            qry.Append($"\n, c.warehousing_date                                                             ");
            qry.Append($"\n, c.is_warehousing                                                               ");
            qry.Append($"\n, c.agency                                                                       ");
            qry.Append($"\n, a.remark                                                                       ");
            qry.Append($"\n, c.sanitary_certificate                                                         ");
            qry.Append($"\n, c.is_quarantine                                                                ");
            qry.Append($"\n, a.quarantine_type                                                              ");
            qry.Append($"\n, a.result_estimated_date                                                        ");
            qry.Append($"\nFROM(                                                                             ");
            qry.Append($"\nSELECT                                                                            ");
            qry.Append($"\n  c1.id                                                                           ");
            qry.Append($"\n, c1.division                                                                     ");
            qry.Append($"\n, c1.bl_no                                                                        ");
            qry.Append($"\n, c1.etd                                                                          ");
            qry.Append($"\n, c1.eta                                                                          ");
            qry.Append($"\n, c2.product                                                                      ");
            qry.Append($"\n, c2.cnt                                                        ");
            qry.Append($"\n, c2.product_group                                                        ");
            qry.Append($"\n, c1.ato_no                                                                       ");
            qry.Append($"\n, c1.broker                                                                       ");
            qry.Append($"\n, c1.warehouse                                                                    ");
            qry.Append($"\n, c1.warehousing_date                                                             ");
            qry.Append($"\n, c1.is_warehousing                                                               ");
            qry.Append($"\n, c1.agency_type AS agency                                                        ");
            qry.Append($"\n, c1.is_quarantine AS is_quarantine                                               ");
            qry.Append($"\n, c2.sanitary_certificate                                                         ");
            qry.Append($"\nFROM(                                                                             ");
            qry.Append($"\n	   SELECT                                                                           ");
            qry.Append($"\n	     id                                                                             ");
            qry.Append($"\n	   , division                                                                       ");
            qry.Append($"\n	   , bl_no                                                                          ");
            qry.Append($"\n	   , etd                                                                            ");
            qry.Append($"\n	   , eta                                                                            ");
            qry.Append($"\n	   , ato_no                                                                         ");
            qry.Append($"\n	   , broker                                                                         ");
            qry.Append($"\n	   , warehouse                                                                      ");
            qry.Append($"\n	   , IFNULL(warehousing_date, '') AS warehousing_date                                                                                   ");
            qry.Append($"\n	   , CASE WHEN IFNULL(warehousing_date, '') = \"\" THEN FALSE WHEN warehousing_date > DATE_FORMAT(NOW(), '%Y-%m-%d') THEN FALSE ELSE TRUE END AS is_warehousing                                  ");
            qry.Append($"\n	   , agency_type                                                                    ");
            qry.Append($"\n	   , IFNULL(is_quarantine, FALSE) AS is_quarantine                                  ");
            qry.Append($"\n	   FROM t_customs                                                                   ");
            qry.Append($"\n    WHERE 1=1                                                                        ");
            qry.Append($"\n      AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                      ");
            qry.Append($"\n      AND cc_status <> '확정'                                                        ");
            qry.Append($"\n      AND cc_status <> '통관'                                                        ");
            qry.Append($"\n	     AND bl_no IS NOT NULL AND  bl_no <> ''                                         ");
            //qry.Append($"\n      AND (warehousing_date IS NULL OR warehousing_date = '' OR warehousing_date > NOW() OR DATE_FORMAT(warehousing_date,'%Y-%m-%d') IS NULL)   ");
            //qry.Append($"\n      AND (warehousing_date IS NULL OR warehousing_date = '')   ");
            qry.Append($"\n      AND contract_year >= {sttYear} AND contract_year <= {endYear}                  ");
            qry.Append($"\n      AND sub_id = 1                                                              ");
            //qry.Append($"\n	   GROUP BY id, division, bl_no, etd, eta, ato_no, broker, warehouse, agency_type   ");
            qry.Append($"\n) AS c1                                                                           ");
            qry.Append($"\nINNER JOIN (                                                                      ");
            qry.Append($"\n	SELECT                                                                           ");
            qry.Append($"\n	  c1.id                                                                          ");
            qry.Append($"\n	, CASE WHEN c2.cnt = 1 THEN c1.product                                           ");
            qry.Append($"\n		   WHEN c2.cnt > 1 THEN CONCAT(c1.product, ' 등 ', c2.cnt, '개')             ");
            qry.Append($"\n	  END AS product                                                                 ");
            qry.Append($"\n	, c2.cnt                                                        ");
            qry.Append($"\n	, c2.product_group                                                        ");
            qry.Append($"\n	, c1.sanitary_certificate                                                        ");
            qry.Append($"\n	FROM(                                                                            ");
            qry.Append($"\n		SELECT                                                                       ");
            qry.Append($"\n		  id                                                                         ");
            qry.Append($"\n		, product                                                                    ");
            qry.Append($"\n	   , sanitary_certificate                                                           ");
            qry.Append($"\n		FROM t_customs                                                               ");
            qry.Append($"\n		WHERE sub_id = 1                                                             ");
            qry.Append($"\n      AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                      ");
            qry.Append($"\n      AND cc_status <> '확정'                                                        ");
            qry.Append($"\n      AND cc_status <> '통관'                                                        ");
            qry.Append($"\n	     AND bl_no IS NOT NULL AND  bl_no <> ''                                         ");
            //qry.Append($"\n      AND (warehousing_date IS NULL OR warehousing_date = '' OR warehousing_date > NOW() OR DATE_FORMAT(warehousing_date,'%Y-%m-%d') IS NULL)   ");
            //qry.Append($"\n      AND (warehousing_date IS NULL OR warehousing_date = '')   ");
            qry.Append($"\n      AND contract_year >= {sttYear} AND contract_year <= {endYear}                  ");
            qry.Append($"\n	) AS c1                                                                          ");
            qry.Append($"\n	INNER JOIN(                                                                      ");
            qry.Append($"\n		SELECT                                                                       ");
            qry.Append($"\n		  id                                                                         ");
            qry.Append($"\n		, COUNT(*) AS cnt                                                            ");
            qry.Append($"\n		, group_concat(concat(product, ' | ' , origin, ' | ', sizes, ' | ', box_weight)) AS product_group                                                            ");
            qry.Append($"\n		FROM t_customs                                                               ");
            qry.Append($"\n		GROUP BY id                                                                  ");
            qry.Append($"\n	) AS c2                                                                          ");
            qry.Append($"\n	  ON c1.id = c2.id                                                               ");
            qry.Append($"\n) AS c2                                                                           ");
            qry.Append($"\n  ON c1.id = c2.id                                                                ");
            qry.Append($"\nWHERE 1=1                                                                         ");
            

            if (!string.IsNullOrEmpty(division))
            {
                qry.Append($"\n  AND c1.division Like '%{division}%'                                             ");
            }
            if (!string.IsNullOrEmpty(bl_no))
            {
                qry.Append($"\n  AND c1.bl_no Like '%{bl_no}%'                                             ");
            }
            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n  AND c1.ato_no Like '%{ato_no}%'                                             ");
            }
            if (!string.IsNullOrEmpty(broker))
            {
                qry.Append($"\n  AND c1.broker Like '%{broker}%'                                             ");
            }
            if (!string.IsNullOrEmpty(agency_type))
            {
                qry.Append($"\n  AND c1.agency_type Like '%{agency_type}%'                                             ");
            }
            if (!string.IsNullOrEmpty(warehouse))
            {
                qry.Append($"\n  AND c1.warehouse Like '%{warehouse}%'                                             ");
            }
            qry.Append($"\n) AS c                                                ");
            qry.Append($"\nLEFT OUTER JOIN t_arrival AS a                        ");
            qry.Append($"\n  ON c.id = a.id                                      ");
            qry.Append($"\nWHERE 1=1                                             ");

            qry.Append($"\n  AND c.is_quarantine = FALSE                                             ");
            //입고전
            if (type == 1)
                qry.Append($"\n  AND (a.receipt_document IS NULL OR a.receipt_document ='')                    ");
            //입고후
            else if (type == 2)
            {
                qry.Append($"\n  AND (a.receipt_document IS NULL OR a.receipt_document ='O')                     ");
                qry.Append($"\n  AND c.is_warehousing = FALSE                                                     ");
            }
            //검역전
            else if (type == 3)
            {
                qry.Append($"\n  AND (a.receipt_document IS NULL OR a.receipt_document ='O')                     ");
                qry.Append($"\n  AND c.is_warehousing = TRUE                                                     ");
            }


            if (!string.IsNullOrEmpty(shipping_company))
            {
                qry.Append($"\n  AND a.formarder Like '%{shipping_company}%'                                             ");
            }
            if (!string.IsNullOrEmpty(bl_status))
            {
                qry.Append($"\n  AND a.bl_status Like '%{bl_status}%'                                             ");
            }
            qry.Append($"\n ORDER BY c.eta, c.id, c.ato_no                              ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);


            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable SelectProduct(string product = null, string origin = null, string sizes = null, string unit = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                ");
            qry.Append($"\n   IFNULL(origin, '') AS origin                                                                        ");
            qry.Append($"\n , IFNULL(product, '') AS product                                                                      ");
            qry.Append($"\n , IFNULL(sizes, '') AS sizes                                                                          ");
            qry.Append($"\n , IFNULL(box_weight, '') AS box_weight                                                                ");
            qry.Append($"\n FROM t_customs                                                                                        ");
            qry.Append($"\n WHERE 1=1                                                                                             ");
            qry.Append($"\n   AND product = '{product}'                                                                           ");
            qry.Append($"\n   AND origin = '{origin}'                                                                             ");
            qry.Append($"\n   AND sizes = '{sizes}'                                                                               ");
            qry.Append($"\n   AND box_weight = '{unit}'                                                                           ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductTable(string product = null, string origin = null, string sizes = null, string unit = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                ");
            qry.Append($"\n   IFNULL(origin, '') AS origin                                                                        ");
            qry.Append($"\n , IFNULL(product, '') AS product                                                                      ");
            qry.Append($"\n , IFNULL(sizes, '') AS sizes                                                                          ");
            qry.Append($"\n , IFNULL(box_weight, '') AS box_weight                                                                ");
            qry.Append($"\n FROM t_customs                                                                                        ");
            qry.Append($"\n WHERE 1=1                                                                                             ");

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
                            tempWhr = $"\n	   product  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR product   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND product   LIKE '%{product}%'                                                                         ");
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
                            tempWhr = $"\n	   origin   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR origin   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND origin   LIKE '%{origin}%'                                                                         ");
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
                            tempWhr = $"\n	   sizes   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR sizes   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND sizes   LIKE '%{sizes}%'                                                                         ");
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
                            tempWhr = $"\n	   box_weight   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR box_weight   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND box_weight   LIKE '%{unit}%'                                                                         ");
                }
            }

            qry.Append($"\n GROUP BY origin, product, sizes, box_weight");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public List<CustomsModel> GetAll(DateTime dt, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                                                         ");
            qry.Append($"\n   id                                                                                                         ");
            qry.Append($"\n , ato_no                                                                                                     ");
            qry.Append($"\n , payment_date                                                                                               ");
            qry.Append($"\n , payment_date_status                                                                                        ");
            qry.Append($"\n , payment_bank                                                                                               ");
            qry.Append($"\n , SUM(unit_price * (box_weight*quantity_on_paper)) AS total_amount                                           ");
            qry.Append($"\n , manager                                                                                                    ");
            qry.Append($"\n  FROM t_customs                                                                                              ");
            qry.Append($"\n  WHERE 1=1                                                                                                   ");
            qry.Append($"\n  AND payment_date = '{dt.ToString("yyyy-MM-dd")}'                                                            ");
            qry.Append($"\n GROUP BY id, pi_date, shipper, manager                                                                       ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetRegSalesInfoModel(dr);
        }

        private List<CustomsModel> GetMonthTotalModel(MySqlDataReader rd)
        {
            List<CustomsModel> CustomsModelList = new List<CustomsModel>();
            while (rd.Read())
            {
                CustomsModel model = new CustomsModel();
                model.payment_date = rd["payment_date"].ToString();
                model.payment_bank = rd["currency"].ToString();
                model.total_amount = double.Parse(rd["total_amount"].ToString());
                model.currency = rd["currency"].ToString();
                CustomsModelList.Add(model); ;
            }
            rd.Close();
            return CustomsModelList;
        }

        private List<CustomsModel> GetRegSalesInfoModel(MySqlDataReader rd)
        {
            List<CustomsModel> CustomsModelList = new List<CustomsModel>();
            while (rd.Read())
            {
                CustomsModel model = new CustomsModel();
                model.id = int.Parse(rd["Id"].ToString());
                model.ato_no = rd["ato_no"].ToString();
                model.payment_bank = rd["payment_bank"].ToString();
                model.payment_date = rd["payment_date"].ToString();
                model.payment_date_status = rd["payment_date_status"].ToString();
                model.total_amount = double.Parse(rd["total_amount"].ToString());
                model.product = rd["product"].ToString();
                model.manager = rd["manager"].ToString();
                model.usance_type = rd["usance_type"].ToString();
                model.division = rd["division"].ToString();
                model.payment_type = rd["payment_type"].ToString();
                CustomsModelList.Add(model); ;
            }
            rd.Close();
            return CustomsModelList;
        }

        private List<CustomsModel> GetUnkownPendingModel(MySqlDataReader rd)
        {
            List<CustomsModel> CustomsModelList = new List<CustomsModel>();
            while (rd.Read())
            {
                CustomsModel model = new CustomsModel();
                model.id = int.Parse(rd["Id"].ToString());
                model.ato_no = rd["ato_no"].ToString();
                model.payment_bank = rd["payment_bank"].ToString();
                model.payment_date = rd["payment_date"].ToString();
                model.payment_date_status = rd["payment_date_status"].ToString();
                model.total_amount = double.Parse(rd["total_amount"].ToString());
                model.product = rd["product"].ToString();
                model.manager = rd["manager"].ToString();
                model.usance_type = rd["usance_type"].ToString();
                model.division = rd["division"].ToString();
                model.accuracy = rd["accuracy"].ToString();
                CustomsModelList.Add(model); ;
            }
            rd.Close();
            return CustomsModelList;
        }

        public List<CustomsSimpleModel> GetCustom(int id, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   id                      ");
            qry.Append($"\n , sub_id                  ");
            qry.Append($"\n , contract_year           ");
            qry.Append($"\n , ato_no                  ");
            qry.Append($"\n , pi_date                 ");
            qry.Append($"\n , contract_no             ");
            qry.Append($"\n , shipper                 ");
            qry.Append($"\n , lc_open_date            ");
            qry.Append($"\n , lc_payment_date         ");
            qry.Append($"\n , bl_no                   ");
            qry.Append($"\n , shipment_date           ");
            qry.Append($"\n , etd                     ");
            qry.Append($"\n , eta                     ");
            qry.Append($"\n , warehousing_date        ");
            qry.Append($"\n , pending_check           ");
            qry.Append($"\n , warehouse               ");
            qry.Append($"\n , cc_status               ");
            qry.Append($"\n , origin                  ");
            qry.Append($"\n , product                 ");
            qry.Append($"\n , sizes                   ");
            qry.Append($"\n , box_weight              ");
            qry.Append($"\n , unit_price              ");
            qry.Append($"\n , quantity_on_paper       ");
            qry.Append($"\n , qty                     ");
            qry.Append($"\n , (quantity_on_paper*box_weight)  AS custom_weight          ");
            qry.Append($"\n , manager                 ");
            qry.Append($"\n , edit_user               ");
            qry.Append($"\n , updatetime              ");
            qry.Append($"\n , (tariff_rate*100)  AS tariff_rate         ");
            qry.Append($"\n , (vat_rate*100)   AS vat_rate           ");
            qry.Append($"\n , IF(loading_cost_per_box IS NULL,0,loading_cost_per_box) AS loading_cost_per_box  ");
            qry.Append($"\n , IF(refrigeration_charge IS NULL,0,refrigeration_charge) AS refrigeration_charge  ");
            qry.Append($"\n , IF(total_amount_seaover IS NULL,0,total_amount_seaover) AS total_amount_seaover  ");
            qry.Append($"\n , remark                  ");
            qry.Append($"\n , trq_amount              ");
            qry.Append($"\n , payment_date            ");
            qry.Append($"\n , payment_date_status     ");
            qry.Append($"\n , payment_bank            ");
            qry.Append($"\n , usance_type             ");
            qry.Append($"\n , agency_type             ");
            qry.Append($"\n , division                ");
            qry.Append($"FROM t_customs               ");
            qry.Append($"WHERE id ={id}               ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetCustomsModel(dr);
        }


        public CustomsTitleModel GetCustomTitle(int id, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   contract_year           ");
            qry.Append($"\n , ato_no                  ");
            qry.Append($"\n , contract_no             ");
            qry.Append($"\n , shipper                 ");
            qry.Append($"\n , SUM(quantity_on_paper*box_weight)               AS total_weight     ");
            qry.Append($"\n , SUM(unit_price*(quantity_on_paper*box_weight))  AS total_amount     ");
            qry.Append($"\n , IF(cc_status IS NULL, '', cc_status) AS cc_status                   ");
            qry.Append($"\n , IF(usance_type IS NULL, '', usance_type) AS usance_type             ");
            qry.Append($"\n , IF(agency_type IS NULL, '', agency_type) AS agency_type             ");
            qry.Append($"\n , IF(division IS NULL, '', division) AS division                      ");
            qry.Append($"\n , manager                 ");
            qry.Append($"\n , updatetime              ");
            qry.Append($"FROM t_customs               ");
            qry.Append($"WHERE id ={id}               ");
            qry.Append($"GROUP BY contract_year, ato_no, contract_no, shipper, manager, updatetime, cc_status, usance_type, division");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetCustomsTitleModel(dr);
        }

        private CustomsTitleModel GetCustomsTitleModel(MySqlDataReader rd)
        {
            CustomsTitleModel model = new CustomsTitleModel();
            while (rd.Read())
            {
                model.ato_no = rd["ato_no"].ToString();
                model.contract_year = Convert.ToInt32(rd["contract_year"]);
                model.contract_no = rd["contract_no"].ToString();
                model.shipper = rd["shipper"].ToString();
                model.total_amount = double.Parse(rd["total_amount"].ToString());
                model.total_weight = double.Parse(rd["total_weight"].ToString());
                model.cc_status = rd["cc_status"].ToString();
                model.usance_type = rd["usance_type"].ToString();
                model.agency_type = rd["agency_type"].ToString();
                model.division = rd["division"].ToString();
                model.manager = rd["manager"].ToString();
                model.updatetime = rd["updatetime"].ToString();
            }
            rd.Close();
            return model;
        }

        public List<CustomsModel> GetCustomMonthTotal(DateTime sttdate, DateTime enddate, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                  ");
            qry.Append($"\n   payment_date                                                       ");
            qry.Append($"\n , currency                                                           ");
            qry.Append($"\n , SUM(total_amount) AS total_amount                                  ");
            qry.Append($"\n FROM(                                                                ");
            qry.Append($"\n SELECT                                                               ");
            qry.Append($"\n   day(payment_date) AS payment_date                                  ");
            qry.Append($"\n , 'USD' AS currency                                                  ");
            qry.Append($"\n , SUM(unit_price * (box_weight*quantity_on_paper)) AS total_amount   ");
            qry.Append($"\n FROM t_customs                                                       ");
            qry.Append($"\n WHERE 1=1                                                            ");
            qry.Append($"\n  AND payment_date >= '{sttdate.ToString("yyyy-MM-dd")}'              ");
            qry.Append($"\n  AND payment_date <= '{enddate.ToString("yyyy-MM-dd")}'              ");
            qry.Append($"\n GROUP BY payment_date                                                ");
            qry.Append($"\n UNION ALL                                                            ");
            qry.Append($"\n SELECT                                                               ");
            qry.Append($"\n   sday AS payment_date                                               ");
            qry.Append($"\n , currency                                                           ");
            qry.Append($"\n , pay_amount AS total_amount                                         ");
            qry.Append($"\n FROM t_memo                                                          ");
            qry.Append($"\n WHERE 1=1                                                            ");
            qry.Append($"\n   AND syear = {sttdate.ToString("yyyy")}                             ");
            qry.Append($"\n   AND smonth = {sttdate.ToString("MM")}                              ");
            qry.Append($"\n ) AS m                                                               ");
            qry.Append($"\n WHERE m.total_amount > 0                                             ");
            qry.Append($"\n GROUP BY m.payment_date, m.currency                                  ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetMonthTotalModel(dr);
        }


        public List<CustomsModel> GetCustomMonth(DateTime sttdate, DateTime enddate, string ato_no, string bl_no, string lc_no, string product, string origin, string sizes, string manager, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                     ");
            qry.Append($"\n  *                                                                                                                                         ");
            qry.Append($"\n FROM (                                                                                                                                     ");
            qry.Append($"\n SELECT                                                                                                                                     ");
            qry.Append($"\n   c.id                                                                                                                                     ");
            qry.Append($"\n , c.ato_no                                                                                                                                 ");
            qry.Append($"\n , c.payment_date                                                                                                                           ");
            qry.Append($"\n , c.payment_date_status                                                                                                                    ");
            qry.Append($"\n , c.payment_bank                                                                                                                           ");
            qry.Append($"\n , c.total_amount - IFNULL(p.payment_amount, 0) AS total_amount                                                                             ");
            qry.Append($"\n , c.manager                                                                                                                                ");
            qry.Append($"\n , c.usance_type                                                                                                                            ");
            qry.Append($"\n , c.division                                                                                                                               ");
            qry.Append($"\n , c.product                                                                                                                                ");
            qry.Append($"\n , c.payment_type                                                                                                                                ");
            qry.Append($"\n FROM (                                                                                                                                     ");
            qry.Append($"\n 	SELECT                                                                                                                                 ");
            qry.Append($"\n       c.id                                                                                                                                 ");
            qry.Append($"\n 	, c.ato_no                                                                                                                             ");
            qry.Append($"\n 	, c.payment_date                                                                                                                       ");
            qry.Append($"\n 	, c.payment_date_status                                                                                                                ");
            qry.Append($"\n 	, c.payment_bank                                                                                                                       ");
            qry.Append($"\n 	, SUM(c.total_amount) AS total_amount                                                                                                  ");
            qry.Append($"\n 	, c.manager                                                                                                                            ");
            qry.Append($"\n 	, c.usance_type                                                                                                                        ");
            qry.Append($"\n 	, c.division                                                                                                                           ");
            qry.Append($"\n     , GROUP_CONCAT(c.product) AS product                                                                                                   ");
            qry.Append($"\n     , 0 AS payment_type                                                                                                                    ");
            qry.Append($"\n     FROM (                                                                                                                                 ");
            qry.Append($"\n 		SELECT                                                                                                                             ");
            qry.Append($"\n 		  c.id                                                                                                                             ");
            qry.Append($"\n 		, c.ato_no                                                                                                                         ");
            qry.Append($"\n 		, c.payment_date                                                                                                                   ");
            qry.Append($"\n 		, c.payment_date_status                                                                                                            ");
            qry.Append($"\n 		, c.payment_bank                                                                                                                   ");
            qry.Append($"\n 		,  CASE WHEN IFNULL(weight_calculate, 1) = 0 AND IFNULL(c.cost_unit, 0) > 0 THEN c.unit_price * (c.cost_unit*c.quantity_on_paper)            ");
            qry.Append($"\n 			 ELSE c.unit_price * (c.box_weight*c.quantity_on_paper) END AS total_amount                                                    ");
            qry.Append($"\n 		, c.manager                                                                                                                        ");
            qry.Append($"\n 		, c.usance_type                                                                                                                    ");
            qry.Append($"\n 		, c.division                                                                                                                       ");
            qry.Append($"\n 		, CONCAT('	', c.product, ' | ', c.origin, ' | ', c.sizes, ' | ', c.box_weight, ' : ', c.quantity_on_paper) AS product             ");
            qry.Append($"\n 		FROM t_customs AS c                                                                                                                ");
            qry.Append($"\n 		WHERE 1=1                                                                                                                          ");
            qry.Append($"\n 		 AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                                                                         ");
            qry.Append($"\n 		 AND IFNULL(c.is_calendar, 1)  = 1                                                                                                 ");
            qry.Append($"\n 		 AND c.payment_date_status <> ''                                                                                                   ");
            qry.Append($"\n 		 AND c.payment_date_status IS NOT NULL                                                                                             ");
            qry.Append($"\n 		 AND c.payment_date_status <> '미확정'                                                                                                ");
            qry.Append($"\n          AND c.payment_date >= '{sttdate.ToString("yyyy-MM-dd")}'                                                      ");
            qry.Append($"\n          AND c.payment_date <= '{enddate.ToString("yyyy-MM-dd")}'                                                      ");
            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n  AND c.ato_no LIKE '%{ato_no}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(bl_no))
            {
                qry.Append($"\n  AND c.bl_no LIKE '%{bl_no}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(lc_no))
            {
                qry.Append($"\n  AND c.lc_payment_date LIKE '%{lc_no}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n  AND c.product LIKE '%{product}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n  AND c.sizes LIKE '%{sizes}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n  AND c.origin LIKE '%{origin}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n  AND c.manager LIKE '%{manager}%'                                                      ");
            }
            qry.Append($"\n 	) AS c                                                                                                                                 ");
            qry.Append($"\n     GROUP BY c.id, c.ato_no, c.payment_date, c.payment_date_status, c.payment_bank, c.manager, c.usance_type, c.division                   ");
            qry.Append($"\n ) AS c                                                                                                                                     ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                          ");
            qry.Append($"\n   SELECT contract_id, division, SUM(payment_amount) AS payment_amount FROM t_payment                                                       ");
            //qry.Append($"\n   WHERE payment_date_status <> '결제완료'                                                                                                  ");
            qry.Append($"\n   GROUP BY contract_id, division                                                                                                           ");
            qry.Append($"\n ) AS p                                                                                                                                     ");
            qry.Append($"\n   ON c.id = p.contract_id                                                                                                                  ");
            qry.Append($"\n   AND p.division = 'pending'                                                                                                               ");
            qry.Append($"\n UNION ALL                                                                                                                                  ");
            qry.Append($"\n SELECT                                                                                                                                     ");
            qry.Append($"\n   c.contract_id                                                                                                                            ");
            qry.Append($"\n , CONCAT('(선)',c.ato_no) AS ato_no                                                                                                        ");
            qry.Append($"\n , c.payment_date                                                                                                                           ");
            qry.Append($"\n , c.payment_date_status                                                                                                                    ");
            qry.Append($"\n , c.payment_bank                                                                                                                           ");
            qry.Append($"\n , c.payment_amount AS payment_amount                                                                                                       ");
            qry.Append($"\n , c.manager                                                                                                                                ");
            qry.Append($"\n , c.usance_type                                                                                                                            ");
            qry.Append($"\n , c.division                                                                                                                               ");
            qry.Append($"\n , GROUP_CONCAT(c.product) AS product                                                                                                       ");
            qry.Append($"\n , c.payment_type AS payment_type                                                                                                           ");
            qry.Append($"\n FROM (                                                                                                                                     ");
            qry.Append($"\n 	SELECT                                                                                                                                 ");
            qry.Append($"\n 	  p.contract_id                                                                                                                        ");
            qry.Append($"\n 	, c.ato_no                                                                                                                             ");
            qry.Append($"\n 	, p.payment_date                                                                                                                       ");
            qry.Append($"\n 	, p.payment_date_status                                                                                                                ");
            qry.Append($"\n 	, c.payment_bank                                                                                                                       ");
            qry.Append($"\n 	, p.payment_amount                                                                                                                     ");
            qry.Append($"\n 	, c.manager                                                                                                                            ");
            qry.Append($"\n 	, c.usance_type                                                                                                                        ");
            qry.Append($"\n 	, c.division                                                                                                                           ");
            qry.Append($"\n 	, CONCAT('	', c.product, ' | ', c.origin, ' | ', c.sizes, ' | ', c.box_weight, ' : ', c.quantity_on_paper) AS product                 ");
            qry.Append($"\n 	, p.id AS payment_type                                                                                                                 ");
            qry.Append($"\n 	FROM t_payment AS p                                                                                                                    ");
            qry.Append($"\n 	INNER JOIN t_customs AS c                                                                                                              ");
            qry.Append($"\n 	  ON p.contract_id = c.id                                                                                                              ");
            qry.Append($"\n 	WHERE 1=1                                                                                                                              ");
            qry.Append($"\n       AND c.ato_no NOT LIKE '%취소%' AND c.ato_no NOT LIKE '%삭제%'                                                                         ");
            qry.Append($"\n       AND p.payment_date >= '{sttdate.ToString("yyyy-MM-dd")}'                                                      ");
            qry.Append($"\n       AND p.payment_date <= '{enddate.ToString("yyyy-MM-dd")}'                                                      ");
            //qry.Append($"\n       AND p.payment_date_status <> '결제완료'                                                                       ");
            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n  AND c.ato_no LIKE '%{ato_no}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(bl_no))
            {
                qry.Append($"\n  AND c.bl_no LIKE '%{bl_no}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(lc_no))
            {
                qry.Append($"\n  AND c.lc_payment_date LIKE '%{lc_no}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n  AND c.product LIKE '%{product}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n  AND c.sizes LIKE '%{sizes}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n  AND c.origin LIKE '%{origin}%'                                                      ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n  AND c.manager LIKE '%{manager}%'                                                      ");
            }
            qry.Append($"\n ) AS c                                                                                                                                     ");
            qry.Append($"\n GROUP BY c.contract_id, c.ato_no, c.payment_date, c.payment_date_status, c.payment_amount, c.payment_bank, c.manager, c.usance_type, c.division              ");
            qry.Append($"\n ) AS c                                                                                                                                     ");
            qry.Append($"\n WHERE c.total_amount > 0                                                                                                                                     ");


            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetRegSalesInfoModel(dr);
        }

        public List<CustomsModel> GetCustomTotal(DateTime sttdate, DateTime enddate, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                                                         ");
            qry.Append($"\n   id                                                                                                         ");
            qry.Append($"\n , payment_date                                                                                               ");
            qry.Append($"\n , SUM(unit_price * (box_weight*quantity_on_paper)) AS total_amount                                           ");
            qry.Append($"\n  FROM t_customs                                                                                              ");
            qry.Append($"\n  WHERE 1=1                                                                                                   ");
            qry.Append($"\n  AND payment_date >= '{sttdate.ToString("yyyy-MM-dd")}'                                                      ");
            qry.Append($"\n  AND payment_date <= '{enddate.ToString("yyyy-MM-dd")}'                                                      ");
            qry.Append($"\n GROUP BY payment_date                                                                                        ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetTotalModel(dr);
        }

        public int UpdateCustom(int id, DateTime payment_date, string payment_date_status, IDbTransaction transaction = null)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET                                            ");
            qry.Append($"\n   payment_date        =  '{payment_date}'               ");
            qry.Append($"\n , payment_date_status =  '{payment_date_status}'        ");
            qry.Append($"\n WHERE  id = {id}                     ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public StringBuilder UpdateStatus(int id, string status, string warehousing_date)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET                             ");
            qry.Append($"\n   cc_status        =  '{status}'               ");
            if(!string.IsNullOrEmpty(warehousing_date))
                qry.Append($"\n , warehousing_date =  '{warehousing_date}'     ");
            qry.Append($"\n WHERE  id = {id}                               ");

            return qry;
        }

        public int UpdateHOCO(int id, string val)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET                                      ");
            qry.Append($"\n   sanitary_certificate =  '{val}'                       ");
            qry.Append($"\n WHERE  id = {id}                     ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }


        public List<CustomsModel> GetShipmentMonth(DateTime sttdate, DateTime enddate, string ato_no, string bl_no, string lc_no, string product, string origin, string sizes, string manager, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                                                         ");
            qry.Append($"\n  t.id                                                                                                                         ");
            qry.Append($"\n, t.ato_no                                                                                                                     ");
            qry.Append($"\n, t.manager                                                                                                                    ");
            qry.Append($"\n, t.etd                                                                                                                        ");
            qry.Append($"\n, GROUP_CONCAT(t.product) AS product                                                                                           ");
            qry.Append($"\nFROM (                                                                                                                         ");
            qry.Append($"\n	SELECT                                                                                                                        ");
            qry.Append($"\n	  c.id                                                                                                                        ");
            qry.Append($"\n	, c.ato_no                                                                                                                    ");
            qry.Append($"\n	, c.manager                                                                                                                   ");
            qry.Append($"\n	, CASE                                                                                                                        ");
            qry.Append($"\n	   WHEN IFNULL(c.etd, '') <> '' THEN IFNULL(c.etd, '')                                                                        ");
            qry.Append($"\n	   WHEN IFNULL(c.eta, '') <> '' THEN DATE_ADD(c.eta, INTERVAL -IFNULL(t.delivery_days, 15) DAY)                               ");
            qry.Append($"\n	   WHEN IFNULL(c.shipment_date, '') <> '' THEN IFNULL(c.shipment_date, '')                                                    ");
            qry.Append($"\n	  END AS etd                                                                                                                  ");
            qry.Append($"\n	, CONCAT('\t', c.product, ' | ', c.origin, ' | ', c.sizes, ' | ', c.box_weight, ' : ', c.quantity_on_paper) AS product        ");
            qry.Append($"\n	FROM t_customs AS c                                                                                                           ");
            qry.Append($"\n	LEFT OUTER JOIN t_country AS t                                                                                                ");
            qry.Append($"\n	  ON c.origin = t.country_name                                                                                                ");
            qry.Append($"\n	  AND c.sub_id = 1                                                                                                            ");
            qry.Append($"\n	WHERE 1=1                                                                                                                     ");
            qry.Append($"\n	  AND c.ato_no NOT LIKE '%취소%' AND c.ato_no NOT LIKE '%삭제%'                                                               ");
            qry.Append($"\n	  AND IFNULL(c.is_shipment_qty, 1) = 1                                                                                                                     ");
            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n  AND c.ato_no LIKE '%{ato_no}%'                                                     ");
            }
            if (!string.IsNullOrEmpty(bl_no))
            {
                qry.Append($"\n  AND c.bl_no LIKE '%{bl_no}%'                                                     ");
            }
            if (!string.IsNullOrEmpty(lc_no))
            {
                qry.Append($"\n  AND c.lc_payment_date LIKE '%{lc_no}%'                                                     ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n  AND c.product LIKE '%{product}%'                                                     ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n  AND c.origin LIKE '%{origin}%'                                                     ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n  AND c.sizes LIKE '%{sizes}%'                                                     ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n  AND c.manager LIKE '%{manager}%'                                                     ");
            }
            qry.Append($"\n) AS t                                                                                                                         ");
            qry.Append($"\nWHERE 1=1                                                                                                                    ");
            qry.Append($"\n    AND t.etd >= '{sttdate.ToString("yyyy-MM-dd")}'                                                     ");
            qry.Append($"\n    AND t.etd <= '{enddate.ToString("yyyy-MM-dd")}'                                                     ");
            qry.Append($"\nGROUP BY t.id, t.ato_no, t.manager, t.etd                                                                                      ");
            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetShipmentoModel(dr);
        }

        private List<CustomsModel> GetShipmentoModel(MySqlDataReader rd)
        {
            List<CustomsModel> CustomsModelList = new List<CustomsModel>();
            while (rd.Read())
            {
                CustomsModel model = new CustomsModel();
                model.id = int.Parse(rd["Id"].ToString());
                model.ato_no = rd["ato_no"].ToString();
                model.manager = rd["manager"].ToString();
                model.etd = rd["etd"].ToString();
                model.product = rd["product"].ToString();
                CustomsModelList.Add(model); ;
            }
            rd.Close();
            return CustomsModelList;
        }
        private List<CustomsModel> GetTotalModel(MySqlDataReader rd)
        {
            List<CustomsModel> CustomsModelList = new List<CustomsModel>();
            while (rd.Read())
            {
                CustomsModel model = new CustomsModel();
                model.payment_date = rd["payment_date"].ToString();
                model.total_amount = double.Parse(rd["total_amount"].ToString());
                CustomsModelList.Add(model); ;
            }
            rd.Close();
            return CustomsModelList;
        }

        public List<UncommfirmModel> GetUncomfirm(string cYear, string atoNo, string shipper, string origin, string product, string sizes, string manager, string ccStatus, string payStatus, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   id                      ");
            qry.Append($"\n , sub_id                  ");
            qry.Append($"\n , contract_year           ");
            qry.Append($"\n , ato_no                  ");
            qry.Append($"\n , shipper                 ");
            qry.Append($"\n , shipment_date           ");
            qry.Append($"\n , etd                     ");
            qry.Append($"\n , eta                     ");
            qry.Append($"\n , cc_status               ");
            qry.Append($"\n , origin                  ");
            qry.Append($"\n , product                 ");
            qry.Append($"\n , sizes                   ");
            qry.Append($"\n , box_weight              ");
            qry.Append($"\n , unit_price              ");
            qry.Append($"\n , quantity_on_paper       ");
            qry.Append($"\n , qty                     ");
            qry.Append($"\n , (quantity_on_paper*box_weight)  AS custom_weight          ");
            qry.Append($"\n , manager                 ");
            qry.Append($"\n , (tariff_rate*100)  AS tariff_rate         ");
            qry.Append($"\n , (vat_rate*100)   AS vat_rate           ");
            qry.Append($"\n , remark                  ");
            qry.Append($"\n , IF(payment_date IS NULL, '', payment_date) AS payment_date           ");
            qry.Append($"\n , IF(payment_date_status IS NULL, '', payment_date_status) AS payment_date_status     ");
            qry.Append($"\n , IF(payment_bank IS NULL, '', payment_bank) AS payment_bank            ");
            qry.Append($"\n , usance_type                 ");
            qry.Append($"\n , division                   ");
            qry.Append($"\n FROM t_customs               ");
            qry.Append($"\n WHERE 1=1                ");
            if (cYear != "")
            {
                qry.Append($"\n   AND contract_year = {cYear}             ");
            }
            if (atoNo != "")
            {
                qry.Append($"\n   AND ato_no LIKE '%{atoNo}%'             ");
            }
            if (shipper != "")
            {
                qry.Append($"\n   AND shipper LIKE '%{shipper}%'             ");
            }
            if (origin != "")
            {
                qry.Append($"\n   AND origin LIKE '%{origin}%'             ");
            }
            if (product != "")
            {
                qry.Append($"\n   AND product LIKE '%{product}%'             ");
            }
            if (sizes != "")
            {
                qry.Append($"\n   AND sizes LIKE '%{sizes}%'             ");
            }
            if (manager != "")
            {
                qry.Append($"\n   AND manager LIKE '%{manager}%'          ");
            }
            if (ccStatus != "" && ccStatus != "전체")
            {
                if (ccStatus == "통관")
                {
                    qry.Append($"\n   AND cc_status = '{ccStatus}'          ");
                }
                else if (ccStatus == "미통관")
                {
                    qry.Append($"\n   AND cc_status LIKE '%미통%'          ");
                }

            }
            if (payStatus == "미작성")
            {
                qry.Append($"\n   AND (payment_date = '' OR payment_date IS NULL)          ");
            }
            else if (payStatus == "작성")
            {
                qry.Append($"\n   AND (payment_date != '' OR payment_date IS NOT NULL)          ");
            }
            qry.Append($"\n ORDER BY id, sub_id                ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetCustomModel(dr);
        }
        private List<UncommfirmModel> GetCustomModel(MySqlDataReader rd)

        {
            List<UncommfirmModel> CustomsModelList = new List<UncommfirmModel>();
            while (rd.Read())
            {
                UncommfirmModel model = new UncommfirmModel();

                model.id = int.Parse(rd["Id"].ToString());
                model.sub_d = int.Parse(rd["sub_Id"].ToString());
                model.contract_year = Convert.ToInt32(rd["contract_year"].ToString());
                model.ato_no = rd["ato_no"].ToString();
                model.shipper = rd["shipper"].ToString();
                model.shipment_date = rd["shipment_date"].ToString();
                model.eta = rd["eta"].ToString();
                model.etd = rd["etd"].ToString();
                model.cc_status = rd["cc_status"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();
                model.unit_price = Convert.ToDouble(rd["unit_price"].ToString());
                model.quantity_on_paper = Convert.ToDouble(rd["quantity_on_paper"].ToString());
                model.qty = Convert.ToDouble(rd["qty"].ToString());
                model.custom_weight = Convert.ToDouble(rd["custom_weight"].ToString());
                model.tariff_rate = Convert.ToDouble(rd["tariff_rate"].ToString());
                model.vat_rate = Convert.ToDouble(rd["vat_rate"].ToString());
                model.payment_date = rd["payment_date"].ToString();
                model.payment_date_status = rd["payment_date_status"].ToString();
                model.payment_bank = rd["payment_bank"].ToString();
                model.remark = rd["remark"].ToString();
                model.manager = rd["manager"].ToString();
                model.usance_type = rd["usance_type"].ToString();
                model.division = rd["division"].ToString();

                CustomsModelList.Add(model); ;
            }
            rd.Close();
            return CustomsModelList;
        }

        private List<CustomsSimpleModel> GetCustomsModel(MySqlDataReader rd)
        {
            List<CustomsSimpleModel> CustomsModelList = new List<CustomsSimpleModel>();
            while (rd.Read())
            {
                CustomsSimpleModel model = new CustomsSimpleModel();
                model.pi_date = rd["pi_date"].ToString();
                model.lc_open_date = rd["lc_open_date"].ToString();
                model.lc_payment_date = rd["lc_payment_date"].ToString();
                model.bl_no = rd["bl_no"].ToString();
                model.shipment_date = rd["shipment_date"].ToString();
                model.etd = rd["etd"].ToString();
                model.eta = rd["eta"].ToString();
                model.warehousing_date = rd["warehousing_date"].ToString();
                model.pending_check = rd["pending_check"].ToString();
                model.warehouse = rd["warehouse"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();
                model.unit_price = double.Parse(rd["unit_price"].ToString());
                model.quantity_on_paper = double.Parse(rd["quantity_on_paper"].ToString());
                model.qty = double.Parse(rd["qty"].ToString());
                model.custom_weight = double.Parse(rd["custom_weight"].ToString());
                model.tariff_rate = double.Parse(rd["tariff_rate"].ToString());
                model.vat_rate = double.Parse(rd["vat_rate"].ToString());
                model.remark = rd["remark"].ToString();
                model.custom_weight = double.Parse(rd["custom_weight"].ToString());
                model.payment_date = rd["payment_date"].ToString();
                model.payment_date_status = rd["payment_date_status"].ToString();
                model.payment_bank = rd["payment_bank"].ToString();

                CustomsModelList.Add(model);
            }
            rd.Close();
            return CustomsModelList;
        }

        public StringBuilder UpdateSql2(string id, string division, string bl_no, string eta, string broker, string warehouse, string warehousing_date, string agency, string sanitary_certificate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_customs SET division = '{division}', bl_no = '{bl_no}', eta = '{eta}', broker = '{broker}', warehouse = '{warehouse}'"
                        + $", warehousing_date = '{warehousing_date}', agency_type = '{agency}', sanitary_certificate = '{sanitary_certificate}'  ");
            qry.Append($"\n WHERE id = {id}                                                                                                         ");

            return qry;
        }

        public StringBuilder UpdatePaymentComplete(string id, string payment_date)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_customs SET                    ");
            qry.Append($"\n   payment_date_status = '결제완료'      ");
            qry.Append($"\n , payment_date = '{payment_date}'       ");
            qry.Append($"\n , updatetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'       ");
            qry.Append($"\n WHERE id = {id}                         ");

            return qry;
        }

        public StringBuilder UpdateSql(UncommfirmModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE t_customs SET         ");
            qry.Append($"   warehouse             = '{model.warehouse}'                 ");
            qry.Append($" , broker                = '{model.broker}'                    ");
            qry.Append($" , origin                = '{model.origin}'                    ");
            qry.Append($" , product               = '{model.product}'                   ");
            qry.Append($" , sizes                 = '{model.sizes}'                     ");
            qry.Append($" , weight_calculate      =  {model.weight_calculate}           ");
            qry.Append($" , box_weight            = '{model.box_weight}'                ");
            qry.Append($" , cost_unit             = '{model.cost_unit}'                 ");
            qry.Append($" , unit_price            = {model.unit_price}                  ");
            qry.Append($" , quantity_on_paper     = {model.quantity_on_paper}           ");
            qry.Append($" , qty                   = {model.qty}                         ");
            qry.Append($" , tariff_rate           = {model.tariff_rate}                 ");
            qry.Append($" , vat_rate              = {model.vat_rate}                    ");
            qry.Append($" , clearance_rate        = {model.clearance_rate}              ");
            qry.Append($" , loading_cost_per_box  = {model.loading_cost_per_box}        ");
            qry.Append($" , refrigeration_charge  = {model.refrigeration_charge}        ");
            qry.Append($" , remark                = '{model.remark}'                    ");
            qry.Append($" , trq_amount            = {model.trq_amount}                  ");
            qry.Append($" , shipping_trq_amount   = {model.shipping_trq_amount}                  ");
            qry.Append($" , box_price_adjust      = {model.box_price_adjust}            ");
            qry.Append($" , shipping_box_price_adjust      = {model.shipping_box_price_adjust}            ");
            qry.Append($" WHERE 1=1                                                     ");
            qry.Append($"   AND id = {model.id}                                         ");
            qry.Append($"   AND sub_id = {model.sub_d}                                  ");

            return qry;
        }

        public int GetNextAtoNo(string ato_no)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                               ");
            qry.Append($"\n    IF(MAX(t.ato_no) IS NULL, 1, MAX(t.ato_no) + 1) AS ato_no         ");
            qry.Append($"\n  FROM(                                                               ");
            qry.Append($"\n     SELECT                                                           ");
            qry.Append($"\n  	  CONVERT(SUBSTRING(ato_no, {ato_no.Length + 1}, 2), signed integer) AS ato_no     ");
            qry.Append($"\n  	FROM t_customs                                                   ");
            qry.Append($"\n  	WHERE ato_no LIKE '{ato_no}%'                                    ");
            qry.Append($"\n  	  AND contract_year = {DateTime.Now.Year}                        ");
            qry.Append($"\n  	  AND ato_no NOT LIKE '%취소%'                                   ");
            qry.Append($"\n  	  AND ato_no NOT LIKE '%삭제%'                                   ");
            qry.Append($"\n ) AS t;                                                              ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }
        public int GetCustomsId(string ato_no)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                    ");
            qry.Append($"\n  id                       ");
            qry.Append($"\n FROM t_customs            ");
            qry.Append($"\n WHERE ato_no = '{ato_no}' ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public int UpdateCustomTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
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

        public List<CustomsModel> GetCustomUnknown(DateTime sttdate, DateTime enddate, string ato_no, string bl_no, string lc_no, string product, string origin, string sizes, string manager, IDbTransaction transaction = null)
        {
            if(sttdate < new DateTime(2022, 1, 1))
                sttdate = new DateTime(2022, 1, 1);

            StringBuilder qry = new StringBuilder();

            qry.Append($"\n SELECT                                                                                                                                                         ");
            qry.Append($"\n   t.id                                                                                                                                                         ");
            qry.Append($"\n , t.ato_no  AS ato_no                                                                                                                                          ");
            qry.Append($"\n , t.payment_date                                                                                                                                               ");
            qry.Append($"\n , t.payment_date_status                                                                                                                                        ");
            qry.Append($"\n , t.payment_bank                                                                                                                                               ");
            qry.Append($"\n , t.total_amount - IFNULL(p.payment_amount, 0) AS total_amount                                                                                                 ");
            qry.Append($"\n , t.manager                                                                                                                                                    ");
            qry.Append($"\n , t.usance_type                                                                                                                                                ");
            qry.Append($"\n , t.division                                                                                                                                                   ");
            qry.Append($"\n , t.accuracy                                                                                                                                                   ");
            qry.Append($"\n , t.document_days                                                                                                                                              ");
            qry.Append($"\n , t.product                                                                                                                                                    ");
            qry.Append($"\n FROM(                                                                                                                                                          ");
            qry.Append($"\n 	SELECT                                                                                                                                                     ");
            qry.Append($"\n 	  t.*                                                                                                                                                      ");
            qry.Append($"\n 	FROM(                                                                                                                                                      ");
            qry.Append($"\n 		SELECT                                                                                                                                                 ");
            qry.Append($"\n 		  t.*                                                                                                                                                  ");
            qry.Append($"\n 		FROM(                                                                                                                                                  ");
            qry.Append($"\n 			SELECT                                                                                                                                             ");
            qry.Append($"\n 			c.id                                                                                                                                               ");
            qry.Append($"\n 			, c.ato_no  AS ato_no                                                                                                                              ");
            qry.Append($"\n 			, CASE                                                                                                                                             ");
            qry.Append($"\n 				WHEN c.usance_type = 'O' OR c.usance_type = 'us' OR c.usance_type = 'US' OR c.usance_type = '유산스' OR c.usance_type = 'usance' OR c.usance_type = 'USANCE' THEN date_add(c.eta, INTERVAL 60 DAY)    ");
            qry.Append($"\n 				WHEN c.usance_type = 'tt' OR c.usance_type = 'TT' OR c.usance_type = 'T/T' OR c.usance_type = '티티' THEN c.eta                                  ");
            qry.Append($"\n 				ELSE date_add(c.etd, INTERVAL c.document_days DAY)                                                                                             ");
            qry.Append($"\n 			  END AS payment_date                                                                                                                              ");
            qry.Append($"\n 			, c.payment_date_status                                                                                                                            ");
            qry.Append($"\n 			, c.payment_bank                                                                                                                                   ");
            qry.Append($"\n 			, SUM(c.total_amount) AS total_amount                                                                                                              ");
            qry.Append($"\n 			, c.manager                                                                                                                                        ");
            qry.Append($"\n 			, c.usance_type                                                                                                                                    ");
            qry.Append($"\n 			, c.division                                                                                                                                       ");
            qry.Append($"\n 			, c.accuracy                                                                                                                                       ");
            qry.Append($"\n 			, c.document_days                                                                                                                                  ");
            qry.Append($"\n 			, GROUP_CONCAT(c.product) AS product                                                                                                               ");
            qry.Append($"\n 			FROM(                                                                                                                                              ");
            qry.Append($"\n 			SELECT                                                                                                                                             ");
            qry.Append($"\n 			   c.id                                                                                                                                            ");
            qry.Append($"\n 			 , c.ato_no  AS ato_no                                                                                                                             ");
            qry.Append($"\n 			 , CASE                                                                                                                                            ");
            qry.Append($"\n 				WHEN IFNULL(c.etd, '') <> '' THEN IFNULL(c.etd, '')                                                                                            ");
            qry.Append($"\n 				WHEN IFNULL(c.etd, '') = '' AND IFNULL(c.eta, '') <> '' THEN date_add(c.eta, INTERVAL -IFNULL(s.delivery_days, 15) DAY)                        ");
            qry.Append($"\n 				WHEN IFNULL(c.shipment_date, '') <> '' AND IFNULL(c.etd, '') = '' AND IFNULL(c.eta, '') = '' THEN IFNULL(c.shipment_date, '')                  ");
            qry.Append($"\n 			   END AS etd                                                                                                                                      ");
            qry.Append($"\n 			  , CASE                                                                                                                                           ");
            qry.Append($"\n 				WHEN IFNULL(c.etd, '') <> '' THEN 70                                                                                                           ");
            qry.Append($"\n 				WHEN IFNULL(c.eta, '') <> '' THEN 60                                                                                                           ");
            qry.Append($"\n 				WHEN IFNULL(c.shipment_date, '') <> '' THEN 50                                                                                                 ");
            qry.Append($"\n 			   END AS accuracy                                                                                                                                 ");
            qry.Append($"\n 			 , CASE                                                                                                                                            ");
            qry.Append($"\n 				WHEN IFNULL(c.eta, '') <> '' THEN IFNULL(c.eta, '')                                                                                            ");
            qry.Append($"\n 				WHEN IFNULL(c.etd, '') <> '' THEN date_add(c.etd, INTERVAL IFNULL(s.delivery_days, 15) DAY)                                                    ");
            qry.Append($"\n 				WHEN IFNULL(c.shipment_date, '') <> '' THEN date_add(c.shipment_date, INTERVAL IFNULL(s.delivery_days, 15) DAY)                                ");
            qry.Append($"\n 			   END AS eta                                                                                                                                      ");
            qry.Append($"\n 			 , IFNULL(s.delivery_days, 15)  AS delivery_days                                                                                                   ");
            qry.Append($"\n 			 , c.payment_date_status                                                                                                                           ");
            qry.Append($"\n 			 , c.payment_bank                                                                                                                                  ");
            qry.Append($"\n 			  , CASE                                                                                                                                           ");
            qry.Append($"\n 					 WHEN IFNULL(s.delivery_days, 15) <= 5 THEN 5                                                                                              ");
            qry.Append($"\n 				  WHEN IFNULL(s.delivery_days, 15) <= 10 THEN 8                                                                                                ");
            qry.Append($"\n 				  WHEN IFNULL(s.delivery_days, 15) <= 15 THEN 10                                                                                               ");
            qry.Append($"\n 				  ELSE ROUND(IFNULL(s.delivery_days, 15) * 0.7)                                                                                                ");
            qry.Append($"\n 				   END AS document_days                                                                                                                        ");
            qry.Append($"\n 				 , CASE                                                                                                                                        ");
            qry.Append($"\n 					WHEN IFNULL(c.weight_calculate, 1) = 0 AND IFNULL(c.cost_unit,0) > 0 THEN c.unit_price * (c.cost_unit * c.quantity_on_paper)               ");
            qry.Append($"\n 				 ELSE c.unit_price * (c.box_weight * c.quantity_on_paper)                                                                                      ");
            qry.Append($"\n 				 END AS total_amount                                                                                                                           ");
            qry.Append($"\n 			 , c.manager                                                                                                                                       ");
            qry.Append($"\n 			 , c.usance_type                                                                                                                                   ");
            qry.Append($"\n 			 , c.division                                                                                                                                      ");
            qry.Append($"\n 			 , CONCAT('	', c.product, ' | ', c.origin, ' | ', c.sizes, ' | ', c.box_weight, ' : ', c.quantity_on_paper) AS product                             ");
            qry.Append($"\n 			 FROM (                                                                                                                                            ");
            qry.Append($"\n 				SELECT * FROM t_customs                                                                                                                        ");
            qry.Append($"\n 				 WHERE (payment_date IS NULL OR payment_date = '')                                                                                             ");
            qry.Append($"\n 				   AND IFNULL(is_calendar, 1) = 1                                                                                                              ");
            qry.Append($"\n 				   AND payment_date_status <> '결제완료'                                                                                                       ");
            qry.Append($"\n 				   AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                                                                                   ");
            if (!string.IsNullOrEmpty(ato_no))
                qry.Append($"\n    AND ato_no LIKE '%{ato_no}%'");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n    AND product LIKE '%{product}%'");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n    AND sizes LIKE '%{sizes}%'");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n    AND origin LIKE '%{origin}%'");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n    AND manager LIKE '%{manager}%'");
            qry.Append($"\n 			 ) AS c                                                                                                                                            ");
            qry.Append($"\n 			 LEFT OUTER JOIN t_country AS s                                                                                                                    ");
            qry.Append($"\n 			   ON c.origin = s.country_name                                                                                                                    ");
            qry.Append($"\n 			) AS c                                                                                                                                             ");
            qry.Append($"\n 			WHERE 1=1                                                                                                                                          ");
            qry.Append($"\n 			GROUP BY c.id, c.ato_no, c.payment_date_status, c.payment_bank, c.manager, c.usance_type, c.division                                               ");
            qry.Append($"\n 			UNION ALL                                                                                                                                          ");
            qry.Append($"\n 			SELECT                                                                                                                                             ");
            qry.Append($"\n 			id                                                                                                                                                 ");
            qry.Append($"\n 			, ato_no                                                                                                                                           ");
            qry.Append($"\n 			, payment_date                                                                                                                                     ");
            qry.Append($"\n 			, payment_date_status                                                                                                                              ");
            qry.Append($"\n 			, payment_bank                                                                                                                                     ");
            qry.Append($"\n 			, SUM(CASE                                                                                                                                         ");
            qry.Append($"\n 				WHEN IFNULL(weight_calculate, 1) = 0 AND IFNULL(cost_unit,0) > 0 THEN unit_price * (cost_unit * quantity_on_paper)                             ");
            qry.Append($"\n 				ELSE unit_price * (box_weight * quantity_on_paper)                                                                                             ");
            qry.Append($"\n 			  END) AS total_amount                                                                                                                             ");
            qry.Append($"\n 			, manager                                                                                                                                          ");
            qry.Append($"\n 			, usance_type                                                                                                                                      ");
            qry.Append($"\n 			, division                                                                                                                                         ");
            qry.Append($"\n 			, 80 AS accuracy                                                                                                                                   ");
            qry.Append($"\n 			, 0 AS document_days                                                                                                                               ");
            qry.Append($"\n 			, GROUP_CONCAT('	', product, ' | ', origin, ' | ', sizes, ' | ', box_weight, ' : ', quantity_on_paper) AS product                               ");
            qry.Append($"\n 			FROM t_customs                                                                                                                                     ");
            qry.Append($"\n 			WHERE 1=1                                                                                                                                          ");
            qry.Append($"\n 			  AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                                                                                   ");
            qry.Append($"\n 			  AND (payment_date_status = ''                                                                                                                    ");
            qry.Append($"\n 			  OR  payment_date_status IS NULL                                                                                                                  ");
            qry.Append($"\n 			  OR  payment_date_status = '미확정')                                                                                                                 ");
            qry.Append($"\n 			  AND payment_date IS NOT NULL                                                                                                                     ");
            qry.Append($"\n 			  AND payment_date <> ''                                                                                                                           ");
            if (!string.IsNullOrEmpty(ato_no))
                qry.Append($"\n    AND ato_no LIKE '%{ato_no}%'");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n    AND product LIKE '%{product}%'");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n    AND sizes LIKE '%{sizes}%'");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n    AND origin LIKE '%{origin}%'");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n    AND manager LIKE '%{manager}%'");
            qry.Append($"\n 			GROUP BY id , ato_no , payment_date , payment_bank , payment_date_status, manager , usance_type , division , etd , eta                             ");
            qry.Append($"\n 		) AS t                                                                                                                                                 ");
            qry.Append($"\n	      WHERE t.payment_date >= '{sttdate.ToString("yyyy-MM-dd")}'                                                                                        ");
            qry.Append($"\n	        AND t.payment_date < '{enddate.ToString("yyyy-MM-dd")}'                                                                                         ");
            qry.Append($"\n 	) AS t                                                                                                                                                     ");
            qry.Append($"\n ) AS t                                                                                                                                                         ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                                                                                     ");
            qry.Append($"\n       contract_id                                                                                                                                              ");
            qry.Append($"\n     , SUM(payment_amount) AS payment_amount                                                                                                                    ");
            qry.Append($"\n     FROM t_payment                                                                                                                                             ");
            qry.Append($"\n     GROUP BY contract_id                                                                                                                                       ");
            qry.Append($"\n ) AS p                                                                                                                                                         ");
            qry.Append($"\n   ON t.id = p.contract_id                                                                                                                                      ");
            qry.Append($"\n ORDER BY t.payment_date, t.accuracy DESC                                                                                                                       ");

            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetUnkownPendingModel(dr);
        }

        public List<CustomsModel> GetCustomUnknown2(DateTime enddate, string ato_no, string product, string origin, string sizes, string manager, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                                                                                                             ");
            qry.Append($"\n  t.*                                                                                                                                                                                                                ");
            qry.Append($"\n  FROM (                                                                                                                                                                                                             ");
            qry.Append($"\n 	  SELECT                                                                                                                                                                                                 ");
            qry.Append($"\n		c.id                                                                                                                                                                                                     ");
            qry.Append($"\n	  , c.ato_no  AS ato_no                                                                                                                                                                                      ");
            qry.Append($"\n	  , CASE                                                                                                                                                                                                     ");
            qry.Append($"\n		  WHEN c.usance_type = 'O' OR c.usance_type = 'us' OR c.usance_type = 'US' OR c.usance_type = '유산스' OR c.usance_type = 'usance' OR c.usance_type = 'USANCE' THEN date_add(c.eta, INTERVAL 60 DAY)        ");
            qry.Append($"\n		  WHEN c.usance_type = 'tt' OR c.usance_type = 'TT' OR c.usance_type = 'T/T' OR c.usance_type = '티티' THEN c.eta                                                                                          ");
            qry.Append($"\n		  ELSE date_add(c.etd, INTERVAL c.document_days DAY)                                                                                                                                                     ");
            qry.Append($"\n		END AS payment_date                                                                                                                                                                                      ");
            qry.Append($"\n	  , c.payment_date_status                                                                                                                                                                                    ");
            qry.Append($"\n	  , c.payment_bank                                                                                                                                                                                           ");
            qry.Append($"\n	  , SUM(c.total_amount) AS total_amount                                                                                                                                                                      ");
            qry.Append($"\n	  , c.manager                                                                                                                                                                                                ");
            qry.Append($"\n	  , c.usance_type                                                                                                                                                                                            ");
            qry.Append($"\n	  , c.division                                                                                                                                                                                               ");
            qry.Append($"\n	  , c.accuracy                                                                                                                                                                                               ");
            qry.Append($"\n	  , c.document_days                                                                                                                                                                                          ");
            qry.Append($"\n	  , GROUP_CONCAT(c.product) AS product                                                                                                                                                                       ");
            qry.Append($"\n	 FROM(                                                                                                                                                                                                       ");
            qry.Append($"\n		SELECT                                                                                                                                                                                                   ");
            qry.Append($"\n		   c.id                                                                                                                                                                                                  ");
            qry.Append($"\n		 , c.ato_no  AS ato_no                                                                                                                                                                                   ");
            qry.Append($"\n		 , CASE                                                                                                                                                                                                  ");
            qry.Append($"\n			WHEN IFNULL(c.etd, '') <> '' THEN IFNULL(c.etd, '')                                                                                                                                                  ");
            qry.Append($"\n			WHEN IFNULL(c.etd, '') = '' AND IFNULL(c.eta, '') <> '' THEN date_add(c.eta, INTERVAL -IFNULL(s.delivery_days, 15) DAY)                                                                              ");
            qry.Append($"\n			WHEN IFNULL(c.shipment_date, '') <> '' AND IFNULL(c.etd, '') = '' AND IFNULL(c.eta, '') = '' THEN IFNULL(c.shipment_date, '')                                                                        ");
            qry.Append($"\n		   END AS etd                                                                                                                                                                                            ");
            qry.Append($"\n		  , CASE                                                                                                                                                                                                 ");
            qry.Append($"\n			WHEN IFNULL(c.etd, '') <> '' THEN 70                                                                                                                                                                 ");
            qry.Append($"\n			WHEN IFNULL(c.eta, '') <> '' THEN 60                                                                                                                                                                 ");
            qry.Append($"\n			WHEN IFNULL(c.shipment_date, '') <> '' THEN 50                                                                                                                                                       ");
            qry.Append($"\n		   END AS accuracy                                                                                                                                                                                       ");
            qry.Append($"\n		 , CASE                                                                                                                                                                                                  ");
            qry.Append($"\n			WHEN IFNULL(c.eta, '') <> '' THEN IFNULL(c.eta, '')                                                                                                                                                  ");
            qry.Append($"\n			WHEN IFNULL(c.etd, '') <> '' THEN date_add(c.etd, INTERVAL IFNULL(s.delivery_days, 15) DAY)                                                                                                          ");
            qry.Append($"\n			WHEN IFNULL(c.shipment_date, '') <> '' THEN date_add(c.shipment_date, INTERVAL IFNULL(s.delivery_days, 15) DAY)                                                                                      ");
            qry.Append($"\n		   END AS eta                                                                                                                                                                                            ");
            qry.Append($"\n																																															                     ");
            qry.Append($"\n		 , IFNULL(s.delivery_days, 15)  AS delivery_days                                                                                                                                                         ");
            qry.Append($"\n		 , c.payment_date_status                                                                                                                                                                                 ");
            qry.Append($"\n		 , c.payment_bank                                                                                                                                                                                        ");
            qry.Append($"\n		  , CASE                                                                                                                                                                                                 ");
            qry.Append($"\n				 WHEN IFNULL(s.delivery_days, 15) <= 5 THEN 5                                                                                                                                                    ");
            qry.Append($"\n			  WHEN IFNULL(s.delivery_days, 15) <= 10 THEN 8                                                                                                                                                      ");
            qry.Append($"\n			  WHEN IFNULL(s.delivery_days, 15) <= 15 THEN 10                                                                                                                                                     ");
            qry.Append($"\n			  ELSE ROUND(IFNULL(s.delivery_days, 15) * 0.7)                                                                                                                                                      ");
            qry.Append($"\n			   END AS document_days                                                                                                                                                                              ");
            qry.Append($"\n			 , CASE                                                                                                                                                                                              ");
            qry.Append($"\n				WHEN IFNULL(c.cost_unit,0) > 0 THEN c.unit_price * (c.cost_unit * c.quantity_on_paper)                                                                                                           ");
            qry.Append($"\n			 ELSE c.unit_price * (c.box_weight * c.quantity_on_paper)                                                                                                                                            ");
            qry.Append($"\n			 END AS total_amount                                                                                                                                                                                 ");
            qry.Append($"\n		 , c.manager                                                                                                                                                                                             ");
            qry.Append($"\n		 , c.usance_type                                                                                                                                                                                         ");
            qry.Append($"\n		 , c.division                                                                                                                                                                                            ");
            qry.Append($"\n		  , CONCAT('	', c.product, ' | ', c.origin, ' | ', c.sizes, ' | ', c.box_weight, ' : ', c.quantity_on_paper) AS product                                                                               ");
            qry.Append($"\n		 FROM (                                                                                                                                                                                                  ");
            qry.Append($"\n			SELECT * FROM t_customs                                                                                                                                                                              ");
            qry.Append($"\n			 WHERE (payment_date IS NULL OR payment_date = '')                                                                                                                                                   ");
            qry.Append($"\n			   AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                                                                                                                                                                 ");
            qry.Append($"\n			   AND payment_date_status <> '결제완료'                                                                                                                                                                 ");
            //qry.Append($"\n			   AND payment_date_status <> '확정'                                                                                                                                                                   ");
            if(!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n    AND ato_no LIKE '%{ato_no}%'");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n    AND product LIKE '%{product}%'");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n    AND sizes LIKE '%{sizes}%'");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n    AND origin LIKE '%{origin}%'");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n    AND manager LIKE '%{manager}%'");
            }
            qry.Append($"\n		 ) AS c                                                                                                                                                                                                  ");
            qry.Append($"\n		 LEFT OUTER JOIN t_country AS s                                                                                                                                                                          ");
            qry.Append($"\n		   ON c.origin = s.country_name                                                                                                                                                                          ");
            qry.Append($"\n	 ) AS c                                                                                                                                                                                                      ");
            qry.Append($"\n	  WHERE 1=1                                                                                                                                                                                                  ");
            qry.Append($"\n		AND CASE                                                                                                                                                                                                 ");
            qry.Append($"\n		  WHEN c.usance_type = 'O' OR c.usance_type = 'us' OR c.usance_type = 'US' OR c.usance_type = '유산스' OR c.usance_type = 'usance' OR c.usance_type = 'USANCE' THEN date_add(c.etd, INTERVAL 90 DAY)        ");
            qry.Append($"\n		  ELSE date_add(c.etd, INTERVAL 5 DAY)                                                                                                                                                                   ");
            qry.Append($"\n		END >= '{new DateTime(2022,1,1).ToString("yyyy-MM-dd")}'                                                                                                                                                                                      ");
            qry.Append($"\n		AND CASE                                                                                                                                                                                                 ");
            qry.Append($"\n		  WHEN c.usance_type = 'O' OR c.usance_type = 'us' OR c.usance_type = 'US' OR c.usance_type = '유산스' OR c.usance_type = 'usance' OR c.usance_type = 'USANCE' THEN date_add(c.etd, INTERVAL 90 DAY)        ");
            qry.Append($"\n		  ELSE date_add(c.etd, INTERVAL 5 DAY)                                                                                                                                                                   ");
            qry.Append($"\n		END < '{enddate.ToString("yyyy-MM-dd")}'                                                                                                                                                                                      ");
            qry.Append($"\n	 GROUP BY c.id, c.ato_no, c.payment_date_status, c.payment_bank, c.manager, c.usance_type, c.division                                                                                                        ");


            qry.Append($"\n  UNION ALL                                                                                                                                                                                                          ");
            qry.Append($"\n 	  SELECT                                                                                                                                                                                                        ");
            qry.Append($"\n 	   id                                                                                                                                                                                                           ");
            qry.Append($"\n 	 , ato_no                                                                                                                                                                                                       ");
            qry.Append($"\n 	 , payment_date                                                                                                                                                                                                 ");
            qry.Append($"\n 	 , payment_date_status                                                                                                                                                                                          ");
            qry.Append($"\n 	 , payment_bank                                                                                                                                                                                                 ");
            qry.Append($"\n 	 , SUM(IFNULL(total_amount,0)) AS total_amount                                                                                                                                                                  ");
            qry.Append($"\n 	 , manager                                                                                                                                                                                                      ");
            qry.Append($"\n 	 , usance_type                                                                                                                                                                                                  ");
            qry.Append($"\n 	 , division                                                                                                                                                                                                     ");
            qry.Append($"\n      , 100 AS accuracy                                                                                                                                                                                              ");
            qry.Append($"\n      , 0 AS document_day                                                                                                                                                                                            ");
            qry.Append($"\n      , GROUP_CONCAT(products) AS product                                                                                                                                                                            ");
            qry.Append($"\n 	  FROM(                                                                                                                                                                                                         ");
            qry.Append($"\n 		SELECT                                                                                                                                                                                                      ");
            qry.Append($"\n 		  id                                                                                                                                                                                                        ");
            qry.Append($"\n 		, ato_no                                                                                                                                                                                                    ");
            qry.Append($"\n 		, payment_date                                                                                                                                                                                              ");
            qry.Append($"\n 		, payment_date_status                                                                                                                                                                                       ");
            qry.Append($"\n 		, payment_bank                                                                                                                                                                                              ");
            qry.Append($"\n 		, CASE                                                                                                                                                                                                      ");
            qry.Append($"\n 			WHEN IFNULL(cost_unit,0) > 0 THEN unit_price * (cost_unit * quantity_on_paper)                                                                                                                          ");
            qry.Append($"\n 			ELSE unit_price * (box_weight * quantity_on_paper)                                                                                                                                                      ");
            qry.Append($"\n 			END AS total_amount                                                                                                                                                                                     ");
            qry.Append($"\n 		, manager                                                                                                                                                                                                   ");
            qry.Append($"\n 		, usance_type                                                                                                                                                                                               ");
            qry.Append($"\n 		, division                                                                                                                                                                                                  ");
            qry.Append($"\n 		, CONCAT('\t', product, ' | ', origin, ' | ', sizes, ' | ', box_weight, ' : ', quantity_on_paper) AS products                                                                                               ");
            qry.Append($"\n 		 FROM t_customs                                                                                                                                                                                             ");            
            qry.Append($"\n 		 WHERE payment_date < '{enddate.ToString("yyyy-MM-dd")}' AND payment_date_status <> '결제완료'                                                                                                                                        ");
            qry.Append($"\n			   AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                                                                                                                                                                 ");
            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n    AND ato_no LIKE '%{ato_no}%'");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n    AND product LIKE '%{product}%'");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n    AND sizes LIKE '%{sizes}%'");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n    AND origin LIKE '%{origin}%'");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n    AND manager LIKE '%{manager}%'");
            }
            qry.Append($"\n 	  ) AS t                                                                                                                                                                                                        ");
            qry.Append($"\n 	  GROUP BY id, ato_no, payment_date_status, payment_bank, manager, usance_type, division                                                                                                                        ");
            qry.Append($"\n   ) AS t                                                                                                                                                                                                            ");
            qry.Append($"\n   ORDER BY t.payment_date                                                                                                                                                                                           ");

            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetUnkownPendingModel(dr);
        }

        //선결제 재고
        public DataTable GetPrepayment()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                                    ");
            qry.Append($"\n    c.ID                                                                                                                                    ");
            qry.Append($"\n  , c.ato_no                                                                                                                                ");
            qry.Append($"\n  , SUM(CASE WHEN IFNULL(c.cost_unit, 0) > 0 THEN IFNULL(c.cost_unit, 0) * c.unit_price * c.quantity_on_paper ELSE IFNULL(c.box_weight, 0) * c.unit_price * c.quantity_on_paper END) AS total_price                                              ");
            qry.Append($"\n  , payment_date                                                                                                                            ");     
            qry.Append($"\n  FROM t_customs AS c                                                                                                                       ");
            qry.Append($"\n  WHERE 1 = 1                                                                                                                               ");
            qry.Append($"\n    AND c.cc_status <> '확정'                                                                                                               ");
            qry.Append($"\n    AND c.cc_status <> '통관'                                                                                                               ");
            qry.Append($"\n    AND c.bl_no IS NOT NULL AND c.bl_no <> ''                                                                                               ");
            qry.Append($"\n    AND (c.warehousing_date IS NULL OR c.warehousing_date = '')                                                                             ");
            qry.Append($"\n    AND payment_date_status = '결제완료'                                                                                                    ");
            qry.Append($"\n  GROUP BY ID, ato_no, payment_date                                                                                                         ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetOfferPriceList(string sttdate, string enddate, string product, string origin, string sizes, string unit, string purchase_price = "", string updatetime = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                  ");
            qry.Append($"\n   p.*                                   ");
            qry.Append($"\n FROM (                                  ");
            qry.Append($"\n 	SELECT                              ");
            qry.Append($"\n 	p.product                           ");
            qry.Append($"\n 	, p.origin                          ");
            qry.Append($"\n 	, p.sizes                           ");
            qry.Append($"\n 	, p.unit                            ");
            qry.Append($"\n 	, p.purchase_price                  ");
            qry.Append($"\n 	, p.updatetime                      ");
            qry.Append($"\n 	, c.name AS company                 ");
            qry.Append($"\n 	, 1 AS division                     ");
            qry.Append($"\n 	FROM t_purchase_price AS p          ");
            qry.Append($"\n 	LEFT OUTER JOIN t_company AS c      ");
            qry.Append($"\n 	ON p.cid = c.id                     ");
            qry.Append($"\n 	WHERE 1=1                           ");
            if(!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n 	AND p.updatetime >= '{sttdate}'    ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n 	AND p.updatetime <= '{enddate}'    ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n 	AND p.product = '{product}'        ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n 	AND p.origin = '{origin}'          ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n 	AND p.sizes = '{sizes}'            ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n 	AND p.unit = '{unit}'              ");
            qry.Append($"\n 	UNION ALL                           ");
            qry.Append($"\n 	SELECT                              ");
            qry.Append($"\n 	product                             ");
            qry.Append($"\n 	, origin                            ");
            qry.Append($"\n 	, sizes                             ");
            qry.Append($"\n 	, box_weight                        ");
            qry.Append($"\n 	, unit_price AS purchase_price      ");
            qry.Append($"\n 	, pi_date AS updatetime             ");
            qry.Append($"\n 	, shipper                           ");
            qry.Append($"\n 	, 2 AS division                     ");
            qry.Append($"\n 	FROM t_customs                      ");
            qry.Append($"\n 	WHERE 1=1                           ");
            qry.Append($"\n 	AND unit_price > 0                  ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n 	AND pi_date >= '{sttdate}'    ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n 	AND pi_date <= '{enddate}'    ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n 	AND product = '{product}'        ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n 	AND origin = '{origin}'          ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n 	AND sizes = '{sizes}'            ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n 	AND box_weight = '{unit}'              ");
            qry.Append($"\n ) AS p                                  ");
            qry.Append($"\n WHERE 1=1                                  ");
            if (!string.IsNullOrEmpty(purchase_price))
                qry.Append($"\n 	AND purchase_price = {purchase_price}             ");
            if (!string.IsNullOrEmpty(updatetime))
                qry.Append($"\n 	AND DATE_FORMAT(updatetime, '%Y-%m-%d') = '{updatetime}'              ");
            qry.Append($"\n ORDER BY p.updatetime                   ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        //미통관내역
        public List<CustomsInfoModel> GetPending(string type, string sYear, string eYear, string atoNo, bool isStart, string contractNo, string shipper
                                                , string lcNo, string blNo, string import_number, string origin, string product, string sizes, string box_weight, string manager
                                                , string usance, string division, string agency, string payment, string cc_status, IDbTransaction transaction = null, int sorttype = 1
                                                , int isDelete = 1)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                ");
            qry.Append($"\n   c.*                                                                 ");
            qry.Append($"\n FROM (                                                                ");
            qry.Append($"\n   SELECT                                                              ");
            qry.Append($"\n     c.ID                                                                ");
            qry.Append($"\n   , c.sub_id                                                            ");
            qry.Append($"\n   , c.contract_year                                                     ");
            qry.Append($"\n   , c.ato_no                                                            ");
            qry.Append($"\n   , c.pi_date                                                           ");
            qry.Append($"\n   , c.contract_no                                                       ");
            qry.Append($"\n   , c.shipper                                                           ");
            qry.Append($"\n   , c.lc_open_date                                                      ");
            qry.Append($"\n   , c.lc_payment_date                                                   ");
            qry.Append($"\n   , c.bl_no                                                             ");
            qry.Append($"\n   , c.shipment_date                                                     ");
            qry.Append($"\n   , c.etd                                                               ");
            qry.Append($"\n   , c.eta                                                               ");
            qry.Append($"\n   , c.warehousing_date                                                  ");
            qry.Append($"\n   , c.pending_check                                                     ");
            qry.Append($"\n   , c.cc_status                                                         ");
            qry.Append($"\n   , c.broker                                                            ");
            qry.Append($"\n   , c.warehouse                                                         ");
            qry.Append($"\n   , c.origin                                                            ");
            qry.Append($"\n   , c.product                                                           ");
            qry.Append($"\n   , c.sizes                                                             ");
            qry.Append($"\n   , c.box_weight                                                        ");
            qry.Append($"\n   , c.cost_unit                                                         ");
            qry.Append($"\n   , c.unit_price                                                        ");
            qry.Append($"\n   , c.quantity_on_paper                                                 ");
            qry.Append($"\n   , c.qty                                                               ");
            qry.Append($"\n   , c.tariff_rate                                                       ");
            qry.Append($"\n   , c.vat_rate                                                          ");
            qry.Append($"\n   , c.remark                                                            ");
            qry.Append($"\n   , c.import_number                                                     ");
            qry.Append($"\n   , c.payment_date                                                      ");
            qry.Append($"\n   , c.payment_date_status                                               ");
            qry.Append($"\n   , c.payment_bank                                                      ");
            qry.Append($"\n   , c.usance_type                                                       ");
            qry.Append($"\n   , c.agency_type                                                       ");
            qry.Append($"\n   , c.division                                                          ");
            qry.Append($"\n   , c.sanitary_certificate                                              ");
            qry.Append($"\n   , c.manager                                                           ");
            qry.Append($"\n   , c.edit_user                                                         ");
            qry.Append($"\n   , c.updatetime                                                        ");
            qry.Append($"\n   , IF(IFNULL(c.weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate                                         ");
            
            qry.Append($"\n    FROM t_customs AS c                                                ");
            /*qry.Append($"\n    LEFT OUTER JOIN t_product_other_cost AS p                          ");
            qry.Append($"\n      ON c.product = p.product                                         ");
            qry.Append($"\n     AND c.origin = p.origin                                           ");
            qry.Append($"\n     AND c.sizes = p.sizes                                             ");
            qry.Append($"\n     AND c.box_weight = p.unit                                         ");*/
            qry.Append($"\n    WHERE 1 = 1                                                        ");
            if (isDelete == 3)
                qry.Append($"\n      AND (c.ato_no LIKE '%취소%' OR c.ato_no LIKE '%삭제%')                                ");
            else if (isDelete == 2)
                qry.Append($"\n      AND c.ato_no NOT LIKE '%취소%' AND c.ato_no NOT LIKE '%삭제%'                         ");
            //조회 구분
            if (type == "Contract")
            {
                qry.Append($"\n      AND c.cc_status <> '확정'                                ");
                qry.Append($"\n	     AND c.cc_status <> '통관'                                ");
                qry.Append($"\n	     AND (c.bl_no IS NULL OR c.bl_no = '')                    ");
            }
            else if (type == "Shipping")
            {
                qry.Append($"\n      AND c.cc_status <> '확정'                                ");
                qry.Append($"\n      AND c.cc_status <> '통관'                                ");
                qry.Append($"\n	     AND c.bl_no IS NOT NULL AND c.bl_no <> ''                ");
                //qry.Append($"\n      AND (c.warehousing_date IS NULL OR c.warehousing_date = '' OR c.warehousing_date > NOW() OR DATE_FORMAT(c.warehousing_date,'%Y-%m-%d') IS NULL)   ");
                qry.Append($"\n      AND (c.warehousing_date IS NULL OR c.warehousing_date = '')   ");
            }
            else if (type == "AfterStock")
            {
                qry.Append($"\n      AND c.cc_status <> '확정'                                ");
                qry.Append($"\n      AND c.cc_status <> '통관'                                ");
                qry.Append($"\n	     AND c.bl_no IS NOT NULL AND c.bl_no <> ''                ");
                //qry.Append($"\n      AND c.warehousing_date <= NOW()                          ");
                qry.Append($"\n      AND c.warehousing_date <> ''                             ");
                qry.Append($"\n      AND c.warehousing_date IS NOT NULL                       ");
            }
            else if (type == "Customs")
                qry.Append($"\n      AND c.cc_status = '통관'                                ");
            else if (type == "Comfirm")
                qry.Append($"\n      AND c.cc_status = '확정'                                ");
            //정보검색
            if (!string.IsNullOrEmpty(sYear))
                qry.Append($"\n   AND c.contract_year >= {sYear}                                                         ");
            if (!string.IsNullOrEmpty(eYear))
                qry.Append($"\n   AND c.contract_year <= {eYear}                                                         ");
            if (!string.IsNullOrEmpty(atoNo))
                qry.Append($"\n  {commonRepository.whereSql("c.ato_no", atoNo, isStart)}                                                         "); 
                
            if (!string.IsNullOrEmpty(contractNo))
                qry.Append($"\n  {commonRepository.whereSql("c.contract_no", contractNo)}                                                         ");
            if (!string.IsNullOrEmpty(shipper))
                qry.Append($"\n  {commonRepository.whereSql("c.shipper", shipper)}                                                         ");
            if (!string.IsNullOrEmpty(lcNo))
                qry.Append($"\n  {commonRepository.whereSql("c.lc_payment_date", lcNo)}                                                         ");
            if (!string.IsNullOrEmpty(blNo))
                qry.Append($"\n  {commonRepository.whereSql("c.bl_no", blNo)}                                                         ");
            if (!string.IsNullOrEmpty(import_number))
                qry.Append($"\n  {commonRepository.whereSql("c.import_number", import_number)}                                                         ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n  {commonRepository.whereSql("c.origin", origin)}                                                         ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n  {commonRepository.whereSql("c.product", product)}                                                         ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n  {commonRepository.whereSql("c.sizes", sizes)}                                                         ");
            if (!string.IsNullOrEmpty(box_weight))
                qry.Append($"\n  {commonRepository.whereSql("c.box_weight", box_weight)}                                                         ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n  {commonRepository.whereSql("c.manager", manager)}                                                         ");
            if (!string.IsNullOrEmpty(usance))
            {
                if (usance == "O" || usance == "US" || usance == "us" || usance == "USANCE" || usance == "usance" || usance == "유산스" || usance == "유")
                    usance = "US";
                else if (usance == "AT" || usance == "at" || usance == "ATSIGHT" || usance == "atsight" || usance == "AT SIGHT" || usance == "at sight" || usance == "At sight" || usance == "엣사이트")
                    usance = "AT";
                else if (usance == "TT" || usance == "tt" || usance == "T/T" || usance == "티티")
                    usance = "T/T";

                qry.Append($"\n   AND c.usance_type LIKE '%{usance}%'                                                         ");
            }
            if (!string.IsNullOrEmpty(agency))
                qry.Append($"\n  {commonRepository.whereSql("c.agency_type", agency)}                                                         ");
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n  {commonRepository.whereSql("c.division", division)}                                                         ");
            if (!string.IsNullOrEmpty(payment))
                qry.Append($"\n  {commonRepository.whereSql("c.payment_date_status", payment)}                                                         ");
            if (!string.IsNullOrEmpty(cc_status))
                qry.Append($"\n  {commonRepository.whereSql("c.cc_status", cc_status)}                                                         ");

            qry.Append($"\n    UNION ALL                                                          ");
            qry.Append($"\n    SELECT                                                             ");
            qry.Append($"\n     c.ID                                                                ");
            qry.Append($"\n   , 9999 AS sub_id                                                    ");
            qry.Append($"\n   , c.contract_year AS contract_year                                    ");
            qry.Append($"\n   , c.ato_no AS ato_no                                                  ");
            qry.Append($"\n   , '' AS pi_date                                                     ");
            qry.Append($"\n   , '' AS contract_no                                                 ");
            qry.Append($"\n   , '' AS shipper                                                     ");
            qry.Append($"\n   , '' AS lc_open_date                                                ");
            qry.Append($"\n   , '' AS lc_payment_date                                             ");
            qry.Append($"\n   , '' AS bl_no                                                       ");
            qry.Append($"\n   , '' AS shipment_date                                               ");
            qry.Append($"\n   , '' AS etd                                                         ");
            qry.Append($"\n   , '' AS eta                                                         ");
            qry.Append($"\n   , '' AS warehousing_date                                            ");
            qry.Append($"\n   , '' AS pending_check                                               ");
            qry.Append($"\n   , '' AS cc_status                                                   ");
            qry.Append($"\n   , '' AS broker                                                      ");
            qry.Append($"\n   , '' AS warehouse                                                   ");
            qry.Append($"\n   , '' AS origin                                                      ");
            qry.Append($"\n   , '' AS product                                                     ");
            qry.Append($"\n   , '' AS sizes                                                       ");
            qry.Append($"\n   , '' AS box_weight                                                  ");
            qry.Append($"\n   , '' AS cost_unit                                                   ");
            qry.Append($"\n   , '' AS unit_price                                                  ");
            qry.Append($"\n   , '' AS quantity_on_paper                                           ");
            qry.Append($"\n   , '' AS qty                                                         ");
            qry.Append($"\n   , '' AS tariff_rate                                                 ");
            qry.Append($"\n   , '' AS vat_rate                                                    ");
            qry.Append($"\n   , IFNULL(s.remark, '') AS remark                                    ");
            qry.Append($"\n   , '' AS import_number                                                   ");
            qry.Append($"\n   , '' AS payment_date                                                ");
            qry.Append($"\n   , '' AS payment_date_status                                         ");
            qry.Append($"\n   , '' AS payment_bank                                                ");
            qry.Append($"\n   , '' AS usance_type                                                 ");
            qry.Append($"\n   , '' AS agency_type                                                 ");
            qry.Append($"\n   , '' AS division                                                    ");
            qry.Append($"\n   , '' AS sanitary_certificate                                        ");
            qry.Append($"\n   , '' AS manager                                                     ");
            qry.Append($"\n   , '' AS edit_user                                                   ");
            qry.Append($"\n   , updatetime AS updatetime                                          ");
            qry.Append($"\n   , 'TRUE' AS weight_calculate                                            ");
            
            qry.Append($"\n    FROM t_customs AS c                                                ");
            qry.Append($"\n    LEFT OUTER JOIN t_shipping AS s                                    ");
            qry.Append($"\n      ON c.id = s.custom_id                                            ");
            qry.Append($"\n    WHERE 1 = 1                                                        ");
            if (isDelete == 3)
                qry.Append($"\n      AND (c.ato_no LIKE '%취소%' OR c.ato_no LIKE '%삭제%')                                ");
            else if (isDelete == 2)
                qry.Append($"\n      AND c.ato_no NOT LIKE '%취소%' AND c.ato_no NOT LIKE '%삭제%'                         ");
            //조회 구분
            if (type == "Contract")
            {
                qry.Append($"\n      AND c.cc_status <> '확정'                                     ");
                qry.Append($"\n	     AND c.cc_status <> '통관'                                     ");
                qry.Append($"\n	     AND (c.bl_no IS NULL OR c.bl_no = '')                           ");
            }
            else if (type == "Shipping")
            {
                qry.Append($"\n      AND c.cc_status <> '확정'                                       ");
                qry.Append($"\n      AND c.cc_status <> '통관'                                       ");
                qry.Append($"\n	     AND c.bl_no IS NOT NULL AND c.bl_no <> ''                         ");
                //qry.Append($"\n      AND (warehousing_date IS NULL OR warehousing_date = '' OR warehousing_date > NOW() OR DATE_FORMAT(warehousing_date,'%Y-%m-%d') IS NULL)   ");
                qry.Append($"\n      AND (c.warehousing_date IS NULL OR c.warehousing_date = '')");
            }
            else if (type == "AfterStock")
            {
                qry.Append($"\n      AND c.cc_status <> '확정'                                ");
                qry.Append($"\n      AND c.cc_status <> '통관'                                ");
                qry.Append($"\n	     AND c.bl_no IS NOT NULL AND c.bl_no <> ''                  ");
                //qry.Append($"\n      AND warehousing_date <= NOW()                          ");
                qry.Append($"\n      AND c.warehousing_date IS NOT NULL                       ");
                qry.Append($"\n      AND c.warehousing_date <> ''                             ");
            }
            else if (type == "Customs")
                qry.Append($"\n      AND c.cc_status = '통관'                                ");
            else if (type == "Comfirm")
                qry.Append($"\n      AND c.cc_status = '확정'                                ");
            //정보검색
            if (!string.IsNullOrEmpty(sYear))
                qry.Append($"\n   AND c.contract_year >= {sYear}                                                         ");
            if (!string.IsNullOrEmpty(eYear))
                qry.Append($"\n   AND c.contract_year <= {eYear}                                                         ");
            if (!string.IsNullOrEmpty(atoNo))
            {
                qry.Append($"\n  {commonRepository.whereSql("c.ato_no", atoNo, isStart)}                                                         ");
            }
            if (!string.IsNullOrEmpty(contractNo))
                qry.Append($"\n  {commonRepository.whereSql("c.contract_no", contractNo)}                                                         ");
            if (!string.IsNullOrEmpty(shipper))
                qry.Append($"\n  {commonRepository.whereSql("c.shipper", shipper)}                                                         ");
            if (!string.IsNullOrEmpty(lcNo))
                qry.Append($"\n  {commonRepository.whereSql("c.lc_payment_date", lcNo)}                                                         ");
            if (!string.IsNullOrEmpty(blNo))
                qry.Append($"\n  {commonRepository.whereSql("c.bl_no", blNo)}                                                         ");
            if (!string.IsNullOrEmpty(import_number))
                qry.Append($"\n  {commonRepository.whereSql("c.import_number", import_number)}                                                         ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n  {commonRepository.whereSql("c.origin", origin)}                                                         ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n  {commonRepository.whereSql("c.product", product)}                                                         ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n  {commonRepository.whereSql("c.sizes", sizes)}                                                         ");
            if (!string.IsNullOrEmpty(box_weight))
                qry.Append($"\n  {commonRepository.whereSql("c.box_weight", box_weight)}                                                         ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n  {commonRepository.whereSql("c.manager", manager)}                                                         ");
            if (!string.IsNullOrEmpty(usance))
                qry.Append($"\n  {commonRepository.whereSql("c.usance_type", usance)}                                                         ");
            if (!string.IsNullOrEmpty(agency))
                qry.Append($"\n  {commonRepository.whereSql("c.agency_type", agency)}                                                         ");
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n  {commonRepository.whereSql("c.division", division)}                                                         ");
            if (!string.IsNullOrEmpty(payment))
                qry.Append($"\n  {commonRepository.whereSql("c.payment_date_status", payment)}                                                         ");
            if (!string.IsNullOrEmpty(cc_status))
                qry.Append($"\n  {commonRepository.whereSql("c.cc_status", cc_status)}                                                         ");
            qry.Append($"\n    GROUP BY c.ID, DATE_FORMAT(c.updatetime, '%Y-%m-%d')                                            ");
            qry.Append($"\n ) AS c                                                                ");

            //Order by
            /*if (type == "Customs" || type == "Comfirm")
                qry.Append($"\n ORDER BY DATE_FORMAT(c.updatetime, '%Y-%m-%d'), c.id, c.sub_id, c.ato_no                           ");
            else
                qry.Append($"\n ORDER BY ID, sub_id, ato_no                            ");*/
            if (sorttype == 1)
                qry.Append($"\n ORDER BY ID, sub_id, ato_no                            ");
            else if (sorttype == 2)
                qry.Append($"\n ORDER BY DATE_FORMAT(c.updatetime, '%Y-%m-%d'), c.id, c.sub_id, c.ato_no                           ");
            else if (sorttype == 3)
                qry.Append($"\n ORDER BY ato_no, ID, sub_id                             ");
            else if (sorttype == 4)
                qry.Append($"\n ORDER BY pi_date, ID, sub_id, ato_no                             ");
            else if (sorttype == 5)
                qry.Append($"\n ORDER BY ETD, ID, sub_id, ato_no                            ");
            else if (sorttype == 6)
                qry.Append($"\n ORDER BY ETA, ID, sub_id, ato_no                            ");
            else if (sorttype == 7)
                qry.Append($"\n ORDER BY warehousing_date, ID, sub_id, ato_no                            ");


            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetPendingModel(dr);
        }


        public List<CustomsInfoModel> GetPendingInfo(string id, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                       ");
            qry.Append($"\n     ID                                                                     ");
            qry.Append($"\n   , sub_id                                                                 ");
            qry.Append($"\n   , contract_year                                                          ");
            qry.Append($"\n   , ato_no                                                                 ");
            qry.Append($"\n   , pi_date                                                                ");
            qry.Append($"\n   , contract_no                                                            ");
            qry.Append($"\n   , shipper                                                                ");
            qry.Append($"\n   , lc_open_date                                                           ");
            qry.Append($"\n   , lc_payment_date                                                        ");
            qry.Append($"\n   , bl_no                                                                  ");
            qry.Append($"\n   , shipment_date                                                          ");
            qry.Append($"\n   , etd                                                                    ");
            qry.Append($"\n   , eta                                                                    ");
            qry.Append($"\n   , warehousing_date                                                       ");
            qry.Append($"\n   , pending_check                                                          ");
            qry.Append($"\n   , cc_status                                                              ");
            qry.Append($"\n   , warehouse                                                              ");
            qry.Append($"\n   , origin                                                                 ");
            qry.Append($"\n   , product                                                                ");
            qry.Append($"\n   , sizes                                                                  ");
            qry.Append($"\n   , box_weight                                                             ");
            qry.Append($"\n   , unit_price                                                             ");
            qry.Append($"\n   , quantity_on_paper                                                      ");
            qry.Append($"\n   , qty                                                                    ");
            qry.Append($"\n   , tariff_rate * 100 AS tariff_rate                                       ");
            qry.Append($"\n   , vat_rate * 100 AS vat_rate                                             ");
            qry.Append($"\n   , payment_date                                                           ");
            qry.Append($"\n   , payment_date_status                                                    ");
            qry.Append($"\n   , payment_bank                                                           ");
            qry.Append($"\n   , usance_type                                                            ");
            qry.Append($"\n   , agency_type                                                            ");
            qry.Append($"\n   , division                                                               ");
            qry.Append($"\n   , manager                                                                ");
            qry.Append($"\n   , edit_user                                                              ");
            qry.Append($"\n   , updatetime                                                             ");
            qry.Append($"\n   , IF(IFNULL(weight_calculate, 1) = 'TRUE', 'FALSE') AS weight_calculate  ");
            qry.Append($"\n   , import_number                                                          ");
            qry.Append($"\n    FROM t_customs                                                          ");
            qry.Append($"\n    WHERE 1 = 1                                                             ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n   AND id = {id}             ");
            qry.Append($"\n ORDER BY ID, sub_id                                 ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetPendingModel(dr);
        }
        public DataTable GetFindData(string ato_no, string lc_no, string bl_no, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                       ");
            qry.Append($"\n     ID                                                                     ");
            qry.Append($"\n   , sub_id                                                                 ");
            qry.Append($"\n   , contract_year                                                          ");
            qry.Append($"\n   , ato_no                                                                 ");
            qry.Append($"\n   , pi_date                                                                ");
            qry.Append($"\n   , contract_no                                                            ");
            qry.Append($"\n   , shipper                                                                ");
            qry.Append($"\n   , lc_open_date                                                           ");
            qry.Append($"\n   , lc_payment_date                                                        ");
            qry.Append($"\n   , bl_no                                                                  ");
            qry.Append($"\n   , shipment_date                                                          ");
            qry.Append($"\n   , etd                                                                    ");
            qry.Append($"\n   , eta                                                                    ");
            qry.Append($"\n   , warehousing_date                                                       ");
            qry.Append($"\n   , pending_check                                                          ");
            qry.Append($"\n   , cc_status                                                              ");
            qry.Append($"\n   , warehouse                                                              ");
            qry.Append($"\n   , origin                                                                 ");
            qry.Append($"\n   , product                                                                ");
            qry.Append($"\n   , sizes                                                                  ");
            qry.Append($"\n   , box_weight                                                             ");
            qry.Append($"\n   , unit_price                                                             ");
            qry.Append($"\n   , quantity_on_paper                                                      ");
            qry.Append($"\n   , qty                                                                    ");
            qry.Append($"\n   , tariff_rate * 100 AS tariff_rate                                       ");
            qry.Append($"\n   , vat_rate * 100 AS vat_rate                                             ");
            qry.Append($"\n   , payment_date                                                           ");
            qry.Append($"\n   , payment_date_status                                                    ");
            qry.Append($"\n   , payment_bank                                                           ");
            qry.Append($"\n   , usance_type                                                            ");
            qry.Append($"\n   , agency_type                                                            ");
            qry.Append($"\n   , division                                                               ");
            qry.Append($"\n   , manager                                                                ");
            qry.Append($"\n   , edit_user                                                              ");
            qry.Append($"\n   , updatetime                                                             ");
            qry.Append($"\n    FROM t_customs                                                          ");
            qry.Append($"\n    WHERE 1 = 1                                                             ");
            if (!string.IsNullOrEmpty(ato_no))
            { 
                qry.Append($"\n      AND ato_no LIKE '%{ato_no}%'                                          ");
            }
            if (!string.IsNullOrEmpty(lc_no))
            {
                qry.Append($"\n      AND lc_payment_date LIKE '%{lc_no}%'                                          ");
            }
            if (!string.IsNullOrEmpty(bl_no))
            {
                qry.Append($"\n      AND bl_no LIKE '%{bl_no}%'                                          ");
            }
            qry.Append($"\n ORDER BY ID, sub_id                                 ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public List<AllCustomsModel> GetAllTypePending(string type, string id, bool isNew = false, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                   ");
            qry.Append($"\n    c.id                                                                   ");
            qry.Append($"\n  , c.sub_id                                                               ");
            qry.Append($"\n  , c.contract_year                                                        ");
            qry.Append($"\n  , c.ato_no                                                               ");
            qry.Append($"\n  , c.pi_date                                                              ");
            qry.Append($"\n  , c.contract_no                                                          ");
            qry.Append($"\n  , c.shipper                                                              ");
            qry.Append($"\n  , c.lc_open_date                                                         ");
            qry.Append($"\n  , c.lc_payment_date                                                      ");
            qry.Append($"\n  , c.bl_no                                                                ");
            qry.Append($"\n  , c.shipment_date                                                        ");
            qry.Append($"\n  , c.etd                                                                  ");
            qry.Append($"\n  , c.eta                                                                  ");
            qry.Append($"\n  , c.warehousing_date                                                     ");
            qry.Append($"\n  , c.pending_check                                                        ");
            qry.Append($"\n  , c.cc_status                                                            ");
            qry.Append($"\n  , c.warehouse                                                            ");
            qry.Append($"\n  , c.origin                                                               ");
            qry.Append($"\n  , c.product                                                              ");
            qry.Append($"\n  , c.sizes                                                                ");
            qry.Append($"\n  , c.box_weight                                                           ");
            qry.Append($"\n  , c.cost_unit                                                            ");
            qry.Append($"\n  , c.unit_price                                                           ");
            qry.Append($"\n  , c.quantity_on_paper                                                    ");
            qry.Append($"\n  , c.qty                                                                  ");
            qry.Append($"\n  , c.manager                                                              ");
            qry.Append($"\n  , c.edit_user                                                            ");
            qry.Append($"\n  , c.updatetime                                                           ");
            qry.Append($"\n  , c.tariff_rate                                                          ");
            qry.Append($"\n  , c.vat_rate                                                             ");
            qry.Append($"\n  , c.loading_cost_per_box                                                 ");
            qry.Append($"\n  , c.refrigeration_charge                                                 ");
            qry.Append($"\n  , c.total_amount_seaover                                                 ");
            qry.Append($"\n  , c.remark                                                               ");
            qry.Append($"\n  , IFNULL(c.trq_amount, 0) AS trq_amount                                  ");
            qry.Append($"\n  , IFNULL(c.shipping_trq_amount, 0) AS shipping_trq_amount                ");
            qry.Append($"\n  , c.payment_date                                                         ");
            qry.Append($"\n  , c.payment_date_status                                                  ");
            qry.Append($"\n  , c.payment_bank                                                         ");
            qry.Append($"\n  , c.usance_type                                                          ");
            qry.Append($"\n  , c.division                                                             ");
            qry.Append($"\n  , c.agency_type                                                          ");
            qry.Append($"\n  , c.clearance_rate                                                       ");
            qry.Append($"\n  , c.clearance_rate                                                       ");
            qry.Append($"\n  , c.broker                                                               ");
            qry.Append($"\n  , c.sanitary_certificate                                                 ");
            qry.Append($"\n  , IFNULL(c.box_price_adjust, 0) AS box_price_adjust                      ");
            qry.Append($"\n  , IFNULL(c.shipping_box_price_adjust, 0) AS shipping_box_price_adjust                      ");
            qry.Append($"\n  , IFNULL(p.tax, 0) AS tax                                                ");
            qry.Append($"\n  , IFNULL(p.custom, 0) AS custom                                          ");
            qry.Append($"\n  , IFNULL(p.incidental_expense, 0) AS incidental_expense                  ");
            qry.Append($"\n  , IF(c.weight_calculate = 0, 'FALSE', 'TRUE') AS weight_calculate                  ");
            qry.Append($"\n  , IF(IFNULL(c.is_calendar, 1) =  1, 'TRUE', 'FALSE') AS is_calendar                  ");
            qry.Append($"\n  , IF(IFNULL(c.is_shipment_qty, 1) =  1, 'TRUE', 'FALSE') AS is_shipment_qty          ");
            qry.Append($"\n  , c.import_number                                                 ");
            qry.Append($"\n  , IF(IFNULL(c.is_quarantine, false) = 0, 'FALSE', 'TRUE') AS is_quarantine                    ");
            qry.Append($"\n FROM t_customs AS c                                                       ");
            qry.Append($"\n LEFT OUTER JOIN t_product_other_cost AS p                                 ");
            qry.Append($"\n   ON c.product = p.product                                                ");
            qry.Append($"\n   AND c.origin = p.origin                                                 ");
            qry.Append($"\n   AND c.sizes = p.sizes                                                   ");
            qry.Append($"\n   AND box_weight = p.unit                                                 ");
            qry.Append($"\n WHERE 1 = 1                                                           ");

            if (string.IsNullOrEmpty(id))
            { 
                if (type == "미통관")
                {
                    qry.Append($"\n      AND ( c.cc_status = '미통관' OR c.cc_status = '미통' OR c.cc_status = '' OR c.cc_status IS NULL)       ");
                }
                else if (type == "통관")
                {
                    qry.Append($"\n      AND c.cc_status = '통관' ");
                }
                else if (type == "확정")
                {
                    qry.Append($"\n      AND c.cc_status = '확정' ");
                }
            }
            else
            {
                qry.Append($"\n   AND c.id = {id}             ");
            }
            qry.Append($"\n ORDER BY c.id, c.sub_id  ");
            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetUnPendingModel2(dr);
        }

        public List<AllCustomsModel> GetUnPending(string id, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                    ");
            qry.Append($"\n   id                                             ");
            qry.Append($"\n , sub_id                                         ");
            qry.Append($"\n , contract_year                                  ");
            qry.Append($"\n , ato_no                                         ");
            qry.Append($"\n , pi_date                                        ");
            qry.Append($"\n , contract_no                                    ");
            qry.Append($"\n , shipper                                        ");
            qry.Append($"\n , lc_open_date                                   ");
            qry.Append($"\n , lc_payment_date                                ");
            qry.Append($"\n , bl_no                                          ");
            qry.Append($"\n , shipment_date                                  ");
            qry.Append($"\n , etd                                            ");
            qry.Append($"\n , eta                                            ");
            qry.Append($"\n , warehousing_date                               ");
            qry.Append($"\n , pending_check                                  ");
            qry.Append($"\n , cc_status                                      ");
            qry.Append($"\n , warehouse                                      ");
            qry.Append($"\n , origin                                         ");
            qry.Append($"\n , product                                        ");
            qry.Append($"\n , sizes                                          ");
            qry.Append($"\n , box_weight                                     ");
            qry.Append($"\n , unit_price                                     ");
            qry.Append($"\n , quantity_on_paper                              ");
            qry.Append($"\n , qty                                            ");
            qry.Append($"\n , manager                                        ");
            qry.Append($"\n , edit_user                                      ");
            qry.Append($"\n , updatetime                                     ");
            qry.Append($"\n , tariff_rate                                    ");
            qry.Append($"\n , vat_rate                                       ");
            qry.Append($"\n , loading_cost_per_box                           ");
            qry.Append($"\n , refrigeration_charge                           ");
            qry.Append($"\n , total_amount_seaover                           ");
            qry.Append($"\n , remark                                         ");
            qry.Append($"\n , trq_amount                                     ");
            qry.Append($"\n , IFNULL(shipping_trq_amount, 0) AS shipping_trq_amount                                     ");
            qry.Append($"\n , payment_date                                   ");
            qry.Append($"\n , payment_date_status                            ");
            qry.Append($"\n , payment_bank                                   ");
            qry.Append($"\n , usance_type                                    ");
            qry.Append($"\n , division                                       ");
            qry.Append($"\n , agency_type                                    ");
            qry.Append($"\n , clearance_rate                                 ");
            qry.Append($"\n , clearance_rate                                  ");
            qry.Append($"\n , broker                                          ");
            qry.Append($"\n , sanitary_certificate                            ");
            qry.Append($"\n , IFNULL(box_price_adjust, 0) AS box_price_adjust ");
            qry.Append($"\n , IFNULL(shipping_box_price_adjust, 0) AS shipping_box_price_adjust ");
            qry.Append($"\n , IF(IFNULL(is_calendar, 1) = 1, 'TRUE', 'FALSE') AS is_calendar ");
            qry.Append($"\n , IF(IFNULL(is_shipment_qty, 1) = 1, 'TRUE', 'FALSE') AS is_shipment_qty ");
            qry.Append($"\n , import_number                            ");
            qry.Append($"\n FROM t_customs                                                          ");
            qry.Append($"\n WHERE 1 = 1                                                             ");
            //qry.Append($"\n      AND ( cc_status = '미통관' OR cc_status = '미통' OR cc_status = '' OR cc_status IS NULL)       ");
            if (id != "")
            {
                qry.Append($"\n   AND id = {id}             ");
            }
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            
            return GetUnPendingModel(dr);
        }

        private List<AllCustomsModel> GetUnPendingModel2(MySqlDataReader rd)
        {
            List<AllCustomsModel> CustomsModelList = new List<AllCustomsModel>();
            while (rd.Read())
            {
                AllCustomsModel model = new AllCustomsModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.sub_id = Convert.ToInt32(rd["sub_id"].ToString());
                model.contract_year = Convert.ToInt32(rd["contract_year"].ToString());
                model.ato_no = rd["ato_no"].ToString();
                model.pi_date = rd["pi_date"].ToString();
                model.contract_no = rd["contract_no"].ToString();
                model.shipper = rd["shipper"].ToString();
                model.lc_open_date = rd["lc_open_date"].ToString();
                model.lc_payment_date = rd["lc_payment_date"].ToString();
                model.bl_no = rd["bl_no"].ToString();
                model.shipment_date = rd["shipment_date"].ToString();
                model.etd = rd["etd"].ToString();
                model.eta = rd["eta"].ToString();
                model.warehousing_date = rd["warehousing_date"].ToString();
                model.pending_check = rd["pending_check"].ToString();
                model.cc_status = rd["cc_status"].ToString();
                model.warehouse = rd["warehouse"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();
                model.cost_unit = rd["cost_unit"].ToString();

                bool is_calendar;
                if (!bool.TryParse(rd["is_calendar"].ToString(), out is_calendar))
                    is_calendar = false;
                model.is_calendar = is_calendar;

                bool is_shipment_qty;
                if (!bool.TryParse(rd["is_shipment_qty"].ToString(), out is_shipment_qty))
                    is_shipment_qty = false;
                model.is_shipment_qty = is_shipment_qty;

                if (rd["unit_price"] == null || rd["unit_price"].ToString() == "")
                    model.unit_price = 0;
                else
                    model.unit_price = Convert.ToDouble(rd["unit_price"].ToString());

                if (rd["quantity_on_paper"] == null || rd["quantity_on_paper"].ToString() == "")
                    model.quantity_on_paper = 0;
                else
                    model.quantity_on_paper = Convert.ToDouble(rd["quantity_on_paper"].ToString());

                if (rd["qty"] == null || rd["qty"].ToString() == "")
                    model.qty = 0;
                else
                    model.qty = Convert.ToDouble(rd["qty"].ToString());

                model.manager = rd["manager"].ToString();
                model.edit_user = rd["edit_user"].ToString();
                model.updatetime = rd["updatetime"].ToString();

                if (rd["tariff_rate"] == null || rd["tariff_rate"].ToString() == "")
                    model.tariff_rate = 0;
                else
                    model.tariff_rate = Convert.ToDouble(rd["tariff_rate"].ToString());

                if (rd["vat_rate"] == null || rd["vat_rate"].ToString() == "")
                    model.vat_rate = 0;
                else
                    model.vat_rate = Convert.ToDouble(rd["vat_rate"].ToString());

                if (rd["loading_cost_per_box"] == null || rd["loading_cost_per_box"].ToString() == "")
                    model.loading_cost_per_box = 0;
                else
                    model.loading_cost_per_box = Convert.ToDouble(rd["loading_cost_per_box"].ToString());

                if (rd["refrigeration_charge"] == null || rd["refrigeration_charge"].ToString() == "")
                    model.refrigeration_charge = 0;
                else
                    model.refrigeration_charge = Convert.ToDouble(rd["refrigeration_charge"].ToString());

                model.total_amount_seaover = rd["total_amount_seaover"].ToString();
                model.remark = rd["remark"].ToString();

                if (rd["trq_amount"] == null || rd["trq_amount"].ToString() == "")
                    model.trq_amount = 0;
                else
                    model.trq_amount = Convert.ToDouble(rd["trq_amount"].ToString());

                model.shipping_trq_amount = Convert.ToDouble(rd["shipping_trq_amount"].ToString());
                model.payment_date = rd["payment_date"].ToString();
                model.payment_date_status = rd["payment_date_status"].ToString();
                model.payment_bank = rd["payment_bank"].ToString();
                model.usance_type = rd["usance_type"].ToString();
                model.division = rd["division"].ToString();
                model.agency_type = rd["agency_type"].ToString();

                if (rd["clearance_rate"] == null || rd["clearance_rate"].ToString() == "")
                    model.clearance_rate = 0;
                else
                    model.clearance_rate = Convert.ToDouble(rd["clearance_rate"].ToString());

                model.broker = rd["broker"].ToString();
                model.sanitary_certificate = rd["sanitary_certificate"].ToString();
                model.box_price_adjust = Convert.ToDouble(rd["box_price_adjust"].ToString());
                model.shipping_box_price_adjust = Convert.ToDouble(rd["shipping_box_price_adjust"].ToString());

                model.tax = Convert.ToDouble(rd["tax"].ToString());
                model.custom = Convert.ToDouble(rd["custom"].ToString());
                model.incidental_expense = Convert.ToDouble(rd["incidental_expense"].ToString());

                model.weight_calculate = Convert.ToBoolean(rd["weight_calculate"].ToString());
                model.import_number = rd["import_number"].ToString();

                model.is_quarantine = Convert.ToBoolean(rd["is_quarantine"].ToString());

                CustomsModelList.Add(model);
            }
            rd.Close();
            return CustomsModelList;
        }
        private List<AllCustomsModel> GetUnPendingModel(MySqlDataReader rd)
        {
            List<AllCustomsModel> CustomsModelList = new List<AllCustomsModel>();
            while (rd.Read())
            {
                AllCustomsModel model = new AllCustomsModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.sub_id = Convert.ToInt32(rd["sub_id"].ToString());
                model.contract_year = Convert.ToInt32(rd["contract_year"].ToString());
                model.ato_no = rd["ato_no"].ToString();
                model.pi_date = rd["pi_date"].ToString();
                model.contract_no = rd["contract_no"].ToString();
                model.shipper = rd["shipper"].ToString();
                model.lc_open_date = rd["lc_open_date"].ToString();
                model.lc_payment_date = rd["lc_payment_date"].ToString();
                model.bl_no = rd["bl_no"].ToString();
                model.shipment_date = rd["shipment_date"].ToString();
                model.etd = rd["etd"].ToString();
                model.eta = rd["eta"].ToString();
                model.warehousing_date = rd["warehousing_date"].ToString();
                model.pending_check = rd["pending_check"].ToString();
                model.cc_status = rd["cc_status"].ToString();
                model.warehouse = rd["warehouse"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();

                bool is_calendar;
                if (!bool.TryParse(rd["is_calendar"].ToString(), out is_calendar))
                    is_calendar = false;
                model.is_calendar = is_calendar;
                bool is_shipment_qty;
                if (!bool.TryParse(rd["is_shipment_qty"].ToString(), out is_shipment_qty))
                    is_shipment_qty = false;
                model.is_shipment_qty = false;
                
                if (rd["unit_price"] == null || rd["unit_price"].ToString() == "")
                {
                    model.unit_price = 0;
                }
                else
                {
                    model.unit_price = Convert.ToDouble(rd["unit_price"].ToString());
                }

                if (rd["quantity_on_paper"] == null || rd["quantity_on_paper"].ToString() == "")
                {
                    model.quantity_on_paper = 0;
                }
                else
                {
                    model.quantity_on_paper = Convert.ToDouble(rd["quantity_on_paper"].ToString());
                }

                if (rd["qty"] == null || rd["qty"].ToString() == "")
                {
                    model.qty = 0;
                }
                else
                {
                    model.qty = Convert.ToDouble(rd["qty"].ToString());
                }

                model.manager = rd["manager"].ToString();
                model.edit_user = rd["edit_user"].ToString();
                model.updatetime = rd["updatetime"].ToString();

                if (rd["tariff_rate"] == null || rd["tariff_rate"].ToString() == "")
                {
                    model.tariff_rate = 0;
                }
                else
                {
                    model.tariff_rate = Convert.ToDouble(rd["tariff_rate"].ToString());
                }

                if (rd["vat_rate"] == null || rd["vat_rate"].ToString() == "")
                {
                    model.vat_rate = 0;
                }
                else
                {
                    model.vat_rate = Convert.ToDouble(rd["vat_rate"].ToString());
                }

                if (rd["loading_cost_per_box"] == null || rd["loading_cost_per_box"].ToString() == "")
                {
                    model.loading_cost_per_box = 0;
                }
                else
                {
                    model.loading_cost_per_box = Convert.ToDouble(rd["loading_cost_per_box"].ToString());
                }

                if (rd["refrigeration_charge"] == null || rd["refrigeration_charge"].ToString() == "")
                {
                    model.refrigeration_charge = 0;
                }
                else
                {
                    model.refrigeration_charge = Convert.ToDouble(rd["refrigeration_charge"].ToString());
                }

                model.total_amount_seaover = rd["total_amount_seaover"].ToString();
                model.remark = rd["remark"].ToString();

                if (rd["trq_amount"] == null || rd["trq_amount"].ToString() == "")
                {
                    model.trq_amount = 0;
                }
                else
                {
                    model.trq_amount = Convert.ToDouble(rd["trq_amount"].ToString());
                }
                model.shipping_trq_amount = Convert.ToDouble(rd["shipping_trq_amount"].ToString());
                model.payment_date = rd["payment_date"].ToString();
                model.payment_date_status = rd["payment_date_status"].ToString();
                model.payment_bank = rd["payment_bank"].ToString();
                model.usance_type = rd["usance_type"].ToString();
                model.division = rd["division"].ToString();
                model.agency_type = rd["agency_type"].ToString();

                if (rd["clearance_rate"] == null || rd["clearance_rate"].ToString() == "")
                {
                    model.clearance_rate = 0;
                }
                else
                {
                    model.clearance_rate = Convert.ToDouble(rd["clearance_rate"].ToString());
                }

                model.broker = rd["broker"].ToString();
                model.sanitary_certificate = rd["sanitary_certificate"].ToString();
                model.box_price_adjust = Convert.ToDouble(rd["box_price_adjust"].ToString());
                model.shipping_box_price_adjust = Convert.ToDouble(rd["shipping_box_price_adjust"].ToString());

                CustomsModelList.Add(model);
            }
            rd.Close();
            return CustomsModelList;
        }


        private List<CustomsInfoModel> GetPendingModel(MySqlDataReader rd)
        {
            List<CustomsInfoModel> CustomsModelList = new List<CustomsInfoModel>();
            while (rd.Read())
            {
                CustomsInfoModel model = new CustomsInfoModel();
                model.ID = Convert.ToInt32(rd["id"].ToString());
                model.sub_id = Convert.ToInt32(rd["sub_id"].ToString());
                model.contract_year = Convert.ToInt32(rd["contract_year"].ToString());
                model.ato_no = rd["ato_no"].ToString();
                model.pi_date = rd["pi_date"].ToString();
                model.contract_no = rd["contract_no"].ToString();
                model.shipper = rd["shipper"].ToString();
                model.lc_open_date = rd["lc_open_date"].ToString();
                model.lc_payment_date = rd["lc_payment_date"].ToString();
                model.bl_no = rd["bl_no"].ToString();
                model.shipment_date = rd["shipment_date"].ToString();
                model.etd = rd["etd"].ToString();
                model.eta = rd["eta"].ToString();
                model.warehousing_date = rd["warehousing_date"].ToString();
                model.pending_check = rd["pending_check"].ToString();
                model.cc_status = rd["cc_status"].ToString();
                model.warehouse = rd["warehouse"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();
                model.cost_unit = rd["cost_unit"].ToString();
                model.broker = rd["broker"].ToString();
                model.remark = rd["remark"].ToString();
                double unit_price;
                if(!double.TryParse(rd["unit_price"].ToString(), out unit_price))
                    unit_price = 0;
                model.unit_price = unit_price;

                double quantity_on_paper;
                if (!double.TryParse(rd["quantity_on_paper"].ToString(), out quantity_on_paper))
                    quantity_on_paper = 0;
                model.quantity_on_paper = quantity_on_paper;

                double qty;
                if (!double.TryParse(rd["qty"].ToString(), out qty))
                    qty = 0;
                model.qty = qty;

                double tariff_rate;
                if (!double.TryParse(rd["tariff_rate"].ToString(), out tariff_rate))
                    tariff_rate = 0;
                model.tariff_rate = tariff_rate;

                double vat_rate;
                if (!double.TryParse(rd["vat_rate"].ToString(), out vat_rate))
                    vat_rate = 0;
                model.vat_rate = vat_rate;

                DateTime payment_date;
                if (DateTime.TryParse(rd["payment_date"].ToString(), out payment_date))
                    model.payment_date = payment_date.ToString("yyyy-MM-dd");

                model.payment_date_status = rd["payment_date_status"].ToString();
                model.payment_bank = rd["payment_bank"].ToString(); 
                model.usance_type = rd["usance_type"].ToString(); 
                model.agency_type = rd["agency_type"].ToString();
                model.division = rd["division"].ToString(); 
                model.manager = rd["manager"].ToString(); 
                model.edit_user = rd["edit_user"].ToString();

                DateTime updatetime;
                if (DateTime.TryParse(rd["updatetime"].ToString(), out updatetime))
                    model.updatetime = updatetime.ToString("yyyy-MM-dd");

                model.sanitary_certificate = rd["sanitary_certificate"].ToString();
                bool weight_calculate;
                if (bool.TryParse(rd["weight_calculate"].ToString(), out weight_calculate))
                    model.weight_calculate = weight_calculate;
                else
                    model.weight_calculate = true;
                model.import_number = rd["import_number"].ToString();

                CustomsModelList.Add(model);
            }
            rd.Close();
            return CustomsModelList;
        }

        public StringBuilder DeleteSql(string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_customs WHERE id = {id}              ");
          
            return qry;
        }

        public StringBuilder InsertSql(CustomsInfoModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_customs (                                                         ");
            qry.Append($"  id");
            qry.Append($" , sub_id");
            qry.Append($" , contract_year");
            qry.Append($" , ato_no");
            qry.Append($" , pi_date");
            qry.Append($" , contract_no");
            qry.Append($" , shipper");
            qry.Append($" , lc_open_date");
            qry.Append($" , lc_payment_date");
            qry.Append($" , bl_no");
            qry.Append($" , shipment_date");
            qry.Append($" , etd");
            qry.Append($" , eta");
            qry.Append($" , warehousing_date");
            qry.Append($" , pending_check");
            qry.Append($" , cc_status");
            qry.Append($" , warehouse");
            qry.Append($" , origin");
            qry.Append($" , product");
            qry.Append($" , sizes");
            qry.Append($" , box_weight");
            qry.Append($" , unit_price");
            qry.Append($" , quantity_on_paper");
            qry.Append($" , qty");
            qry.Append($" , manager");
            qry.Append($" , edit_user");
            qry.Append($" , updatetime");
            qry.Append($" , tariff_rate");
            qry.Append($" , vat_rate");
            qry.Append($" , loading_cost_per_box");
            qry.Append($" , refrigeration_charge");
            qry.Append($" , total_amount_seaover");
            qry.Append($" , remark");
            qry.Append($" , trq_amount");
            qry.Append($" , payment_date");
            qry.Append($" , payment_date_status");
            qry.Append($" , payment_bank");
            qry.Append($" , usance_type");
            qry.Append($" , agency_type");
            qry.Append($" , division");
            qry.Append($" )  VALUES ( ");
            qry.Append($"    {model.ID}");
            qry.Append($" ,  {model.sub_id}");
            qry.Append($" ,  {model.contract_year}");
            qry.Append($" , '{model.ato_no}'");
            qry.Append($" , '{model.pi_date}'");
            qry.Append($" , '{model.contract_no}'");
            qry.Append($" , '{model.shipper}'");
            qry.Append($" , '{model.lc_open_date}'");
            qry.Append($" , '{model.lc_payment_date}'");
            qry.Append($" , '{model.bl_no}'");
            qry.Append($" , '{model.shipment_date}'");
            qry.Append($" , '{model.etd}'");
            qry.Append($" , '{model.eta}'");
            qry.Append($" , '{model.warehousing_date}'");
            qry.Append($" , '{model.pending_check}'");
            qry.Append($" , '{model.cc_status}'");
            qry.Append($" , '{model.warehouse}'");
            qry.Append($" , '{model.origin}'");
            qry.Append($" , '{model.product}'");
            qry.Append($" , '{model.sizes}'");
            qry.Append($" , '{model.box_weight}'");
            qry.Append($" ,  {model.unit_price}");
            qry.Append($" ,  {model.quantity_on_paper}");
            qry.Append($" ,  {model.qty}");
            qry.Append($" , '{model.manager}'");
            qry.Append($" , '{model.edit_user}'");
            qry.Append($" , '{DateTime.Now.ToString("yyyy-MM-dd")}'");
            qry.Append($" ,  {model.tariff_rate}");
            qry.Append($" ,  {model.vat_rate}");
            qry.Append($" ,  {model.loading_cost_per_box}");
            qry.Append($" ,  {model.refrigeration_charge}");
            qry.Append($" , '{model.total_amount_seaover}'");
            qry.Append($" , '{model.remark}'");
            qry.Append($" ,  {model.trq_amount}");
            qry.Append($" , '{model.payment_date}'");
            qry.Append($" , '{model.payment_date_status}'");
            qry.Append($" , '{model.payment_bank}'");
            qry.Append($" , '{model.usance_type}'");
            qry.Append($" , '{model.agency_type}'");
            qry.Append($" , '{model.division}'");
            qry.Append($"  )");

            return qry;
        }

        public StringBuilder InsertSql2(AllCustomsModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_customs (                                                         ");
            qry.Append($"\n   id                     ");
            qry.Append($"\n , sub_id                 ");
            qry.Append($"\n , contract_year          ");
            qry.Append($"\n , ato_no                 ");
            qry.Append($"\n , pi_date                ");
            qry.Append($"\n , contract_no            ");
            qry.Append($"\n , shipper                ");
            qry.Append($"\n , lc_open_date           ");
            qry.Append($"\n , lc_payment_date        ");
            qry.Append($"\n , bl_no                  ");
            qry.Append($"\n , shipment_date          ");
            qry.Append($"\n , etd                    ");
            qry.Append($"\n , eta                    ");
            qry.Append($"\n , warehousing_date       ");
            qry.Append($"\n , pending_check          ");
            qry.Append($"\n , cc_status              ");
            qry.Append($"\n , origin                 ");
            qry.Append($"\n , product                ");
            qry.Append($"\n , sizes                  ");
            qry.Append($"\n , box_weight             ");
            qry.Append($"\n , cost_unit              ");
            qry.Append($"\n , unit_price             ");
            qry.Append($"\n , quantity_on_paper      ");
            qry.Append($"\n , qty                    ");
            qry.Append($"\n , manager                ");
            qry.Append($"\n , edit_user              ");
            qry.Append($"\n , updatetime             ");
            qry.Append($"\n , tariff_rate            ");
            qry.Append($"\n , vat_rate               ");
            qry.Append($"\n , loading_cost_per_box   ");
            qry.Append($"\n , refrigeration_charge   ");
            qry.Append($"\n , total_amount_seaover   ");
            qry.Append($"\n , remark                 ");
            qry.Append($"\n , trq_amount             ");
            qry.Append($"\n , shipping_trq_amount             ");
            qry.Append($"\n , payment_date           ");
            qry.Append($"\n , payment_date_status    ");
            qry.Append($"\n , payment_bank           ");
            qry.Append($"\n , usance_type            ");
            qry.Append($"\n , division               ");
            qry.Append($"\n , agency_type            ");
            qry.Append($"\n , warehouse              ");
            qry.Append($"\n , clearance_rate         ");
            qry.Append($"\n , broker                 ");
            qry.Append($"\n , sanitary_certificate   ");
            qry.Append($"\n , weight_calculate       ");
            qry.Append($"\n , box_price_adjust       ");
            qry.Append($"\n , shipping_box_price_adjust       ");
            qry.Append($"\n , is_calendar            ");
            qry.Append($"\n , is_shipment_qty        ");
            qry.Append($"\n , import_number          ");
            qry.Append($"\n , is_quarantine          ");
            qry.Append($"\n )  VALUES (              ");
            qry.Append($"\n   '{model.id                  }'   ");
            qry.Append($"\n , '{model.sub_id              }'   ");
            qry.Append($"\n , '{model.contract_year       }'   ");
            qry.Append($"\n , '{model.ato_no              }'   ");
            qry.Append($"\n , '{model.pi_date             }'   ");
            qry.Append($"\n , '{model.contract_no         }'   ");
            qry.Append($"\n , '{model.shipper             }'   ");
            qry.Append($"\n , '{model.lc_open_date        }'   ");
            qry.Append($"\n , '{model.lc_payment_date     }'   ");
            qry.Append($"\n , '{model.bl_no               }'   ");
            qry.Append($"\n , '{model.shipment_date       }'   ");
            qry.Append($"\n , '{model.etd                 }'   ");
            qry.Append($"\n , '{model.eta                 }'   ");
            qry.Append($"\n , '{model.warehousing_date    }'   ");
            qry.Append($"\n , '{model.pending_check       }'   ");
            qry.Append($"\n , '{model.cc_status           }'   ");
            qry.Append($"\n , '{model.origin              }'   ");
            qry.Append($"\n , '{model.product             }'   ");
            qry.Append($"\n , '{model.sizes               }'   ");
            qry.Append($"\n , '{model.box_weight          }'   ");
            qry.Append($"\n , '{model.cost_unit           }'   ");
            qry.Append($"\n ,  {model.unit_price          }    ");
            qry.Append($"\n ,  {model.quantity_on_paper   }    ");
            qry.Append($"\n ,  {model.qty                 }    ");
            qry.Append($"\n , '{model.manager             }'   ");
            qry.Append($"\n , '{model.edit_user           }'   ");
            qry.Append($"\n , '{DateTime.Now.ToString("yyyy-MM-dd")}'   ");
            qry.Append($"\n ,  {model.tariff_rate         }    ");
            qry.Append($"\n ,  {model.vat_rate            }    ");
            qry.Append($"\n ,  {model.loading_cost_per_box}    ");
            qry.Append($"\n ,  {model.refrigeration_charge}    ");
            qry.Append($"\n , '{model.total_amount_seaover}'   ");
            qry.Append($"\n , '{model.remark              }'   ");
            qry.Append($"\n ,  {model.trq_amount          }    ");
            qry.Append($"\n ,  {model.shipping_trq_amount}    ");
            if (string.IsNullOrEmpty(model.payment_date))
                qry.Append($"\n , null                             ");
            else
                qry.Append($"\n , '{model.payment_date        }'   ");
            qry.Append($"\n , '{model.payment_date_status }'   ");
            qry.Append($"\n , '{model.payment_bank        }'   ");
            qry.Append($"\n , '{model.usance_type         }'   ");
            qry.Append($"\n , '{model.division            }'   ");
            qry.Append($"\n , '{model.agency_type         }'   ");
            qry.Append($"\n , '{model.warehouse           }'   ");
            qry.Append($"\n ,  {model.clearance_rate      }    ");
            qry.Append($"\n , '{model.broker              }'   ");
            qry.Append($"\n , '{model.sanitary_certificate}'   ");
            qry.Append($"\n , {model.weight_calculate}   ");
            qry.Append($"\n , {model.box_price_adjust}   ");
            qry.Append($"\n , {model.shipping_box_price_adjust}   ");
            qry.Append($"\n , {model.is_calendar}   ");
            qry.Append($"\n , {model.is_shipment_qty}   ");
            qry.Append($"\n , '{model.import_number}'   ");
            qry.Append($"\n , {model.is_quarantine}   ");
            qry.Append($"  )");

            string sql = qry.ToString();

            return qry;
        }

        public int GetNextId()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT IF( MAX(id) IS NULL, 1, MAX(id) +1) AS id FROM t_customs");
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public int UpdateData(string id, string col_name, string col_value)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET {col_name} = '{col_value}' WHERE id = {id}");
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public int UpdateDataAsOne(string id, string sub_id, string col_name, string col_value)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET {col_name} = '{col_value}' WHERE id = {id} AND sub_id = {sub_id}");
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public StringBuilder UpdateDataString(string id, string col_name, string col_value)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET {col_name} = '{col_value}' WHERE id = {id}");
            return qry;
        }

        public int UpdatePayment(string id, string status)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET payment_date_status = '{status}' WHERE id = {id}");
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public int UpdatePaymentDate(string id, string paymentDate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET payment_date = '{paymentDate}' WHERE id = {id}");
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public int DelayPaymentDate(string id, string paymentDate)
        {
            StringBuilder qry = new StringBuilder();
            if (paymentDate != null)
            {
                qry.Append($" UPDATE t_customs SET payment_date = '{paymentDate}', payment_date_status = '미확정' WHERE id = {id}");
            }
            else
            { 
                qry.Append($" UPDATE t_customs SET payment_date = NULL, payment_date_status = '' WHERE id = {id}");
            }

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public int DelayDate(string id, string col, string val)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET {col} = '{val}' WHERE id = {id}");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public List<ArriveModel> GetArrive(DateTime sttdate, DateTime enddate, string product = null, string origin = null, string sizes = null, string manager = null, int id = 0, int sub_id = 0, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                            ");
            qry.Append($"\n   t.id                                                                                                                            ");
            qry.Append($"\n , t.sub_id                                                                                                                        ");
            qry.Append($"\n , t.product                                                                                                                       ");
            qry.Append($"\n , t.origin                                                                                                                        ");
            qry.Append($"\n , t.sizes                                                                                                                         ");
            qry.Append($"\n , t.box_weight                                                                                                                    ");
            qry.Append($"\n , t.quantity_on_paper                                                                                                             ");
            qry.Append($"\n , t.cc_status                                                                                                                     ");
            /*qry.Append($"\n , CASE WHEN t.cc_status = '미통관' OR t.cc_status = '' THEN IF('{DateTime.Now.ToString("yyyy-MM-dd")}' > date_add(t.eta, INTERVAL 5 DAY) , date_add(NOW(), INTERVAL 5 DAY), date_add(t.eta, INTERVAL 5 DAY)) ELSE date_add(t.eta, INTERVAL 5 DAY) END AS pending_date  ");
            qry.Append($"\n , CASE WHEN t.cc_status = '미통관' OR t.cc_status = '' THEN IF('{DateTime.Now.ToString("yyyy-MM-dd")}' > date_add(t.eta, INTERVAL 5 DAY) , 50, t.score) ELSE t.score END AS score                                                              ");*/
            qry.Append($"\n , t.eta  AS pending_date                                                                                                          ");
            qry.Append($"\n , t.score                                                                                                                         ");
            qry.Append($"\n , t.manager                                                                                                                       ");
            qry.Append($"\n FROM(                                                                                                                             ");
            qry.Append($"\n 	SELECT                                                                                                                        ");
            qry.Append($"\n 	  c.id                                                                                                                        ");
            qry.Append($"\n 	, c.sub_id                                                                                                                    ");
            qry.Append($"\n 	, c.product                                                                                                                   ");
            qry.Append($"\n 	, c.origin                                                                                                                    ");
            qry.Append($"\n 	, c.sizes                                                                                                                     ");
            qry.Append($"\n 	, c.box_weight                                                                                                                ");
            qry.Append($"\n 	, c.quantity_on_paper                                                                                                         ");
            qry.Append($"\n 	, c.cc_status                                                                                                                 ");
            qry.Append($"\n 	, CASE                                                                                                                        ");
            qry.Append($"\n 		WHEN date_add(c.pending_check, INTERVAL 1 DAY) IS NOT NULL AND IFNULL(c.pending_check, '') <> '' THEN IFNULL(c.pending_check, '')                   ");
            qry.Append($"\n 		WHEN IFNULL(c.eta, '') <> '' THEN date_add(IFNULL(c.eta, ''), INTERVAL 5 DAY)                                                 ");
            qry.Append($"\n 		WHEN IFNULL(c.etd, '') <> '' THEN date_add(c.etd, INTERVAL IFNULL(d.delivery_days, 15) + 5 DAY)                               ");
            qry.Append($"\n 		WHEN IFNULL(c.shipment_date, '') <> '' THEN date_add(c.shipment_date, INTERVAL IFNULL(d.delivery_days, 15) + 5 DAY)           ");
            qry.Append($"\n 	   END AS eta                                                                                                                 ");
            qry.Append($"\n 	, CASE                                                                                                                        ");
            qry.Append($"\n 		WHEN date_add(c.pending_check, INTERVAL 1 DAY) IS NOT NULL AND IFNULL(c.pending_check, '') <> '' THEN 90                   ");
            qry.Append($"\n 		WHEN IFNULL(c.eta, '') <> '' THEN 80                                                                                      ");
            qry.Append($"\n 		WHEN IFNULL(c.etd, '') <> '' THEN 70                                                                                      ");
            qry.Append($"\n 		WHEN IFNULL(c.shipment_date, '') <> '' THEN 60                                                                            ");
            qry.Append($"\n 	   END AS score                                                                                                               ");
            qry.Append($"\n 	, c.manager                                                                                                                   ");
            qry.Append($"\n 	FROM t_customs AS c                                                                                                           ");
            qry.Append($"\n 	LEFT OUTER JOIN t_country AS d                                                                                                ");
            qry.Append($"\n 	  ON c.origin = d.country_name                                                                                                ");
            qry.Append($"\n 	WHERE 1=1                                                                                                                     ");
            qry.Append($"\n	      AND IFNULL(c.is_shipment_qty, 1) = 1                                                                                                                     ");
            qry.Append($"\n 	  AND contract_year >= 2022                                                                                                   ");
            qry.Append($"\n 	  AND ato_no NOT LIKE '%취소%'                                                                                                ");
            qry.Append($"\n ) AS t                                                                                                                            ");
            /*qry.Append($"\n WHERE CASE WHEN t.cc_status = '미통관' OR t.cc_status = '' THEN IF('2022-11-28' > date_add(t.eta, INTERVAL 5 DAY) , date_add(NOW(), INTERVAL 5 DAY), date_add(t.eta, INTERVAL 5 DAY)) ELSE date_add(t.eta, INTERVAL 5 DAY) END >= '{sttdate.ToString("yyyy-MM-dd")}'                                                       ");
            qry.Append($"\n   AND CASE WHEN t.cc_status = '미통관' OR t.cc_status = '' THEN IF('2022-11-28' > date_add(t.eta, INTERVAL 5 DAY) , date_add(NOW(), INTERVAL 5 DAY), date_add(t.eta, INTERVAL 5 DAY)) ELSE date_add(t.eta, INTERVAL 5 DAY) END <= '{enddate.ToString("yyyy-MM-dd")}'                                                       ");*/
            qry.Append($"\n WHERE t.eta >= '{sttdate.ToString("yyyy-MM-dd")}'                                                       ");
            qry.Append($"\n   AND t.eta <= '{enddate.ToString("yyyy-MM-dd")}'                                                       ");
            if(!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n   AND t.product LIKE '%{product}%'                                                                                                ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n   AND t.origin LIKE '%{origin}%'                                                                                                  ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n   AND t.sizes LIKE '%{sizes}%'                                                                                                    ");
            if (!string.IsNullOrEmpty(manager.Trim()))
                qry.Append($"\n   AND t.manager LIKE '%{manager}%'                                                                                                ");

            qry.Append($"\n ORDER BY t.eta, FIELD(t.cc_status, '확정', '통관', '', '미통관'), product, origin, sizes                                          ");
            

            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetArriveModel(dr);
        }
         
        private List<ArriveModel> GetArriveModel(MySqlDataReader rd)
        {
            List<ArriveModel> GetArriveModelList = new List<ArriveModel>();
            while (rd.Read())
            {
                ArriveModel model = new ArriveModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.sub_id = Convert.ToInt32(rd["sub_id"].ToString());
                model.score = Convert.ToInt32(rd["score"].ToString());
                model.pending_date = rd["pending_date"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();
                model.quantity_on_paper = Convert.ToDouble(rd["quantity_on_paper"].ToString());
                model.cc_status = rd["cc_status"].ToString();
                model.manager = rd["manager"].ToString();

                GetArriveModelList.Add(model);
            }
            rd.Close();
            return GetArriveModelList;
        }

        public DataTable GetProblemPending()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                      ");
            qry.Append($"\n   t.id                                                                                                                                      ");
            qry.Append($"\n , t.contract_year                                                                                                                           ");
            qry.Append($"\n , t.ato_no                                                                                                                                  ");
            qry.Append($"\n , IF(t.cc_status = '', '미통관', t.cc_status) AS cc_status                                                                                  ");
            qry.Append($"\n , t.eta AS pending_date                                                                                                                     ");
            qry.Append($"\n , t.manager                                                                                                                                 ");
            qry.Append($"\n , t.score                                                                                                                                   ");
            qry.Append($"\n , GROUP_CONCAT(product, '') AS product                                                                                                      ");
            qry.Append($"\n FROM(                                                                                                                                       ");
            qry.Append($"\n 	SELECT                                                                                                                                  ");
            qry.Append($"\n 	  id                                                                                                                                    ");
            qry.Append($"\n 	, contract_year                                                                                                                         ");
            qry.Append($"\n 	, ato_no                                                                                                                                ");
            qry.Append($"\n 	, cc_status                                                                                                                             ");
            qry.Append($"\n 	, CASE                                                                                                                                  ");
            qry.Append($"\n 		WHEN date_add(c.pending_check, INTERVAL 1 DAY) IS NOT NULL AND IFNULL(c.pending_check, '') <> '' THEN IFNULL(c.pending_check, '')    ");
            qry.Append($"\n 		WHEN IFNULL(c.eta, '') <> '' AND DATE_FORMAT(c.eta, '%y/%m/%d') IS NOT NULL THEN date_add(IFNULL(c.eta, ''), INTERVAL 5 DAY)                                                       ");
            qry.Append($"\n 		WHEN IFNULL(c.etd, '') <> '' AND DATE_FORMAT(c.etd, '%y/%m/%d') IS NOT NULL THEN date_add(c.etd, INTERVAL IFNULL(d.delivery_days, 15) + 5 DAY)                                     ");
            qry.Append($"\n 		WHEN IFNULL(c.shipment_date, '') <> '' THEN date_add(c.shipment_date, INTERVAL IFNULL(d.delivery_days, 15) + 5 DAY)                 ");
            qry.Append($"\n 	   END AS eta                                                                                                                           ");
            qry.Append($"\n 	, CASE                                                                                                                                  ");
            qry.Append($"\n 		WHEN date_add(c.pending_check, INTERVAL 1 DAY) IS NOT NULL AND IFNULL(c.pending_check, '') <> '' THEN 90                             ");
            qry.Append($"\n 		WHEN IFNULL(c.eta, '') <> '' THEN 80                                                                                                ");
            qry.Append($"\n 		WHEN IFNULL(c.etd, '') <> '' THEN 70                                                                                                ");
            qry.Append($"\n 		WHEN IFNULL(c.shipment_date, '') <> '' THEN 60                                                                                      ");
            qry.Append($"\n 	   END AS score                                                                                                                         ");
            qry.Append($"\n 	, manager                                                                                                                               ");
            qry.Append($"\n 	, concat(product, '|', origin, '|', sizes, '|', box_weight) AS product                                                                  ");
            qry.Append($"\n 	FROM t_customs AS c                                                                                                                     ");
            qry.Append($"\n 	LEFT OUTER JOIN t_country AS d                                                                                                          ");
            qry.Append($"\n 	  ON c.origin = d.country_name                                                                                                          ");
            qry.Append($"\n 	WHERE 1=1                                                                                                                               ");
            qry.Append($"\n 	  AND contract_year >= 2022                                                                                                             ");
            qry.Append($"\n       AND !(cc_status = '통관' OR cc_status = '확정')                                                                                       ");
            qry.Append($"\n       AND ato_no NOT LIKE '%취소%' AND ato_no NOT LIKE '%삭제%'                                                                             ");
            /*qry.Append($"\n       AND ato_no NOT LIKE '%dw%'                                                                                                            ");
            qry.Append($"\n       AND ato_no NOT LIKE '%jd%'                                                                                                            ");
            qry.Append($"\n       AND ato_no NOT LIKE '%hs%'                                                                                                            ");
            qry.Append($"\n       AND ato_no NOT LIKE '%od%'                                                                                                            ");*/
            qry.Append($"\n     ORDER BY c.product, c.origin, c.sizes, c.box_weight                                                            ");
            qry.Append($"\n ) AS t                                                                                                                                      ");
            qry.Append($"\n  WHERE IFNULL(t.eta, '') = '' OR t.eta < '{DateTime.Now.ToString("yyyy-MM-dd")}'                                                            ");
            qry.Append($"\n GROUP BY id, contract_year, ato_no, cc_status, pending_date, manager                                                                        ");
            qry.Append($"\n ORDER BY pending_date, ato_no                                                                                                               ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public List<ArriveModel> GetArriveDetails(DateTime sttdate, DateTime enddate, string product = null, string sizes = null, string origin = null, string manager = null, int id = 0, int sub_id = 0, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                 ");
            qry.Append($"\n   *                                                                                                    ");
            qry.Append($"\n FROM (                                                                                                 ");
            qry.Append($"\n SELECT                                                                                                 ");
            qry.Append($"\n   c1.id                                                                                                ");
            qry.Append($"\n , c1.sub_id                                                                                            ");
            qry.Append($"\n , c1.etd                                                                                               ");
            qry.Append($"\n , c1.eta                                                                                               ");
            qry.Append($"\n , c1.warehousing_date                                                                                  ");
            qry.Append($"\n , c1.pending_date                                                                                      ");
            qry.Append($"\n , c1.eta_status                                                                                        ");
            qry.Append($"\n , c1.origin                                                                                            ");
            qry.Append($"\n , c1.product                                                                                           ");
            qry.Append($"\n , c1.sizes                                                                                             ");
            qry.Append($"\n , c1.box_weight                                                                                        ");
            qry.Append($"\n , c1.quantity_on_paper                                                                                 ");
            qry.Append($"\n , c1.manager                                                                                           ");
            qry.Append($"\n , c1.cc_status                                                                                           ");
            qry.Append($"\n FROM(                                                                                                  ");
            qry.Append($"\n 	SELECT                                                                                             ");
            qry.Append($"\n 	  c1.id                                                                                            ");
            qry.Append($"\n 	, c1.sub_id                                                                                        ");
            qry.Append($"\n 	, c1.etd                                                                                           ");
            qry.Append($"\n 	, date_add(c1.etd, INTERVAL c1.delivery_days DAY) AS eta                                           ");
            qry.Append($"\n 	, '' AS warehousing_date                                                                           ");
            qry.Append($"\n 	, date_add(date_add(c1.etd, INTERVAL c1.delivery_days DAY), INTERVAL 5 DAY) AS pending_date        ");
            qry.Append($"\n 	, c1.eta_status                                                                                    ");
            qry.Append($"\n 	, c1.origin                                                                                        ");
            qry.Append($"\n 	, c1.product                                                                                       ");
            qry.Append($"\n 	, c1.sizes                                                                                         ");
            qry.Append($"\n 	, c1.box_weight                                                                                    ");
            qry.Append($"\n 	, c1.quantity_on_paper                                                                             ");
            qry.Append($"\n 	, c1.manager                                                                                       ");
            qry.Append($"\n 	, c1.cc_status                                                                                       ");
            qry.Append($"\n 	FROM(                                                                                              ");
            qry.Append($"\n 		SELECT                                                                                         ");
            qry.Append($"\n 		  c1.id                                                                                        ");
            qry.Append($"\n 		, c1.sub_id                                                                                    ");
            qry.Append($"\n 		, c1.etd                                                                                       ");
            qry.Append($"\n 		, '미확정(etd기준)' AS eta_status                                                                  ");
            qry.Append($"\n 		, c1.origin                                                                                    ");
            qry.Append($"\n 		, c1.product                                                                                   ");
            qry.Append($"\n 		, c1.sizes                                                                                     ");
            qry.Append($"\n 		, c1.box_weight                                                                                ");
            qry.Append($"\n 		, c1.quantity_on_paper                                                                         ");
            qry.Append($"\n 		, IF(c2.delivery_days IS NULL, 10, c2.delivery_days)  AS delivery_days                         ");
            qry.Append($"\n 		, c1.manager                                                                                   ");
            qry.Append($"\n 		, c1.cc_status                                                                                   ");
            qry.Append($"\n 		FROM t_customs AS c1                                                                           ");
            qry.Append($"\n 		LEFT OUTER JOIN t_country AS c2                                                                ");
            qry.Append($"\n 		  ON c1.origin = c2.country_name                                                               ");
            qry.Append($"\n 		WHERE 1=1                                                                                      ");
            qry.Append($"\n 		  AND (c1.pending_check IS NULL OR c1.pending_check = '')                                      ");
            qry.Append($"\n 		  AND (c1.eta IS NULL OR c1.eta = '')                                                          ");
            qry.Append($"\n 		  AND (c1.warehousing_date IS NULL OR c1.warehousing_date = '')                                ");
            qry.Append($"\n 	) AS c1                                                                                            "); 
            qry.Append($"\n 	WHERE date_add(date_add(c1.etd, INTERVAL c1.delivery_days DAY), INTERVAL 5 DAY) IS NOT NULL        ");
            qry.Append($"\n 	  AND date_add(date_add(c1.etd, INTERVAL c1.delivery_days DAY), INTERVAL 5 DAY) >= '{sttdate.ToString("yyyy-MM-dd")}'     ");
            qry.Append($"\n 	  AND date_add(date_add(c1.etd, INTERVAL c1.delivery_days DAY), INTERVAL 5 DAY) <= '{enddate.ToString("yyyy-MM-dd")}'     ");
            qry.Append($"\n 	UNION ALL                                                                                          ");
            qry.Append($"\n 	SELECT                                                                                             ");
            qry.Append($"\n 	  c1.id                                                                                            ");
            qry.Append($"\n 	, c1.sub_id                                                                                        ");
            qry.Append($"\n 	, c1.etd                                                                                           ");
            qry.Append($"\n 	, c1.eta                                                                                           ");
            qry.Append($"\n 	, c1.warehousing_date                                                                              ");
            qry.Append($"\n 	, date_add(c1.eta, INTERVAL 5 DAY) AS pending_date                                                 ");
            qry.Append($"\n 	, '미확정(eta기준)' AS eta_status                                                                  ");
            qry.Append($"\n 	, c1.origin                                                                                        ");
            qry.Append($"\n 	, c1.product                                                                                       ");
            qry.Append($"\n 	, c1.sizes                                                                                         ");
            qry.Append($"\n 	, c1.box_weight                                                                                    ");
            qry.Append($"\n 	, c1.quantity_on_paper                                                                             ");
            qry.Append($"\n 	, c1.manager                                                                                       ");
            qry.Append($"\n 	, c1.cc_status                                                                                     ");
            qry.Append($"\n 	FROM t_customs AS c1                                                                               ");
            qry.Append($"\n 	WHERE 1=1                                                                                          ");
            qry.Append($"\n 	  AND (c1.pending_check IS NULL OR c1.pending_check = '')                                          ");
            qry.Append($"\n 	  AND (c1.warehousing_date IS NULL OR c1.warehousing_date = '')                                    ");
            qry.Append($"\n 	  AND date_add(c1.eta, INTERVAL 5 DAY) >= '{sttdate.ToString("yyyy-MM-dd")}'                                              ");
            qry.Append($"\n 	  AND date_add(c1.eta, INTERVAL 5 DAY) <= '{enddate.ToString("yyyy-MM-dd")}'                                              ");
            qry.Append($"\n 	UNION ALL                                                                                          ");
            qry.Append($"\n 	SELECT                                                                                             ");
            qry.Append($"\n 	  c1.id                                                                                            ");
            qry.Append($"\n 	, c1.sub_id                                                                                        ");
            qry.Append($"\n 	, c1.etd                                                                                           ");
            qry.Append($"\n 	, c1.eta                                                                                           ");
            qry.Append($"\n 	, c1.warehousing_date                                                                              ");
            qry.Append($"\n 	, date_add(c1.warehousing_date, INTERVAL 5 DAY) AS pending_date                                    ");
            qry.Append($"\n 	, '미확정(창고입고일 기준)' AS eta_status                                                                   ");
            qry.Append($"\n 	, c1.origin                                                                                        ");
            qry.Append($"\n 	, c1.product                                                                                       ");
            qry.Append($"\n 	, c1.sizes                                                                                         ");
            qry.Append($"\n 	, c1.box_weight                                                                                    ");
            qry.Append($"\n 	, c1.quantity_on_paper                                                                             ");
            qry.Append($"\n 	, c1.manager                                                                                       ");
            qry.Append($"\n 	, c1.cc_status                                                                                     ");
            qry.Append($"\n 	FROM t_customs AS c1                                                                               ");
            qry.Append($"\n 	WHERE 1=1                                                                                          ");
            qry.Append($"\n 	  AND (c1.pending_check IS NULL OR c1.pending_check = '')                                          ");
            qry.Append($"\n 	  AND date_add(c1.warehousing_date, INTERVAL 5 DAY) >= '{sttdate.ToString("yyyy-MM-dd")}'          ");
            qry.Append($"\n 	  AND date_add(c1.warehousing_date, INTERVAL 5 DAY) <= '{enddate.ToString("yyyy-MM-dd")}'          ");
            qry.Append($"\n ) AS c1                                                                                                ");
            qry.Append($"\n WHERE 1=1                                                                                              ");
            if (id != 0)
            {
                qry.Append($"\t AND c1.id = {id}");
            }
            if (sub_id != 0)
            {
                qry.Append($"\t AND c1.sub_id = {sub_id}");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\t AND c1.product = '{product}'");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\t AND c1.sizes = '{sizes}'");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\t AND c1.origin = '{origin}'");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\t AND c1.manager = '{manager}'");
            }
            qry.Append($"\n UNION ALL                                                                                              ");
            qry.Append($"\n SELECT                                                                                                 ");
            qry.Append($"\n   c1.id                                                                                                ");
            qry.Append($"\n , c1.sub_id                                                                                            ");
            qry.Append($"\n , c1.etd                                                                                               ");
            qry.Append($"\n , c1.eta                                                                                               ");
            qry.Append($"\n , c1.warehousing_date                                                                                  ");
            qry.Append($"\n , c1.pending_check AS pending_date                                                                     ");
            qry.Append($"\n , '확정' AS eta_status                                                                                 ");
            qry.Append($"\n , c1.origin                                                                                            ");
            qry.Append($"\n , c1.product                                                                                           ");
            qry.Append($"\n , c1.sizes                                                                                             ");
            qry.Append($"\n , c1.box_weight                                                                                        ");
            qry.Append($"\n , c1.quantity_on_paper                                                                                 ");
            qry.Append($"\n , c1.manager                                                                                           ");
            qry.Append($"\n , c1.cc_status                                                                                           ");
            qry.Append($"\n FROM t_customs AS c1                                                                                   ");
            qry.Append($"\n WHERE 1=1                                                                                              ");
            qry.Append($"\n   AND c1.pending_check >= '{sttdate.ToString("yyyy-MM-dd")}'                                           ");
            qry.Append($"\n   AND c1.pending_check <= '{enddate.ToString("yyyy-MM-dd")}'                                           ");
            if (id != 0)
            {
                qry.Append($"\t AND c1.id = {id}");
            }
            if (sub_id != 0)
            {
                qry.Append($"\t AND c1.sub_id = {sub_id}");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\t AND c1.product = '{product}'");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\t AND c1.sizes = '{sizes}'");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\t AND c1.origin = '{origin}'");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\t AND c1.manager = '{manager}'");
            }
            qry.Append($"\n ) AS c1                                                                                                ");


            qry.Append($"\n ORDER BY c1.pending_date, c1.origin, c1.product, c1.sizes                                              ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetArriveDetailsModel(dr);
        }

        private List<ArriveModel> GetArriveDetailsModel(MySqlDataReader rd)
        {
            List<ArriveModel> GetArriveModelList = new List<ArriveModel>();
            while (rd.Read())
            {
                ArriveModel model = new ArriveModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.sub_id = Convert.ToInt32(rd["sub_id"].ToString());
                model.etd = rd["etd"].ToString();
                model.eta = rd["eta"].ToString();
                model.warehousing_date = rd["warehousing_date"].ToString();
                model.pending_date = rd["pending_date"].ToString();
                model.eta_status = rd["eta_status"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();
                model.quantity_on_paper = Convert.ToDouble(rd["quantity_on_paper"].ToString());
                model.manager = rd["manager"].ToString();
                model.cc_status = rd["cc_status"].ToString();

                GetArriveModelList.Add(model); ;
            }
            rd.Close();
            return GetArriveModelList;
        }

        public StringBuilder UpdatePending(AllCustomsModel model)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET                                            ");
            qry.Append($"\n   warehouse              =  '{model.warehouse}'               ");
            qry.Append($"\n , broker                 =  '{model.broker}'                  ");
            qry.Append($"\n , origin                 =  '{model.origin}'                  ");
            qry.Append($"\n , product                =  '{model.product}'                 ");
            qry.Append($"\n , sizes                  =  '{model.sizes}'                   ");
            qry.Append($"\n , box_weight             =  '{model.box_weight}'              ");
            qry.Append($"\n , unit_price             =   {model.unit_price}               ");
            qry.Append($"\n , quantity_on_paper      =   {model.quantity_on_paper}        ");
            qry.Append($"\n , qty                    =   {model.qty}                      ");
            qry.Append($"\n , tariff_rate            =   {model.tariff_rate}              ");
            qry.Append($"\n , vat_rate               =   {model.vat_rate}                 ");
            qry.Append($"\n , clearance_rate         =   {model.clearance_rate}           ");
            qry.Append($"\n , loading_cost_per_box   =   {model.loading_cost_per_box}     ");
            qry.Append($"\n , refrigeration_charge   =   {model.refrigeration_charge}     ");
            qry.Append($"\n , remark                 =  '{model.remark}'                  ");
            qry.Append($"\n , trq_amount             =   {model.trq_amount}               ");
            qry.Append($"\n WHERE  id = {model.id}                                        ");
            qry.Append($"\n   AND  sub_id = {model.sub_id}                                ");

            string sql = qry.ToString();
            return qry;
        }

        public DataTable GetUnpendingProduct(string product, string origin, string sizes, string unit, bool integrated_unit, bool isExactly)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                               ");
            qry.Append($"\n	   t.product                                                                                                                          ");
            qry.Append($"\n  , t.origin                                                                                                                           ");
            qry.Append($"\n  , t.sizes                                                                                                                            ");
            qry.Append($"\n  , IF(t.weight_calculate = 'FALSE' AND cost_unit = 0, 'TRUE', t.weight_calculate) AS weight_calculate                                 ");
            qry.Append($"\n  , t.box_weight                                                                                                                       ");
            qry.Append($"\n  , t.cost_unit                                                                                                                        ");
            qry.Append($"\n  , t.unit_price                                                                                                                       ");
            qry.Append($"\n  , t.contract_no                                                                                                                      ");
            qry.Append($"\n  , t.ato_no                                                                                                                           ");
            qry.Append($"\n  , t.bl_no                                                                                                                            ");
            qry.Append($"\n  , t.delivery_days                                                                                                                    ");
            qry.Append($"\n  , t.etd                                                                                                                              ");
            qry.Append($"\n  , t.eta                                                                                                                              ");
            qry.Append($"\n  , t.quantity_on_paper                                                                                                                ");
            qry.Append($"\n  , t.warehousing_date                                                                                                                 ");
            qry.Append($"\n  FROM (                                                                                                                               ");
            qry.Append($"\n      SELECT                                                                                                                           ");
            qry.Append($"\n        product                                                                                                                        ");
            qry.Append($"\n      , origin                                                                                                                         ");
            qry.Append($"\n      , sizes                                                                                                                          ");
            qry.Append($"\n      , IF(IFNULL(weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate                                                       ");
            qry.Append($"\n      , box_weight                                                                                                                     ");
            qry.Append($"\n      , IF(IFNULL(cost_unit, 0) = 0, 0, cost_unit) AS cost_unit                                                                        ");
            qry.Append($"\n      , unit_price                                                                                                                     ");
            qry.Append($"\n      , contract_no                                                                                                                    ");
            qry.Append($"\n      , ato_no                                                                                                                         ");
            qry.Append($"\n      , bl_no                                                                                                                          ");
            qry.Append($"\n      , IFNULL(t.delivery_days, 15) AS delivery_days                                                                                   ");
            qry.Append($"\n      , CASE WHEN IFNULL(etd, '') <> '' THEN IFNULL(etd, '')                                                                           ");
            //qry.Append($"\n             WHEN IFNULL(etd, '') = '' AND IFNULL(eta, '') != '' THEN DATE_ADD(eta, INTERVAL  -IFNULL(t.delivery_days, 15) DAY)        ");
            qry.Append($"\n             WHEN IFNULL(shipment_date, '') <> '' THEN IFNULL(shipment_date, '')                                                       ");
            qry.Append($"\n  		ELSE IFNULL(etd, '') END etd                                                                                                  ");
            /*qry.Append($"\n      , CASE WHEN IFNULL(eta, '') <> '' THEN IFNULL(eta, '')                                                                           ");
            qry.Append($"\n             WHEN IFNULL(eta, '') = '' AND IFNULL(etd, '') != '' THEN DATE_ADD(etd, INTERVAL  IFNULL(t.delivery_days, 15) DAY)         ");
            qry.Append($"\n             WHEN IFNULL(shipment_date, '') != '' THEN DATE_ADD(shipment_date, INTERVAL  IFNULL(t.delivery_days, 15) DAY)              ");
            qry.Append($"\n  		ELSE IFNULL(eta, '') END eta                                                                                                  ");*/
            qry.Append($"\n  	 , IFNULL(eta, '') AS        eta                                                                                                  "); 
            qry.Append($"\n      , quantity_on_paper AS quantity_on_paper                                                                                         ");
            qry.Append($"\n      , warehousing_date AS warehousing_date                                                                                         ");
            qry.Append($"\n      FROM t_customs AS c                                                                                                              ");
            qry.Append($"\n      LEFT OUTER JOIN t_country AS t                                                                                                   ");
            qry.Append($"\n         ON c.origin = t.country_name                                                                                                  ");
            qry.Append($"\n      WHERE cc_status != '통관' AND cc_status != '확정'                                                                                    ");
            qry.Append($"\n	   AND CASE WHEN ato_no LIKE 'HS%' THEN FALSE                ");
            qry.Append($"\n				WHEN ato_no LIKE 'DLA%' THEN TRUE                ");
            qry.Append($"\n			    WHEN ato_no LIKE 'DL%' THEN FALSE                ");
            qry.Append($"\n                WHEN ato_no LIKE 'LT%' THEN FALSE             ");
            qry.Append($"\n                ELSE TRUE END = TRUE                          ");
            qry.Append($"\n  	   AND quantity_on_paper > 0                                                                                                      ");
            qry.Append($"\n  	   AND IFNULL(c.is_shipment_qty, 1) = 1                                                                                                      ");
            if (!isExactly)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   {commonRepository.whereSql("product", product)}             ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   {commonRepository.whereSql("origin", origin)}             ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n   {commonRepository.whereSql("sizes", sizes)}             ");
                if (!integrated_unit && !string.IsNullOrEmpty(unit))
                    qry.Append($"\n   {commonRepository.whereSql("box_weight", unit)}             ");
            }
            else
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND product = '{product}'             ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   AND origin = '{origin}'             ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n   AND sizes = '{sizes}'             ");
                if (!integrated_unit && !string.IsNullOrEmpty(unit))
                    qry.Append($"\n   AND box_weight = '{unit}'             ");
            }
            qry.Append($"\n  ) AS t                                                                                                                               ");
            qry.Append($"\n  ORDER BY IF(t.etd = '', '2999-12-31', t.etd), ato_no                                                                                 ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetUnpendingProduct3(string product, string origin, string sizes, string unit, string sub_product, bool isMerge, bool isExactly, bool isSimulation = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                               ");
            qry.Append($"\n	   t.id                                                                                                                          ");
            qry.Append($"\n  , t.product                                                                                                                           ");
            qry.Append($"\n  , t.origin                                                                                                                           ");
            qry.Append($"\n  , t.sizes                                                                                                                            ");
            qry.Append($"\n  , IF(t.weight_calculate = 'FALSE' AND cost_unit = 0, 'TRUE', t.weight_calculate) AS weight_calculate                                 ");
            qry.Append($"\n  , t.box_weight                                                                                                                       ");
            qry.Append($"\n  , t.cost_unit                                                                                                                        ");
            qry.Append($"\n  , t.unit_price                                                                                                                       ");
            qry.Append($"\n  , t.contract_no                                                                                                                      ");
            qry.Append($"\n  , t.ato_no                                                                                                                           ");
            qry.Append($"\n  , t.bl_no                                                                                                                            ");
            qry.Append($"\n  , t.delivery_days                                                                                                                    ");
            qry.Append($"\n  , t.etd                                                                                                                              ");
            qry.Append($"\n  , t.eta                                                                                                                              ");
            qry.Append($"\n  , t.quantity_on_paper                                                                                                                ");
            qry.Append($"\n  , t.warehousing_date                                                                                                                 ");
            qry.Append($"\n  , t.domestic_id                                                                                                                 ");
            qry.Append($"\n  , t.cost_price                                                                                                                 ");
            qry.Append($"\n  , t.is_domestic                                                                                                                 ");
            qry.Append($"\n  , t.shipper                                                                                                                 ");
            qry.Append($"\n  FROM (                                                                                                                               ");
            qry.Append($"\n      SELECT                                                                                                                           ");
            qry.Append($"\n        id                                                                                                                             ");
            qry.Append($"\n      , product                                                                                                                        ");
            qry.Append($"\n      , origin                                                                                                                         ");
            qry.Append($"\n      , sizes                                                                                                                          ");
            qry.Append($"\n      , IF(IFNULL(weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate                                                       ");
            qry.Append($"\n      , box_weight                                                                                                                     ");
            qry.Append($"\n      , IF(IFNULL(cost_unit, 0) = 0, 0, cost_unit) AS cost_unit                                                                        ");
            qry.Append($"\n      , unit_price                                                                                                                     ");
            qry.Append($"\n      , contract_no                                                                                                                    ");
            qry.Append($"\n      , ato_no                                                                                                                         ");
            qry.Append($"\n      , bl_no                                                                                                                          ");
            qry.Append($"\n      , IFNULL(t.delivery_days, 15) AS delivery_days                                                                                   ");
            qry.Append($"\n      , CASE WHEN IFNULL(etd, '') <> '' THEN IFNULL(etd, '')                                                                           ");
            //qry.Append($"\n             WHEN IFNULL(etd, '') = '' AND IFNULL(eta, '') != '' THEN DATE_ADD(eta, INTERVAL  -IFNULL(t.delivery_days, 15) DAY)        ");
            qry.Append($"\n             WHEN IFNULL(shipment_date, '') <> '' THEN IFNULL(shipment_date, '')                                                       ");
            qry.Append($"\n  		ELSE IFNULL(etd, '') END etd                                                                                                  ");
            /*qry.Append($"\n      , CASE WHEN IFNULL(eta, '') <> '' THEN IFNULL(eta, '')                                                                           ");
            qry.Append($"\n             WHEN IFNULL(eta, '') = '' AND IFNULL(etd, '') != '' THEN DATE_ADD(etd, INTERVAL  IFNULL(t.delivery_days, 15) DAY)         ");
            qry.Append($"\n             WHEN IFNULL(shipment_date, '') != '' THEN DATE_ADD(shipment_date, INTERVAL  IFNULL(t.delivery_days, 15) DAY)              ");
            qry.Append($"\n  		ELSE IFNULL(eta, '') END eta                                                                                                  ");*/
            qry.Append($"\n  	 , IFNULL(eta, '') AS        eta                                                                                                  ");
            qry.Append($"\n      , quantity_on_paper AS quantity_on_paper                                                                                         ");
            qry.Append($"\n      , warehousing_date AS warehousing_date                                                                                         ");
            qry.Append($"\n      , 0 AS domestic_id                                                                                         ");
            qry.Append($"\n      , 0 AS cost_price                                                                                         ");
            qry.Append($"\n      , 'FALSE' AS is_domestic                                                                                         ");
            qry.Append($"\n      , shipper AS shipper                                                                                         ");
            qry.Append($"\n      FROM t_customs AS c                                                                                                              ");
            qry.Append($"\n      LEFT OUTER JOIN t_country AS t                                                                                                   ");
            qry.Append($"\n         ON c.origin = t.country_name                                                                                                  ");
            qry.Append($"\n      WHERE cc_status != '통관' AND cc_status != '확정'                                                                                    ");
            qry.Append($"\n        AND c.ato_no NOT LIKE '%삭제%' AND c.ato_no NOT LIKE '%취소%'                                                                                     ");
            //2023-08-03 소희대리님 요청으로 팬딩수정 체크박스 활용
            /*qry.Append($"\n	   AND CASE WHEN ato_no LIKE 'HS%' THEN FALSE                ");
            qry.Append($"\n				WHEN ato_no LIKE 'DLA%' THEN TRUE                ");
            qry.Append($"\n			    WHEN ato_no LIKE 'DL%' THEN FALSE                ");
            qry.Append($"\n                WHEN ato_no LIKE 'LT%' THEN FALSE             ");
            qry.Append($"\n                ELSE TRUE END = TRUE                          ");*/
            qry.Append($"\n  	   AND quantity_on_paper > 0                                                                                                      ");
            qry.Append($"\n  	   AND IFNULL(c.is_shipment_qty, 1) = 1                                                                                                      ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n       AND product = '{product}'                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n       AND origin = '{origin}'                                                                     ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n       AND sizes = '{sizes}'                                                                     ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n       AND box_weight = '{unit}'                                                                     ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n       AND (                                                                     ");
                    for (int i = 0; i < products.Length; i++)
                    {
                        if (i == 0)
                            qry.Append($"\n  (");
                        else
                            qry.Append($"\n  OR (");
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            qry.Append($" product = '{sub[0]}'");
                            qry.Append($" AND origin = '{sub[1]}'");
                            qry.Append($" AND sizes = '{sub[2]}'");
                            qry.Append($" AND box_weight = '{sub[6]}'");
                            qry.Append($")");
                        }
                    }
                    qry.Append($"\n  )");
                }
            }
            if (isSimulation)
            {
                qry.Append($"\n    UNION ALL                                 ");
                qry.Append($"\n    SELECT                                 ");
                qry.Append($"\n      id                                   ");
                qry.Append($"\n    , product                              ");
                qry.Append($"\n    , origin                               ");
                qry.Append($"\n    , sizes                                ");
                qry.Append($"\n    , 'TRUE'                               ");
                qry.Append($"\n    , seaover_unit AS box_weight           ");
                qry.Append($"\n    , ''                                   ");
                qry.Append($"\n    , ''                                   ");
                qry.Append($"\n    , ''                                   ");
                qry.Append($"\n    , ''                                   ");
                qry.Append($"\n    , ''                                   ");
                qry.Append($"\n    , ''                                   ");
                qry.Append($"\n    , etd                                  ");
                qry.Append($"\n    , ''                                   ");
                qry.Append($"\n    , qty                                  ");
                qry.Append($"\n    , warehousing_date                     ");
                qry.Append($"\n    , id                                   ");
                qry.Append($"\n    , cost_price                           ");
                qry.Append($"\n    , 'TRUE' AS is_domestic                                                                                         ");
                qry.Append($"\n    , '' AS shipper                                                                                         ");
                qry.Append($"\n    FROM t_domestic                        ");
                qry.Append($"\n    WHERE 1=1                              ");
                if (!isMerge)
                {
                    if (!string.IsNullOrEmpty(product))
                        qry.Append($"\n       AND product = '{product}'                                                                     ");
                    if (!string.IsNullOrEmpty(origin))
                        qry.Append($"\n       AND origin = '{origin}'                                                                     ");
                    if (!string.IsNullOrEmpty(sizes))
                        qry.Append($"\n       AND sizes = '{sizes}'                                                                     ");
                    if (!string.IsNullOrEmpty(unit))
                        qry.Append($"\n       AND seaover_unit = '{unit}'                                                                     ");
                }
                else
                {
                    string[] products = sub_product.Trim().Split('\n');
                    if (product.Length > 0)
                    {
                        qry.Append($"\n       AND (                                                                     ");
                        for (int i = 0; i < products.Length; i++)
                        {
                            if (i == 0)
                                qry.Append($"\n  (");
                            else
                                qry.Append($"\n  OR (");
                            string[] sub = products[i].Trim().Split('^');
                            if (sub.Length > 3)
                            {
                                qry.Append($" product = '{sub[0]}'");
                                qry.Append($" AND origin = '{sub[1]}'");
                                qry.Append($" AND sizes = '{sub[2]}'");
                                qry.Append($" AND seaover_unit = '{sub[6]}'");
                                qry.Append($")");
                            }
                        }
                        qry.Append($"\n  )");
                    }
                }
                qry.Append($"\n    AND DATE_ADD(CAST(warehousing_date AS DATETIME) , INTERVAL 15 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}'       ");
            }
            qry.Append($"\n  ) AS t                                                                                                                               ");
            qry.Append($"\n  ORDER BY IF(t.etd = '', '2999-12-31', t.etd), ato_no                                                                                 ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetUnpendingProduct2(string product, string origin, string sizes, string unit, bool integrated_unit, bool isExactly, bool isMerge = false, string sub_product = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                               ");
            qry.Append($"\n	   t.product                                                                                                                          ");
            qry.Append($"\n  , t.origin                                                                                                                           ");
            qry.Append($"\n  , t.sizes                                                                                                                            ");
            qry.Append($"\n  , IF(t.weight_calculate = 'FALSE' AND cost_unit = 0, 'TRUE', t.weight_calculate) AS weight_calculate                                 ");
            qry.Append($"\n  , t.box_weight                                                                                                                       ");
            qry.Append($"\n  , t.cost_unit                                                                                                                        ");
            qry.Append($"\n  , t.unit_price                                                                                                                       ");
            qry.Append($"\n  , t.cost_price                                                                                                                       ");
            qry.Append($"\n  , t.contract_no                                                                                                                      ");
            qry.Append($"\n  , t.ato_no                                                                                                                           ");
            qry.Append($"\n  , t.bl_no                                                                                                                            ");
            qry.Append($"\n  , t.delivery_days                                                                                                                    ");
            qry.Append($"\n  , t.etd                                                                                                                              ");
            qry.Append($"\n  , t.eta                                                                                                                              ");
            qry.Append($"\n  , t.quantity_on_paper                                                                                                                ");
            qry.Append($"\n  , t.warehousing_date                                                                                                                 ");
            qry.Append($"\n  FROM (                                                                                                                               ");
            qry.Append($"\n      SELECT                                                                                                                           ");
            qry.Append($"\n        product                                                                                                                        ");
            qry.Append($"\n      , origin                                                                                                                         ");
            qry.Append($"\n      , sizes                                                                                                                          ");
            qry.Append($"\n      , IF(IFNULL(weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate                                                       ");
            qry.Append($"\n      , box_weight                                                                                                                     ");
            qry.Append($"\n      , IF(IFNULL(cost_unit, 0) = 0, 0, cost_unit) AS cost_unit                                                                        ");
            qry.Append($"\n      , unit_price                                                                                                                     ");
            qry.Append($"\n      , 0 AS cost_price                                                                                                                ");
            qry.Append($"\n      , contract_no                                                                                                                    ");
            qry.Append($"\n      , ato_no                                                                                                                         ");
            qry.Append($"\n      , bl_no                                                                                                                          ");
            qry.Append($"\n      , IFNULL(t.delivery_days, 15) AS delivery_days                                                                                   ");
            qry.Append($"\n      , CASE WHEN IFNULL(etd, '') <> '' THEN IFNULL(etd, '')                                                                           ");
            //qry.Append($"\n             WHEN IFNULL(etd, '') = '' AND IFNULL(eta, '') != '' THEN DATE_ADD(eta, INTERVAL  -IFNULL(t.delivery_days, 15) DAY)        ");
            qry.Append($"\n             WHEN IFNULL(shipment_date, '') <> '' THEN IFNULL(shipment_date, '')                                                       ");
            qry.Append($"\n  		ELSE IFNULL(etd, '') END etd                                                                                                  ");
            /*qry.Append($"\n      , CASE WHEN IFNULL(eta, '') <> '' THEN IFNULL(eta, '')                                                                           ");
            qry.Append($"\n             WHEN IFNULL(eta, '') = '' AND IFNULL(etd, '') != '' THEN DATE_ADD(etd, INTERVAL  IFNULL(t.delivery_days, 15) DAY)         ");
            qry.Append($"\n             WHEN IFNULL(shipment_date, '') != '' THEN DATE_ADD(shipment_date, INTERVAL  IFNULL(t.delivery_days, 15) DAY)              ");
            qry.Append($"\n  		ELSE IFNULL(eta, '') END eta                                                                                                  ");*/
            qry.Append($"\n  	 , IFNULL(eta, '') AS        eta                                                                                                  ");
            qry.Append($"\n      , quantity_on_paper AS quantity_on_paper                                                                                         ");
            qry.Append($"\n      , warehousing_date AS warehousing_date                                                                                         ");
            qry.Append($"\n      FROM t_customs AS c                                                                                                              ");
            qry.Append($"\n      LEFT OUTER JOIN t_country AS t                                                                                                   ");
            qry.Append($"\n         ON c.origin = t.country_name                                                                                                  ");
            qry.Append($"\n      WHERE cc_status != '통관' AND cc_status != '확정'                                                                                    ");
            qry.Append($"\n	       AND c.ato_no NOT LIKE '%삭제%' AND c.ato_no NOT LIKE '%취소%'               ");
            //2023-08-03 소회대리님 요청으로 팬딩수정에 체크박스를 활용
            /*qry.Append($"\n	   AND CASE WHEN ato_no LIKE 'HS%' THEN FALSE                ");
            qry.Append($"\n				WHEN ato_no LIKE 'DLA%' THEN TRUE                ");
            qry.Append($"\n			    WHEN ato_no LIKE 'DL%' THEN FALSE                ");
            qry.Append($"\n                WHEN ato_no LIKE 'LT%' THEN FALSE             ");
            qry.Append($"\n                ELSE TRUE END = TRUE                          ");*/
            qry.Append($"\n  	   AND quantity_on_paper > 0                                                                                                      ");
            qry.Append($"\n  	   AND IFNULL(c.is_shipment_qty, 1) = 1                                                                                                      ");
            if (!isMerge)
            {
                if (!isExactly)
                {
                    if (!string.IsNullOrEmpty(product))
                        qry.Append($"\n   {commonRepository.whereSql("product", product)}             ");
                    if (!string.IsNullOrEmpty(origin))
                        qry.Append($"\n   {commonRepository.whereSql("origin", origin)}             ");
                    if (!string.IsNullOrEmpty(sizes))
                        qry.Append($"\n   {commonRepository.whereSql("sizes", sizes)}             ");
                    /*if (!integrated_unit && !string.IsNullOrEmpty(unit))
                        qry.Append($"\n   {whereSql("box_weight", unit)}             ");*/
                }
                else
                {
                    if (!string.IsNullOrEmpty(product))
                        qry.Append($"\n   AND product = '{product}'             ");
                    if (!string.IsNullOrEmpty(origin))
                        qry.Append($"\n   AND origin = '{origin}'             ");
                    if (!string.IsNullOrEmpty(sizes))
                        qry.Append($"\n   AND sizes = '{sizes}'             ");
                    /*if (!integrated_unit && !string.IsNullOrEmpty(unit))
                        qry.Append($"\n   AND box_weight = '{unit}'             ");*/
                }
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n       AND (                                                                     ");
                    for (int i = 0; i < products.Length; i++)
                    {
                        if (i == 0)
                            qry.Append($"\n  (");
                        else
                            qry.Append($"\n  OR (");
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            qry.Append($" product = '{sub[0]}'");
                            qry.Append($" AND origin = '{sub[1]}'");
                            qry.Append($" AND sizes = '{sub[2]}'");
                            qry.Append($" AND box_weight = '{sub[6]}'");
                            qry.Append($")");
                        }
                    }
                    qry.Append($"\n  )");
                }
            }
            /*qry.Append($"\n     UNION ALL                                      ");
            qry.Append($"\n     SELECT                                         ");
            qry.Append($"\n        product                                     ");
            qry.Append($"\n      , origin                                      ");
            qry.Append($"\n      , sizes                                       ");
            qry.Append($"\n      , 'TRUE' AS weight_calculate                  ");
            qry.Append($"\n      , seaover_unit                                ");
            qry.Append($"\n      , 0 AS cost_unit                              ");
            qry.Append($"\n      , 0 AS unit_price                             ");
            qry.Append($"\n      , cost_price                                  ");
            qry.Append($"\n      , '' AS contract_no                           ");
            qry.Append($"\n      , '' AS ato_no                                ");
            qry.Append($"\n      , '' AS bl_no                                 ");
            qry.Append($"\n      , 0 AS delivery_days                          ");
            qry.Append($"\n      , '' AS etd                                   ");
            qry.Append($"\n  	  , ''  AS eta                                 ");
            qry.Append($"\n      , qty AS quantity_on_paper                    ");
            qry.Append($"\n      , warehousing_date AS warehousing_date        ");
            qry.Append($"\n     FROM t_domestic                                ");
            qry.Append($"\n     WHERE 1=1                                      ");
            qry.Append($"\n       AND DATE_ADD(CAST(warehousing_date AS DATETIME) , INTERVAL 15 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}'                                      ");
            
            if (!isExactly)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   {whereSql("product", product)}             ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   {whereSql("origin", origin)}             ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n   {whereSql("sizes", sizes)}             ");
            }
            else
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND product = '{product}'             ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   AND origin = '{origin}'             ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n   AND sizes = '{sizes}'             ");
            }*/

            qry.Append($"\n  ) AS t                                                                                                                               ");
            qry.Append($"\n  ORDER BY IF(t.etd = '', '2999-12-31', t.etd), ato_no                                                                                 ");

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

        public DataTable GetUnpending(string product, string origin, string sizes, string unit, int shipDays = 0)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                                   ");
            qry.Append($"\n   t.*                                                                                                                                 ");
            qry.Append($"\n , IF(t.eta != '', DATE_ADD(etd, INTERVAL 5 DAY), '') AS warehousing_date                                                              ");
            qry.Append($"\n FROM (                                                                                                                                ");
            qry.Append($"\n     SELECT                                                                                                                            ");
            qry.Append($"\n       contract_no                                                                                                                     ");
            qry.Append($"\n     , ato_no                                                                                                                          ");
            qry.Append($"\n     , bl_no                                                                                                                           ");
            qry.Append($"\n     , IFNULL(t.delivery_days, 15) AS delivery_days                                                                                    ");
            if (shipDays > 0)
            {
                qry.Append($"\n     , CASE WHEN IFNULL(etd, '') <> '' THEN IFNULL(etd, '')                                                                        ");
                qry.Append($"\n            WHEN IFNULL(etd, '') = '' AND IFNULL(eta, '') != '' THEN DATE_ADD(eta, INTERVAL  -{shipDays} DAY)                      ");
                qry.Append($"\n            WHEN IFNULL(shipment_date, '') <> '' THEN IFNULL(shipment_date, '')                                                    ");
                qry.Append($"\n			ELSE IFNULL(etd, '') END etd                                                                                              ");
                qry.Append($"\n     , CASE WHEN IFNULL(eta, '') <> '' THEN IFNULL(eta, '')                                                                        ");
                qry.Append($"\n            WHEN IFNULL(eta, '') = '' AND IFNULL(etd, '') != '' THEN DATE_ADD(etd, INTERVAL  {shipDays} DAY)                       ");
                qry.Append($"\n            WHEN IFNULL(shipment_date, '') != '' THEN DATE_ADD(shipment_date, INTERVAL  {shipDays} DAY)                            ");
                qry.Append($"\n			ELSE IFNULL(eta, '') END eta                                                                                              ");
            }
            else
            {
                qry.Append($"\n     , CASE WHEN IFNULL(etd, '') <> '' THEN IFNULL(etd, '')                                                                        ");
                qry.Append($"\n            WHEN IFNULL(etd, '') = '' AND IFNULL(eta, '') != '' THEN DATE_ADD(eta, INTERVAL  -IFNULL(t.delivery_days, 15) DAY)     ");
                qry.Append($"\n            WHEN IFNULL(shipment_date, '') <> '' THEN IFNULL(shipment_date, '')                                                    ");
                qry.Append($"\n			ELSE IFNULL(etd, '') END etd                                                                                              ");
                qry.Append($"\n     , CASE WHEN IFNULL(eta, '') <> '' THEN IFNULL(eta, '')                                                                        ");
                qry.Append($"\n            WHEN IFNULL(eta, '') = '' AND IFNULL(etd, '') != '' THEN DATE_ADD(etd, INTERVAL  IFNULL(t.delivery_days, 15) DAY)      ");
                qry.Append($"\n            WHEN IFNULL(shipment_date, '') != '' THEN DATE_ADD(shipment_date, INTERVAL  IFNULL(t.delivery_days, 15) DAY)           ");
                qry.Append($"\n			ELSE IFNULL(eta, '') END eta                                                                                              ");
            }  
            qry.Append($"\n     , quantity_on_paper AS quantity_on_paper                                                                                          ");
            qry.Append($"\n     FROM t_customs AS c                                                                                                               ");
            qry.Append($"\n     LEFT OUTER JOIN t_country AS t                                                                                                    ");
            qry.Append($"\n        ON c.origin = t.country_name                                                                                                   ");
            qry.Append($"\n     WHERE cc_status != '통관' AND cc_status != '확정'                                                                                 ");
            qry.Append($"\n       AND (warehousing_date IS NULL OR warehousing_date = '')                                                                         ");
            //제외 아토번호
            qry.Append($"\n       AND NOT ( ato_no LIKE 'HS%' OR ato_no LIKE 'LT%')                                                                               ");

            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n       AND product = '{product}'             ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n       AND origin = '{origin}'             ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n       AND sizes = '{sizes}'             ");
            }
            if (!string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n       AND box_weight = '{unit}'             ");
            }
            qry.Append($"\n ) AS t               ");
            qry.Append($"\n ORDER BY IF(t.etd = '', '2999-12-31', t.etd), ato_no                                ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }




        public DataTable GetPendingPriceList(string sdate, string product, string origin, string sizes, string unit)
        {
            string[] tempStr = null;
            string tempWhr = "";
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                  ");
            qry.Append($"\n   t1.product                                                            ");
            qry.Append($"\n , t1.origin                                                             ");
            qry.Append($"\n , t1.sizes                                                              ");
            qry.Append($"\n , t1.box_weight                                                         ");
            qry.Append($"\n , t1.cost_unit                                                          ");
            qry.Append($"\n , CASE WHEN t1.cost_unit > 0 THEN t1.cost_unit                          ");
            qry.Append($"\n        ELSE t1.box_weight END AS unit                                   ");
            qry.Append($"\n , t1.unit_price                                                         ");
            qry.Append($"\n , IFNULL(t2.exchange_rate, 0) AS exchange_rate                          ");
            qry.Append($"\n FROM(                                                                   ");
            qry.Append($"\n 	SELECT                                                              ");
            qry.Append($"\n 	  product                                                           ");
            qry.Append($"\n 	, origin                                                            ");
            qry.Append($"\n 	, sizes                                                             ");
            qry.Append($"\n 	, IFNULL(box_weight, 0) AS box_weight                               ");
            qry.Append($"\n 	, IFNULL(cost_unit, 0) AS cost_unit                                 ");
            qry.Append($"\n 	, unit_price                                                        ");
            qry.Append($"\n     , NOW() AS sdate                                                    ");
            qry.Append($"\n 	FROM t_customs                                                      ");
            qry.Append($"\n 	WHERE cc_status <> '결제완료'                                       ");
            qry.Append($"\n 	GROUP BY product, origin, sizes, box_weight, cost_unit              ");
            qry.Append($"\n 	UNION ALL                                                           ");
            qry.Append($"\n 	SELECT                                                              ");
            qry.Append($"\n 	  product                                                           ");
            qry.Append($"\n 	, origin                                                            ");
            qry.Append($"\n 	, sizes                                                             ");
            qry.Append($"\n 	, IFNULL(box_weight, 0) AS box_weight                               ");
            qry.Append($"\n 	, IFNULL(cost_unit, 0) AS cost_unit                                 ");
            qry.Append($"\n 	, unit_price                                                        ");
            qry.Append($"\n     , payment_date AS sdate                                             ");
            qry.Append($"\n 	FROM t_customs                                                      ");
            qry.Append($"\n 	WHERE cc_status = '결제완료'                                        ");
            qry.Append($"\n 	  AND payment_date >= '{sdate}'                                     ");
            qry.Append($"\n 	GROUP BY product, origin, sizes, box_weight, cost_unit              ");
            qry.Append($"\n ) AS t1                                                                 ");
            qry.Append($"\n LEFT OUTER JOIN t_finance AS t2                                         ");
            qry.Append($"\n   ON t1.sdate = t2.base_date                                            ");
            qry.Append($"\n WHERE 1=1                                                               ");
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
                            tempWhr = $"\n	   t1.product  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t1.product   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t1.product   LIKE '%{product}%'                                                                         ");
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
                            tempWhr = $"\n	   t1.origin   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t1.origin   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t1.origin   LIKE '%{origin}%'                                                                         ");
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
                            tempWhr = $"\n	   t1.sizes   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t1.sizes   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t1.sizes   LIKE '%{sizes}%'                                                                         ");
                }
            }

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetPendingProductByAtono(string ato_no, string product, string origin, string sizes, string unit)
        {
            string[] tempStr = null;
            string tempWhr = "";
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                ");
            qry.Append($"\n   ato_no                                                                              ");
            qry.Append($"\n , product                                                                             ");
            qry.Append($"\n , origin                                                                              ");
            qry.Append($"\n , sizes                                                                               ");
            qry.Append($"\n , box_weight AS unit                                                                  ");
            qry.Append($"\n , CASE WHEN cost_unit > 0 THEN box_weight / cost_unit ELSE box_weight END AS unit2    ");
            qry.Append($"\n , IF(cost_unit IS NULL OR cost_unit = '', 0, cost_unit) AS cost_unit                  ");
            qry.Append($"\n , unit_price                                                                          ");
            qry.Append($"\n FROM t_customs                                                                        ");
            qry.Append($"\n WHERE 1=1                                                                             ");
            qry.Append($"\n   AND cc_status <> '확정'                                                               ");
            qry.Append($"\n   AND cc_status <> '통관'                                                               ");
            qry.Append($"\n   AND bl_no IS NOT NULL AND bl_no <> ''                                               ");
            qry.Append($"\n   AND warehousing_date <> ''                                                          ");
            qry.Append($"\n   AND warehousing_date IS NOT NULL                                                    ");
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
                            tempWhr = $"\n	   product  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR product   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND product   LIKE '%{product}%'                                                                         ");
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
                            tempWhr = $"\n	   origin   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR origin   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND origin   LIKE '%{origin}%'                                                                         ");
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
                            tempWhr = $"\n	   sizes   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR sizes   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND sizes   LIKE '%{sizes}%'                                                                         ");
                }
            }

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetPayInfo(string division)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                ");
            qry.Append($"\n   *                                                                                   ");
            qry.Append($"\n FROM t_customs                                                                        ");
            qry.Append($"\n WHERE 1=1                                                                             ");            

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductForNotSalesCost(string product, string origin, string sizes, string unit, DataTable productDt, string[] sub_product = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                                                                     ");
            qry.Append($"\n    t.*                                                                                                                                                                      ");
            qry.Append($"\n  , IF(t.payment_ex_rate IS NULL OR t.payment_ex_rate = '', f.exchange_rate, t.payment_ex_rate) AS exchange_rate                                                             ");
            qry.Append($"\n  FROM(                                                                                                                                                                      ");
            qry.Append($"\n 	 SELECT                                                                                                                                                                 ");
            qry.Append($"\n 	   t.id                                                                                                                                                                 ");
            qry.Append($"\n 	 , t.sub_id                                                                                                                                                             ");
            qry.Append($"\n 	 , t.ato_no                                                                                                                                                             ");
            qry.Append($"\n 	 , t.product                                                                                                                                                            ");
            qry.Append($"\n 	 , t.origin                                                                                                                                                             ");
            qry.Append($"\n 	 , t.sizes                                                                                                                                                              ");
            qry.Append($"\n 	 , t.unit                                                                                                                                                               ");
            qry.Append($"\n 	 , t.weight_calculate                                                                                                                                                   ");
            qry.Append($"\n 	 , t.cost_unit                                                                                                                                                          ");
            qry.Append($"\n 	 , t.custom                                                                                                                                                             ");
            qry.Append($"\n 	 , t.tax                                                                                                                                                                ");
            qry.Append($"\n 	 , t.incidental_expense                                                                                                                                                 ");
            qry.Append($"\n 	 , t.unit_price                                                                                                                                                         ");
            qry.Append($"\n 	 , t.qty                                                                                                                                                                ");
            qry.Append($"\n 	 , t.trq_amount                                                                                                                                                                ");
            qry.Append($"\n 	 , CASE WHEN t.ato_no LIKE 'DW%' OR t.ato_no LIKE 'HS%' THEN                                                                                                            ");
            qry.Append($"\n 				CASE WHEN t.etd <> '' THEN t.etd                                                                                                                            ");
            qry.Append($"\n 					WHEN t.etd = '' AND t.eta <> '' THEN date_add(t.eta, INTERVAL - IFNULL(c.delivery_days, 15) DAY)                                                        ");
            qry.Append($"\n 					WHEN t.shipment_date <> '' THEN t.shipment_date END                                                                                                     ");
            qry.Append($"\n 			ELSE                                                                                                                                                            ");
            qry.Append($"\n 				CASE                                                                                                                                                         ");
            qry.Append($"\n 				    " + ChangeEta(productDt) );
            qry.Append($"\n 				    WHEN t.eta <> '' THEN t.eta                                                                                                                              ");
            qry.Append($"\n 					WHEN t.eta = '' AND t.etd <> '' THEN date_add(t.etd, INTERVAL  IFNULL(c.delivery_days, 15) DAY)                                                         ");
            qry.Append($"\n 					WHEN t.shipment_date <> '' THEN date_add(t.shipment_date, INTERVAL  IFNULL(c.delivery_days, 15) DAY) END                                                ");
            qry.Append($"\n 			END AS ex_date                                                                                                                                                  ");
            qry.Append($"\n 	 , etd2                                                                                                                                                                 ");
            qry.Append($"\n 	 , IF(t.payment_ex_rate = 0 OR t.payment_ex_rate = '', NULL, t.payment_ex_rate) AS payment_ex_rate                                                                      ");
            qry.Append($"\n 	 FROM(                                                                                                                                                                  ");
            qry.Append($"\n 		 SELECT                                                                                                                                                             ");
            qry.Append($"\n 		   t1.id                                                                                                                                                            ");
            qry.Append($"\n 		 , t1.sub_id                                                                                                                                                        ");
            qry.Append($"\n 		 , t1.ato_no                                                                                                                                                        ");
            qry.Append($"\n 		 , t1.product                                                                                                                                                       ");
            qry.Append($"\n 		 , t1.origin                                                                                                                                                        ");
            qry.Append($"\n 		 , t1.sizes                                                                                                                                                         ");
            qry.Append($"\n 		 , t1.box_weight AS unit                                                                                                                                            ");
            qry.Append($"\n 		 , IF(t1.weight_calculate = 0, 'FALSE', 'TRUE') AS weight_calculate                                                                                                 ");
            qry.Append($"\n 		 , IF(IFNULL(t2.cost_unit, '') = '', 0, IFNULL(t2.cost_unit, '')) AS cost_unit                                                                                      ");
            qry.Append($"\n 		 , IFNULL(t2.custom             , 0) AS custom                                                                                                                      ");
            qry.Append($"\n 		 , IFNULL(t2.tax                , 0) AS tax                                                                                                                         ");
            qry.Append($"\n 		 , IFNULL(t2.incidental_expense , 0) AS incidental_expense                                                                                                          ");
            qry.Append($"\n 		 , t1.unit_price                                                                                                                                                    ");
            qry.Append($"\n 		 , IFNULL(t1.quantity_on_paper , 0) AS qty                                                                                                                          ");
            qry.Append($"\n 		 , IFNULL(shipment_date, '') AS shipment_date                                                                                                                       ");
            qry.Append($"\n 		 , IFNULL(etd, '') AS etd                                                                                                                                           ");
            qry.Append($"\n 		 , IFNULL(eta, '') AS eta                                                                                                                                           ");
            qry.Append($"\n 		 , CASE WHEN IFNULL(etd, '') <> '' THEN  etd                                                                                                                        ");
            qry.Append($"\n 		        WHEN IFNULL(eta, '') <> '' THEN  DATE_ADD(eta, INTERVAL -delivery_days DAY)                                                                                 ");
            qry.Append($"\n 		        ELSE IFNULL(shipment_date, '') END AS etd2                                                                                                                  ");
            qry.Append($"\n 		 , t1.payment_ex_rate AS payment_ex_rate                                                                                                                            ");
            qry.Append($"\n 		 , IFNULL(trq_amount, 0) AS trq_amount                                                                                                                              ");
            qry.Append($"\n 		 FROM (                                                                                                                                                             ");
            qry.Append($"\n			    SELECT                                                                                                                                                             ");
            qry.Append($"\n			      c1.*                                                                                                                                                             ");
            qry.Append($"\n			    , IFNULL(c2.delivery_days, 15) AS delivery_days                                                                                                                    ");
            qry.Append($"\n			    FROM (                                                                                                                                                             ");
            qry.Append($"\n			        SELECT                                                                                                                                                             ");
            qry.Append($"\n			        c.*                                                                                                                                                                ");
            qry.Append($"\n			        , s.payment_ex_rate                                                                                                                                                ");
            qry.Append($"\n			        FROM t_customs AS c                                                                                                                                                ");
            qry.Append($"\n			        LEFT OUTER JOIN t_shipping AS s                                                                                                                                    ");
            qry.Append($"\n                   ON c.id = s.custom_id                                                                                                                                         ");
            qry.Append($"\n                 WHERE 1=1                                                                                                                                                     ");
            qry.Append($"\n 		          AND IFNULL(c.is_shipment_qty, 1) = 1                                                                                                                                ");
            qry.Append($"\n 		          AND c.contract_year >= 2022                                                                                                                                ");
            if (sub_product == null || sub_product.Length == 0)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n                 {commonRepository.whereSql("c.product", product)}             ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n              {commonRepository.whereSql("c.origin", origin)}             ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n              {commonRepository.whereSql("c.sizes", sizes)}             ");
            }
            else
            {
                if (sub_product.Length > 0)
                {
                    string whr = "";
                    for (int i = 0; i < sub_product.Length; i++)
                    {
                        if (sub_product[i].Contains('^'))
                        {
                            if (i == 0)
                                whr += $"\n                 (         ";
                            else
                                whr += $"\n                 OR (         ";

                            string[] products = sub_product[i].Trim().Split('^');
                            if (products.Length > 5)
                            {
                                whr += $"\n                 c.product = '{products[0]}'            ";
                                whr += $"\n                 AND c.origin = '{products[1]}'         ";
                                whr += $"\n                 AND c.sizes = '{products[2]}'          ";
                                whr += $"\n                 AND c.box_weight = '{products[6]}'     ";
                                whr += $"\n                  )                                     ";
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(whr))
                    {
                        whr = "                 AND (         " + whr + $"\n                 )         ";
                        qry.Append($"{whr}                                                                                                                                                        ");
                    }
                }
            }
            qry.Append($"\n             ) AS c1                                                                                                                                                         ");
            qry.Append($"\n             LEFT OUTER JOIN t_country AS c2                                                                                                                                 ");
            qry.Append($"\n                 ON c1.origin = c2.country_name                                                                                                                              ");
            qry.Append($"\n         ) AS t1                                                                                                                                                             ");
            qry.Append($"\n 		 LEFT OUTER JOIN t_product_other_cost AS t2                                                                                                                         ");
            qry.Append($"\n 		   ON t1.product = t2.product                                                                                                                                       ");
            qry.Append($"\n 		   AND t1.origin = t2.origin                                                                                                                                        ");
            qry.Append($"\n 		   AND t1.sizes = t2.sizes                                                                                                                                          ");
            qry.Append($"\n 		   AND t1.box_weight = t2.unit                                                                                                                                      ");
            qry.Append($"\n 		 WHERE 1=1                                                                                                                                                          ");
            qry.Append($"\n 		 GROUP BY  t1.ato_no, t1.product, t1.origin, t1.sizes, t1.box_weight, IFNULL(t2.custom, 0), IFNULL(t2.tax, 0), IFNULL(t2.incidental_expense , 0), t1.unit_price     ");
            qry.Append($"\n 	 ) AS t                                                                                                                                                                 ");
            qry.Append($"\n 	 LEFT OUTER JOIN t_country AS c                                                                                                                                         ");
            qry.Append($"\n 	   ON t.origin = c.country_name                                                                                                                                         ");
            qry.Append($"\n  ) AS t                                                                                                                                                                     ");
            qry.Append($"\n  LEFT OUTER JOIN t_finance AS f                                                                                                                                             ");
            qry.Append($"\n    ON t.ex_date = f.base_date                                                                                                                                               ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
              
            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }


        private string ChangeEta(DataTable dt)
        {
            string txt = "";

            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                if (dt.Rows[i]["AtoNo"].ToString().Length >= 2 && !(dt.Rows[i]["AtoNo"].ToString().Substring(0, 2) == "HS"
                    || dt.Rows[i]["AtoNo"].ToString().Substring(0, 2) == "hs"
                    || dt.Rows[i]["AtoNo"].ToString().Substring(0, 2) == "DW"
                    || dt.Rows[i]["AtoNo"].ToString().Substring(0, 2) == "dw"))
                {
                    txt += "\n WHEN ato_no = '" + dt.Rows[i]["AtoNo"].ToString() + "' THEN '" + Convert.ToDateTime(dt.Rows[i]["입고일자"].ToString()).ToString("yyyy-MM-dd") + "'";
                }
            }
            return txt;
        }

    }
}

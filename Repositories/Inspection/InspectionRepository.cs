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
    public class InspectionRepository : ClassRoot, IinspectionRepository
    {

        public DataTable GetInspectinoList(string etdSttdate, string etdEnddate, string etaSttdate, string etaEnddate, string inSttdate, string inEnddate
                                        , string ato_no, string bl_no, string warehouse, string cc_status, string income_manger
                                        , string product, string origin, string sizes, string unit, string inspection_manager, string inspection_status
                                        , int id = 0, int sub_id = 0)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                                                                      ");
            qry.Append($"\n  i.id                                                                                                                                      ");
            qry.Append($"\n, i.sub_id                                                                                                                                  ");
            qry.Append($"\n, i.inspection_cnt                                                                                                                          ");
            qry.Append($"\n, i.ato_no                                                                                                                                  ");
            qry.Append($"\n, i.bl_no                                                                                                                                   ");
            qry.Append($"\n, i.contract_no                                                                                                                             ");
            qry.Append($"\n, i.contract_year                                                                                                                             ");
            qry.Append($"\n, i.etd                                                                                                                                     ");
            qry.Append($"\n, i.eta                                                                                                                                     ");
            qry.Append($"\n, CASE WHEN IFNULL(i.warehousing_date, '') = '' THEN date_add(i.eta, INTERVAL 5 DAY) ELSE i.warehousing_date END AS warehousing_date        ");
            qry.Append($"\n, i.cc_status                                                                                                                               ");
            qry.Append($"\n, i.product                                                                                                                                 ");
            qry.Append($"\n, i.origin                                                                                                                                  ");
            qry.Append($"\n, i.sizes                                                                                                                                   ");
            qry.Append($"\n, i.box_weight                                                                                                                              ");
            qry.Append($"\n, i.qty                                                                                                                                     ");
            qry.Append($"\n, i.warehouse                                                                                                                               ");
            qry.Append($"\n, i.remark                                                                                                                                  ");
            qry.Append($"\n, i.income_manager                                                                                                                          ");
            qry.Append($"\n, i.inspection_status                                                                                                                       ");
            qry.Append($"\n, i.inspection_date                                                                                                                         ");
            qry.Append($"\n, i.inspection_remark                                                                                                        ");
            qry.Append($"\n, i.inspection_manager                                                                                                                      ");
            qry.Append($"\nFROM (                                                                                                                                      ");
            qry.Append($"\n	SELECT                                                                                                                                     ");
            qry.Append($"\n	  i.id                                                                                                                                     ");
            qry.Append($"\n	, i.sub_id                                                                                                                                 ");
            qry.Append($"\n	, i.inspection_cnt                                                                                                                         ");
            qry.Append($"\n	, i.ato_no                                                                                                                                 ");
            qry.Append($"\n	, i.bl_no                                                                                                                                  ");
            qry.Append($"\n	, i.contract_no                                                                                                                        ");
            qry.Append($"\n	, i.contract_year                                                                                                                        ");
            qry.Append($"\n	, CASE WHEN IFNULL(i.etd, '') = '' THEN i.shipment_date ELSE i.etd END AS etd                                                              ");
            qry.Append($"\n	, CASE WHEN IFNULL(i.eta, '') = '' AND IFNULL(i.etd, '') <> '' THEN date_add(i.etd, INTERVAL IFNULL(c.delivery_days, 15) DAY)              ");
            qry.Append($"\n			WHEN IFNULL(i.eta, '') = '' AND IFNULL(i.etd, '') = '' THEN date_add(i.shipment_date, INTERVAL IFNULL(c.delivery_days, 15) DAY)    ");
            qry.Append($"\n			ELSE i.eta END AS eta                                                                                                              ");
            qry.Append($"\n	, i.warehousing_date                                                                                                                       ");
            qry.Append($"\n	, i.cc_status                                                                                                                              ");
            qry.Append($"\n	, i.product                                                                                                                                ");
            qry.Append($"\n	, i.origin                                                                                                                                 ");
            qry.Append($"\n	, i.sizes                                                                                                                                  ");
            qry.Append($"\n	, i.box_weight                                                                                                                             ");
            qry.Append($"\n	, i.qty                                                                                                                                    ");
            qry.Append($"\n	, i.warehouse                                                                                                                              ");
            qry.Append($"\n , i.remark                                                                                                                                 ");
            qry.Append($"\n	, i.income_manager                                                                                                                         ");
            qry.Append($"\n	, i.inspection_status                                                                                                                      ");
            qry.Append($"\n	, i.inspection_date                                                                                                                        ");
            qry.Append($"\n	, i.inspection_remark                                                                                                        ");
            qry.Append($"\n	, i.inspection_manager                                                                                                                     ");
            qry.Append($"\n	FROM (                                                                                                                                     ");
            qry.Append($"\n		SELECT                                                                                                                                 ");
            qry.Append($"\n		  c.id                                                                                                                                 ");
            qry.Append($"\n		, c.sub_id                                                                                                                             ");
            qry.Append($"\n		, i.inspection_cnt                                                                                                                             ");
            qry.Append($"\n		, c.ato_no                                                                                                                             ");
            qry.Append($"\n		, c.bl_no                                                                                                                              ");
            qry.Append($"\n		, c.contract_no                                                                                                                        ");
            qry.Append($"\n		, c.contract_year                                                                                                                        ");
            qry.Append($"\n		, c.shipment_date                                                                                                                      ");
            qry.Append($"\n		, c.etd                                                                                                                                ");
            qry.Append($"\n		, c.eta                                                                                                                                ");
            qry.Append($"\n		, c.warehousing_date                                                                                                                   ");
            qry.Append($"\n		, c.cc_status                                                                                                                          ");
            qry.Append($"\n		, c.product                                                                                                                            ");
            qry.Append($"\n		, c.origin                                                                                                                             ");
            qry.Append($"\n		, c.sizes                                                                                                                              ");
            qry.Append($"\n		, c.box_weight                                                                                                                         ");
            qry.Append($"\n		, c.quantity_on_paper AS qty                                                                                                           ");
            qry.Append($"\n		, c.warehouse                                                                                                                          ");
            qry.Append($"\n		, c.remark                                                                                                                             ");
            qry.Append($"\n		, c.manager AS income_manager                                                                                                          ");
            qry.Append($"\n		, i.status AS inspection_status                                                                                                        ");
            qry.Append($"\n		, i.inspection_date                                                                                                                    ");
            qry.Append($"\n		, i.remark AS inspection_remark                                                                                                        ");
            qry.Append($"\n		, i.manager AS inspection_manager                                                                                                      ");
            qry.Append($"\n		FROM t_customs AS c                                                                                                                    ");
            qry.Append($"\n		LEFT OUTER JOIN (                                                                                                                      ");
            qry.Append($"\n			SELECT                                                                                                                             ");
            qry.Append($"\n			  *                                                                                                                                ");
            qry.Append($"\n			FROM t_inspection AS a                                                                                                             ");
            qry.Append($"\n			WHERE inspection_cnt = (SELECT MAX(inspection_cnt) FROM t_inspection AS b WHERE a.id = b.id AND a.sub_id = b.sub_id)               ");
            qry.Append($"\n		) AS i                                                                                                                                 ");
            qry.Append($"\n		  ON c.id = i.id                                                                                                                       ");
            qry.Append($"\n		  AND c.sub_id = i.sub_id                                                                                                              ");
            
            if(id > 0 && sub_id > 0)
                qry.Append($"\n		  WHERE c.id = {id} AND c.sub_id = {sub_id}                                                                                ");
            else
                qry.Append($"\n		WHERE c.contract_year >= 2022 AND c.cc_status <> '확정'                                                                               ");

            if (cc_status == "미통관")
                qry.Append($"\n		  AND c.cc_status <> '통관'                                                                                ");
            else if (cc_status == "미통관")
                qry.Append($"\n		  AND c.cc_status = '통관'                                                                                ");

            if (!string.IsNullOrEmpty(ato_no))
                qry.Append($"\n		  AND c.ato_no LIKE '%{ato_no}%'                                                                                ");
            if (!string.IsNullOrEmpty(bl_no))
                qry.Append($"\n		  AND c.bl_no LIKE '%{bl_no}%'                                                                                ");
            if (!string.IsNullOrEmpty(warehouse))
                qry.Append($"\n		  AND c.warehouse LIKE '%{warehouse}%'                                                                                ");
            if (!string.IsNullOrEmpty(income_manger))
                qry.Append($"\n		  AND c.manager LIKE '%{income_manger}%'                                                                                ");

            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n		  AND c.product LIKE '%{product}%'                                                                                ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n		  AND c.origin LIKE '%{origin}%'                                                                                ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n		  AND c.sizes LIKE '%{sizes}%'                                                                                ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n		  AND c.box_weight LIKE '%{unit}%'                                                                                ");

            if (inspection_status == "완료")
                qry.Append($"\n		  AND i.status = '완료'                                                                                 ");
            else if (inspection_status == "대기")
                qry.Append($"\n		  AND i.status <> '완료'                                                                                ");

            if (!string.IsNullOrEmpty(inspection_manager))
                qry.Append($"\n		  AND i.manager LIKE '%{inspection_manager}%'                                                                                ");


            qry.Append($"\n	) AS i                                                                                                                                     ");
            qry.Append($"\n	LEFT OUTER JOIN t_country AS c                                                                                                             ");
            qry.Append($"\n	  ON i.origin = c.country_name                                                                                                             ");
            qry.Append($"\n) AS i                                                                                                                                      ");
            qry.Append($"\nWHERE 1=1         ");

            if (!string.IsNullOrEmpty(etdSttdate))
                qry.Append($"\n  AND i.etd >= '{etdSttdate}'        ");
            if (!string.IsNullOrEmpty(etdEnddate))
                qry.Append($"\n  AND i.etd <= '{etdEnddate}'        ");

            if (!string.IsNullOrEmpty(etaSttdate))
                qry.Append($"\n  AND i.eta >= '{etaSttdate}'        ");
            if (!string.IsNullOrEmpty(etaEnddate))
                qry.Append($"\n  AND i.eta <= '{etaEnddate}'        ");

            if (!string.IsNullOrEmpty(inSttdate))
                qry.Append($"\n  AND CASE WHEN IFNULL(i.warehousing_date, '') = '' THEN date_add(i.eta, INTERVAL 5 DAY) ELSE i.warehousing_date END >= '{inSttdate}'        ");
            if (!string.IsNullOrEmpty(inEnddate))
                qry.Append($"\n  AND CASE WHEN IFNULL(i.warehousing_date, '') = '' THEN date_add(i.eta, INTERVAL 5 DAY) ELSE i.warehousing_date END <= '{inEnddate}'        ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public List<InspectionModel> GetInspection(DateTime sttdate, DateTime enddate, string warehouse, string origin, string product, string sizes, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                   ");
            qry.Append($"\n   c.id                                   ");
            qry.Append($"\n , c.sub_id                               ");
            qry.Append($"\n , c.warehousing_date                     ");
            qry.Append($"\n , 100 AS warehousing_date_score          ");
            qry.Append($"\n , c.warehouse                            ");
            qry.Append($"\n , c.origin                               ");
            qry.Append($"\n , c.product                              ");
            qry.Append($"\n , c.sizes                                ");
            qry.Append($"\n , c.box_weight                           ");
            qry.Append($"\n , c.quantity_on_paper                    ");
            qry.Append($"\n , s.inspection_date                      ");
            qry.Append($"\n , s.inspection_results                   ");
            qry.Append($"\n , s.inspection_manager                   ");
            qry.Append($"\n , s.edit_user                            ");
            qry.Append($"\n , s.edit_date                            ");
            qry.Append($"\n FROM t_customs  AS c                     ");
            qry.Append($"\n LEFT OUTER JOIN t_inspection AS s        ");
            qry.Append($"\n   ON c.id = s.id                         ");
            qry.Append($"\n   AND c.sub_id = s.sub_id                ");
            qry.Append($"\n WHERE c.warehousing_date >= '{sttdate}' ");
            qry.Append($"\n   AND c.warehousing_date <= '{enddate}' ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                qry.Append($"\n   AND c.warehouse LIKE '%{warehouse}%' ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n   AND c.origin LIKE '%{origin}%' ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n   AND c.product LIKE '%{product}%' ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n   AND c.sizes LIKE '%{sizes}%' ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n   AND s.inspection_manager LIKE '%{manager}%' ");
            }
            qry.Append($"\n ORDER BY c.warehousing_date, c.product, c.sizes");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return InspectionInfo(dr);
        }

        private List<InspectionModel> InspectionInfo(MySqlDataReader rd)
        {
            List<InspectionModel> list = new List<InspectionModel>();
            while (rd.Read())
            {
                InspectionModel model = new InspectionModel();

                model.id = Convert.ToInt32(rd["id"]);
                model.sub_id = Convert.ToInt32(rd["sub_id"]);
                model.warehousing_date = rd["warehousing_date"].ToString();
                model.warehousing_date_score = Convert.ToInt32(rd["warehousing_date_score"].ToString());
                model.warehouse = rd["warehouse"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();
                model.quantity_on_paper = Convert.ToDouble(rd["quantity_on_paper"]);
                model.inspection_date = rd["inspection_date"].ToString();
                model.inspection_results = rd["inspection_results"].ToString();
                model.inspection_manager = rd["inspection_manager"].ToString();
                model.edit_user = rd["edit_user"].ToString();
                model.edit_date = rd["edit_date"].ToString();
                list.Add(model);

            }
            rd.Close();
            return list;
        }


        public StringBuilder DeleteInspection(InspectionInfoModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_inspection         ");
            qry.Append($" WHERE id = {model.id} AND sub_id = {model.sub_id}                        ");

            return qry;
        }

        public StringBuilder InsertInspection(InspectionInfoModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT t_inspection (        ");
            qry.Append($"   id                        ");
            qry.Append($" , sub_id                    ");
            qry.Append($" , inspection_cnt            ");
            qry.Append($" , inspection_date           ");
            qry.Append($" , status                    ");
            qry.Append($" , remark                    ");
            qry.Append($" , manager                   ");
            qry.Append($" , edit_user                 ");
            qry.Append($" , updatetime                ");
            qry.Append($") VALUES ( ");
            qry.Append($"  {model.id}                           ");
            qry.Append($", {model.sub_id}                       ");
            qry.Append($", {model.inspection_cnt}               ");
            qry.Append($", '{model.inspection_date}'            ");
            qry.Append($", '{model.status}'                     ");
            qry.Append($", '{model.remark}'                     ");
            qry.Append($", '{model.manager}'                    ");
            qry.Append($", '{model.edit_user}'                  ");
            qry.Append($", '{model.updatetime}'                 ");
            qry.Append($") ");

            return qry;
        }

        public int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
        {
            if (sqlList.Count > 0)
            {

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

        public List<InspectionModel> GetInspectionSchedule(DateTime sttdate, DateTime enddate, string origin, string product, string sizes, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                            ");
            qry.Append($"\n   c.id                                                                                                                            ");
            qry.Append($"\n , c.sub_id                                                                                                                        ");
            qry.Append($"\n , c.warehousing_date                                                                                                              ");
            qry.Append($"\n , c.warehousing_date_score                                                                                                        ");
            qry.Append($"\n , c.warehouse                                                                                                                     ");
            qry.Append($"\n , c.origin                                                                                                                        ");
            qry.Append($"\n , c.product                                                                                                                       ");
            qry.Append($"\n , c.sizes                                                                                                                         ");
            qry.Append($"\n , c.box_weight                                                                                                                    ");
            qry.Append($"\n , c.quantity_on_paper                                                                                                             ");
            qry.Append($"\n , s.inspection_date                                                                                                               ");
            qry.Append($"\n , s.remark AS inspection_results                                                                                                  ");
            qry.Append($"\n , s.manager AS inspection_manager                                                                                                 ");
            qry.Append($"\n , s.edit_user                                                                                                                     ");
            qry.Append($"\n , s.updatetime AS edit_date                                                                                                       ");
            qry.Append($"\n FROM (                                                                                                                            ");
            qry.Append($"\n	SELECT                                                                                                                            ");
            qry.Append($"\n	  i.id                                                                                                                            ");
            qry.Append($"\n	, i.sub_id                                                                                                                        ");
            qry.Append($"\n	, i.inspection_cnt                                                                                                                ");
            qry.Append($"\n	, i.ato_no                                                                                                                        ");
            qry.Append($"\n	, i.bl_no                                                                                                                         ");
            qry.Append($"\n	, i.contract_no                                                                                                                   ");
            qry.Append($"\n	, i.contract_year                                                                                                                 ");
            qry.Append($"\n	, CASE WHEN IFNULL(i.etd, '') = '' THEN i.shipment_date ELSE i.etd END AS etd                                                                                        ");
            qry.Append($"\n	, CASE WHEN IFNULL(i.eta, '') = '' AND IFNULL(i.etd, '') <> '' THEN date_add(i.etd, INTERVAL IFNULL(c.delivery_days, 15) DAY)                                        ");
            qry.Append($"\n			WHEN IFNULL(i.eta, '') = '' AND IFNULL(i.etd, '') = '' THEN date_add(i.shipment_date, INTERVAL IFNULL(c.delivery_days, 15) DAY)                              ");
            qry.Append($"\n			ELSE i.eta END AS eta                                                                                                                                        ");
            qry.Append($"\n	, CASE WHEN IFNULL(i.warehousing_date, '') <> '' THEN i.warehousing_date                                                                                             ");
            qry.Append($"\n			WHEN IFNULL(i.eta, '') <> '' THEN date_add(i.eta, INTERVAL 5 DAY)                                                                                            ");
            qry.Append($"\n            WHEN IFNULL(i.eta, '') = '' AND IFNULL(i.etd, '') <> '' THEN date_add(date_add(i.etd, INTERVAL IFNULL(c.delivery_days, 15) DAY), INTERVAL 5 DAY)          ");
            qry.Append($"\n            ELSE date_add(date_add(i.shipment_date, INTERVAL IFNULL(c.delivery_days, 15) DAY), INTERVAL 5 DAY) END AS warehousing_date                                ");
            qry.Append($"\n    , CASE WHEN IFNULL(i.warehousing_date, '') <> '' THEN 100                                                                                                         ");
            qry.Append($"\n			WHEN IFNULL(i.eta, '') <> '' THEN 90                                                                                                                         ");
            qry.Append($"\n            WHEN IFNULL(i.eta, '') = '' AND IFNULL(i.etd, '') <> '' THEN 80                                                                                           ");
            qry.Append($"\n            ELSE 70 END AS warehousing_date_score                                                                                                                     ");
            qry.Append($"\n	, i.cc_status                                                                                                                     ");
            qry.Append($"\n	, i.product                                                                                                                       ");
            qry.Append($"\n	, i.origin                                                                                                                        ");
            qry.Append($"\n	, i.sizes                                                                                                                         ");
            qry.Append($"\n	, i.box_weight                                                                                                                    ");
            qry.Append($"\n	, i.quantity_on_paper                                                                                                             ");
            qry.Append($"\n	, i.warehouse                                                                                                                     ");
            qry.Append($"\n	, i.remark                                                                                                                        ");
            qry.Append($"\n	, i.income_manager                                                                                                                ");
            qry.Append($"\n	, i.inspection_status                                                                                                             ");
            qry.Append($"\n	, i.inspection_date                                                                                                               ");
            qry.Append($"\n	, i.inspection_remark                                                                                                             ");
            qry.Append($"\n	, i.inspection_manager                                                                                                            ");
            qry.Append($"\n	FROM (                                                                                                                            ");
            qry.Append($"\n		SELECT                                                                                                                        ");
            qry.Append($"\n		  c.id                                                                                                                        ");
            qry.Append($"\n		, c.sub_id                                                                                                                    ");
            qry.Append($"\n		, i.inspection_cnt                                                                                                            ");
            qry.Append($"\n		, c.ato_no                                                                                                                    ");
            qry.Append($"\n		, c.bl_no                                                                                                                     ");
            qry.Append($"\n		, c.contract_no                                                                                                               ");
            qry.Append($"\n		, c.contract_year                                                                                                             ");
            qry.Append($"\n		, c.shipment_date                                                                                                             ");
            qry.Append($"\n		, c.etd                                                                                                                       ");
            qry.Append($"\n		, c.eta                                                                                                                       ");
            qry.Append($"\n		, c.warehousing_date                                                                                                          ");
            qry.Append($"\n		, c.cc_status                                                                                                                 ");
            qry.Append($"\n		, c.product                                                                                                                   ");
            qry.Append($"\n		, c.origin                                                                                                                    ");
            qry.Append($"\n		, c.sizes                                                                                                                     ");
            qry.Append($"\n		, c.box_weight                                                                                                                ");
            qry.Append($"\n		, c.quantity_on_paper                                                                                                         ");
            qry.Append($"\n		, c.warehouse                                                                                                                 ");
            qry.Append($"\n		, c.remark                                                                                                                    ");
            qry.Append($"\n		, c.manager AS income_manager                                                                                                 ");
            qry.Append($"\n		, i.status AS inspection_status                                                                                               ");
            qry.Append($"\n		, i.inspection_date                                                                                                           ");
            qry.Append($"\n		, i.remark AS inspection_remark                                                                                               ");
            qry.Append($"\n		, i.manager AS inspection_manager                                                                                             ");
            qry.Append($"\n		FROM t_customs AS c                                                                                                           ");
            qry.Append($"\n		LEFT OUTER JOIN (                                                                                                             ");
            qry.Append($"\n			SELECT                                                                                                                    ");
            qry.Append($"\n			  *                                                                                                                       ");
            qry.Append($"\n			FROM t_inspection AS a                                                                                                    ");
            qry.Append($"\n			WHERE inspection_cnt = (SELECT MAX(inspection_cnt) FROM t_inspection AS b WHERE a.id = b.id AND a.sub_id = b.sub_id)      ");
            qry.Append($"\n		) AS i                                                                                                                        ");
            qry.Append($"\n		  ON c.id = i.id                                                                                                              ");
            qry.Append($"\n		  AND c.sub_id = i.sub_id                                                                                                     ");
            qry.Append($"\n		WHERE c.contract_year >= 2022 AND c.cc_status <> '확정'                                                                         ");
            qry.Append($"\n	      AND IFNULL(c.is_shipment_qty, 1) = 1                                                                                                                     ");
            qry.Append($"\n	) AS i                                                                                                                            ");
            qry.Append($"\n	LEFT OUTER JOIN t_country AS c                                                                                                    ");
            qry.Append($"\n	  ON i.origin = c.country_name                                                                                                    ");
            qry.Append($"\n )  AS c                                                                                                                           ");
            qry.Append($"\n LEFT OUTER JOIN t_inspection AS s                                                                                                 ");
            qry.Append($"\n   ON c.id = s.id                                                                                                                  ");
            qry.Append($"\n   AND c.sub_id = s.sub_id                                                                                                         ");
            qry.Append($"\n WHERE c.warehousing_date >= '{sttdate.ToString("yyyy-MM-dd")}' ");
            qry.Append($"\n   AND c.warehousing_date <= '{enddate.ToString("yyyy-MM-dd")}' ");
            
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n   AND c.product LIKE '%{product}%' ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n   AND c.origin LIKE '%{origin}%' ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n   AND c.sizes LIKE '%{sizes}%' ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n   AND c.manager LIKE '%{manager}%' ");
            }

            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return InspectionInfo(dr);
        }
    }
}

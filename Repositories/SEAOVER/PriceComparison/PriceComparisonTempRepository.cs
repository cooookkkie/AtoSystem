using Libs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Runtime.Remoting.Lifetime;
using AdoNetWindow.Model;
using Libs.Tools;
using static System.Windows.Forms.AxHost;

namespace Repositories.SEAOVER.PriceComparison
{
    public class PriceComparisonTempRepository : ClassRoot, IPriceComparisonTempRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public DataTable GetPrieComparisonTempData(DateTime standardDate, int isOutStockLimintDays = 0, string product = "", string origin = "", string sizes = "", string unit = "", string division = "", string manager = "", string contents = "", string remark = "")
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            DateTime eDate = DateTime.Now;
            //영업일
            int work_days_1 = 0;
            common.GetWorkDay(eDate.AddMonths(-1), eDate, out work_days_1);
            int work_days_45 = 0;
            common.GetWorkDay(eDate.AddDays(-45), eDate, out work_days_45);
            int work_days_2 = 0;
            common.GetWorkDay(eDate.AddMonths(-2), eDate, out work_days_2);
            int work_days_3 = 0;
            common.GetWorkDay(eDate.AddMonths(-3), eDate, out work_days_3);
            int work_days_6 = 0;
            common.GetWorkDay(eDate.AddMonths(-6), eDate, out work_days_6);
            int work_days_12 = 0;
            common.GetWorkDay(eDate.AddMonths(-12), eDate, out work_days_12);
            int work_days_18 = 0;
            common.GetWorkDay(eDate.AddMonths(-18), eDate, out work_days_18);
            //오늘하루 제외
            if (eDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                work_days_1--;
                work_days_45--;
                work_days_2--;
                work_days_3--;
                work_days_6--;
                work_days_12--;
                work_days_18--;
            }


            StringBuilder qry = new StringBuilder();
            qry.Append($"\n select                                                                              ");
            qry.Append($"\n   '' AS is_new                                                                      ");
            qry.Append($"\n , t1.product                                                                        ");
            qry.Append($"\n , t1.origin                                                                         ");
            qry.Append($"\n , t1.sizes                                                                          ");
            qry.Append($"\n , t1.unit                                                                           ");
            qry.Append($"\n , t1.price_unit                                                                     ");
            qry.Append($"\n , t1.unit_count                                                                     ");
            qry.Append($"\n , t1.seaover_unit                                                                   ");
            qry.Append($"\n , CONCAT(t1.product, '_', t1.origin, '_', t1.sizes, '_', t1.unit, '_', t1.price_unit, '_', t1.unit_count, '_', t1.seaover_unit) AS product_code                      ");
            qry.Append($"\n , CONCAT(t1.product, '_', t1.origin, '_', t1.sizes, '_', t1.seaover_unit) AS product_code2                      ");
            qry.Append($"\n , t1.shipment_qty + t1.unpending_qty_before AS stock1                               ");
            qry.Append($"\n , t1.shipment_qty                                                                   ");
            qry.Append($"\n , t1.unpending_qty_before                                                           ");
            qry.Append($"\n , t1.seaover_unpending + t1.seaover_pending - t1.reserved_stock AS stock2           ");
            qry.Append($"\n , t1.seaover_unpending                                                              ");
            qry.Append($"\n , t1.seaover_pending                                                                ");
            qry.Append($"\n , t1.reserved_stock                                                                 ");
            qry.Append($"\n , t1.shipment_qty + t1.unpending_qty_before + t1.seaover_unpending + t1.seaover_pending - t1.reserved_stock AS stock3           ");
            qry.Append($"\n , LEAST(t1.sales_count1, t1.sales_count45, t1.sales_count2, t1.sales_count3, t1.sales_count6, t1.sales_count12, t1.sales_count18) AS min_sales_count                                                                  ");
            qry.Append($"\n , t1.sales_count1                                                                   ");
            qry.Append($"\n , t1.sales_count45                                                                  ");
            qry.Append($"\n , t1.sales_count2                                                                   ");
            qry.Append($"\n , t1.sales_count3                                                                   ");
            qry.Append($"\n , t1.sales_count6                                                                   ");
            qry.Append($"\n , t1.sales_count12                                                                  ");
            qry.Append($"\n , t1.sales_count18                                                                  ");
            qry.Append($"\n , t1.sales_count1  / {work_days_1 } AS sales_count_day1                                                                   ");
            qry.Append($"\n , t1.sales_count45 / {work_days_45}  AS sales_count_day45                                                                  ");
            qry.Append($"\n , t1.sales_count2  / {work_days_2 } AS sales_count_day2                                                                   ");
            qry.Append($"\n , t1.sales_count3  / {work_days_3 } AS sales_count_day3                                                                   ");
            qry.Append($"\n , t1.sales_count6  / {work_days_6 } AS sales_count_day6                                                                   ");
            qry.Append($"\n , t1.sales_count12 / {work_days_12}  AS sales_count_day12                                                                  ");
            qry.Append($"\n , t1.sales_count18 / {work_days_18}  AS sales_count_day18                                                                  ");
            qry.Append($"\n , t1.sales_count1  / {work_days_1 }  * 21 AS sales_count_month1                                                                   ");
            qry.Append($"\n , t1.sales_count45 / {work_days_45}  * 21 AS sales_count_month45                                                                  ");
            qry.Append($"\n , t1.sales_count2  / {work_days_2 }  * 21 AS sales_count_month2                                                                   ");
            qry.Append($"\n , t1.sales_count3  / {work_days_3 }  * 21 AS sales_count_month3                                                                   ");
            qry.Append($"\n , t1.sales_count6  / {work_days_6 }  * 21 AS sales_count_month6                                                                   ");
            qry.Append($"\n , t1.sales_count12 / {work_days_12}  * 21 AS sales_count_month12                                                                  ");
            qry.Append($"\n , t1.sales_count18 / {work_days_18}  * 21 AS sales_count_month18                                                                  ");
            qry.Append($"\n , LEAST(t1.enable_sales_days1, t1.enable_sales_days45, t1.enable_sales_days2, t1.enable_sales_days3, t1.enable_sales_days6 ,t1.enable_sales_days12,t1.enable_sales_days18) AS min_enable_sales_days                                                                  ");
            qry.Append($"\n , t1.enable_sales_days1                                                             ");
            qry.Append($"\n , t1.enable_sales_days45                                                            ");
            qry.Append($"\n , t1.enable_sales_days2                                                             ");
            qry.Append($"\n , t1.enable_sales_days3                                                             ");
            qry.Append($"\n , t1.enable_sales_days6                                                             ");
            qry.Append($"\n , t1.enable_sales_days12                                                            ");
            qry.Append($"\n , t1.enable_sales_days18                                                            ");
            qry.Append($"\n , LEAST(t1.exhausted_date1, t1.exhausted_date45, t1.exhausted_date2, t1.exhausted_date3, t1.exhausted_date6, t1.exhausted_date12, t1.exhausted_date18) AS min_exhausted_date1                                                                  ");
            qry.Append($"\n , t1.exhausted_date1                                                                ");
            qry.Append($"\n , t1.exhausted_date45                                                               ");
            qry.Append($"\n , t1.exhausted_date2                                                                ");
            qry.Append($"\n , t1.exhausted_date3                                                                ");
            qry.Append($"\n , t1.exhausted_date6                                                                ");
            qry.Append($"\n , t1.exhausted_date12                                                               ");
            qry.Append($"\n , t1.exhausted_date18                                                               ");

            qry.Append($"\n , CAST(DATEDIFF(t1.exhausted_date1, NOW())  AS CHAR) AS exhausted_date_until_days1                                                                       ");
            qry.Append($"\n , CAST(DATEDIFF(t1.exhausted_date45, NOW()) AS CHAR)  AS exhausted_date_until_days45                                                                     ");
            qry.Append($"\n , CAST(DATEDIFF(t1.exhausted_date2, NOW())  AS CHAR) AS exhausted_date_until_days2                                                                       ");
            qry.Append($"\n , CAST(DATEDIFF(t1.exhausted_date3, NOW())  AS CHAR) AS exhausted_date_until_days3                                                                       ");
            qry.Append($"\n , CAST(DATEDIFF(t1.exhausted_date6, NOW())  AS CHAR) AS exhausted_date_until_days6                                                                       ");
            qry.Append($"\n , CAST(DATEDIFF(t1.exhausted_date12, NOW()) AS CHAR)  AS exhausted_date_until_days12                                                                     ");
            qry.Append($"\n , CAST(DATEDIFF(t1.exhausted_date18, NOW()) AS CHAR)  AS exhausted_date_until_days18                                                                     ");

            qry.Append($"\n , date_add(t1.exhausted_date1, INTERVAL -(t1.delivery_days + 5) DAY) AS etd1                                              ");
            qry.Append($"\n , date_add(t1.exhausted_date45, INTERVAL -(t1.delivery_days + 5) DAY) AS etd45                                            ");
            qry.Append($"\n , date_add(t1.exhausted_date2, INTERVAL -(t1.delivery_days + 5) DAY) AS etd2                                              ");
            qry.Append($"\n , date_add(t1.exhausted_date3, INTERVAL -(t1.delivery_days + 5) DAY) AS etd3                                              ");
            qry.Append($"\n , date_add(t1.exhausted_date6, INTERVAL -(t1.delivery_days + 5) DAY) AS etd6                                              ");
            qry.Append($"\n , date_add(t1.exhausted_date12, INTERVAL -(t1.delivery_days + 5) DAY) AS etd12                                            ");
            qry.Append($"\n , date_add(t1.exhausted_date18, INTERVAL -(t1.delivery_days + 5) DAY) AS etd18                                            ");

            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date1, INTERVAL -(t1.delivery_days + 5) DAY) , NOW()) AS CHAR) AS etd_until_days1                                    ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date45, INTERVAL -(t1.delivery_days + 5) DAY), NOW()) AS CHAR) AS etd_until_days45                                   ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date2, INTERVAL -(t1.delivery_days + 5) DAY) , NOW()) AS CHAR) AS etd_until_days2                                    ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date3, INTERVAL -(t1.delivery_days + 5) DAY) , NOW()) AS CHAR) AS etd_until_days3                                    ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date6, INTERVAL -(t1.delivery_days + 5) DAY) , NOW()) AS CHAR) AS etd_until_days6                                    ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date12, INTERVAL -(t1.delivery_days + 5) DAY), NOW()) AS CHAR) AS etd_until_days12                                   ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date18, INTERVAL -(t1.delivery_days + 5) DAY), NOW()) AS CHAR) AS etd_until_days18                                   ");

            qry.Append($"\n , date_add(t1.exhausted_date1, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) AS contract_date1               ");
            qry.Append($"\n , date_add(t1.exhausted_date45, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) AS contract_date45             ");
            qry.Append($"\n , date_add(t1.exhausted_date2, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) AS contract_date2               ");
            qry.Append($"\n , date_add(t1.exhausted_date3, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) AS contract_date3               ");
            qry.Append($"\n , date_add(t1.exhausted_date6, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) AS contract_date6               ");
            qry.Append($"\n , date_add(t1.exhausted_date12, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) AS contract_date12             ");
            qry.Append($"\n , date_add(t1.exhausted_date18, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) AS contract_date18             ");

            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date1, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) AS contract_until_days1          ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date45, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY), NOW()) AS CHAR) AS contract_until_days45         ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date2, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) AS contract_until_days2          ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date3, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) AS contract_until_days3          ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date6, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) AS contract_until_days6          ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date12, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY), NOW()) AS CHAR) AS contract_until_days12         ");
            qry.Append($"\n , CAST(DATEDIFF(date_add(t1.exhausted_date18, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY), NOW()) AS CHAR) AS contract_until_days18         ");

            qry.Append($"\n , t1.delivery_days                                                                  ");
            qry.Append($"\n , t1.production_days                                                                ");
            qry.Append($"\n , t1.division                                                                       ");
            qry.Append($"\n , t1.manager1                                                                       ");
            qry.Append($"\n , t1.manager2                                                                       ");
            qry.Append($"\n , t1.manager3                                                                       ");
            qry.Append($"\n , t1.updatetime                                                                     ");
            qry.Append($"\n , t1.main_id                                                                        ");
            qry.Append($"\n , t1.sub_id                                                                         ");
            qry.Append($"\n , IF(IFNULL(t2.is_hide, 0) = 0, 'FALSE', 'TRUE') AS is_hide                         ");
            qry.Append($"\n , IF(IFNULL(t2.confirmation_date, '') = '', '1900-01-01', DATE_FORMAT(t2.confirmation_date, '%Y-%m-%d')) AS confirmation_date                         ");
            qry.Append($"\n , CASE WHEN IF(IFNULL(t2.confirmation_date, '') = '', '1900-01-01', DATE_FORMAT(t2.confirmation_date, '%Y-%m-%d')) = '1900-01-01' THEN '0'                                     ");
            qry.Append($"\n		WHEN DATEDIFF(IF(IFNULL(t2.confirmation_date, '') = '', '1900-01-01', DATE_FORMAT(t2.confirmation_date, '%Y-%m-%d')), DATE_FORMAT(NOW(), '%Y-%m-%d')) <= 5 THEN '1'        ");
            qry.Append($"\n        ELSE '2' END AS sheet_type                                                                                                                                              ");
            //qry.Append($"\n , IF(IF(IFNULL(t2.confirmation_date, '') = '', '1900-01-01', DATE_FORMAT(t2.confirmation_date, '%Y-%m-%d')) > DATE_FORMAT(NOW(), '%Y-%m-%d'), 'TRUE', 'FALSE') AS is_confirmation                        ");
            qry.Append($"\n , t2.contents                                                                       ");
            qry.Append($"\n , t2.remark                                                                         ");
            qry.Append($"\n FROM t_pricecomparison_temp AS t1                                                   ");
            qry.Append($"\n LEFT OUTER JOIN t_pricecomparison_temp_setting AS t2                                ");
            qry.Append($"\n   ON CONCAT(t1.product, '_', t1.origin, '_', t1.sizes, '_', t1.unit, '_', t1.price_unit, '_', t1.unit_count, '_', t1.seaover_unit) = t2.product_code                                                   ");
            qry.Append($"\n WHERE 1 = 1                                                                      ");
            qry.Append($"\n   AND DATE_FORMAT(t1.updatetime, '%Y-%m-%d') = '{standardDate.ToString("yyyy-MM-dd")}'       ");
            qry.Append($"\n   AND (t1.main_id = 0 OR (t1.main_id > 0 AND t1.sub_id = 9999))                            ");

            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("t1.product", product)}                                                                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("t1.origin", origin)}                                                                      ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("t1.sizes", sizes)}                                                                      ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   {commonRepository.whereSql("t1.unit", unit)}                                                                      ");
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n   {commonRepository.whereSql("t1.division", division)}                                                                      ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {commonRepository.whereSql("t1.manager3", manager)}                                                                      ");
            if (!string.IsNullOrEmpty(contents))
                qry.Append($"\n   {commonRepository.whereSql("t2.contents", contents)}                                                                      ");
            if (!string.IsNullOrEmpty(remark))
                qry.Append($"\n   {commonRepository.whereSql("t2.remark", remark)}                                                                      ");

            if (isOutStockLimintDays > 0)
            {
                qry.Append($"\n   AND ( CAST(DATEDIFF(date_add(t1.exhausted_date1, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) < {isOutStockLimintDays}      ");
                qry.Append($"\n        OR CAST(DATEDIFF(date_add(t1.exhausted_date45, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) < {isOutStockLimintDays}      ");
                qry.Append($"\n        OR CAST(DATEDIFF(date_add(t1.exhausted_date2, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) < {isOutStockLimintDays}      ");
                qry.Append($"\n        OR CAST(DATEDIFF(date_add(t1.exhausted_date3, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) < {isOutStockLimintDays}      ");
                qry.Append($"\n        OR CAST(DATEDIFF(date_add(t1.exhausted_date6, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) < {isOutStockLimintDays}      ");
                qry.Append($"\n        OR CAST(DATEDIFF(date_add(t1.exhausted_date12, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) < {isOutStockLimintDays}      ");
                qry.Append($"\n        OR CAST(DATEDIFF(date_add(t1.exhausted_date18, INTERVAL -(t1.production_days + t1.delivery_days + 5) DAY) , NOW()) AS CHAR) < {isOutStockLimintDays})      ");
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

        public StringBuilder DeleteSettingData(string product_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_pricecomparison_temp_setting WHERE product_code = '{product_code}'                                       ");

            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder InsertSettingData(PriceComparisonTempSettingModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_pricecomparison_temp_setting (      ");
            qry.Append($"\n   product_code                                    ");
            qry.Append($"\n , is_hide                                         ");
            qry.Append($"\n , confirmation_date                               ");
            qry.Append($"\n , contents                                        ");
            qry.Append($"\n , remark                                          ");
            qry.Append($"\n , updatetime                                      ");
            qry.Append($"\n , edit_user                                       ");
            qry.Append($"\n ) VALUES (                                        ");
            qry.Append($"\n   '{model.product_code     }'                     ");
            qry.Append($"\n ,  {model.is_hide          }                      ");
            qry.Append($"\n , '{model.confirmation_date}'                     ");
            qry.Append($"\n , '{model.contents         }'                     ");
            qry.Append($"\n , '{model.remark           }'                     ");
            qry.Append($"\n , '{model.updatetime       }'                     ");
            qry.Append($"\n , '{model.edit_user        }'                     ");
            qry.Append($"\n )                                                 ");

            string sql = qry.ToString();
            return qry;
        }
    }
}

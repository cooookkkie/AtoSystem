using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SalesPartner
{
    public class DailyBusinessRepository : ClassRoot, IDailyBusinessRepository
    {
        public DataTable GetTempList(string user_id, int document_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                    ");
            qry.Append($"\n  distinct document_id, id, DATE_FORMAT(updatetime,'%Y-%m-%d %H:%i') AS updatetime  ");
            qry.Append($"\n FROM t_daily_business_temp                                     ");
            qry.Append($"\n WHERE 1=1                                                 ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n   AND user_id = '{user_id}'         ");
            if (document_id > 0)
                qry.Append($"\n   AND document_id = {document_id}         ");
            qry.Append($"\n ORDER BY id DESC                                  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetDocumentList(string user_id, string document_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                    ");
            qry.Append($"\n  user_id, document_id , document_name, MAX(updatetime) AS updatetime, MAX(createtime) AS createtime  ");
            qry.Append($"\n FROM t_daily_business                                     ");
            qry.Append($"\n WHERE 1=1                                                 ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n   AND user_id = '{user_id}'         ");
            if (!string.IsNullOrEmpty(document_name))
                qry.Append($"\n   AND document_name LIKE '%{document_name}%'         ");
            qry.Append($"\n GROUP BY user_id, document_id                             ");
            qry.Append($"\n ORDER BY MAX(updatetime)                                  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCompanyInquireText(string user_id, string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                    ");
            qry.Append($"\n   user_id                                 ");
            qry.Append($"\n , document_id                             ");
            qry.Append($"\n , sub_id                                  ");
            qry.Append($"\n , input_date                              ");
            qry.Append($"\n , company                                 ");
            qry.Append($"\n , product                                 ");
            qry.Append($"\n , origin                                  ");
            qry.Append($"\n , sizes                                   ");
            qry.Append($"\n , unit                                    ");
            qry.Append($"\n , qty                                     ");
            qry.Append($"\n , sale_price                              ");
            qry.Append($"\n , warehouse                               ");
            qry.Append($"\n , purchase_company                        ");
            qry.Append($"\n , purchase_price                          ");
            qry.Append($"\n , remark                                  ");
            qry.Append($"\n , freight                                 ");
            qry.Append($"\n , inquire                                 ");
            qry.Append($"\n , updatetime                              ");
            qry.Append($"\n FROM t_daily_business                      ");
            qry.Append($"\n WHERE 1=1                                   ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n   AND user_id = '{user_id}'         ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n   AND company = '{company}'         ");
            qry.Append($"\n ORDER BY input_date DESC, id, company                     ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetTempData(string user_id, int document_id, int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                    ");
            qry.Append($"\n    user_id                                                                                                                 ");
            qry.Append($"\n  , document_id                                                                                                             ");
            qry.Append($"\n  , document_name                                                                                                           ");
            qry.Append($"\n  , sub_id                                                                                                                  ");
            qry.Append($"\n  , input_date                                                                                                              ");
            qry.Append($"\n  , company                                                                                                                 ");
            qry.Append($"\n  , product                                                                                                                 ");
            qry.Append($"\n  , origin                                                                                                                  ");
            qry.Append($"\n  , sizes                                                                                                                   ");
            qry.Append($"\n  , unit                                                                                                                    ");
            qry.Append($"\n  , qty                                                                                                                     ");
            qry.Append($"\n  , sale_price                                                                                                              ");
            qry.Append($"\n  , warehouse                                                                                                               ");
            qry.Append($"\n  , purchase_company                                                                                                        ");
            qry.Append($"\n  , purchase_price                                                                                                          ");
            qry.Append($"\n  , remark                                                                                                                  ");
            qry.Append($"\n  , freight                                                                                                                 ");
            qry.Append($"\n  , inquire                                                                                                                 ");
            qry.Append($"\n  , updatetime                                                                                                              ");
            qry.Append($"\n  , createtime                                                                                                              ");
            qry.Append($"\n  , cell_style                                                                                                              ");
            qry.Append($"\n  FROM t_daily_business_temp AS a                                                                                                ");
            qry.Append($"\n  WHERE 1=1                                                                                                ");
            qry.Append($"\n   AND user_id = '{user_id}'               ");
            qry.Append($"\n   AND document_id = {document_id}         ");
            qry.Append($"\n   AND id = {id}                           ");
            qry.Append($"\n  ORDER BY input_date, sub_id, company                                                                                          ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;

        }
        public DataTable GetDailyBusiness(string user_id, string sttdate, string enddate, string company, string product, int document_id = 0)
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n  SELECT                                                                                                                    ");
            qry.Append($"\n    user_id                                                                                                                 ");
            qry.Append($"\n  , document_id                                                                                                             ");
            qry.Append($"\n  , document_name                                                                                                           ");
            qry.Append($"\n  , sub_id                                                                                                                  ");
            qry.Append($"\n  , input_date                                                                                                              ");
            qry.Append($"\n  , company                                                                                                                 ");
            qry.Append($"\n  , product                                                                                                                 ");
            qry.Append($"\n  , origin                                                                                                                  ");
            qry.Append($"\n  , sizes                                                                                                                   ");
            qry.Append($"\n  , unit                                                                                                                    ");
            qry.Append($"\n  , qty                                                                                                                     ");
            qry.Append($"\n  , sale_price                                                                                                              ");
            qry.Append($"\n  , warehouse                                                                                                               ");
            qry.Append($"\n  , purchase_company                                                                                                        ");
            qry.Append($"\n  , purchase_price                                                                                                          ");
            qry.Append($"\n  , remark                                                                                                                  ");
            qry.Append($"\n  , freight                                                                                                                 ");
            qry.Append($"\n  , inquire                                                                                                                 ");
            qry.Append($"\n  , updatetime                                                                                                              ");
            qry.Append($"\n  , createtime                                                                                                              ");
            qry.Append($"\n  FROM t_daily_business_temp AS a                                                                                                ");
            qry.Append($"\n  WHERE 1=1                                                                                                                 ");

            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n   AND a.user_id = '{user_id}'         ");
            qry.Append($"\n    AND id = ANY(                                                                                                     ");
            qry.Append($"\n 		SELECT distinct b.id                                                                                      ");
            qry.Append($"\n         FROM t_daily_business_temp AS b                                                                                         ");
            qry.Append($"\n         WHERE 1=1                                                                                                          ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n   AND b.user_id = '{user_id}'         ");
            qry.Append($"\n           AND b.id = ( SELECT MAX(id) FROM t_daily_business_temp AS c WHERE 1=1    ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n   AND c.user_id = '{user_id}'         ");
            qry.Append($"\n           )   ");
            qry.Append($"\n    )                                                                                                                       ");

            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND a.input_date >= '{sttdate}'         ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND a.input_date <= '{enddate}'         ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n   AND a.company LIKE '%{company}%'         ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND a.product LIKE '%{product}%'         ");
                
            qry.Append($"\n  ORDER BY input_date, sub_id, company                                                                                          ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public StringBuilder InsertData(DailyBusinessModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_daily_business (        ");
            qry.Append($"\n   user_id                                 ");
            qry.Append($"\n , document_id                             ");
            qry.Append($"\n , sub_id                                  ");
            qry.Append($"\n , id                                      ");
            qry.Append($"\n , document_name                           ");
            qry.Append($"\n , input_date                              ");
            qry.Append($"\n , company                                 ");
            qry.Append($"\n , product                                 ");
            qry.Append($"\n , origin                                  ");
            qry.Append($"\n , sizes                                   ");
            qry.Append($"\n , unit                                    ");
            qry.Append($"\n , qty                                     ");
            qry.Append($"\n , sale_price                              ");
            qry.Append($"\n , warehouse                               ");
            qry.Append($"\n , purchase_company                        ");
            qry.Append($"\n , purchase_price                          ");
            qry.Append($"\n , freight                                 ");
            qry.Append($"\n , remark                                  ");
            qry.Append($"\n , inquire                                 ");
            qry.Append($"\n , updatetime                              ");
            qry.Append($"\n , createtime                              ");
            qry.Append($"\n , cell_style                              ");
            qry.Append($" ) VALUES (                                  ");
            qry.Append($"\n   '{model.user_id         }'                        ");
            qry.Append($"\n ,  {model.document_id     }                         ");
            qry.Append($"\n ,  {model.sub_id          }                         ");
            qry.Append($"\n ,  {model.id              }                         ");
            qry.Append($"\n , '{model.document_name   }'                        ");
            qry.Append($"\n , '{model.input_date      }'                        ");
            qry.Append($"\n , '{model.company         }'                        ");
            qry.Append($"\n , '{model.product         }'                        ");
            qry.Append($"\n , '{model.origin          }'                        ");
            qry.Append($"\n , '{model.sizes           }'                        ");
            qry.Append($"\n , '{model.unit           }'                         ");
            qry.Append($"\n ,  '{model.qty             }'                         ");
            qry.Append($"\n ,  {model.sale_price      }                         ");
            qry.Append($"\n , '{model.warehouse       }'                        ");
            qry.Append($"\n , '{model.purchase_company}'                        ");
            qry.Append($"\n ,  {model.purchase_price  }                         ");
            qry.Append($"\n , '{model.freight         }'                        ");
            qry.Append($"\n , '{model.remark         }'                        ");
            qry.Append($"\n , '{model.inquire         }'                        ");
            qry.Append($"\n , '{model.updatetime      }'                        ");
            qry.Append($"\n , '{model.createtime      }'                        ");
            qry.Append($"\n , '{model.cell_style      }'                        ");
            qry.Append($" )                                  ");
            string sql = qry.ToString();

            return qry;
        }
        public StringBuilder InsertTempData(DailyBusinessModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_daily_business_temp (        ");
            qry.Append($"\n   user_id                                 ");
            qry.Append($"\n , document_id                             ");
            qry.Append($"\n , sub_id                                  ");
            qry.Append($"\n , id                                      ");
            qry.Append($"\n , document_name                           ");
            qry.Append($"\n , input_date                              ");
            qry.Append($"\n , company                                 ");
            qry.Append($"\n , product                                 ");
            qry.Append($"\n , origin                                  ");
            qry.Append($"\n , sizes                                   ");
            qry.Append($"\n , unit                                    ");
            qry.Append($"\n , qty                                     ");
            qry.Append($"\n , sale_price                              ");
            qry.Append($"\n , warehouse                               ");
            qry.Append($"\n , purchase_company                        ");
            qry.Append($"\n , purchase_price                          ");
            qry.Append($"\n , freight                                 ");
            qry.Append($"\n , remark                                  ");
            qry.Append($"\n , inquire                                 ");
            qry.Append($"\n , updatetime                              ");
            qry.Append($"\n , createtime                              ");
            qry.Append($"\n , cell_style                              ");
            qry.Append($" ) VALUES (                                  ");
            qry.Append($"\n   '{model.user_id         }'                                    ");
            qry.Append($"\n ,  {model.document_id     }                                     ");
            qry.Append($"\n ,  {model.sub_id          }                                     ");
            qry.Append($"\n ,  {model.id              }                                     ");
            qry.Append($"\n , '{AddSlashes(model.document_name)   }'                        ");
            qry.Append($"\n , '{AddSlashes(model.input_date)      }'                        ");
            qry.Append($"\n , '{AddSlashes(model.company)         }'                        ");
            qry.Append($"\n , '{AddSlashes(model.product)         }'                        ");
            qry.Append($"\n , '{AddSlashes(model.origin)          }'                        ");
            qry.Append($"\n , '{AddSlashes(model.sizes)           }'                        ");
            qry.Append($"\n , '{AddSlashes(model.unit)            }'                        ");
            qry.Append($"\n , '{AddSlashes(model.qty)             }'                        ");
            qry.Append($"\n ,  {model.sale_price                  }                         ");
            qry.Append($"\n , '{AddSlashes(model.warehouse)       }'                        ");
            qry.Append($"\n , '{AddSlashes(model.purchase_company)}'                        ");
            qry.Append($"\n ,  {model.purchase_price              }                         ");
            qry.Append($"\n , '{model.freight                     }'                        ");
            qry.Append($"\n , '{AddSlashes(model.remark)          }'                        ");
            qry.Append($"\n , '{AddSlashes(model.inquire)         }'                        ");
            qry.Append($"\n , '{model.updatetime                  }'                        ");
            qry.Append($"\n , '{model.createtime                  }'                        ");
            qry.Append($"\n , '{model.cell_style                  }'                        ");
            qry.Append($" )                                  ");
            string sql = qry.ToString();

            return qry;
        }
        public StringBuilder DeleteTempData(string user_id, int document_id, int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_daily_business_temp WHERE 1=1        ");
            qry.Append($"\n   AND user_id = '{user_id}'             ");
            qry.Append($"\n   AND document_id = {document_id}             ");
            qry.Append($"\n   AND id <= {id - 10}             ");
            /*qry.Append($"\n AND input_date >= '{sttdate}'       ");
            qry.Append($"\n AND input_date <= '{enddate}'       ");*/

            return qry;
        }
        public StringBuilder DeleteData(string user_id, int document_id, string sttdate, string enddate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_daily_business WHERE 1=1        ");
            qry.Append($"\n   AND user_id = '{user_id}'             ");
            //qry.Append($"\n   AND document_id = {document_id}             ");
            /*qry.Append($"\n AND input_date >= '{sttdate}'       ");
            qry.Append($"\n AND input_date <= '{enddate}'       ");*/

            return qry;
        }
    }
}

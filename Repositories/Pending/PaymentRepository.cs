using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Pending
{
    public class PaymentRepository : ClassRoot, IPaymentRepository
    {
        public StringBuilder DeleteSql(PaymentModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_payment             ");
            qry.Append($"\n WHERE 1 = 1                       ");
            qry.Append($"\n   AND id = {model.id}    ");

            string sql = qry.ToString();

            return qry;
        }
        public StringBuilder InsertSql(PaymentModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_payment (           ");
            qry.Append($"\n   id                              ");
            qry.Append($"\n , contract_id                     ");
            qry.Append($"\n , division                        ");
            qry.Append($"\n , payment_date_status             ");
            qry.Append($"\n , payment_date                    ");
            qry.Append($"\n , payment_currency                ");
            qry.Append($"\n , payment_amount                  ");
            qry.Append($"\n , remark                          ");
            qry.Append($"\n , updatetime                      ");
            qry.Append($"\n , edit_user                       ");
            qry.Append($"\n ) VALUES (                        ");
            qry.Append($"\n    {model.id}                     ");
            qry.Append($"\n ,  {model.contract_id}            ");
            qry.Append($"\n , '{model.division}'              ");
            qry.Append($"\n , '{model.payment_date_status}'   ");
            qry.Append($"\n , '{model.payment_date}'          ");
            qry.Append($"\n , '{model.payment_currency}'      ");
            qry.Append($"\n , {model.payment_amount}          ");
            qry.Append($"\n , '{model.remark}'                ");
            qry.Append($"\n , '{model.updatetime}'            ");
            qry.Append($"\n , '{model.edit_user}'             ");
            qry.Append($"\n )                                 ");

            string sql = qry.ToString();

            return qry;
        }
        public DataTable GetPayment(string id, string contract_id, string division, string pay_date)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                             ");
            qry.Append($"\n  *                                ");
            qry.Append($"\nFROM t_payment                     ");
            qry.Append($"\nWHERE 1 = 1                        ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n  AND id = {id}                    ");
            if (!string.IsNullOrEmpty(contract_id))
                qry.Append($"\n  AND contract_id = {contract_id}            ");
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n  AND division = '{division}'      ");
            if (!string.IsNullOrEmpty(pay_date))
                qry.Append($"\n  AND payment_date = '{pay_date}'    ");
            qry.Append($"\n ORDER BY CASE WHEN payment_date_status = '결제완료' THEN 9 ELSE 1 END, payment_date         ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetPaymentAsOne(string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                    ");
            qry.Append($"\n   p.contract_id                                                                                                           ");
            qry.Append($"\n , c.ato_no                                                                                                                ");
            qry.Append($"\n , c.bl_no                                                                                                                 ");
            qry.Append($"\n , c.lc_payment_date                                                                                                       ");
            qry.Append($"\n , date_format(p.payment_date, '%Y-%m-%d') AS payment_date                                                                 ");
            qry.Append($"\n , p.payment_date_status                                                                                                   ");
            qry.Append($"\n , c.payment_bank                                                                                                          ");
            qry.Append($"\n , p.payment_amount                                                                                                        ");
            qry.Append($"\n , p.payment_currency                                                                                                      ");
            qry.Append($"\n , p.remark                                                                                                                ");
            qry.Append($"\n , c.manager                                                                                                               ");
            qry.Append($"\n , c.usance_type                                                                                                           ");
            qry.Append($"\n , c.agency_type                                                                                                           ");
            qry.Append($"\n , c.division                                                                                                              ");
            qry.Append($"\n , c.product                                                                                                               ");
            qry.Append($"\n , c.origin                                                                                                                ");
            qry.Append($"\n , c.sizes                                                                                                                 ");
            qry.Append($"\n , c.box_weight                                                                                                            ");
            qry.Append($"\n , c.quantity_on_paper                                                                                                     ");
            qry.Append($"\n FROM t_payment AS p                                                                                                       ");
            qry.Append($"\n INNER JOIN t_customs AS c                                                                                                 ");
            qry.Append($"\n   ON p.contract_id = c.id                                                                                                 ");
            qry.Append($"\n WHERE 1=1                                                                                                                 ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n  AND p.contract_id = {id}                    ");
            

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public int UpdateStatus(string id, string status, string payment_date)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_payment SET                   ");
            qry.Append($"\n   payment_date_status = '{status}'   ");
            qry.Append($"\n WHERE  id = {id}                     ");
            qry.Append($"\n   AND  payment_date = '{payment_date}'                     ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }
    }
}

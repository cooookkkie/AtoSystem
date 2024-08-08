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
    public class MemoRepository : ClassRoot, IMemoRepository
    {
        public MemoRepository()
        { }

        //INSERT
        public int AddMemo(MemoModel model, IDbTransaction transaction = null)
        {
            //ID가 0인 경우 새ID
            if(model.id ==0){ model.id = GetNextId(); }
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_memo (             ");
            qry.Append($"\n   id                           ");
            qry.Append($"\n , syear                        ");
            qry.Append($"\n , smonth                       ");
            qry.Append($"\n , sday                         ");
            qry.Append($"\n , currency                     ");
            qry.Append($"\n , pay_amount                   ");
            qry.Append($"\n , pay_bank                     ");
            qry.Append($"\n , pay_status                   ");
            qry.Append($"\n , contents                     ");
            qry.Append($"\n , manager                      ");
            qry.Append($"\n , updatetime                   ");
            qry.Append($"\n , backColor                    ");
            qry.Append($"\n , fontColor                    ");
            qry.Append($"\n , font                         ");
            qry.Append($"\n , font_size                    ");
            qry.Append($"\n , font_bold                    ");
            qry.Append($"\n , font_italic                  ");
            qry.Append($"\n , user_auth                    ");
            qry.Append($"\n ) VALUES (                     ");
            qry.Append($"\n    {model.id}                  ");
            qry.Append($"\n ,  {model.syear}               ");
            qry.Append($"\n ,  {model.smonth}              ");
            qry.Append($"\n ,  {model.sday}                ");
            qry.Append($"\n , '{model.currency}'           ");
            qry.Append($"\n ,  {model.pay_amount}          ");
            qry.Append($"\n , '{model.pay_bank}'           ");
            qry.Append($"\n , '{model.pay_status}'         ");
            qry.Append($"\n , '{model.contents}'           ");
            qry.Append($"\n , '{model.manager}'            ");
            qry.Append($"\n , '{model.updatetime}'         ");
            qry.Append($"\n , '{model.backColor}'          ");
            qry.Append($"\n , '{model.fontColor}'          ");
            qry.Append($"\n , '{model.font}'               ");
            qry.Append($"\n , '{model.font_size}'          ");
            qry.Append($"\n ,  {model.font_bold}           ");
            qry.Append($"\n ,  {model.font_italic}         ");
            qry.Append($"\n ,  {model.user_auth}           ");
            qry.Append($"\n )                              ");

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public int GetNextId(IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT IF(MAX(id) IS NULL, 0, MAX(id)) + 1 ");
            qry.Append($" FROM t_memo");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return int.Parse(command.ExecuteScalar().ToString());
        }

        public List<MemoModel> GetMemo(int year, int month, int id, bool dm = false, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                        ");
            qry.Append($"\n   id                       ");
            qry.Append($"\n,  '기타결제' AS division                    ");
            qry.Append($"\n,  syear                    ");
            qry.Append($"\n,  smonth                   ");
            qry.Append($"\n,  sday                     ");
            qry.Append($"\n,  currency                 ");
            qry.Append($"\n,  IF(pay_amount IS NULL, 0 , pay_amount) AS pay_amount            ");
            qry.Append($"\n,  pay_bank                 ");
            qry.Append($"\n,  pay_status               ");
            qry.Append($"\n,  contents                 ");
            qry.Append($"\n,  manager                  ");
            qry.Append($"\n,  updatetime               ");
            qry.Append($"\n,  backColor                ");
            qry.Append($"\n,  fontColor                ");
            qry.Append($"\n,  font                     ");
            qry.Append($"\n,  font_size                ");
            qry.Append($"\n,  font_bold                ");
            qry.Append($"\n,  font_italic              ");
            qry.Append($"\n,  user_auth              ");
            qry.Append($"FROM t_memo                   ");
            qry.Append($"WHERE 1=1                     ");
            if (id != 0) 
            {
                qry.Append($"AND id ={id}              ");
            }
            qry.Append($"AND syear ={year}             ");
            qry.Append($"AND smonth ={month}           ");
            if (dm == false)
            {
                qry.Append($"AND (sday IS NULL OR sday = '') ");
            }
            else
            {
                qry.Append($"AND (sday IS NOT NULL AND sday <> '') ");
            }
            

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetMemoModel(dr);
        }

        public List<MemoModel> GetMemoDay(int year, int month, int days, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                        ");
            qry.Append($"\n   id                       ");
            qry.Append($"\n,  '기타결제' AS division                    ");
            qry.Append($"\n,  syear                    ");
            qry.Append($"\n,  smonth                   ");
            qry.Append($"\n,  sday                     ");
            qry.Append($"\n,  currency                 ");
            qry.Append($"\n,  IF(pay_amount IS NULL, 0 , pay_amount) AS pay_amount            ");
            qry.Append($"\n,  pay_bank                 ");
            qry.Append($"\n,  pay_status               ");
            qry.Append($"\n,  contents                 ");
            qry.Append($"\n,  manager                  ");
            qry.Append($"\n,  updatetime               ");
            qry.Append($"\n,  backColor                ");
            qry.Append($"\n,  fontColor                ");
            qry.Append($"\n,  font                     ");
            qry.Append($"\n,  font_size                ");
            qry.Append($"\n,  font_bold                ");
            qry.Append($"\n,  font_italic              ");
            qry.Append($"\n,  user_auth                ");
            qry.Append($"FROM t_memo                   ");
            qry.Append($"WHERE 1=1                     ");
            qry.Append($"AND syear ={year}             ");
            qry.Append($"AND smonth ={month}           ");
            qry.Append($"AND sday ={days}              ");
            
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetMemoModel(dr);
        }


        private List<MemoModel> GetMemoModel(MySqlDataReader rd)
        {
            List<MemoModel> modelList = new List<MemoModel>();
            while (rd.Read())
            {
                MemoModel model = new MemoModel();
                model.id = int.Parse(rd["id"].ToString());
                model.division = rd["division"].ToString();
                model.syear = int.Parse(rd["syear"].ToString());
                model.smonth = int.Parse(rd["smonth"].ToString());
                model.sday = int.Parse(rd["sday"].ToString());

                model.currency = rd["currency"].ToString();
                model.pay_bank = rd["pay_bank"].ToString();
                model.pay_amount = Convert.ToDouble(rd["pay_amount"]);
                model.pay_status = rd["pay_status"].ToString();

                model.contents =rd["contents"].ToString();
                model.manager = rd["manager"].ToString();
                model.updatetime = rd["updatetime"].ToString();

                model.backColor= rd["backColor"].ToString();
                model.fontColor = rd["fontColor"].ToString();
                model.font = rd["font"].ToString();
                model.font_size = rd["font_size"].ToString();
                model.font_bold = rd["font_bold"].ToString();
                model.font_italic = rd["font_italic"].ToString();

                model.user_auth = Convert.ToInt16(rd["user_auth"]);
                  
                modelList.Add(model);
            }
            rd.Close();
            return modelList;
        }

        public MemoModel GetMemoAsOne(int id = 0, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                          ");
            qry.Append($"\n   id                       ");
            qry.Append($"\n,  syear                    ");
            qry.Append($"\n,  smonth                   ");
            qry.Append($"\n,  sday                     ");
            qry.Append($"\n,  currency                 ");
            qry.Append($"\n,  IF(pay_amount IS NULL, 0 , pay_amount) AS pay_amount            ");
            qry.Append($"\n,  pay_bank                 ");
            qry.Append($"\n,  pay_status               ");
            qry.Append($"\n,  contents                 ");
            qry.Append($"\n,  manager                  ");
            qry.Append($"\n,  updatetime               ");
            qry.Append($"\n,  backColor                ");
            qry.Append($"\n,  fontColor                ");
            qry.Append($"\n,  font                     ");
            qry.Append($"\n,  font_size                ");
            qry.Append($"\n,  font_bold                ");
            qry.Append($"\n,  font_italic              ");
            qry.Append($"\n,  user_auth                ");
            qry.Append($"\n FROM t_memo                  ");
            qry.Append($"\n WHERE 1=1                    ");
            qry.Append($"AND id ={id}                    ");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetMemoAsOneModel(dr);
        }
        private MemoModel GetMemoAsOneModel(MySqlDataReader rd)
        {
            MemoModel model = null;
            while (rd.Read())
            {
                model = new MemoModel();
                model.id = int.Parse(rd["id"].ToString());
                model.syear = int.Parse(rd["syear"].ToString());
                model.smonth = int.Parse(rd["smonth"].ToString());
                model.sday = int.Parse(rd["sday"].ToString());
                model.currency = rd["currency"].ToString();
                model.pay_bank = rd["pay_bank"].ToString();
                model.pay_amount = Convert.ToDouble(rd["pay_amount"]);
                model.pay_status = rd["pay_status"].ToString();
                model.contents = rd["contents"].ToString();
                model.manager = rd["manager"].ToString();
                model.updatetime = rd["updatetime"].ToString();
                model.backColor = rd["backColor"].ToString();
                model.fontColor = rd["fontColor"].ToString();
                model.font = rd["font"].ToString();
                model.font_size = rd["font_size"].ToString();
                model.font_bold = rd["font_bold"].ToString();
                model.font_italic = rd["font_italic"].ToString();
                model.user_auth= Convert.ToInt32(rd["user_auth"].ToString());
            }
            rd.Close();
            return model;
        }

        public int UpdateMemo(MemoModel model, IDbTransaction transaction = null)
        {
            //ID가 0인 경우 새ID
            if (model.id == 0) { model.id = GetNextId(); }
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_memo SET                              ");
            qry.Append($"\n   currency    =  '{model.currency}'          ");
            qry.Append($"\n , pay_bank    =  '{model.pay_bank}'          ");
            qry.Append($"\n , pay_amount  =   {model.pay_amount}         ");
            qry.Append($"\n , pay_status  =  '{model.pay_status}'        ");
            qry.Append($"\n , contents    =  '{model.contents}'          ");
            qry.Append($"\n , updatetime  =  '{model.updatetime}'        ");
            qry.Append($"\n , backColor   =  '{model.backColor}'         ");
            qry.Append($"\n , fontColor   =  '{model.fontColor}'         ");
            qry.Append($"\n , font        =  '{model.font}'              ");
            qry.Append($"\n , font_size   =  '{model.font_size}'         ");
            qry.Append($"\n , font_bold   =   {model.font_bold}          ");
            qry.Append($"\n , font_italic =   {model.font_italic}        ");
            qry.Append($"\n , user_auth   =   {model.user_auth}          ");
            qry.Append($"\n , syear       =   {model.syear}              ");
            qry.Append($"\n , smonth      =   {model.smonth}             ");
            qry.Append($"\n , sday        =   {model.sday}               ");
            qry.Append($"\n WHERE  id = {model.id}                       ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }
        public int PaymentMemo(int id, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_memo SET                              ");
            qry.Append($"\n   pay_status    =  '결제완료'                ");
            
            qry.Append($"\n WHERE  id = {id}                       ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public int DeleteMemo(int id, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_memo                     ");
            qry.Append($"\n WHERE  id = {id}                     ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public List<MemoModel> GetSaleMemo(DateTime sttdate, DateTime enddate, int id = 0, bool dm = false, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                        ");
            qry.Append($"\n   id                       ");
            qry.Append($"\n,  '영업메모' AS division                    ");
            qry.Append($"\n,  syear                    ");
            qry.Append($"\n,  smonth                   ");
            qry.Append($"\n,  sday                     ");
            qry.Append($"\n,  currency                 ");
            qry.Append($"\n,  IF(pay_amount IS NULL, 0 , pay_amount) AS pay_amount            ");
            qry.Append($"\n,  pay_bank                 ");
            qry.Append($"\n,  pay_status               ");
            qry.Append($"\n,  contents                 ");
            qry.Append($"\n,  manager                  ");
            qry.Append($"\n,  updatetime               ");
            qry.Append($"\n,  backColor                ");
            qry.Append($"\n,  fontColor                ");
            qry.Append($"\n,  font                     ");
            qry.Append($"\n,  font_size                ");
            qry.Append($"\n,  font_bold                ");
            qry.Append($"\n,  font_italic              ");
            qry.Append($"\n,  user_auth                ");
            qry.Append($"\nFROM t_memo                   ");
            qry.Append($"\nWHERE 1=1                     ");
            qry.Append($"\n  ANd user_auth < 50         ");
            if (id != 0)
            {
                qry.Append($"\nAND id ={id}              ");
            }
            qry.Append($"\nAND STR_TO_DATE(concat(syear, '-', smonth, '-', sday), '%Y-%m-%d')  >= '{sttdate.ToString("yyyy-MM-dd")}'           ");
            qry.Append($"\nAND STR_TO_DATE(concat(syear, '-', smonth, '-', sday), '%Y-%m-%d')  <= '{enddate.ToString("yyyy-MM-dd")}'           ");
            if (dm == false)
            {
                qry.Append($"\nAND (sday IS NULL OR sday = '') ");
            }
            else
            {
                qry.Append($"\nAND (sday IS NOT NULL AND sday <> '') ");
            }
            qry.Append($"\n UNION ALL                                                                 ");
            qry.Append($"\n SELECT                                                                    ");
            qry.Append($"\n  id                                                                       ");
            qry.Append($"\n , '보험만료일' AS division                                                    ");
            qry.Append($"\n , year(insurance_enddate) AS syear                                        ");
            qry.Append($"\n , month(insurance_enddate) AS smonth                                      ");
            qry.Append($"\n , day(insurance_enddate) AS sday                                          ");
            qry.Append($"\n , ''                                                                      ");
            qry.Append($"\n , IFNULL(insurance_amount, 0)                                             ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  CONCAT(name, ' 보험만료일')                                                                      ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  ''                                                                     ");
            qry.Append($"\n ,  0                                                                     ");
            qry.Append($"\n FROM t_company                                                            ");
            qry.Append($"\n WHERE IFNULL(insurance_enddate, '') <> ''                                 ");
            qry.Append($"\n AND STR_TO_DATE(insurance_enddate, '%Y-%m-%d')  >= '{sttdate.ToString("yyyy-MM-dd")}'           ");
            qry.Append($"\n AND STR_TO_DATE(insurance_enddate, '%Y-%m-%d')  <= '{enddate.ToString("yyyy-MM-dd")}'           ");


            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetMemoModel(dr);
        }

        public List<MemoModel> GetTradeMemo(DateTime sttdate, DateTime enddate, int id = 0, bool dm = false, string manager = "", string contents = "", IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                        ");
            qry.Append($"\n   id                       ");
            qry.Append($"\n,  '기타결제' AS division                    ");
            qry.Append($"\n,  syear                    ");
            qry.Append($"\n,  smonth                   ");
            qry.Append($"\n,  sday                     ");
            qry.Append($"\n,  IF(currency = '', 'KRW', currency) AS currency                 ");
            qry.Append($"\n,  IF(pay_amount IS NULL, 0 , pay_amount) AS pay_amount            ");
            qry.Append($"\n,  pay_bank                 ");
            qry.Append($"\n,  pay_status               ");
            qry.Append($"\n,  contents                 ");
            qry.Append($"\n,  manager                  ");
            qry.Append($"\n,  updatetime               ");
            qry.Append($"\n,  backColor                ");
            qry.Append($"\n,  fontColor                ");
            qry.Append($"\n,  font                     ");
            qry.Append($"\n,  font_size                ");
            qry.Append($"\n,  font_bold                ");
            qry.Append($"\n,  font_italic              ");
            qry.Append($"\n,  user_auth                ");
            qry.Append($"\nFROM t_memo                   ");
            qry.Append($"\nWHERE 1=1                     ");
            qry.Append($"\n  AND user_auth >= 50          ");
            if (id != 0)
            {
                qry.Append($"\n AND id ={id}              ");
            }
            qry.Append($"\n AND STR_TO_DATE(concat(syear, '-', smonth, '-', sday), '%Y-%m-%d')  >= '{sttdate.ToString("yyyy-MM-dd")}'           ");
            qry.Append($"\n AND STR_TO_DATE(concat(syear, '-', smonth, '-', sday), '%Y-%m-%d')  <= '{enddate.ToString("yyyy-MM-dd")}'           ");
            if (dm == false)
            {
                qry.Append($"\n AND (sday IS NULL OR sday = '') ");
            }
            else
            {
                qry.Append($"\n AND (sday IS NOT NULL AND sday <> '') ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n AND manager LIKE '%{manager}%'           ");
            }
            if (!string.IsNullOrEmpty(contents))
            {
                qry.Append($"\n AND contents LIKE '%{contents}%'           ");
            }

            string sql = qry.ToString();

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetMemoModel(dr);
        }

        public List<MemoModel> GetTradeMemo2(DateTime enddate, int id = 0, bool dm = false, string manager = "", IDbTransaction transaction = null)
        {
            
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                        ");
            qry.Append($"\n   id                       ");
            qry.Append($"\n,  '기타결제' AS division                    ");
            qry.Append($"\n,  syear                    ");
            qry.Append($"\n,  smonth                   ");
            qry.Append($"\n,  sday                     ");
            qry.Append($"\n,  currency                 ");
            qry.Append($"\n,  IF(pay_amount IS NULL, 0 , pay_amount) AS pay_amount            ");
            qry.Append($"\n,  pay_bank                 ");
            qry.Append($"\n,  pay_status               ");
            qry.Append($"\n,  contents                 ");
            qry.Append($"\n,  manager                  ");
            qry.Append($"\n,  updatetime               ");
            qry.Append($"\n,  backColor                ");
            qry.Append($"\n,  fontColor                ");
            qry.Append($"\n,  font                     ");
            qry.Append($"\n,  font_size                ");
            qry.Append($"\n,  font_bold                ");
            qry.Append($"\n,  font_italic              ");
            qry.Append($"\n,  user_auth                ");
            qry.Append($"\nFROM t_memo                   ");
            qry.Append($"\nWHERE 1=1                     ");
            qry.Append($"\n  AND user_auth >= 50          ");
            if (id != 0)
            {
                qry.Append($"\n AND id ={id}              ");
            }
            qry.Append($"\n AND STR_TO_DATE(concat(syear, '-', smonth, '-', sday), '%Y-%m-%d')  < '{enddate.ToString("yyyy-MM-dd")}'           ");
            qry.Append($"\n AND pay_status<> '결제완료'           ");
            qry.Append($"\n AND (sday IS NOT NULL AND sday <> '') ");

            
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n AND manager LIKE '%{manager}%'           ");
            }

            string sql = qry.ToString();

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetMemoModel(dr);
        }
    }
}

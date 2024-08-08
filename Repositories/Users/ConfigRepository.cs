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
    public class ConfigRepository : ClassRoot, IConfigRepository
    {
        public ConfigModel GetConfigChecked(string user_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   *                       ");
            qry.Append($"FROM user_config               ");
            qry.Append($"WHERE user_id = '{user_id}'  ");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return ConfigCheckedModel(dr);
        }

        public int InsertConfig(string user_id, string whr, string val)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO user_config (         ");
            qry.Append($"\n   user_id                       ");
            qry.Append($"\n , shipment_checked              ");
            qry.Append($"\n , payment_checked               ");
            qry.Append($"\n , memo_checked                  ");
            qry.Append($"\n , unknown_checked               ");
            qry.Append($"\n , salememo_checked              ");
            qry.Append($"\n , arrive_checked                ");
            qry.Append($"\n , inspection_checked            ");
            qry.Append($"\n ) VALUES (                      ");
            qry.Append($"\n    '{user_id}'                  ");
            if (whr == "shipment_checked")
            {
                qry.Append($"\n ,   {val}                   ");
            }
            else
            {
                qry.Append($"\n ,   false                   ");
            }
            if (whr == "payment_checked")
            {
                qry.Append($"\n ,   {val}                   ");
            }
            else
            {
                qry.Append($"\n ,   false                   ");
            }
            if (whr == "memo_checked")
            {
                qry.Append($"\n ,   {val}                   ");
            }
            else
            {
                qry.Append($"\n ,   false                   ");
            }
            if (whr == "unknown_checked")
            {
                qry.Append($"\n ,   {val}                   ");
            }
            else
            {
                qry.Append($"\n ,   false                   ");
            }
            if (whr == "salememo_checked")
            {
                qry.Append($"\n ,   {val}                   ");
            }
            else
            {
                qry.Append($"\n ,   false                   ");
            }
            if (whr == "arrive_checked")
            {
                qry.Append($"\n ,   {val}                   ");
            }
            else
            {
                qry.Append($"\n ,   false                   ");
            }
            if (whr == "inspection_checked")
            {
                qry.Append($"\n ,   {val}                   ");
            }
            else
            {
                qry.Append($"\n ,   false                   ");
            }
            qry.Append($"\n )                              ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        
        public int updateConfig(string user_id, string whr, string val)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE user_config SET            ");
            qry.Append($"\n   {whr} =  {val}                ");            
            qry.Append($"\n   WHERE user_id =  '{user_id}'  ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        

        private ConfigModel ConfigCheckedModel(MySqlDataReader rd)
        {
            ConfigModel model = new ConfigModel();
            while (rd.Read())
            {
                model.user_id = rd["user_id"].ToString();
                model.shipment_checked = Convert.ToBoolean(rd["shipment_checked"]);
                model.payment_checked = Convert.ToBoolean(rd["payment_checked"]);
                model.memo_checked = Convert.ToBoolean(rd["memo_checked"]);
                model.unknown_checked = Convert.ToBoolean(rd["unknown_checked"]);
                model.salememo_checked = Convert.ToBoolean(rd["salememo_checked"]);
                model.arrive_checked = Convert.ToBoolean(rd["arrive_checked"]);
                model.inspection_checked = Convert.ToBoolean(rd["inspection_checked"]);
            }
            rd.Close();

            if(string.IsNullOrEmpty(model.user_id))
            {
                model = null;
            }

            return model;
        }

        public DataTable GetCountryDelivery(string origin)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   *                       ");
            qry.Append($"FROM t_country                 ");
            qry.Append($"WHERE country_name LIKE '%{origin}%'  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public CountryModel GetCountryConfigAsOne(string name = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   *                       ");
            qry.Append($"FROM t_country               ");
            if (name != null)
            {
                qry.Append($"WHERE country_name = '{name}'  ");
            }
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return CountryAsOneModel(dr);
        }

        public DataTable GetCountry(string country)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                         ");
            qry.Append($"\n  country_name               ");
            qry.Append($"\n, delivery_days              ");
            qry.Append($"\nFROM t_country               ");
            if (!string.IsNullOrEmpty(country))
            {
                qry.Append($"\nWHERE country_name = '{country}'  ");
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


        private CountryModel CountryAsOneModel(MySqlDataReader rd)
        {

            CountryModel model = null;
            while (rd.Read())
            {
                model = new CountryModel();
                model.country_name = rd["country_name"].ToString();
                model.delivery_days = Convert.ToInt32(rd["delivery_days"]);
            }
            rd.Close();

            return model;
        }

        public List<CountryModel> GetCountryConfig(string name = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   *                       ");
            qry.Append($"FROM t_country               ");
            if (name != null)
            {
                qry.Append($"WHERE country_name = '{name}'  ");
            }
            

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return CountryModel(dr);
        }

        private List<CountryModel> CountryModel(MySqlDataReader rd)
        {
            List<CountryModel> countryModel = new List<CountryModel>();
            
            while (rd.Read())
            {
                CountryModel model = new CountryModel();
                model.country_name = rd["country_name"].ToString();
                model.delivery_days = Convert.ToInt32(rd["delivery_days"]);
                countryModel.Add(model);
            }
            rd.Close();

            return countryModel;
        }

        public int InsertCountry(string name, int days)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_country (           ");
            qry.Append($"\n   country_name                  ");
            qry.Append($"\n , delivery_days                 ");
            qry.Append($"\n ) VALUES (                      ");
            qry.Append($"\n   '{name}'                      ");
            qry.Append($"\n ,  {days}                       ");
            qry.Append($"\n )                               ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }
        public int updateCountry(string name, int days)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_country SET                ");
            qry.Append($"\n   delivery_days =  {days}                  ");
            qry.Append($"\n   WHERE country_name =  '{name}'  ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public int DeleteCountry(string name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_country           ");
            qry.Append($"\n   WHERE country_name =  '{name}'  ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }
    }
}

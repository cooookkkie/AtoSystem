using MySql.Data.MySqlClient;

namespace Libs.Tools
{
    internal class MySqlCommand
    {
        private string v;
        private MySqlConnection connection;

        public MySqlCommand(string v, MySqlConnection connection)
        {
            this.v = v;
            this.connection = connection;
        }
    }
}
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RelatorioFinanceiroV5.Classes
{
    public class Connection
    {
        public static MySqlConnection conn()
        {
            string cs = ConfigurationManager.ConnectionStrings["spreadsheetConnectionString"].ConnectionString;
            MySqlConnection myConn = new MySqlConnection(cs);
            return myConn;
        }
    }
}
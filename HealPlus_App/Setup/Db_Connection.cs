using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HealPlus_App.Setup
{
    class Db_Connection
    {
        public SqlConnection SqlConnect;
        public string SqlNotice;
        private readonly string dbserver;
        private readonly string dbname;
        public Db_Connection()
        {
            SqlConnect = new SqlConnection();
            dbserver = @"LAPTOP-OD4256VB";
            dbname = "DB_HPLUS_PROJECT";
        }
        private void Notice_Handler(object sender, SqlInfoMessageEventArgs e)
        {
            SqlNotice = e.Message;
        }
        public bool OpenConnection()
        {
            //SqlConnect.ConnectionString = @"Data Source=LAPTOP-OD4255VB;Initial Catalog=DB_HPLUS_PROJECT;Integrated Security=True";
            SqlConnect.ConnectionString = $"Server={dbserver};Database={dbname};Trusted_Connection=yes";
            SqlConnect.InfoMessage += Notice_Handler;
            SqlConnect.FireInfoMessageEventOnUserErrors = true;
            SqlConnect.Open();
            return true;
        }
        public bool CloseConnection()
        {
            SqlConnect.InfoMessage -= Notice_Handler;
            SqlConnect.Close();
            return true;
        }
    }
}

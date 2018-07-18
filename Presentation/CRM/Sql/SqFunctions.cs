using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Sql
{
    public class SqFunctions
    {

        public string sqlExecute(string commando)
        {
            string string_conecta = ConfigurationManager.ConnectionStrings["oracle_entities"].ConnectionString;
            SqlConnection connection = new SqlConnection(string_conecta);
            SqlCommand cmd = new SqlCommand(commando, connection);
            SqlDataReader reader;
            string columnName = "";
            cmd.CommandType = CommandType.Text;
            connection.Open();
            reader = cmd.ExecuteReader();
            if (reader.Read()) // you only have one row so you can use "if" instead of "while"
            {
                columnName = Convert.ToString(reader.GetString(0).ToString());

            }

            connection.Close();


            return columnName;
        }
    }
}
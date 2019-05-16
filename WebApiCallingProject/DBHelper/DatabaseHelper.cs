using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DBHelper
{
    public class DatabaseHelper
    {
        SqlConnection sqlConnection = new SqlConnection();

        public DataTable GetResults(string sqlQuery)
        {
            OpenSqlConn();

            DataTable DtResult = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlQuery, sqlConnection);
            sqlDataAdapter.Fill(DtResult);

            return DtResult;
        }

        public DataTable PostValues(string sqlQuery)
        {
            OpenSqlConn();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            return null;
        }

        public DataTable StoreProcedureQuery(string storeProcedureMethod)
        {
            DataTable Dtresult = new DataTable();
            OpenSqlConn();

            SqlCommand sqlCommand = new SqlCommand(storeProcedureMethod, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            Dtresult.Load(sqlCommand.ExecuteReader());

            return Dtresult;
        }

        //public void StoreProcedureQuery(string storeProcedureMethod, DBDataModel dbDataModel)
        public DataTable StoreProcedureSearchQuery(string storeProcedureMethod, object property)
        {
            DataTable Dtresult = new DataTable();
            //DataTable Dtresult = new DataTable();
            OpenSqlConn();

            SqlCommand sqlCommand = new SqlCommand(storeProcedureMethod, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            foreach (var parameter in property.GetType().GetProperties())
            {
                var commandParameter = sqlCommand.CreateParameter();
                commandParameter.ParameterName = "@" + parameter.Name;
                commandParameter.Value = parameter.GetValue(property);
                sqlCommand.Parameters.Add(commandParameter);
            }
            Dtresult.Load(sqlCommand.ExecuteReader());

            return Dtresult;
        }

        public void StoreProcedureOtherQuery(string storeProcedureMethod, object property)
        {
            DataTable Dtresult = new DataTable();
            //DataTable Dtresult = new DataTable();
            OpenSqlConn();

            SqlCommand sqlCommand = new SqlCommand(storeProcedureMethod, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (var parameter in property.GetType().GetProperties())
            {
                var commandParameter = sqlCommand.CreateParameter();
                commandParameter.ParameterName = "@" + parameter.Name;
                commandParameter.Value = parameter.GetValue(property);
                sqlCommand.Parameters.Add(commandParameter);
            }

            sqlCommand.ExecuteNonQuery();
        }

        private void OpenSqlConn()
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.ConnectionString = "Data Source = 192.168.12.20; Initial Catalog = Training2019; User ID=qancs_imarc; Password=qancs@2019  ";
                sqlConnection.Open();
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using BlogAdapter.Extensions;

namespace BlogsAdapter.Repository
{
    public class DatabaseRepository
    {
        #region Connection
        private const string _password = "T3mpPassword*";
        private const string _server = "localhost";
        private const string _dataBase = "BlogPosts_Master";
        private const string _username = "sa";
        
        public string ConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Persist Security Info=True;", _server, _dataBase, _username, _password);

        #endregion

        #region Procedures
        public DataTable GetDataTable(string storedProcedure, string connectionString, Dictionary<string,object> parameters)
        {
            try
            {
                DataTable dataTable = new DataTable();
                using (var sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCommand.Fill(parameters);
                    sqlCommand.Connection.Open();
                    using (DbDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        dataTable.Load(dataReader);
                    }
                }
                return dataTable;
            }
            catch(System.Exception e)
            {
                throw new System.Exception(e.Message + e.StackTrace);
            }
        }

        public void Execute(string storedProcedure, string connectionString, Dictionary<string, object> parameters)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConnection))
            {
                sqlCommand.Fill(parameters);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
            }
        }

        
        #endregion

    }
}
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BlogAdapter.Extensions
{
    public static class SqlCommandExtension
    {
        public static void Fill(this SqlCommand sqlCommand, Dictionary<string, object> parameters)
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (var parameter in parameters)
            {
                SqlParameter sqlParameter = new SqlParameter(parameter.Key, parameter.Value);
                sqlCommand.Parameters.Add(sqlParameter);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using Npgsql;

namespace VectorTile
{
    public class PgsqlHelper
    {
        private static string connStr = @"PORT=5432;DATABASE=postgisdbss;HOST=localhost;PASSWORD=geoserver;USER ID=postgres";
        #region 查询操作
        public static DataTable ExecuteQuery(string sqrstr)
        {
            NpgsqlConnection sqlConn = new NpgsqlConnection(connStr);
            DataTable ds = new DataTable();
            try
            {
                using (NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sqrstr, sqlConn))
                {
                    sqldap.Fill(ds);
                }
                return ds;
            }
            catch (System.Exception ex)
            {
                // throw ex;
                return ds;
            }
        }
        public static DataTable ExecuteQuery(string sqrstr, params NpgsqlParameter[] npgsqlParameters)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqrstr;
                    cmd.Parameters.AddRange(npgsqlParameters);

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    return dataset.Tables[0];
                }
            }
        }
        #endregion
        #region 增删改操作
        public static int ExecuteNonQuery(string sqrstr, params NpgsqlParameter[] npgsqlParameters)
        {


            NpgsqlConnection sqlConn = new NpgsqlConnection(connStr);
            try
            {
                sqlConn.Open();
                using (NpgsqlCommand pgsqlCommand = new NpgsqlCommand(sqrstr, sqlConn))
                {
                    foreach (NpgsqlParameter parm in npgsqlParameters)
                        pgsqlCommand.Parameters.Add(parm);
                    int r = pgsqlCommand.ExecuteNonQuery();  //执行查询并返回受影响的行数
                    sqlConn.Close();
                    return r; //r如果是>0操作成功！ 
                }
            }
            catch (System.Exception ex)
            {
                return 0;
            }

        }
        #endregion
    }
}

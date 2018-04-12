using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceFormatConvert
{
    public class SQLHelper
    {
        private string m_SqlConnectionString = "";
        private SqlConnection conn = null;

        public SQLHelper(string sqlConnectionString)
        {
            if (sqlConnectionString.Length > 0)
                m_SqlConnectionString = sqlConnectionString;
            if (!TestSqlConnection()) throw new Exception("数据库连接失败");
        }

        private SqlConnection GetConn()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }
        private bool TestSqlConnection()
        {
            bool IsCanConnect = true;
            try
            {
                conn = new SqlConnection(m_SqlConnectionString);
                conn.Open();
            }
            catch
            {
                IsCanConnect = false;
            }
            finally
            {
                conn.Close();
            }
            return IsCanConnect;
        }

        /// <summary>  
        /// 执行增删改操作  
        /// </summary>  
        /// <param name="cmdText"></param>  
        /// <param name="cmdType"></param>  
        /// <returns></returns>  
        public int ExecuteNoQuery(string cmdText, CommandType cmdType)
        {
            int ret = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, GetConn()))
                {
                    cmd.CommandType = cmdType;
                    ret = cmd.ExecuteNonQuery();
                }
                return ret;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>  
        /// 执行查询操作  
        /// </summary>  
        /// <param name="cmdText"></param>  
        /// <param name="cmdType"></param>  
        /// <returns></returns>  
        public DataTable ExecuteQuery2DataTable(string cmdText, CommandType cmdType)
        {
            DataTable dt = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, GetConn()))
                {
                    cmd.CommandType = cmdType;
                    using (SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        dt = new DataTable();
                        dt.Load(sdr);
                    }
                }
                return dt;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>  
        /// 执行查询操作  
        /// </summary>  
        /// <param name="cmdText"></param>  
        /// <param name="cmdType"></param>  
        /// <returns></returns>  
        public DataSet ExecuteQuery2DataSet(string cmdText)
        {
            DataSet ds = null;
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmdText, GetConn()))
                {
                    ds = new DataSet();
                    da.Fill(ds);
                }
                return ds;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
    }  
}
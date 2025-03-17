using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AIOllama.common
{
    internal class MysqlHelper
    {
        private static string connString = "Host=localhost;DataBase=hrip;user id=root;" +
            "password=hxy135246!@#;pooling=false;charset=utf8";

        public static int ExecuteSql(string SQLString)
        {
            //string _connString = "server=localhost;database=mytest;username=root;password=root";
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, conn))
                {
                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();

                        conn.Close();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }
        public static DataSet SelectSql(string SQLString)
        {
            MySqlDataAdapter myadp;
            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandType = System.Data.CommandType.Text;
                        myadp = new MySqlDataAdapter(SQLString, conn);
                        myadp.Fill(ds, "table");
                        conn.Close();
                        return ds;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }
        public static Boolean IsConnectData()
        {
            try
            {
                //创建数据库连接对象
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    //打开连接
                    conn.Open();
                    conn.Close();
                    return true; ;
                }
            }
            catch
            {
                return false; ;
            }
        }

    }
}

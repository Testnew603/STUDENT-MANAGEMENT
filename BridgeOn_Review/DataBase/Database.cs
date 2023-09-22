using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BridgeOn_Review.DataBase
{
    public class Database
    {
        string DefaultConnection= "Server=DESKTOP-317LICC\\SQLEXPRESS;Database=BridgeOnDb;Integrated Security=True;trustServerCertificate=true";        
            SqlConnection con = null;
            public Database()
            {
                con = new SqlConnection(DefaultConnection);
            }
            public void Excecute(string Query, params object[] values)
            {
                try
                {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                if(values.Length >= 1)
                {
                    int counter = 1;
                    foreach(object value in values) 
                    {
                        cmd.Parameters.AddWithValue("@" + counter, value);
                        counter++;
                    }
                }
                cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string msg = (ex.Message + "Error Occured");
                string errorMessage = ex.Message;
                
                //int errorCode = ex.ErrorCode;
            }

                finally 
                { 
                    con.Close();
                }
            }

            public SqlDataReader Select(string Query, params string[] values)
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.CommandType= CommandType.StoredProcedure;
                if (values.Length >= 1)
                {
                    int counter = 1;

                    foreach (string value in values)
                    {
                        cmd.Parameters.AddWithValue("@" + counter, value);
                        counter++;
                    }
                }
                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    string msg = (ex.Message + "Error Occured");
                }
                return null;
            }

        public string SelectSingle(string Query, params string[] values) 
        {
            var reader = Select(Query, values);
            return reader[0].ToString();
        }

        public List<string> SelectList(string Query, params string[] values)
        {
            var reader = Select(Query, values);

            List<string> list = new List<string>();
       
            while (reader.Read())
            {
                list.Add(reader[1].ToString());
             
            }
            return list;
        }
        public DataTable SelectTable(string Query, params object[] values)
        {
           DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.StoredProcedure;       
            if (values != null && values.Length >= 0)
            {               
                int counter = 1;

                foreach (object value in values)
                {
                    cmd.Parameters.AddWithValue("@" + counter, value);
                    counter++;
                }
            }
            try
            {
                con.Open();
                SqlDataReader reader1 = cmd.ExecuteReader();
                dt.Load(reader1);  
             }
            catch(Exception ex)
            {
                throw;
            }            
            return dt;
        }



        public void ExcecuteParam(string Query, object? type, params object[] values)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.CommandType = CommandType.StoredProcedure;
               
                if (values.Length >= 1)
                {
                    int counter = 1;
                    foreach (object value in values)
                    {
                        cmd.Parameters.AddWithValue("@" + counter, value);
                        counter++;
                    }
                }
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string msg = (ex.Message + "Error Occured");
            }

            finally
            {
                con.Close();
            }
        }

        public SqlDataReader SelectParam(string Query, string? type, params string[] values)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                if (values.Length >= 1)
                {
                    int counter = 1;

                    foreach (string value in values)
                    {
                        cmd.Parameters.AddWithValue("@" + counter, value);
                        counter++;
                    }
                }
                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                string msg = (ex.Message + "Error Occured");
            }
            return null;
        }



        public DataTable SelectTableParam(string Query, string? type, params object[] values)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", type);
            if (values != null && values.Length >= 0)
            {
                int counter = 1;
                foreach (object value in values)
                {
                    cmd.Parameters.AddWithValue("@" + counter, value);
                    counter++;
                }
            }
            try
            {
                con.Open();
                SqlDataReader reader1 = cmd.ExecuteReader();
                dt.Load(reader1);
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }
    }       
    }


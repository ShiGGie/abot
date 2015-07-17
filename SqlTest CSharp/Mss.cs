using System;

using SqlTest_CSharp;
/* http://stackoverflow.com/questions/8475083/multiple-threaded-database-updated
 * Seems like its best to keep functions here, and the caller of this class to be wrapped in its own
 *  using(connection) statement, then passing connection to this class. */

namespace SqlTest_CSharp
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Diagnostics;
    using System.Collections;
    using System.Data.Odbc;

    // Microsoft SQL Server Expresss 2012

    //Wrapper class for Database functions
    [Serializable]
    static public class Mss
    {
        //(using connection) outside of every call.
        //Depends on caller to handle exceptions

        //variables must also be static to be used in static methods
        static private DBConfiguration dbConfig = new DBConfiguration();
        static private String connectionString = buildConnectionString();

        //Windows Auth login
        //Requires caller to close connection
        private static String buildConnectionString()
        {
            //Stringbuilder improves performance
            StringBuilder buildString = new StringBuilder();
            buildString.Append("Server=");
            //TODO, GUI for specifying these things
            //host = (host[host.Length-1] == '/') ? host.Remove(host.Length-1) : host;
            buildString.Append(dbConfig.Host);
            buildString.Append(";Integrated Security=True;Database=");
            buildString.Append(dbConfig.DBName);
            return buildString.ToString();

        }

        private static Boolean tableExists()
        {
            Boolean exists;

            try
            {
                // ANSI SQL way.  Works in PostgreSQL, MSSQL, MySQL.  
                var cmd = new OdbcCommand(
                  "select case when exists((select * from information_schema.tables where table_name = '" + dbConfig.ContentTable + "')) then 1 else 0 end");

                exists = (int)cmd.ExecuteScalar() == 1;
            }
            catch
            {
                try
                {
                    // Other RDBMS.  Graceful degradation
                    exists = true;
                    var cmdOthers = new OdbcCommand("select 1 from " + dbConfig.ContentTable + " where 1 = 0");
                    cmdOthers.ExecuteNonQuery();
                }
                catch
                {
                    exists = false;
                }
            }
            return exists;
        }
        public static void setupDatabase()
        {
            if (!tableExists())
            {
                StringBuilder buildString = new StringBuilder();
                buildString.Append("CREATE TABLE ");
                buildString.Append(dbConfig.ContentTable);
                buildString.Append("(");
                buildString.Append(dbConfig.ContentTableSchema);
                buildString.Append(")");

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connectionString;
                    SqlCommand create = new SqlCommand(buildString.ToString(), conn);
                    try
                    {
                        conn.Open();
                        create.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        //Return table of URIs
        public static ArrayList findContentWithUri(Uri uri)
        {
            var list = new ArrayList();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                SqlCommand query = new SqlCommand("SELECT * FROM c WHERE uri=@uri", conn);
                query.Parameters.AddWithValue("@uri", uri.ToString());
                try
                {
                    conn.Open();
                    using (var reader = query.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string data1 = reader["uri"].ToString();
                                string data2 = reader["content"].ToString();
                                list.Add(data1);
                                list.Add(data2);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return list;
        }

        public static ArrayList getUriWithCriteria(String criteria)
        {
            var list = new ArrayList();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                SqlCommand query = new SqlCommand("SELECT * FROM c WHERE content=@text", conn);
                query.Parameters.AddWithValue("@text", criteria);
                try
                {
                    conn.Open();
                    using (var reader = query.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string data1 = reader["uri"].ToString();
                                list.Add(data1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return list;
        }


        //Add entry to database
        public static Boolean addRecord(Uri uri, String content)
        {
            Boolean ret = false;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                SqlCommand cmd = new SqlCommand("INSERT INTO c (varchar(256), text) VALUES (@0, @1)", conn);
                cmd.Parameters.AddWithValue("@0", uri.ToString());
                cmd.Parameters.AddWithValue("@1", content);

                try
                {
                    conn.Open();
                    Console.WriteLine("Inserted" + uri.ToString() + ". Total rows affected are " + cmd.ExecuteNonQuery());
                    ret = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return ret;
        }

        //Drop entries in database
        static Boolean dropRecordByUri(Uri uri)
        {
            Boolean ret = false;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                SqlCommand cmd = new SqlCommand("DELETE FROM c WHERE uri=@0", conn);
                cmd.Parameters.AddWithValue("@0", uri.ToString());

                try
                {
                    conn.Open();
                    Console.WriteLine("Deleted" + uri.ToString() + ". Total rows affected are " + cmd.ExecuteNonQuery());
                    ret = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return ret;
        }

        //Tests only.
        //Boolean dropTable();

    }
}
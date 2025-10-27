using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Society_Management_System
{
    /// <summary>
    /// Database Helper class for executing queries and managing connections
    /// </summary>
    public static class DBHelper
    {
        private static readonly string connStr;

        static DBHelper()
        {
            var cs = ConfigurationManager.ConnectionStrings["societyDB"];
            if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString))
                throw new InvalidOperationException("Connection string 'societyDB' is missing from Web.config.");
            connStr = cs.ConnectionString;
        }

        public static DataTable GetData(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandTimeout = 60;
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static int Execute(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandTimeout = 60;
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandTimeout = 60;
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteScalar();
                }
            }
        }

        public static DataTable ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters = null)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(procedureName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    return con.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool ExecuteTransaction(string[] queries, SqlParameter[][] parameters)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < queries.Length; i++)
                        {
                            using (SqlCommand cmd = new SqlCommand(queries[i], con, transaction))
                            {
                                cmd.CommandTimeout = 60;
                                if (parameters != null && parameters.Length > i && parameters[i] != null && parameters[i].Length > 0)
                                    cmd.Parameters.AddRange(parameters[i]);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        try { transaction.Rollback(); } catch { }
                        return false;
                    }
                }
            }
        }
    }
}
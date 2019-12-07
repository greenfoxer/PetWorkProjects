using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dbfCoreDLL.dbfHelper;

namespace dbfCoreDLL.sqlHelper
{
    public class SQLParameters
    {
        public string sqlConnectionString = null;
        public int sqlBulkCopyBatchSize = 100000;
        //Параметр для записи в DBF (какую таблицу будем пытаться записать в DBF)
        public string sqlTableName = null;
        public int rowsToSelect = 1000000;
    }

    public static class SQLHelper
    {
        /// <summary>
        public static event dbfCoreEventHelper BeginSomeStep;
        public static event dbfCoreEventHelper BeginSomeIteration;
        public static event dbfCoreEventHelper BeginSomeError;
        public static string className = "SQLHelper";
        /// </summary>
        private static Dictionary<Type, string> dataTypes = new Dictionary<Type, string>()
        {
            {typeof(Boolean), "BIT"},
            {typeof(Byte), "TINYINT"},
            {typeof(Int16), "SMALLINT"},
            {typeof(Int32), "INT"},
            {typeof(Int64), "BIGINT"},
            {typeof(Decimal), "DECIMAL (20, [Precision])"},
            {typeof(Double), "FLOAT"},
            {typeof(DateTime), "DATETIME"},
            {typeof(String), "VARCHAR ([MaxLength])"}
        };

        public static string GenerateScript(DataTable dt)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine(string.Format("IF OBJECT_ID(N'dbo.{0}', N'U') IS NOT NULL DROP TABLE dbo.{0};", dt.TableName));
            sbSql.AppendLine(string.Format("CREATE TABLE [{0}] (", dt.TableName));

            string sColDefinition;
            bool isFirst = true;
            foreach (DataColumn col in dt.Columns)
            {
                sColDefinition = string.Format("\t{0}[{1}] {2}", isFirst ? " " : ",", col.ColumnName, dataTypes[col.DataType]).Replace("[MaxLength]", col.MaxLength == -1 ? "MAX" : col.MaxLength.ToString());
                if (col.ExtendedProperties.Contains("N_Precision")) sColDefinition = sColDefinition.Replace("[Precision]", col.ExtendedProperties["N_Precision"].ToString());
                if (dt.PrimaryKey.Contains(col) && dt.PrimaryKey.Length == 1)
                    sColDefinition += " PRIMARY KEY";
                sbSql.AppendLine(sColDefinition);
                isFirst = false;
            }
            if ( dt.PrimaryKey.Length > 1)
            { 
                string line = "CONSTRAINT PK_"+dt.TableName+" PRIMARY KEY (";
                foreach (var key in dt.PrimaryKey)
                    line += key.ColumnName+",";
                line = line.Remove(line.Length - 1, 1) + ")";
                sbSql.Append(line);
            }
            sbSql.AppendLine(");");
            return sbSql.ToString();
        }

        public static bool CreateTable(DataTable dataTable, SQLParameters sqlParameters)
        {
            try
            {
                string sSQL = GenerateScript(dataTable);

                if (BeginSomeStep != null)
                    BeginSomeStep(className, new dbfCoreEventArgs(sSQL, ""));
                //Console.WriteLine(sSQL);

                using (SqlConnection sqlConnection = new SqlConnection(sqlParameters.sqlConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        //cmd.Transaction = sqlParameters.sqlTransaction;
                        cmd.CommandText = sSQL;
                        cmd.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {

                if (BeginSomeError != null)
                    BeginSomeError(className, new dbfCoreEventArgs(ex.ToString(), ""));
                //Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static int BulkCopyToSQL(DataTable dataTable, SQLParameters sqlParameters, string targetTable = "")
        {
            if (string.IsNullOrEmpty(targetTable)) targetTable = dataTable.TableName;

            int cnt = 0;

            SqlTransaction sqlTransaction = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlParameters.sqlConnectionString))
                {
                    sqlConnection.Open();
                    using (sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction))
                        {
                            bulkCopy.DestinationTableName = targetTable;
                            bulkCopy.BatchSize = sqlParameters.sqlBulkCopyBatchSize;
                            bulkCopy.WriteToServer(dataTable);
                            sqlTransaction.Commit();
                        }

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            //cmd.Transaction = sqlTransaction;
                            cmd.CommandText = string.Format("SELECT COUNT(*) FROM {0}", targetTable);
                            cnt = (int)cmd.ExecuteScalar();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                throw;
            }

            return cnt;
        }

        public static string GenerateSelectScript(string tableName, int rowsToSelect, int batchNumber)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine(string.Format("SELECT * FROM {0} WHERE RowNumber BETWEEN {1} AND {2}", tableName, (batchNumber * rowsToSelect).ToString(), (((batchNumber+1) * rowsToSelect) -1).ToString()));
            return sbSql.ToString();
        }

        public static string GenerateScriptCount(string tableName)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine(string.Format("SELECT COUNT(*) FROM {0}", tableName));
            return sbSql.ToString();
        }
        public static int GetRowCountFromTable(SQLParameters sqlParameters)
        {
            try
            {
                string sSQL = GenerateScriptCount(sqlParameters.sqlTableName);


                if (BeginSomeStep != null)
                    BeginSomeStep(className, new dbfCoreEventArgs(sSQL, ""));
                //Console.WriteLine(sSQL);
                int result;
                using (SqlConnection sqlConnection = new SqlConnection(sqlParameters.sqlConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = sSQL;
                        result = (int)cmd.ExecuteScalar();
                    }
                    sqlConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {

                if (BeginSomeError != null)
                    BeginSomeError(className, new dbfCoreEventArgs(ex.ToString(), ""));
                //Console.WriteLine(ex.ToString());
                return 0;
            }
        }
        public static DataTable GetDataFromTable(SQLParameters sqlParameters, int batchNumber=0)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string sSQL = GenerateSelectScript(sqlParameters.sqlTableName, sqlParameters.rowsToSelect, batchNumber);

                if (BeginSomeStep != null)
                    BeginSomeStep(className, new dbfCoreEventArgs(sSQL, ""));
                //Console.WriteLine(sSQL);

                using (SqlConnection sqlConnection = new SqlConnection(sqlParameters.sqlConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = sSQL;
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataTable);
                        dataAdapter.Dispose();
                    }
                    sqlConnection.Close();
                }
                return dataTable;
            }
            catch (Exception ex)
            {

                if (BeginSomeError != null)
                    BeginSomeError(className, new dbfCoreEventArgs(ex.ToString(), ""));
                //Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}

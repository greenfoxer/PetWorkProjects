using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using dbfCoreDLL.dbfHelper;
using dbfCoreDLL.sqlHelper;
using System.Configuration;

namespace MarkerMaker
{
    class Program
    {
        static Logger Logger = LogManager.GetCurrentClassLogger();
        class parameters
        {
            public string shared_directory = Properties.Settings.Default.SharedDir;
            public string extracting_directory;// = Properties.Settings.Default.ExtractedDir;
            public string extension = Properties.Settings.Default.Extension;
            public string SQLdbConnectionstring;// = Properties.Settings.Default.ConnectionString;
            public string destination;
            public string extract_destination;
            public string ended_dir;
            public char separator;
            public char antiseparator;
            public SqlConnection connection;
            public SqlCommand sqlcmd;
            //public DataTable table;
            //public SqlBulkCopy bulk;
            public SQLParameters sqlParameters;

            public parameters(char ch)
            {
                //log = new logger();
                var settings = new System.Collections.Specialized.NameValueCollection();
                try
                {
                    settings = System.Configuration.ConfigurationManager.GetSection(ch.ToString()) as System.Collections.Specialized.NameValueCollection;
                    SQLdbConnectionstring = settings["ConnectionString"];
                    extracting_directory= settings["ExtractedDir"];
                    connection = new SqlConnection(SQLdbConnectionstring);
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = connection;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Logger.Error(e);
                    //log.Add(e.ToString());
                }
                
                sqlParameters = new SQLParameters();
                sqlParameters.sqlBulkCopyBatchSize = Properties.Settings.Default.SqlBulkCopyBatchSize;
                sqlParameters.sqlConnectionString = SQLdbConnectionstring;

                destination = shared_directory + extracting_directory + extension;
                extract_destination = shared_directory + extracting_directory;
                ended_dir = extract_destination + extracting_directory;

                separator = Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                antiseparator = separator == ',' ? '.' : ',';

            }
        }
        class methods
        {
            parameters param;
            public methods(ref parameters p)
            {
                this.param = p;
            }

            public void Extract()
            {
                try
                {
                    if (Directory.Exists(param.extract_destination))
                        Directory.Delete(param.extract_destination, true);

                    Directory.CreateDirectory(param.extract_destination);

                    System.IO.Compression.ZipFile.ExtractToDirectory(param.destination, param.extract_destination, Encoding.GetEncoding(866));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //param.log.Add(e.Message + "\n" + e.ToString());
                    Logger.Error(e);
                }
            }
            public void RenameDir()
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(param.extract_destination);
                    DirectoryInfo renamedir = dir.GetDirectories()[0];
                    Directory.Move(renamedir.FullName, param.ended_dir);
                }
                catch (Exception e)
                {
                    //param.log.Add(e.Message + "\n" + e.ToString());
                    Logger.Error(e);
                    Console.WriteLine(e.Message);
                }
            }

            public void ExecSQL(string comm)
            {
                param.sqlcmd.CommandText = comm;
                try
                {
                    param.sqlcmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    //param.log.Add(e.Message + "\n" + e.ToString());
                    Logger.Error(e);
                    Console.WriteLine(e.Message);
                }
            }
            //void BulkWriter(string dbName)
            //{
            //    try
            //    {
            //        param.bulk = new SqlBulkCopy(param.connection);
            //        param.bulk.BatchSize = 100000;
            //        //param.bulk.BulkCopyTimeout = 45;
            //        param.bulk.DestinationTableName = "[" + dbName + "]";
            //        param.bulk.WriteToServer(param.table);
            //        param.bulk.Close();
            //    }
            //    catch (Exception e)
            //    {
            //        //param.log.Add(e.Message + "\n" + e.ToString());
            //        Logger.Error(e);
            //        Console.WriteLine(e.Message);
            //    }
            //}
            public void PewPewDBFs()
            {
                try
                {
                    DirectoryInfo dirInfoZip = new DirectoryInfo(param.ended_dir);
                    List<string> files = new List<string>(Directory.GetFiles(dirInfoZip.ToString(), "*.DBF", SearchOption.TopDirectoryOnly)
                                                            .Where(t => !t.Contains("MET"))
                                                            .Where(t => !t.Contains("$")));
                    foreach (string file in files)
                    {
                        //Console.WriteLine(file);
                        dbfCoreDLL.dbfHelper.Reader.dataTableMaxRows = 1000000;
                        dbfCoreDLL.dbfHelper.Reader.sqlParameters = param.sqlParameters;
                        dbfCoreDLL.dbfHelper.Reader.ReadDBF(file);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    Console.WriteLine(e.Message);
                }
            }
        }

        static public void StartReplication(char subject)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Logger.Info("Start replication DBF-files to SQL Server.");
            parameters param = new parameters(subject);
            methods meth = new methods(ref param);
            try
            {
                if (File.Exists(param.destination))
                {
                    meth.Extract();
                    meth.RenameDir();
                    meth.PewPewDBFs();
                }
            }

            catch (Exception e)
            {
                //param.log.Add(e.ToString());
                Logger.Error(e);
            }
            watch.Stop();
            var ellapsed = watch.Elapsed;
            Console.WriteLine("Replication ended. Ellapsed = " + ellapsed.TotalMinutes + " min.");
            Logger.Info("Replication ended. Ellapsed = " + ellapsed.TotalMinutes + " min.");
        }
        static void Main(string[] args)
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //############### INITIALISATION ###############//
            //Зададим параметры логгирования для SQLHelper, Writer, Reader
            SQLHelper.BeginSomeError += PrintError;//Делегат для определения формы логирования
            SQLHelper.BeginSomeStep += PrintLog;//Делегат для определения формы логирования
            SQLHelper.BeginSomeIteration += PrintLog;//Делегат для определения формы логирования
            Reader.BeginSomeError += PrintError;//Делегат для определения формы логирования
            Reader.BeginSomeStep += PrintLog;//Делегат для определения формы логирования
            Reader.BeginSomeIteration += PrintNotLineLog;//Делегат для определения формы логирования
            Writer.BeginSomeStep += PrintLog;//Делегат для определения формы логирования
            //###############INITIALISATION###############//

            Console.WriteLine("Start replication DBF-files to SQL Server.");

            StartReplication('M');
            StartReplication('P');

            #region old_ver
            //Logger.Info("Start replication DBF-files to SQL Server.");
            //parameters param = new parameters('P');
            //methods meth = new methods(ref param);
            //try
            //{
            //    if (File.Exists(param.destination))
            //    {
            //        meth.Extract();
            //        meth.RenameDir();
            //        meth.PewPewDBFs();
            //    }
            //}

            //catch (Exception e)
            //{
            //    //param.log.Add(e.ToString());
            //    Logger.Error(e);
            //}
            //watch.Stop();
            //var ellapsed = watch.Elapsed;
            //Console.WriteLine("Replication ended. Ellapsed = " + ellapsed.TotalMinutes + " min.");
            //Logger.Info("Replication ended. Ellapsed = " + ellapsed.TotalMinutes + " min.");
            //Console.ReadKey();
            #endregion

        }
        //############## dbfCore logging delegates ##############//

        static void PrintLog(string o, dbfCoreEventArgs e)//Экземпляр делегата для логирования
        {
            Logger.Info(o + ": " + e.message);
        }
        static void PrintError(string o, dbfCoreEventArgs e)//Экземпляр делегата для логирования
        {
            Logger.Error(o + ": " + e.message);
        }
        static void PrintNotLineLog(string o, dbfCoreEventArgs e)//Экземпляр делегата для логирования
        {
            Logger.Info(o + ": " + e.message);
        }
    }
}

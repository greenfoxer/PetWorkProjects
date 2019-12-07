using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using dbfCoreDLL.dbfHelper;
using dbfCoreDLL.sqlHelper;

namespace dbfCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Зададим параметры логгирования для SQLHelper, Writer, Reader
            SQLHelper.BeginSomeError += PrintLog;//Делегат для определения формы логирования
            SQLHelper.BeginSomeStep += PrintLog;//Делегат для определения формы логирования
            SQLHelper.BeginSomeIteration += PrintLog;//Делегат для определения формы логирования
            Reader.BeginSomeError += PrintLog;//Делегат для определения формы логирования
            Reader.BeginSomeStep += PrintLog;//Делегат для определения формы логирования
            Reader.BeginSomeIteration += PrintNotLineLog;//Делегат для определения формы логирования
            Writer.BeginSomeStep += PrintLog;//Делегат для определения формы логирования

            //Установим параметры SQL
            SQLParameters sqlParameters = new SQLParameters();
            sqlParameters.sqlBulkCopyBatchSize = 50000;
            sqlParameters.sqlConnectionString = @"Data Source=Servertest01;Initial Catalog=test_DB_for_MRK;Integrated Security=True"; 


            //////ИМПОРТ В SQL
            string[] files = Directory.GetFiles(@"c:\SKLAD\", "*.DBF", SearchOption.TopDirectoryOnly);
            try
            {
                foreach (string file in files)
                {
                    int i;
                    if (file == "PLAN")
                        i = 0;
                    dbfCoreDLL.dbfHelper.Reader.dataTableMaxRows = 1000000;
                    dbfCoreDLL.dbfHelper.Reader.sqlParameters = sqlParameters;
                    try
                    {
                        dbfCoreDLL.dbfHelper.Reader.ReadDBF(file);
                    }
                    catch
                    {
                        i = 0;
                    }
                }
            }
            catch
            {
                throw;
            }
            //dbfCoreDLL.dbfHelper.Reader.dataTableMaxRows = 1000000;
            //dbfCoreDLL.dbfHelper.Reader.sqlParameters = sqlParameters;
            //dbfCoreDLL.dbfHelper.Reader.ReadDBF(@"U:\OPPO\DOS\PEO\PLAN\2017\171103\PLAN\PLAN.dbf");

            ////ЗАПИСЬ В DBF
            //sqlParameters.sqlTableName = "dbo.NA";
            //string templateFile = @"c:\WORK\dbf\NA1.DBF";
            //string outputFile = @"c:\WORK\dbf\NA_output.DBF";
            //dbfCoreDLL.dbfHelper.Writer.sqlParameters = sqlParameters;
            //dbfCoreDLL.dbfHelper.Writer.WriteDBF(templateFile, outputFile);
        }

        static void PrintLog(string o, dbfCoreEventArgs e)//Экземпляр делегата для логирования
        {
            Console.WriteLine(o +": "+e.message);
        }
        static void PrintNotLineLog(string o, dbfCoreEventArgs e)//Экземпляр делегата для логирования
        {
            Console.Write(o + ": " + e.message);
        }
    }
}

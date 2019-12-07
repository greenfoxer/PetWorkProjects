using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dbfCoreDLL.dbfStructures;
using dbfCoreDLL.sqlHelper;

namespace dbfCoreDLL.dbfHelper
{
    public static class Writer
    {
        /// /////////////////////////////////////////////////////////////////////////////////////
        public static event dbfCoreEventHelper BeginSomeStep;
        public static event dbfCoreEventHelper BeginSomeIteration;
        public static event dbfCoreEventHelper BeginSomeError;
        public static string className = "Writer";
        //private void CallEvent(dbfCoreEventArgs e, dbfCoreEventHelper handler)
        //{
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        //private   void OnSomeStep(dbfCoreEventArgs e) { CallEvent(e, BeginSomeStep); }
        //private   void OnSomeIteration(dbfCoreEventArgs e) { CallEvent(e, BeginSomeIteration); }
        //private   void OnSomeError(dbfCoreEventArgs e) { CallEvent(e, BeginSomeError); }

        /////////////////////////////////////////////////////////////////////////////////////////
        public static bool readDeleted = true;
        public static Encoding encoding = Encoding.GetEncoding(866);
        public static SQLParameters sqlParameters = null;
        public static int dataTableMaxRows = 0;
        // Read an entire standard DBF file into a DataTable
        public static DataTable WriteDBF(string dbfFileTemplate, string outputFile)
        {
            // If there isn't even a file, just return an empty DataTable
            if (!File.Exists(dbfFileTemplate)) return new DataTable();
            string tableName = Path.GetFileNameWithoutExtension(dbfFileTemplate);
            return WriteDBF(File.OpenRead(dbfFileTemplate), tableName, outputFile);
        }
        static void RecreateFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Create(path).Dispose();
        }
        static void WriteToFile(byte[] buffer, string path)
        {
            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        static void WriteToFile(string buffer, string path, int isNewLine = 0)
        {
            if (isNewLine == 0)
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.Write(buffer);
                    tw.Close();
                }
            }
            else
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(buffer);
                    tw.Close();
                }
            }
        }
        // Read an entire standard DBF stream into a DataTable
        public static DataTable WriteDBF(Stream dbfStream, string tableName, string path)
        {
            if(BeginSomeStep!=null)
                BeginSomeStep(className, new dbfCoreEventArgs("Обработка таблицы " + tableName + "...", ""));
            //Console.WriteLine("Обработка таблицы {0}...", tableName);

            DataTable dt = new DataTable();
            BinaryReader recReader;
            DataRow row;
            int fieldIndex;
            RecreateFile(path);

            dt.TableName = tableName;

            BinaryReader dbfReader = null;

            try
            {
                //br = new BinaryReader(new FileStream(dbfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                dbfReader = new BinaryReader(dbfStream);
                // Read the header into a buffer
                byte[] buffer = dbfReader.ReadBytes(Marshal.SizeOf(typeof(DBFHeader)));
                
                int tableRowNum =SQLHelper.GetRowCountFromTable(sqlParameters);
                byte[] rowNum = BitConverter.GetBytes(tableRowNum);
                for (int i = 0; i < rowNum.Length;i++ )
                    buffer[4+i] = rowNum[i];
                WriteToFile(buffer, path);
                
                // Marshall the header into a DBFHeader structure
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                DBFHeader header = (DBFHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(DBFHeader));
                handle.Free();

                // Read in all the field descriptors. Per the spec, 13 (0D) marks the end of the field descriptors
                ArrayList fields = new ArrayList();

                while ((13 != dbfReader.PeekChar()))
                {
                    buffer = dbfReader.ReadBytes(Marshal.SizeOf(typeof(FieldDescriptor)));
                    handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    fields.Add((FieldDescriptor)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(FieldDescriptor)));
                    handle.Free();
                    WriteToFile(buffer, path);

                    if (fields.Count > Int16.MaxValue) return null;
                }

                //Вставим символ перехода на новую строку(к данным DBF)
                buffer = dbfReader.ReadBytes(1);
                WriteToFile(buffer, path);
                buffer = dbfReader.ReadBytes(1);

                
                int iteration = (tableRowNum / sqlParameters.rowsToSelect) + 1;
                using (var stream = new FileStream(path, FileMode.Append))
                {
                    for (int i = 0; i < iteration; i++)
                    {

                        //Получим набор данных, который будем заносить в DBF
                        DataTable dataToWrite = SQLHelper.GetDataFromTable(sqlParameters, i);

                        //Создаем строки для запими в DBF
                        foreach (DataRow r in dataToWrite.Rows)
                        {
                            int position = 1;
                            StringBuilder result = new StringBuilder();
                            result.Append((bool)r["IsDeleted"] ? "*" : " ", result.Length, 1);
                            foreach (FieldDescriptor c in fields)
                            {
                                StringBuilder curVal = new StringBuilder(r[c.fieldName].ToString());
                                if (c.fieldType == dBaseType.D)
                                    curVal = dBaseConverter.DateTime_ToD(curVal);
                                if (curVal.Length < c.fieldLen)
                                    curVal.Insert(curVal.Length, " ", c.fieldLen - curVal.Length);
                                if (curVal.Length > c.fieldLen)
                                    curVal.Remove(c.fieldLen, curVal.Length - c.fieldLen);
                                result.Append(curVal);
                                position += c.fieldLen;
                                //Console.WriteLine(r[c.fieldName] + "------" + c.fieldLen.ToString());
                            }
                            if (result.Length == header.recordLen)
                            {
                                //WriteToFile(Encoding.GetEncoding(866).GetBytes(result.ToString()), path);
                                stream.Write(Encoding.GetEncoding(866).GetBytes(result.ToString()), 0, result.Length);
                            }
                            else
                                Console.WriteLine("wr");
                        }

                        dataToWrite.Dispose();
                    }
                    stream.Close();
                }
                WriteToFile(buffer, path);
            }
      
            catch
            {

                throw;
            }
            finally
            {
                if (dbfReader != null)
                {
                    dbfReader.Close();
                    dbfReader = null;
                }
            }
            return null;
        }
    }
}
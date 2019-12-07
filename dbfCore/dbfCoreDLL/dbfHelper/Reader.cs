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
using dbfCoreDLL.dbfStructures;
using dbfCoreDLL.sqlHelper;
using System.Data.SqlClient;

namespace dbfCoreDLL.dbfHelper
{
    public static class Reader
    {

        public static event dbfCoreEventHelper BeginSomeStep;
        public static event dbfCoreEventHelper BeginSomeIteration;
        public static event dbfCoreEventHelper BeginSomeError;
        public static string className = "Reader";
        #region DBF-Read-Funtions

        public static bool readDeleted = true;
        public static Encoding encoding = Encoding.GetEncoding(866);
        public static SQLParameters sqlParameters = null;
        public static int dataTableMaxRows = 0;

        // Read an entire standard DBF file into a DataTable
        public static DataTable ReadDBF(string dbfFile)
        {
            // If there isn't even a file, just return an empty DataTable
            if (!File.Exists(dbfFile)) return new DataTable();
            string tableName = Path.GetFileNameWithoutExtension(dbfFile);
            //if (tableName == "NZ") //comment 14012019
            //{
            OpenMemoFile(dbfFile);
            return ReadDBF(File.OpenRead(dbfFile), tableName);
            //}
            //return null;
        }

        // Read an entire standard DBF stream into a DataTable
        public static DataTable ReadDBF(Stream dbfStream, string tableName)
        {
            if (BeginSomeStep != null)
                BeginSomeStep(className, new dbfCoreEventArgs("Обработка таблицы " + tableName + "...", ""));
            //Console.WriteLine("Обработка таблицы {0}...", tableName);
            
            DataTable dt = new DataTable();
            BinaryReader recReader;
            DataRow row;
            int fieldIndex;

            dt.TableName = tableName;

            BinaryReader dbfReader = null;

            try
            {
                //br = new BinaryReader(new FileStream(dbfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                dbfReader = new BinaryReader(dbfStream);

                // Read the header into a buffer
                byte[] buffer = dbfReader.ReadBytes(Marshal.SizeOf(typeof(DBFHeader)));

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

                    if (fields.Count > Int16.MaxValue) return null;
                }

                // Read in the first row of records, we need this to help determine column types below
                (dbfReader.BaseStream).Seek(header.headerLen + 1, SeekOrigin.Begin);
                buffer = dbfReader.ReadBytes(header.recordLen);
                recReader = new BinaryReader(new MemoryStream(buffer));

                // Create the columns in our new DataTable
                DataColumn col = null;

                if (readDeleted) dt.Columns.Add(new DataColumn("ISDELETE", typeof(bool)));

                col = new DataColumn("IDENTIFIER", typeof(long));
                col.AutoIncrement = true;
                col.AutoIncrementSeed = 1;
                dt.Columns.Add(col);
                dt.PrimaryKey = new DataColumn[] { dt.Columns["IDENTIFIER"] };

                foreach (FieldDescriptor field in fields)
                {
                    byte[] NumberByteArray = recReader.ReadBytes(field.fieldLen);
                    switch (field.fieldType)
                    {
                        case dBaseType.N:
                            if (dBaseConverter.N_IsDecimal(NumberByteArray))
                            {
                                col = new DataColumn(field.fieldName, typeof(decimal));
                                col.ExtendedProperties.Add("N_Precision", field.decimalCount);
                            }
                            else
                            {
                                col = new DataColumn(field.fieldName, typeof(int));
                            }
                            break;
                        case dBaseType.C:
                            col = new DataColumn(field.fieldName, typeof(string));
                            col.MaxLength = field.fieldLen;
                            break;
                        case dBaseType.T:
                            col = new DataColumn(field.fieldName, typeof(DateTime));
                            break;
                        case dBaseType.D:
                            col = new DataColumn(field.fieldName, typeof(DateTime));
                            break;
                        case dBaseType.L:
                            col = new DataColumn(field.fieldName, typeof(bool));
                            break;
                        case dBaseType.F:
                            col = new DataColumn(field.fieldName, typeof(Double));
                            break;
                        case dBaseType.M: // MEMO
                            //col = new DataColumn(field.fieldName, typeof(byte[]));
                            col = new DataColumn(field.fieldName, typeof(string));
                            break;
                    }
                    int i = 0;
                    var name = col.ColumnName;
                    while (dt.Columns.Contains(col.ColumnName))
                    {
                        i++;
                        col.ColumnName = string.Format("{0}_{1}", name, i);
                    }
                    dt.Columns.Add(col);
                }

                // Skip past the end of the header. 
                (dbfReader.BaseStream).Seek(header.headerLen, SeekOrigin.Begin);

       
                if (sqlParameters != null)
                {
                    SQLHelper.CreateTable(dt, sqlParameters);
                }
                

                int totalRowsCount = 0;
                bool readOnlyOneBlock = tableName.ToLower() == "n_shk";

                // Read in all the records
                for (int counter = 0; counter < header.numRecords; counter++)
                {
                    // First we'll read the entire record into a buffer and then read each field from the buffer
                    // This helps account for any extra space at the end of each record and probably performs better
                    buffer = dbfReader.ReadBytes(header.recordLen);
                    recReader = new BinaryReader(new MemoryStream(buffer));

                    // All dbf field records begin with a deleted flag field. Deleted - 0x2A (asterisk) else 0x20 (space)
                    bool isDeleted = recReader.ReadChar() == '*'; // Читаем первый байт записи, если "*" - запись удалена
                    if (isDeleted && !readDeleted) continue;

                    // Loop through each field in a record
                    row = dt.NewRow();
                    if (readDeleted)
                    {
                        row[0] = isDeleted;
                        fieldIndex = 2;  // Данные начинаются с 3-го поля
                    }
                    else fieldIndex = 1; // Данные начинаются со 2-го поля, отсутствует колонка с признаком удаления

                    foreach (FieldDescriptor field in fields)
                    {
                        switch (field.fieldType)
                        {
                            case dBaseType.N:  // Number
                                byte[] NumberBytes = recReader.ReadBytes(field.fieldLen);
                                if (dBaseConverter.N_IsDecimal(NumberBytes))
                                {
                                    row[fieldIndex] = dBaseConverter.N_ToDecimal(NumberBytes);
                                }
                                else
                                {
                                    row[fieldIndex] = dBaseConverter.N_ToInt(NumberBytes);
                                }
                                break;

                            case dBaseType.C: // String
                                row[fieldIndex] = dBaseConverter.C_ToString(recReader.ReadBytes(field.fieldLen), encoding);
                                break;

                            case dBaseType.M: // Memo
                                //row[fieldIndex] = ReadMemoBlock(dBaseConverter.N_ToInt(recReader.ReadBytes(field.fieldLen)));
                                row[fieldIndex] = dBaseConverter.C_ToString(ReadMemoBlock(dBaseConverter.N_ToInt(recReader.ReadBytes(field.fieldLen)), readOnlyOneBlock), encoding);
                                //int n = dBaseConverter.N_ToInt(recReader.ReadBytes(field.fieldLen)); row[fieldIndex] = n.ToString() + ';' + dBaseConverter.C_ToString(ReadMemoBlock(n), encoding);
                                break;

                            case dBaseType.D: // Date (YYYYMMDD)
                                DateTime DTFromFile = dBaseConverter.D_ToDateTime(recReader.ReadBytes(8));
                                if (DTFromFile == DateTime.MinValue)
                                {
                                    row[fieldIndex] = System.DBNull.Value;
                                }
                                else
                                {
                                    row[fieldIndex] = DTFromFile;
                                }
                                break;

                            case dBaseType.T:
                                row[fieldIndex] = dBaseConverter.T_ToDateTime(recReader.ReadBytes(8));
                                break;

                            case dBaseType.L: // Boolean (Y/N)
                                row[fieldIndex] = dBaseConverter.L_ToBool(recReader.ReadByte());
                                break;

                            case dBaseType.F:
                                row[fieldIndex] = dBaseConverter.F_ToDouble(recReader.ReadBytes(field.fieldLen));
                                break;
                        }
                        fieldIndex++;
                    }

                    recReader.Close();
                    dt.Rows.Add(row);

                    if ((dataTableMaxRows > 0 && (counter + 1) % dataTableMaxRows == 0) || counter == header.numRecords - 1)
                    {
                        StringBuilder str = new StringBuilder("\rВыполнено: " + ((counter + 1.0) / header.numRecords * 100.0).ToString() + "% (" + (counter + 1).ToString()+ " из " +header.numRecords+").");
                        if (BeginSomeIteration != null)
                            BeginSomeIteration(className, new dbfCoreEventArgs(str.ToString(), ""));
                        //Console.Write("\rВыполнено: {0}% ({1} из {2}).", (int)((counter + 1.0) / header.numRecords * 100.0), counter + 1, header.numRecords);
                        if (sqlParameters != null)
                        {
                            totalRowsCount = SQLHelper.BulkCopyToSQL(dt, sqlParameters);
                            dt.Rows.Clear();
                        }
                    }
                }

                //Console.WriteLine();
                if (BeginSomeStep != null)
                    BeginSomeStep(className, new dbfCoreEventArgs("Всего записей в DBF: " + header.numRecords, ""));
                //Console.WriteLine("Всего записей в DBF: {0}", header.numRecords);
                if (BeginSomeStep != null)
                    BeginSomeStep(className, new dbfCoreEventArgs("Всего записано в БД: " + totalRowsCount, ""));
                //Console.WriteLine("Всего записано в БД: {0}", totalRowsCount);
                if (BeginSomeStep != null)
                    BeginSomeStep(className, new dbfCoreEventArgs(new string('-', 70), ""));
                //Console.WriteLine(new string('-', 79));
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

                if (fptReader != null)
                {
                    fptReader.Close();
                    fptReader = null;
                }
            }

            //long count = DateTime.Now.Ticks - start;

            return dt;
        }
        #endregion
        #region FPT (Memo) Functions
        private static int memoBlockLength = 512;
        private static int nextBlockID = 0;
        private static BinaryReader fptReader = null;
        private static void OpenMemoFile(string dbfFile, string memoFileExt = "FPT")
        {
            string fptFile = Path.Combine(Path.GetDirectoryName(dbfFile), Path.GetFileNameWithoutExtension(dbfFile) + "." + memoFileExt);

            if (File.Exists(fptFile))
            {
                fptReader = null;
                try
                {
                    // fptReader = new BinaryReader(new FileStream(dbtFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                    // fptReader = new BinaryReader(ZipHelper.GetReadStream(zipfile, dbtFile));
                    fptReader = new BinaryReader(File.OpenRead(fptFile));

                    // Read the header into a buffer
                    byte[] buffer = fptReader.ReadBytes(Marshal.SizeOf(typeof(FPTHeader)));

                    // Marshall the header into a DBTHeader structure
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    FPTHeader header = (FPTHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(FPTHeader));
                    handle.Free();

                    memoBlockLength = header.blockLength;
                    nextBlockID = header.nextBlockID;
                }
                catch (Exception)
                {
                }
            }
        }

        private static byte[] ReadMemoBlock(int recordnumber, bool readOnlyOneBlock = false)
        {
            if (recordnumber == 0 || fptReader == null || (nextBlockID > 0 && recordnumber >= nextBlockID))
                return new byte[0];

            // Position reader at beginning of current block
            fptReader.BaseStream.Position = memoBlockLength * recordnumber;

            // Read the memo field header into a buffer
            byte[] buffer = fptReader.ReadBytes(Marshal.SizeOf(typeof(MemoHeader)));

            Int32 bytesToRead = 0;

            if (readOnlyOneBlock)
            {
                bytesToRead = memoBlockLength - 8;
            }
            else
            {
                // Marshall the header into a MemoHeader structure
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                MemoHeader memHeader = (MemoHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(MemoHeader));
                handle.Free();

                if (!memHeader.correctHeader) return new byte[0]; // как оказалось не везде так...

                bytesToRead = memHeader.recordLength;
            }

            return fptReader.ReadBytes(bytesToRead);
        }

        private static byte[] ReadMemoBlock0(int recordnumber)
        {
            byte[] blockSize = new byte[4];
            fptReader.BaseStream.Seek(recordnumber * memoBlockLength, SeekOrigin.Begin);
            fptReader.BaseStream.Seek(4, SeekOrigin.Current); // advance type
            fptReader.BaseStream.Read(blockSize, 0, 4);
            int memoSize = BitConverter.ToInt32(blockSize.Reverse().ToArray(), 0);
            byte[] bytes = new byte[memoSize];
            fptReader.BaseStream.Read(bytes, 0, memoSize);
            //return Encoding.ASCII.GetString(bytes);
            return bytes;
        }

        #endregion
    }
}

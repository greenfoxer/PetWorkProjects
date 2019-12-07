using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace dll_file_saver
{
    public class file_saver
    {
        public static void save_file(string path, string name, byte[] data)
        {

            using (System.IO.FileStream fs = new System.IO.FileStream(path+Path.DirectorySeparatorChar+name, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
            {
                using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                {
                    bw.Write(data);
                    bw.Close();
                }
            }
        }
        public static void delete_file(string path)
        {
            File.Delete(path);
        }

        public static void delete_all_files(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
        }

    }
}

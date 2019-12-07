using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static string address = @"http://127.0.0.1/input/";
        static int port = 8008;
        static void Main(string[] args)
        {
            string sendingFilePath = @"c:\work\phones.xml";
            using(var client = new HttpClient())
            using(var formData = new MultipartFormDataContent())
            using (var fileStream = File.OpenRead(sendingFilePath))
            {
                HttpContent fileStreamContent = new StreamContent(fileStream);
                formData.Add(fileStreamContent, "xml");
                var response = client.PostAsync(address, formData).Result;
                byte[] contents = response.Content.ReadAsByteArrayAsync().Result;
                string resp = Encoding.Unicode.GetString(contents);
                Console.WriteLine(response.Headers);
                Console.WriteLine(resp);
            }

        }
    }
}

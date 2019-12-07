using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace HTTPClient
{
    class Program
    {
        static string address = @"http://127.0.0.1/input/";
        static int port = 8008;
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(address);
            listener.Start();
            Console.WriteLine("Waiting for received messages...");
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                Console.WriteLine("New {0} request", request.HttpMethod);
                string text;
                using (var reader = new StreamReader(request.InputStream,
                                                     request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }
                string responseStr = "<html><head><meta charset='utf8'></head><body>Hello. Ur request type is "+request.HttpMethod;
                if (!string.IsNullOrEmpty(text))
                {
                    responseStr = responseStr + "<b>Data u received: " + text;
                    Console.WriteLine("Received data: " + text);
                }
                responseStr = responseStr + "</body></html>";
                byte[] buffer = Encoding.Unicode.GetBytes(responseStr);

                response.ContentLength64 = buffer.Length;
                response.StatusCode = 200;
                response.KeepAlive = false;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            listener.Stop();

        }
    }
}

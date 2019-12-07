using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "client";
            Uri address = new Uri("http://localhost:55555/IContract");
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);
            ChannelFactory<IContract> factory = new ChannelFactory<IContract>(binding,endpoint);
            IContract channel = factory.CreateChannel();
            string st = "";
            string answer;
            while (st != "end")
            {
                Console.Write("me>");
                st = Console.ReadLine();
                
                try { Console.WriteLine(channel.Say(st)); }
                catch { Console.WriteLine("something wrong!"); }
            }
        }
    }
}

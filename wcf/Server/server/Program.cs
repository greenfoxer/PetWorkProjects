using System;
using System.ServiceModel;
using System.Collections.ObjectModel;


namespace server
{
    class Program
    {
        public static ObservableCollection<string> users;

        static void Main(string[] args)
        {
            Console.Title = "server";
            Uri address = new Uri("http://localhost:55555/IContract");
            BasicHttpBinding binding = new BasicHttpBinding();
            Type contract = typeof(IContract);
            ServiceHost host = new ServiceHost(typeof(Service));
            host.AddServiceEndpoint(contract, binding, address);
            host.Open();
            Console.WriteLine("Waiting...");
            Console.ReadKey();
            host.Close();
        }
    }
}

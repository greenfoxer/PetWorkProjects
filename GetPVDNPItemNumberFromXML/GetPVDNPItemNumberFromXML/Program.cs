using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;

namespace GetPVDNPItemNumberFromXML
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"c:\WORK\080819\3537MTG.xml";
            if (args.Length == 1)
                file = args[0];
            else
                return;

            var document = XDocument.Load(file);
            XNamespace ns0 = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ns0_1 = "http://xsd.gspvd/v001/personalization/pc/dpc-notifications";
            List<string> itemsNumbers = new List<string>(document.Root.Descendants(ns0 + "Body").Descendants(ns0 + "BoxShipmentNotificationA").Descendants(ns0_1 + "DocumentEntry").Select(p => p.Element(ns0_1 + "DocumentNumber").Value));

            string file_out_name = file.Split('.')[0] +"_"+itemsNumbers.Count.ToString()+ "_out.txt";
            var file_out = new FileInfo(file_out_name);
            StreamWriter writer = file_out.CreateText();
            foreach (string number in itemsNumbers)
                writer.WriteLine(number);
            writer.WriteLine(writer.NewLine);
            foreach (string number in itemsNumbers)
                writer.WriteLine("'"+number+"',");
            writer.Close();

        }
    }
}

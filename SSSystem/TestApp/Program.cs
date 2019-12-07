using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete;
using Domain.Entities;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EFSIZRepository context = new EFSIZRepository();
            foreach (var i in context.SIZGoodsList)
                Console.WriteLine(i.name+i.t_siz_manufacturer.name);
        }
    }
}

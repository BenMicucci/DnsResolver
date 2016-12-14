using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDnsResolver
{
    public class Program
    {
        static void Main(string[] args)
        {
            string domain = "test.com";
            DnsResolver.DnsResolver dnsResolver = new DnsResolver.DnsResolver();

            var txRecords = dnsResolver.GetTxtRecords(domain);
            var mxRecords = dnsResolver.GetMxRecords(domain);
            var nsRecords = dnsResolver.GetNsRecords(domain);

            Console.WriteLine("Done. Please hit [Enter] to exit.");
            Console.ReadLine();

        }
    }
}

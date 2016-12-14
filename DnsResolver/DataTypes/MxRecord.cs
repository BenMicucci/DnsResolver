using System;
using System.Net;

namespace DnsResolver
{
	public class MxRecord
	{
        public string Domain { get; set; }
        public string NameExchange { get; set; }
		public int Preference { get; set; }        
	}
}

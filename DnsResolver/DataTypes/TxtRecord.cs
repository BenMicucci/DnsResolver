using System;
using System.Net;

namespace DnsResolver
{
	public class TxtRecord
	{
        public string Domain { get; set; }
        public string NameAdministrator { get; set; }
		public string Strings { get; set; }
        public bool HasSPF { get; set; }
        public bool HasDMARC { get; set; }
        public bool HasMS { get; set; }	
	}
}

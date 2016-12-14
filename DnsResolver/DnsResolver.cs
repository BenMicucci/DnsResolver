using System;
using System.Collections.Generic;
using System.Management.Automation; //C:\Program Files (x86)\Reference Assemblies\Microsoft\WindowsPowerShell\3.0
using System.Collections.ObjectModel;

namespace DnsResolver
{
    public class DnsResolver
    {
        private const string DnsServerIP = "172.22.4.197";

        public DnsResolver()
        {

        }

        public List<TxtRecord> GetTxtRecords(string domain)
        {
            var txtRecords = new List<TxtRecord>();

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript("param($domain, $dnsServer) Resolve-DnsName $domain -type TXT -Server $dnsServer -ErrorAction Stop | where { $_.Name -eq $domain }");
                PowerShellInstance.AddParameter("domain", domain);
                PowerShellInstance.AddParameter("dnsServer", DnsServerIP);

                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                if (PowerShellInstance.Streams.Error.Count == 0)
                {
                    foreach (PSObject outputItem in PSOutput)
                    {
                        if (outputItem != null)
                        {
                            var txtRecord = new TxtRecord();
                            txtRecord.Domain = domain;

                            if (outputItem.Members.Match("Strings").Count > 0)
                            {
                                txtRecord.Strings = (string)((string[])outputItem.Members["Strings"].Value).GetValue(0);
                                if (txtRecord.Strings.Contains("MS="))
                                {
                                    txtRecord.HasMS = true;
                                }
                                if (txtRecord.Strings.Contains("v=DMARC"))
                                {
                                    txtRecord.HasDMARC = true;
                                }
                                if (txtRecord.Strings.Contains("v=spf"))
                                {
                                    txtRecord.HasSPF = true;
                                }
                            }
                            if (outputItem.Members.Match("NameAdministrator").Count > 0)
                            {
                                txtRecord.NameAdministrator = outputItem.Members["NameAdministrator"].ToString();
                            }
                            txtRecords.Add(txtRecord);
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("{0} does not has any TXT Records", domain));
                }
                return txtRecords;
            }
        }

        public List<MxRecord> GetMxRecords(string domain)
        {
            var mxRecords = new List<MxRecord>();

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript("param($domain, $dnsServer) Resolve-DnsName $domain -type MX -Server $dnsServer -ErrorAction Stop | where { $_.Name -eq $domain }");
                PowerShellInstance.AddParameter("domain", domain);
                PowerShellInstance.AddParameter("dnsServer", DnsServerIP);

                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                if (PowerShellInstance.Streams.Error.Count == 0)
                {
                    foreach (PSObject outputItem in PSOutput)
                    {
                        if (outputItem != null)
                        {
                            var mxRecord = new MxRecord();

                            mxRecord.Domain = domain;
                            mxRecord.NameExchange = outputItem.Members["NameExchange"].Value.ToString();
                            mxRecord.Preference = Convert.ToInt16(outputItem.Members["Preference"].Value);

                            mxRecords.Add(mxRecord);
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("{0} does not has any MX Records", domain));
                }
                return mxRecords;
            }
        }

        public List<NsRecord> GetNsRecords(string domain)
        {
            var nsRecords = new List<NsRecord>();

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript("param($domain, $dnsServer) Resolve-DnsName $domain -type NS -Server $dnsServer -ErrorAction Stop | where { $_.Name -eq $domain }");
                PowerShellInstance.AddParameter("domain", domain);
                PowerShellInstance.AddParameter("dnsServer", DnsServerIP);

                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                if (PowerShellInstance.Streams.Error.Count == 0)
                {
                    foreach (PSObject outputItem in PSOutput)
                    {
                        if (outputItem != null)
                        {
                            var nsRecord = new NsRecord();

                            nsRecord.Domain = domain;
                            nsRecord.NameHost = outputItem.Members["NameHost"].Value.ToString();

                            nsRecords.Add(nsRecord);
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("{0} does not has any NS Records", domain));
                }
                return nsRecords;
            }
        }

    }
}


using System;
using System.Collections.Generic;
using Heijden.DNS;
using System.Net;
using System.Diagnostics;
using MingleView;
using Microsoft.Win32;
namespace p2p.dns
{
    
    class P2PDNShlpr
    {
        private const string PNRP_SEED_SERVER = "pnrpv2.ipv6.microsoft.com";
        private const string PNRP_SYNC_HOST_CMD ="p2p pnrp cloud synchronize host ";
        private const string TEREDO_STATUS_CMD = "int ipv6 show teredo ";
        private const string TEREDO_ENTERPRISE_CMD = "int ipv6 set teredo enterpriseclient ";
        private const string TEREDO_MANAGED_STATUS = "client is in a managed";
        private const string PNRP_CLOUD_TYPE = " Global_";
        private const int SYNC_LAUNCH_COUNT = 3;
        static private bool _bSyncCompleted=false;

        /******************88
         * this finds the seed server ipv6 address & sync pnrp names with it
         * ******************/
        public bool SyncWithPeers()
        {
            string[] ipv6Seedaddresses;

            IsIPv6Enabled();

            //already synced no need to try again
            if (_bSyncCompleted)
            {
                return true;
            }

            //check if teredo started.
            fixTeredo();

            //try manullay sending dns udp
            //sync al users to one seed server
            ipv6Seedaddresses = resolveDNSManually(PNRP_SEED_SERVER);
            if (ipv6Seedaddresses == null)
            {
                    return false;
            }
            //try syncing with seed servers one by one
            foreach (string seedAddress in ipv6Seedaddresses)
            {
                _bSyncCompleted = syncWithPNRPServer(seedAddress);
                if (_bSyncCompleted == true)
                    break;
            }

            return _bSyncCompleted;
        }

        /****************************************
         * checks if ipv6 enabled or disabled in this system
         * ***************************************/
        public bool IsIPv6Enabled()
        {
            RegistryKey hklmIPv6State = Registry.LocalMachine;

            //if some antivirus or firewall disable ipv6
            hklmIPv6State = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\Tcpip6\\Parameters\\",false);
            if (hklmIPv6State == null)
            {
                Logger.Trace(Logger.MessageType.Informational, " P2PDNShlpr::IsIPv6Enabled no disable entry ");
  
                return true;
            }
            Object ipv6State = hklmIPv6State.GetValue("DisabledComponents");
            if (ipv6State != null)
            {
                if ((int)ipv6State != 0)
                {
                    Logger.Trace(Logger.MessageType.Informational, " P2PDNShlpr::IsIPv6Enabled " + (ipv6State as string) );
                    return false;
                }
            }
            Logger.Trace(Logger.MessageType.Informational, " P2PDNShlpr::IsIPv6Enabled no disable entry ");
  
            return true;
        }

        /******************
         *launch netsh command to sync pnrp
         * *******************/
        private bool syncWithPNRPServer(string sServerAddr)
        {
            Process netshpnrpsyncCmd = new Process();
            string sCmdLine = PNRP_SYNC_HOST_CMD + sServerAddr + PNRP_CLOUD_TYPE;
            string sCmdOutput;

            netshpnrpsyncCmd.StartInfo.FileName = "netsh.exe";
            netshpnrpsyncCmd.StartInfo.Arguments =  sCmdLine;
            netshpnrpsyncCmd.StartInfo.UseShellExecute = false;
            netshpnrpsyncCmd.StartInfo.CreateNoWindow = true;
            netshpnrpsyncCmd.StartInfo.RedirectStandardOutput = true;

            netshpnrpsyncCmd.Start();
            netshpnrpsyncCmd.WaitForExit();
            sCmdOutput = netshpnrpsyncCmd.StandardOutput.ReadToEnd();
            Logger.Trace(Logger.MessageType.Informational, " P2PDNShlpr::syncWithPNRPServer sync return: " + sCmdOutput);
            return sCmdOutput.Contains("returned"); 
        }
        /**************************
         * it fixes minor error in teredo starting
         * ************************/
        void fixTeredo()
        {
            Process netshteredoStatusCmd = new Process();
            string sCmdLine = TEREDO_STATUS_CMD ;
            string sCmdOutput;


            netshteredoStatusCmd.StartInfo.FileName = "netsh.exe";
            netshteredoStatusCmd.StartInfo.Arguments = sCmdLine;
            netshteredoStatusCmd.StartInfo.UseShellExecute = false;
            netshteredoStatusCmd.StartInfo.CreateNoWindow = true;
            netshteredoStatusCmd.StartInfo.RedirectStandardOutput = true;

            netshteredoStatusCmd.Start();
            netshteredoStatusCmd.WaitForExit();
            sCmdOutput = netshteredoStatusCmd.StandardOutput.ReadToEnd();
            Logger.Trace(Logger.MessageType.Informational, " P2PDNShlpr::fixTeredo status return: " + sCmdOutput);
            if (sCmdOutput.Contains(TEREDO_MANAGED_STATUS ))
            {
                //if managed run it as enterprise client
                Process netshteredoManagedCmd = new Process();
                ProcessStartInfo procInfo = new ProcessStartInfo();
                sCmdLine = TEREDO_ENTERPRISE_CMD;
                
                netshteredoManagedCmd.StartInfo.FileName = "netsh.exe";
                netshteredoManagedCmd.StartInfo.Arguments = sCmdLine;
                netshteredoManagedCmd.StartInfo.UseShellExecute = true;
                netshteredoManagedCmd.StartInfo.CreateNoWindow = true;
                netshteredoManagedCmd.StartInfo.Verb = "runas"; 

                netshteredoManagedCmd.Start();
                netshteredoManagedCmd.WaitForExit();
                Logger.Trace(Logger.MessageType.Informational, " P2PDNShlpr::fixTeredo setting enterprise client return: ");

            }
        }



        /*******************
         * resolve DNS manually
         * ***************/
        private string[] resolveDNSManually(string sHostName)
        {
            Resolver resolver = new Resolver();
            string[] ipv6Addresses=null;
            int iDnsIndex=0;
            resolver.DnsServer = "8.8.8.8";
            resolver.TimeOut = 60000;
            resolver.TransportType = Heijden.DNS.TransportType.Udp;
            Response response = resolver.Query(PNRP_SEED_SERVER, QType.AAAA, QClass.IN);
            Logger.Trace(Logger.MessageType.Informational, " P2PDNShlpr::resolveDNSManually  return: " + response.RecordsAAAA[0].Address.ToString());
            if(response.RecordsAAAA.Length==0)
                                return ipv6Addresses;
            ipv6Addresses = new string[response.RecordsAAAA.Length];
            foreach (RecordAAAA record in response.RecordsAAAA)
                ipv6Addresses[iDnsIndex++] = record.Address.ToString();
            return ipv6Addresses;     
        }

        /***********
         * resolves using system default DNS server
         * **************/
        private string resolveDNSBySystemDNSServer(string sHostName)
        {
            IPAddress[] ips;

            try
            {
                ips = Dns.GetHostAddresses(sHostName);
            }
            catch (System.Net.Sockets.SocketException se)
            {
                Logger.Trace(Logger.MessageType.Failure, "Caught exception in P2PDNShlpr::resolveDNSBySystemDNSServer : " + se.Message);
                return null;
            }

            return ips.GetValue(0).ToString() ;
        }
    }
}

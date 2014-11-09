using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.PeerToPeer;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using MingleView.Firewall;
using MingleView.Properties;
using System.Threading; 
using MingleView;

namespace p2p.dns
{
    class P2PDNS
    {
        private const string LANOI_CERT_NAME = "cf50eb3649588a64e5fbc71f593b5600fb7f95df";
        private System.Net.PeerToPeer.PeerName m_DNSName;
        private PeerNameRegistration m_GlobalNameRegistar;
        //private PeerNameRegistration m_LocalNameRegistar;
        private string _globalIPv6Address=null;
        private string _localIPv6Address=null;
        private const string GLOBAL_IPV6_ADDRESS_PREFIX = "2";
        private const string GLOBAL_IPV6_TEREDO_PREFIX = "2001";
        private const string LOCAL_IPV6_ADDRESS_PREFIX = "fe80";
        const int MAX_PNRP_RESOLVE_TRY = 9;
        private P2PDNShlpr _p2pHlpr= new P2PDNShlpr();
        public P2PDNS()
        {
            P2PNative p2pNative = new P2PNative();
            Firewall allowIPv6 = new Firewall();
            //import the LANoi  pro identity into the system
            p2pNative.PeerImportIdentityXML(MingleView.Properties.Strings.P2PDNS_LANOI_IDENTITY, "Test");

            //add firewall
            allowIPv6.AllowApplicationForIPv6();
        }
        /********************
         * property to store global ipv6 address
         * ******************/
        public string GlobalIPV6Address
        {
            get { return _globalIPv6Address; }
            set { _globalIPv6Address = value; }
        }
        /**********************
         * to get local ipv6 address
         * ************/
        public string LocalIPv6Address
        {
            get { return _localIPv6Address; }
            set { _localIPv6Address = value; }
        }
        /**********************************
         * initialise pnrp network
         * ********************************/
        public void Init()
        {
            //sync with others
            _p2pHlpr.SyncWithPeers();
            setIPv6Address();
        }
        /******
         * this will use the uniquename as the classifier for the LANoi peername
         * cf50eb3649588a64e5fbc71f593b5600fb7f95df.sUniqueDNSName
         * throws mingleview exception
         * ********/
        public bool RegisterDNS(string sUniqueDNSName,byte[] byDNSPayload)
        {
            PeerName LANoiPeerName = new PeerName(MingleView.Properties.Strings.P2PDNS_LANOI_PEER_NAME);


            m_DNSName=PeerName.CreateRelativePeerName(LANoiPeerName,sUniqueDNSName);
            Logger.Trace(Logger.MessageType.Informational, "P2PDNS::RegisterDNS:: " + m_DNSName.Authority + "." + m_DNSName.Classifier); 
            //register globally
            m_GlobalNameRegistar = new PeerNameRegistration();
            m_GlobalNameRegistar.PeerName = m_DNSName;
            m_GlobalNameRegistar.Data = byDNSPayload;
            m_GlobalNameRegistar.Cloud = Cloud.GetCloudByName("Global_");
            try
            {
                m_GlobalNameRegistar.Start();
            }
            catch (PeerToPeerException pe)
            {
                Logger.Trace(Logger.MessageType.Failure, "P2PDNS::RegisterDNS:: " + pe.Message + "." + pe.InnerException.Message);
                throw new MingleViewException(Strings.P2PDNS_RegisterDNS_ErrorMsg + pe.Message + "." + pe.InnerException.Message); 
            }
            return true;
        }
        /***************************
         * this does the reverse of register
         * throws mingleview exception if failed to resolve
         * ************************/
        public byte[] ResolveDNS(string sUniqueDNSName)
        {
            PeerName LANoiPeerName = new PeerName(MingleView.Properties.Strings.P2PDNS_LANOI_PEER_NAME );
            PeerName RelativePeerName;
            PeerNameResolver NameResolver = new PeerNameResolver();
            PeerNameRecordCollection Nameresults=new PeerNameRecordCollection() ;
            Byte[]    byPayload;
            Dictionary<IAsyncResult, NameResolveDelegate> resolveThreadMap = new Dictionary<IAsyncResult, NameResolveDelegate>();
            bool bResolved = false;


            RelativePeerName = PeerName.CreateRelativePeerName(LANoiPeerName, sUniqueDNSName); 
            for (int iThreadCount = 0; iThreadCount < MAX_PNRP_RESOLVE_TRY; iThreadCount++)
            {
                NameResolveDelegate thread= new NameResolveDelegate(resolvePnrp);
                IAsyncResult result = thread.BeginInvoke(RelativePeerName, null, null);
                resolveThreadMap.Add(result,thread);    
            }

            do{
                List<WaitHandle>  resolverThreadHandles = new List<WaitHandle> (
                (from thread in  resolveThreadMap select thread.Key.AsyncWaitHandle).ToList());
            
                int iThreadCompletedIndex = WaitHandle.WaitAny(resolverThreadHandles.ToArray() );
                try
                {
                    Logger.Trace(Logger.MessageType.Informational, "P2PDNS::ResolveDNS:: thread completed = " + iThreadCompletedIndex);    
                    foreach (KeyValuePair<IAsyncResult,NameResolveDelegate>  pair in resolveThreadMap)
                      if (pair.Key.AsyncWaitHandle == resolverThreadHandles[iThreadCompletedIndex]   ) 
                      {
                          Nameresults = pair.Value.EndInvoke(pair.Key);
                          bResolved = (Nameresults.Count > 0);
                          resolveThreadMap.Remove(pair.Key);
                          break;//anyways found the thread which completed
                      }
                 }
                catch(PeerToPeerException pe)
                {
                    Logger.Trace(Logger.MessageType.Failure, "P2PDNS::ResolveDNS:: " + pe.Message + "." + pe.InnerException.Message);
                    throw new MingleViewException(Strings.P2PDNS_ResolveDNS_ErrorMsg + pe.Message + "." + pe.InnerException.Message); 
                }
            } while (resolveThreadMap.Count > 1 && bResolved==false);

            
            //failed to resolve DNS name
            if (bResolved == false)
            {
                Logger.Trace(Logger.MessageType.Failure, "P2PDNS::ResolveDNS:: failed to resolve name ");
                throw new MingleViewException(Strings.P2PDNS_ResolveDNS_ErrorMsg + "peer not found"  ); 
            }
            foreach (PeerNameRecord record in Nameresults)
            {
                if (record.Data != null)
                {
                   byPayload = new byte[record.Data.Length];
                    Array.Copy(record.Data ,byPayload,record.Data.Length);
                    return byPayload;
                }
            }
              
            //thro exception
             return null ; 
            
        }

        private delegate   PeerNameRecordCollection NameResolveDelegate(PeerName peerName);

        /************
        * thread routine to resolve pnrp name
        * *************/
        static PeerNameRecordCollection resolvePnrp(PeerName peerName)
        {
            PeerNameResolver NameResolver = new PeerNameResolver();
            Logger.Trace(Logger.MessageType.Informational, "resolvePnrp" + peerName.PeerHostName);

            return NameResolver.Resolve(peerName, Cloud.GetCloudByName("Global_"));
        }

        /*******************
         * checks whether its a global ipv6 address
         * *******************/
        private bool isGlobalIPv6Address(string sAddress)
        {
            if (sAddress != null &&
                         sAddress.Substring(0, GLOBAL_IPV6_ADDRESS_PREFIX.Length) == GLOBAL_IPV6_ADDRESS_PREFIX)
            {
                return true;
            }
            return false;
                         
        }
        /*****************
        * stores the global & local ipv6 address which is used in PNRP
        * ***************/
        private void setIPv6Address()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in nics)
            {
                // get only ipv6 address
                if (adapter.Supports(NetworkInterfaceComponent.IPv6) == false)
                {
                    continue;
                }

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                if (uniCast.Count == 0)
                    continue;

                foreach (UnicastIPAddressInformation ipv6address in uniCast)
                {

                    if (ipv6address.Address.AddressFamily != AddressFamily.InterNetworkV6 ||
                        ipv6address.Address.ToString().Length < 4
                        )
                        continue; //we are interested only in ipv6
                    string sIPv6address = ipv6address.Address.ToString();
                    //set global address
                    if (isGlobalIPv6Address(sIPv6address))
                    {
                        //if we don't have native ipv6 then set it
                        //if(!isGlobalNativeIPv6Address(sIPv6address))
                        _globalIPv6Address = ipv6address.Address.ToString();
                    }
                    //set local address
                    if (isLocalInterface(adapter) &&
                      isLocalIPv6Address(sIPv6address))
                    {
                        _localIPv6Address = ipv6address.Address.ToString();
                    }

                }
            }
            if (_globalIPv6Address == null)
            {
                //no global address nothing will work
                Logger.Trace(Logger.MessageType.Failure, "P2PDNS::setIPv6Address:: failed to find ipv6 address ");
                throw new MingleViewException(Strings.P2PDNS_NoGlobalIPv6_ErrorMsg); 
            }
                
        }

        /*****************
         * checks whether its a global native ipv6 address
         * *********/
        private bool isGlobalNativeIPv6Address(string sAddress)
        {
            if (sAddress != null &&
                 sAddress.Substring(0, GLOBAL_IPV6_TEREDO_PREFIX.Length) == GLOBAL_IPV6_TEREDO_PREFIX)
            {
                return true;
            }
            return false;
            
        }
        /*****************
       * checks whether its a local ipv6 address
       * *********/
        private bool isLocalIPv6Address(string sAddress)
        {
            if (sAddress != null &&
                 sAddress.Substring(0, 4) == LOCAL_IPV6_ADDRESS_PREFIX)
            {
                return true;
            }
            return false;

        }
        /**************
         * check to identify its LAN,WLAN ip address
         * **********/
        private bool isLocalInterface(NetworkInterface netAdapter)
        {
            if ((netAdapter.OperationalStatus == OperationalStatus.Up) &&
                netAdapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                )
                return true;
            return false;
        }

       
            


    }
}

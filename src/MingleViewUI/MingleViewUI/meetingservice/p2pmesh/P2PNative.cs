using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MingleView;
namespace p2p
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WSAData
    {
        public short wVersion;
        public short wHighVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x101)]
        public string szDescription;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x81)]
        public string szSystemStatus;
        public short iMaxSockets;
        public short iMaxUdpDg;
        public IntPtr lpVendorInfo;
    };
    internal class P2PNative
    { 
   	    [DllImport("ws2_32.dll")]
	    internal static extern int WSAStartup(short wVersionRequested, ref WSAData lpWSAData);

	    [DllImport("ws2_32.dll")]
	    internal static extern int WSACleanup();

        [DllImport("ws2_32.dll")]
        internal static extern int WSAGetLastError();

        [DllImport("p2p.dll", CharSet=CharSet.Unicode)]
	    internal static extern int PeerPnrpStartup(int iVersion );

        [DllImport("p2p.dll", CharSet=CharSet.Unicode)]
	    internal static extern int PeerPnrpShutdown();

        [DllImport("p2p.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int PeerIdentityImport(string sIdentityXML, string sPassword, ref IntPtr pIdentity);
        
         [DllImport("p2p.dll", CharSet=CharSet.Unicode)]
         internal static extern void  PeerFreeData( IntPtr pvData);

		    //
		    // WSA Error codes
		    //
        public const int WSAEFAULT = 10014; 	// The system detected an invalid pointer address in attempting to use a pointer argument in a call.
	    public const int WSA_E_NO_MORE = 10110;	// No more results can be returned by WSALookupServiceNext.
        //
        //PNRP version
        //
        public const int PNRP_VERSION = 0x00000002;
        internal sealed class WSAService
        {
            private WSAService()
            {
                WSAData data = new WSAData();
                int err = WSAStartup(0x0002, ref data);
                if (err != 0) throw new System.Net.Sockets.SocketException(WSAGetLastError());
            }
            ~WSAService()
            {
                WSACleanup();
            }
            public static readonly WSAService Instance = new WSAService();
        }
    	

        private static WSAService service = WSAService.Instance;
        public P2PNative ()
        {
           PeerPnrpStartup(PNRP_VERSION);
        }
        ~P2PNative()
        {
            PeerPnrpShutdown();
        }
        public void PeerImportIdentityXML(string sIdentityXML,string sPassword)
        {
            IntPtr  pIdentity=IntPtr.Zero;
            int iGetLastError;

            PeerIdentityImport(sIdentityXML,sPassword,ref pIdentity);
            iGetLastError=Marshal.GetLastWin32Error();
            Logger.Trace(Logger.MessageType.Informational, "P2PNative::PeerImportIdentityXML iGetLastError::" + iGetLastError);
            if (pIdentity != IntPtr.Zero)
            {
                PeerFreeData(pIdentity);
            }
        }
    }
}

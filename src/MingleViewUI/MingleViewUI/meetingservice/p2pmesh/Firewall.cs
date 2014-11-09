using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using NetFwTypeLib;
using RDPCOMAPILib;
using System.Windows.Forms;
namespace MingleView.Firewall
{

    public class Firewall
    {     
        private Guid IID_INetFwPolicy2 = new Guid("98325047-C671-4174-8D81-DEFCD3F03186");
        private Guid CLSID_NetFwPolicy2 = new Guid("{E2B3C97F-6AE1-41AC-817A-F6F92166D7DD}");
        /**************888
         * adds in windows firewall with edge traversal
         * sAppName - unique value to identify entry in the firewall
         * *******************/
        public bool AllowApplicationForIPv6()
        {
            string sAppName=Application.ProductName ;
            string sAppPath=Application.ExecutablePath;
            string sDescription = sAppName; 
         
           

            try
            {
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                         Type.GetTypeFromProgID("HNetCfg.FwPolicy2")); ;
                INetFwRules		fwRules = null;
                

                //enumerate all rules
                fwRules = firewallPolicy.Rules;
                foreach (INetFwRule rule in fwRules)
                {
                    if (rule.ApplicationName == sAppPath &&
                        rule.EdgeTraversal == true 
                        )
                    {
                        return true;
                    }
                }
                //not found then add entry
                AddRuleWithUAC(sAppName, sAppPath, sDescription);   
            }
            catch(Exception e)
            {
                Logger.Trace(Logger.MessageType.Failure, "Firewall::AllowApplicationForIPv6 exception"+e.Message );
                return false; 
            }
            return true;
        }

        /*********************
         * add rules with a UAC prompt
         * ******************/
        private bool AddRuleWithUAC(string sAppName,string sAppPath,string sDescription)
        {
       /*      RDPSession pRdpSession = new RDPSession(); ;

            Logger.Trace(Logger.MessageType.Informational, "Firewall::AddRuleWithUAC");
            //create a dummy invitation to show a firewall prompt
            pRdpSession.Open();
            pRdpSession.Invitations.CreateInvitation("dummy", "test", "", 5);
            pRdpSession.Close();
            Marshal.ReleaseComObject(pRdpSession);
            */
           INetFwPolicy2 firewallPolicy = null;
            INetFwRule firewallRule = null;
            firewallRule = (INetFwRule)Activator.CreateInstance(
                     Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRule.Description = sDescription;
            firewallRule.Name = sAppName;
            firewallRule.ApplicationName = sAppPath;


            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRule.Enabled = true;
            firewallRule.EdgeTraversal = true;
            //enumerate the rules to find old entry 
            firewallPolicy = (INetFwPolicy2)launchElevatedCOMObject(CLSID_NetFwPolicy2, IID_INetFwPolicy2);
            if (firewallPolicy == null)
            {
                return false;
            }

            firewallPolicy.Rules.Add(firewallRule);
            Marshal.ReleaseComObject(firewallPolicy);  
            return true;
        }
        #region NativeComDecalarations
        [StructLayout(LayoutKind.Sequential)]
        public struct BIND_OPTS3
        {
            internal uint cbStruct;
            internal uint grfFlags;
            internal uint grfMode;
            internal uint dwTickCountDeadline;
            internal uint dwTrackFlags;
            internal uint dwClassContext;
            internal uint locale;
            internal IntPtr pServerInfo; // will be passing null, so type doesn't matter
            internal IntPtr hwnd;
        }


        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int CoGetObject(
           string pszName,
           [In] ref BIND_OPTS3 pBindOptions,
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)] out object rReturnedComObject);

            [Flags]
            public enum CLSCTX
            {
                CLSCTX_INPROC_SERVER = 0x1,
                CLSCTX_INPROC_HANDLER = 0x2,
                CLSCTX_LOCAL_SERVER = 0x4,
                CLSCTX_REMOTE_SERVER = 0x10,
                CLSCTX_NO_CODE_DOWNLOAD = 0x400,
                CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
                CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
                CLSCTX_NO_FAILURE_LOG = 0x4000,
                CLSCTX_DISABLE_AAA = 0x8000,
                CLSCTX_ENABLE_AAA = 0x10000,
                CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,
                CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,
                CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
                CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
            }

            /**********
         * helper to run  COM with UAC prompt
         * ********/
        public static object launchElevatedCOMObject(Guid Clsid, Guid InterfaceID)
        {
            string CLSID = Clsid.ToString("B"); // B formatting directive: returns {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} 
            string monikerName = "Elevation:Administrator!new:" + CLSID;

            BIND_OPTS3 bo = new BIND_OPTS3();
            bo.cbStruct = (uint)Marshal.SizeOf(bo);
            bo.hwnd = IntPtr.Zero;
            bo.dwClassContext = (int)CLSCTX.CLSCTX_LOCAL_SERVER;

            object retVal=null;
            CoGetObject(monikerName, ref bo, InterfaceID, out retVal);

            return (retVal);
        }
        #endregion
    }
}

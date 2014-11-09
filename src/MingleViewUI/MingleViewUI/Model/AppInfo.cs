using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows.Data;
namespace MingleView.Model
{
    [DataContract]
    [Serializable]
    public class AppInfo
    {
        [DataMember]
        public string Version
        { get; set; }

        [DataMember]
        public string DownloadPath
        { get; set; }

        public AppInfo()
        {
        }
        public AppInfo(string sVersion,string sDownloadURL)
        {
            Version = sVersion;
            DownloadPath=sDownloadURL; 

        }
        /******************
         * finds which app info is greater
         * **********************/
        public static bool operator >(AppInfo operand1, AppInfo operand2)
        {
            Version version1 =new Version(operand1.Version);
            Version version2 =new Version(operand2.Version);
            if (version1.Major > version2.Major)
            {
                return true;
            }
            if (version1.Major == version2.Major)
            {
                if (version1.Minor > version2.Minor)
                    return true;
            }
            return false;
        }
        /******************
        * finds which app info is lesse
        * **********************/
        public static bool operator <(AppInfo operand1, AppInfo operand2)
        {
            Version version1 = new Version(operand1.Version);
            Version version2 = new Version(operand2.Version);
            if (version1.Major < version2.Major)
            {
                return true;
            }
            if (version1.Major == version2.Major)
            {
                if (version1.Minor < version2.Minor)
                    return true;
            }
            return false;
        }
    }
}

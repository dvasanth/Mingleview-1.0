using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MingleView.Model;
using System.Windows.Forms;
using System.Diagnostics;
using MingleView.UI.Service;
using MingleView.Properties;
namespace MingleView
{
    public class AutoUpdater
    {
        const string DOWNLOAD_URL = "http://www.mingleview.com/MingleView.exe";
        const string UPDATER_RELATIVE_PATH = "\\crowsoft\\mingleview\\assembly\\Updater.exe";
        AppInfo _thisappInfo=new AppInfo(Application.ProductVersion,DOWNLOAD_URL);
        public AutoUpdater()
        {
        }
        /******************
         * static fucntion to create new appinfo of this process
         * *******************/
        public static AppInfo GetAppInfo()
        {
            return new AppInfo(Application.ProductVersion,DOWNLOAD_URL);
        }
        /**************************************
         * update check if appinfo higher verion then 
         * download from that URL
         * ************************************/
        public void Update(AppInfo otherAppInfo)
        {
            //check if update required
            if(otherAppInfo > _thisappInfo)
            {
                //download new
                 string sUpdaterPath;

                //prompt the user for update
                 MessageBoxService updatemsg = new MessageBoxService();
                 GenericMessageBoxResult ret;

                ret= updatemsg.Show(Strings.AutoUpdater_Ask_For_update,
                     Strings.MainWindowViewModel_DisplayName,
                     GenericMessageBoxButton.YesNo );

                if (ret == GenericMessageBoxResult.No)
                {
                    return;//user not interested in update
                }
                 sUpdaterPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                         UPDATER_RELATIVE_PATH ;

                //launch update with new download URL
                Process updaterLaunch = new Process();
                string sCmdLine = otherAppInfo.DownloadPath + " "+Application.ExecutablePath;

                updaterLaunch.StartInfo.FileName = sUpdaterPath;
                updaterLaunch.StartInfo.Arguments = sCmdLine;
                updaterLaunch.StartInfo.UseShellExecute = true;

                updaterLaunch.Start();
                //quit the application
                Environment.Exit(0);  
            }
        }

    }
}

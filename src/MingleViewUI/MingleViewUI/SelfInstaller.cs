using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Win32;
using MingleView.Properties;
using System.Windows;
namespace MingleView.UI
{
    class SelfInstaller
    {
        const string ASSEMBLY_PATH = "\\crowsoft\\mingleview\\assembly";
        const string DOT_NET_PATH = "http://www.microsoft.com/downloads/en/details.aspx?FamilyId=333325fd-ae52-4e35-b531-508d977d32a6&displaylang=en";
        Dictionary<string, string> _assemblyToDllPath = new Dictionary<string,string>(); 
        /**********************************
         * this registers the appdomai for any assemble resolve event
         * *********************************/
        public bool Init(AppDomain currentDomain)
        {
            //Check if .net 3.5 installed or not
            if (!CheckIfDotNet35Installed())
            {
                App.Current.Shutdown(1);
                return false;
            }

            currentDomain.AssemblyResolve += new ResolveEventHandler(resolvePathEventHandler);
            //extract the files from reource to appdata
            extractToAppdata("AvalonDock.dll", "AvalonDock.dll");
            extractToAppdata("Interop.NetFwTypeLib.dll", "Interop.NetFwTypeLib.dll");
            extractToAppdata("AxInterop.RDPCOMAPILib.dll", "AxInterop.RDPCOMAPILib.dll");
            extractToAppdata("Interop.RDPCOMAPILib.dll", "Interop.RDPCOMAPILib.dll");
            extractToAppdata("mingle.pfx", "mingle.pfx");
            extractToAppdata("Microsoft.Windows.Shell.dll", "Microsoft.Windows.Shell.dll");
            extractToAppdata("Updater.exe", "Updater.exe");
            return true;
        }
        /**************************************
         * Dot net 3.5 is required to run
         * ***********************************/
        bool CheckIfDotNet35Installed()
        {
            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            string[] version_names = installed_versions.GetSubKeyNames();
            //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
            double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
            if (Framework < 3.5)
            {
                
                if (MessageBox.Show(Strings.SelfInstaller_DotNet_Install_Prompt,Strings.MainWindowViewModel_DisplayName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Process.Start(DOT_NET_PATH);
                    return false;
                }
                
            }
            return true;
        }
        /********************
         * extract the resource to the appdata path
         * ********************/
        void extractToAppdata(string sResourcename,string sDllName)
        {
            string sAssemblyDir,sAssemblyDllPath;

            sAssemblyDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                         ASSEMBLY_PATH ;
            Directory.CreateDirectory(sAssemblyDir); 
            sAssemblyDllPath = sAssemblyDir + "\\" + sDllName;
            writeResourceToFile(sResourcename,sAssemblyDllPath );
            //add to map, maps assembly name to dll path
            _assemblyToDllPath.Add(sResourcename, sAssemblyDllPath);
        }

        /******************
         * helper to extract the resource
         * ******************/
        void writeResourceToFile(string resourceName, string filepath)
        {
            string[] names = this.GetType().Assembly.GetManifestResourceNames();
            using (Stream s = this.GetType().Assembly.GetManifestResourceStream("MingleView.dependency." + resourceName))
            {
                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                using (var sw = new BinaryWriter(File.Open(filepath, FileMode.Create)))
                {
                    sw.Write(buffer);
                }
            }
        }
        /**************************
        * Find the assembly extracted path & load it
        * **********************/
        Assembly resolvePathEventHandler(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.
            Assembly loadedAssembly;
            string strInstallerAssmPath = "";

            //Loop through the array of referenced assembly names.
             foreach (KeyValuePair<string,string>  pair in _assemblyToDllPath)
             {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                 if (pair.Key == args.Name.Substring(0, args.Name.IndexOf(","))+".dll")
                     {
                         //Build the path of the assembly from where it has to be loaded.				
                         strInstallerAssmPath = pair.Value;
                         break;
                     }
                      
             }
            //Load the assembly from the specified path. 					
             loadedAssembly = Assembly.LoadFrom(strInstallerAssmPath);

            //Return the loaded assembly.
             return loadedAssembly;	
        }

    }//self-isntaller
}

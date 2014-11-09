using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
namespace Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
   
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
   
            DownloadProgress downloadForm = new DownloadProgress(args[0], args[1]);
            Application.Run(downloadForm);
            //execute the process
            if (downloadForm.Completed == true)
            {
                Process mingleView = new Process();
                mingleView.StartInfo.FileName = args[1];
                mingleView.Start();
            }
        }
    }
}

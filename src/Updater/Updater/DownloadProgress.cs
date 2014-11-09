using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Updater
{
    public partial class DownloadProgress : Form
    {
        public bool Completed = false; 
        public DownloadProgress(string sFileURL,string sDownloadPath)
        {
            InitializeComponent();
  
            System.Net.WebClient _WebClient = new System.Net.WebClient();
            _WebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadFileCompleted);
            _WebClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(downloadProgressChanged);
            _WebClient.DownloadFileAsync(new Uri(sFileURL), sDownloadPath);

        }

        // Occurs when an asynchronous file download operation completes.
        private void downloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // File download completed
            if(e.Cancelled!=true)
                Completed = true;
            this.Close(); 
        }
        // Occurs when an asynchronous download operation successfully transfers some or all of the data.
        private void downloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
           // Update progress bar
           progressDownload.Value = e.ProgressPercentage;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // application quit
            Application.Exit();   
        }

    

    }
}

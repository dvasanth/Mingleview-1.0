using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace MingleView.UI
{
    class ErrorReporting
    {
        const int LOG_FILE_READ_SIZE = 20 * 1024;//reduce the last 25kb for analysing
        const string MINGLE_POST_URL = "http://www.mingleview.com/help/module.php?module=HelpCenter";

        /*******************************
         * sends the information & trace file to support
         * *****************************/
        public void Send(string sUsername,string sUserEmail,string sSubject,string sMailContent)
        {
            string sPostData;   
            // Create the Web Request Object 
            WebRequest emailRequest = WebRequest.Create(MINGLE_POST_URL);
            // Specify that you want to POST data
            emailRequest.Method = "POST";

            emailRequest.ContentType = "application/x-www-form-urlencoded";
            sPostData = "departmentid=1" + "&name="+sUsername  + "&email=" + sUserEmail+
                "&subject="+sSubject + "&message="+ sMailContent + getLogFileData()  + "&email_send=Submit";
            Stream postDataStream = emailRequest.GetRequestStream();
            UTF8Encoding utf8Encoder= new UTF8Encoding();
            byte[] postData = utf8Encoder.GetBytes(sPostData);
            //post the data to mingle support
            postDataStream.Write(postData, 0, postData.Length);
            postDataStream.Close();
            HttpWebResponse response = (HttpWebResponse)emailRequest.GetResponse();
            StreamReader responseStream = new StreamReader(response.GetResponseStream(),Encoding.UTF8);
            string httpErrorCode = responseStream.ReadToEnd();

        }   

        /*****************************
         * reads the logfile to be send to the support
         * ***************************/
        string getLogFileData()
        {
            FileStream fileStream = null;
            StreamReader streamRead = null;
            char[] readBuffer;
            //
            try
            {

                fileStream = new FileStream(Logger.FileLocation, FileMode.Open,
                      FileAccess.Read, FileShare.ReadWrite);
                streamRead = new StreamReader(fileStream);

                //read the last 
                if (fileStream.Length < LOG_FILE_READ_SIZE)
                {
                    readBuffer = new char[fileStream.Length];
                    streamRead.Read(readBuffer, 0, (int)fileStream.Length);
                }
                else
                {
                    fileStream.Position = fileStream.Length - LOG_FILE_READ_SIZE;
                    readBuffer = new char[LOG_FILE_READ_SIZE];
                    streamRead.Read(readBuffer, 0, LOG_FILE_READ_SIZE);
                }
                
            }
            catch(System.IO.IOException )
            {
                if (fileStream != null)
                    fileStream.Close();
                fileStream = null;
                if (streamRead != null)
                    streamRead.Close();
                streamRead = null;
                return "";
            }

            //convert string
            return new string(readBuffer);
        }
    }
}

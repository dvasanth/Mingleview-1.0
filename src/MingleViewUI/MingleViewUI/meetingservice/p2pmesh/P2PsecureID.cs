using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Security.Cryptography;
namespace p2p.encrypt
{
    class p2psecureID
    {
        const string    ALLOWED_PASS_CHARS = "BCDFGHJKLMNPQRSTVWXYZ23456789";
        const int       HASH_OUTPUT_SIZE = 20;
        Encoding        UnicodeText = Encoding.Unicode;
        const int       PASS_CHAR_LEN=6;
        const int       HASH_16_BYTES = 16;
       
        /********************
         * Generates the password from the  data needed to be exchanged between the peers
         * *****************/
        public void GenerateID(string sP2PData,out string sPassCode,ref byte[] byEncP2PData,out string sUniqueName)
        {    
            const int       PASS_GEN_HASH_INPUT_SIZE = 8000;
            byte[]          byP2Pdata = UnicodeText.GetBytes (sP2PData);
            byte[]          byHashResult;
            char[]          byPassChars =  new char[PASS_CHAR_LEN];

            //do a repetitive hash
            byHashResult=GetRepetiveHash(byP2Pdata, PASS_GEN_HASH_INPUT_SIZE);

            //character mapping
            for (int iIndex = 0; iIndex < byPassChars.Length; iIndex++)
            {
                int iPassCharIndex = (byHashResult[iIndex]*ALLOWED_PASS_CHARS.Length) / 256 ;
                byPassChars[iIndex] = ALLOWED_PASS_CHARS[iPassCharIndex]; 
            }
            //passcode
            sPassCode= new string(byPassChars);
            //sPassCode = UnicodeText.(byPassChars));

            //encrypt the data with pass code
            byEncP2PData = EncryptDecryptData(true, byP2Pdata, UnicodeText.GetBytes(sPassCode));

            //create unique name with the pass
            sUniqueName = GetUniqueName(UnicodeText.GetBytes(sPassCode));

        }
        /*************
         *derives  the name from the pass code
         * ************/
        public string GetUniqueName(string sPassCode)
        {
            return GetUniqueName(UnicodeText.GetBytes(sPassCode));
        }
        /**********************
         * decrypts the data with the pass code provided
         * *******************/
        public string DecryptP2PData(byte[] byEncData, string sPassCode)
        {
            byte[] DecryptBytes = EncryptDecryptData(false, byEncData, UnicodeText.GetBytes(sPassCode));
            return UnicodeText.GetString(DecryptBytes);
        }
        /******************
         * Generate a name from the pass code
         * **************/
        private string GetUniqueName(byte[] byPassData)
        {
            byte[] byHashResult = new byte[HASH_OUTPUT_SIZE];
            byte[] by16ByteHash;

            byHashResult=GetTimeDependentHash(byPassData);
            by16ByteHash = new byte[HASH_16_BYTES];
            Array.Copy(byHashResult, by16ByteHash, HASH_16_BYTES);
            return BitConverter.ToString(by16ByteHash).Replace("-", ""); 
        }
        /**************************
         * Encrypt the data with pass key
         * *************************/
        private byte[] EncryptDecryptData(bool bEncrypt,byte[] byData, byte[] byPassData)
        {
            byte[]          byHashResult, by16ByteHash;
            SHA1            Sha1Cal = new SHA1CryptoServiceProvider();
            string          s16ByteHash;
            byte[]          byHashInput;



            ///get the time dependpent hash
            byHashResult = GetTimeDependentHash(byPassData);
            
            //use first 16 bytes to form unicode char
            by16ByteHash = new byte[HASH_16_BYTES];
            Array.Copy(byHashResult, by16ByteHash, HASH_16_BYTES);
            s16ByteHash = BitConverter.ToString (by16ByteHash);
            s16ByteHash = s16ByteHash.Replace("-","");
            byHashInput = System.Text.Encoding.Unicode.GetBytes(s16ByteHash);
            byHashResult = Sha1Cal.ComputeHash(byHashInput);
            //use this hash to encrypt data
            Array.Copy(byHashResult, by16ByteHash, HASH_16_BYTES);



            return bEncrypt?Aes128Encrypt(byData, by16ByteHash):
                            Aes128Decrypt(byData, by16ByteHash); 
        }
        /****************
         * method for aes encrypt
         * ********/
        private byte[] Aes128Encrypt(byte[] byData, byte[] byKey)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.Key = byKey;
            aes.IV = byKey;
            MemoryStream msOut = new MemoryStream();
            ICryptoTransform aesEncrypt =  aes.CreateEncryptor();
            CryptoStream cryptStream = new CryptoStream(msOut, aesEncrypt, CryptoStreamMode.Write );
            cryptStream.Write(byData, 0, byData.Length);
            cryptStream.FlushFinalBlock();
            cryptStream.Close();
            return msOut.ToArray();
        }
        /****************
        * method for aes decrypt
        * ********/
        private byte[] Aes128Decrypt(byte[] byData, byte[] byKey)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            byte[] decryptData = new byte[byData.Length];
            int iDecryptLen;
  
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.Key = byKey;
            aes.IV = byKey;
            MemoryStream msOut = new MemoryStream(byData);
            ICryptoTransform aesEncrypt = aes.CreateDecryptor();
            CryptoStream cryptStream = new CryptoStream(msOut, aesEncrypt, CryptoStreamMode.Read);
            iDecryptLen= cryptStream.Read(decryptData, 0, decryptData.Length);
            Array.Resize(ref decryptData, iDecryptLen);
            cryptStream.Close();
            return decryptData;
        }
        /**********************
         * microsoft method to calculate a time dependent hash
         * ****************/
        private byte[] GetTimeDependentHash(byte[] byData)
        {
            DateTime    UtcTime=  DateTime.UtcNow;
            long        lMinElaspedSinceUTC = UtcTime.Ticks / 10000000;
            long        lHoursElaspedSinceUTC = (lMinElaspedSinceUTC / 3600)*2; //get the half hours elasped since Jan 1, 1970
            string      sUtcHoursElapsed = lHoursElaspedSinceUTC.ToString();
            int         iHashInputSize = byData.Length+ UnicodeText.GetBytes(sUtcHoursElapsed).Length;
            byte[]      byHashInputBuff = new byte[iHashInputSize];
            byte[]      byHashResult= new byte[HASH_OUTPUT_SIZE];


            //copy the input bytes data
            Array.Copy(byData,byHashInputBuff, byData.Length);
            //copy the utc time to unicode chars
            Array.Copy(UnicodeText.GetBytes(sUtcHoursElapsed),0,byHashInputBuff, byData.Length,
                                                   UnicodeText.GetBytes(sUtcHoursElapsed).Length);

            byHashResult=GetRepetiveHash(byHashInputBuff, -1);
            return byHashResult;  
        }
        /************
         * microsoft documented repetitive hash method for ID calcualteion for pnrp RA
         * *************/
        private byte[] GetRepetiveHash(byte[] byData,int iMaxHashInputSize)
        {
            int iDataInputSize = (iMaxHashInputSize == -1) ? byData.Length : Math.Min(byData.Length, iMaxHashInputSize);//which ever is minimum
            int iHashInputSize = iDataInputSize+ HASH_OUTPUT_SIZE;
            byte[] byHashInputBuff = new byte[iHashInputSize];
            const int HASH_REPETIIONS=100000;
            byte[] byHashResult=new byte[HASH_OUTPUT_SIZE];
            SHA1 Sha1Cal = new SHA1CryptoServiceProvider(); 


            //zero all bytes
            Array.Clear(byHashInputBuff, 0, iHashInputSize-1);
            Array.Clear(byHashResult, 0, HASH_OUTPUT_SIZE - 1);
            for (int iRepeat = 0; iRepeat < HASH_REPETIIONS; iRepeat++)
            {
                //copy the original data to the hashinput buffer
                Array.Copy(byData,byHashInputBuff, iDataInputSize);
                //append the previous hash result to the original data
                Array.Copy(byHashResult, 0, byHashInputBuff, iDataInputSize, HASH_OUTPUT_SIZE);  
                //compute the hash
                byHashResult=Sha1Cal.ComputeHash(byHashInputBuff);
            }
            return byHashResult;
        }
    }
}

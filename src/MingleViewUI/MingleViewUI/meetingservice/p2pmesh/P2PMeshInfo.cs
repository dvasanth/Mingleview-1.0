using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// Note: When building this code, you must reference the
// System.Runtime.Serialization.Formatters.Soap.dll assembly.
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
namespace p2p.mesh
{
    [Serializable()]
    class P2PMeshInfo
    {
         
        private string _customResolverURI;
         
        private string _meshURI;
         
        private string _meshPassword;

        public string CustomResolverURI
        {
            get { return _customResolverURI;}
            set { _customResolverURI = value; }
        }
        public string MeshURI
        {
            get { return _meshURI; }
            set { _meshURI = value; }
        }
        public string MeshPassword
        {
            get { return _meshPassword; }
            set { _meshPassword = value; }
        }
        /*********
         * throw Mingleview exception
         * ******/
        public string Serialize()
        {
            SoapFormatter Formatter = new SoapFormatter();
            MemoryStream stream = new MemoryStream();
            UTF8Encoding enc=new UTF8Encoding();     

            Formatter.Serialize(stream,this);
            return enc.GetString( stream.ToArray());
        }
        /****************
         *  throw Mingleview exception
         * *********/
        static public P2PMeshInfo DeSerialize(string sData)
        {
            SoapFormatter Formatter = new SoapFormatter();
            
            UTF8Encoding dec=new UTF8Encoding();
            MemoryStream stream = new MemoryStream(dec.GetBytes(sData) );

            return (P2PMeshInfo)Formatter.Deserialize(stream);
            
        }
    }
}

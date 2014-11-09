using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MingleView.Model
{
    [DataContract]
    [Serializable]
    public class RDInvitation : IEquatable<RDInvitation>
    {
        [DataMember]
        public string ConnectionString;

        [DataMember]
        public Participant Inviter;

        [DataMember]
        public string Password;

        public RDInvitation()
        {
        }
        /***************
         * copy constructor
         * *************/
        public RDInvitation(RDInvitation invitation)
        {
            this.ConnectionString = invitation.ConnectionString;
            this.Inviter = invitation.Inviter;
            this.Password = invitation.Password; 
        }
        public RDInvitation(string sConnectionString,Participant inviter,string password )
        {
            ConnectionString = sConnectionString;
            Inviter = inviter;
            Password = password; 
        }
        /*****************
        * objects differs  by connection string
        * ************/
        public bool Equals(RDInvitation other)
        {
            return ConnectionString.Equals(other.ConnectionString);
        }
    }
}

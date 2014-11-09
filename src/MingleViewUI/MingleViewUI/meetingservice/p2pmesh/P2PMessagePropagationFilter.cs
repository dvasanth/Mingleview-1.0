using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Diagnostics;
namespace p2p.mesh
{
    class P2PMessagePropagationFilter : PeerMessagePropagationFilter
    {
        public P2PMessagePropagationFilter()
        {

        }

        public override PeerMessagePropagation ShouldMessagePropagate(Message message, PeerMessageOrigination origination)
        {

            PeerMessagePropagation destination = PeerMessagePropagation.LocalAndRemote;

           
           
            return destination;

        }
    }
}

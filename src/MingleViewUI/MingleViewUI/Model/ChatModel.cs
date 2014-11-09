using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MingleView.Model
{
    public class ChatModel //: System.ComponentModel.INotifyPropertyChanged
    {
        public string ChatMsg { get; set; }
        public string AllChatMsgs { get; set; }
    }
   
    public class AllMsgLst
    {
        public string AllChatMsg { get; set; }
    }
    public class UsersList
    {
        public string UserName{ get; set; }
    }
}

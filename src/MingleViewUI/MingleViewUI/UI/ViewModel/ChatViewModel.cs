using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Data;
using System.Collections.ObjectModel;
using MingleView.Model;
using MingleView.Service;
using System.Windows.Threading;


namespace MingleView.UI.ViewModel
{
    public class ChatViewModel : MeetingWorkspaceViewModel
    {
        #region Fields
        RelayCommand _ChatSendCmd;
        string _chatMsg="";
        public string _AllChatMsg;
        public string _UserName;
        readonly IMeetingManager _meetingManager;
        private readonly Dispatcher _dispatcher;

        ObservableCollection<ChatMessage> _chatMsgCollection = new ObservableCollection<ChatMessage>();
        #endregion //Fields

        public ChatViewModel(IMeetingManager meetingManager, Dispatcher dispatcher)
        {
            _meetingManager = meetingManager;
            _dispatcher = dispatcher;
            //register the event handlers
            _meetingManager.MeetingEvents.ChatMessageReceived += delegate(object sender, ChatMessageEventArgs chatArgs)
            {
                this._dispatcher.Invoke(
                    (Action)delegate { OnChatMsg(chatArgs.Message); },
                    null);
            };

        }
        public ObservableCollection<ChatMessage> ChatMsgCollection
        {
            get { return _chatMsgCollection; }
        }

        #region Chat Properties
        public string ChatMsg
        {
            get { return _chatMsg; }
            set
            {
                if (value == _chatMsg)
                    return;

                _chatMsg = value;

                base.OnPropertyChanged("ChatMsg");
            }
        }
        #endregion //Chat Properties

        #region Chat Activities
        public ICommand ChatSendCmd
        {
            get
            {
                if (_ChatSendCmd == null)
                {
                    _ChatSendCmd = new RelayCommand(param => this.OnSend());
                }
                return _ChatSendCmd;
            }
        }
        /*********************
         * send chat message
         * *****************/
        void OnSend()
        {
            if (this.ChatMsg.Length == 0)
                return;

            ChatMessage msg = new ChatMessage(_meetingManager.Self, this.ChatMsg, DateTime.Now);

            _meetingManager.InformAll.Chat(msg);
            this.ChatMsg = "";
        }
        #endregion
        #region Event Notification
        /******************
         * received a chat message
         * ***************/
        public void OnChatMsg(ChatMessage message)
        {
            _chatMsgCollection.Add(message);
        }
        #endregion
    }
}


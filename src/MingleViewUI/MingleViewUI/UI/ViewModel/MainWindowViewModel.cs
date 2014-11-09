using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Globalization;
using MingleView.UI.ViewModel;
using MingleView.UI.View;
using MingleView.Properties;
using MingleView.Model;
using MingleView.Service;
using MingleView.UI.Service;
namespace MingleView.UI.ViewModel
{
    public class MainWindowViewModel:MeetingWorkspaceViewModel
    {
        #region Fields
        bool _showMeetingItems;
        bool _isProgressRunning;
        string _loadingText;
        string _progressValue;
        ChatViewModel _chtModel;
        ParticipantViewModel _participantModel;
        QuickLaunchViewModel _quickLaunchModel;
        RemoteScreenViewModel _remoteScreenModel;
        QuickStartMeetingViewModel _quickStartMeetingModel;
        JoinMeetingViewModel _joinMeetingModel;
        RelayCommand _closeCommand;
        ObservableCollection<MeetingWorkspaceViewModel> _workspaces;
        IMeetingManager _meetingManager;
        private readonly System.Windows.Threading.Dispatcher _dispatcher; 
        #endregion // Fields

        #region Constructor

        public MainWindowViewModel()
        {
            //ShowMeetingItems = false;
            base.DisplayName = MingleView.Properties.Strings.MainWindowViewModel_DisplayName;
            _dispatcher = (MingleView.UI.App.Current != null) ? MingleView.UI.App.Current.Dispatcher : System.Windows.Threading.Dispatcher.CurrentDispatcher;
        }

        #endregion // Constructor

        #region Commands
        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => this.onExit());

                return _closeCommand;
            }
        }
        /// <summary>
        /// Returns a read-only list of commands 
        /// that the UI can display and execute.
        /// </summary>
  
        #endregion // Commands

        #region Workspaces

        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<MeetingWorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<MeetingWorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }
        /*****************************************/
        //Show/Hide property for Docking manager 
        /*****************************************/
        public bool ShowMeetingItems
        {
            get 
            {
                return _showMeetingItems;
            }
            set 
            {
                if (value == _showMeetingItems)
                    return;

                _showMeetingItems = value;

                base.OnPropertyChanged("ShowMeetingItems");
            }
        }
       
        public QuickStartMeetingViewModel QuickStartMeetingModel
        {
            get
            {
                return _quickStartMeetingModel;
            }
            set
            {
                if (value == _quickStartMeetingModel)
                    return;

                _quickStartMeetingModel = value;

                base.OnPropertyChanged("QuickStartMeetingModel");
            }
        }
        public JoinMeetingViewModel JoinMeetingModel
        {
            get
            {
                return _joinMeetingModel;
            }
            set
            {
                if (value == _joinMeetingModel)
                    return;

                _joinMeetingModel = value;

                base.OnPropertyChanged("JoinMeetingModel");
            }
        }
     
        public ChatViewModel ChatModel
        {
            get
            {
                return _chtModel;
            }
            set
            {
                if (value == _chtModel)
                    return;

                _chtModel = value;

                base.OnPropertyChanged("ChatModel");
            }
        }
        public ParticipantViewModel ParticipantModel
        {
            get
            {
                return _participantModel;
            }
            set
            {
                if (value == _participantModel)
                    return;

                _participantModel = value;

                base.OnPropertyChanged("ParticipantModel");
            }
        }
        public QuickLaunchViewModel QuickLaunchModel
        {
            get
            {
                return _quickLaunchModel;
            }
            set
            {
                if (value == _quickLaunchModel)
                    return;

                _quickLaunchModel = value;

                base.OnPropertyChanged("QuickLaunchModel");
            }
        }
        public RemoteScreenViewModel RemoteScreenModel
        {
            get
            {
                return _remoteScreenModel;
            }
            set
            {
                if (value == _remoteScreenModel)
                    return;

                _remoteScreenModel = value;

                base.OnPropertyChanged("RemoteScreenModel");
            }
        }

        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (MeetingWorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (MeetingWorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            MeetingWorkspaceViewModel workspace = sender as MeetingWorkspaceViewModel;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers


        /***************
         * If a viewmodel already exists removes it & creates new one
         * ****************/
        void CreateView(MeetingWorkspaceViewModel   workspace,string sViewModelName)
        {
            //Launch it if already created
            MeetingWorkspaceViewModel   existingWorkspace =
               this.Workspaces.FirstOrDefault(vm => vm.ToString() == sViewModelName);
            if (existingWorkspace != null)
            {
                this.Workspaces.Remove(existingWorkspace);
            }
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }
        void SetActiveWorkspace(MeetingWorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

       

        #endregion // Private Helpers

        #region Event Interactions between view models
        void QuickStartMeetingEvnt(object sender, QuickStartMeetingViewModelEvntArgs e)
        {
            if (e.ViewModel != null)
            {
                startMeeting();
            }
        }
        void JoinMeetingEvnt(object sender, JoinMeetingViewModelEvntArgs e)
        {
            if (e.ViewModel != null)
            {
                joinMeeting();
            }
        }
        #endregion //Event Interactions between view models

        #region Menu Items
        public List<MenuItem> MenuOptions
        {
            get
            {
                var menu = new List<MenuItem>();
                
                //Meeting menu items
                var miMeeting = new MenuItem();
                miMeeting.Header = "_Meeting";
                //Start Meeting
                var miStart = new MenuItem();
                miStart.Header = "_Start Meeting";
                miStart.Command = new RelayCommand(param => launchQuickStartMeeting());
                miStart.Visibility = ShowMeetingItems ? Visibility.Collapsed : Visibility.Visible;
                miMeeting.Items.Add(miStart);
                //Join Meeting
                var miJoin = new MenuItem();
                miJoin.Header = "_Join Meeting";
                miJoin.Command = new RelayCommand(param => launchJoinMeeting());
                miJoin.Visibility = ShowMeetingItems ? Visibility.Collapsed : Visibility.Visible;
                miMeeting.Items.Add(miJoin);
                //Leave Meeting
                var miLeave = new MenuItem();
                miLeave.Header = "_Leave Meeting";
                miLeave.Command = new RelayCommand(param => leaveMeeting());
                miLeave.Visibility = ShowMeetingItems ? Visibility.Visible : Visibility.Collapsed;
                miMeeting.Items.Add(miLeave);
                //Add separator
                miMeeting.Items.Add(new Separator());
                //Exit Meeting
                var miExit = new MenuItem();
                miExit.Header = "_Exit";
                miExit.Command = new RelayCommand(param => onExit()); 
                miMeeting.Items.Add(miExit);

                //Help menu items
                var miHelp = new MenuItem();
                miHelp.Header = "_Help";
                //Send Error Report
                var miSER = new MenuItem();
                miSER.Header = "_Contact Support";
                miSER.Command = new RelayCommand(param => ShowSendErrorReport());
                miHelp.Items.Add(miSER);
                //Add separator
                miHelp.Items.Add(new Separator());
                //Exit Meeting
                var miAbout = new MenuItem();
                miAbout.Header = "_About";
                miAbout.Command = new RelayCommand(param => ShowAboutMVDlg());
                miHelp.Items.Add(miAbout);

                menu.Add(miMeeting);
                menu.Add(miHelp);
                return menu;
            }
        }
        #endregion //Menu Items
        #region MeetingService
        /***************************
         * starts the meeting & shows progress
         * ************************/
         /******** Quick Start **************/
         void launchQuickStartMeeting()
        {
            if(null != _joinMeetingModel)
                _joinMeetingModel.ShowJoinMeeting = false;
            if (null == _quickStartMeetingModel)
            {
                QuickStartMeetingModel = new QuickStartMeetingViewModel();
                QuickStartMeetingModel.StartMeeting += new EventHandler<QuickStartMeetingViewModelEvntArgs>(QuickStartMeetingEvnt);
            }
            _quickStartMeetingModel.ShowStartMeeting = true;
        }
        /******** Quick Start **************/
        /******** Joint Meeting **************/
         void launchJoinMeeting()
        {
            if (null != _quickStartMeetingModel)
                _quickStartMeetingModel.ShowStartMeeting = false;
            if (null == _joinMeetingModel)
            {
                JoinMeetingModel = new JoinMeetingViewModel();
                JoinMeetingModel.JoinMeeting += new EventHandler<JoinMeetingViewModelEvntArgs>(JoinMeetingEvnt);
            }
            _joinMeetingModel.ShowJoinMeeting = true;
        }
        /******** Joint Meeting **************/
        void startMeeting()
        {
            _quickStartMeetingModel.ShowStartMeeting = false;
            ShowProgress(Strings.MainWindowViewModel_Meeting_Progress_caption, true);
            Participant host = new Participant(_quickStartMeetingModel.ParticipantName); 
            host.ChangeToHost();
            //initially host is also presenter 
            host.ChangeToPresenter();
            _meetingManager = new MeetingService();

            _meetingManager.MeetingEvents.MeetingCreationStatusUpdate += delegate(object sender, MeetingStatusEventArgs meshDetails)
            {
                this._dispatcher.Invoke(
                    (Action)delegate { onStartMeeting(meshDetails.MeshID, meshDetails.ErrorMsg); },
                    null);
            };
            _meetingManager.MeetingEvents.MeetingClosed += delegate(object sender, MeetingStatusEventArgs meshDetails)
            {
                this._dispatcher.Invoke(
                    (Action)delegate
                {
                    MessageBoxService msgui = new MessageBoxService();
                    //show a error message box
                    msgui.Show(meshDetails.ErrorMsg);   

                    //close the meeting
                    leaveMeeting();
                },
                    null);
            };
            _meetingManager.CreateNewMeeting(host);
            UpdateProgress(10);
        }
        /***********************************
         * exit the application
         * **********************************/
        void onExit()
        {
            leaveMeeting();
            base.CloseCommand.Execute(this);
        }
        void ShowSendErrorReport()
        {
            SendErrorReportViewModel ErrReportModel = new SendErrorReportViewModel();
            SendErrorReport ErrReportWindow = new SendErrorReport();

            /*** Close window handling from viewmodel *****/
            EventHandler handler = null;
            handler = delegate
            {
                ErrReportModel.RequestClose -= handler;
                ErrReportWindow.Close();
            };
            ErrReportModel.RequestClose += handler;
            /*** Close window handling from viewmodel *****/

            ErrReportWindow.DataContext = ErrReportModel;
            ErrReportWindow.ShowDialog();
            
        }
        void ShowAboutMVDlg()
        {
            About dlg = new About();
            dlg.ShowDialog();
        }
        /***********************
         * event notifiers
         * *********************/
        void onStartMeeting(string sMeetingID,string sErrorMessage)
        {
            UpdateProgress(100);
            Logger.Trace(Logger.MessageType.Informational, "start meeting:" + sMeetingID + "--" + sErrorMessage);
            ParticipantModel = new ParticipantViewModel(_meetingManager, _dispatcher);
            CreateView(_participantModel, "ParticipantViewModel");

            ChatModel = new ChatViewModel(_meetingManager, _dispatcher);
            CreateView(_chtModel, "ChatViewModel");

            QuickLaunchModel = new QuickLaunchViewModel(_meetingManager, _dispatcher);
            CreateView(QuickLaunchModel, "QuickLaunchViewModel");

            RemoteScreenModel = new RemoteScreenViewModel(_meetingManager, _dispatcher);
            CreateView(RemoteScreenModel, "RemoteScreenViewModel");

            ShowProgress(string.Empty, false);
            ShowMeetingItems = true;
            base.OnPropertyChanged("MenuOptions");
        }
        /***************
         * close the existing meeting
         * **************/
        void leaveMeeting()
        {
            ShowProgress(Strings.MainWindowViewModel_Meeting_Leaving_caption, false);
            ShowMeetingItems = false;
            
            if (_meetingManager != null)
            {
                //destroy all forms
                //then destroy meeting service
                _meetingManager.LeaveMeeting();
                base.OnPropertyChanged("MenuOptions");
                _meetingManager = null; 
            }
        }
        /************************
         * join the meeting with ID
         * **********************/
        void joinMeeting()
        {
            _joinMeetingModel.ShowJoinMeeting = false;
            ShowProgress(Strings.MainWindowViewModel_Meeting_Progress_caption, true);
            Participant host = new Participant(_joinMeetingModel.ParticipantName);
            _meetingManager = new MeetingService();

            _meetingManager.MeetingEvents.MeetingJoinStatusUpdate += delegate(object sender, MeetingStatusEventArgs meshDetails)
            {
                this._dispatcher.Invoke(
                    (Action)delegate { onStartMeeting(meshDetails.MeshID, meshDetails.ErrorMsg); },
                    null);
            };
            _meetingManager.MeetingEvents.MeetingClosed += delegate(object sender, MeetingStatusEventArgs meshDetails)
            {
                this._dispatcher.Invoke(
                    (Action)delegate
                {
                    MessageBoxService msgui=new MessageBoxService(); 
                    //show a error message box
                    msgui.Show(meshDetails.ErrorMsg);   
                    //close the meeting
                    leaveMeeting();
                },
                    null);
            };
            _meetingManager.JoinMeeting(_joinMeetingModel.TicketID, host);
         
        }
        #endregion //meetingservice

        /************ Progress window *******************/
        public bool IsProgressRunning
        {
            get
            {
                return _isProgressRunning;
            }
            set
            {
                if (_isProgressRunning == value)
                    return;

                _isProgressRunning = value;
                base.OnPropertyChanged("IsProgressRunning");
            }
        }
        public string LoadingText
        {
            get
            {
                return _loadingText;
            }
            set
            {
                if (_loadingText == value)
                    return;

                _loadingText = value;
                base.OnPropertyChanged("LoadingText");
            }
        }
        public string ProgressValue
        {
            get
            {
                return _progressValue;
            }
            set
            {
                if (_progressValue == value)
                    return;

                _progressValue = value;
                base.OnPropertyChanged("ProgressValue");
            }
        }
        void ShowProgress(string sLoadingText, bool bShow)
        {
            LoadingText = sLoadingText;
            IsProgressRunning = bShow;
        }
        void UpdateProgress(int value)
        {
            //Commented as of now...
            //ProgressValue = Convert.ToString(value) + "%";
        }
        /************ Progress Window *******************/
    }
}

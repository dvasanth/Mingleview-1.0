using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MingleView.UI.ViewModel;
using System.ComponentModel;
using AxRDPCOMAPILib;
using MingleView.Model;
namespace MingleView.UI.View
{
    /// <summary>
    /// Interaction logic for RemoteScreenView.xaml
    /// </summary>
    public partial class RemoteScreenView : UserControl
    {
        private AxRDPCOMAPILib.AxRDPViewer _axRdpViewer;
        // Create the interop host control.
        System.Windows.Forms.Integration.WindowsFormsHost _axFormHost = null;
        private bool _bScaleAxRDPScreen = false;    //initially activex is not scaled
        private bool _bControlDesktop = false;    //initially control is not possible
        public RemoteScreenView()
        {
            InitializeComponent();
        
            _axFormHost = new System.Windows.Forms.Integration.WindowsFormsHost();
            
            // Create the ActiveX control.
            _axRdpViewer = new AxRDPCOMAPILib.AxRDPViewer();

            // Assign the ActiveX control as the host control's child.
            _axFormHost.Child = _axRdpViewer;
            // Add the interop host control to the Grid
            // control's collection of child controls.
            this.gridRDPHolder.Children.Add(_axFormHost);

       
            this.DataContextChanged+=new DependencyPropertyChangedEventHandler(RemoteScreenView_DataContextChanged);
        }
   
     void RemoteScreenView_DataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
      {
          //monitor the property changes in the view model
          if (this.DataContext != null)
              (this.DataContext as RemoteScreenViewModel).PropertyChanged += PropertyChangedEventHandler;
   
      }


        private void gridRDPHolder_Initialized(object sender, EventArgs e)
        {
         
          
          }
        private void gridRDPHolder_Loaded(object sender, RoutedEventArgs e)
        {
            rdpScreenEnabled(false);
            btnRequestReleaseControl.Content = Properties.Strings.RemoteScreenViewModel_Request_Control_Text;
            btnScale.Content = Properties.Strings.RemoteScreenViewModel_Screen_Fit_to_size;
        }
        /*********************
         * ********************/
        void rdpScreenEnabled(bool bEnabled)
        {
            btnRequestReleaseControl.Visibility = bEnabled?Visibility.Visible  :Visibility.Hidden  ;
            btnScale.Visibility = bEnabled ? Visibility.Visible : Visibility.Hidden; 
        }

        /****************
         * *****************/
        private void OnConnectionEstablished(object sender, EventArgs e)
        {
            rdpScreenEnabled(true);
 
        }
        /***********
         * error so exists
         * ********/
        private void OnError(object sender, _IRDPSessionEvents_OnErrorEvent e)
        {
            //disable request control
            rdpScreenEnabled(false);
        }
        /****************
         * connection got terminated
         * *************/
        private void OnConnectionTerminated(object sender, _IRDPSessionEvents_OnConnectionTerminatedEvent e)
        {
           //disable request control
            rdpScreenEnabled(false);
        }
        /*****************
         * ***************/
        private void OnConnectionFailed(object sender, EventArgs e)
        {
            //diable request control
            rdpScreenEnabled(false);
        }
        /***************
         * RequestControl method works only after attendee connected
         * ****************/
        void _axRdpViewer_OnAttendeeConnected(object sender, AxRDPCOMAPILib._IRDPSessionEvents_OnAttendeeConnectedEvent e)
        {
            RDPCOMAPILib.IRDPSRAPIAttendee pAttendee = e.pAttendee as RDPCOMAPILib.IRDPSRAPIAttendee;
            if (pAttendee.RemoteName == (this.DataContext as RemoteScreenViewModel).RDAttendeeName)
            {
                //request only for our connection event
                _axRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_VIEW);
                _axRdpViewer.RequestColorDepthChange(16);
                ((System.Windows.Interop.IKeyboardInputSink)_axFormHost).TabInto(new System.Windows.Input.TraversalRequest(FocusNavigationDirection.First));
            }
        }
        /**************
         * rdp properties change notification
         * (**************/
        void PropertyChangedEventHandler(Object sender,PropertyChangedEventArgs e)
        {
            //invitaion update
            if (e.PropertyName == "ScreenInvitation")
            {
                RDInvitation invitation = (this.DataContext as RemoteScreenViewModel).ScreenInvitation;

                if (invitation != null)
                {

                    //register for activex events
                    this._axRdpViewer.OnError += this.OnError;
                    this._axRdpViewer.OnConnectionFailed += this.OnConnectionFailed;
                    this._axRdpViewer.OnConnectionTerminated += this.OnConnectionTerminated;
                    this._axRdpViewer.OnConnectionEstablished += this.OnConnectionEstablished;
                    this._axRdpViewer.OnAttendeeConnected += new AxRDPCOMAPILib._IRDPSessionEvents_OnAttendeeConnectedEventHandler(_axRdpViewer_OnAttendeeConnected);
    
                    _axRdpViewer.Connect(invitation.ConnectionString,
                                           (this.DataContext as RemoteScreenViewModel).RDAttendeeName,
                                            invitation.Password);  
                }
            }

    
        }
        /********************
         * *
         * *****************/
        private void btnRequestReleaseControl_Click(object sender, RoutedEventArgs e)
        {
            //toggle it
            _bControlDesktop = (_bControlDesktop == true) ? false : true;

            if (_bControlDesktop)
            {
                _axRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE);
               ((System.Windows.Interop.IKeyboardInputSink)_axFormHost).TabInto(new System.Windows.Input.TraversalRequest(FocusNavigationDirection.First));
                btnRequestReleaseControl.Content = Properties.Strings.RemoteScreenViewModel_Release_Control_Text; 
            }
            else
            {
                _axRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_VIEW);
                btnRequestReleaseControl.Content = Properties.Strings.RemoteScreenViewModel_Request_Control_Text; 
            }
        }
        /***********************
         * 
         * ***********************/
        private void btnScale_Click(object sender, RoutedEventArgs e)
        {
            //toggle it
            _bScaleAxRDPScreen = (_bScaleAxRDPScreen == true) ? false : true;
            _axRdpViewer.SmartSizing = (_bScaleAxRDPScreen == true) ? true : false;

            if (_bScaleAxRDPScreen)
            {
                btnScale.Content = Properties.Strings.RemoteScreenViewModel_Screen_restore;
            }
            else
            {
                btnScale.Content = Properties.Strings.RemoteScreenViewModel_Screen_Fit_to_size;
            }
            ((System.Windows.Interop.IKeyboardInputSink)_axFormHost).TabInto(new System.Windows.Input.TraversalRequest(FocusNavigationDirection.First));
        }
    }
}

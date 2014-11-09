using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MingleView.Model;
using MingleView.Service;
using System.Windows.Threading;
namespace MingleView.UI.ViewModel
{
    public class ParticipantViewModel :  MeetingWorkspaceViewModel
    {
        #region Fields
        RelayCommand _AddParticipantCmd;
        ObservableCollection<Participant> _participantCollection = new ObservableCollection<Participant>();
        readonly IMeetingManager _meetingManager;
        private readonly Dispatcher _dispatcher; 
      
        #endregion //Fields

        #region Constructor
        public ParticipantViewModel(IMeetingManager meetingManager, Dispatcher dispatcher)
        {
            _meetingManager = meetingManager;
            _dispatcher= dispatcher; 
            //register the event handlers
            _meetingManager.MeetingEvents.ParticipantJoined += delegate(object sender, ParticipantEventArgs joinedRgs)
            {
                this._dispatcher.Invoke(
                    (Action)delegate { OnJoin(joinedRgs.ParticipantInfo ); },
                    null);
            };
            _meetingManager.MeetingEvents.ParticipantUpdated += delegate(object sender, ParticipantEventArgs updateRgs)
            {
                this._dispatcher.Invoke(
                    (Action)delegate { OnUpdate(updateRgs.ParticipantInfo); },
                    null);
            };
            _meetingManager.MeetingEvents.ParticipantDeparted += delegate(object sender, ParticipantEventArgs departRgs)
            {
                this._dispatcher.Invoke(
                    (Action)delegate { OnDepart(departRgs.ParticipantInfo); },
                    null);
            };

        }
        #endregion //Constructor

        public ObservableCollection<Participant> ParticipantCollection
        {
            get { return _participantCollection; }
        }

        #region Commands
        public ICommand AddParticipantCmd
        {
            get
            {
                if (_AddParticipantCmd == null)
                {
                    _AddParticipantCmd = new RelayCommand(param => this.AddParticipant());
                }
                return _AddParticipantCmd;
            }
        }
        private void AddParticipant()
        {
            //Testing purpose
  //          _participantCollection.Add(new Participant { Name = "Newly Added" });
        }
        #endregion //Commands
        #region Event Notifiers
        /*******
         * new guy joined
         * *****/
        void OnJoin(Participant joinedParticipant)
        {
            //isPresenter();
            joinedParticipant.Icon = Properties.Resources.Presenter;
            _participantCollection.Add(joinedParticipant);
        }
        /***************
         * Guy left
         * ***********/
        void OnDepart(Participant departedParticipant)
        {
            _participantCollection.Remove(departedParticipant);  
        }
        /************
         * partipant profile updated
         * ************/
        void OnUpdate(Participant updateParticipant)
        {
            //remove the old entry
            _participantCollection.Remove(updateParticipant);
            //add the new one
            _participantCollection.Add(updateParticipant);    
        }
        #endregion //event notifiers
    }
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Icon)
            {
                var stream = new MemoryStream();
                ((Icon)value).Save(stream);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.EndInit();

                return bitmap;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

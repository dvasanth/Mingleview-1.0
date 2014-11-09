using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows.Data;
namespace MingleView.Model
{
    [DataContract]
    [Serializable]
    public class ChatMessage : IEquatable<ChatMessage>
    {
        [DataMember]
        public string Text
        { get; set; }

        [DataMember]
        public Participant Source
        { get; set; }

        [DataMember]
        public DateTime UtcCreatedTime
        {get; private set;}

        public ChatMessage() { }

        public ChatMessage(ChatMessage msgCopy) 
        {
            this.Source = msgCopy.Source;
            this.Text = msgCopy.Text;
            this.UtcCreatedTime = msgCopy.UtcCreatedTime;
        }
        public ChatMessage(Participant source, string text, DateTime utcCreatedTime)
        {
            Source = source;
            Text = text;
            UtcCreatedTime = utcCreatedTime;
        }
        /*****************
         * objects differs only by UTC time
         * ************/
        public bool Equals(ChatMessage other)
        {
            return UtcCreatedTime.Equals(other.UtcCreatedTime);
        }
    }
    /***********************
     * get the participant name from source message
     * ********************/
    public class ParticipantNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Participant user = (Participant)value;
            return user.Name + ": "; //Format: User name, Colon and space...
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /*********************************
     * converts time to local
     * ********************************/
    public class MessageDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dtTime = (DateTime)value;
            return "[Sent at: " + dtTime.ToString() + "]"; //Format: [Sent at: datetime]
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

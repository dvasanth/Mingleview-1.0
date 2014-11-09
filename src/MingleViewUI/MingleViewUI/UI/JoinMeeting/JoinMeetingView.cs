using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MingleView.UI
{
    public partial class JoinMeetingView : Form
    {
        public string ParticipantName;
        public string MeetingTicket;
        public JoinMeetingView()
        {
            InitializeComponent();
        }

        private void buttonJoin_Click(object sender, EventArgs e)
        {
            ParticipantName = textParticipantName.Text;
            MeetingTicket = textMeetingTicket.Text;  
            Close(); 
        }
    }
}

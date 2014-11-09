using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MingleView.UI.ViewModel;

namespace MingleView.UI
{
    public class QuickStartMeetingViewModelEvntArgs : EventArgs
    {
        public QuickStartMeetingViewModelEvntArgs(QuickStartMeetingViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public QuickStartMeetingViewModel ViewModel { get; private set; }
    }
    public class JoinMeetingViewModelEvntArgs : EventArgs
    {
        public JoinMeetingViewModelEvntArgs(JoinMeetingViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public JoinMeetingViewModel ViewModel { get; private set; }
    }
    public class SendErrorReportViewModelEvntArgs : EventArgs
    {
        public SendErrorReportViewModelEvntArgs(SendErrorReportViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public SendErrorReportViewModel ViewModel { get; private set; }
    }
    
}

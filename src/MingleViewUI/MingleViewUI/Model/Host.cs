using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MingleView.Model
{
    public class Host:Participant
    {
   /*     public Host()
        {
        }*/
        public Host(string sName):base(sName)
        {
            ScreenPresentLevel = SCREEN_PRESENT_LEVEL.NONE;
            ScreenControlLevel = SCREEN_CTRL_LEVEL.CONTROL ;
            PrivilegeChangeLevel = PRIVILEGE_CHANGE_LEVEL.CHANGE_SCREEN_CTRL_LEVEL |
                                    PRIVILEGE_CHANGE_LEVEL.CHANGE_SCREEN_PRESENT_LEVEL;


        }
    }
}

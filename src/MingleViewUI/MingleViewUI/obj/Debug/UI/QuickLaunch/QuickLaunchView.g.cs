﻿#pragma checksum "..\..\..\..\UI\QuickLaunch\QuickLaunchView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "59BA7754D71278EA522FCE8CF56C08BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4952
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MingleView.UI.View {
    
    
    /// <summary>
    /// QuickLaunchView
    /// </summary>
    public partial class QuickLaunchView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\UI\QuickLaunch\QuickLaunchView.xaml"
        internal System.Windows.Controls.Label labelMeetingID;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\UI\QuickLaunch\QuickLaunchView.xaml"
        internal System.Windows.Controls.TextBox textBoxMeetingID;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\UI\QuickLaunch\QuickLaunchView.xaml"
        internal System.Windows.Controls.Button btnShareDesktops;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MingleView;component/ui/quicklaunch/quicklaunchview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UI\QuickLaunch\QuickLaunchView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.labelMeetingID = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.textBoxMeetingID = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.btnShareDesktops = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

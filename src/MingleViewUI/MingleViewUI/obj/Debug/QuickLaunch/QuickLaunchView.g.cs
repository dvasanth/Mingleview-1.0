﻿#pragma checksum "..\..\..\QuickLaunch\QuickLaunchView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B5318727A76035D1247726D0C9BDD63F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
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


namespace MingleViewUI.View {
    
    
    /// <summary>
    /// QuickLaunchView
    /// </summary>
    public partial class QuickLaunchView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\QuickLaunch\QuickLaunchView.xaml"
        internal System.Windows.Controls.Button btnStartPresentation;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\QuickLaunch\QuickLaunchView.xaml"
        internal System.Windows.Controls.Button btnShareApplication;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\QuickLaunch\QuickLaunchView.xaml"
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
            System.Uri resourceLocater = new System.Uri("/MingleViewUI;component/quicklaunch/quicklaunchview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\QuickLaunch\QuickLaunchView.xaml"
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
            this.btnStartPresentation = ((System.Windows.Controls.Button)(target));
            return;
            case 2:
            this.btnShareApplication = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.btnShareDesktops = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

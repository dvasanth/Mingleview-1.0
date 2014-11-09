﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using MingleView.UI.ViewModel;

namespace MingleView.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static MainWindowViewModel _mainWindowModel;
        SelfInstaller _installer= new SelfInstaller();
        protected override void OnStartup(StartupEventArgs e)
        {
             
            if(!_installer.Init(AppDomain.CurrentDomain))
            return;
            
            base.OnStartup(e);

            // Create the ViewModel to which 
            // the main window binds.
            _mainWindowModel = new MainWindowViewModel();
            MainWindow window = new MainWindow();
            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler handler = null;
            handler = delegate
            {
                _mainWindowModel.RequestClose -= handler;
                window.Close();
            };
            _mainWindowModel.RequestClose += handler;

            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            window.DataContext = _mainWindowModel;
            window.Show();
        }
        static public MainWindowViewModel MainWindowModel
        {
            get
            {
                return _mainWindowModel;
            }
            set
            {
                if (value == _mainWindowModel)
                    return;

                _mainWindowModel = value;

            }
        }
    
    }
}

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using app.History;
using app.models;
using app.models.Entity;
using C_sharb_voenmeh_coursework;
using C_sharb_voenmeh_coursework.Command;
using C_sharb_voenmeh_coursework.History;

namespace app
{
    public class MainApp: Command // Наследование BaseViewModel нужно так как там  INotifyPropertyChanged
    {
        #region Variebles
        
        #endregion

      

        #region Constructors

        public MainApp()
        {
            CallLeftPanel();
            OpenDirectory();
        }

        #endregion
        
    }
}
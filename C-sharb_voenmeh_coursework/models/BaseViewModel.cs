using C_sharb_voenmeh_coursework.Actions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace app.models
{
    public class BaseViewModel : INotifyPropertyChanged
    {


        //private bool _isEnabled;

        //public bool IsEnabled
        //{
        //    get { return _isEnabled; }
        //    set
        //    {
        //        _isEnabled = value;
        //        OnPropertyChanged("IsEnabled");
        //    }
        //}


        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Protected Methods
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
           
        }

     

        #endregion


    }
}
using C_sharb_voenmeh_coursework.Command;

namespace app
{
    public class MainApp : Command // Наследование BaseViewModel нужно так как там  INotifyPropertyChanged
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
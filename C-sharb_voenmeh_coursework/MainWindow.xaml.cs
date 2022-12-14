using System.Windows;
using System.Windows.Controls;
using app;

namespace C_sharb_voenmeh_coursework
{
    public partial class MainWindow : Window
    {
        private MainApp mainApp = new MainApp();

        public MainWindow()
        {
           
          //  mainApp.IsEnabled = true;
            DataContext = mainApp;
            InitializeComponent();


        }


}


}
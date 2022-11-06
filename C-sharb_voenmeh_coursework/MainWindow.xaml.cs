using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using app;
using GongSolutions.Wpf.DragDrop;


namespace C_sharb_voenmeh_coursework
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
          
            DataContext = new DragDrop();
           // DataContext = new MainApp();
            
            InitializeComponent();
           
        }
    }

   

        
}
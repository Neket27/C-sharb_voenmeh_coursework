﻿using System.Windows;
using System.Windows.Media;
using app;


namespace C_sharb_voenmeh_coursework
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainApp();

            InitializeComponent();
           
        }
    }
}
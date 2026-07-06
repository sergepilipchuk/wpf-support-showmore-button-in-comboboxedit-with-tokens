using System;
using System.Windows;
using System.Windows.Controls;

namespace MoreTokensApp
{
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyTimeTable.Views.LoginWindow
{
    /// <summary>
    /// Interaction logic for EnterCode.xaml
    /// </summary>
    public partial class EnterCode : Page
    {

        public static PasswordBox ma;
        public EnterCode()
        {
            InitializeComponent();
        }

        private void FloatingPasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            ma = FloatingPasswordBox;
        }
    }
}

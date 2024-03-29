﻿using EasyTimeTable.ViewModel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Window = System.Windows.Window;

namespace EasyTimeTable.Views.Student
{
    /// <summary>
    /// Interaction logic for StudentMainWindow.xaml
    /// </summary>
    public partial class StudentMainWindow : Window
    {

        public static ToggleButton Slidebtn;
        public static TextBlock funcTitle;
        public StudentMainWindow()
        {
            InitializeComponent();
        }

        private void MainFrame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SlideButton.IsChecked = false;
        }

        private void SlideButton_Checked(object sender, RoutedEventArgs e)
        {
            shadow.Visibility = Visibility.Visible;
        }

        private void SlideButton_Unchecked(object sender, RoutedEventArgs e)
        {
            shadow.Visibility = Visibility.Collapsed;
        }

        private void SlideButton_Loaded(object sender, RoutedEventArgs e)
        {
            Slidebtn = SlideButton;
        }

        private void FuncTitle_Loaded(object sender, RoutedEventArgs e)
        {
            funcTitle = FuncTitle;
        }

    }
}

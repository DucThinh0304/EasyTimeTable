using EasyTimeTable.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Xml.Linq;

namespace EasyTimeTable.Views.OpenCourse
{
    /// <summary>
    /// Interaction logic for OpenCoursePage.xaml
    /// </summary>
    public partial class OpenCoursePage : Page
    {
        public OpenCoursePage()
        {
            InitializeComponent();
        }

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab.SelectedIndex == 0)
            {
                Select.Visibility = Visibility.Visible;
                Selected.Visibility = Visibility.Hidden;
                Button.Content = "Đăng kí môn học";
            }
            else
            {
                Selected.Visibility = Visibility.Visible;
                Select.Visibility = Visibility.Hidden;
                Button.Content = "Hủy môn học";
            }
        }

       
    }
}

using EasyTimeTable.Model;
using EasyTimeTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace EasyTimeTable.Views.Student.Tuition
{
    /// <summary>
    /// Interaction logic for StudentTuitionPage.xaml
    /// </summary>
    public partial class StudentTuitionPage : Page
    {
        public OpenCourseModel current;

        public StudentTuitionPage()
        {
            InitializeComponent();
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            current = new OpenCourseModel();
            var item = sender as ListViewItem;
            if (item != null)
            {
                current = (OpenCourseModel)item.DataContext;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (current.MaHocPhan.Length == 9)
            {
                CustomYesNoDialog customYesNoDialog = new CustomYesNoDialog(current.MaHocPhan, current.TenMon, true);
                customYesNoDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Bạn không thể xóa môn thực hành");
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tab.TabIndex = 2;
        }
    }
}

using System;
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
using System.Windows.Shapes;

namespace EasyTimeTable.Views.Student.Course
{
    /// <summary>
    /// Interaction logic for RequestCoursesWindow.xaml
    /// </summary>
    public partial class RequestCoursesWindow : Window
    {
        public RequestCoursesWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Gui_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
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

namespace EasyTimeTable.Views.Staff
{
    /// <summary>
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        //Liên kết TogggleButton và Title với VM
        public static ToggleButton Slidebtn;
        public static TextBlock funcTitle;

        // Control cho Window
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

        public StaffWindow()
        {
            InitializeComponent();
        }

        private void selectFunc_Loaded(object sender, RoutedEventArgs e)
        {
            funcTitle = FuncTitle;
        }
    }
}

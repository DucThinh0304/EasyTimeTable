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
        private int start;
        private int end;
        public RequestCoursesWindow()
        {
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
                ComboBox.Items.Add(i.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox.SelectedIndex <= 4)
            {
                ComboBoxEnd.Items.Clear();
                for (int i = 2; i <= 5; i++)
                    ComboBoxEnd.Items.Add(i.ToString());
                ComboBoxEnd.SelectedIndex = 0;
            }
            if (ComboBox.SelectedIndex > 4)
            {
                ComboBoxEnd.Items.Clear();
                for (int i = 7; i <= 10; i++)
                {
                    ComboBoxEnd.Items.Add(i.ToString());
                }
                ComboBoxEnd.SelectedIndex = 0;
            }
                
        }

        private void ComboBoxEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

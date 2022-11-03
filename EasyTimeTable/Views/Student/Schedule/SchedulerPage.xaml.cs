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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyTimeTable.Views.Student.Calendar
{
    /// <summary>
    /// Interaction logic for SchedulerPage.xaml
    /// </summary>
    public partial class SchedulerPage : Page
    {
        public SchedulerPage()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN");
        }
        private void Thang_Click(object sender, RoutedEventArgs e)
        {
            Schedule.ViewType = Syncfusion.UI.Xaml.Scheduler.SchedulerViewType.Month;
        }
        private void Tuan_Click(object sender, RoutedEventArgs e)
        {
            Schedule.ViewType = Syncfusion.UI.Xaml.Scheduler.SchedulerViewType.Week;
        }
    }
}

using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
            CultureInfo culture = CultureInfo.CreateSpecificCulture("vi-vn");
            Thread.CurrentThread.CurrentCulture = culture;
            this.Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
            
        }
        private void Schedule_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {
            e.AppointmentEditorOptions = AppointmentEditorOptions.All | (~AppointmentEditorOptions.Reminder & ~AppointmentEditorOptions.Resource
                & ~AppointmentEditorOptions.TimeZone & ~AppointmentEditorOptions.Description);
        }



        private void ChangeType(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)   
            {
                case "Week":
                    {
                        Schedule.ViewType = SchedulerViewType.Week;
                        break;
                    }
                case "Month":
                    {
                        Schedule.ViewType = SchedulerViewType.Month;
                        break;
                    }
            }
        }
        
    }
}

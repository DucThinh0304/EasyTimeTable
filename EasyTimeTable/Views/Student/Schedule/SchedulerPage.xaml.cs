using EasyTimeTable.ViewModel;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
            this.Schedule.RecurringAppointmentBeginningEdit += scheduler_RecurringAppointmentBeginningEdit;
            this.Schedule.AppointmentEditorClosing += Schedule_AppointmentEditorClose;

        }
        private void Schedule_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {

            if (e.Appointment != null)
            {
                if (SchedulerViewModel.j < Convert.ToInt32(e.Appointment.Id))
                {
                    e.AppointmentEditorOptions = AppointmentEditorOptions.All | (~AppointmentEditorOptions.Reminder & ~AppointmentEditorOptions.Resource
                    & ~AppointmentEditorOptions.TimeZone & ~AppointmentEditorOptions.Description);
                }
                else
                {
                    MessageBox.Show("Không thể chỉnh sửa thời gian môn học");
                    e.Cancel = true;
                }
            }
            else
            {
                e.AppointmentEditorOptions = AppointmentEditorOptions.All | (~AppointmentEditorOptions.Reminder & ~AppointmentEditorOptions.Resource
                    & ~AppointmentEditorOptions.TimeZone & ~AppointmentEditorOptions.Description);
            }
        }
        private void Schedule_AppointmentEditorClose(object sender, AppointmentEditorClosingEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            if (e.Action == AppointmentEditorAction.Add)
            {
                var cmd = new SqlCommand("Insert into lich (ID, RecurrenceRule, StartTime, EndTime, AppointmentBackground, Foreground, Subject, Location, IsAllDay) values (@ID, @RecurrenceRule, @StartTime, @EndTime, @AppointmentBackground, @Foreground, @Subject, @Location, @IsAllDay)", con);
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int);
                cmd.Parameters["@ID"].Value = Convert.ToInt32(e.Appointment.Id);
                cmd.Parameters.Add("@RecurrenceRule", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@RecurrenceRule"].Value = e.Appointment.RecurrenceRule;
                cmd.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@StartTime"].Value = e.Appointment.StartTime;
                cmd.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@EndTime"].Value = e.Appointment.EndTime;
                cmd.Parameters.Add("@AppointmentBackground", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@AppointmentBackground"].Value = e.Appointment.AppointmentBackground.ToString();
                cmd.Parameters.Add("@Foreground", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@Foreground"].Value = e.Appointment.Foreground.ToString();
                cmd.Parameters.Add("@Subject", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@Subject"].Value = e.Appointment.Subject;
                cmd.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@Location"].Value = e.Appointment.Location;
                cmd.Parameters.Add("@IsAllDay", System.Data.SqlDbType.Int);
                cmd.Parameters["@IsAllDay"].Value = e.Appointment.IsAllDay;
                cmd.ExecuteNonQuery();
            }
            else if (e.Action == AppointmentEditorAction.Delete)
            {
                var cmd = new SqlCommand("Delete from lich where ID = '" + e.Appointment.Id + "'", con);
                cmd.ExecuteNonQuery();
            }
            else if (e.Action == AppointmentEditorAction.Edit)
            {
                var cmd = new SqlCommand("Update lich set RecurrenceRule = @RecurrenceRule, StartTime = @StartTime, EndTime = @EndTime, AppointmentBackground = @AppointmentBackground, Foreground = @Foreground, Subject = @Subject, Location = @Location, IsAllDay = @IsAllDay where Id = @ID", con);
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int);
                cmd.Parameters["@ID"].Value = Convert.ToInt32(e.Appointment.Id);
                cmd.Parameters.Add("@RecurrenceRule", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@RecurrenceRule"].Value = e.Appointment.RecurrenceRule;
                cmd.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@StartTime"].Value = e.Appointment.StartTime;
                cmd.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@EndTime"].Value = e.Appointment.EndTime;
                cmd.Parameters.Add("@AppointmentBackground", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@AppointmentBackground"].Value = e.Appointment.AppointmentBackground.ToString();
                cmd.Parameters.Add("@Foreground", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@Foreground"].Value = e.Appointment.Foreground.ToString();
                cmd.Parameters.Add("@Subject", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@Subject"].Value = e.Appointment.Subject;
                cmd.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@Location"].Value = e.Appointment.Location;
                cmd.Parameters.Add("@IsAllDay", System.Data.SqlDbType.Int);
                cmd.Parameters["@IsAllDay"].Value = e.Appointment.IsAllDay;
                cmd.ExecuteNonQuery();
            }
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
        public void Reminder()
        {
            
        }

        private void scheduler_RecurringAppointmentBeginningEdit(object sender, RecurringAppointmentBeginningEditEventArgs e)
        {
             e.EditMode = RecurringAppointmentEditMode.Series;
        }
    }
}

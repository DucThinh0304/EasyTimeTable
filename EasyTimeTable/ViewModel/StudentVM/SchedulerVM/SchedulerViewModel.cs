using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Media;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class SchedulerViewModel
    {
        public ScheduleAppointmentCollection scheduleAppointmentCollection { get; set; } = new ScheduleAppointmentCollection();
        public SchedulerViewModel()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select * from monhoc", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                var scheduleAppointment = new ScheduleAppointment()
                {
                    Id = 1,
                    StartTime = new DateTime(2022, 9, 5, 11, 0, 0),
                    EndTime = new DateTime(2022, 9, 5, 12, 0, 0),
                    Subject = "",
                    AppointmentBackground = Brushes.RoyalBlue,
                    Foreground = Brushes.White,
                };
                scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=15";
                scheduleAppointmentCollection.Add(scheduleAppointment);

            }
        }
    }
}

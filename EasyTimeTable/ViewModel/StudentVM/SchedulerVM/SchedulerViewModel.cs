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
            var cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien where  " +
                "lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon", con);
            var dr = cmd.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                i++;
                DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
                DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
                datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
                dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
                var scheduleAppointment = new ScheduleAppointment()
                {
                    Id = i,
                    StartTime = datestart,
                    EndTime = dateend,
                    Subject = "Môn học: " + dr.GetString(2) + "\nGiáo viên: " + dr.GetString(3),
                    AppointmentBackground = PickBrush(i),
                    Foreground = Brushes.White,
                };
                scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=15";
                scheduleAppointmentCollection.Add(scheduleAppointment);
            }
        }

        // Random Brush
        private Brush PickBrush(int i)
        {
            switch (i)
            {
                case 1:
                    return Brushes.Crimson;
                case 2:
                    return Brushes.Cyan;
                default:
                    return Brushes.Black;

            }
        }
    }
}

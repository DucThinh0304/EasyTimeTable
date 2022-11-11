using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class SchedulerViewModel
    {
        [ObservableProperty]
        public ScheduleAppointmentCollection scheduleAppointmentCollection;

        public ICommand LoadSchedulerCM { get; set; }

        public SchedulerViewModel()
        {

            LoadSchedulerCM = new RelayCommand<object>((p) => {
                ScheduleAppointmentCollection = new ScheduleAppointmentCollection();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien where  " +
                    "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon", con);
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
                    ScheduleAppointmentCollection.Add(scheduleAppointment);
                }
            });
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
                case 3:
                    return Brushes.Purple;
                case 4:
                    return Brushes.Orange;
                case 5:
                    return Brushes.Yellow;
                case 6:
                    return Brushes.SpringGreen;
                case 7:
                    return Brushes.Pink;
                default:
                    return Brushes.Black;

            }
        }
    }
}

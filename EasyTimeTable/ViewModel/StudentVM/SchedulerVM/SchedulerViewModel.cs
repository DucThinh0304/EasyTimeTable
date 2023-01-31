using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class SchedulerViewModel
    {
        [ObservableProperty]
        public ScheduleAppointmentCollection? scheduleAppointmentCollection;

        [ObservableProperty]
        private IEnumerable events;


        [ObservableProperty]
        private Visibility mask;

        private int i = 0;
        static public int j = 0;

        [ObservableProperty]
        private string countToday;


        private int count = 0;

        [ObservableProperty]
        private bool isLoading;

        SolidColorBrush[] listColor = new SolidColorBrush[]
        {
            new SolidColorBrush(Colors.MediumOrchid),
            new SolidColorBrush(Colors.ForestGreen),
            new SolidColorBrush(Colors.DodgerBlue),
            new SolidColorBrush(Colors.DarkOrange),
            new SolidColorBrush(Colors.Teal),
            new SolidColorBrush(Colors.YellowGreen),
            new SolidColorBrush(Colors.Firebrick),
            new SolidColorBrush(Colors.MediumBlue),
            new SolidColorBrush(Colors.SlateGray),
            new SolidColorBrush(Colors.BlueViolet),
            new SolidColorBrush(Colors.DarkSlateBlue),
            new SolidColorBrush(Colors.DeepPink),
            new SolidColorBrush(Colors.Pink)
        };

        [RelayCommand]
        private async Task Load()
        {
            i = 0;
            count = 0;
            Mask = Visibility.Visible;
            IsLoading = true;
            ScheduleAppointmentCollection = new ScheduleAppointmentCollection();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu, sophong, toa, lophocphansinhvien.mahocphan from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
                "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '" + LoginViewModel.mssv + "' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and lanhoc=1", con);
            var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                if (await dr.IsDBNullAsync(5) == false)
                {
                    if (dr.GetString(8).Length == 9)
                    {
                        for (int x = 0; x <= 11; x++)
                        {
                            i++;
                            DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2 + x * 7);
                            DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2 + x * 7);
                            datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
                            dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
                            var scheduleAppointment = new ScheduleAppointment()
                            {
                                Id = i,
                                StartTime = datestart,
                                EndTime = dateend,
                                Subject = "Môn học: " + dr.GetString(2) + "\nGiáo viên: " + dr.GetString(3) + "\nSố phòng: " + dr.GetString(7) + "." + dr.GetString(6) + "\nBuổi: " + (x + 1),
                                AppointmentBackground = listColor[(i - 1) / 12],
                                Foreground = Brushes.White,
                            };
                            scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=1";
                            ScheduleAppointmentCollection.Add(scheduleAppointment);
                        }
                    }
                    else
                    {
                        for (int x = 0; x <= 11; x++)
                        {
                            i++;
                            DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2 + x * 7);
                            DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2 + x * 7);
                            datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
                            dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
                            var scheduleAppointment = new ScheduleAppointment()
                            {
                                Id = i,
                                StartTime = datestart,
                                EndTime = dateend,
                                Subject = "Môn học: " + dr.GetString(2) + " (thực hành)\nGiáo viên: " + dr.GetString(3) + "\nSố phòng: " + dr.GetString(7) + "." + dr.GetString(6) + "\nBuổi: " + (x + 1),
                                AppointmentBackground = listColor[(i - 1) / 12],
                                Foreground = Brushes.White,
                            };
                            scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=1";
                            ScheduleAppointmentCollection.Add(scheduleAppointment);
                        }
                    }
                }
            }
            dr.Close();
            cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu, sophong, toa, lophocphansinhvien.mahocphan from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
                "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '" + LoginViewModel.mssv + "' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and lanhoc>1", con);
            dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                if (await dr.IsDBNullAsync(5) == false)
                {
                    if (dr.GetString(8).Length == 9)
                    {
                        for (int x = 0; x <= 11; x++)
                        {
                            i++;
                            DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2 + x * 7);
                            DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2 + x * 7);
                            datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
                            dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
                            var scheduleAppointment = new ScheduleAppointment()
                            {
                                Id = i,
                                StartTime = datestart,
                                EndTime = dateend,
                                Subject = "Môn học: " + dr.GetString(2) + "\nGiáo viên: " + dr.GetString(3) + "\nSố phòng: " + dr.GetString(7) + "." + dr.GetString(6) + "\nBuổi: " + (x + 1),
                                AppointmentBackground = listColor[(i - 1) / 12],
                                Foreground = Brushes.OrangeRed,
                                
                            };
                            scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=1";
                            ScheduleAppointmentCollection.Add(scheduleAppointment);
                        }
                    }
                    else
                    {
                        for (int x = 0; x <= 11; x++)
                        {
                            i++;
                            DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2 + x * 7);
                            DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2 + x * 7);
                            datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
                            dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
                            var scheduleAppointment = new ScheduleAppointment()
                            {
                                Id = i,
                                StartTime = datestart,
                                EndTime = dateend,
                                Subject = "Môn học: " + dr.GetString(2) + " (thực hành)\nGiáo viên: " + dr.GetString(3) + "\nSố phòng: " + dr.GetString(7) + "." + dr.GetString(6) + "\nBuổi: " + (x + 1),
                                AppointmentBackground = listColor[(i - 1) / 12],
                                Foreground = Brushes.OrangeRed,
                            };
                            scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=1";
                            ScheduleAppointmentCollection.Add(scheduleAppointment);
                        }
                    }
                }
            }
            j = i;
            dr.Close();
            cmd = new SqlCommand("Select * from lich", con);
            dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                var scheduleAppointment = new ScheduleAppointment()
                {
                    Id = dr.GetInt32(0),
                    StartTime = dr.GetDateTime(2),
                    EndTime = dr.GetDateTime(3),
                    Subject = dr.GetString(6),
                    AppointmentBackground = (SolidColorBrush)new BrushConverter().ConvertFrom(dr.GetString(4)),
                    Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(dr.GetString(5)),
                    Location = dr.GetString(7),
                    IsAllDay = (dr.GetInt32(8) == 0) ? false : true,
                };
                scheduleAppointment.RecurrenceRule = dr.GetString(1);
                ScheduleAppointmentCollection.Add(scheduleAppointment);
            }

            foreach (var scheduleAppointment in ScheduleAppointmentCollection)
            {
                if (scheduleAppointment.StartTime > DateTime.Today & scheduleAppointment.EndTime < DateTime.Today.AddDays(1))
                    count++;
            }
            CountToday = "Số sự kiện hôm nay: " + count + " sự kiện";
            Mask = Visibility.Collapsed;
            IsLoading = false;

        }
    }
}

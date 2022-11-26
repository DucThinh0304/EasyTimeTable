using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
        public ScheduleAppointmentCollection? scheduleAppointmentCollectionReal;

        //public ICommand LoadSchedulerCM { get; set; }
        //public ICommand LoadSchedulerRealCM { get; set; }
        public ICommand LoadOnDemandCommand { get; set; }

        [ObservableProperty]
        private Visibility mask;

        [ObservableProperty]
        private bool isLoading;

        public SchedulerViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                this.LoadOnDemandCommand = new DelegateCommand(ExecuteOnDemandLoading, CanExecuteOnDemandLoading);
            }
            IsLoading = false;
            Mask = Visibility.Collapsed;
            //LoadSchedulerCM = new RelayCommand<object>(async (p) =>
            //{
            //    Mask = Visibility.Visible;
            //    IsLoading = true;
            //    ScheduleAppointmentCollection = new ScheduleAppointmentCollection();
            //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //    con.Open();
            //    var cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
            //        "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '20520782' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam ", con);
            //    var dr = await cmd.ExecuteReaderAsync();
            //    int i = 0;
            //    while (await dr.ReadAsync())
            //    {
            //        i++;
            //        DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
            //        DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
            //        datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
            //        dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
            //        var scheduleAppointment = new ScheduleAppointment()
            //        {
            //            Id = i,
            //            StartTime = datestart,
            //            EndTime = dateend,
            //            Subject = "Môn học: " + dr.GetString(2) + "\nGiáo viên: " + dr.GetString(3),
            //            AppointmentBackground = PickBrush(i),
            //            Foreground = Brushes.White,
            //        };
            //        scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=15";
            //        ScheduleAppointmentCollection.Add(scheduleAppointment);
            //    }
            //    dr.Close();
            //    cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
            //       "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '20520782' and (thamso.ki <> hocphan.ky or thamso.namhoc <> hocphan.nam) and daduyet = 1", con);
            //    dr = await cmd.ExecuteReaderAsync();
            //    while (await dr.ReadAsync())
            //    {
            //        i++;
            //        DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
            //        DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
            //        datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
            //        dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
            //        var scheduleAppointment = new ScheduleAppointment()
            //        {
            //            Id = i,
            //            StartTime = datestart,
            //            EndTime = dateend,
            //            Subject = "Môn học: " + dr.GetString(2) + "\nGiáo viên: " + dr.GetString(3),
            //            AppointmentBackground = Brushes.Black,
            //            Foreground = Brushes.White,
            //        };
            //        scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=15";
            //        ScheduleAppointmentCollection.Add(scheduleAppointment);
            //    }
            //    IsLoading = false;
            //    Mask = Visibility.Collapsed;
            //});

            //LoadSchedulerRealCM = new RelayCommand<object>(async (p) =>
            //{
            //    Mask = Visibility.Visible;
            //    IsLoading = true;
            //    ScheduleAppointmentCollectionReal = new ScheduleAppointmentCollection();
            //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //    con.Open();
            //    var cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
            //        "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '20520782' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and daduyet = 1", con);
            //    var dr = await cmd.ExecuteReaderAsync();
            //    int i = 0;
            //    while (await dr.ReadAsync())
            //    {
            //        i++;
            //        DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
            //        DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
            //        datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
            //        dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
            //        var scheduleAppointment = new ScheduleAppointment()
            //        {
            //            Id = i,
            //            StartTime = datestart,
            //            EndTime = dateend,
            //            Subject = "Môn học: " + dr.GetString(2) + "\nGiáo viên: " + dr.GetString(3),
            //            AppointmentBackground = PickBrush(i),
            //            Foreground = Brushes.White,
            //        };
            //        scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=15";
            //        ScheduleAppointmentCollectionReal.Add(scheduleAppointment);
            //    }
            //    dr.Close();
            //    cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
            //       "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '20520782' and (thamso.ki <> hocphan.ky or thamso.namhoc <> hocphan.nam) and daduyet = 1", con);
            //    dr = await cmd.ExecuteReaderAsync();
            //    while (await dr.ReadAsync())
            //    {
            //        i++;
            //        DateTime datestart = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
            //        DateTime dateend = dr.GetDateTime(0).AddDays(dr.GetInt32(5) - 2);
            //        datestart += Converter.Converter.StartTime(dr.GetString(4)).ToTimeSpan();
            //        dateend += Converter.Converter.EndTime(dr.GetString(4)).ToTimeSpan();
            //        var scheduleAppointment = new ScheduleAppointment()
            //        {
            //            Id = i,
            //            StartTime = datestart,
            //            EndTime = dateend,
            //            Subject = "Môn học: " + dr.GetString(2) + "\nGiáo viên: " + dr.GetString(3),
            //            AppointmentBackground = Brushes.Black,
            //            Foreground = Brushes.White,
            //        };
            //        scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=15";
            //        ScheduleAppointmentCollectionReal.Add(scheduleAppointment);
            //    }
            //    IsLoading = false;
            //    Mask = Visibility.Collapsed;
            //});
        }

        private bool CanExecuteOnDemandLoading(object sender)
        {
            return true;
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

        public async void ExecuteOnDemandLoading(object parameter)
        {
            if (parameter == null)
            {
                return;
            }

            IsLoading = true;
            Mask = Visibility.Visible;
            await Task.Delay(500);
            await Application.Current.MainWindow.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(async () =>
            {
                await GenerateSchedulerAppointments();
            }));
            IsLoading = false;
            Mask = Visibility.Collapsed;
        }

        private async Task GenerateSchedulerAppointments()
        {
            ScheduleAppointmentCollection = new ScheduleAppointmentCollection();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
                "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '20520782' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam ", con);
            var dr = await cmd.ExecuteReaderAsync();
            int i = 0;
            while (await dr.ReadAsync())
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
            dr.Close();
            cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
               "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '20520782' and (thamso.ki <> hocphan.ky or thamso.namhoc <> hocphan.nam) and daduyet = 1", con);
            dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
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
                    AppointmentBackground = Brushes.Black,
                    Foreground = Brushes.White,
                };
                scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=15";
                ScheduleAppointmentCollection.Add(scheduleAppointment);
            }
            await GenerateSchedulerAppointments2();
        }
        private async Task GenerateSchedulerAppointments2()
        {
            ScheduleAppointmentCollectionReal = new ScheduleAppointmentCollection();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
                "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '20520782' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and daduyet = 1", con);
            var dr = await cmd.ExecuteReaderAsync();
            int i = 0;
            while (await dr.ReadAsync())
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
                ScheduleAppointmentCollectionReal.Add(scheduleAppointment);
            }
            dr.Close();
            cmd = new SqlCommand("Select ngaybatdau, ngayketthuc, tenmon, tengv, tiethoc, thu from hocphan, lophocphansinhvien, monhoc, giaovien, thamso where  " +
               "lophocphansinhvien.mahocphan = hocphan.mahocphan and giaovien.magv = hocphan.magv and hocphan.mamon = monhoc.mamon and masv = '20520782' and (thamso.ki <> hocphan.ky or thamso.namhoc <> hocphan.nam) and daduyet = 1", con);
            dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
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
                    AppointmentBackground = Brushes.Black,
                    Foreground = Brushes.White,
                };
                scheduleAppointment.RecurrenceRule = "FREQ=DAILY;INTERVAL=7;COUNT=15";
                ScheduleAppointmentCollectionReal.Add(scheduleAppointment);
            }
        }
    }
}

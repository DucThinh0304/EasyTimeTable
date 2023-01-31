using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class ListRequestVM
    {
        private string mssv;

        [ObservableProperty]
        public ObservableCollection<Request> requestList;

        [ObservableProperty]
        public ObservableCollection<Request> requestTakeList;
        public ListRequestVM()
        {
            mssv = LoginViewModel.mssv;
            RequestList = new ObservableCollection<Request>();
            RequestTakeList = new ObservableCollection<Request>();
        }
        public string getBuoi(int t)
        {
            if (t == 1) return "Buổi sáng";
            else return "Buổi chiều";
        }

        [RelayCommand]
        private void LoadList()
        {
            RequestList.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select yeucaumolop.mamon, tenmon, sotclt, sotcth, buoi, thu, tengv, siso, yeucaumolop.mayeucau from monhoc, yeucaumolop, giaovien where magvdexuat = magv and " +
                "yeucaumolop.MAYEUCAU NOT IN (Select MAYC from sinhvienyeucau where masv ='" + mssv + "') and yeucaumolop.mamon = monhoc.mamon", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                RequestList.Add(new Request
                {
                    MaMon = dr.GetString(0),
                    TenMon = dr.GetString(1),
                    SoTC = dr.GetInt32(2) + dr.GetInt32(3),
                    Buoi = getBuoi(dr.GetInt32(4)),
                    Thu = dr.GetInt32(5),
                    TenGV = dr.GetString(6),
                    SiSo = Count(dr.GetString(8)) + "/" + dr.GetInt32(7),
                    MaYeuCau = dr.GetString(8)
                });
            }
        }
        [RelayCommand]
        private void LoadRequest()
        {
            RequestTakeList.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select yeucaumolop.mamon, tenmon, sotclt, sotcth, buoi, thu, tengv, siso, yeucaumolop.mayeucau from monhoc, yeucaumolop, giaovien, sinhvienyeucau where " +
                "magvdexuat = magv and yeucaumolop.MAYEUCAU = sinhvienyeucau.mayc and masv ='" + mssv + "' and sinhvienyeucau.mayc = yeucaumolop.mayeucau and yeucaumolop.mamon = monhoc.mamon", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                RequestTakeList.Add(new Request
                {
                    MaMon = dr.GetString(0),
                    TenMon = dr.GetString(1),
                    SoTC = dr.GetInt32(2) + dr.GetInt32(3),
                    Buoi = getBuoi(dr.GetInt32(4)),
                    Thu = dr.GetInt32(5),
                    TenGV = dr.GetString(6),
                    SiSo = Count(dr.GetString(8)) + "/" + dr.GetInt32(7),
                    MaYeuCau = "YC" + dr.GetString(8)
                });
            }
        }

        private int Count(string mayc)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select count(*) from sinhvienyeucau where mayc = '" + mayc + "'", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read()) return dr.GetInt32(0);
            else return 0;
        }
    }
}

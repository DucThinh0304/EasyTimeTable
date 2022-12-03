using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class ListRequestVM
    {
        private string mssv;

        [ObservableProperty]
        public ObservableCollection<Request> requestList;

        public ICommand LoadListCommand { get; set; }
        public ListRequestVM()
        {
            mssv = LoginViewModel.mssv;
            RequestList = new ObservableCollection<Request>();

            LoadListCommand = new RelayCommand<object>((p) =>
            {
                RequestList.Clear();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("Select yeucaumolop.mamon, tenmon, sotclt, sotcth, buoi, thu, tengv, siso, yeucaumolop.mayeucau from monhoc, yeucaumolop, buoiyeucau, giaovien where yeucaumolop.mayeucau = buoiyeucau.mayeucau and " +
                    "magvdexuat = magv and yeucaumolop.MAYEUCAU NOT IN (Select mayeucau from sinhvienyeucau where masinhvien ='" + mssv + "') and yeucaumolop.mamon = monhoc.mamon", con);
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
                        SiSo = dr.GetInt32(7),
                        MaYeuCau = dr.GetString(8)
                    });
                }
            });
        }
        public string getBuoi(int t)
        {
            if (t == 1) return "Buổi sáng";
            else return "Buổi chiều";
        }
    }
}

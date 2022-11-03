using CommunityToolkit.Mvvm.ComponentModel;
using EasyTimeTable.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class TuitionViewModel
    {
        [ObservableProperty]
        private string? soTinChi;



        private int TC;
        
        public TuitionViewModel()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT sum(sotclt) + sum(sotcth) from monhoc, sinhvienmonhoc where monhoc.mamon = sinhvienmonhoc.mamon AND masv = '20520782'", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TC = dr.GetInt32(0);
            }
            dr.Close();
            SoTinChi = "Số tín chỉ đã đăng ký: " + TC;
        }
    }
}

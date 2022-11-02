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
        
        public TuitionViewModel()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                
            }
            dr.Close();
            SoTinChi = "Số tín chỉ đã đăng ký: " + "24";
        }
    }
}

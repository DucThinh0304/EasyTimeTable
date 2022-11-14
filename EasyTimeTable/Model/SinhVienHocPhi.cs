using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTimeTable.Model
{
    public partial class SinhVienHocPhi
    {
        public int STT { get; set; }
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string Lop { get; set; }
        public int HocPhi { get; set; }  
        public int HocPhiDaDong { get; set; }
        public string TinhTrangHocPhi { get; set; }

    }
}

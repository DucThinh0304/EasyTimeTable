using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTimeTable.Model
{
    public class Request
    {

        public string MaYeuCau { get; set; }
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public int SoTC { get; set; }

        public string Buoi { get; set; }

        public int Thu { get; set; }    

        public string TenGV { get; set; }
        public int SiSo { get; set; }
    }
}

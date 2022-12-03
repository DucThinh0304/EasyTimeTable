using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTimeTable.Model
{
    public partial class YeuCauModel
    {
        public string MaYeuCau { get; set; }
        public string MaMon { get; set; }
        public int SiSo { get; set; }
        public string MaGV { get; set; }
        public string LyDo { get; set; }
        public List<Buoi> listBuoi { get; set; }
    }
}

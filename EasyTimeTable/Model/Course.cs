﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTimeTable.Model
{
    public class Course
    {
        public int STT { get; set; }
        public string? MaHocPhan { get; set; }
        public string? TenMon { get; set; }
        public string? TenGV { get; set; }
        public int Nam { get; set; }
        public int Ki { get; set; }
        public string? SoPhong { get; set; }
        public string? Toa { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? TietHoc { get; set; }
        public int Thu { get; set; }
        public int SiSo { get; set; }
        public int SoTinChi { get; set; }
        public bool IsSignUp { get; set; }
        public string LanHoc { get; set; }
        public int SDK { get; set; }
    }
}

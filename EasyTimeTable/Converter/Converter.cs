using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTimeTable.Converter
{
    public partial class Converter
    {
        public static TimeOnly StartTime(string a)
        {

         var groups = a.Select((c, ix) => new { Char = c, Index = ix })
        .GroupBy(x => x)
        .Select(g => String.Concat(g.Select(x => x.Char)));
            string result = string.Join(",", groups);
            var numbers = result?.Split(",")?.Select(Int32.Parse)?.ToList();
            int min = numbers[0];
            switch (min)
            {
                case 1:
                    return new TimeOnly(7, 30, 00);
                case 2:
                    return new TimeOnly(8, 15, 00);
                case 3:
                    return new TimeOnly(9, 00, 00);
                case 4:
                    return new TimeOnly(10, 00, 00);
                case 5:
                    return new TimeOnly(10, 45, 00);
                case 6:
                    return new TimeOnly(13, 00, 00);
                case 7:
                    return new TimeOnly(13, 45, 00);
                case 8:
                    return new TimeOnly(14, 30, 00);
                case 9:
                    return new TimeOnly(15, 30, 00);
                case 0:
                    return new TimeOnly(16, 15, 00);
                default:
                    return new TimeOnly(00, 00, 00);
            }
        }
        public static TimeOnly EndTime(string a)
        {

            var groups = a.Select((c, ix) => new { Char = c, Index = ix })
           .GroupBy(x => x)
           .Select(g => String.Concat(g.Select(x => x.Char)));
            string result = string.Join(",", groups);
            var numbers = result?.Split(",")?.Select(Int32.Parse)?.ToList();
            int max = numbers[numbers.Count - 1];
            switch (max)
            {
                case 1:
                    return new TimeOnly(8, 15, 00);
                case 2:
                    return new TimeOnly(9, 00, 00);
                case 3:
                    return new TimeOnly(9, 45, 00);
                case 4:
                    return new TimeOnly(10, 45, 00);
                case 5:
                    return new TimeOnly(11, 30, 00);
                case 6:
                    return new TimeOnly(13, 45, 00);
                case 7:
                    return new TimeOnly(14, 30, 00);
                case 8:
                    return new TimeOnly(15, 30, 00);
                case 9:
                    return new TimeOnly(16, 15, 00);
                case 0:
                    return new TimeOnly(17, 00, 00);
                default:
                    return new TimeOnly(00, 00, 00);
            }
        }
        // Hàm so sánh nếu 2 khung thời gian có trùng nhau không
        public static bool Compare(string t1, string t2, int d1, int d2)
        {
            // true nếu không trùng thứ
            if (d1 != d2)
            {
                return true;
            }
            // true nếu trùng thứ nhưng không trùng tiết
            else
            {
                var dChar = CommonChars(t1, t2);
                if (dChar == 0) return true;
            }
            return false;
        }
        // Trả về số lượng kí tự trùng trong 2 string
        public static int CommonChars(string left, string right)
        {
            return left.GroupBy(c => c)
                .Join(
                    right.GroupBy(c => c),
                    g => g.Key,
                    g => g.Key,
                    (lg, rg) => lg.Zip(rg, (l, r) => l).Count())
                .Sum();
        }
        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
    }
}

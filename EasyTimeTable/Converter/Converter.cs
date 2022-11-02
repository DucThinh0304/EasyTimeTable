using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTimeTable.Converter
{
    public class Converter
    {
        public static TimeOnly StartTime(string a)
        {

            var groups = a.Select((c, ix) => new { Char = c, Index = ix })
        .GroupBy(x => x)
        .Select(g => String.Concat(g.Select(x => x.Char)));
            string result = string.Join(",", groups);
            var numbers = result?.Split(",")?.Select(Int32.Parse)?.ToList();
            int min = numbers.Min();
            switch (min)
            {
                case 1:
                    return new TimeOnly(7, 30, 00);
                    break;
                case 2:
                    return new TimeOnly(8, 15, 00);
                    break;
                case 3:
                    return new TimeOnly(9, 00, 00);
                    break;
                case 4:
                    return new TimeOnly(10, 00, 00);
                    break;
                case 5:
                    return new TimeOnly(10, 45, 00);
                    break;
                default:
                    return new TimeOnly(00, 00, 00);
            }
        }
    }
}

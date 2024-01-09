using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableSpeedTest
{
    public static class Globals
    {
        public static string DateFormat { get; } = "yyyy MMM dd - HH:mm:ss";

        public static string IntegerFormat { get; } = "#,###,##0";
        public static string Decimal1Format { get; } = "#,###,##0.#";
        public static string Decimal2Format { get; } = "#,###,##0.0#";

        public static string Decimal3Format { get; } = "#,###,##0.0##";
    }
}

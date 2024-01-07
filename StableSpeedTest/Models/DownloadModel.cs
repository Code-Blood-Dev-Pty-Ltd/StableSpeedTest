using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace StableSpeedTest.Models
{
    public class Downloads: INullable
    {
        public Downloads()
        {
            Items = new List<Download>();
        }

        public List<Download> Items { get; set; }

        [JsonIgnore]
        public bool IsNull { get; }
    }

    public class Download : INullable
    {
        public Download()
        {
            Histories = new SpeedHistories();
            Url = string.Empty;
        }

        public string Url { get; set; }

        public SpeedHistories Histories { get; set; }

        [JsonIgnore]
        public double AverageBandwidthMbps
        {
            get
            {
                return Histories.Items.Average(x => x.BandwidthMbps);
            }
        }

        [JsonIgnore]
        public bool IsNull { get; }
    }
}

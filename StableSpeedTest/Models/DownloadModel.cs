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
    public class DownloadModels: INullable
    {
        public DownloadModels()
        {
            Items = new List<DownloadModel>();
        }

        public List<DownloadModel> Items { get; set; }

        [JsonIgnore]
        public bool IsNull { get; }
    }

    public class DownloadModel : INullable
    {
        public DownloadModel()
        {
            Histories = new SpeedHistoryModels();
            Url = string.Empty;
        }

        public string Url { get; set; }

        public SpeedHistoryModels Histories { get; set; }

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

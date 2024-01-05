using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableSpeedTest.Models
{
    public class SpeedHistoryModels : INullable
    {
        public SpeedHistoryModels()
        {
            Items = new List<SpeedHistoryModel>();
        }

        [JsonIgnore]
        public bool IsNull { get; }

        public List<SpeedHistoryModel> Items { get; set; }

        [JsonIgnore]
        public double AverageBandwidthMbps
        {
            get
            {
                return Items.Average(x => x.BandwidthMbps);
            }
        }

        [JsonIgnore]
        public double AverageTimeInSeconds
        {
            get
            {
                return Items.Average(x => x.TimeInSeconds);
            }
        }
    }

    public class SpeedHistoryModel : INullable
    {
        public SpeedHistoryModel()
        {
            TestedEvent = DateTime.Now;
        }

        public DateTime TestedEvent { get; set; }

        [JsonIgnore]
        public bool IsNull { get; }

        public long FileSizeInBytes { get; set; }
        public double TimeInSeconds { get; set; }
        public double BandwidthMbps
        {
            get
            {
                return (FileSizeInBytes * 8) / (TimeInSeconds * 1000000);
            }
        }
    }
}

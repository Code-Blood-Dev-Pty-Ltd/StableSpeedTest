using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableSpeedTest.Models
{
    public class DownloadProgress
    {
        public static DownloadProgress New(int progress, double speed)
        {
            var result = new DownloadProgress();
            result.Progress = progress;
            result.Speed = speed;
            return result;
        }

        public int Progress { get; set; }
        public double Speed { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace StableSpeedTest.Models
{
    /// <summary>
    /// Represents the result of a speed test.
    /// </summary>
    public class SpeedTestResult
    {
        [JsonIgnore]
        public int RowId { get; set; }

        /// <summary>
        /// Gets or sets the URL used for the speed test.
        /// </summary>
        public string URL { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the duration of the speed test.
        /// </summary>
        public TimeSpan TimeDuration { get; set; }

        /// <summary>
        /// Gets or sets the exception that occurred during the speed test, if any.
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Gets or sets the size of the downloaded file in bytes.
        /// </summary>
        public long DownloadSizeInBytes { get; set; }

        /// <summary>
        /// Gets the size of the downloaded file in megabytes.
        /// </summary>
        public double DownloadSizeInMB => !IsFaulted ? DownloadSizeInBytes / 1048576.0 : 0;

        /// <summary>
        /// Gets the bandwidth in Mbps (megabits per second) for the speed test.
        /// </summary>
        public double BandwidthMbps => !IsFaulted ? (DownloadSizeInBytes * 8) / (TimeDuration.TotalSeconds * 1000000) : 0;

        /// <summary>
        /// Gets a value indicating whether the speed test encountered an exception.
        /// </summary>
        public bool IsFaulted => Exception != null && Exception.GetType() != typeof(TaskCanceledException);

        /// <summary>
        /// Gets a value indicating whether the speed test was cancelled.
        /// </summary>
        public bool IsCancelled => Exception is TaskCanceledException;

        /// <summary>
        /// Logs the specified exception for the speed test.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        internal void ExceptionLog(Exception ex)
        {
            Exception = ex;
        }
    }
}

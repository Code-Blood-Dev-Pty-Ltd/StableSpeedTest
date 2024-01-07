using StableSpeedTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StableSpeedTest.Routines
{
    /// <summary>
    /// Provides HTTP Routines, such as download file, etc.
    /// </summary>
    public static class HTTPRoutines
    {
        /// <summary>
        /// Downloads a file from the specified URL using the provided HttpClient.
        /// </summary>
        /// <param name="httpClient">The HttpClient instance used to make the request.</param>
        /// <param name="url">The URL of the file to download.</param>
        /// <param name="bufferSize">The size of the buffer used for reading the response stream. Default is 4096 bytes.</param>
        /// <param name="progress">An optional progress object to report the download progress. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the download operation. Default is null.</param>
        /// <returns>A Task that represents the asynchronous download operation. The task result is a SpeedTestResult object containing information about the download.</returns>
        public static async Task<SpeedTestResult> DownloadFile(HttpClient httpClient, string url, int bufferSize = 4096, IProgress<DownloadProgress>? progress = default, CancellationToken cancellationToken = default)
        {
            var result = new SpeedTestResult();

            try
            {
                httpClient.CancelPendingRequests();

                var stopwatch = Stopwatch.StartNew();
                var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                var content = response.Content;
                var totalBytes = content.Headers.ContentLength ?? -1;
                var buffer = new byte[bufferSize];
                var totalBytesRead = 0;

                using (var stream = await content.ReadAsStreamAsync())
                {
                    int bytesRead;
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                    {
                        totalBytesRead += bytesRead;

                        if (progress != null)
                        {
                            progress.Report(DownloadProgress.New((int)((double)totalBytesRead / totalBytes * 100), (totalBytesRead * 8) / (stopwatch.Elapsed.TotalSeconds * 1000000)));
                        }
                    }
                }

                stopwatch.Stop();

                result.TimeDuration = stopwatch.Elapsed;
                result.URL = url;
                result.DownloadSizeInBytes = totalBytesRead;
            }
            catch (HttpRequestException ex)
            {
                result.ExceptionLog(ex);
            }
            catch (TaskCanceledException ex)
            {
                result.ExceptionLog(ex);
            }
            catch (Exception ex)
            {
                result.ExceptionLog(ex);
            }

            return result;
        }
    }
}

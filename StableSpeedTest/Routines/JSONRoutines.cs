using Newtonsoft.Json;
using StableSpeedTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableSpeedTest.Routines
{
    /// <summary>
    /// Provides JSON Serialization Routines.
    /// </summary>
    internal static class JSONRoutines
    {
        /// <summary>
        /// Writes the specified DownloadModel object to a JSON file.
        /// </summary>
        /// <param name="f">The file path.</param>
        /// <param name="m">The DownloadModel object.</param>
        public static void WriteToJsonFile(string f, Download m)
        {
            string json = JsonConvert.SerializeObject(m, Formatting.Indented);
            File.WriteAllText(f, json);
        }

        /// <summary>
        /// Writes the specified DownloadModels object to a JSON file.
        /// </summary>
        /// <param name="f">The file path.</param>
        /// <param name="m">The DownloadModels object.</param>
        public static void WriteToJsonFile(string f, Downloads m)
        {
            string json = JsonConvert.SerializeObject(m, Formatting.Indented);
            File.WriteAllText(f, json);
        }

        /// <summary>
        /// Parses the specified JSON string to a DownloadModels object.
        /// </summary>
        /// <param name="j">The JSON string.</param>
        /// <returns>The parsed DownloadModels object.</returns>
        public static Downloads ParseJsonToModels(string j)
        {
            if (!string.IsNullOrEmpty(j))
            {
                return JsonConvert.DeserializeObject<Downloads>(j);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parses the specified JSON string to a DownloadModel object.
        /// </summary>
        /// <param name="j">The JSON string.</param>
        /// <returns>The parsed DownloadModel object.</returns>
        public static Download ParseJsonToModel(string j)
        {
            if (!string.IsNullOrEmpty(j))
            {
                return JsonConvert.DeserializeObject<Download>(j);
            }
            else
            {
                return null;
            }
        }
    }
}

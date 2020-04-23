using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MusicTagEditor
{
    /// <summary>
    /// Manages files
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// The full path to the file
        /// </summary>
        private string _filepath;

        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The full path to the file with the new name
        /// </summary>
        public string NewFilePath { get; private set; }

        /// <summary>
        /// The track number
        /// </summary>
        public string TrackNumber { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="filePath">The full path to the file</param>
        public FileManager(string filePath) => _filepath = filePath;

        /// <summary>
        /// Parse the file name
        /// </summary>
        public void ParseFile()
        {
            // Get the file and parse it
            var filename = Path.GetFileName(_filepath);
            var fileParts = filename.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // Get the track number and the file name
            TrackNumber = fileParts.FirstOrDefault();
            FileName = string.Join("'", fileParts.Skip(1).SkipLast(1));

            // Calculate the new file path
            NewFilePath = Path.Combine(Path.GetDirectoryName(_filepath), $"{FileName}{Path.GetExtension(_filepath)}");
        }
    }
}

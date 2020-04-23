using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MusicTagEditor
{
    /// <summary>
    /// Tags the files
    /// </summary>
    public class FileTagger
    {
        /// <summary>
        /// The command line options
        /// </summary>
        private Options _options;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options">The command line arguments</param>
        public FileTagger(Options options) => _options = options;

        /// <summary>
        /// Update the file tags
        /// </summary>
        public void TagFiles()
        {
            // Get all mp3 files in current directory
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mp3");
            uint trackNumber = 1;
            files.ToList().ForEach(filePath =>
            {
                // Parse the file
                var fileInfo = new FileManager(filePath);
                fileInfo.ParseFile();

                // Set the album
                var file = TagLib.File.Create(filePath);
                file.Tag.Album = _options.Album ?? file.Tag.Album;

                // Set the contributing artist
                file.Tag.AlbumArtists = (!string.IsNullOrWhiteSpace(_options.ContributingArtist)) ? new string[] { _options.ContributingArtist }
                : file.Tag.AlbumArtists;

                // Set the genre
                file.Tag.Genres = (!string.IsNullOrWhiteSpace(_options.Genre)) ? new string[] { _options.Genre } : file.Tag.Genres;

                // Update the title and the track number
                file.Tag.Title = fileInfo.FileName;
                if(_options.UseTrackNumber)
                {
                    file.Tag.Track = (string.IsNullOrWhiteSpace(fileInfo.TrackNumber)) ? trackNumber : Convert.ToUInt32(fileInfo.TrackNumber);
                }
                file.Save();

                // Update the track and rename the file
                trackNumber++;
                File.Move(filePath, fileInfo.NewFilePath);
                Console.WriteLine($"Tagged file {filePath}");
            });
        }
    }
}

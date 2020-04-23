using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicTagEditor
{
    /// <summary>
    /// Command line argument parameters
    /// </summary>
    public class Options
    {
        /// <summary>
        /// The name of the album
        /// </summary>
        [Option('a', "album", HelpText ="The name of the album" )]
        public string Album { get; set; }

        /// <summary>
        /// The contributing artist
        /// </summary>
        [Option('c', "ca", HelpText ="The name of the contributing artist")]
        public string ContributingArtist { get; set; }

        /// <summary>
        /// The genre
        /// </summary>
        [Option('g', "genre", HelpText ="The album genre")]
        public string Genre { get; set; }

        /// <summary>
        /// The track number to use
        /// </summary>
        [Option('t', "track", HelpText ="True to use the track number specified in the filename. Uses auto numbering if no track number found")]
        public bool UseTrackNumber { get; set; }
    }

    /// <summary>
    /// Program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">The list of arguments</param>
        static void Main(string[] args)
        {
            // Configure the command line parser
            var parser = new Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<Options>(args);

            // Parse the command line arguments
            parserResult.WithParsed<Options>(options => Run(options))
              .WithNotParsed(errs => DisplayHelp(parserResult, errs));
        }

        /// <summary>
        /// Runs the job
        /// </summary>
        /// <param name="options">The command line options</param>
        private static void Run(Options options)
        {
            // Create the tagger
            Console.WriteLine("\r\nCreating the file tagger");
            var tagger = new FileTagger(options);
            
            // Tag the files
            tagger.TagFiles();
            Console.WriteLine("Complete");
        }

        /// <summary>
        /// Generate the help menu
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="result">The parser result</param>
        /// <param name="errs">The list of errors</param>
        static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            // Generate the help page
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "Music Tag Editor v1.0";
                h.Copyright = "Copyright (c) 2020. All Rights Reserved";
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);

            // Display to the user
            Console.WriteLine(helpText);
        }
    }
}

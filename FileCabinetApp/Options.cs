using CommandLine;

namespace FileCabinetApp
{
    /// <summary>
    /// Options for command line parser.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets property contains command line argument for rule.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('v', "validation-rules", Default ="default", Required = false, HelpText = "Validation rules")]
        public string Rule { get; set; }

        /// <summary>
        /// Gets or sets property contains command line argument for storage.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('s', "storage", Default ="memory", Required = false, HelpText = "Storage to use")]
        public string Storage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets property contains command line argument for measuring runtime.
        /// </summary>
        /// <value>True if use stopwatch.</value>
        [Option("use-stopwatch", Default = false, Required = false, HelpText = "For measuring runtime")]
        public bool Stopwatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets property contains command line argument for using logger.
        /// </summary>
        /// <value>True if use logger.</value>
        [Option("use-logger", Default = false, Required = false, HelpText = "For measuring runtime")]
        public bool Log { get; set; }
    }
}

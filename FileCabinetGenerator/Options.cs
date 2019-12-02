using CommandLine;

namespace FileCabinetApp
{
    /// <summary>
    /// Options for command line parser.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets property contains command line argument for input file type.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('t', "output-type", Required = true, HelpText = "Input file type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets property contains command line argument for file path.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('o', "output", Required = true, HelpText = "File path")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets property contains command line argument for id of first record.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('i', "start-id", Default = 1, Required = false, HelpText = "First id number")]
        public int StartId { get; set; }

        /// <summary>
        /// Gets or sets property contains command line argument for amount of records.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('a', "records-amount", Default = 20, Required = false, HelpText = "Amount of records")]
        public int Amount { get; set; }
    }
}

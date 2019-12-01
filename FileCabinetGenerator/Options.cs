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
        [Option('t', "output-type", Required = true, HelpText = "Validation rules")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets property contains command line argument for storage.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('o', "output", Required = true, HelpText = "Storage to use")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sers id of first record.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('i', "start-id", Default = 1, Required = false, HelpText = "First id number")]
        public int StartId { get; set; }

        /// <summary>
        /// Gets or sets amount of records.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('a', "records-amount", Default = 20, Required = false, HelpText = "Amount of records")]
        public int Amount { get; set; }
    }
}

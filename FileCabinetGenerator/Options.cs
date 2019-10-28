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
        [Option('t', "output-type", Default ="csv", Required = true, HelpText = "Validation rules")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets property contains command line argument for storage.
        /// </summary>
        /// <value>Command line argument.</value>
        [Option('o', "output", Default ="", Required = true, HelpText = "Storage to use")]
        public string FileName { get; set; }

        [Option('i', "start-id	", Default = 1, Required = false, HelpText = "Storage to use")]
        public int StartId { get; set; }

        [Option('a', "records-amount", Default = 20, Required = false, HelpText = "Storage to use")]
        public int Amount { get; set; }
    }
}

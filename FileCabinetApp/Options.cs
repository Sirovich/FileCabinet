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
    }
}

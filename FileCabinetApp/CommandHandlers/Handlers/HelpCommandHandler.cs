using System;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for help.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "insert", "creates new record", "The 'insert' command creates new record." },
            new string[] { "delete", "removes records", "The 'delete' command removes records." },
            new string[] { "update", "updates records", "The 'update' command updates records." },
            new string[] { "purge", "do defragmenation", "The 'purge' command does defragmenation." },
            new string[] { "stat", "prints count of records", "The 'stat' command prints count of records." },
            new string[] { "select", "prints records", "The 'select' command prints records." },
            new string[] { "export", "saves records in file", "The 'export' command saves records in file." },
            new string[] { "import", "imports records in file", "The 'import' command imports records in file." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                Console.WriteLine(Source.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            if (commandRequest.Command is null)
            {
                Console.WriteLine(Source.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            if (commandRequest.Command.Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                this.PrintHelp(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine(Source.Resource.GetString("noExplanationMessage", CultureInfo.InvariantCulture), parameters);
                }
            }
            else
            {
                Console.WriteLine(Source.Resource.GetString("availableMessage", CultureInfo.InvariantCulture));

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine($"\t{helpMessage[CommandHelpIndex]}\t- {helpMessage[DescriptionHelpIndex]}");
                }
            }

            Console.WriteLine();
        }
    }
}

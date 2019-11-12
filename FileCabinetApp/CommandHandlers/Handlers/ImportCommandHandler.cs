using System;
using System.Globalization;
using System.IO;
using System.Xml;
using FileCabinetApp;
using FileCabinetApp.Services;
using FileCabinetApp.Snapshots;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for import.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        public ImportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                Console.WriteLine(Resources.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            if (commandRequest.Command is null)
            {
                Console.WriteLine(Resources.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            if (commandRequest.Command.Equals("Import", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Import(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Import(string parameters)
        {
            if (parameters == null)
            {
                Console.WriteLine(Resources.Resource.GetString("exportArgumentsException", CultureInfo.InvariantCulture));
                Console.WriteLine(Resources.Resource.GetString("exportFormat", CultureInfo.InvariantCulture));
                return;
            }

            var arguments = parameters.Split(' ');

            if (arguments.Length == 1)
            {
                Console.WriteLine(Resources.Resource.GetString("importArgumentsException", CultureInfo.InvariantCulture));
                Console.WriteLine(Resources.Resource.GetString("importFormat", CultureInfo.InvariantCulture));
                return;
            }
            else if (arguments.Length == 2)
            {
                const int typeIndex = 0;
                const int pathIndex = 1;

                var snapshot = new FileCabinetServiceSnapshot();

                if (arguments[typeIndex].Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (var fileStream = new StreamReader(arguments[pathIndex]))
                    {
                        snapshot.LoadFromCsv(fileStream);
                        int numberOfStored = this.Service.Restore(snapshot);
                        Console.WriteLine(Resources.Resource.GetString("importFileComplete", CultureInfo.InvariantCulture), numberOfStored, arguments[pathIndex]);
                    }
                }
                else if (arguments[typeIndex].Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";

                    using (var fileStream = new StreamReader(arguments[pathIndex]))
                    using (var xmlReader = XmlReader.Create(fileStream))
                    {
                        snapshot.LoadFromXml(xmlReader);
                        int numberOfImported = this.Service.Restore(snapshot);
                        Console.WriteLine(Resources.Resource.GetString("importFileComplete", CultureInfo.InvariantCulture), numberOfImported, arguments[pathIndex]);
                    }
                }
            }
            else
            {
                Console.WriteLine(Resources.Resource.GetString("importUnknownArgument", CultureInfo.InvariantCulture), arguments[2]);
            }
        }
    }
}

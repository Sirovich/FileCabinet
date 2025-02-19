﻿using System;
using System.Globalization;
using System.IO;
using System.Xml;
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
                Console.WriteLine(Source.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            if (commandRequest.Command is null)
            {
                Console.WriteLine(Source.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
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
                Console.WriteLine(Source.Resource.GetString("exportArgumentsException", CultureInfo.InvariantCulture));
                Console.WriteLine(Source.Resource.GetString("exportFormat", CultureInfo.InvariantCulture));
                return;
            }

            var arguments = parameters.Split(' ');

            if (arguments.Length == 1)
            {
                Console.WriteLine(Source.Resource.GetString("importArgumentsException", CultureInfo.InvariantCulture));
                Console.WriteLine(Source.Resource.GetString("importFormat", CultureInfo.InvariantCulture));
                return;
            }
            else if (arguments.Length == 2)
            {
                const int typeIndex = 0;
                const int pathIndex = 1;

                var snapshot = new FileCabinetServiceSnapshot();

                if (arguments[typeIndex].Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!File.Exists(arguments[pathIndex]))
                    {
                        Console.WriteLine(Source.Resource.GetString("fileNotFound", CultureInfo.InvariantCulture));
                        return;
                    }

                    using (var fileStream = new StreamReader(arguments[pathIndex]))
                    {
                        try
                        {
                            snapshot.LoadFromCsv(fileStream);
                            int numberOfStored = this.Service.Restore(snapshot);
                            Console.WriteLine(Source.Resource.GetString("importFileComplete", CultureInfo.InvariantCulture), numberOfStored, arguments[pathIndex]);
                        }
                        catch (ArgumentNullException)
                        {
                            Console.WriteLine(Source.Resource.GetString("importFailed", CultureInfo.InvariantCulture));
                        }
                    }
                }
                else if (arguments[typeIndex].Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!File.Exists(arguments[pathIndex]))
                    {
                        Console.WriteLine(Source.Resource.GetString("fileNotFound", CultureInfo.InvariantCulture));
                        return;
                    }

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";

                    using (var fileStream = new StreamReader(arguments[pathIndex]))
                    using (var xmlReader = XmlReader.Create(fileStream))
                    {
                        snapshot.LoadFromXml(xmlReader);
                        int numberOfImported = this.Service.Restore(snapshot);
                        Console.WriteLine(Source.Resource.GetString("importFileComplete", CultureInfo.InvariantCulture), numberOfImported, arguments[pathIndex]);
                    }
                }
                else
                {
                    Console.WriteLine(Source.Resource.GetString("importUnknownFormat", CultureInfo.InvariantCulture), arguments[typeIndex]);
                }
            }
            else
            {
                Console.WriteLine(Source.Resource.GetString("importUnknownArgument", CultureInfo.InvariantCulture), arguments[2]);
            }
        }
    }
}

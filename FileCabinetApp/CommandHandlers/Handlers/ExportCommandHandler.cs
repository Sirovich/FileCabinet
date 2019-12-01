using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for export.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        public ExportCommandHandler(IFileCabinetService fileCabinetService)
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

            if (commandRequest.Command.Equals("export", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Export(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Export(string parameters)
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
                Console.WriteLine(Source.Resource.GetString("exportArgumentsException", CultureInfo.InvariantCulture));
                Console.WriteLine(Source.Resource.GetString("exportFormat", CultureInfo.InvariantCulture));
                return;
            }
            else if (arguments.Length == 2)
            {
                const int pathIndex = 1;
                const int typeIndex = 0;

                var drive = Path.GetPathRoot(arguments[pathIndex]);

                if (drive.Trim(' ', '\\').Length != 0 && !Environment.GetLogicalDrives().Contains(drive, StringComparer.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine(Source.Resource.GetString("missingDiskDrive", CultureInfo.InvariantCulture));
                    return;
                }

                var path = Path.GetDirectoryName(arguments[pathIndex]);

                if (!Directory.Exists(path) && path.Trim(' ', '\\').Length != 0)
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(arguments[pathIndex]))
                {
                    Console.WriteLine(Source.Resource.GetString("fileExistMessage", CultureInfo.InvariantCulture), arguments[pathIndex]);
                    var answer = Console.ReadLine();
                    if (answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        File.Delete(arguments[pathIndex]);
                    }
                    else
                    {
                        return;
                    }
                }

                var snapshot = this.Service.MakeSnapshot();

                if (arguments[typeIndex].Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        using (var fileStream = new StreamWriter(arguments[pathIndex]))
                        {
                            fileStream.WriteLine(Source.Resource.GetString("fileHeader", CultureInfo.InvariantCulture));
                            snapshot.SaveToCsv(fileStream);
                            Console.WriteLine(Source.Resource.GetString("exportFileComplete", CultureInfo.InvariantCulture), arguments[pathIndex]);
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
                else if (arguments[typeIndex].Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    try
                    {
                        using (var fileStream = XmlWriter.Create(arguments[pathIndex], settings))
                        {
                            snapshot.SaveToXml(fileStream);
                            Console.WriteLine(Source.Resource.GetString("exportFileComplete", CultureInfo.InvariantCulture), arguments[pathIndex]);
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine(Source.Resource.GetString("exportUnknownFormat", CultureInfo.InvariantCulture), arguments[typeIndex]);
                }
            }
            else
            {
                Console.WriteLine(Source.Resource.GetString("exportUnknownArgument", CultureInfo.InvariantCulture), arguments[2]);
            }
        }
    }
}

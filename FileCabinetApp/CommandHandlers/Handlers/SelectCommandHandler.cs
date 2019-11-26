using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsoleTables;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Select command handler.
    /// </summary>
    public class SelectCommandHandler : SelectCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        /// <param name="printer">Source printer.</param>
        public SelectCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(fileCabinetService)
        {
            this.printer = printer;
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

            if (commandRequest.Command.Equals("select", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Select(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private static void PrintResult(IEnumerable<FileCabinetRecord> records, IEnumerable<string> arguments)
        {
            var table = new ConsoleTable();
            table.AddColumn(arguments);

            foreach (var record in records)
            {
                var fields = new List<object>();
                foreach (var column in arguments)
                {
                    if (column.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fields.Add(record.Id);
                        continue;
                    }

                    if (column.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fields.Add(record.FirstName);
                        continue;
                    }

                    if (column.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fields.Add(record.LastName);
                        continue;
                    }

                    if (column.Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fields.Add(record.DateOfBirth.ToShortDateString());
                        continue;
                    }

                    if (column.Equals("sex", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fields.Add(record.Sex);
                        continue;
                    }

                    if (column.Equals("weight", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fields.Add(record.Weight);
                        continue;
                    }

                    if (column.Equals("height", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fields.Add(record.Height);
                        continue;
                    }

                    return;
                }

                table.AddRow(fields.ToArray());
            }

            table.Write(format: Format.Alternative);
        }

        private void Select(string parameters)
        {
            if (parameters is null)
            {
                Console.WriteLine(Source.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            if (parameters.Length == 0)
            {
                var fields = new List<string>();
                fields.Add("id");
                fields.Add("firstname");
                fields.Add("lastname");
                fields.Add("dateofbirth");
                fields.Add("sex");
                fields.Add("weight");
                fields.Add("height");
                PrintResult(this.Service.GetRecords(), fields);
                return;
            }

            var arguments = parameters.Split("where", 2);

            if (arguments.Length == 1)
            {
                PrintResult(this.Service.GetRecords(), parameters.Split(',').Select(x => x.Trim()));
            }
            else
            {
                var fields = arguments[0].Split(',').Select(x => x.Trim());
                var records = this.GetRecords(arguments[1]);

                if (records is null)
                {
                    return;
                }
                else
                {
                    PrintResult(records, fields);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for find.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        /// <param name="printer">Source printer.</param>
        public FindCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> printer)
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

            if (commandRequest.Command.Equals("find", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Find(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Find(string parameters)
        {
            if (parameters is null)
            {
                Console.WriteLine(Source.Resource.GetString("invalidArgument", CultureInfo.InvariantCulture));
                return;
            }

            var methods = new Tuple<string, Func<string, IEnumerable<FileCabinetRecord>>>[]
            {
                 new Tuple<string, Func<string, IEnumerable<FileCabinetRecord>>>("firstname", this.Service.FindByFirstName),
                 new Tuple<string, Func<string, IEnumerable<FileCabinetRecord>>>("lastname", this.Service.FindByLastName),
                 new Tuple<string, Func<string, IEnumerable<FileCabinetRecord>>>("dateofbirth", this.Service.FindByDateOfBirth),
            };

            var arguments = parameters.Split(' ', 2);

            if (arguments.Length < 2)
            {
                Console.WriteLine(Source.Resource.GetString("noRecordsMessage", CultureInfo.InvariantCulture));
                return;
            }

            var index = Array.FindIndex(methods, 0, methods.Length, i => i.Item1.Equals(arguments[0], StringComparison.InvariantCultureIgnoreCase));
            const int argumentIndex = 1;
            if (index >= 0)
            {
                var records = methods[index].Item2(arguments[argumentIndex]);
                if (records is null)
                {
                    Console.WriteLine(Source.Resource.GetString("noRecordsMessage", CultureInfo.InvariantCulture));
                }
                else
                {
                    this.printer(records);
                }
            }
        }
    }
}

using System;
using System.Globalization;
using FileCabinetApp.Printer;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for list.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        /// <param name="printer">Source printer.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService, IRecordPrinter printer)
            : base(fileCabinetService)
        {
            this.printer = printer;
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

            if (commandRequest.Command.Equals("list", StringComparison.InvariantCultureIgnoreCase))
            {
                this.List(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void List(string parameters)
        {
            var list = this.Service.GetRecords();

            if (list.Count == 0)
            {
                Console.WriteLine(Resources.Resource.GetString("noRecords", CultureInfo.InvariantCulture));
            }

            this.printer.Print(list);
        }
    }
}

using System;
using System.Globalization;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for stat.
    /// </summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        public StatCommandHandler(IFileCabinetService fileCabinetService)
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

            if (commandRequest.Command.Equals("stat", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Stat(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Stat(string parameters)
        {
            var recordsCount = this.Service.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }
    }
}

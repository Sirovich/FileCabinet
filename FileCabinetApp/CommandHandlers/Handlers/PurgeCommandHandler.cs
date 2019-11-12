using System;
using System.Globalization;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for purge.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
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

            if (commandRequest.Command.Equals("purge", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Purge(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Purge(string parameters)
        {
            this.Service.Purge();
        }
    }
}

using System;
using System.Globalization;
using FileCabinetApp;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for remove.
    /// </summary>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
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

            if (commandRequest.Command.Equals("remove", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Remove(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Remove(string parameters)
        {
            int id = 0;
            if (!int.TryParse(parameters, out id))
            {
                Console.WriteLine(Resources.Resource.GetString("invalidInputMessage", CultureInfo.InvariantCulture), parameters);
                return;
            }

            if (!this.Service.RemoveRecord(id))
            {
                Console.WriteLine(Resources.Resource.GetString("recordNotExist", CultureInfo.InvariantCulture), id);
            }
            else
            {
                Console.WriteLine(Resources.Resource.GetString("removeSuccess", CultureInfo.InvariantCulture), id);
            }
        }
    }
}

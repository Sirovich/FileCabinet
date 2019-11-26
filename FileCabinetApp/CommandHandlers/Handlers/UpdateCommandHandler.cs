using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Update command handler.
    /// </summary>
    public class UpdateCommandHandler : SelectCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        public UpdateCommandHandler(IFileCabinetService fileCabinetService)
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

            if (commandRequest.Command.Equals("update", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Update(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Update(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Substring(0, 3).Equals("set", StringComparison.InvariantCulture))
            {
                parameters = parameters.Remove(0, 3);
            }
            else
            {
                Console.WriteLine();
            }

            var arguments = parameters.Split("where ", 2);
            if (arguments.Length < 2)
            {
                Console.WriteLine();
                return;
            }
            else
            {
                var fieldsToReplace = arguments[0].Split(',');
                var fieldsAndValuesToReplace = fieldsToReplace.Select(x => x.Split('=').Select(y => y.Trim('\'', ' ')));
                var records = this.GetRecords(arguments[1]);
                if (records is null)
                {
                    return;
                }
                else
                {
                    this.Service.Update(records, fieldsAndValuesToReplace);
                    RefreshMemoization();
                }
            }
        }
    }
}

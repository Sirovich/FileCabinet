using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Delete command handler.
    /// </summary>
    public class DeleteCommandHandler : SelectCommandHandlerBase
    {
        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="recordValidator">Source record validator.</param>
        /// <param name="fileCabinetService">Source service.</param>
        public DeleteCommandHandler(IRecordValidator recordValidator, IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
            this.recordValidator = recordValidator;
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

            if (commandRequest.Command.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Delete(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Delete(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var arguments = parameters.Split("where ", 2);
            if (arguments.Length < 2)
            {
                Console.WriteLine(Source.Resource.GetString("unknownArgument", CultureInfo.InvariantCulture), arguments[0]);
                return;
            }
            else
            {
                var records = this.GetRecords(arguments[1]);
                if (records is null)
                {
                    return;
                }
                else
                {
                    this.Service.Delete(records);
                    RefreshMemoization();
                }
            }
        }
    }
}

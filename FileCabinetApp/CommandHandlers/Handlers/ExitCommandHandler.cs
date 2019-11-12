using System;
using System.Globalization;
using FileCabinetApp;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Command handler for exit.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="action">Delegate to stop working.</param>
        public ExitCommandHandler(Action<bool> action)
        {
            this.action = action;
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

            if (commandRequest.Command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Exit();
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Exit()
        {
            Console.WriteLine(Source.Resource.GetString("exitMessage", CultureInfo.InvariantCulture));
            this.action(false);
        }
    }
}

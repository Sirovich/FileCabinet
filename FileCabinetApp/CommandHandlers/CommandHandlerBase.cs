using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base command handler class.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler commandHandler;

        /// <inheritdoc/>
        public virtual void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                return;
            }

            if (this.commandHandler != null)
            {
                this.commandHandler.Handle(commandRequest);
            }
            else
            {
                PrintMissedCommandInfo(commandRequest.Command);
            }
        }

        /// <inheritdoc/>
        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
            return this.commandHandler;
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}

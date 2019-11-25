namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler interface.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets next handle.
        /// </summary>
        /// <param name="commandHandler">Next handler to set.</param>
        /// <returns>Next handeler.</returns>
        ICommandHandler SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Handle request and try process command.
        /// </summary>
        /// <param name="commandRequest">Source command request.</param>
        void Handle(AppCommandRequest commandRequest);
    }
}

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command request.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Source command.</param>
        /// <param name="parameters">Source parameters.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets command.
        /// </summary>
        /// <value>Command.</value>
        public string Command { get; }

        /// <summary>
        /// Gets parameters.
        /// </summary>
        /// <value>Parameters.</value>
        public string Parameters { get; }
    }
}

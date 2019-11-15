using System;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for handlers with service.
    /// </summary>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Source service.</param>
        protected ServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            this.Service = fileCabinetService;
        }

        /// <summary>
        /// Gets service.
        /// </summary>
        /// <value>Service.</value>
        protected IFileCabinetService Service { get; private set; }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            base.Handle(commandRequest);
        }
    }
}

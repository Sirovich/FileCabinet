using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Service with default rules.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates new validator with default rules.
        /// </summary>
        /// <returns>New default validator.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}

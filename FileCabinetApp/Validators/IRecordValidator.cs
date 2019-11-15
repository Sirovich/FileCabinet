using System;
using System.Resources;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Interface for validators.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Checks input parameters according to any rules.
        /// </summary>
        /// <param name="record">Source record.</param>
        /// <returns>True if record is valid.</returns>
        Tuple<bool, string> ValidateParameters(FileCabinetRecord record);
    }
}

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
        /// <exception cref="ArgumentException">Throws when any parameter is not valid.</exception>
        /// <exception cref="ArgumentNullException">Throws when resource manager is null.</exception>
        /// <param name="firstName">Person first name.</param>
        /// <param name="lastName">Person last name.</param>
        /// <param name="dateOfBirth">Person date of birth.</param>
        /// <param name="sex">Sex of a person.</param>
        /// <param name="height">Person height.</param>
        /// <param name="weight">Person weight.</param>
        /// <param name="resource">Source resource manager.</param>
        void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight, ResourceManager resource);
    }
}

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

        /// <summary>
        /// Checks input parameters according to any rules.
        /// </summary>
        /// <param name="record">Source record.</param>
        /// <param name="resource">Source resource manager.</param>
        void ValidateParameters(FileCabinetRecord record, ResourceManager resource);

        /// <summary>
        /// Validates input string with any rule.
        /// </summary>
        /// <param name="firstName">Person first name.</param>
        /// <returns>True if compliant with the rules and exception message.</returns>
        Tuple<bool, string> ValidateFirstName(string firstName);

        /// <summary>
        /// Validates input string with any rule.
        /// </summary>
        /// <param name="lastName">Person last name.</param>
        /// <returns>True if compliant with the rules and exception message.</returns>
        Tuple<bool, string> ValidateLastName(string lastName);

        /// <summary>
        /// Validates input string with any rule.
        /// </summary>
        /// <param name="dateOfBirth">Person date of birth.</param>
        /// <returns>True if compliant with the rules and exception message.</returns>
        Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Validates input string with any rule.
        /// </summary>
        /// <param name="sex">Person sex.</param>
        /// <returns>True if compliant with the rules and exception message.</returns>
        Tuple<bool, string> ValidateSex(char sex);

        /// <summary>
        /// Validates input string with any rule.
        /// </summary>
        /// <param name="weight">Person weight.</param>
        /// <returns>True if compliant with the rules and exception message.</returns>
        Tuple<bool, string> ValidateWeight(decimal weight);

        /// <summary>
        /// Validates input string with any rule.
        /// </summary>
        /// <param name="height">Person height.</param>
        /// <returns>True if compliant with the rules and exception message.</returns>
        Tuple<bool, string> ValidateHeight(short height);
    }
}

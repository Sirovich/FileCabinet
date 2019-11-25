using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Input validator.
    /// </summary>
    public interface IInputValidator
    {
        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="firstName">Person first name.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        Tuple<bool, string> ValidateFirstName(string firstName);

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="lastName">Person last name.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        Tuple<bool, string> ValidateLastName(string lastName);

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="dateOfBirth">Person date of birth.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="sex">Person sex.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        Tuple<bool, string> ValidateSex(char sex);

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="weight">Person weight.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        Tuple<bool, string> ValidateWeight(decimal weight);

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="height">Person height.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        Tuple<bool, string> ValidateHeight(short height);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator with default rules.
    /// </summary>
    public class DefaultInputValidator : IInputValidator
    {
        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="firstName">Person first name.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            if (firstName == null || firstName.Length < 2 || firstName.Length > 60 || firstName.Trim(' ').Length == 0)
            {
                return new Tuple<bool, string>(false, firstName);
            }

            return new Tuple<bool, string>(true, firstName);
        }

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="lastName">Person last name.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (lastName == null || lastName.Length < 2 || lastName.Length > 60 || lastName.Trim(' ').Length == 0)
            {
                return new Tuple<bool, string>(false, lastName);
            }

            return new Tuple<bool, string>(true, lastName);
        }

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="dateOfBirth">Person date of birth.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth == null || dateOfBirth < new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Now)
            {
                return new Tuple<bool, string>(false, "Wrong date");
            }

            return new Tuple<bool, string>(true, null);
        }

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="sex">Person sex.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateSex(char sex)
        {
            if (sex == ' ')
            {
                return new Tuple<bool, string>(false, "Wrong sex");
            }

            return new Tuple<bool, string>(true, null);
        }

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="weight">Person weight.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateWeight(decimal weight)
        {
            if (weight < 0 || weight > 5000)
            {
                return new Tuple<bool, string>(false, "Wrong weight");
            }

            return new Tuple<bool, string>(true, null);
        }

        /// <summary>
        /// Checks input parameter according to default rule.
        /// </summary>
        /// <param name="height">Person height.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateHeight(short height)
        {
            if (height < 0 || height > 5000)
            {
                return new Tuple<bool, string>(false, "Wrong height");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}

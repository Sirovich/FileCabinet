using System;
using System.Globalization;
using System.Resources;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator with custom rules.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Checks input parameter according to custom rule.
        /// </summary>
        /// <param name="firstName">Person first name.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            if (firstName == null || firstName.Length < 3 || firstName.Length > 70 || firstName.Trim(' ').Length < 1)
            {
                return new Tuple<bool, string>(false, firstName);
            }

            return new Tuple<bool, string>(true, firstName);
        }

        /// <summary>
        /// Checks input parameter according to custom rule.
        /// </summary>
        /// <param name="lastName">Person last name.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (lastName == null || lastName.Length < 3 || lastName.Length > 70 || lastName.Trim(' ').Length == 0)
            {
                return new Tuple<bool, string>(false, lastName);
            }

            return new Tuple<bool, string>(true, lastName);
        }

        /// <summary>
        /// Checks input parameter according to custom rule.
        /// </summary>
        /// <param name="dateOfBirth">Person date of birth.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth == null || dateOfBirth < new DateTime(1918, 03, 25) || dateOfBirth > DateTime.Now)
            {
                return new Tuple<bool, string>(false, "Wrong date");
            }

            return new Tuple<bool, string>(true, null);
        }

        /// <summary>
        /// Checks input parameter according to custom rule.
        /// </summary>
        /// <param name="sex">Person sex.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateSex(char sex)
        {
            if (sex == 'F')
            {
                return new Tuple<bool, string>(false, "Wrong sex");
            }

            return new Tuple<bool, string>(true, null);
        }

        /// <summary>
        /// Checks input parameter according to custom rule.
        /// </summary>
        /// <param name="weight">Person weight.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateWeight(decimal weight)
        {
            if (weight < 60)
            {
                return new Tuple<bool, string>(false, "Wrong weight");
            }

            return new Tuple<bool, string>(true, null);
        }

        /// <summary>
        /// Checks input parameter according to custom rule.
        /// </summary>
        /// <param name="height">Person height.</param>
        /// <returns>True if first name complies with the rules, false if not.</returns>
        public Tuple<bool, string> ValidateHeight(short height)
        {
            if (height < 146)
            {
                return new Tuple<bool, string>(false, "Wrong height");
            }

            return new Tuple<bool, string>(true, null);
        }

        /// <summary>
        /// Checks input parameters according to custom rules.
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
        public void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight, ResourceManager resource)
        {
            if (resource is null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            if (weight < 60)
            {
                throw new ArgumentException(resource.GetString("weightException", CultureInfo.InvariantCulture));
            }

            if (height < 146)
            {
                throw new ArgumentException(resource.GetString("heightException", CultureInfo.InvariantCulture));
            }

            if (sex == 'F')
            {
                throw new ArgumentException(resource.GetString("sexException", CultureInfo.InvariantCulture));
            }

            if (dateOfBirth == null || dateOfBirth < new DateTime(1918, 03, 25) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException(resource.GetString("dateOfBirthException", CultureInfo.InvariantCulture));
            }

            if (firstName == null || firstName.Length < 3 || firstName.Length > 70 || firstName.Trim(' ').Length < 1)
            {
                throw new ArgumentException(resource.GetString("firstNameException", CultureInfo.InvariantCulture));
            }

            if (lastName == null || lastName.Length < 3 || lastName.Length > 70 || lastName.Trim(' ').Length == 0)
            {
                throw new ArgumentException(resource.GetString("lastNameException", CultureInfo.InvariantCulture));
            }
        }
    }
}

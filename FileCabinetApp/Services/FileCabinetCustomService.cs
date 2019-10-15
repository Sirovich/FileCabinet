using System;
using System.Globalization;
using System.Resources;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// File cabinet service with custom validation.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
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
        protected override void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight, ResourceManager resource)
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

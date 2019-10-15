using System;
using System.Globalization;
using System.Resources;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// File cabinet service with default validation.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Checks input parameters according to default rules.
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

            if (weight < 0)
            {
                throw new ArgumentException(resource.GetString("weightException", CultureInfo.InvariantCulture));
            }

            if (height < 0)
            {
                throw new ArgumentException(resource.GetString("heightException", CultureInfo.InvariantCulture));
            }

            if (sex == ' ')
            {
                throw new ArgumentException(resource.GetString("sexException", CultureInfo.InvariantCulture));
            }

            if (dateOfBirth == null || dateOfBirth < new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException(resource.GetString("dateOfBirthException", CultureInfo.InvariantCulture));
            }

            if (firstName == null || firstName.Length < 2 || firstName.Length > 60 || firstName.Trim(' ').Length == 0)
            {
                throw new ArgumentException(resource.GetString("firstNameException", CultureInfo.InvariantCulture));
            }

            if (lastName == null || lastName.Length < 2 || lastName.Length > 60 || lastName.Trim(' ').Length == 0)
            {
                throw new ArgumentException(resource.GetString("lastNameException", CultureInfo.InvariantCulture));
            }
        }
    }
}

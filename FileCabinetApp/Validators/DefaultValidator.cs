using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator with default rules.
    /// </summary>
    public static class DefaultValidator
    {
        /// <summary>
        /// Creates new composite validator with default rules.
        /// </summary>
        /// <param name="validatorBuilder">Source builder.</param>
        /// <param name="configuration">Source configuration.</param>
        /// <returns>New composite validator.</returns>
        public static CompositeValidator CreateValidator(this ValidatorBuilder validatorBuilder, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var firstName = configuration.GetSection("firstName");
            var lastName = configuration.GetSection("lastName");
            var date = configuration.GetSection("dateOfBirth");
            var sex = configuration.GetSection("sex");
            var weight = configuration.GetSection("weight");
            var height = configuration.GetSection("height");
            date.GetSection("from");
            try
            {
                 var result = validatorBuilder?.ValidateFirstName(Convert.ToInt32(firstName.GetSection("min").Value, CultureInfo.InvariantCulture), Convert.ToInt32(firstName.GetSection("max").Value, CultureInfo.InvariantCulture))
                    .ValidateLastName(Convert.ToInt32(lastName.GetSection("min").Value, CultureInfo.InvariantCulture), Convert.ToInt32(lastName.GetSection("max").Value, CultureInfo.InvariantCulture))
                    .ValidateDateBirth(DateTime.ParseExact(date.GetSection("from").Value, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date.GetSection("to").Value, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .ValidateSex(sex.GetSection("from").Value.ToCharArray())
                    .ValidateWeight(Convert.ToInt32(weight.GetSection("min").Value, CultureInfo.InvariantCulture), Convert.ToInt32(weight.GetSection("max").Value, CultureInfo.InvariantCulture))
                    .ValidateHeight(Convert.ToInt32(height.GetSection("min").Value, CultureInfo.InvariantCulture), Convert.ToInt32(height.GetSection("max").Value, CultureInfo.InvariantCulture))
                    .Create();

                 return result;
            }
            catch (FormatException)
            {
                Console.WriteLine(Source.Resource.GetString("invalidJsonData", CultureInfo.InvariantCulture));
                Environment.Exit(1478);
            }
            catch (OverflowException)
            {
                Console.WriteLine(Source.Resource.GetString("invalidJsonData", CultureInfo.InvariantCulture));
                Environment.Exit(1478);
            }

            return null;
        }
    }
}

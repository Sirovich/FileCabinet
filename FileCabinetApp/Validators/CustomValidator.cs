using System;
using System.Globalization;
using System.Resources;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator with custom rules.
    /// </summary>
    public static class CustomValidator
    {
        /// <summary>
        /// Creates new composite validator with custom rules.
        /// </summary>
        /// <param name="validatorBuilder">Source builder.</param>
        /// <returns>New composite validator.</returns>
        public static CompositeValidator CreateCustom(this ValidatorBuilder validatorBuilder)
        {
            return validatorBuilder?.ValidateFirstName(3, 70)
                .ValidateLastName(3, 70)
                .ValidateDateBirth(new DateTime(1918, 03, 25), DateTime.Now)
                .ValidateSex('F')
                .ValidateWeight(60, 5000)
                .ValidateHeight(146, 5000)
                .Create();
        }
    }
}

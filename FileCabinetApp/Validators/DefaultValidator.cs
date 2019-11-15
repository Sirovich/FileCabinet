using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using FileCabinetApp;
using FileCabinetApp.Validators.FieldValidators;

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
        /// <returns>New composite validator.</returns>
        public static CompositeValidator CreateDefault(this ValidatorBuilder validatorBuilder)
        {
            return validatorBuilder?.ValidateFirstName(2, 60)
                .ValidateLastName(2, 60)
                .ValidateDateBirth(new DateTime(1950, 01, 01), DateTime.Now)
                .ValidateSex(' ')
                .ValidateWeight(0, 5000)
                .ValidateHeight(0, 5000)
                .Create();
        }
    }
}

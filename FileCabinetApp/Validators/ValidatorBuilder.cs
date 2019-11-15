using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Validators.FieldValidators;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator builder.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBuilder"/> class.
        /// </summary>
        public ValidatorBuilder()
        {
            this.validators = new List<IRecordValidator>();
        }

        /// <summary>
        /// Creates first name validator.
        /// </summary>
        /// <param name="min">Minimal size.</param>
        /// <param name="max">Maximal size.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates last name validator.
        /// </summary>
        /// <param name="min">Miniimal size.</param>
        /// <param name="max">Maximal size.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates date validator.
        /// </summary>
        /// <param name="min">Minimal date.</param>
        /// <param name="max">Maximal date.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateDateBirth(DateTime min, DateTime max)
        {
            this.validators.Add(new DateValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates sex validator.
        /// </summary>
        /// <param name="unaccept">Unaccept char.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateSex(char unaccept)
        {
            this.validators.Add(new SexValidator(unaccept));
            return this;
        }

        /// <summary>
        /// Creates sex validator.
        /// </summary>
        /// <param name="unaccept">Unaccept array of chars.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateSex(char[] unaccept)
        {
            this.validators.Add(new SexValidator(unaccept));
            return this;
        }

        /// <summary>
        /// Creates weight validator.
        /// </summary>
        /// <param name="min">Minimal size.</param>
        /// <param name="max">Maximal size.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateWeight(int min, int max)
        {
            this.validators.Add(new WeightValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates height validator.
        /// </summary>
        /// <param name="min">Minimal size.</param>
        /// <param name="max">Maximal size.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateHeight(int min, int max)
        {
            this.validators.Add(new HeightValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates composite validator.
        /// </summary>
        /// <returns>New composite validator.</returns>
        public CompositeValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}

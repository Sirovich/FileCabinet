using System;

namespace FileCabinetApp.Validators.FieldValidators
{
    /// <summary>
    /// First name validator.
    /// </summary>
    public class FirstNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">Minimal length.</param>
        /// <param name="maxLength">Maximal length.</param>
        public FirstNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <inheritdoc/>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.FirstName == null || record.FirstName.Length < this.minLength || record.FirstName.Length > this.maxLength || record.FirstName.Trim(' ').Length == 0)
            {
                return new Tuple<bool, string>(false, record.FirstName);
            }

            return new Tuple<bool, string>(true, record.FirstName);
        }
    }
}

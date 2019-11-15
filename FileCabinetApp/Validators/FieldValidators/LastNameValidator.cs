using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Validators.FieldValidators
{
    /// <summary>
    /// Last name validator.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">Minimal length.</param>
        /// <param name="maxLength">Maximal length.</param>
        public LastNameValidator(int minLength, int maxLength)
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

            if (record.LastName == null || record.LastName.Length < this.minLength || record.LastName.Length > this.maxLength || record.LastName.Trim(' ').Length == 0)
            {
                return new Tuple<bool, string>(false, record.LastName);
            }

            return new Tuple<bool, string>(true, record.LastName);
        }
    }
}

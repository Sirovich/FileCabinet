using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.FieldValidators
{
    /// <summary>
    /// Weight validator.
    /// </summary>
    public class WeightValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightValidator"/> class.
        /// </summary>
        /// <param name="minLength">Minimal size.</param>
        /// <param name="maxLength">Maximal size.</param>
        public WeightValidator(int minLength, int maxLength)
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

            if (record.Weight < this.minLength || record.Weight > this.maxLength)
            {
                return new Tuple<bool, string>(false, "Wrong weight");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}

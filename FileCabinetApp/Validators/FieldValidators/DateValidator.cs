using System;

namespace FileCabinetApp.Validators.FieldValidators
{
    /// <summary>
    /// Date validator.
    /// </summary>
    public class DateValidator : IRecordValidator
    {
        private DateTime minDate;
        private DateTime maxDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateValidator"/> class.
        /// </summary>
        /// <param name="minDate">Minimal date.</param>
        /// <param name="maxDate">Maximim date.</param>
        public DateValidator(DateTime minDate, DateTime maxDate)
        {
            this.minDate = minDate;
            this.maxDate = maxDate;
        }

        /// <inheritdoc/>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.DateOfBirth == null || record.DateOfBirth < this.minDate || record.DateOfBirth > this.maxDate)
            {
                return new Tuple<bool, string>(false, "Wrong date");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}

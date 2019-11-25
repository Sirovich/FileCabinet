using System;

namespace FileCabinetApp.Validators.FieldValidators
{
    /// <summary>
    /// Height validator.
    /// </summary>
    public class HeightValidator : IRecordValidator
    {
        private int min;
        private int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightValidator"/> class.
        /// </summary>
        /// <param name="min">Minimal size.</param>
        /// <param name="max">Maximal size.</param>
        public HeightValidator(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        /// <inheritdoc/>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Height < this.min || record.Height > this.max)
            {
                return new Tuple<bool, string>(false, "Wrong height");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}

using System;
using System.Linq;

namespace FileCabinetApp.Validators.FieldValidators
{
    /// <summary>
    /// Sex validator.
    /// </summary>
    public class SexValidator : IRecordValidator
    {
        private char[] unacceptable;

        /// <summary>
        /// Initializes a new instance of the <see cref="SexValidator"/> class.
        /// </summary>
        /// <param name="unacceptable">Source char.</param>
        public SexValidator(char unacceptable)
        {
            this.unacceptable = new char[1] { unacceptable };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SexValidator"/> class.
        /// </summary>
        /// <param name="unacceptable">Source char array.</param>
        public SexValidator(char[] unacceptable)
        {
            this.unacceptable = unacceptable;
        }

        /// <inheritdoc/>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (this.unacceptable.Contains(record.Sex))
            {
                return new Tuple<bool, string>(false, "Wrong sex");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}

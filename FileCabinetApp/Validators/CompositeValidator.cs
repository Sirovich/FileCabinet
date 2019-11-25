using System;
using System.Collections.Generic;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Composite validator.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">Source validators.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            if (validators is null)
            {
                throw new ArgumentNullException(nameof(validators));
            }

            this.validators = new List<IRecordValidator>();

            foreach (var validator in validators)
            {
                this.validators.Add(validator);
            }
        }

        /// <inheritdoc/>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            foreach (var validator in this.validators)
            {
                var result = validator.ValidateParameters(record);
                if (result.Item1 == false)
                {
                    return new Tuple<bool, string>(false, result.Item2);
                }
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}

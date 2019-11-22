using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class contains record fields.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets Id of record.
        /// </summary>
        /// <value>Id of record.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets person weight.
        /// </summary>
        /// <value>Person weight.</value>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets person height.
        /// </summary>
        /// <value>Person height.</value>
        public short Height { get; set; }

        /// <summary>
        /// Gets or sets sex of a person.
        /// </summary>
        /// <value>Sex of a person.</value>
        public char Sex { get; set; }

        /// <summary>
        /// Gets or sets person first name.
        /// </summary>
        /// <value>Person first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets preson last name.
        /// </summary>
        /// <value>Person last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets persong date of birth.
        /// </summary>
        /// <value>Person date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        public static bool operator ==(FileCabinetRecord a, FileCabinetRecord b)
        {
            return a?.Id == b?.Id && a.FirstName.Equals(b.FirstName, StringComparison.InvariantCulture)
                && a.LastName.Equals(b.LastName, StringComparison.InvariantCulture) && a.DateOfBirth.Equals(b.DateOfBirth)
                && a.Sex == b.Sex && a.Weight == b.Weight && a.Height == b.Height;
        }

        public static bool operator !=(FileCabinetRecord a, FileCabinetRecord b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Compares two records.
        /// </summary>
        /// <param name="record">Source record.</param>
        /// <returns>True if all fields is equal.</returns>
        public bool Equals(FileCabinetRecord record)
        {
            return this == record;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

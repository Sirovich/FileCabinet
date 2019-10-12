using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(short height, decimal weight, char sex, string firstName, string lastName, DateTime dateOfBirth)
        {
            if (weight < 0)
            {
                throw new ArgumentException("Not valid weight");
            }

            if (height < 0)
            {
                throw new ArgumentException("Not valid height");
            }

            if (sex == ' ')
            {
                throw new ArgumentException("Not valid sex");
            }

            if (dateOfBirth < new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Not valid date of birth");
            }

            if (firstName == null || firstName.Length < 2 || firstName.Length > 60 || firstName.Trim(' ').Length == 0)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (lastName == null || lastName.Length < 2 || lastName.Length > 60 || lastName.Trim(' ').Length == 0)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                Sex = sex,
                Weight = weight,
                Height = height,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}

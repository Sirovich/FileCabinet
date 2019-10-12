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

            if (dateOfBirth == null || dateOfBirth < new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Not valid date of birth");
            }

            if (firstName == null || firstName.Length < 2 || firstName.Length > 60 || firstName.Trim(' ').Length == 0)
            {
                throw new ArgumentException("Not valid first name");
            }

            if (lastName == null || lastName.Length < 2 || lastName.Length > 60 || lastName.Trim(' ').Length == 0)
            {
                throw new ArgumentException("Not valid last name");
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

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight)
        {
            var records = this.list;
            foreach (FileCabinetRecord record in records)
            {
                if (record.Id == id)
                {
                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.Sex = sex;
                    record.Weight = weight;
                    record.Height = height;
                    return;
                }
            }

            throw new ArgumentException($"{id} record is not found.");
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

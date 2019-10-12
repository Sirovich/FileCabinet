using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

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

            if (!this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord>());
            }

            if (!this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord>());
            }

            if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord>());
            }

            this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
            this.lastNameDictionary[record.LastName].Add(record);
            this.firstNameDictionary[record.FirstName].Add(record);
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
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.Sex = sex;
                    record.Weight = weight;
                    record.Height = height;
                    if (!this.firstNameDictionary.ContainsKey(record.FirstName))
                    {
                        this.firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord>());
                    }

                    if (!this.lastNameDictionary.ContainsKey(record.LastName))
                    {
                        this.lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord>());
                    }

                    if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
                    {
                        this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord>());
                    }

                    this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
                    this.lastNameDictionary[record.LastName].Add(record);
                    this.firstNameDictionary[record.FirstName].Add(record);
                    return;
                }
            }

            throw new ArgumentException($"{id} record is not found.");
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return this.firstNameDictionary[firstName].ToArray();
            }

            return null;
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return this.lastNameDictionary[lastName].ToArray();
            }

            return null;
        }

        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            DateTime date = default;
            if (DateTime.TryParse(dateOfBirth, out date))
            {
                if (this.dateOfBirthDictionary.ContainsKey(date))
                {
                    return this.dateOfBirthDictionary[date].ToArray();
                }
            }

            return null;
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

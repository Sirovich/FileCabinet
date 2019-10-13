﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class provides methods for working with records.
    /// </summary>
    public class FileCabinetService
    {
        private static readonly ResourceManager Resource = new ResourceManager("FileCabinetApp.res", typeof(Program).Assembly);

        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <exception cref="ArgumentException">Throws when any value does not meet the requirements.</exception>
        /// <param name="height">Persong height.</param>
        /// <param name="weight">Person weight.</param>
        /// <param name="sex">Sex of a person.</param>
        /// <param name="firstName">Person first name.</param>
        /// <param name="lastName">Person last name.</param>
        /// <param name="dateOfBirth">Person date of birth.</param>
        /// <returns>Id of created record.</returns>
        public int CreateRecord(short height, decimal weight, char sex, string firstName, string lastName, DateTime dateOfBirth)
        {
            if (weight < 0)
            {
                throw new ArgumentException(Resource.GetString("weightException", CultureInfo.InvariantCulture));
            }

            if (height < 0)
            {
                throw new ArgumentException(Resource.GetString("heightException", CultureInfo.InvariantCulture));
            }

            if (sex == ' ')
            {
                throw new ArgumentException(Resource.GetString("sexException", CultureInfo.InvariantCulture));
            }

            if (dateOfBirth == null || dateOfBirth < new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException(Resource.GetString("dateOfBirthException", CultureInfo.InvariantCulture));
            }

            if (firstName == null || firstName.Length < 2 || firstName.Length > 60 || firstName.Trim(' ').Length == 0)
            {
                throw new ArgumentException(Resource.GetString("firstNameException", CultureInfo.InvariantCulture));
            }

            if (lastName == null || lastName.Length < 2 || lastName.Length > 60 || lastName.Trim(' ').Length == 0)
            {
                throw new ArgumentException(Resource.GetString("lastNameException", CultureInfo.InvariantCulture));
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

        /// <summary>
        /// Edits an existing record.
        /// </summary>
        /// <exception cref="ArgumentException">Throws when record with this id does not exist.</exception>
        /// <param name="id">Existing record id.</param>
        /// <param name="firstName">New first name of person.</param>
        /// <param name="lastName">New last name of person.</param>
        /// <param name="dateOfBirth">New date of birth of person.</param>
        /// <param name="sex">New sex of a person.</param>
        /// <param name="height">New height of person.</param>
        /// <param name="weight">New weight of person.</param>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight)
        {
            var records = this.list;
            foreach (FileCabinetRecord record in records)
            {
                if (record.Id == id)
                {
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.Sex = sex;
                    record.Weight = weight;
                    record.Height = height;
                    record.DateOfBirth = dateOfBirth;

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

        /// <summary>
        /// Finds all records with this first name.
        /// </summary>
        /// <param name="firstName">First name to search.</param>
        /// <returns>Array of records with this first name.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return this.firstNameDictionary[firstName].ToArray();
            }

            return null;
        }

        /// <summary>
        /// Finds all records with this last name.
        /// </summary>
        /// <param name="lastName">Last name to search.</param>
        /// <returns>Array of records with this last name.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return this.lastNameDictionary[lastName].ToArray();
            }

            return null;
        }

        /// <summary>
        /// Finds all records with this date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search.</param>
        /// <returns>Array of records with this date of birth.</returns>
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

        /// <summary>
        /// Gets array of records.
        /// </summary>
        /// <returns>Array of records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// Gets count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }
    }
}

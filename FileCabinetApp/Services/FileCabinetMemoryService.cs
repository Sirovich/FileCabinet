using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using FileCabinetApp.Snapshots;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class provides methods for working with records.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private static readonly ResourceManager Resource = new ResourceManager("FileCabinetApp.res", typeof(Program).Assembly);

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="recordValidator">Source validator.</param>
        public FileCabinetMemoryService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
        }

        /// <summary>
        /// Captures the status of the service.
        /// </summary>
        /// <returns>Returns snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

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
            var temp = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                Sex = sex,
                Weight = weight,
                Height = height,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };

            if (this.recordValidator.ValidateParameters(temp).Item1)
            {
                throw new ArgumentException(this.recordValidator.ValidateParameters(temp).Item2);
            }

            var record = temp;
            this.AddToDictionaries(record);
            this.list.Add(record);

            return record.Id;
        }

        /// <summary>
        /// Remove record.
        /// </summary>
        /// <param name="id">Source id.</param>
        /// <returns>True if record with source id is exist.</returns>
        public bool RemoveRecord(int id)
        {
            FileCabinetRecord temp = null;

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    temp = record;
                    this.list.Remove(record);
                    break;
                }
            }

            if (temp != null)
            {
                this.RemoveRecordFromDictionaries(temp);
                return true;
            }

            return false;
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
            var temp = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                Sex = sex,
                Weight = weight,
                Height = height,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };

            this.recordValidator.ValidateParameters(temp);
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

                    this.AddToDictionaries(record);
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
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
            }

            return null;
        }

        /// <summary>
        /// Finds all records with this last name.
        /// </summary>
        /// <param name="lastName">Last name to search.</param>
        /// <returns>Array of records with this last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);
            }

            return null;
        }

        /// <summary>
        /// Finds all records with this date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search.</param>
        /// <returns>Array of records with this date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            DateTime date = default;
            if (DateTime.TryParse(dateOfBirth, out date))
            {
                if (this.dateOfBirthDictionary.ContainsKey(date))
                {
                    return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[date]);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets array of records.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Gets count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Import records from file.
        /// </summary>
        /// <param name="snapshot">Source snapshot.</param>
        /// <returns>Number of stored.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null || snapshot.Records is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var list = new List<FileCabinetRecord>();
            var importData = snapshot.Records;
            int sourceIndex = 0;
            int importIndex = 0;

            for (; sourceIndex < this.list.Count && importIndex < importData.Count;)
            {
                if (this.list[sourceIndex].Id < importData[importIndex].Id)
                {
                    list.Add(this.list[sourceIndex]);
                    sourceIndex++;
                }
                else if (this.list[sourceIndex].Id == importData[importIndex].Id)
                {
                    try
                    {
                        this.recordValidator.ValidateParameters(importData[importIndex]);
                        list.Add(importData[importIndex]);

                        this.firstNameDictionary[this.list[sourceIndex].FirstName].Remove(this.list[sourceIndex]);
                        this.lastNameDictionary[this.list[sourceIndex].LastName].Remove(this.list[sourceIndex]);
                        this.dateOfBirthDictionary[this.list[sourceIndex].DateOfBirth].Remove(this.list[sourceIndex]);

                        this.AddToDictionaries(importData[importIndex]);

                        importIndex++;
                        sourceIndex++;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(Resource.GetString("importFailValidation", CultureInfo.InvariantCulture), importData[importIndex].Id, ex.Message);
                        importIndex++;
                        sourceIndex++;
                        continue;
                    }
                }
                else
                {
                    try
                    {
                        this.recordValidator.ValidateParameters(importData[importIndex]);
                        this.AddToDictionaries(importData[importIndex]);
                        list.Add(importData[importIndex]);
                        importIndex++;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(Resource.GetString("importFailValidation", CultureInfo.InvariantCulture), importData[importIndex].Id, ex.Message);
                        importIndex++;
                        continue;
                    }
                }
            }

            for (; importIndex < importData.Count; importIndex++)
            {
                try
                {
                    this.recordValidator.ValidateParameters(importData[importIndex]);
                    list.Add(importData[importIndex]);
                    this.AddToDictionaries(importData[importIndex]);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(Resource.GetString("importFailValidation", CultureInfo.InvariantCulture), importData[importIndex].Id, ex.Message);
                    continue;
                }
            }

            for (; sourceIndex < this.list.Count; sourceIndex++)
            {
                list.Add(this.list[sourceIndex]);
            }

            this.list = list;

            return this.list.Count;
        }

        /// <summary>
        /// Do defragmetation.
        /// </summary>
        public void Purge()
        {
            return;
        }

        private void RemoveRecordFromDictionaries(FileCabinetRecord record)
        {
            this.firstNameDictionary[record.FirstName].Remove(record);
            this.lastNameDictionary[record.LastName].Remove(record);
            this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
        }

        private void AddToDictionaries(FileCabinetRecord record)
        {
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
        }
    }
}
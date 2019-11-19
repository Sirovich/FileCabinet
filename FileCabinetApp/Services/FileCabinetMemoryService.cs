using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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

        private readonly List<int> idсache = new List<int>();

        private List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private IRecordValidator recordValidator;

        private int maxId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="recordValidator">Source validator.</param>
        public FileCabinetMemoryService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
            this.maxId = 0;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        /// <inheritdoc/>
        public bool RemoveRecord(int id)
        {
            FileCabinetRecord temp = null;

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    temp = record;
                    this.idсache.Remove(temp.Id);
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                foreach (var record in this.firstNameDictionary[firstName])
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Finds all records with this last name.
        /// </summary>
        /// <param name="lastName">Last name to search.</param>
        /// <returns>Array of records with this last name.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                foreach (var record in this.lastNameDictionary[lastName])
                {
                    yield return record;
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            DateTime date = default;
            if (DateTime.TryParse(dateOfBirth, out date))
            {
                if (this.dateOfBirthDictionary.ContainsKey(date))
                {
                    foreach (var record in this.dateOfBirthDictionary[date])
                    {
                        yield return record;
                    }
                }
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null || snapshot.Records is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            int count = 0;
            var importData = snapshot.Records;

            foreach (var record in importData)
            {
                var validationResult = this.recordValidator.ValidateParameters(record);
                if (!validationResult.Item1)
                {
                    Console.WriteLine(Resource.GetString("importFailValidation", CultureInfo.InvariantCulture), record.Id, validationResult.Item2);
                    continue;
                }

                if (this.idсache.Contains(record.Id))
                {
                    var temp = this.list.Find(x => x.Id == record.Id);
                    this.RemoveRecordFromDictionaries(temp);
                    temp.FirstName = record.FirstName;
                    temp.LastName = record.LastName;
                    temp.Sex = record.Sex;
                    temp.Weight = record.Weight;
                    temp.Height = record.Height;
                    this.AddToDictionaries(temp);
                    count++;
                }
                else
                {
                    if (record.Id > this.maxId)
                    {
                        this.maxId = record.Id;
                    }

                    this.idсache.Add(record.Id);
                    this.list.Add(record);
                    this.AddToDictionaries(record);
                    count++;
                }
            }

            return count;
        }

        /// <inheritdoc/>
        public void Purge()
        {
            return;
        }

        /// <inheritdoc/>
        public bool Insert(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (!this.recordValidator.ValidateParameters(record).Item1)
            {
                Console.WriteLine(this.recordValidator.ValidateParameters(record).Item2);
                return false;
            }

            if (this.idсache.Contains(record.Id))
            {
                Console.WriteLine(Source.Resource.GetString("idAlreadyExists", CultureInfo.InvariantCulture));
                return false;
            }

            this.list.Add(record);
            this.idсache.Add(record.Id);
            return true;
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
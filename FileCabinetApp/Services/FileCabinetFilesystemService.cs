using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using FileCabinetApp.Snapshots;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class provides methods for working with records in filesystem.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private const int StringSize = 122;
        private const int CharSize = 1;
        private const int IntSize = 4;
        private const int ShortSize = 2;
        private const int DecimalSize = 16;
        private const int RecordSize = 281;

        private static readonly ResourceManager Resource = new ResourceManager("FileCabinetApp.res", typeof(Program).Assembly);

        private BinaryWriter fileWriter;
        private BinaryReader fileReader;
        private int lastId = 0;
        private int offset = 0;

        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Source stream.</param>
        /// <param name="recordValidator">Source validator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator recordValidator)
        {
            this.fileWriter = new BinaryWriter(fileStream);
            this.fileReader = new BinaryReader(fileStream);
            this.recordValidator = recordValidator;
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
            this.recordValidator.ValidateParameters(firstName, lastName, dateOfBirth, sex, height, weight, Resource);
            this.lastId++;
            this.offset += ShortSize;
            this.fileWriter.Seek(this.offset, 0);
            this.fileWriter.Write(this.lastId);
            this.offset += IntSize;
            this.fileWriter.Write(firstName);
            this.offset += StringSize;
            this.fileWriter.Seek(this.offset, 0);
            this.fileWriter.Write(lastName);
            this.offset += StringSize;
            this.fileWriter.Seek(this.offset, 0);
            this.fileWriter.Write(dateOfBirth.Day);
            this.offset += IntSize;
            this.fileWriter.Write(dateOfBirth.Month);
            this.offset += IntSize;
            this.fileWriter.Write(dateOfBirth.Year);
            this.offset += IntSize;
            this.fileWriter.Write(sex);
            this.offset += CharSize;
            this.fileWriter.Write(weight);
            this.offset += DecimalSize;
            this.fileWriter.Write(height);
            this.offset += ShortSize;
            return this.lastId;
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
            this.recordValidator.ValidateParameters(firstName, lastName, dateOfBirth, sex, height, weight, Resource);
            int localOffset = (id - 1) * RecordSize;

            this.fileWriter.Seek(localOffset, 0);
            if (this.fileReader.ReadBoolean())
            {
                return;
            }

            this.fileWriter.Seek(localOffset + ShortSize, 0);
            this.fileWriter.Write(this.lastId);
            localOffset += IntSize;
            this.fileWriter.Write(firstName);
            localOffset += StringSize;
            this.fileWriter.Seek(localOffset, 0);
            this.fileWriter.Write(lastName);
            localOffset += StringSize;
            this.fileWriter.Seek(localOffset, 0);
            this.fileWriter.Write(dateOfBirth.Day);
            this.fileWriter.Write(dateOfBirth.Month);
            this.fileWriter.Write(dateOfBirth.Year);
            this.fileWriter.Write(sex);
            this.fileWriter.Write(weight);
            this.fileWriter.Write(height);
        }

        /// <summary>
        /// Finds all records with this date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search.</param>
        /// <returns>Array of records with this date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            DateTime date;
            if (!DateTime.TryParse(dateOfBirth, out date))
            {
                return null;
            }

            const int datePosition = ShortSize + IntSize + StringSize + StringSize;
            int currentId = 1;
            int fileLength = Convert.ToInt32(this.fileReader.BaseStream.Length);
            int localOffset = 0;
            var list = new List<FileCabinetRecord>();

            int dayDefault = 1;
            int monthDefault = 1;
            int yearDefault = 0001;

            while (localOffset < fileLength)
            {
                DateTime temp = new DateTime(yearDefault, monthDefault, dayDefault);
                this.fileReader.BaseStream.Seek(localOffset, 0);

                if (this.fileReader.ReadBoolean())
                {
                    localOffset += RecordSize;
                    continue;
                }

                this.fileReader.BaseStream.Seek(localOffset + ShortSize, 0);
                currentId = this.fileReader.ReadInt32();
                this.fileReader.BaseStream.Seek(localOffset + datePosition, 0);
                int day = this.fileReader.ReadInt32();
                int month = this.fileReader.ReadInt32();
                int year = this.fileReader.ReadInt32();

                temp = temp.AddDays(day - 1);
                temp = temp.AddMonths(month - 1);
                temp = temp.AddYears(year - 1);

                if (DateTime.Compare(temp, date) == 0)
                {
                    list.Add(this.GetRecord(localOffset));
                }

                localOffset += RecordSize;
            }

            return list.AsReadOnly();
        }

        /// <summary>
        /// Finds all records with this first name.
        /// </summary>
        /// <param name="firstName">First name to search.</param>
        /// <returns>Array of records with this first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            int currentId;
            int fileLength = Convert.ToInt32(this.fileReader.BaseStream.Length);
            int localOffset = 0;
            var list = new List<FileCabinetRecord>();
            string temp = null;

            while (localOffset < fileLength)
            {
                this.fileReader.BaseStream.Seek(localOffset, 0);

                if (this.fileReader.ReadBoolean())
                {
                    localOffset += RecordSize;
                    continue;
                }

                this.fileReader.BaseStream.Seek(localOffset + ShortSize, 0);
                currentId = this.fileReader.ReadInt32();
                temp = this.fileReader.ReadString();

                if (temp.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    list.Add(this.GetRecord(localOffset));
                }

                localOffset += RecordSize;
            }

            return list.AsReadOnly();
        }

        /// <summary>
        /// Finds all records with this last name.
        /// </summary>
        /// <param name="lastName">Last name to search.</param>
        /// <returns>Array of records with this last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            const int lastNamePosition = ShortSize + IntSize + StringSize;
            int currentId;
            int fileLength = Convert.ToInt32(this.fileReader.BaseStream.Length);
            int localOffset = 0;
            var list = new List<FileCabinetRecord>();
            string temp = null;

            while (localOffset < fileLength)
            {
                this.fileReader.BaseStream.Seek(localOffset, 0);

                if (this.fileReader.ReadBoolean())
                {
                    localOffset += RecordSize;
                    continue;
                }

                this.fileReader.BaseStream.Seek(localOffset + ShortSize, 0);
                currentId = this.fileReader.ReadInt32();
                this.fileReader.BaseStream.Seek(localOffset + lastNamePosition, 0);
                temp = this.fileReader.ReadString();
                if (temp.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    list.Add(this.GetRecord(localOffset));
                }

                localOffset += RecordSize;
            }

            return list.AsReadOnly();
        }

        /// <summary>
        /// Gets array of records.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var list = new List<FileCabinetRecord>();
            long offset = 0;
            this.fileReader.BaseStream.Seek(offset, 0);
            while (offset < this.fileReader.BaseStream.Length)
            {
                this.fileReader.BaseStream.Seek(offset, 0);
                var tempRecord = new FileCabinetRecord();
                if (this.fileReader.ReadBoolean())
                {
                    offset += RecordSize;
                    continue;
                }

                offset += ShortSize;
                this.fileReader.BaseStream.Seek(offset, 0);
                tempRecord.Id = this.fileReader.ReadInt32();
                offset += IntSize;
                tempRecord.FirstName = this.fileReader.ReadString();
                this.fileReader.BaseStream.Seek(offset + StringSize, 0);
                tempRecord.LastName = this.fileReader.ReadString();
                offset += StringSize;
                this.fileReader.BaseStream.Seek(offset + StringSize, 0);
                int day = this.fileReader.ReadInt32();
                int month = this.fileReader.ReadInt32();
                int year = this.fileReader.ReadInt32();
                tempRecord.DateOfBirth = tempRecord.DateOfBirth.AddDays(day - 1);
                tempRecord.DateOfBirth = tempRecord.DateOfBirth.AddMonths(month - 1);
                tempRecord.DateOfBirth = tempRecord.DateOfBirth.AddYears(year - 1);
                tempRecord.Sex = this.fileReader.ReadChar();
                tempRecord.Weight = this.fileReader.ReadDecimal();
                tempRecord.Height = this.fileReader.ReadInt16();
                offset = this.fileReader.BaseStream.Position;
                list.Add(tempRecord);
            }

            return list.AsReadOnly();
        }

        /// <summary>
        /// Gets count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat()
        {
            return Convert.ToInt32(this.fileReader.BaseStream.Length) / RecordSize;
        }

        /// <summary>
        /// Captures the status of the service.
        /// </summary>
        /// <returns>Returns snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>(this.GetRecords()).ToArray());
        }

        /// <summary>
        /// Implementation of IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Restore records from snapshot.
        /// </summary>
        /// <param name="snapshot">Source snapshot.</param>
        /// <returns>Number of imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var list = new List<FileCabinetRecord>();
            var importData = snapshot.Records;
            var source = this.GetRecords();

            int sourceIndex = 0;
            int importIndex = 0;

            for (; sourceIndex < source.Count && importIndex < importData.Count;)
            {
                if (source[sourceIndex].Id < importData[importIndex].Id)
                {
                    list.Add(source[sourceIndex]);
                    sourceIndex++;
                }
                else if (source[sourceIndex].Id == importData[importIndex].Id)
                {
                    try
                    {
                        this.recordValidator.ValidateParameters(importData[importIndex], Resource);
                        list.Add(importData[importIndex]);
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
                        this.recordValidator.ValidateParameters(importData[importIndex], Resource);
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
                    this.recordValidator.ValidateParameters(importData[importIndex], Resource);
                    list.Add(importData[importIndex]);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(Resource.GetString("importFailValidation", CultureInfo.InvariantCulture), importData[importIndex].Id, ex.Message);
                    continue;
                }
            }

            for (; sourceIndex < source.Count; sourceIndex++)
            {
                list.Add(source[sourceIndex]);
            }

            this.lastId = list[^1].Id;
            this.WriteImportToFile(list);

            return list.Count;
        }

        /// <summary>
        /// Remove record.
        /// </summary>
        /// <param name="id">Source id.</param>
        /// <returns>True if record with source id is exist.</returns>
        public bool RemoveRecord(int id)
        {
            int offset = ShortSize;
            this.fileReader.BaseStream.Seek(offset, 0);
            while (offset < this.fileReader.BaseStream.Length)
            {
                this.fileReader.BaseStream.Seek(offset, 0);

                int currentId = this.fileReader.ReadInt32();
                if (id == currentId)
                {
                    this.fileReader.BaseStream.Seek(offset - ShortSize, 0);
                    this.fileWriter.Write(true);
                    return true;
                }

                offset += RecordSize;
            }

            return false;
        }

        /// <summary>
        /// Do defragmentation.
        /// </summary>
        public void Purge()
        {
            int offset = 0;
            int positionOfDeleted = 0;

            while (offset < this.fileReader.BaseStream.Length)
            {
                this.fileReader.BaseStream.Seek(offset, 0);

                if (this.fileReader.ReadBoolean())
                {
                    positionOfDeleted = offset;
                    offset += RecordSize;
                    while (offset < this.fileReader.BaseStream.Length)
                    {
                        this.fileReader.BaseStream.Seek(offset, 0);
                        if (!this.fileReader.ReadBoolean())
                        {
                            var record = this.GetRecord(offset);
                            this.fileWriter.BaseStream.Seek(offset, 0);
                            this.fileWriter.Write(true);
                            this.WriteToFile(record, positionOfDeleted);
                            offset = positionOfDeleted;
                            break;
                        }

                        offset += RecordSize;
                    }

                    if (offset >= this.fileReader.BaseStream.Length)
                    {
                        this.fileReader.BaseStream.SetLength(positionOfDeleted);
                        this.offset = positionOfDeleted;
                    }
                }

                offset += RecordSize;
            }
        }

        /// <summary>
        /// Performs the actual work of releasing resources.
        /// </summary>
        /// <param name="disposing">Bool parameter.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.fileWriter.Close();
                this.fileReader.Close();
            }
        }

        private FileCabinetRecord GetRecord(int offset)
        {
            offset += ShortSize;
            this.fileReader.BaseStream.Seek(offset, 0);
            var tempRecord = new FileCabinetRecord();
            tempRecord.Id = this.fileReader.ReadInt32();
            offset += IntSize;
            tempRecord.FirstName = this.fileReader.ReadString();
            this.fileReader.BaseStream.Seek(offset + StringSize, 0);
            tempRecord.LastName = this.fileReader.ReadString();
            offset += StringSize;
            this.fileReader.BaseStream.Seek(offset + StringSize, 0);
            int day = this.fileReader.ReadInt32();
            int month = this.fileReader.ReadInt32();
            int year = this.fileReader.ReadInt32();
            tempRecord.DateOfBirth = tempRecord.DateOfBirth.AddDays(day - 1);
            tempRecord.DateOfBirth = tempRecord.DateOfBirth.AddMonths(month - 1);
            tempRecord.DateOfBirth = tempRecord.DateOfBirth.AddYears(year - 1);
            tempRecord.Sex = this.fileReader.ReadChar();
            tempRecord.Weight = this.fileReader.ReadDecimal();
            tempRecord.Height = this.fileReader.ReadInt16();

            return tempRecord;
        }

        private void WriteImportToFile(List<FileCabinetRecord> records)
        {
            this.fileWriter.BaseStream.Seek(0, 0);
            this.offset = 0;
            foreach (var record in records)
            {
                this.offset += ShortSize;
                this.fileWriter.Seek(this.offset, 0);
                this.fileWriter.Write(record.Id);
                this.offset += IntSize;
                this.fileWriter.Write(record.FirstName);
                this.offset += StringSize;
                this.fileWriter.Seek(this.offset, 0);
                this.fileWriter.Write(record.LastName);
                this.offset += StringSize;
                this.fileWriter.Seek(this.offset, 0);
                this.fileWriter.Write(record.DateOfBirth.Day);
                this.offset += IntSize;
                this.fileWriter.Write(record.DateOfBirth.Month);
                this.offset += IntSize;
                this.fileWriter.Write(record.DateOfBirth.Year);
                this.offset += IntSize;
                this.fileWriter.Write(record.Sex);
                this.offset += CharSize;
                this.fileWriter.Write(record.Weight);
                this.offset += DecimalSize;
                this.fileWriter.Write(record.Height);
                this.offset += ShortSize;
            }
        }

        private void WriteToFile(FileCabinetRecord record, int offset)
        {
            this.fileWriter.Seek(offset, 0);
            this.fileWriter.Write(false);
            offset += ShortSize;
            this.fileWriter.Seek(offset, 0);
            this.fileWriter.Write(record.Id);
            offset += IntSize;
            this.fileWriter.Write(record.FirstName);
            offset += StringSize;
            this.fileWriter.Seek(offset, 0);
            this.fileWriter.Write(record.LastName);
            offset += StringSize;
            this.fileWriter.Seek(offset, 0);
            this.fileWriter.Write(record.DateOfBirth.Day);
            this.fileWriter.Write(record.DateOfBirth.Month);
            this.fileWriter.Write(record.DateOfBirth.Year);
            this.fileWriter.Write(record.Sex);
            this.fileWriter.Write(record.Weight);
            this.fileWriter.Write(record.Height);
        }
    }
}

﻿using System;
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
        private const int IntSize = 4;
        private const int ShortSize = 2;
        private const int RecordSize = 281;

        private readonly Dictionary<int, int> idсache = new Dictionary<int, int>();

        private BinaryWriter fileWriter;
        private BinaryReader fileReader;
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

        /// <inheritdoc/>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight)
        {
            var temp = new FileCabinetRecord
            {
                Sex = sex,
                Weight = weight,
                Height = height,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };

            if (!this.recordValidator.ValidateParameters(temp).Item1)
            {
                throw new ArgumentException(this.recordValidator.ValidateParameters(temp).Item2);
            }

            int localOffset = (id - 1) * RecordSize;

            this.fileWriter.Seek(localOffset, 0);
            if (this.fileReader.ReadBoolean())
            {
                return;
            }

            this.fileWriter.Seek(localOffset + ShortSize, 0);
            this.fileWriter.Write(id);
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

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            DateTime date;
            if (DateTime.TryParse(dateOfBirth, out date))
            {
                const int datePosition = ShortSize + IntSize + StringSize + StringSize;
                int fileLength = Convert.ToInt32(this.fileReader.BaseStream.Length);
                int localOffset = 0;

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

                    this.fileReader.BaseStream.Seek(localOffset + datePosition, 0);
                    int day = this.fileReader.ReadInt32();
                    int month = this.fileReader.ReadInt32();
                    int year = this.fileReader.ReadInt32();

                    temp = temp.AddDays(day - 1);
                    temp = temp.AddMonths(month - 1);
                    temp = temp.AddYears(year - 1);

                    if (DateTime.Compare(temp, date) == 0)
                    {
                        yield return this.GetRecord(localOffset);
                    }

                    localOffset += RecordSize;
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            int fileLength = Convert.ToInt32(this.fileReader.BaseStream.Length);
            int localOffset = 0;

            while (localOffset < fileLength)
            {
                this.fileReader.BaseStream.Seek(localOffset, 0);

                if (this.fileReader.ReadBoolean())
                {
                    localOffset += RecordSize;
                    continue;
                }

                this.fileReader.BaseStream.Seek(localOffset + ShortSize + IntSize, 0);

                if (this.fileReader.ReadString().Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return this.GetRecord(localOffset);
                }

                localOffset += RecordSize;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            const int lastNamePosition = ShortSize + IntSize + StringSize;
            int fileLength = Convert.ToInt32(this.fileReader.BaseStream.Length);
            int localOffset = 0;

            while (localOffset < fileLength)
            {
                this.fileReader.BaseStream.Seek(localOffset, 0);

                if (this.fileReader.ReadBoolean())
                {
                    localOffset += RecordSize;
                    continue;
                }

                this.fileReader.BaseStream.Seek(localOffset + lastNamePosition, 0);
                if (this.fileReader.ReadString().Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return this.GetRecord(localOffset);
                }

                localOffset += RecordSize;
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public int GetStat()
        {
            return Convert.ToInt32(this.fileReader.BaseStream.Length) / RecordSize;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>(this.GetRecords()).ToArray());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null || snapshot.Records is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var list = new List<FileCabinetRecord>();
            var importData = snapshot.Records;
            var source = this.GetRecords();
            int count = 0;

            foreach (var record in importData)
            {
                var validationResult = this.recordValidator.ValidateParameters(record);
                if (!validationResult.Item1)
                {
                    Console.WriteLine(Source.Resource.GetString("importFailValidation", CultureInfo.InvariantCulture), record.Id, validationResult.Item2);
                    continue;
                }

                if (this.idсache.ContainsKey(record.Id))
                {
                    this.WriteToFile(record, this.idсache[record.Id]);
                    count++;
                }
                else
                {
                    this.WriteToFile(record, this.offset);
                    this.offset += RecordSize;
                    count++;
                }
            }

            return count;
        }

        /// <inheritdoc/>
        public bool RemoveRecord(int id)
        {
            if (this.idсache.ContainsKey(id))
            {
                this.fileReader.BaseStream.Seek(this.idсache[id], 0);
                this.fileWriter.Write(true);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
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

            if (this.idсache.ContainsKey(record.Id))
            {
                this.fileReader.BaseStream.Seek(this.idсache[record.Id], 0);
                if (!this.fileReader.ReadBoolean())
                {
                    Console.WriteLine(Source.Resource.GetString("idAlreadyExists", CultureInfo.InvariantCulture));
                    return false;
                }

                this.WriteToFile(record, this.offset);
                this.offset += RecordSize;
                return true;
            }

            this.WriteToFile(record, this.offset);
            this.idсache.Add(record.Id, this.offset);
            this.offset += RecordSize;
            return true;
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

        private int FindRecordById(int id)
        {
            int localOffset = ShortSize;
            this.fileReader.BaseStream.Seek(localOffset, 0);

            while (this.fileReader.BaseStream.Length > this.fileReader.BaseStream.Position)
            {
                if (this.fileReader.ReadInt32() == id)
                {
                    return localOffset;
                }

                localOffset += RecordSize;
            }

            return localOffset;
        }
    }
}

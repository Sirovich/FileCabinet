using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
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
                    Console.WriteLine(Source.Resource.GetString("importReplaceRecord", CultureInfo.InvariantCulture), record.Id);
                    count++;
                }
                else
                {
                    this.idсache.Add(record.Id, this.offset);
                    this.WriteToFile(record, this.offset);
                    this.offset += RecordSize;
                    count++;
                }
            }

            return count;
        }

        /// <inheritdoc/>
        public void Delete(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var record in records)
            {
                this.RemoveRecord(record.Id);
            }
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
        public void Update(IEnumerable<FileCabinetRecord> records, IEnumerable<IEnumerable<string>> fieldsAndValuesToReplace)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            if (fieldsAndValuesToReplace is null)
            {
                throw new ArgumentNullException(nameof(fieldsAndValuesToReplace));
            }

            foreach (var record in records)
            {
                this.UpdateFields(record, fieldsAndValuesToReplace);
                this.WriteToFile(record, this.idсache[record.Id]);
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

        private void UpdateFields(FileCabinetRecord record, IEnumerable<IEnumerable<string>> fieldsAndValuesToReplace)
        {
            foreach (var pair in fieldsAndValuesToReplace)
            {
                var key = pair.First();
                var value = pair.Last();

                if (key.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine(Source.Resource.GetString("idChange", CultureInfo.InvariantCulture));
                    return;
                }

                if (key.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
                {
                    var source = record.FirstName;
                    record.FirstName = value;
                    var validationResult = this.recordValidator.ValidateParameters(record);
                    if (!validationResult.Item1)
                    {
                        record.FirstName = source;
                        Console.WriteLine(validationResult.Item2, CultureInfo.InvariantCulture);
                        return;
                    }

                    continue;
                }

                if (key.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
                {
                    var source = record.LastName;
                    record.LastName = value;
                    var validationResult = this.recordValidator.ValidateParameters(record);
                    if (!validationResult.Item1)
                    {
                        record.LastName = source;
                        Console.WriteLine(validationResult.Item2, CultureInfo.InvariantCulture);
                        return;
                    }

                    continue;
                }

                if (key.Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
                {
                    DateTime temp;
                    if (!DateTime.TryParse(value, out temp))
                    {
                        Console.WriteLine(Source.Resource.GetString("dateOfBirthException", CultureInfo.InvariantCulture));
                        return;
                    }

                    var source = record.DateOfBirth;
                    record.DateOfBirth = temp;
                    var validationResult = this.recordValidator.ValidateParameters(record);
                    if (!validationResult.Item1)
                    {
                        record.DateOfBirth = source;
                        Console.WriteLine(validationResult.Item2, CultureInfo.InvariantCulture);
                        return;
                    }

                    continue;
                }

                if (key.Equals("sex", StringComparison.InvariantCultureIgnoreCase))
                {
                    char temp;
                    if (!char.TryParse(value, out temp))
                    {
                        Console.WriteLine(Source.Resource.GetString("sexException", CultureInfo.InvariantCulture));
                        return;
                    }

                    var source = record.Sex;
                    record.Sex = temp;
                    var validationResult = this.recordValidator.ValidateParameters(record);
                    if (!validationResult.Item1)
                    {
                        record.Sex = source;
                        Console.WriteLine(validationResult.Item2, CultureInfo.InvariantCulture);
                        return;
                    }

                    continue;
                }

                if (key.Equals("weight", StringComparison.InvariantCultureIgnoreCase))
                {
                    decimal temp;
                    if (!decimal.TryParse(value, out temp))
                    {
                        Console.WriteLine(Source.Resource.GetString("weightException", CultureInfo.InvariantCulture));
                        return;
                    }

                    var source = record.Weight;
                    record.Weight = temp;
                    var validationResult = this.recordValidator.ValidateParameters(record);
                    if (!validationResult.Item1)
                    {
                        record.Weight = source;
                        Console.WriteLine(validationResult.Item2, CultureInfo.InvariantCulture);
                        return;
                    }

                    continue;
                }

                if (key.Equals("height", StringComparison.InvariantCultureIgnoreCase))
                {
                    short temp;
                    if (!short.TryParse(value, out temp))
                    {
                        Console.WriteLine(Source.Resource.GetString("heightException", CultureInfo.InvariantCulture));
                        return;
                    }

                    var source = record.Height;
                    record.Height = temp;
                    var validationResult = this.recordValidator.ValidateParameters(record);
                    if (!validationResult.Item1)
                    {
                        record.Height = source;
                        Console.WriteLine(validationResult.Item2, CultureInfo.InvariantCulture);
                        return;
                    }

                    continue;
                }
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

        private bool RemoveRecord(int id)
        {
            if (this.idсache.ContainsKey(id))
            {
                this.fileReader.BaseStream.Seek(this.idсache[id], 0);
                this.fileWriter.Write(true);
                this.idсache.Remove(id);
                return true;
            }

            return false;
        }
    }
}

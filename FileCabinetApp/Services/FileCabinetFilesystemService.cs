using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.Snapshots;

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

        private BinaryWriter fileWriter;
        private BinaryReader fileReader;
        private int lastId = 0;
        private int offset = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Source stream.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileWriter = new BinaryWriter(fileStream);
            this.fileReader = new BinaryReader(fileStream);
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
            this.fileWriter.Seek(this.offset, 0);
            this.fileWriter.Write(dateOfBirth.Month);
            this.offset += IntSize;
            this.fileWriter.Seek(this.offset, 0);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all records with this date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search.</param>
        /// <returns>Array of records with this date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all records with this first name.
        /// </summary>
        /// <param name="firstName">First name to search.</param>
        /// <returns>Array of records with this first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all records with this last name.
        /// </summary>
        /// <param name="lastName">Last name to search.</param>
        /// <returns>Array of records with this last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
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
            while (this.fileReader.BaseStream.Position < this.fileReader.BaseStream.Length)
            {
                var tempRecord = new FileCabinetRecord();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Captures the status of the service.
        /// </summary>
        /// <returns>Returns snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
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
    }
}

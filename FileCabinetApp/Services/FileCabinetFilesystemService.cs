using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.Snapshots;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class provides methods for working with records in filesystem.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Source stream.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}

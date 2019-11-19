using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.Snapshots;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Contains basic functionality for service.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Captures the status of the service.
        /// </summary>
        /// <returns>Returns snapshot.</returns>
        FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="record">Source record.</param>
        /// <returns>True if complete.</returns>
        bool Insert(FileCabinetRecord record);

        /// <summary>
        /// Remove record.
        /// </summary>
        /// <param name="id">Source id.</param>
        /// <returns>True if record with source id is exist.</returns>
        bool RemoveRecord(int id);

        /// <summary>
        /// Do defragmentation.
        /// </summary>
        void Purge();

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
        void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight);

        /// <summary>
        /// Finds all records with this first name.
        /// </summary>
        /// <param name="firstName">First name to search.</param>
        /// <returns>Array of records with this first name.</returns>
        IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds all records with this last name.
        /// </summary>
        /// <param name="lastName">Last name to search.</param>
        /// <returns>Array of records with this last name.</returns>
        IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds all records with this date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search.</param>
        /// <returns>Array of records with this date of birth.</returns>
        IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// Gets array of records.
        /// </summary>
        /// <returns>Array of records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        int GetStat();

        /// <summary>
        /// Import records from file.
        /// </summary>
        /// <param name="snapshot">Service status.</param>
        /// <returns>Number of stored records.</returns>
        int Restore(FileCabinetServiceSnapshot snapshot);
    }
}

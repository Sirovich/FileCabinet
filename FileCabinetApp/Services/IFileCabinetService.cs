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
        /// Deletes records from service.
        /// </summary>
        /// <param name="records">Source records.</param>
        void Delete(IEnumerable<FileCabinetRecord> records);

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="record">Source record.</param>
        /// <returns>True if complete.</returns>
        bool Insert(FileCabinetRecord record);

        /// <summary>
        /// Do defragmentation.
        /// </summary>
        void Purge();

        /// <summary>
        /// Updates records.
        /// </summary>
        /// <param name="records">Records to update.</param>
        /// <param name="fieldsAndValuesToReplace">Fields and values to update.</param>
        void Update(IEnumerable<FileCabinetRecord> records, IEnumerable<IEnumerable<string>> fieldsAndValuesToReplace);

        /// <summary>
        /// Gets array of records.
        /// </summary>
        /// <returns>Array of records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        Tuple<int, int> GetStat();

        /// <summary>
        /// Import records from file.
        /// </summary>
        /// <param name="snapshot">Service status.</param>
        /// <returns>Number of stored records.</returns>
        int Restore(FileCabinetServiceSnapshot snapshot);
    }
}

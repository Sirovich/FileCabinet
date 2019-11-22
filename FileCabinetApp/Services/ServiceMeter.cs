using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using FileCabinetApp.Snapshots;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Service meter.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Source service.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var watch = Stopwatch.StartNew();
            var result = this.service.MakeSnapshot();
            this.ShowTime(Source.Resource.GetString("makeSnapshotTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
            return result;
        }

        /// <inheritdoc/>
        public void Purge()
        {
            var watch = Stopwatch.StartNew();
            this.service.Purge();
            this.ShowTime(Source.Resource.GetString("purgeTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
        }

        /// <inheritdoc/>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight)
        {
            var watch = Stopwatch.StartNew();
            this.service.EditRecord(id, firstName, lastName, dateOfBirth, sex, height, weight);
            this.ShowTime(Source.Resource.GetString("editTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var watch = Stopwatch.StartNew();
            var result = this.service.FindByFirstName(firstName);
            this.ShowTime(Source.Resource.GetString("findFirstNameTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var watch = Stopwatch.StartNew();
            var result = this.service.FindByLastName(lastName);
            this.ShowTime(Source.Resource.GetString("findLastNameTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var watch = Stopwatch.StartNew();
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            this.ShowTime(Source.Resource.GetString("findDateTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var watch = Stopwatch.StartNew();
            var result = this.service.GetRecords();
            this.ShowTime(Source.Resource.GetString("getRecordsTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
            return result;
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            var watch = Stopwatch.StartNew();
            var result = this.service.GetStat();
            this.ShowTime(Source.Resource.GetString("getStatTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
            return result;
        }

        /// <inheritdoc/>
        public bool Insert(FileCabinetRecord record)
        {
            var watch = Stopwatch.StartNew();
            var result = this.service.Insert(record);
            this.ShowTime(Source.Resource.GetString("insertTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
            return result;
        }

        /// <inheritdoc/>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            var watch = Stopwatch.StartNew();
            var result = this.service.Restore(snapshot);
            this.ShowTime(Source.Resource.GetString("restoreTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
            return result;
        }

        private void ShowTime(string message, long milliseconds)
        {
            Console.WriteLine(message, milliseconds);
        }

        public void Delete(IEnumerable<FileCabinetRecord> records)
        {
            var watch = Stopwatch.StartNew();
            this.service.Delete(records);
            this.ShowTime(Source.Resource.GetString("removeTime", CultureInfo.InvariantCulture), watch.ElapsedTicks);
        }
    }
}

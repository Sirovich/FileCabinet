using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.Services;
using FileCabinetApp.Snapshots;

namespace FileCabinetApp.Loggers
{
    /// <summary>
    /// Service logger.
    /// </summary>
    public sealed class ServiceLogger : IFileCabinetService, IDisposable
    {
        private IFileCabinetService service;
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">Source service.</param>
        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service;
            this.writer = new StreamWriter(@"Work.log");
            this.writer.AutoFlush = true;
        }

        /// <inheritdoc/>
        public int CreateRecord(short height, decimal weight, char sex, string firstName, string lastName, DateTime dateOfBirth)
        {
            this.writer.WriteLine(Source.Resource.GetString("createLog", CultureInfo.InvariantCulture), DateTime.Now, firstName, lastName, dateOfBirth, sex, weight, height);
            var id = this.service.CreateRecord(height, weight, sex, firstName, lastName, dateOfBirth);
            this.writer.WriteLine(Source.Resource.GetString("createResultLog", CultureInfo.InvariantCulture), DateTime.Now, id);
            return id;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, short height, decimal weight)
        {
            this.writer.WriteLine(Source.Resource.GetString("editLog", CultureInfo.InvariantCulture), DateTime.Now, firstName, lastName, dateOfBirth, sex, weight, height);
            this.service.CreateRecord(height, weight, sex, firstName, lastName, dateOfBirth);
            this.writer.WriteLine(Source.Resource.GetString("editResultLog", CultureInfo.InvariantCulture), DateTime.Now);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.writer.WriteLine(Source.Resource.GetString("findDateLog", CultureInfo.InvariantCulture), DateTime.Now, dateOfBirth);
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            this.writer.WriteLine(Source.Resource.GetString("findDateResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.writer.WriteLine(Source.Resource.GetString("findFirstNameLog", CultureInfo.InvariantCulture), DateTime.Now, firstName);
            var result = this.service.FindByFirstName(firstName);
            this.writer.WriteLine(Source.Resource.GetString("findFirstNameResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.writer.WriteLine(Source.Resource.GetString("findLastNameLog", CultureInfo.InvariantCulture), DateTime.Now, lastName);
            var result = this.service.FindByLastName(lastName);
            this.writer.WriteLine(Source.Resource.GetString("findLastNameResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.writer.WriteLine(Source.Resource.GetString("getRecordsLog", CultureInfo.InvariantCulture), DateTime.Now);
            var result = this.service.GetRecords();
            this.writer.WriteLine(Source.Resource.GetString("getRecordsResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            return result;
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            this.writer.WriteLine(Source.Resource.GetString("getStatLog", CultureInfo.InvariantCulture), DateTime.Now);
            var result = this.service.GetStat();
            this.writer.WriteLine(Source.Resource.GetString("getStatResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            return result;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.writer.WriteLine(Source.Resource.GetString("makeSnapshotLog", CultureInfo.InvariantCulture), DateTime.Now);
            var result = this.service.MakeSnapshot();
            this.writer.WriteLine(Source.Resource.GetString("makeSnapshotResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            return result;
        }

        /// <inheritdoc/>
        public void Purge()
        {
            this.writer.WriteLine(Source.Resource.GetString("purgeLog", CultureInfo.InvariantCulture), DateTime.Now);
            this.service.Purge();
            this.writer.WriteLine(Source.Resource.GetString("purgeResultLog", CultureInfo.InvariantCulture), DateTime.Now);
        }

        /// <inheritdoc/>
        public bool RemoveRecord(int id)
        {
            this.writer.WriteLine(Source.Resource.GetString("removeLog", CultureInfo.InvariantCulture), DateTime.Now, id);
            var result = this.service.RemoveRecord(id);
            if (result)
            {
                this.writer.WriteLine(Source.Resource.GetString("removeResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            }
            else
            {
                this.writer.WriteLine(Source.Resource.GetString("removeFailedResultLog", CultureInfo.InvariantCulture), DateTime.Now, id);
            }

            return result;
        }

        /// <inheritdoc/>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.writer.WriteLine(Source.Resource.GetString("restoreLog", CultureInfo.InvariantCulture), DateTime.Now);
            var result = this.service.Restore(snapshot);
            this.writer.WriteLine(Source.Resource.GetString("restoreResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            return result;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.writer.Close();
        }
    }
}

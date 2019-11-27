using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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

        /// <inheritdoc/>
        public bool Insert(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.writer.WriteLine(Source.Resource.GetString("insertLog", CultureInfo.InvariantCulture), DateTime.Now, record.Id, record.FirstName, record.LastName, record.Sex, record.Weight, record.Height);
            var result = this.service.Insert(record);
            if (result)
            {
                this.writer.WriteLine(Source.Resource.GetString("insertResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            }
            else
            {
                this.writer.WriteLine(Source.Resource.GetString("insertFailedResultLog", CultureInfo.InvariantCulture), DateTime.Now);
            }

            return result;
        }

        /// <inheritdoc/>
        public void Delete(IEnumerable<FileCabinetRecord> records)
        {
            this.writer.WriteLine(Source.Resource.GetString("removeLog", CultureInfo.InvariantCulture), DateTime.Now, 1);
            this.service.Delete(records);
            this.writer.WriteLine(Source.Resource.GetString("removeResultLog", CultureInfo.InvariantCulture), DateTime.Now);
        }

        /// <inheritdoc/>
        public void Update(IEnumerable<FileCabinetRecord> records, IEnumerable<IEnumerable<string>> fieldsAndValuesToReplace)
        {
            this.writer.WriteLine(Source.Resource.GetString("updateLog", CultureInfo.InvariantCulture), DateTime.Now);
            this.service.Update(records, fieldsAndValuesToReplace);
            this.writer.WriteLine(Source.Resource.GetString("updateResultLog", CultureInfo.InvariantCulture), DateTime.Now);
        }
    }
}

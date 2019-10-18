using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Writers
{
    /// <summary>
    /// Class for write to csv file.
    /// </summary>
    public class FileCabinetRecordCsvWriter : IWriter, IDisposable
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Source stream.</param>
        public FileCabinetRecordCsvWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Implementation of <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            this.writer.Dispose();
        }

        /// <summary>
        /// Write record to csv file.
        /// </summary>
        /// <param name="record">Source record.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var result = new StringBuilder();
            result.AppendLine(string.Format(CultureInfo.InvariantCulture, $"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)},{record.Sex},{record.Weight},{record.Height}."));

            this.writer.Write(result.ToString());
        }
    }
}

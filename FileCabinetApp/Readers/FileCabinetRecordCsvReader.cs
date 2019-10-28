using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// FileCabinetRecordCsvReader.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private StreamReader fileReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="fileReader">Source stream reader.</param>
        public FileCabinetRecordCsvReader(StreamReader fileReader)
        {
            this.fileReader = fileReader;
        }

        /// <summary>
        /// Read records from file.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var list = new List<FileCabinetRecord>();
            this.fileReader.BaseStream.Seek(0, 0);
            var names = this.fileReader.ReadLine().Split(',');

            while (!this.fileReader.EndOfStream)
            {
                var fields = this.fileReader.ReadLine().Split(',');
                var record = new FileCabinetRecord();
                record.Id = int.Parse(fields[0], CultureInfo.InvariantCulture);
                record.FirstName = fields[1];
                record.LastName = fields[2];
                record.DateOfBirth = DateTime.Parse(fields[3], CultureInfo.InvariantCulture);
                record.Sex = char.Parse(fields[4]);
                record.Weight = decimal.Parse(fields[5], CultureInfo.InvariantCulture);
                record.Height = short.Parse(fields[6].Split('.')[0], CultureInfo.InvariantCulture);
                list.Add(record);
            }

            return list;
        }
    }
}

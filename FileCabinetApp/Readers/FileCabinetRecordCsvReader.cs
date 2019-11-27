using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

            var names = this.fileReader.ReadLine()?.Split(',', '.');
            if (names is null || names.Length < 6)
            {
                Console.WriteLine(Source.Resource.GetString("badCsvFile", CultureInfo.InvariantCulture));
                return null;
            }

            int firstNameIndex = Array.IndexOf(names, "First Name");
            int lastNameIndex = Array.IndexOf(names, "Last Name");
            int dateIndex = Array.IndexOf(names, "Date of Birth");
            int idindex = Array.IndexOf(names, "Id");
            int sexIndex = Array.IndexOf(names, "Sex");
            int weightIndex = Array.IndexOf(names, "Weight");
            int heightIndex = Array.IndexOf(names, "Height");
            while (!this.fileReader.EndOfStream)
            {
                try
                {
                    var fields = this.fileReader.ReadLine()?.Split(',');
                    for (int i = 0; i < fields.Length; i++)
                    {
                        fields[i] = fields[i].Trim(' ', '.');
                    }

                    var record = new FileCabinetRecord();
                    record.Id = int.Parse(fields[idindex], CultureInfo.InvariantCulture);
                    record.FirstName = fields[firstNameIndex];
                    record.LastName = fields[lastNameIndex];
                    record.DateOfBirth = DateTime.Parse(fields[dateIndex], CultureInfo.InvariantCulture);
                    record.Sex = char.Parse(fields[sexIndex]);
                    record.Weight = decimal.Parse(fields[weightIndex], CultureInfo.InvariantCulture);
                    record.Height = short.Parse(fields[heightIndex], CultureInfo.InvariantCulture);
                    list.Add(record);
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine(Source.Resource.GetString("badCsvFile", CultureInfo.InvariantCulture));
                    return null;
                }
            }

            return list;
        }
    }
}

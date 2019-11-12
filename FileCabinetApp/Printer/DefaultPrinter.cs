using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp.Printer
{
    /// <summary>
    /// Default printer.
    /// </summary>
    public class DefaultPrinter : IRecordPrinter
    {
        /// <inheritdoc/>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Sex}, {record.Weight}, {record.Height}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}");
            }
        }
    }
}

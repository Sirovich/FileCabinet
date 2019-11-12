using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Printer
{
    /// <summary>
    /// Interface for printers.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints records.
        /// </summary>
        /// <param name="records">Source records.</param>
        void Print(IEnumerable<FileCabinetRecord> records);
    }
}

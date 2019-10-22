using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp;

namespace FileCabinet.Writers
{
    public class CsvWriter
    {
        StreamWriter fileStream;
        FileCabinetRecord[] records;

        public CsvWriter(StreamWriter fileStream, FileCabinetRecord[] records)
        {
            this.fileStream = fileStream;
            this.records = records;
        }

        public void Write()
        {
            this.fileStream.WriteLine("Id,First Name,Last Name,Date of Birth,Sex,Weight,Height");
            foreach (var record in this.records)
            {
                var result = new StringBuilder();
                result.AppendLine(string.Format(CultureInfo.InvariantCulture, $"{record.Id},{record.Name.FirstName},{record.Name.LastName},{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)},{record.Sex},{record.Weight},{record.Height}."));
                
                this.fileStream.Write(result.ToString());
            }
        }
    }
}

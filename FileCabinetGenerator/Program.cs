using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CommandLine;
using FileCabinet.Writers;
using FileCabinetApp;


namespace FileCabinet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = GetCommandLineArguments(args);
            var records = Generate(options.StartId, options.Amount);
            
            if (options.Type.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                var fileWriter = new StreamWriter(options.FileName);
                var csvWriter = new CsvWriter(fileWriter, records.ToArray());
                csvWriter.Write();
                fileWriter.Close();
            }
            else if(options.Type.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };
                using (var fileWriter = XmlWriter.Create(options.FileName, settings))
                {
                    var xmlWriter = new XmlWriters(fileWriter, records.ToArray());
                    xmlWriter.Write();
                }
            }
        }

        public static Options GetCommandLineArguments(string[] args)
        {
            if (args is null)
            {
                return null;
            }

            var opts = new Options();
            var result = Parser.Default.ParseArguments<Options>(args).WithParsed(parsed => opts = parsed);

            return opts;
        }

        public static List<FileCabinetRecord> Generate(int startId, int amount)
        {
            var list = new List<FileCabinetRecord>();
            var random = new Random();
            for (int i = 0; i < amount; i++)
            {
                var record = new FileCabinetRecord();
                record.Id = startId;
                startId++;
                record.Name.FirstName = RandomString(random.Next(4, 60));
                record.Name.LastName = RandomString(random.Next(4, 60));
                var day = random.Next(1, 30);
                var month = random.Next(1, 12);
                var year = random.Next(1950, 2019);
                record.Sex = (char)random.Next(64, 100);
                record.Weight = random.Next(30, 200);
                record.Height = Convert.ToInt16(random.Next(120, 240));
                record.DateOfBirth = record.DateOfBirth.AddDays(day - 1);
                record.DateOfBirth = record.DateOfBirth.AddMonths(month - 1);
                record.DateOfBirth = record.DateOfBirth.AddYears(year - 1);
                list.Add(record);
            }

            return list;
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

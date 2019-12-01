using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using CommandLine;
using FileCabinet.Writers;
using FileCabinetApp;


namespace FileCabinet
{
    public class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Input parameters.</param>
        public static void Main(string[] args)
        {
            var options = GetCommandLineArguments(args);
            var records = Generate(options.StartId, options.Amount);

            var drive = Path.GetPathRoot(options.FileName);

            if (drive.Trim(' ', '\\').Length != 0 && !Environment.GetLogicalDrives().Contains(drive, StringComparer.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("No such disk drive");
                return;
            }

            var path = Path.GetDirectoryName(options.FileName);

            if (path.Trim(' ', '\\').Length != 0 && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (options.Type.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    var fileWriter = new StreamWriter(options.FileName);
                    var csvWriter = new CsvWriter(fileWriter, records.ToArray());
                    csvWriter.Write();
                    fileWriter.Close();
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else if (options.Type.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                try
                {
                    using (var fileWriter = XmlWriter.Create(options.FileName, settings))
                    {
                        var xmlWriter = new XmlWriters(fileWriter, records.ToArray());
                        xmlWriter.Write();
                    }
                }
                catch(UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }

        private static Options GetCommandLineArguments(string[] args)
        {
            if (args is null)
            {
                return null;
            }

            var opts = new Options();
            var result = Parser.Default.ParseArguments<Options>(args).WithParsed(parsed => opts = parsed);
            
            if (result.Tag.ToString().Equals("NotParsed"))
            {
                Environment.Exit(1488);
            }
            return opts;
        }

        private static List<FileCabinetRecord> Generate(int startId, int amount)
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

        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHI JKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

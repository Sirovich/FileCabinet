using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Readers.Models;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// FileCabinetRecordXmlReader.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private XmlReader xmlReader;
        private FileCabinetRecordsXmlModel records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="xmlReader">Source xml reader.</param>
        public FileCabinetRecordXmlReader(XmlReader xmlReader)
        {
            this.xmlReader = xmlReader;
        }

        /// <summary>
        /// Read records from file.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FileCabinetRecordsXmlModel));
            try
            {
                this.records = (FileCabinetRecordsXmlModel)xmlSerializer.Deserialize(this.xmlReader);

                if (this.records.Records is null)
                {
                    return null;
                }

                return TransformToBaseModel(new List<FileCabinetRecordXmlModel>(this.records.Records));
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static IList<FileCabinetRecord> TransformToBaseModel(List<FileCabinetRecordXmlModel> source)
        {
            var list = new List<FileCabinetRecord>();
            foreach (var element in source)
            {
                var record = new FileCabinetRecord();
                record.Id = element.Id;
                record.FirstName = element.Name.FirstName;
                record.LastName = element.Name.LastName;
                record.Sex = element.Sex[0];
                record.Weight = element.Weight;
                record.Height = element.Height;

                DateTime temp;
                if (!DateTime.TryParse(element.DateOfBirth, out temp))
                {
                    Console.WriteLine(Source.Resource.GetString("dateOfBirthException", CultureInfo.InvariantCulture));
                    continue;
                }

                record.DateOfBirth = temp;
                list.Add(record);
            }

            return list;
        }
    }
}

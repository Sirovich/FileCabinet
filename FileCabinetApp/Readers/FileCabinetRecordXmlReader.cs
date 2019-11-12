using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
            this.records = (FileCabinetRecordsXmlModel)xmlSerializer.Deserialize(this.xmlReader);

            if (this.records.Records is null)
            {
                return null;
            }

            return TransformToBaseModel(new List<FileCabinetRecordXmlModel>(this.records.Records));
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
                record.DateOfBirth = DateTime.ParseExact(element.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                list.Add(record);
            }

            return list;
        }
    }
}

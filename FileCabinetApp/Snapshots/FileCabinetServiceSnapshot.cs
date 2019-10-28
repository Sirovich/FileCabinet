using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using FileCabinetApp.Readers;
using FileCabinetApp.Writers;

namespace FileCabinetApp.Snapshots
{
    /// <summary>
    /// Class to work with service status.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private List<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Array of records.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = new List<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        public FileCabinetServiceSnapshot()
        {
            this.records = new List<FileCabinetRecord>();
        }

        /// <summary>
        /// Gets array of records.
        /// </summary>
        /// <value>Array of records.</value>
        public ReadOnlyCollection<FileCabinetRecord> Records
        {
            get
            {
                return this.records.AsReadOnly();
            }
        }

        /// <summary>
        /// Write records in csv file.
        /// </summary>
        /// <param name="writer">Source stream.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(writer);
            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>
        /// Write records in xml file.
        /// </summary>
        /// <param name="writer">Source stream.</param>
        public void SaveToXml(XmlWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartElement("records");

            var xmlWriter = new FileCabinetRecordXmlWriter(writer);
            foreach (var record in this.records)
            {
                xmlWriter.Write(record);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Read records from file.
        /// </summary>
        /// <param name="streamReader">Source stream reader.</param>
        public void LoadFromCsv(StreamReader streamReader)
        {
            var csvReader = new FileCabinetRecordCsvReader(streamReader);
            this.records = new List<FileCabinetRecord>(csvReader.ReadAll());
        }

        /// <summary>
        /// Read records from file.
        /// </summary>
        /// <param name="xmlReader">Source xml reader.</param>
        public void LoadFromXml(XmlReader xmlReader)
        {
            var fileXmlReader = new FileCabinetRecordXmlReader(xmlReader);
            this.records = new List<FileCabinetRecord>(fileXmlReader.ReadAll());
        }
    }
}

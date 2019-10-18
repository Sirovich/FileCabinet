using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using FileCabinetApp.Writers;

namespace FileCabinetApp.Snapshots
{
    /// <summary>
    /// Class to work with service status.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Array of records.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Write records in csv file.
        /// </summary>
        /// <param name="writer">Source stream.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            using (var csvWriter = new FileCabinetRecordCsvWriter(writer))
            {
                foreach (var record in this.records)
                {
                    csvWriter.Write(record);
                }
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
            using (var xmlWriter = new FileCabinetRecordXmlWriter(writer))
            {
                foreach (var record in this.records)
                {
                    xmlWriter.Write(record);
                }
            }

            writer.WriteEndElement();
        }
    }
}

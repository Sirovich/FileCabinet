using System;
using System.Globalization;
using System.Xml;

namespace FileCabinetApp.Writers
{
    /// <summary>
    /// Class for write to xml file.
    /// </summary>
    public class FileCabinetRecordXmlWriter : IDisposable, IWriter
    {
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Source stream.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Implementation of <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Write record to xml file.
        /// </summary>
        /// <param name="record">Source record.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.writer.WriteStartElement("record");
            this.writer.WriteAttributeString("id", record.Id.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("first", record.FirstName);
            this.writer.WriteAttributeString("last", record.LastName);
            this.writer.WriteEndElement();
            this.writer.WriteStartElement("dateOfBirth");
            this.writer.WriteString(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();
            this.writer.WriteStartElement("Sex");
            this.writer.WriteString(record.Sex.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();
            this.writer.WriteStartElement("Weight");
            this.writer.WriteString(record.Weight.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();
            this.writer.WriteStartElement("Height");
            this.writer.WriteString(record.Height.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();
            this.writer.WriteEndElement();
        }
    }
}

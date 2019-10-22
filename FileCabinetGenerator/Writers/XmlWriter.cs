using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using FileCabinetApp;
using System.Xml;

namespace FileCabinet.Writers
{
    [XmlRoot("records")]
    public sealed class FileCabinetRecords
    {
        [XmlElement("record")]
        public FileCabinetRecord[] records;
    }

    public class XmlWriters
    {
        private XmlWriter fileStream;
        FileCabinetRecords records = new FileCabinetRecords();

        public XmlWriters(XmlWriter fileStream, FileCabinetRecord[] records)
        {
            this.fileStream = fileStream;
            this.records.records = records;
        }

        public void Write()
        {
            XmlSerializerNamespaces emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var xmlSerializer = new XmlSerializer(typeof(FileCabinetRecords));
            xmlSerializer.Serialize(this.fileStream, this.records, emptyNamespaces);
        }
    }
}

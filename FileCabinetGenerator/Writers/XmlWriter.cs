using System.Xml.Serialization;
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

    /// <summary>
    /// Xml writer.
    /// </summary>
    public class XmlWriters
    {
        private XmlWriter fileStream;
        FileCabinetRecords records = new FileCabinetRecords();

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlWriters"/> class.
        /// </summary>
        /// <param name="fileStream">Source fileStream.</param>
        /// <param name="records">Source records.</param>
        public XmlWriters(XmlWriter fileStream, FileCabinetRecord[] records)
        {
            this.fileStream = fileStream;
            this.records.records = records;
        }

        /// <summary>
        /// Write to file.
        /// </summary>
        public void Write()
        {
            XmlSerializerNamespaces emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var xmlSerializer = new XmlSerializer(typeof(FileCabinetRecords));
            xmlSerializer.Serialize(this.fileStream, this.records, emptyNamespaces);
        }
    }
}

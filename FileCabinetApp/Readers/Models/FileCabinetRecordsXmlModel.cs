using System.Xml.Serialization;

namespace FileCabinetApp.Readers.Models
{
    /// <summary>
    /// FileCabinetRecordsXmlModel.
    /// </summary>
    [XmlRoot("records")]
    public class FileCabinetRecordsXmlModel
    {
        /// <summary>
        /// Gets or sets records.
        /// </summary>
        /// <value>Array of records.</value>
        [XmlElement("record")]
        public FileCabinetRecordXmlModel[] Records { get; set; }
    }
}

using System.Collections.Generic;
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
        /// Initializes a new instance of the <see cref="FileCabinetRecordsXmlModel"/> class.
        /// </summary>
        public FileCabinetRecordsXmlModel()
        {
            this.Records = new List<FileCabinetRecordXmlModel>();
        }

        /// <summary>
        /// Gets records.
        /// </summary>
        /// <value>Array of records.</value>
        [XmlElement("record")]
        public List<FileCabinetRecordXmlModel> Records { get; private set; }
    }
}

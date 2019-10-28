using System.Xml.Serialization;

namespace FileCabinetApp.Readers.Models
{
    /// <summary>
    /// FileCabinetRecordXmlName.
    /// </summary>
    public class FileCabinetRecordXmlName
    {
        /// <summary>
        /// Gets or sets person first name.
        /// </summary>
        /// <value>Person first name.</value>
        [XmlAttribute("first")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets preson last name.
        /// </summary>
        /// <value>Person last name.</value>
        [XmlAttribute("last")]
        public string LastName { get; set; }
    }
}

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace FileCabinetApp.Readers.Models
{
    /// <summary>
    /// Contains property.
    /// </summary>
    [XmlRoot("record")]
    public class FileCabinetRecordXmlModel
    {
        /// <summary>
        /// Gets or sets Id of record.
        /// </summary>
        /// <value>Id of record.</value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first and last names.
        /// </summary>
        /// <value></value>
        [XmlElement("name")]
        public FileCabinetRecordXmlName Name { get; set; }

        /// <summary>
        /// Gets or sets persong date of birth.
        /// </summary>
        /// <value>Person date of birth.</value>
        [XmlElement("DateOfBirth")]
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets sex of a person.
        /// </summary>
        /// <value>Sex of a person.</value>
        [XmlElement("Sex")]
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets person weight.
        /// </summary>
        /// <value>Person weight.</value>
        [XmlElement("Weight")]
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets person height.
        /// </summary>
        /// <value>Person height.</value>
        [XmlElement("Height")]
        public short Height { get; set; }
    }
}

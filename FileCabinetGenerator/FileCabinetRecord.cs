using System;
using System.Globalization;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    [Serializable]
    /// <summary>
    /// Class contains record fields.
    /// </summary>
    public class FileCabinetRecord
    {
        [XmlAttribute("id")]
        /// <summary>
        /// Gets or sets Id of record.
        /// </summary>
        /// <value>Id of record.</value>
        public int Id { get; set; }

        [XmlElement("name")]
        public Name Name = new Name();

        [XmlIgnore]
        /// <summary>
        /// Gets or sets persong date of birth.
        /// </summary>
        /// <value>Person date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        [XmlElement("DateOfBirth")]
        public string SomeDateString
        {
            get { return this.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); }
            set { this.DateOfBirth = DateTime.Parse(value, CultureInfo.InvariantCulture); }
        }

        [XmlElement("Sex")]
        public string SexString 
        {
            get
            {
                return Sex.ToString();
            }

            set
            {
                Sex = value[0];
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets or sets sex of a person.
        /// </summary>
        /// <value>Sex of a person.</value>
        public char Sex { get; set; }

        [XmlElement("Weight")]
        /// <summary>
        /// Gets or sets person weight.
        /// </summary>
        /// <value>Person weight.</value>
        public decimal Weight { get; set; }

        [XmlElement("Height")]
        /// <summary>
        /// Gets or sets person height.
        /// </summary>
        /// <value>Person height.</value>
        public short Height { get; set; }

    }

    public class Name
    {
        [XmlAttribute("first")]
        /// <summary>
        /// Gets or sets person first name.
        /// </summary>
        /// <value>Person first name.</value>
        public string FirstName { get; set; }

        [XmlAttribute("last")]
        /// <summary>
        /// Gets or sets preson last name.
        /// </summary>
        /// <value>Person last name.</value>
        public string LastName { get; set; }
    }
}

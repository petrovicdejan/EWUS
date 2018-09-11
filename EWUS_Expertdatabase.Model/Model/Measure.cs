using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EWUS_Expertdatabase.Model
{
    public class Measure : CoreObject
    {
        public Measure()
        {
            this.MeasureLinks = new Collection<MeasureLink>();
            this.MeasurePictures = new Collection<MeasurePicture>();
            this.OperationType = new Classification();
            this.DocumentItems = new Collection<DocumentItem>();
        }

        /// <summary>
        ///     Get or set serial number of the measure
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        ///     Get or set operation type of the measure
        /// </summary>
        public Classification OperationType { get; set; }

        public int? OperationTypeId { get; set; }

        /// <summary>
        ///     Get or set savings of the measure
        /// </summary>
        public string Saving { get; set; }

        /// <summary>
        ///     Get or set investment cost of the measure
        /// </summary>
        public int InvestmentCost { get; set; }

        /// <summary>
        ///     Get or set collection of measure's pictures
        /// </summary>
        public Collection<MeasurePicture> MeasurePictures { get; set; }

        /// <summary>
        ///     Get or set collection of measure's links
        /// </summary>
        public Collection<MeasureLink> MeasureLinks { get; set; }

        /// <summary>
        ///     Get or set document
        /// </summary>
        public Collection<DocumentItem> DocumentItems { get; set; }
    }
    
    public class MeasurePicture : CoreObject
    {
        public MeasurePicture()
        {
        }

        /// <summary>
        ///     Get or set file name of measure's pictures
        /// </summary>
        public string Filename { get; set; }

        [IgnoreDataMember]
        [XmlIgnore]
        protected Measure Measure { get; set; }
        
        public long MeasureId { get; set; }
    }
    
    public class MeasureLink : CoreObject
    {
        public MeasureLink()
        {
        }

        /// <summary>
        ///     Get or set link of measure's links
        /// </summary>
        public string Link { get; set; }

        [IgnoreDataMember]
        [XmlIgnore]
        protected Measure Measure { get; set; }
        
        public long MeasureId { get; set; }
    }
}

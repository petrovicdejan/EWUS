using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Common
{
    [ComplexType]
    [DataContract]
    public class Attachment 
    {
        [DataMember]
        public byte[] BinaryValue { get; set; }

        [DataMember]
        public string DocumentName { get; set; }

        [DataMember]
        public string DocumentMimeType { get; set; }

        [DataMember]
        public string DocumentSize { get; set; }

        [DataMember]
        public Reference DocumentBehaviorSpecification { get; set; }

        [DataMember]
        public string ObjectId { get; set; }

        [DataMember]
        public bool IsNew { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}

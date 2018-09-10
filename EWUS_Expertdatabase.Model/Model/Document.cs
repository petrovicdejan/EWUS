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
    public class DocumentInstance : CoreObject
    {
        public DocumentInstance()
        {
            this.DocumentItems = new Collection<DocumentItem>();
        }
        
        public Collection<DocumentItem> DocumentItems { get; set; }
        
        //public Reference RefersTo { get; set; }
    }
    
    public class DocumentItem : CoreObject
    {
        public DocumentItem()
        {

        }
        
        //[IgnoreDataMember]
        //[XmlIgnore]
        //protected DocumentInstance DocumentInstance { get; set; }
        
        //public long DocumentInstanceId { get; set; }
        
        public byte[] BinaryValue { get; set; }
        
        public string DocumentName { get; set; }
               
        public string DocumentMimeType { get; set; }
        
        public string DocumentSize { get; set; }
        
        public string ObjectId { get; set; }
    }
}

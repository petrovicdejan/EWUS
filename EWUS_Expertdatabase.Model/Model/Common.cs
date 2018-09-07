using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Model
{
    [ComplexType]
    [Serializable]
    public class Reference 
    {
        public Reference(string typeName)
        {
            this.TypeName = typeName;
        }

        public Reference(long id, string typeName)
        {
            this.Id = id;
            this.TypeName = typeName;
        }
        
        private string _typeName = string.Empty;

        public long Id { get; set; }

        public string TypeName
        {
            get
            {
                return _typeName;
            }

            set
            {
                _typeName = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Model
{
    public interface IReference
    {
        object Value { get; set; }

        string SerializedValue { get; set; }

        string Name { get; set; }

        string Number { get; set; }

        int Version { get; set; }

        string TypeName { get; set; }
        //object Resolve();

        bool HasValue();

        long GetId();
    }

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

        public Reference()
            : this("NullReference")
        {

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

        /// <summary>
        ///     Implicit conversion from refrable to reference
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static implicit operator Reference(long id)
        {
            Reference r = new Reference();

            r.Id = id;

            return r;
        }

        /// <summary>
        ///     Implicit conversion from refrable to reference
        /// </summary>
        /// <param name="value">Referable object</param>
        /// <returns>Create new reference to referable object with value</returns>
        public static implicit operator Reference(CoreObject value)
        {
            Reference r = new Reference();

            r.Id = value.Id;
            
            return r;
        }
    }
}

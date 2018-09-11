using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Model
{
    public abstract class CoreObject 
    {
        /// <summary>
        ///     Empty constructor required for serialization
        /// </summary>
        public CoreObject()
        {

        }
                
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        ///     Gets or sets the string that represents the unique name of the object.
        /// </summary>
        [Column(Order = 2)]
        [MaxLength(255)]
        public string Name { get; set; }
        
        public string Description { get; set; }

        private Guid _Guid = Guid.Empty;
        /// <summary>
        ///    Get the persistent object identifier if exist 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid
        {
            get { return _Guid; }
            set
            {
                if (IsTransient())
                    _Guid = value;
            }
        }

        /// <summary>
        ///     Check if this entity is transient, ie, without identity at this moment
        /// </summary>
        /// <returns>True if entity is transient, else false</returns>
        public bool IsTransient()
        {
            return this.Guid == Guid.Empty;
        }
    }
}

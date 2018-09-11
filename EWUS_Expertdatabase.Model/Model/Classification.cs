using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Model
{
    public class Classification : CoreObject
    {
        public Classification()
        {

        }

        /// <summary>
        ///     Get or set classification type
        /// </summary>
        [MaxLength(255)]
        public string ClassificationType { get; set; }

        /// <summary>
        ///     Get or set classification value
        /// </summary>
        public string Value { get; set; }
    }
}

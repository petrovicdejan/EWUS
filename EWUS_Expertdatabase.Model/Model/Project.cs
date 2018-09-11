using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Model
{
    public class Project : CoreObject
    {
        public Project()
        {
            this.Region = new Classification();
            this.Property = new Classification();
            this.Customer = new Customer();
        }

        /// <summary>
        ///     Gets or set the performance of equipment in a project
        /// </summary>
        public int PerformanceOfEquipment { get; set; }

        /// <summary>
        ///     Gets or set the name about serviced object in a project
        /// </summary>
        [MaxLength(255)]
        public string ServicedObject { get; set; }

        /// <summary>
        ///     Gets or set the value of total investment of the project
        /// </summary>
        public int InvestmentTotal { get; set; }

        /// <summary>
        ///     Gets or set the value of total saving of the project
        /// </summary>
        public int SavingTotal { get; set; }

        /// <summary>
        ///     Gets or set the remark in a project
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     Gets or set the changes of usage in a project
        /// </summary>
        public string ChangesOfUsage { get; set; }

        /// <summary>
        ///     Gets or set the location of the project
        /// </summary>
        [MaxLength(255)]
        public string Location { get; set; }

        /// <summary>
        ///     Gets or set the ZipCode of the project
        /// </summary>
        //[MaxLength(10)]
        public string ZipCode { get; set; }

        /// <summary>
        ///     Gets or set the City of the project
        /// </summary>
        [MaxLength(255)]
        public string City { get; set; }

        /// <summary>
        ///     Gets or set the Telephone of the responsible person in project
        /// </summary>
        //[MaxLength(50)]
        public string Telephone { get; set; }

        /// <summary>
        ///     Gets or set the contact person of the project
        /// </summary>
        //[MaxLength(50)]
        public string ContactPerson { get; set; }

        /// <summary>
        ///     Gets or set the reference to classification Region
        /// </summary>
        public Classification Region { get; set; }

        public int? RegionId { get; set; }

        /// <summary>
        ///     Gets or set the reference to classification Property
        /// </summary>
        public Classification Property { get; set; }

        public int? PropertyId { get; set; }

        /// <summary>
        ///     Gets or set the reference to PropertyNumber
        /// </summary>
        //[MaxLength(10)]
        public string PropertyNumber { get; set; }

        /// <summary>
        ///     Gets or set the reference to Customer object
        /// </summary>
        public Customer Customer { get; set; }

        public int? CustomerId { get; set; }
    }
}

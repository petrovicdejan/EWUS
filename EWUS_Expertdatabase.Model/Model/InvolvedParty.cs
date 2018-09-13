using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EWUS_Expertdatabase.Model
{
    public abstract class InvolvedParty : CoreObject
    {
        public InvolvedParty()
        {
            this.RootType = this.GetType().Name;
            this.DocumentItems = new Collection<DocumentItem>();
        }

        /// <summary>
        ///     Get or set RootType of the InvolvedParty
        /// </summary>
        public string RootType { get; set; }

        /// <summary>
        ///     Get or set Email address of the InvolvedParty
        /// </summary>
        //[MaxLength(50)]
        public string Email { get; set; }

        /// <summary>
        ///     Get or set Logo of the InvolvedParty
        /// </summary>
        [MaxLength(255)]
        public string Logo { get; set; }

        /// <summary>
        ///     Get or set document
        /// </summary>
        public Collection<DocumentItem> DocumentItems { get; set; }

    }
    
    public class Customer : InvolvedParty
    {
        public Customer()
        {
        }

    }
        
    public class MaintenanceCompany : InvolvedParty
    {
        public MaintenanceCompany()
        {
        }
    }
    
    public class Individual : InvolvedParty
    {
        public Individual()
        {
        }
    }
}

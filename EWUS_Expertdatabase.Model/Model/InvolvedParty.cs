using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Model
{
    public abstract class InvolvedParty : CoreObject
    {
        public InvolvedParty()
        {
            this.RootType = this.GetType().Name;
        }

        /// <summary>
        ///     Get or set RootType of the InvolvedParty
        /// </summary>
        public string RootType { get; set; }

        /// <summary>
        ///     Get or set Email address of the InvolvedParty
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Get or set Logo of the InvolvedParty
        /// </summary>
        public string Logo { get; set; }

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

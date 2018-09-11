using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWUS_Expertdatabase.Web.Client
{
    public class ProjectViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }

        public Classification Property { get; set; }

        public int? PropertyId { get; set; }

        public Customer Customer { get; set; }

        public int? CustomerId { get; set; }

        public Classification Region { get; set; }

        public int? RegionId { get; set; }

        public string Location { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public int InvestmentTotal { get; set; }

        public int SavingTotal { get; set; }

        public string ContactPerson { get; set; }

        public string ChangesOfUsage { get; set; }

        public string Remark { get; set; }

        public string ServicedObject { get; set; }

        public int PerformanceOfEquipment { get; set; }

        public string PropertyNumber { get; set; }

    }
}
using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Business
{
    public class ProjectMeasurePoco
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int PerformanseSheetNumber { get; set; }

        public string MeasureName { get; set; }

        //public string PerformanseSheetStatus { get; set; }

        //public string MaintenanceCompany { get; set; }

        public Classification PerformanseSheetStatus { get; set; }

        public int? PerformanseSheetStatusId { get; set; }
        
        public MaintenanceCompany MaintenanceCompany { get; set; }

        public int? MaintenanceCompanyId { get; set; }

        public string OperationType { get; set; }

        public int InvestmentCost { get; set; }

        public long ProjectId { get; set; }

        public long MeasureId { get; set; }

        public DateTime ModificationDate { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public DateTime SubmittedOnDate { get; set; }

        public string SubmittedBy { get; set; }

        public bool Release { get; set; }

        public string Remark { get; set; }

        public Collection<DocumentItem> DocumentItems { get; set; }
    }

    public class MeasurePoco
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Model
{
    public class ProjectMeasure : CoreObject
    {
        public ProjectMeasure()
        {
            //this.PerformanseSheetStatus = new Classification();
            this.DocumentItems = new Collection<DocumentItem>();
        }

        public Project Project { get; set; }

        public Measure Measure { get; set; }
        
        public long ProjectId { get; set; }
        
        public long MeasureId { get; set; }

        public MaintenanceCompany MaintenanceCompany { get; set; }

        public int? MaintenanceCompanyId { get; set; }

        public Classification PerformanseSheetStatus { get; set; }

        public int? PerformanseSheetStatusId { get; set; }

        public int PerformanseSheetNumber { get; set; }

        public DateTime? ModificationDate { get; set; }

        public string Specification { get; set; }

        public int InvestmenCost { get; set; }

        public DateTime? SubmittedOnDate { get; set; }

        public string SubmittedBy { get; set; }

        public bool Release { get; set; }

        public string Remark { get; set; }

        public Collection<DocumentItem> DocumentItems { get; set; }
    }

    public class ProjectMeasurePerformance : CoreObject
    {
        public ProjectMeasurePerformance()
        {
            this.ProjectMeasure = new ProjectMeasure();
        }
        
        public ProjectMeasure ProjectMeasure { get; set; }

        public int ProjectMeasureId { get; set; }

        public DocumentItem DocumentItem { get; set; }

        public int? DocumentItemId { get; set; }
    }
}

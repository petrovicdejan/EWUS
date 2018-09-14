using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Model
{
    public class ProjectMeasure
    {
        public ProjectMeasure()
        {
            this.PerformanseSheetStatus = new Classification();
            this.ProjectMeasurePerformance = new Collection<Model.ProjectMeasurePerformance>();
        }

        public virtual Project Project { get; set; }

        public virtual Measure Measure { get; set; }
        
        public int ProjectId { get; set; }
        
        public int MeasureId { get; set; }

        public MaintenanceCompany MaintenanceCompany { get; set; }

        public int? MaintenanceCompanyId { get; set; }

        public Classification PerformanseSheetStatus { get; set; }

        public int? PerformanseSheetStatusId { get; set; }

        public int PerformanseSheetNumber { get; set; }

        public DateTime ModificationDate { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public int InvestmenCost { get; set; }

        public DateTime SubmittedOnDate { get; set; }

        public string SubmittedBy { get; set; }

        public bool Release { get; set; }

        public string Remark { get; set; }

        public Collection<ProjectMeasurePerformance> ProjectMeasurePerformance { get; set; }
    }

    public class ProjectMeasurePerformance
    {
        public ProjectMeasurePerformance()
        {
            this.ProjectMeasure = new ProjectMeasure();
        }
        
        public ProjectMeasure ProjectMeasure { get; set; }

        public int ProjectMeasureId { get; set; }

        public int Position { get; set; }

        public bool Hide { get; set; }

        public DocumentItem DocumentItem { get; set; }

        public int? DocumentItemId { get; set; }
    }
}

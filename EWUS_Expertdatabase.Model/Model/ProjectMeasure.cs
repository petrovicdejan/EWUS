using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EWUS_Expertdatabase.Model
{
    public class ProjectMeasure : CoreObject
    {
        public ProjectMeasure()
        {
            this.ProjectMeasurePerformances = new Collection<ProjectMeasurePerformance>();
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

        public Collection<ProjectMeasurePerformance> ProjectMeasurePerformances { get; set; }
    }

    public class ProjectMeasurePerformance : CoreObject
    {
        public ProjectMeasurePerformance()
        {
         // this.ProjectMeasure = new ProjectMeasure();
        }

        //[IgnoreDataMember]
        //[XmlIgnore]
        //protected ProjectMeasure ProjectMeasure { get; set; }

        //public int ProjectMeasureId { get; set; }

        public DocumentItem DocumentItem { get; set; }

        public int? DocumentItemId { get; set; }

        public int Position { get; set; }

        public bool Hide { get; set; }
    }
}

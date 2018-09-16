using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWUS_Expertdatabase.Web.Client
{
    public class ProjectMeasureViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int PerformanseSheetNumber { get; set; }

        public string MeasureName { get; set; }

        public string PerformanseSheetStatus { get; set; }

        public string MaintenanceCompany { get; set; }

        public string OperationType { get; set; }

        public int InvestmentCost { get; set; }

        public long ProjectId { get; set; }

        public long MeasureId { get; set; }

    }
}
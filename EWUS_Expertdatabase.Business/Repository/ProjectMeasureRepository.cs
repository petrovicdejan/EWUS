using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EWUS_Expertdatabase.Business
{
    public class ProjectMeasureRepository
    {
        public IEnumerable GetAllProjectMeasures()
        {
            //using (var context = new EWUSDbContext())
            //{
            //    var projectMeasures = from pm in context.ProjectMeasures
            //                          join m in context.Measures on pm.MeasureId equals m.Id
            //                          join p in context.Projects on pm.ProjectId equals p.Id
            //                          join mc in context.MaintenanceCompanies on
            //                                new { f1 = pm.MaintenanceCompanyId }
            //                                equals
            //                                new { f1 = (int?)mc.Id } into cp
            //                          from q1 in cp.DefaultIfEmpty()
            //                          join ot in context.Classifications.Where(x => x.ClassificationType == "Massnahmenart") on
            //                                new { f1 = m.OperationTypeId }
            //                                equals
            //                                new { f1 = (int?)ot.Id } into ap
            //                          from q2 in ap.DefaultIfEmpty()
            //                          join ps in context.Classifications.Where(x => x.ClassificationType == "Leistungsblattstatus") on
            //                                new { f1 = pm.PerformanseSheetStatusId }
            //                                equals
            //                                new { f1 = (int?)ps.Id } into rp
            //                          from q3 in rp.DefaultIfEmpty()
            //                          orderby pm.PerformanseSheetNumber
            //                          select new
            //                          {
            //                              ProjectMeasure = pm,
            //                              Project = p,
            //                              Measure = m,
            //                              MaintenanceCompany = q1,
            //                              OperationType = q2,
            //                              PerformanceStatus = q3
            //                          };

            //    IEnumerable result = projectMeasures.ToList();
            //    if (result != null)
            //    {
            //        return result;
            //    }

            //}
            return null;
        }
    }
}

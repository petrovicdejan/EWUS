using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System;
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
            using (var context = new EWUSDbContext())
            {
                var projectMeasures = from pm in context.ProjectMeasures
                                      join m in context.Measures on pm.MeasureId equals m.Id
                                      join p in context.Projects on pm.ProjectId equals p.Id
                                      join mc in context.MaintenanceCompanies on
                                            new { f1 = pm.MaintenanceCompanyId }
                                            equals
                                            new { f1 = (int?)mc.Id } into cp
                                      from q1 in cp.DefaultIfEmpty()
                                      join ot in context.Classifications.Where(x => x.ClassificationType == "Massnahmenart") on
                                            new { f1 = m.OperationTypeId }
                                            equals
                                            new { f1 = (int?)ot.Id } into ap
                                      from q2 in ap.DefaultIfEmpty()
                                      join ps in context.Classifications.Where(x => x.ClassificationType == "Leistungsblattstatus") on
                                            new { f1 = pm.PerformanseSheetStatusId }
                                            equals
                                            new { f1 = (int?)ps.Id } into rp
                                      from q3 in rp.DefaultIfEmpty()
                                      orderby pm.PerformanseSheetNumber
                                      select new
                                      {
                                          ProjectMeasure = pm,
                                          Project = p,
                                          Measure = m,
                                          MaintenanceCompany = q1,
                                          OperationType = q2,
                                          PerformanceStatus = q3
                                      };

                IEnumerable result = projectMeasures.ToList();
                if (result != null)
                {
                    return result;
                }

            }
            return null;
        }

        public List<ProjectMeasurePoco> GetAllProjectMeasuresForProject(long projectId)
        {
            using (var context = new EWUSDbContext())
            {
                var projectMeasures = from pm in context.ProjectMeasures.Where(pm => pm.ProjectId == projectId)
                                      join m in context.Measures on pm.MeasureId equals m.Id
                                      join p in context.Projects on pm.ProjectId equals p.Id
                                      join mc in context.MaintenanceCompanies on
                                            new { f1 = pm.MaintenanceCompanyId }
                                            equals
                                            new { f1 = (int?)mc.Id } into cp
                                      from q1 in cp.DefaultIfEmpty()
                                      join ot in context.Classifications.Where(x => x.ClassificationType == "Massnahmenart") on
                                            new { f1 = m.OperationTypeId }
                                            equals
                                            new { f1 = (int?)ot.Id } into ap
                                      from q2 in ap.DefaultIfEmpty()
                                      join ps in context.Classifications.Where(x => x.ClassificationType == "Leistungsblattstatus") on
                                            new { f1 = pm.PerformanseSheetStatusId }
                                            equals
                                            new { f1 = (int?)ps.Id } into rp
                                      from q3 in rp.DefaultIfEmpty()
                                      orderby pm.PerformanseSheetNumber
                                      select new ProjectMeasurePoco
                                      {
                                          Id = pm.Id,
                                          Name = pm.Name,
                                          PerformanseSheetNumber = pm.PerformanseSheetNumber,
                                          MeasureName = m.Name,
                                          PerformanseSheetStatus = q3.Value,
                                          MaintenanceCompany = q1.Name,
                                          OperationType = q2.Value,
                                          InvestmentCost = pm.InvestmenCost
                                      };
                                      

                List<ProjectMeasurePoco> result = projectMeasures.ToList();
                if (result != null)
                {
                    return result;
                }

            }
            return null;
        }

        public Result AddMeasureToProject(ProjectMeasurePoco editProjectMeasurePoco)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            using (var ctx = new EWUSDbContext())
            {
                ProjectMeasure projectMeasure = new ProjectMeasure();

                projectMeasure.ProjectId = editProjectMeasurePoco.ProjectId;
                projectMeasure.MeasureId = editProjectMeasurePoco.MeasureId;
                projectMeasure.ModificationDate = DateTime.Now.ToLocalTime();
                projectMeasure.Release = true;

                int maxPerformanseSheetNumber = 0;
                if (ctx.ProjectMeasures.Count() > 0)
                {
                    maxPerformanseSheetNumber = ctx.ProjectMeasures.Max(x => x.PerformanseSheetNumber);
                }

                projectMeasure.PerformanseSheetNumber = ++maxPerformanseSheetNumber;

                ctx.ProjectMeasures.Add(projectMeasure);

                ctx.SaveChanges();

                output = Result.ToResult<ProjectMeasure>(ResultStatus.OK, typeof(ProjectMeasure));
                output.Value = projectMeasure;
            }

            return output;
        }

        public Result DeleteProjectMeasureById(long Id)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            try
            {
                using (var ctx = new EWUSDbContext())
                {
                    ProjectMeasure projectMeasure = ctx.ProjectMeasures.Where(x => x.Id == Id)
                                            .FirstOrDefault();
                    
                    try
                    {
                        ctx.ProjectMeasures.Remove(projectMeasure);

                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146233087)
                        {
                            output.ExceptionMessage = Constants.ErrorMessageReferentialIntegrity;
                            output.Status = ResultStatus.Forbidden;
                        }
                        else
                        {
                            output.ExceptionMessage = "Exception could not be performed !!!";
                            output.Status = ResultStatus.InternalServerError;
                        }
                    }
                }
                output.Status = ResultStatus.OK;
            }
            catch (Exception e)
            {
                output.Status = ResultStatus.InternalServerError;
            }

            return output;
        }
    }
}

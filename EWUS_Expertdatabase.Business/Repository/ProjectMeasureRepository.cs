﻿using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;

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
                                          SavingPercent = pm.SavingPercent,
                                          PerformanseSheetStatus = q3,
                                          MaintenanceCompany = q1,
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
                Project project = ctx.Projects.Where(x => x.Id == projectMeasure.ProjectId).FirstOrDefault();
                projectMeasure.Project = project;
                projectMeasure.MeasureId = editProjectMeasurePoco.MeasureId;
                Measure measure = ctx.Measures.Where(x => x.Id == projectMeasure.MeasureId).FirstOrDefault();
                projectMeasure.Measure = measure;
                projectMeasure.Release = false;

                int maxPerformanseSheetNumber = 0;
                if (ctx.ProjectMeasures.Count() > 0)
                {
                    maxPerformanseSheetNumber = ctx.ProjectMeasures.Max(x => x.PerformanseSheetNumber);
                }

                projectMeasure.PerformanseSheetNumber = ++maxPerformanseSheetNumber;
                                
                projectMeasure.Description = measure != null ? measure.Description : string.Empty;
                projectMeasure.SavingPercent = measure != null ? measure.SavingPercent : 0;

                ctx.ProjectMeasures.Add(projectMeasure);

                ctx.SaveChanges();

                output = Result.ToResult<ProjectMeasure>(ResultStatus.OK, typeof(ProjectMeasure));
                output.Value = new ProjectMeasure() { Id = projectMeasure.Id, Name = projectMeasure.Name }; ;
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
                    ProjectMeasure projectMeasure = ctx.ProjectMeasures.Where(x => x.Id == Id).Include(x => x.ProjectMeasurePerformances)
                                            .FirstOrDefault();

                    try
                    {
                        ctx.ProjectMeasures.Remove(projectMeasure);

                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146233087 || ex.HResult == -2146233079)
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

        public ProjectMeasurePoco GetProjectMeasureById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                var projectMeasure = from pm in context.ProjectMeasures.Where(pm => pm.Id == Id)
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
                                     select new ProjectMeasurePoco
                                     {
                                         Id = pm.Id,
                                         Name = p.Name,
                                         PerformanseSheetNumber = pm.PerformanseSheetNumber,
                                         PropertyNumber = p.PropertyNumber,
                                         PropertyId = p.PropertyId,
                                         Property = p.Property,
                                         MeasureName = m.Name,
                                         PerformanseSheetStatus = q3,
                                         MaintenanceCompany = q1,
                                         OperationType = q2.Value,
                                         InvestmentCost = pm.InvestmenCost,
                                         ModificationDate = pm.ModificationDate ?? pm.ModificationDate.Value,
                                         Description = pm.Description,
                                         MeasureDescription = m.Description,
                                         Specification = pm.Specification,
                                         SubmittedOnDate = pm.SubmittedOnDate ?? pm.SubmittedOnDate.Value,
                                         SubmittedBy = pm.SubmittedBy,
                                         Release = pm.Release,
                                         Remark = pm.Remark,
                                         ProjectMeasurePerformances = pm.ProjectMeasurePerformances,
                                         ProjectId = p.Id,
                                         MeasureId = m.Id,
                                         Location = p.Location,
                                         ZipCode = p.ZipCode,
                                         City = p.City,
                                         SavingPercent = pm.SavingPercent
                                     };

                ProjectMeasurePoco result = projectMeasure.FirstOrDefault();
                IEnumerable<ProjectMeasurePerformance> projectMeasurePerformances = context.ProjectMeasures.Where(pm => pm.Id == Id).SelectMany(x => x.ProjectMeasurePerformances)
                                                                                            .Include(x => x.DocumentItem).OrderBy(x => x.Position).ToList();

                Collection<ProjectMeasurePerformance> pmp = new ObservableCollection<ProjectMeasurePerformance>(projectMeasurePerformances.ToList().Distinct());

                if (projectMeasurePerformances != null)
                {
                    result.ProjectMeasurePerformances = new Collection<ProjectMeasurePerformance>();
                    result.ProjectMeasurePerformances = pmp;
                }

                if (result != null)
                {
                    return result;
                }
                return null;
            }
        }

        public Result SaveProjectMeasure(ProjectMeasurePoco editProjectMeasure)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            using (var ctx = new EWUSDbContext())
            {
                ProjectMeasure projectMeasure = ctx.ProjectMeasures.Where(x => x.Id == editProjectMeasure.Id)
                                                    .Include(x => x.ProjectMeasurePerformances).FirstOrDefault();

                if (projectMeasure != null)
                {
                    Collection<ProjectMeasurePerformance> projectMeasurePerformances = new Collection<ProjectMeasurePerformance>();
                    if (editProjectMeasure.ProjectMeasurePerformances != null && editProjectMeasure.ProjectMeasurePerformances.Count() > 0)
                    {
                        foreach (var edi in editProjectMeasure.ProjectMeasurePerformances)
                        {
                            var pmp = ctx.ProjectMeasurePerformances.Where(x => x.Id == edi.Id).FirstOrDefault();
                            if (pmp == null)
                            {
                                ctx.ProjectMeasurePerformances.Add(edi);
                                projectMeasurePerformances.Add(edi);
                            }
                            else
                            {
                                pmp.Hide = edi.Hide;
                                pmp.PageBreak = edi.PageBreak;
                                pmp.Description = edi.Description;
                                pmp.Position = edi.Position;
                                if (edi.DocumentItem != null)
                                {
                                    var docItem = ctx.DocumentItems.Where(x => x.Id == edi.DocumentItem.Id).FirstOrDefault();
                                    if (docItem != null)
                                    {
                                        pmp.DocumentItem = docItem;
                                    }else
                                    {
                                        pmp.DocumentItem = edi.DocumentItem;
                                    }
                                }
                                projectMeasurePerformances.Add(pmp);
                            }
                        }
                    }

                    var forDelete = from opmp in projectMeasure.ProjectMeasurePerformances
                                    join epmp in editProjectMeasure.ProjectMeasurePerformances on
                                       new { f1 = opmp.Id }
                                       equals
                                       new { f1 = epmp.Id } into cp
                                    from q1 in cp.DefaultIfEmpty()
                                    where q1 == null
                                    select new 
                                    {
                                        Id = opmp.Id
                                    };

                    List<long> idForDelete = forDelete.Select(x => x.Id).ToList();
                    
                    if (idForDelete.Count() > 0)
                    {
                        List<ProjectMeasurePerformance> collPmp = ctx.ProjectMeasurePerformances.Where(x => idForDelete.Contains(x.Id)).ToList();
                        ctx.ProjectMeasurePerformances.RemoveRange(collPmp);
                    }

                    projectMeasure.ProjectMeasurePerformances = projectMeasurePerformances;
                    projectMeasure.InvestmenCost = editProjectMeasure.InvestmentCost;
                    projectMeasure.MaintenanceCompanyId = editProjectMeasure.MaintenanceCompanyId;
                    projectMeasure.ModificationDate = editProjectMeasure.ModificationDate;
                    projectMeasure.Release = editProjectMeasure.Release;
                    projectMeasure.PerformanseSheetNumber = editProjectMeasure.PerformanseSheetNumber;
                    projectMeasure.PerformanseSheetStatusId = editProjectMeasure.PerformanseSheetStatusId;
                    projectMeasure.Remark = editProjectMeasure.Remark;
                    projectMeasure.Description = editProjectMeasure.Description;
                    projectMeasure.Specification = editProjectMeasure.Specification;
                    projectMeasure.SubmittedBy = editProjectMeasure.SubmittedBy;
                    projectMeasure.SubmittedOnDate = editProjectMeasure.SubmittedOnDate;
                    projectMeasure.SavingPercent = editProjectMeasure.SavingPercent;
                }
                else
                {
                    output.Status = ResultStatus.BadRequest;
                }

                ctx.SaveChanges();


                if (!string.IsNullOrEmpty(projectMeasure.Guid.ToString()) && projectMeasure.ProjectMeasurePerformances != null
                    && projectMeasure.ProjectMeasurePerformances.Count > 0)
                {
                    Task.Factory.StartNew(() =>
                    {
                        Collection<DocumentItem> cdi = new Collection<DocumentItem>();
                        foreach (var pmp in projectMeasure.ProjectMeasurePerformances)
                        {
                            if (pmp.DocumentItem != null && !string.IsNullOrEmpty(pmp.DocumentItem.ObjectId))
                            {
                                cdi.Add(pmp.DocumentItem);
                            }
                        }
                        SaveFile.SaveFileInFolder(projectMeasure.Guid.ToString(), typeof(ProjectMeasure).Name, cdi);
                    });
                }

                output = Result.ToResult<ProjectMeasure>(ResultStatus.OK, typeof(ProjectMeasure));
                output.Value = new ProjectMeasure() { Id = projectMeasure.Id, Name = projectMeasure.Name };
            }

            return output;
        }

        public Result DeleteProjectMeasurePerformanceById(long Id)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            try
            {
                using (var ctx = new EWUSDbContext())
                {
                    ProjectMeasurePerformance projectMeasurePerformance = ctx.ProjectMeasurePerformances.Where(x => x.Id == Id)
                                            .FirstOrDefault();

                    try
                    {
                        ctx.ProjectMeasurePerformances.Remove(projectMeasurePerformance);

                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146233087 || ex.HResult == -2146233079)
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

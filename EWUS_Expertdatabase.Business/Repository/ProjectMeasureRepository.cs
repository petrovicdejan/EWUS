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
                projectMeasure.MeasureId = editProjectMeasurePoco.MeasureId;
                //projectMeasure.ModificationDate = DateTime.Now.ToLocalTime();
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
                                      join ip in context.InvolvedPartys on
                                            new { f1 = p.CustomerId }
                                            equals
                                            new { f1 = (int?)ip.Id } into qp
                                      from q4 in qp.DefaultIfEmpty()
                                      select new ProjectMeasurePoco
                                      {
                                          Id = pm.Id,
                                          Name = p.Name,
                                          PerformanseSheetNumber = pm.PerformanseSheetNumber,
                                          MeasureName = m.Name,
                                          PerformanseSheetStatus = q3,
                                          MaintenanceCompany = q1,
                                          OperationType = q2.Value,
                                          InvestmentCost = pm.InvestmenCost,
                                          ModificationDate = pm.ModificationDate ?? pm.ModificationDate.Value,
                                          Description = pm.Description,
                                          Specification = pm.Specification,
                                          SubmittedOnDate = pm.SubmittedOnDate ?? pm.SubmittedOnDate.Value,
                                          SubmittedBy = pm.SubmittedBy,
                                          Release = pm.Release,
                                          Remark = pm.Remark,
                                          DocumentItems = pm.DocumentItems,
                                          ProjectId = p.Id,
                                          MeasureId = m.Id,
                                          Location = p.Location,
                                          ZipCode = p.ZipCode,
                                          City = p.City,
                                          Logo = q4.Logo
                                      };

                ProjectMeasurePoco result = projectMeasure.FirstOrDefault();
                IEnumerable<DocumentItem> documentItems = context.ProjectMeasures.Where(pm => pm.Id == Id).SelectMany(x => x.DocumentItems).OrderBy(x => x.Position).ToList();

                Collection<DocumentItem> cdi = new ObservableCollection<DocumentItem>(documentItems.ToList().Distinct());

                if (documentItems != null)
                {
                    result.DocumentItems = new Collection<DocumentItem>();
                    result.DocumentItems = cdi;
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
                                                    .Include(x => x.DocumentItems).FirstOrDefault();

                if (projectMeasure != null)
                {
                    Collection<DocumentItem> documentItems = new Collection<DocumentItem>();
                    if (editProjectMeasure.DocumentItems != null && editProjectMeasure.DocumentItems.Count() > 0)
                    {
                        foreach (var edi in editProjectMeasure.DocumentItems)
                        {
                            var di = ctx.DocumentItems.Where(x => x.Id == edi.Id).FirstOrDefault();
                            if (di == null)
                            {
                                ctx.DocumentItems.Add(edi);
                                documentItems.Add(edi);
                            }
                            else
                                documentItems.Add(di);
                        }
                    }
                    //if (projectMeasure.DocumentItems != null)
                    //{
                    //    List<DocumentItem> documentItems = projectMeasure.DocumentItems.ToList();
                    //    ctx.DocumentItems.RemoveRange(documentItems);
                    //}

                    projectMeasure.DocumentItems = documentItems;
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
                }
                else
                {
                    output.Status = ResultStatus.BadRequest;
                }
                
                ctx.SaveChanges();


                if (!string.IsNullOrEmpty(projectMeasure.Guid.ToString()) && projectMeasure.DocumentItems != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        SaveFile.SaveFileInFolder(projectMeasure.Guid.ToString(), typeof(ProjectMeasure).Name, projectMeasure.DocumentItems);
                    });
                }

                output = Result.ToResult<ProjectMeasure>(ResultStatus.OK, typeof(ProjectMeasure));
                output.Value = new ProjectMeasure() { Id = projectMeasure.Id, Name = projectMeasure.Name };
            }

            return output;
        }
    }
}

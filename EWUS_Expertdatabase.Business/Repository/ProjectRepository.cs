using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EWUS_Expertdatabase.Business
{
    public class ProjectRepository
    {
        public List<Project> GetAllProjects()
        {
            using (var context = new EWUSDbContext())
            {
                List<Project> projects = new List<Project>();
                projects = context.Projects.AsNoTracking()
                    .Include(x => x.Region)
                    .Include(x => x.Property)
                    .Include(x => x.Customer)
                    .Include(x => x.DocumentItem)
                    .ToList();

                if (projects != null)
                {
                    return projects;
                }
                return null;
            }
        }


        public DocumentItem GetDocumentItemForProject(long projectId)
        {
            DocumentItem logoDoc = null;

            if (projectId == 0)
                return logoDoc;

            using (var context = new EWUSDbContext())
            {
                List<DocumentItem> documentItemsList = context.Projects.Where(i => i.Id == projectId).Include(x => x.Customer).Select(x=> x.Customer).SelectMany(x => x.DocumentItems).ToList();
                logoDoc = documentItemsList.FirstOrDefault();

                return logoDoc;
            }
        }

        public Project GetProjectById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                Project project = new Project();
                project = context.Projects.Where(x => x.Id == Id)
                    .Include(x => x.Region)
                    .Include(x => x.Property)
                    .Include(x => x.Customer)
                    .Include(x => x.DocumentItem)
                    .FirstOrDefault();

                if (project != null)
                {
                    return project;
                }
                return null;
            }
        }

        public Result SaveProject(Project editProject)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            using (var ctx = new EWUSDbContext())
            {
                Project project = null;
                DocumentItem documentItem = null;

                if (editProject.Id > 0)
                {
                    project = ctx.Projects.Where(x => x.Id == editProject.Id)
                          .Include(x => x.Region)
                          .Include(x => x.Property)
                          .Include(x => x.Customer)
                          .Include(x => x.DocumentItem)
                          .FirstOrDefault();
                }
                else
                {
                   project = new Project();
                }

                project.Name = editProject.Name;
                project.Telephone = editProject.Telephone;
                project.PropertyId = editProject.PropertyId;
                project.Property = ctx.Classifications.Where(x => x.Id == editProject.PropertyId).FirstOrDefault();
                project.CustomerId = editProject.CustomerId;
                project.Customer = ctx.Customers.Where(x => x.Id == editProject.CustomerId).FirstOrDefault();
                project.RegionId = editProject.RegionId;
                project.Region = ctx.Classifications.Where(x => x.Id == editProject.RegionId).FirstOrDefault();
                project.Location = editProject.Location;
                project.ZipCode = editProject.ZipCode;
                project.City = editProject.City;
                project.InvestmentTotal = editProject.InvestmentTotal;
                project.SavingTotal = editProject.SavingTotal;
                project.ContactPerson = editProject.ContactPerson;
                project.ChangesOfUsage = editProject.ChangesOfUsage;
                project.ServicedObject = editProject.ServicedObject;
                project.Remark = editProject.Remark;
                project.PropertyNumber = editProject.PropertyNumber;
                project.DocumentItemId = editProject.DocumentItemId;

                if (editProject.DocumentItemId != null && editProject.DocumentItemId != 0)
                    documentItem = ctx.DocumentItems.Where(x => x.Id == editProject.DocumentItemId).FirstOrDefault();
                else
                {
                    documentItem = editProject.DocumentItem;
                    ctx.DocumentItems.Add(documentItem);
                }
                project.DocumentItem = documentItem;

                if (project.Id == 0)
                    ctx.Projects.Add(project);

                ctx.SaveChanges();

                output = Result.ToResult<Project>(ResultStatus.OK, typeof(Project));
                output.Value = project;
            }

            return output;
        }

        public Result DeleteProjectById(long Id)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            try
            {
                using (var ctx = new EWUSDbContext())
                {
                    Project project = ctx.Projects.Where(x => x.Id == Id)
                                    .Include(x => x.Region)
                                      .Include(x => x.Property)
                                      .Include(x => x.Customer)
                                      .Include(x => x.DocumentItem)
                                      .Include(x => x.ProjectMeasures)
                                      .FirstOrDefault();

                    try
                    {
                        ctx.Projects.Remove(project);
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146233087 || ex.HResult == -2146233079)
                        {
                            output.ExceptionMessage = Constants.ErrorMessageReferentialIntegrity; ;
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
            catch
            {
                output.Status = ResultStatus.InternalServerError;
            }

            return output;
        }
        
    }
}

using AutoMapper;
using EWUS_Expertdatabase.Business;
using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EWUS_Expertdatabase.Web.Client
{
    [RoutePrefix("projektmitmassnahmen")]
    public class ProjectMeasureController : Controller
    {
        // GET: ProjectMeasure
        [Route("{projectId:long}")]
        public ActionResult ProjectMeasureIndex(long projectId)
        {
            ViewBag.Id = projectId;
            var projectRepo = new ProjectRepository();
            Project project = projectRepo.GetProjectById(projectId);
            ViewBag.ProjectName = project != null ? project.Name : string.Empty;
            return View();
        }
        
        public JsonResult GetAllProjectMeasures(long Id)
        {
            var projectMeasureRepo = new ProjectMeasureRepository();
            List<ProjectMeasurePoco> projectMeasures = projectMeasureRepo.GetAllProjectMeasuresForProject(Id);
            List<ProjectMeasureViewModel> lstProjectMeasureViewModel = new List<ProjectMeasureViewModel>();
            lstProjectMeasureViewModel = Mapper.Map<List<ProjectMeasurePoco>, List<ProjectMeasureViewModel>>(projectMeasures);

            return Json(lstProjectMeasureViewModel, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public ActionResult AddProjectToMeasure(ProjectMeasureViewModel model)
        {
            ProjectMeasurePoco projectMeasure = new ProjectMeasurePoco();

            projectMeasure = Mapper.Map<ProjectMeasureViewModel, ProjectMeasurePoco>(model);

            var projectMeasureRepo = new ProjectMeasureRepository();
            var item = projectMeasureRepo.AddMeasureToProject(projectMeasure);

            if (item.Success)
                return Json(item.Value);
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error has occured !!!");
        }

        [HttpPost]
        public ActionResult DeleteProjectMeasure(int Id)
        {
            if (Id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var projectMeasureRepo = new ProjectMeasureRepository();
            var item = projectMeasureRepo.DeleteProjectMeasureById(Id);

            if (item.Success)
                return Json(item);
            else
                return new HttpStatusCodeResult(Utils.ToHttpCode(item.Status), item.ExceptionMessage);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ProjectMeasureEdit(string key)
        {
            var projectMeasureRepo = new ProjectMeasureRepository();
            ProjectMeasurePoco projectMeasure = projectMeasureRepo.GetProjectMeasureById(key.ToLong(0)) as ProjectMeasurePoco;

            ViewBag.Input = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(projectMeasure)));

            ViewBag.Id = key.ToLong(0);
            ViewBag.Title = "Leistungsblatt (LB) ";
            ViewBag.TypeName = "ProjectMeasure";
            ViewBag.ProjectId = projectMeasure.ProjectId;
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsPopup = true;
                return PartialView("ProjectMeasure");
            }
            else
            {
                ViewBag.IsPopup = false;
                return View("ProjectMeasure");
            }
        }

        [HttpPost]
        public ActionResult SaveProjectMeasure(ProjectMeasureViewModel model)
        {
            ProjectMeasurePoco projectMeasurePoco = new ProjectMeasurePoco();

            projectMeasurePoco = Mapper.Map<ProjectMeasureViewModel, ProjectMeasurePoco>(model);

            var projectMeasureRepo = new ProjectMeasureRepository();
            var item = projectMeasureRepo.SaveProjectMeasure(projectMeasurePoco);

            if (item.Success)
            {
                return Json(item.Value);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error has occured !!!");
        }
    }
}
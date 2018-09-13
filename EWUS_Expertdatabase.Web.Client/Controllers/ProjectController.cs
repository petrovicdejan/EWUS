using AutoMapper;
using EWUS_Expertdatabase.Business;
using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EWUS_Expertdatabase.Web.Client.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        [Route("projektliste")]
        public ActionResult ProjectIndex()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Project()
        {
            var projectRepo = new ProjectRepository();
            ViewBag.Id = 0;
            ViewBag.Title = "Project";
            ViewBag.TypeName = "Project";
            

            if (Request.IsAjaxRequest())
            {
                ViewBag.IsPopup = true;
                return PartialView();
            }
            else
            {
                ViewBag.IsPopup = false;
                return View();
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ProjectEdit(string key)
        {
            var projectRepo = new ProjectRepository();
            Project measure = projectRepo.GetProjectById(key.ToLong(0)) as Project;

            ViewBag.Input = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(measure)));

            ViewBag.Id = key.ToLong(0);
            ViewBag.Title = "Projekt";
            ViewBag.TypeName = "Project";
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsPopup = true;
                return PartialView("Project");
            }
            else
            {
                ViewBag.IsPopup = false;
                return View("Project");
            }
        }

        // GET: Project
        public JsonResult GetProjects()
        {
            var projectRepo = new ProjectRepository();
            List<Project> projects = projectRepo.GetAllProjects();

            List<ProjectViewModel> lstProjectViewModel = new List<ProjectViewModel>();

            lstProjectViewModel = Mapper.Map<List<Project>, List<ProjectViewModel>>(projects);
            
            return Json(lstProjectViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProject(ProjectViewModel model)
        {
            Project project = new Project();
            
            project = Mapper.Map<ProjectViewModel, Project>(model);

            var projectRepo = new ProjectRepository();
            var item = projectRepo.SaveProject(project);

            if (item.Success)
                return Json(item.Value);
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error has occured !!!");
        }

        [HttpPost]
        public ActionResult DeleteProject(long Id)
        {
            if (Id == 0)            
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var projectRepo = new ProjectRepository();
            var item = projectRepo.DeleteProjectById(Id);

            if (item.Success)
                return Json(item);
            else
                return new HttpStatusCodeResult(Utils.ToHttpCode(item.Status), item.ExceptionMessage);
        }
    }
}
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EWUS_Expertdatabase.Web.Client
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapper.Mapper.Initialize(cfg =>
                                        {
                                            cfg.CreateMap<Measure, MeasureViewModel>().ReverseMap();
                                            cfg.CreateMap<Classification, ClassificationViewModel>().ReverseMap();
                                            cfg.CreateMap<Project, ProjectViewModel>().ReverseMap();
                                            cfg.CreateMap<MaintenanceCompany, MaintenanceCompanyViewModel>().ReverseMap();
                                            cfg.CreateMap<Customer, CustomerViewModel>().ReverseMap();
                                            cfg.CreateMap<Project, ProjectViewModel>().ReverseMap();
                                        });

            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<EWUSDbContext, EWUS_Expertdatabase.Data.Migrations.Configuration>());
        }

    }
}

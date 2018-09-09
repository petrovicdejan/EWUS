using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using EWUS_Expertdatabase.Model;
using EWUS_Expertdatabase.Data;

namespace EWUS_Expertdatabase.Business
{
    public class ProjectRepository
    {
        public List<Project> GetProjects()
        {
            using (var context = new EWUSDbContext())
            {
                List<Project> projects = new List<Project>();
                projects = context.Projects.AsNoTracking()
                    .Include(x => x.Region)
                    .ToList();

                if (projects != null)
                {
                    return projects;
                }
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using EWUS_Expertdatabase.Model;

namespace EWUS_Expertdatabase.Data
{
    public class ProjectRepository
    {
        public List<Project> GetProjects()
        {
            using (var context = new ApplicationDbContext())
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

using EWUS_Expertdatabase.Model;
using System.Collections.Generic;
using System.Linq;

namespace EWUS_Expertdatabase.Data
{
    public class ClassificationRepository
    {
        public List<Classification> GetClassifications()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Classification> classifications = new List<Classification>();
                classifications = context.Classifications.AsNoTracking()
                    .ToList();

                if (classifications != null)
                {
                    return classifications;
                }
                return null;
            }
        }

        public List<Classification> GetClassificationsByType(string type)
        {
            using (var context = new ApplicationDbContext())
            {
                List<Classification> classifications = new List<Classification>();
                classifications = context.Classifications.Where(x => x.ClassificationType == type)
                    .ToList();

                if (classifications != null)
                {
                    return classifications;
                }
                return null;
            }
        }
    }
}

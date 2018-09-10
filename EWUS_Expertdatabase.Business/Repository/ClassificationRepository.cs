using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EWUS_Expertdatabase.Business
{
    public class ClassificationRepository
    {
        public List<Classification> GetClassificationsByType(string type)
        {
            using (var context = new EWUSDbContext())
            {
                List<Classification> classifications = new List<Classification>();
                classifications = context.Classifications.Where(x => x.ClassificationType == type).AsNoTracking()
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

using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EWUS_Expertdatabase.Data
{
    public class MeasureRepository
    {
        public List<Measure> GetMeasures()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Measure> measures = new List<Measure>();
                measures = context.Measures.AsNoTracking()
                    .Include(x => x.MeasureLinks)
                    .Include(x => x.MeasurePictures)
                    .Include(x => x.OperationType)
                    .ToList();

                if (measures != null)
                {
                    return measures;
                }
                return null;
            }
        }

        public bool SaveMeasure(Item value)
        {
            //ovde cu dodati poziv funkcije koja puni objekat Measure u ovom slucaju iz value-a
            //delimicna kompleksnost su fajlovi koji treba da se upload-uju
            Measure measureEdit = null;

            if (measureEdit != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    Measure measure = null;
                    if (measureEdit.Id == 0)
                    {
                        measure = new Measure();
                    }
                    else
                    {
                        measure = context.Measures.Where(x => x.Id == measureEdit.Id).FirstOrDefault();
                    }

                    measure.Description = measureEdit.Description;
                    measure.InvestmentCost = measureEdit.InvestmentCost;
                    measure.MeasureLinks = measureEdit.MeasureLinks;
                    measure.MeasurePictures = measureEdit.MeasurePictures;
                    measure.Name = measureEdit.Name;
                    measure.OperationType= measureEdit.OperationType;
                    measure.Saving = measureEdit.Saving;
                    measure.SerialNumber = measureEdit.SerialNumber;

                    context.Measures.Add(measure);
                    context.SaveChanges();
                    return true;
                }
            }

            return false;
        }
    }
}

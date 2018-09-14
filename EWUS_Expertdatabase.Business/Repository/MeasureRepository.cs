using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EWUS_Expertdatabase.Business
{
    public class MeasureRepository 
    {
        public List<Measure> GetAllMeasures()
        {
            using (var context = new EWUSDbContext())
            {
                List<Measure> measures = new List<Measure>();
                measures = context.Measures.AsNoTracking()
                    .Include(x => x.MeasureLinks)
                    .Include(x => x.MeasurePictures)
                    .Include(x => x.OperationType)
                    .Include(x => x.DocumentItems)
                    .ToList();

                if (measures != null)
                {
                    return measures;
                }
                return null;
            }
        }

        public long GetMaxSerialNumber()
        {
            using (var context = new EWUSDbContext())
            {
                long maxSerialNumber = 0;
                if (context.Measures.Count() > 0)
                {
                    maxSerialNumber = context.Measures.Max(x => x.SerialNumber);
                }
                
                return ++maxSerialNumber;
            }
        }

        public Measure GetMeasureById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                Measure measure = new Measure();
                measure = context.Measures.Where(x => x.Id == Id)
                    .Include(x => x.MeasureLinks)
                    .Include(x => x.MeasurePictures)
                    .Include(x => x.OperationType)
                    .Include(x => x.DocumentItems)
                    .FirstOrDefault();

                if (measure != null)
                {
                    return measure;
                }
                return null;
            }
        }

        public Result SaveMeasure(Measure editMeasure)
        {
            Result output = new Result();
            Result<DocumentInstance> outputDocInstance = new Result<DocumentInstance>();
            outputDocInstance.Status = ResultStatus.OK;
            output.Status = ResultStatus.BadRequest;

            using (var ctx = new EWUSDbContext())
            {
                Measure measure = null;
                if (editMeasure.Id > 0)
                {
                    measure = ctx.Measures.Where(x => x.Id == editMeasure.Id)
                            .Include(x => x.MeasureLinks)
                            .Include(x => x.DocumentItems)
                            .Include(x => x.OperationType)
                            .FirstOrDefault();

                    if (measure != null)
                    {
                        if (measure.MeasureLinks != null)
                        {
                            List<MeasureLink> measureLinks = measure.MeasureLinks.ToList();
                            ctx.MeasureLinks.RemoveRange(measureLinks);
                        }

                        if (measure.DocumentItems != null)
                        {
                            List<DocumentItem> documents = measure.DocumentItems.ToList();
                            ctx.DocumentItems.RemoveRange(documents);
                        }
                    }
                }
                else
                {
                    measure = new Measure();
                }
                
                measure.Name = editMeasure.Name;
                measure.OperationType = ctx.Classifications.Where(x => x.Id == editMeasure.OperationTypeId).FirstOrDefault();
                measure.OperationTypeId = editMeasure.OperationTypeId;
                measure.Saving = editMeasure.Saving;
                measure.SerialNumber = editMeasure.SerialNumber;
                measure.Description = editMeasure.Description;
                measure.InvestmentCost = editMeasure.InvestmentCost;
                measure.MeasureLinks = editMeasure.MeasureLinks;
                measure.DocumentItems = editMeasure.DocumentItems;

                if (measure.Id == 0)
                    ctx.Measures.Add(measure);

                ctx.SaveChanges();

                if (!string.IsNullOrEmpty(measure.Guid.ToString()) && measure.DocumentItems != null)
                {
                    SaveFile.SaveFileInFolder(measure.Guid.ToString(), typeof(Measure).Name, measure.DocumentItems);
                }

                output = Result.ToResult<Measure>(ResultStatus.OK, typeof(Measure));
                output.Value = measure;
            }
            return output;
        }

        public Result DeleteMeasureById(long Id)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            try
            {
                using (var ctx = new EWUSDbContext())
                {
                    Measure measure = ctx.Measures.Where(x => x.Id == Id)
                            .Include(x => x.MeasureLinks)
                            .Include(x => x.DocumentItems)
                            .Include(x => x.OperationType)
                            .FirstOrDefault();
                    
                    try
                    {
                        ctx.Measures.Remove(measure);

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
            catch
            {
                output.Status = ResultStatus.InternalServerError;
            }

            return output;
        }
    }

}

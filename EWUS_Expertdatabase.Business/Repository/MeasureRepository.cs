﻿using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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

        public List<MeasurePoco> GetMeasuresNotRelatedWithProject(long projectId)
        {
            using (var context = new EWUSDbContext())
            {
                var measures = from m in context.Measures
                               join pm in context.ProjectMeasures.Where(x => x.ProjectId == projectId) on
                                            new { f1 = m.Id }
                                            equals
                                            new { f1 = pm.MeasureId } into cp
                               from q1 in cp.DefaultIfEmpty()
                               where q1.Measure == null
                               orderby m.Name
                               select new MeasurePoco
                               {
                                   Id = m.Id,
                                   Name = m.Name
                               };

                List<MeasurePoco> result = measures.ToList();
                if (result != null)
                {
                    return result;
                }

                return null;
            }
        }

        public Result SaveMeasure(Measure editMeasure)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            using (var ctx = new EWUSDbContext())
            {
                Measure measure = null;
                Collection<DocumentItem> documentItems = new Collection<DocumentItem>();
                Collection<MeasureLink> measureLinks = new Collection<MeasureLink>();
                if (editMeasure.Id > 0)
                {
                    measure = ctx.Measures.Where(x => x.Id == editMeasure.Id)
                            .Include(x => x.MeasureLinks)
                            .Include(x => x.DocumentItems)
                            .Include(x => x.OperationType)
                            .FirstOrDefault();
                    
                    if (editMeasure.DocumentItems != null && editMeasure.DocumentItems.Count() > 0)
                    {
                        foreach (var edi in editMeasure.DocumentItems)
                        {
                            var di = ctx.DocumentItems.Where(x => x.Id == edi.Id).FirstOrDefault();
                            if (di == null)
                            {
                                ctx.DocumentItems.Add(edi);
                                documentItems.Add(edi);
                            }
                            else
                            {
                                di.Hide = edi.Hide;
                                di.Description = edi.Description;
                                documentItems.Add(di);
                            }
                        }
                    }

                    var forDelete = from odi in measure.DocumentItems
                                    join edi in editMeasure.DocumentItems on
                                       new { f1 = odi.Id }
                                       equals
                                       new { f1 = edi.Id } into cp
                                    from q1 in cp.DefaultIfEmpty()
                                    where q1 == null
                                    select new 
                                    {
                                        Id = odi.Id
                                    };

                    List<long> idForDelete = forDelete.Select(x => x.Id ).ToList();

                    if (idForDelete.Count() > 0)
                    {
                        List<DocumentItem> collDi = ctx.DocumentItems.Where(x => idForDelete.Contains(x.Id)).ToList();
                        ctx.DocumentItems.RemoveRange(collDi);
                    }

                    if (editMeasure.MeasureLinks != null && editMeasure.MeasureLinks.Count() > 0)
                    {
                        foreach (var eml in editMeasure.MeasureLinks)
                        {
                            var ml = ctx.MeasureLinks.Where(x => x.Id == eml.Id).FirstOrDefault();
                            if (ml == null)
                            {
                                ctx.MeasureLinks.Add(eml);
                                measureLinks.Add(eml);
                            }
                            else
                            {
                                ml.Name = eml.Name;
                                ml.Link = eml.Link;
                                measureLinks.Add(ml);
                            }
                        }
                    }

                    forDelete = from oml in measure.MeasureLinks
                                join eml in editMeasure.MeasureLinks on
                                   new { f1 = oml.Id }
                                   equals
                                   new { f1 = eml.Id } into cp
                                from q1 in cp.DefaultIfEmpty()
                                where q1 == null
                                select new
                                {
                                    Id = oml.Id
                                };

                    idForDelete = forDelete.Select(x => x.Id).ToList();

                    if (idForDelete.Count() > 0)
                    {
                        List<MeasureLink> collMl = ctx.MeasureLinks.Where(x => idForDelete.Contains(x.Id)).ToList();
                        ctx.MeasureLinks.RemoveRange(collMl);
                    }
                }
                else
                {
                    measure = new Measure();
                    documentItems = editMeasure.DocumentItems;
                    measureLinks = editMeasure.MeasureLinks;
                }
                
                measure.Name = editMeasure.Name;
                measure.OperationType = ctx.Classifications.Where(x => x.Id == editMeasure.OperationTypeId).FirstOrDefault();
                measure.OperationTypeId = editMeasure.OperationTypeId;
                measure.Saving = editMeasure.Saving;
                measure.SerialNumber = editMeasure.SerialNumber;
                measure.Description = editMeasure.Description;
                measure.InvestmentCost = editMeasure.InvestmentCost;
                measure.SavingPercent = editMeasure.SavingPercent;
                measure.MeasureLinks = measureLinks;
                measure.DocumentItems = documentItems;

                if (measure.Id == 0)
                    ctx.Measures.Add(measure);

                ctx.SaveChanges();

                if (!string.IsNullOrEmpty(measure.Guid.ToString()) && measure.DocumentItems != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        SaveFile.SaveFileInFolder(measure.Guid.ToString(), typeof(Measure).Name, measure.DocumentItems);
                    });
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
                        if (ex.HResult == -2146233087 || ex.HResult == -2146233079)
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

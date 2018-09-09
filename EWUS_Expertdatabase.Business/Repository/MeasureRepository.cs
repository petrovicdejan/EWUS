using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Transactions;

namespace EWUS_Expertdatabase.Business
{
    public class MeasureRepository : IRepository
    {
        public List<Measure> GetMeasures()
        {
            using (var context = new EWUSDbContext())
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

        public static long GetMaxSerialNumber()
        {
            using (var context = new EWUSDbContext())
            {
                long maxSerialNumber = context.Measures.Max(x => x.SerialNumber);
                
                return ++maxSerialNumber;
            }
        }

        public object GetById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                Measure measure = new Measure();
                measure = context.Measures.Where(x => x.Id == Id)
                    .Include(x => x.MeasureLinks)
                    .Include(x => x.MeasurePictures)
                    .Include(x => x.OperationType)
                    .FirstOrDefault();

                if (measure != null)
                {
                    return measure;
                }
                return null;
            }
        }

        public IEnumerable<object> GetAll()
        {
            using (var context = new EWUSDbContext())
            {
                var measures = context.Measures.AsNoTracking()
                    .Include(x => x.MeasureLinks)
                    .Include(x => x.MeasurePictures)
                    .Include(x => x.OperationType).AsNoTracking()
                    .ToList();

                if (measures != null)
                {
                    return measures;
                }
                return null;
            }
        }

        public Result Save(Item value)
        {
            Result output = new Result();
            Result<DocumentInstance> outputDocInstance = new Result<DocumentInstance>();
            outputDocInstance.Status = ResultStatus.OK;
            output.Status = ResultStatus.BadRequest;

            long documentInstanceId = 0;
            string RefersTo_Type_Name = string.Empty;

            IEnumerable<Item> documentItems = null;
            string documentInformation = string.Empty;

            if (value?.Fields.Count == 0)
            {
                output.Status = ResultStatus.NotFound;
                output.ExceptionMessage = "No fields forward to save!";
                return output;
            }
            else
            {
                documentInstanceId = Convert.ToInt64(value.GetValue("Object_Id", 0));
                Field di = value.Fields.Where(x => x.Name == "DocumentInstances").FirstOrDefault();
                documentItems = di != null ? di.GetValueAsCollectionOfItems() : null;
            }

            using (var ctx = new EWUSDbContext())
            {
                Measure measure = null;
                Measure original = new Measure();
                
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    try
                    {
                        original = ItemExtension.ConvertTo(value, original) as Measure;
                        if (value.ObjectId.ToLong(0) > 0)
                        {
                            long measureId = value.ObjectId.ToLong(0);
                            measure = ctx.Measures.Where(x => x.Id == measureId)
                                        .Include(x => x.MeasureLinks)
                                        .Include(x => x.OperationType)
                                        .FirstOrDefault();

                            if (measure != null)
                            {
                                if (measure.MeasureLinks != null)
                                {
                                    List<MeasureLink> measureLinks = measure.MeasureLinks.ToList();
                                    ctx.MeasureLinks.RemoveRange(measureLinks);
                                }
                            }
                        }
                        else
                        {
                            measure = new Measure();
                        }

                        measure.Name = original.Name;
                        measure.OperationType.Id = original.OperationType.Id;
                        measure.OperationTypeId = (int?)original.OperationType.Id;
                        measure.Saving = original.Saving;
                        measure.SerialNumber = original.SerialNumber;
                        measure.Description = original.Description;
                        measure.InvestmentCost = original.InvestmentCost;
                        measure.MeasureLinks = original.MeasureLinks;


                        if (measure.Id == 0)
                        {
                            ctx.Measures.Add(measure);
                            ctx.Entry(measure.OperationType).State = EntityState.Unchanged;
                        }

                        ctx.SaveChanges();

                        output = Result.ToResult<Measure>(ResultStatus.OK, typeof(Measure));
                        output.Value = measure;

                        outputDocInstance = AddAttachment.InsertAttachment(documentInstanceId, measure.Id, "Measure", documentItems);
                        if (outputDocInstance.Success)
                        {
                            scope.Complete();
                        }

                    }
                    catch (Exception ex)
                    {
                        output.Status = ResultStatus.InternalServerError;
                    }
                }
            }
            return output;
        }

        #region IDisposable Support
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        #endregion

        public Result ExecuteCommand(Item value)
        {
            throw new NotImplementedException();
        }
    }

    public class MeasureLinkRepository : IRepository
    {
        public IEnumerable<object> GetAll()
        {
            using (var context = new EWUSDbContext())
            {
                var measureLinks = context.MeasureLinks.AsNoTracking()
                    .ToList();

                if (measureLinks != null)
                {
                    return measureLinks;
                }
                return null;
            }
        }

        public object GetById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                MeasureLink measureLink = new MeasureLink();
                measureLink = context.MeasureLinks.Where(x => x.Id == Id)
                    .FirstOrDefault();

                if (measureLink != null)
                {
                    return measureLink;
                }
                return null;
            }
        }

        #region IDisposable Support
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        #endregion

        public Result Save(Item value)
        {
            throw new NotImplementedException();
        }

        public Result ExecuteCommand(Item value)
        {
            throw new NotImplementedException();
        }
    }
}

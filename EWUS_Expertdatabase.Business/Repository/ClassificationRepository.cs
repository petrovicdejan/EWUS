using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;

namespace EWUS_Expertdatabase.Business
{
    public class ClassificationRepository : IRepository
    {
        public IEnumerable<object> GetAll()
        {
            using (var context = new EWUSDbContext())
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

        public object GetById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                Classification classification = new Classification();
                classification = context.Classifications.Where(x => x.Id == Id)
                    .FirstOrDefault();

                if (classification != null)
                {
                    return classification;
                }
                return null;
            }
        }
        
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
            throw new System.NotImplementedException();
        }

        public Result ExecuteCommand(Item value)
        {
            throw new System.NotImplementedException();
        }
    }
}

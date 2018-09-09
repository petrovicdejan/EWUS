using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Business
{
    public class DocumentInstanceRepository : IRepository
    {
        public object GetById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                DocumentInstance documentInstance = new DocumentInstance();
                documentInstance = context.DocumentInstances.Where(x => x.Id == Id)
                    .Include(x => x.DocumentItems)
                    .FirstOrDefault();

                if (documentInstance != null)
                {
                    return documentInstance;
                }
                return null;
            }
        }

        public IEnumerable<object> GetAll()
        {
            using (var context = new EWUSDbContext())
            {
                var documentInstances = context.DocumentInstances.AsNoTracking()
                    .Include(x => x.DocumentItems)
                    .ToList();

                if (documentInstances != null)
                {
                    return documentInstances;
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

        public DocumentInstance GetByRefers(long RefersToId, string RefersToTypeName)
        {
            using (var context = new EWUSDbContext())
            {
                DocumentInstance documentInstance = new DocumentInstance();
                documentInstance = context.DocumentInstances.Where(x => x.RefersTo.Id == RefersToId && x.RefersTo.TypeName == RefersToTypeName)
                    .Include(x => x.DocumentItems)
                    .FirstOrDefault();

                if (documentInstance != null)
                {
                    return documentInstance;
                }
                return null;
            }
        }

        public Result Save(Item value)
        {
            throw new NotImplementedException();
        }

        public Result ExecuteCommand(Item value)
        {
            throw new NotImplementedException();
        }
    }

    public class DocumentItemRepository : IRepository
    {
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
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            try
            {
                string objectId = value.ObjectId;

                using (var ctx = new EWUSDbContext())
                {
                    var documentItem = ctx.DocumentItems.Where(x => x.ObjectId == objectId).FirstOrDefault();
                    ctx.DocumentItems.Remove(documentItem);

                    ctx.SaveChanges();
                }
                output.Status = ResultStatus.OK;
            }
            catch
            {
                output.Status = ResultStatus.InternalServerError;
            }

            return output;
        }

        public IEnumerable<object> GetAll()
        {
            throw new NotImplementedException();
        }

        public object GetById(long Id)
        {
            throw new NotImplementedException();
        }

        public Result Save(Item value)
        {
            throw new NotImplementedException();
        }
    }
}

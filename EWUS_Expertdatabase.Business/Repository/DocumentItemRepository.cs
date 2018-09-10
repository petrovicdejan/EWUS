using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System.Linq;

namespace EWUS_Expertdatabase.Business
{
    public class DocumentItemRepository
    {
        public Result DeleteDocumentItem(string docItemObjectId)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            try
            {
                using (var ctx = new EWUSDbContext())
                {
                    DocumentItem documentItem = ctx.DocumentItems.Where(x => x.ObjectId == docItemObjectId).FirstOrDefault();
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
    }
}

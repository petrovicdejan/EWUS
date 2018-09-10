using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace EWUS_Expertdatabase.Business
{
    public class AddAttachment
    {
        public static Result<DocumentInstance> InsertAttachment(long documentInstanceId, long RefersTo_Id, string RefersTo_Type_Name, IEnumerable<Item> documentItems, string objectGuid = "", string objectTypeName = "")
        {
            Result<DocumentInstance> output = new Result<DocumentInstance>();
            output.Status = ResultStatus.OK;

            DocumentInstance documentInstance = null;
            List<DocumentItem> lstDocumentItem = null;
            string documentInformation = string.Empty;

            try
            {
                using (var ctx = new EWUSDbContext())
                {
                    if (documentItems != null && documentItems.Count() > 0)
                    {
                        DocumentInstance originalDocumentInstance = new DocumentInstance();
                        foreach (var item in documentItems)
                        {
                            documentInformation = item.GetValue("Document", string.Empty) as String;

                            DocumentItem documentItem = new DocumentItem();

                            Attachment convertObject = JsonConvert.DeserializeObject<Attachment>(documentInformation.ToString());

                            if (convertObject != null)
                            {
                                documentItem.ObjectId = convertObject.ObjectId;
                                documentItem.DocumentName = convertObject.DocumentName;
                                documentItem.DocumentSize = convertObject.DocumentSize;
                                documentItem.DocumentMimeType = convertObject.DocumentMimeType;
                                documentItem.Description = convertObject.Description;
                                originalDocumentInstance.DocumentItems.Add(documentItem);
                            }
                        }

                        documentInstance = ctx.DocumentInstances.Where(x => x.Id == documentInstanceId)
                                           .Include(x => x.DocumentItems)
                                           .FirstOrDefault();

                        if (documentInstance == null)
                        {
                            //originalDocumentInstance.RefersTo = new Reference(RefersTo_Id, RefersTo_Type_Name);
                            ctx.DocumentInstances.Add(originalDocumentInstance);
                        }
                        else
                        {
                            lstDocumentItem = documentInstance.DocumentItems.ToList();
                            ctx.DocumentItems.RemoveRange(lstDocumentItem);
                            documentInstance.DocumentItems = originalDocumentInstance.DocumentItems;
                        }
                        
                        ctx.SaveChanges();
                        output.Status = ResultStatus.OK;

                       
                    }
                }
            }
            catch (Exception ex)
            {
                output.Status = ResultStatus.InternalServerError;
            }

            return output;
        }
    }
}

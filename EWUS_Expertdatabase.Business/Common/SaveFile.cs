using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Model;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.IO;

namespace EWUS_Expertdatabase.Business
{
    public class SaveFile
    {
        public static void SaveFileInFolder(string objectGuid, string objectTypeName, Collection<DocumentItem> documentItems)
        {
            foreach (var docItem in documentItems)
            {
                string path = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + objectTypeName] + @"\" + docItem.ObjectId);
                string folderName = ConfigurationManager.AppSettings["SharedFolder_" + objectTypeName];
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                Image imgThumbnail = Utils.CreateThumbnail(Image.FromStream(stream), new Size(200, 200));

                string filePrefix = string.Empty;
                string newFileName = string.Empty;
                string thumbnailFileName = string.Empty;
                string fileExtension = docItem.DocumentName.Substring(docItem.DocumentName.IndexOf('.') + 1);

                if (objectTypeName == "Measure")
                    filePrefix = "m";
                else if (objectTypeName == "ProjectMeasure")
                    filePrefix = "pm";
                else if (objectTypeName == "Customer")
                    filePrefix = "c";

                newFileName = filePrefix + "_" + objectGuid + "_o_" + docItem.DocumentName.Substring(0, docItem.DocumentName.IndexOf('.')) + ".png";
                thumbnailFileName = filePrefix + "_" + objectGuid + "_t_" + docItem.DocumentName.Substring(0, docItem.DocumentName.IndexOf('.')) + ".png";

                if (!File.Exists(Path.Combine(folderName, thumbnailFileName)))
                {
                    //copy to thumbnail
                    imgThumbnail.Save(Path.Combine(folderName, thumbnailFileName));
                }

                if (!File.Exists(Path.Combine(folderName, newFileName)))
                {
                    //copy to original with new name
                    using (var fileStream = File.Create(Path.Combine(folderName, newFileName)))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                //using (var ctx = new EWUSDbContext())
                //{
                //    DocumentItem di = ctx.DocumentItems.Where(x => x.ObjectId == docItem.ObjectId).FirstOrDefault();

                //    if (di != null)
                //    {
                //        di.ObjectId = newFileName;
                //        ctx.SaveChanges();
                //    }
                //}

                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //}
            }
        }
    }
}

using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace EWUS_Expertdatabase.Business
{
    public class SaveFile
    {
        public static void SaveFileInFolder(string objectGuid, string objectTypeName, Collection<DocumentItem> documentItems)
        {
            Thread.Sleep(200);
            foreach (var docItem in documentItems)
            {
                string path = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + objectTypeName] + @"\" + docItem.ObjectId);
                string folderName = ConfigurationManager.AppSettings["SharedFolder_" + objectTypeName];
                string viewFileName = string.Empty;

                using (var stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Image imgThumbnail = Utils.CreateThumbnail(Image.FromStream(stream), new Size(200, 200));
                    Image imgView = Utils.CreateThumbnail(Image.FromStream(stream), new Size(400, 400));

                    string filePrefix = string.Empty;
                    string fileExtension = docItem.DocumentName.Substring(docItem.DocumentName.IndexOf('.') + 1);

                    if (objectTypeName == "Measure")
                        filePrefix = "m";
                    else if (objectTypeName == "ProjectMeasure")
                        filePrefix = "pm";
                    else if (objectTypeName == "Customer")
                        filePrefix = "c";

                    string newFileName = filePrefix + "_" + objectGuid + "_o_" + docItem.DocumentName.Substring(0, docItem.DocumentName.IndexOf('.')) + ".png";
                    viewFileName = filePrefix + "_" + objectGuid + "_v_" + docItem.DocumentName.Substring(0, docItem.DocumentName.IndexOf('.')) + ".png";
                    string thumbnailFileName = filePrefix + "_" + objectGuid + "_t_" + docItem.DocumentName.Substring(0, docItem.DocumentName.IndexOf('.')) + ".png";

                    if (!File.Exists(Path.Combine(folderName, thumbnailFileName)))
                    {
                        //copy to thumbnail
                        imgThumbnail.Save(Path.Combine(folderName, thumbnailFileName));
                    }

                    if (!File.Exists(Path.Combine(folderName, viewFileName)))
                    {
                        //copy to thumbnail
                        imgView.Save(Path.Combine(folderName, viewFileName));
                    }

                    if (!File.Exists(Path.Combine(folderName, newFileName)))
                    {
                        //copy to original with new name
                        using (var fileStream = File.Create(Path.Combine(folderName, newFileName)))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
                if (docItem.ObjectId.StartsWith("_"))
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    using (var ctx = new EWUSDbContext())
                    {
                        DocumentItem di = ctx.DocumentItems.Where(x => x.ObjectId == docItem.ObjectId).FirstOrDefault();

                        if (di != null)
                        {
                            di.ObjectId = viewFileName;
                            ctx.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}

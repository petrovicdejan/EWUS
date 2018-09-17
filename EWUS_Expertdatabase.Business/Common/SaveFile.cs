using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System;
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
            Thread.Sleep(3000);
            try
            {
                foreach (var docItem in documentItems)
                {
                    string path = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + objectTypeName] + @"\" + docItem.ObjectId);
                    string folderName = ConfigurationManager.AppSettings["SharedFolder_" + objectTypeName];
                    string newFileName = string.Empty;

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

                        newFileName = filePrefix + "_" + objectGuid + "_o_" + docItem.DocumentName.Substring(0, docItem.DocumentName.IndexOf('.')) + ".png";
                        string viewFileName = filePrefix + "_" + objectGuid + "_v_" + docItem.DocumentName.Substring(0, docItem.DocumentName.IndexOf('.')) + ".png";
                        string thumbnailFileName = filePrefix + "_" + objectGuid + "_t_" + docItem.DocumentName.Substring(0, docItem.DocumentName.IndexOf('.')) + ".png";

                        if (!File.Exists(Path.Combine(folderName, thumbnailFileName)))
                        {
                            imgThumbnail.Save(Path.Combine(folderName, thumbnailFileName));
                        }

                        if (!File.Exists(Path.Combine(folderName, viewFileName)))
                        {
                            imgView.Save(Path.Combine(folderName, viewFileName));
                        }
                    }
                    if (docItem.ObjectId.StartsWith("_"))
                    {
                        if (File.Exists(path))
                        {
                            File.Move(path, Path.Combine(folderName, newFileName));
                        }

                        using (var ctx = new EWUSDbContext())
                        {
                            DocumentItem di = ctx.DocumentItems.Where(x => x.ObjectId == docItem.ObjectId).FirstOrDefault();

                            if (di != null)
                            {
                                di.ObjectId = newFileName;
                                ctx.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}

using EWUS_Expertdatabase.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EWUS_Expertdatabase.Common
{
    public class Utils
    {
        public static void SaveFileInFolder(string objectGuid, string objectTypeName, Collection<DocumentItem> documentItems)
        {
            foreach (var docItem in documentItems)
            {
                string path = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + objectTypeName] + @"\" +docItem.ObjectId);
                string folderName = ConfigurationManager.AppSettings["SharedFolder_" + objectTypeName];
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                System.Drawing.Image imgThumbnail = CreateThumbnail(System.Drawing.Image.FromStream(stream), new Size(200, 200));

                string filePrefix = string.Empty;
                string newFileName = string.Empty;
                string thumbnailFileName = string.Empty;
                string fileExtension = docItem.DocumentName.Substring(docItem.DocumentName.IndexOf('.')+1);

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
            }
        }
        
        public static System.Drawing.Image CreateThumbnail(System.Drawing.Image image, Size thumbnailSize)
        {
            float scalingRatio = CalculateScalingRatio(image.Size, thumbnailSize);

            int scaledWidth = (int)Math.Round((float)image.Size.Width * scalingRatio);
            int scaledHeight = (int)Math.Round((float)image.Size.Height * scalingRatio);
            int scaledLeft = (thumbnailSize.Width - scaledWidth) / 2;
            int scaledTop = (thumbnailSize.Height - scaledHeight) / 2;

            // For portrait mode, adjust the vertical top of the crop area so that we get more of the top area
            if (scaledWidth < scaledHeight && scaledHeight > thumbnailSize.Height)
            {
                scaledTop = (thumbnailSize.Height - scaledHeight) / 4;
            }

            Rectangle cropArea = new Rectangle(scaledLeft, scaledTop, scaledWidth, scaledHeight);

            System.Drawing.Image thumbnail = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);
            using (Graphics thumbnailGraphics = Graphics.FromImage(thumbnail))
            {
                thumbnailGraphics.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                thumbnailGraphics.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraphics.DrawImage(image, cropArea);
            }
            return thumbnail;
        }

        private static float CalculateScalingRatio(Size originalSize, Size targetSize)
        {
            float originalAspectRatio = (float)originalSize.Width / (float)originalSize.Height;
            float targetAspectRatio = (float)targetSize.Width / (float)targetSize.Height;

            float scalingRatio = 0;

            if (targetAspectRatio >= originalAspectRatio)
            {
                scalingRatio = (float)targetSize.Width / (float)originalSize.Width;
            }
            else
            {
                scalingRatio = (float)targetSize.Height / (float)originalSize.Height;
            }

            return scalingRatio;
        }

        public static HttpStatusCode ToHttpCode(ResultStatus status)
        {
            if (status == ResultStatus.OK)
                return HttpStatusCode.OK;
            else if (status == ResultStatus.Created)
                return HttpStatusCode.Created;
            else if (status == ResultStatus.BadRequest)
                return HttpStatusCode.BadRequest;
            else if (status == ResultStatus.NotFound)
                return HttpStatusCode.NotFound;
            else
                return HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Deserializes a JSON formatted string to an object of the defined type
        /// </summary>
        /// <param name="jsonString">JSON formatted string</param>
        /// <param name="objType">The type of the object which the jsonString is to be Deserialized to.</param>
        /// <returns>Deserialized object</returns>
        /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
        public static object JsonStringToObject(string jsonString, Type objType)
        {
            try
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(objType);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
                using (MemoryStream jsonStream = new MemoryStream(jsonBytes))
                {

                    return js.ReadObject(jsonStream);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}


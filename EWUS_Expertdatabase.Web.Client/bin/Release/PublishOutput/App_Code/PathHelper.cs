using System;
using System.Web;

namespace EWUS_Expertdatabase.Web
{
    public static class PathHelper
    {
        private static string appPath = string.Empty;

        public static string FullyQualifiedApplicationPath(HttpRequestBase httpRequestBase)
        {
            string s = System.Configuration.ConfigurationManager.AppSettings["rootUrl"];

            if (String.IsNullOrEmpty(s) == false)
            {
                return s;
            }

            if (string.IsNullOrWhiteSpace(appPath) == false)
                return appPath;

            if (httpRequestBase != null)
            {
                //Formatting the fully qualified website url/name
                appPath = string.Format("{0}://{1}{2}{3}",
                            httpRequestBase.Url.Scheme,
                            httpRequestBase.Url.Host,
                            httpRequestBase.Url.Port == 80 ? string.Empty : ":" + httpRequestBase.Url.Port,
                            httpRequestBase.ApplicationPath);
            }

            if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }

            return appPath;
        }
    }
}
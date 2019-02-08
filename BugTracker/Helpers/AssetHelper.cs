using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Helpers
{

    public static class AssetHelper
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null) return false;
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024) return false;

            try
            {
                foreach(var ext in WebConfigurationManager.AppSettings["ValidExtentions"].Split(','))
                {
                    if (Path.GetExtension(file.FileName).Contains(ext))
                        return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
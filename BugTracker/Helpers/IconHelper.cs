using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class IconHelper
    {
        public static string GetIcon(string FileName)
        {
            var Icon = "";
            var fileExtention = Path.GetExtension(FileName);
            switch (fileExtention)
            {
                case ".pdf":
                    Icon = "fa fa-file-pdf-o";
                    break;
                case ".doc":
                case ".docx":
                    Icon = "fa fa-file-word-o";
                    break;
                case ".xls":
                    Icon = "fa fa-file-exel=o";
                    break;
                case ".txt":
                    Icon = "fa fa-file-txt-o";
                    break;
                case ".png":
                case ".jpeg":
                case ".gif":
                    Icon = "fa fa-file-image-o";
                    break;
                default:
                    Icon = "fa fa-file-code-o";
                    break;
            }
            return Icon;
        }
    }
}
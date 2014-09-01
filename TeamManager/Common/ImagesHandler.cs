using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamManager.Common
{
    public class ImagesHandler
    {

        public static string Upload(HttpPostedFileBase image, string imagesPath, string prefix)
        {
            if (image == null || image.ContentLength <= 0) return String.Empty;

            var filename = String.Format("{0}_{1}", prefix, Path.GetFileName(image.FileName));
            var filepath = getFilePath(imagesPath, filename);

            image.SaveAs(filepath);
            return filename;
        }

        public static void Delete(string imagesPath, string filename)
        {
            if (String.IsNullOrEmpty(filename)) return;

            var filepath = getFilePath(imagesPath, filename);

            if (System.IO.File.Exists(filepath))
                System.IO.File.Delete(filepath);
        }

        private static string getFilePath(string imagesPath, string filename)
        {
            return Path.Combine(imagesPath, filename);
        }
    }
}
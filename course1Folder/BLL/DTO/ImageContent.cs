using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace course1Folder.BLL.DTO
{
    public class ImageContent
    {

        public ImageContent(HttpPostedFileBase file = null)
        {
            if (file != null)
            {
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    Content = binaryReader.ReadBytes(file.ContentLength);
                    Mime = file.ContentType;
                }

            }
        }
        public ImageContent() { }

        public byte[] Content { get; set; }
        public string Mime { get; set; }

    }
}
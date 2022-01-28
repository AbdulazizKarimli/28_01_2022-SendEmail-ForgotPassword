using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Utils
{
    public static class FileUtil
    {
        public static string GenerateFile(string folderPath, IFormFile formFile)
        {
            var fileName = $"{Guid.NewGuid()}-{formFile.FileName}";
            var newFolderPath = Path.Combine(folderPath, fileName);

            using (FileStream stream = new FileStream(newFolderPath, FileMode.Create))
                formFile.CopyTo(stream);

            return fileName;
        }
    }
}

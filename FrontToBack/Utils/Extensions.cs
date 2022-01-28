using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Utils
{
    public static class Extensions
    {
        public static bool IsImage(this IFormFile formFile)
        {
            return formFile.ContentType.Contains("image/");
        }
    }
}

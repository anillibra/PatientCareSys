using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string Message)
        {
            // Add addtional Headers on Response
            response.Headers.Add("Application-Error", Message);
        }
    }
}

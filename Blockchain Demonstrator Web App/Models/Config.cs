using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models
{
    public static class Config
    {
        public static string WebApplicationUrl { get; set; }
        public static string RestApiUrl { get; set; }
    }
}

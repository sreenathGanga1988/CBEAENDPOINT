using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBEAPI.ViewModels
{
    public static class AppSettingsProvider
    {
        public static string DbConnectionString { get; set; }
        public static string BaseApiUri { get; set; }
        public static bool IsDevelopment { get; set; }
    }
}

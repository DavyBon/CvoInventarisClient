using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.DAL.Helpers
{
    public class DatabaseConnection
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["CvoInventarisDBConnection"].ConnectionString;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.App_Code
{
    public class DALutil
    {
        public static object checkIntForDBNUll(int? i)
        {
            if (i == null)
            {
                return DBNull.Value;
            }
            else
            {
                return i;
            }
        }
    }
}
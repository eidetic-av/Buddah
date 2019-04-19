using System;
using System.Collections.Generic;
using System.Text;

namespace Eidetic.Buddah
{
    /// <summary>
    /// Platform specific logging
    /// </summary>
    class Logger
    {
        public static void WriteLine(object o = null, params object[] args)
        {
            var message = "";
            if (o != null)
            {
                var currentTimePrefix = System.DateTime.Now.ToLongTimeString();
                var stringifiedObject = o.ToString();
                message = currentTimePrefix + " - " + stringifiedObject;
            }
#if NETCOREAPP2_2
            Console.WriteLine(message, args);
#endif
        }
    }
}

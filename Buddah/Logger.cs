using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Eidetic.Buddah
{
    /// <summary>
    /// Platform specific logging
    /// </summary>
    public static class Logger
    {
        // Default logger is the dotnet console
        public static Action<string, object[]> LogDelegate { get; set; } = Console.WriteLine;

        public static void WriteLine(object o = null, params object[] args) => LogDelegate.Invoke(o != null ? o.ToString() : "", args);
    }
}

using System;
using System.Reflection;
using NUnit.Common;
using NUnitLite;

namespace ClassLibrary
{
    public static class Program 
    {
    public static int Main(string[] args)
    {
        #if NET40

return new AutoRun(typeof(Program).Assembly).Execute(args);
        #else
            return new AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args);
        #endif
    }
    }
}

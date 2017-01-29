using NUnitLite;
using System.Reflection;

namespace ClassLibrary
{
    internal static class Program
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
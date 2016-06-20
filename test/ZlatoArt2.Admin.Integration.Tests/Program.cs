using NUnitLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Integration.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DNX451
            new AutoRun().Execute(args);
#else
            new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
#endif
        }
    }
}

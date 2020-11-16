using CommandLine;
using System;

namespace Juntar
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Parametros>(args)
            .WithParsed(o => new Main(o).Iniciar());
            GC.Collect();
            Environment.Exit(0);
        }
    }
}

using CommandLine;
using System;

namespace Juntar
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Opcoes>(args)
            .WithParsed(o => new Main(o).Iniciar());
        }
    }
}

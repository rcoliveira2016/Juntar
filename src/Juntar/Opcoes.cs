
using CommandLine;

namespace Juntar
{
    public class Opcoes
    {
        [Option('n', "naoMostrar", Required = false, HelpText = "não mostrar logs.")]
        public bool NaoMostrar { get; set; }

        [Option('s', "subPasta", Required = false, HelpText = "Concaternar sql recursivamente")]
        public bool SubPasta { get; set; }

        [Option('c', "clipBord", Required = false, HelpText = "Apenas copiar na area de tranferencia")]
        public bool ClipBord { get; set; }
    }

}

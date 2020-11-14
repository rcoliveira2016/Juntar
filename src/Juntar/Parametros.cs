
using CommandLine;

namespace Juntar
{
    public class Parametros
    {
        [Option('v', "verbeso", Required = false, HelpText = "Mostrar logs e descrição do processo.")]
        public bool Verbeso { get; set; }

        [Option('s', "subPasta", Required = false, HelpText = "Concaternar sql recursivamente.")]
        public bool SubPasta { get; set; }

        [Option('c', "clipBord", Required = false, HelpText = "Apenas copiar na area de tranferencia.")]
        public bool ClipBord { get; set; }

        [Option('p', "pastaDestino", Required = false, HelpText = "Pasta destino onde será compilado os arquivos.")]
        public string PastaDestino { get; set; }

        [Option('a', "NomeAquivoFinal", Required = false, HelpText = "Nome do aquivo compilado.")]
        public string NomeAquivoFinal { get; set; }
    }

}

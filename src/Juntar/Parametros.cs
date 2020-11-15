
using CommandLine;
using System.Collections.Generic;

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

        [Option('a', "CaminhoAquivoCompilado", Required = false, HelpText = "Caminho do aquivo compilado.")]
        public string CaminhoAquivoCompilado { get; set; }

        [Option(
            'e', 
            "Extencao", 
            Required = false, 
            HelpText = "Exntenção para filtrar. Padão .sql. Exemplo: .sql,.txt.,.cs ",
            Separator =',',
            Default = new string[] { ".sql" })]
        public IList<string> Extencao { get; set; }
    }

}

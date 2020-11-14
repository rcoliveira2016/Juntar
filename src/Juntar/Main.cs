using AutoIt.Common;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juntar
{
    public class Main
    {
        private static readonly string PastaAtual = Environment.CurrentDirectory;
        private static readonly StringBuilder TextoArquivofinal = new StringBuilder();
        private static readonly string[] FiltroExtencaoArquivo = new string[] { ".sql" };
        private static readonly string NomeAquivoFinal = "compilado-juntar.sql";
        private static readonly string NnomeArquivoCompilado = $@"{PastaAtual}\{NomeAquivoFinal}";
        public readonly Opcoes opcoes;
        private static int ContadorDeArquivos = 0;

        public Main(Opcoes opcoes)
        {
            this.opcoes = opcoes;
        }

        public void Iniciar()
        {
            AdicionarArquivosEmSubPastas();

            EscreverConsole($"PASTA - {ObterNomeAmigavel(PastaAtual)}");

            TextoArquivofinal.Append(LerArquivoRetornarTexto(PastaAtual));

            EscreverConsole($"!!AVISO: Foram compilado apenas arquivos com essas extensões {FiltroExtencaoArquivo.Aggregate((a, b) => $"{a}, {b}")} ");
            EscreverConsole($"TOTAL de arquivos compilados {ContadorDeArquivos}");

            if (opcoes.ClipBord)
                WindowsClipboard.SetText(TextoArquivofinal.ToString());
            else
                SalvarArquivoCompilado();
        }

        private void SalvarArquivoCompilado()
        {
            if (File.Exists(NnomeArquivoCompilado))
                File.Delete(NnomeArquivoCompilado);
            File.WriteAllText(NnomeArquivoCompilado, TextoArquivofinal.ToString(), Encoding.UTF8);

        }

        private void AdicionarArquivosEmSubPastas()
        {
            if (!opcoes.SubPasta)
                return;

            var diretorios = Directory.GetDirectories(PastaAtual);
            foreach (var pastas in diretorios)
                TextoArquivofinal.Append(LerSubPastasRetornarTexto(pastas));
        }

        private string LerSubPastasRetornarTexto(string diretorioParametro)
        {
            var retorno = new StringBuilder();
            foreach (var pastaAtual in Directory.GetDirectories(diretorioParametro))
            {
                EscreverConsole($"PASTA - {(pastaAtual)}");
                retorno.Append(LerSubPastasRetornarTexto(pastaAtual));
            }

            EscreverConsole($"PASTA - {ObterNomeAmigavel(diretorioParametro)}");
            retorno.Append(LerArquivoRetornarTexto(diretorioParametro));
            return retorno.ToString();
        }

        private string LerArquivoRetornarTexto(string pasta)
        {
            var retorno = new StringBuilder();
            var arquivos = Directory.GetFiles(pasta, "*.*").Where(s => FiltroExtencaoArquivo.Any(f => s.EndsWith(f))).OrderBy(x => x);
            foreach (var arquivo in arquivos)
            {
                var nomeArquivo = ObterNomeAmigavel(arquivo);
                retorno.AppendLine("-------------------------------------------------------------------------------------------------------");
                retorno.AppendLine($"-- {nomeArquivo}");
                retorno.AppendLine("-------------------------------------------------------------------------------------------------------");
                retorno.AppendLine(ObterTextoAquivo(arquivo));
                EscreverConsole($"Arquivo: {nomeArquivo}");
                ContadorDeArquivos++;
            }

            return retorno.ToString();
        }
        private string ObterTextoAquivo(string arquivo)
        {
            var endodeArquivo = GetEncoding(arquivo);
            var textoArquivo = File.ReadAllText(arquivo, endodeArquivo);
            EscreverConsole(textoArquivo);
            return textoArquivo;
        }

        public static Encoding GetEncoding(string filename)
        {
            var textDetect = new TextEncodingDetect();
            TextEncodingDetect.Encoding encoding = textDetect.DetectEncoding(File.ReadAllBytes(filename), File.ReadAllBytes(filename).Length);
            switch (encoding)
            {
                case TextEncodingDetect.Encoding.None:
                    return Encoding.Default;
                case TextEncodingDetect.Encoding.Ansi:
                    return Encoding.Default;
                case TextEncodingDetect.Encoding.Ascii:
                    return Encoding.ASCII;
                case TextEncodingDetect.Encoding.Utf8Bom:
                    return Encoding.UTF8;
                case TextEncodingDetect.Encoding.Utf8Nobom:
                    return new UTF8Encoding(false);
                case TextEncodingDetect.Encoding.Utf16LeBom:
                    return Encoding.Unicode;
                case TextEncodingDetect.Encoding.Utf16LeNoBom:
                    return new UnicodeEncoding(false, false);
                case TextEncodingDetect.Encoding.Utf16BeBom:
                    return Encoding.BigEndianUnicode;
                case TextEncodingDetect.Encoding.Utf16BeNoBom:
                    return new UnicodeEncoding(true, false);
                default:
                    return Encoding.Default;
            }
        }

        public static string ObterNomeAmigavel(string diretorio)
        {
            return diretorio?.Split('\\')?.LastOrDefault();
        }

        public void EscreverConsole(string mensagem)
        {
            if(!opcoes.NaoMostrar)
                Console.WriteLine(mensagem);
        }
    }
}

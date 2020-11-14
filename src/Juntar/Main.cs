using AutoIt.Common;
using CommandLine;
using Juntar.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juntar
{
    public class Main
    {
        private const string _nomeAquivoFinal = "compilado-juntar.sql";
        private static readonly string[] _filtroExtencaoArquivo = new string[] { ".sql" };
        private static readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly StringBuilder _textoArquivofinal;
        private readonly string _nomeArquivoCompilado;
        private readonly Parametros Parametros;

        private static int _contadorDeArquivos = 0;

        public Main(Parametros parametros)
        {
            _stopwatch.Start();
            this.Parametros = parametros;

            _textoArquivofinal = new StringBuilder();

            if (string.IsNullOrEmpty(this.Parametros.PastaDestino))
                this.Parametros.PastaDestino = Environment.CurrentDirectory;
            if (string.IsNullOrEmpty(this.Parametros.NomeAquivoFinal))
                this.Parametros.NomeAquivoFinal = _nomeAquivoFinal;

            _nomeArquivoCompilado = $@"{Parametros.PastaDestino}\{Parametros.NomeAquivoFinal}";
        }

        public void Iniciar()
        {
            AdicionarArquivosEmSubPastas();

            ObterTextoPastaAtual();

            if (Parametros.ClipBord)
                WindowsClipboard.SetText(_textoArquivofinal.ToString());
            else
                SalvarArquivoCompilado();

            NotificarAposCompilarConteudoArquivos();
        }

        private void NotificarAposCompilarConteudoArquivos()
        {
            _stopwatch.Stop();

            EscreverConsole("!!AVISO: ", true, ConsoleColor.Red);
            EscreverConsole($"Foram compilado apenas arquivos com essas extensões:", true, ConsoleColor.DarkGreen);
            EscreverConsole($" {_filtroExtencaoArquivo.Aggregate((a, b) => $"{a}, {b}")}", true, ConsoleColor.Yellow);

            EscreverConsoleQuebraLinha(true);
            EscreverConsole("TOTAL de ", true, ConsoleColor.DarkGreen);
            EscreverConsole($"{_contadorDeArquivos} ", true, ConsoleColor.Yellow);
            EscreverConsole("arquivos compilados", true, ConsoleColor.DarkGreen);

            EscreverConsoleQuebraLinha(true);
            EscreverConsole("Tempo de excução: ", true, ConsoleColor.DarkGreen);
            EscreverConsole($"{_stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:ff")}", true, ConsoleColor.Yellow);
        }

        private void ObterTextoPastaAtual()
        {
            EscreverTelaNomePasta(this.Parametros.PastaDestino);

            _textoArquivofinal.Append(LerArquivoRetornarTexto(this.Parametros.PastaDestino));
        }

        private void SalvarArquivoCompilado()
        {
            if (File.Exists(_nomeArquivoCompilado))
                File.Delete(_nomeArquivoCompilado);

            File.WriteAllText(_nomeArquivoCompilado, _textoArquivofinal.ToString(), Encoding.UTF8);
        }

        private void AdicionarArquivosEmSubPastas()
        {
            if (!Parametros.SubPasta)
                return;

            var diretorios = Directory.GetDirectories(this.Parametros.PastaDestino);
            foreach (var pastas in diretorios)
                _textoArquivofinal.Append(LerSubPastasRetornarTexto(pastas));
        }

        private string LerSubPastasRetornarTexto(string diretorioParametro)
        {
            var retorno = new StringBuilder();
            foreach (var pastaAtual in Directory.GetDirectories(diretorioParametro))
            {
                EscreverTelaNomePasta(pastaAtual);
                retorno.Append(LerSubPastasRetornarTexto(pastaAtual));
            }

            EscreverTelaNomePasta(diretorioParametro);
            retorno.Append(LerArquivoRetornarTexto(diretorioParametro));
            return retorno.ToString();
        }

        private string LerArquivoRetornarTexto(string pasta)
        {
            var retorno = new StringBuilder();
            var arquivos = Directory.GetFiles(pasta).Where(s => _filtroExtencaoArquivo.Any(f => s.EndsWith(f))).OrderBy(x => x);
            foreach (var arquivo in arquivos)
            {
                var nomeArquivo = ObterNomeAmigavel(arquivo);
                retorno.AppendLine("-------------------------------------------------------------------------------------------------------");
                retorno.AppendLine($"-- {nomeArquivo}");
                retorno.AppendLine("-------------------------------------------------------------------------------------------------------");
                retorno.AppendLine(ObterTextoAquivo(arquivo));
                EscreverConsoleQuebraLinha($"Arquivo: {nomeArquivo}", consoleColor: ConsoleColor.Yellow);
                _contadorDeArquivos++;
            }

            EscreverConsoleQuebraLinha();
            return retorno.ToString();
        }
        private string ObterTextoAquivo(string arquivo)
        {
            var bytesArquivo = File.ReadAllBytes(arquivo);
            var endodeArquivo = GetEncoding(bytesArquivo);
            
            return endodeArquivo.GetString(bytesArquivo);
        }

        public static Encoding GetEncoding(byte[] bytesFile)
        {
            TextEncodingDetect.Encoding encoding = new TextEncodingDetect().DetectEncoding(bytesFile, bytesFile.Length);
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

        private static string ObterNomeAmigavel(string diretorio)
        {
            return diretorio?.Split('\\')?.LastOrDefault();
        }

        private void EscreverTelaNomePasta(string diretorio) => EscreverConsoleQuebraLinha($"PASTA: {diretorio}", consoleColor: ConsoleColor.Magenta);

        private void EscreverConsoleQuebraLinha(string mensagem, bool ignorarVerbeso = false, ConsoleColor consoleColor = default(ConsoleColor))
        {
            EscreverConsoleBase(mensagem, Console.WriteLine, ignorarVerbeso, consoleColor);
        }

        private void EscreverConsole(string mensagem, bool ignorarVerbeso = false, ConsoleColor consoleColor = default(ConsoleColor))
        {
            EscreverConsoleBase(mensagem, Console.Write, ignorarVerbeso, consoleColor);
        }

        private void EscreverConsoleQuebraLinha(bool ignorarVerbeso = false)
        {
            if (ignorarVerbeso || Parametros.Verbeso)
                Console.WriteLine();
        }

        private void EscreverConsoleBase(string mensagem, Action<string> consoleWrite, bool ignorarVerbeso = false, ConsoleColor consoleColor = default(ConsoleColor))
        {
            if(ignorarVerbeso || Parametros.Verbeso)
            {
                Console.ForegroundColor = consoleColor;

                consoleWrite(mensagem);

                Console.ResetColor();
            }
        }
    }
}

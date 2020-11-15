using AutoIt.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Juntar.Text
{
    public static class FileHelper
    {
        public static string ObterTextoAquivo(string arquivo)
        {
            var bytesArquivo = File.ReadAllBytes(arquivo);
            var endodeArquivo = GetEncoding(bytesArquivo);

            return endodeArquivo.GetString(bytesArquivo);
        }

        public static Encoding GetEncoding(byte[] bytesFile)
        {
            // https://stackoverflow.com/questions/33033514/how-can-i-convert-values-stored-in-ansi-windows-1252-in-a-database-to-utf-8
            TextEncodingDetect.Encoding encoding = new TextEncodingDetect().DetectEncoding(bytesFile, bytesFile.Length);
            switch (encoding)
            {
                case TextEncodingDetect.Encoding.None:
                    return Encoding.Default;
                case TextEncodingDetect.Encoding.Ansi:
                    return Encoding.UTF7;
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
    }
}

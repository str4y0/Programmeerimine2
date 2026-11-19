using System.IO;

namespace KooliProjekt.Application.Infrastructure.Files
{
    public static class FileNameHelper
    {
        // Failinimi on ohutu, kui see on ainult nimi (ilma kaustateeta).
        // Vastasel juhul võiks nimi "../../midagi.txt" viia faili kausta alt välja.
        public static bool IsSafeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            if (fileName == "." || fileName == "..")
            {
                return false;
            }

            if (fileName.IndexOfAny(new[] { '/', '\\' }) >= 0)
            {
                return false;
            }

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return false;
            }

            return true;
        }
    }
}

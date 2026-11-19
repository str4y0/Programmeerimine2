using System.IO;

namespace KooliProjekt.Application.Dto
{
    public class FileDto
    {
        public string FileName { get; set; }

        // Faili sisu. Selle sulgeb (dispose) see, kes faili kliendile tagastab
        public Stream Content { get; set; }
    }
}

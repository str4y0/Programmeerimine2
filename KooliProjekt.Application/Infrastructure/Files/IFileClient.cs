using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Infrastructure.Files
{
    public interface IFileClient
    {
        // Salvestab faili. Kui sama nimega fail on juba olemas, kirjutatakse see üle
        Task Save(string fileName, string fileStore, Stream inputStream);

        // Kustutab faili. Tagastab true, kui fail oli olemas ja kustutati
        Task<bool> Delete(string fileName, string fileStore);

        // Tagastab faili sisu voona. Kui faili ei leita, tagastab null.
        // Voo sulgemise eest vastutab see, kes Get'i välja kutsus.
        Task<Stream> Get(string fileName, string fileStore);

        // Tagastab kõigi antud kaustas olevate failide nimed
        Task<IList<string>> List(string fileStore);
    }
}

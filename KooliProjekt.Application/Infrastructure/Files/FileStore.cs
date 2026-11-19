using System.IO;

namespace KooliProjekt.Application.Infrastructure.Files
{
    public static class FileStore
    {
        // Võistkondade failid (lipud, logod)
        public const string Teams = "teams";

        // Iga võistkonna failid on oma alamkaustas: teams/{teamId}
        // Nii ei lähe erinevate võistkondade failid segamini ja võistkonna
        // kustutamisel on lihtne leida kõik temaga seotud failid.
        public static string ForTeam(int teamId)
        {
            return Path.Combine(Teams, teamId.ToString());
        }
    }
}

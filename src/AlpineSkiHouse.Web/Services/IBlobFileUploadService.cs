using System.IO;
using System.Threading.Tasks;

namespace AlpineSkiHouse.Web.Services
{
    public interface IBlobFileUploadService
    {
        Task<string> UploadFileFromStream(string containerName, string targetFilename, Stream fileStream);
    }
}
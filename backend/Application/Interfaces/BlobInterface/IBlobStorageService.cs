using System.IO;
using System.Threading.Tasks;
namespace Application.Interfaces.BlobInterface;


public interface  IBlobStorageService
{

    Task UploadFileAsync(string blobName, Stream fileStream);
    string GetBlobUrl(string blobName);
}
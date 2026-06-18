using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentService
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct);
        bool Delete(string fileName, string FolderName);
        (Stream stream, string ContentType)? GetFile(string fileName, string FolderName);
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _maxFile = 5 * 1024 * 1024;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly ILogger<AttachmentService> _logger;
        private readonly IWebHostEnvironment _env;

        public AttachmentService(ILogger<AttachmentService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public bool Delete(string fileName, string FolderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(FolderName)) return false;
            try
            {
                var filePath = Path.Combine(_env.ContentRootPath, FolderName, fileName);
                if (!File.Exists(filePath)){
                    return false;  
                }
                File.Delete(filePath);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to Delete attachment{fileName}");
                return false;

            }
        }

        public (Stream stream, string ContentType)? GetFile(string fileName, string FolderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(FolderName)) return null;
            var filePath = Path.Combine(_env.ContentRootPath, FolderName, fileName);
            if (!File.Exists(filePath)) return null;
            var contentType = Path.GetExtension(filePath).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/cotet-stream"
            };
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (stream, contentType);
        }

        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct)
        {
            if (fileStream is null || !fileStream.CanRead) return null;
            if (fileStream.Length == 0) return null; 
            if(fileStream.Length> _maxFile)
            {
                _logger.LogWarning($"Reject Upload : file is too large ({fileStream.Length}) Bytes");
                return null;
            }
            var extension = Path.GetExtension(fileName);
            if(string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
            {
                _logger.LogWarning($"Reject Upload : Extension {extension} is not allowed");
                return null;
            }
            var folderPath = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory(folderPath);
            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, storedFileName);

            try
            {
                await using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write,FileShare.None);
                await fileStream.CopyToAsync(fs, ct);
                return storedFileName;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to upload file{fileName}");
                return null;
            }

        }
    }
}

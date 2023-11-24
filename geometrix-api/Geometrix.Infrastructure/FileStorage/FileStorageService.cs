using Geometrix.Application.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Geometrix.Infrastructure.FileStorage;

[Obsolete("Use FileStorageServiceV2 instead")]
public class FileStorageService(
    IHostEnvironment env,
    ILogger<FileStorageService> logger)
    : IFileStorageService
{
    public async Task<string?> SaveFileAsync(
        byte[] dataArray,
        string nameWithoutExtension,
        string extension = "")
    {
        var fileName = $"{nameWithoutExtension}.png";
        var path = Path.Combine(env.ContentRootPath, "wwwroot", "images");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var fullFileLocation = Path.Combine(path, fileName);

        await using var fileStream = new FileStream(fullFileLocation, FileMode.Create);

        // Write the data to the file, byte by byte.
        foreach (var b in dataArray)
        {
            fileStream.WriteByte(b);
        }

        // Set the stream position to the beginning of the file.
        fileStream.Seek(0, SeekOrigin.Begin);

        // Read and verify the data.
        for (var i = 0; i < fileStream.Length; i++)
        {
            if (dataArray[i] == fileStream.ReadByte())
                continue;
            
            logger.LogError("Error writing data");
            return null;
        }

        logger.LogInformation("The data was written to {Name} and verified", fileStream.Name);
        
        return fileName;
    }
}
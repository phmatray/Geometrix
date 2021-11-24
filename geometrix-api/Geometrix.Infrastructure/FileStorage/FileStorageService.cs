using Geometrix.Application.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Geometrix.Infrastructure.FileStorage;

public class FileStorageService : IFileStorageService
{
    private readonly IHostEnvironment _env;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(
        IHostEnvironment env,
        ILogger<FileStorageService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<string?> SaveFileAsync(byte[] dataArray, string nameWithoutExtension)
    {
        string fileName = $"{nameWithoutExtension}.png";
        string path = Path.Combine(_env.ContentRootPath, "wwwroot", "images");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fullFileLocation = Path.Combine(path, fileName);

        await using var fileStream = new FileStream(fullFileLocation, FileMode.Create);

        // Write the data to the file, byte by byte.
        foreach (byte b in dataArray)
        {
            fileStream.WriteByte(b);
        }

        // Set the stream position to the beginning of the file.
        fileStream.Seek(0, SeekOrigin.Begin);

        // Read and verify the data.
        for (int i = 0; i < fileStream.Length; i++)
        {
            if (dataArray[i] != fileStream.ReadByte())
            {
                _logger.LogError("Error writing data.");
                return null;
            }
        }

        _logger.LogInformation("The data was written to {Name} and verified.", fileStream.Name);
        
        return fileName;
    }
}
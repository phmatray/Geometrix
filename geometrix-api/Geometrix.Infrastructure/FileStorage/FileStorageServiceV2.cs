using Geometrix.Application.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Geometrix.Infrastructure.FileStorage;

public class FileStorageServiceV2(
    IHostEnvironment env,
    ILogger<FileStorageServiceV2> logger)
    : IFileStorageService
{
    private const string DefaultImageExtension = ".png";
    private const string DefaultImageFolder = "wwwroot/images";

    public async Task<string?> SaveFileAsync(
        byte[] dataArray,
        string nameWithoutExtension,
        string extension = DefaultImageExtension)
    {
        var fileName = $"{nameWithoutExtension}{extension}";
        var path = Path.Combine(env.ContentRootPath, DefaultImageFolder);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var fullFileLocation = GenerateUniquePath(path, fileName);

        try
        {
            await File.WriteAllBytesAsync(fullFileLocation, dataArray);
            logger.LogInformation("The data was written to {Name}", fullFileLocation);
            return Path.GetFileName(fullFileLocation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error writing data to {Name}", fullFileLocation);
            return null;
        }
    }

    private static string GenerateUniquePath(string path, string fileName)
    {
        var fullPath = Path.Combine(path, fileName);
        if (!File.Exists(fullPath))
            return fullPath;

        var counter = 1;
        while (File.Exists(Path.Combine(path,
                   $"{Path.GetFileNameWithoutExtension(fileName)} ({counter}){Path.GetExtension(fileName)}")))
        {
            counter++;
        }

        return Path.Combine(path,
            $"{Path.GetFileNameWithoutExtension(fileName)} ({counter}){Path.GetExtension(fileName)}");
    }
}
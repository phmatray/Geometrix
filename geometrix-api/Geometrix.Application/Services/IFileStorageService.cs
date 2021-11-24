namespace Geometrix.Application.Services;

public interface IFileStorageService
{
    Task<string?> SaveFileAsync(byte[] bytes, string nameWithoutExtension);
}
namespace Geometrix.Application.Services;

public interface IFileStorageService
{
    Task<string?> SaveFileAsync(
        byte[] dataArray,
        string nameWithoutExtension,
        string extension);
}
using Microsoft.Extensions.Options;
using RentBikeApi.Domain.Interfaces;

namespace RentBikeApi.Infrastructure.Storage
{
    public class LocalFileStorage(IOptions<StorageOptions> options): IFileStorage
    {
        private readonly string _basePath = options.Value.BasePath;

        public async Task<string> SaveAsync(byte[] content, string fileName)
        {
            Directory.CreateDirectory(_basePath);

            var path = Path.Combine(_basePath, fileName);
            await File.WriteAllBytesAsync(path, content);

            return path;
        }

        public Task DeleteAsync(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
            return Task.CompletedTask;
        }
    }
}

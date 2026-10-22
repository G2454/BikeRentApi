
namespace RentBikeApi.Domain.Interfaces
{
    public interface IFileStorage
    {
        Task<string> SaveAsync(byte[] content, string fileName);
        Task DeleteAsync(string path);
    }
}

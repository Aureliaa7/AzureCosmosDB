namespace BlazorAppWithCosmosDB.Data
{
    public interface IUserService
    {
        Task UpsertAsync(User user);

        Task DeleteAsync(string? id, string? partitionKey);

        Task<List<User>> GetAllAsync();

        Task<User> GetAsync(string? id, string? partitionKey);
    }
}

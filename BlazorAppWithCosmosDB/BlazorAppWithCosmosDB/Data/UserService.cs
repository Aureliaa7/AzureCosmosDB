
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace BlazorAppWithCosmosDB.Data
{
    public class UserService : IUserService
    {
        private readonly IOptions<CosmosDbOptions> cosmosDbOptions;

        public UserService(IOptions<CosmosDbOptions> cosmosDbOptions)
        {
            this.cosmosDbOptions = cosmosDbOptions;
        }

        public async Task UpsertAsync(User user)
        {
            try
            {
                if (user.id == null)
                {
                    user.id = Guid.NewGuid();
                }
                var container = GetContainerClient();
                var response = await container.UpsertItemAsync(user, new PartitionKey(user.id.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not update user. Exception thrown: {ex.Message}");
            }
        }

        public async Task DeleteAsync(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.DeleteItemAsync<User>(id, new PartitionKey(partitionKey));
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not delete user. Exception thrown: {ex.Message}");
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();
            try
            {
                var container = GetContainerClient();
                var sqlQueryText = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<User> queryResultSetIterator = container.GetItemQueryIterator<User>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<User> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    users.AddRange(currentResultSet.ToList());
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get all users. Exception thrown: {ex.Message}");
            }
        }

        public async Task<User> GetAsync(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.ReadItemAsync<User>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get user. Exception thrown: {ex.Message}");
            }
        }

        private Container GetContainerClient()
        {
            CosmosClient cosmosClient = new CosmosClient(cosmosDbOptions.Value.ConnectionString);
            Container container = cosmosClient.GetContainer(cosmosDbOptions.Value.DatabaseName, cosmosDbOptions.Value.ContainerName);
            return container;
        }
    }
}
using Amazon.DynamoDBv2;
using RaspberryPi.API.Contracts.Data;

namespace RaspberryPi.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public CommentRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
        }

        public Task<bool> CreateAsync(Comment user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Comment?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Comment user)
        {
            throw new NotImplementedException();
        }
    }
}

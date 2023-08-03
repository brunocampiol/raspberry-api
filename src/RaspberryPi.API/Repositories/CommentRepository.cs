using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Fetchgoods.Text.Json.Extensions;
using RaspberryPi.API.Contracts.Data;
using System.Net;
using System.Text.Json;

namespace RaspberryPi.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private const string _tableName = "Comments";
        private readonly IAmazonDynamoDB _dynamoDb;

        public CommentRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
        }

        public async Task<bool> CreateAsync(Comment user)
        {
            var customerAsJson = user.ToJson();
            var itemAsDocument = Document.FromJson(customerAsJson);
            var itemAsAttributes = itemAsDocument.ToAttributeMap();

            var createItemRequest = new PutItemRequest()
            {
                TableName = _tableName,
                Item = itemAsAttributes
            };

            var response = await _dynamoDb.PutItemAsync(createItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<Comment?> GetAsync(Guid id)
        {
            var request = new GetItemRequest()
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    { "pk", new AttributeValue { S = id.ToString() } },
                    { "sk", new AttributeValue { S = id.ToString() } },
                }
            };

            var response = await _dynamoDb.GetItemAsync(request);
            if (response.Item.Count == 0) { return default; }

            var itemAsDocument = Document.FromAttributeMap(response.Item);
            var dbEntry = itemAsDocument.ToJson().FromJsonTo<Comment>();

            return dbEntry;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var request = new DeleteItemRequest()
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    { "pk", new AttributeValue { S = id.ToString() } },
                    { "sk", new AttributeValue { S = id.ToString() } },
                }
            };

            var response = await _dynamoDb.DeleteItemAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Comment user)
        {
            var customerAsJson = JsonSerializer.Serialize(user);
            var itemAsDocument = Document.FromJson(customerAsJson);
            var itemAsAttributes = itemAsDocument.ToAttributeMap();

            var createItemRequest = new PutItemRequest()
            {
                TableName = _tableName,
                Item = itemAsAttributes
            };

            var response = await _dynamoDb.PutItemAsync(createItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}

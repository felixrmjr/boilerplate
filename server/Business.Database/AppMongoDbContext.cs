using Business.Domain.Model;
using Business.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Business.Database
{
    public class AppMongoDbContext : IDisposable
    {
        private IMongoDatabase _database;
        private IMongoClient _client;

        public AppMongoDbContext(IOptions<MongoConnection> settings)
        {
            _client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress("dynamiclabs.ddns.net", 27017),
                ClusterConfigurator = cluster =>
                {
                    cluster.Subscribe<CommandStartedEvent>(command =>
                    {
                        //var query = command.Command.ToJson(new JsonWriterSettings { Indent = true });
                    });
                }
            });
            if (_client != null)
                _database = _client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<User>? Users
        {
            get
            {
                return _database.GetCollection<User>("users");
            }
        }

        public IMongoCollection<Category>? Categories
        {
            get
            {
                return _database.GetCollection<Category>("categories");
            }
        }

        public void Dispose()
        {
            _client = null;
            _database = null;
        }
    }
}
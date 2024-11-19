using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Showcase.Identity.Data.Documents;
using Showcase.Identity.Settings;

namespace Showcase.Identity.Data.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Session> Session => _database.GetCollection<Session>("Sessions");
}

public static class MongoCollectionExtensions
{
    public static IMongoCollection<T> WithPartitionKey<T>(
        this IMongoCollection<T> collection, 
        string partitionKey)
    {
        var tagSets = new List<TagSet> { new ([new Tag("email", partitionKey)])};
        
        return collection.WithReadPreference(new ReadPreference(ReadPreferenceMode.PrimaryPreferred, tagSets));
    }

    public static async Task<TDocument> FindOneAsync<TDocument, TField>(
        this IMongoCollection<TDocument> collection, 
        Expression<Func<TDocument, TField>> lambda, 
        TField value,
        CancellationToken cancellationToken = default)
        => await collection
            .FindAsync(Builders<TDocument>.Filter.Eq(lambda, value), cancellationToken: cancellationToken)
            .ContinueWith(t => t.Result.FirstOrDefault(), cancellationToken);

    public static async Task<List<TDocument>> FindAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        params KeyValuePair<Expression<Func<TDocument, object>>, object>[] givenFilters)
    {
        var filters = givenFilters
            .Select(x => Builders<TDocument>.Filter.Eq(x.Key, x.Value))
            .ToList();
        
        return await collection
            .FindAsync(Builders<TDocument>.Filter.And(filters))
            .ContinueWith(t => t.Result.ToList());
    }
    
    public static async Task<List<TDocument>> FindAsync<TDocument, TField>(
        this IMongoCollection<TDocument> collection, 
        Expression<Func<TDocument, TField>> lambda, 
        TField value,
        CancellationToken cancellationToken = default)
        => await collection
            .FindAsync(Builders<TDocument>.Filter.Eq(lambda, value), cancellationToken: cancellationToken)
            .ContinueWith(t => t.Result.ToList(), cancellationToken);
    
    public static async Task<List<TDocument>> FindAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        CancellationToken cancellationToken = default)
        => await collection
            .FindAsync(Builders<TDocument>.Filter.Empty, cancellationToken: cancellationToken)
            .ContinueWith(t => t.Result.ToList(), cancellationToken);
}
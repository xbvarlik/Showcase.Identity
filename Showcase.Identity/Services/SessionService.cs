using System.Linq.Expressions;
using System.Runtime.InteropServices;
using MongoDB.Driver;
using Showcase.Identity.Data.Context;
using Showcase.Identity.Data.Documents;
using Showcase.Identity.Data.Models;
using Showcase.Identity.InMemoryCache;

namespace Showcase.Identity.Services;

public class SessionService(MongoDbContext mongoDbContext, ICacheManager cacheManager)
{
    public async Task CreateSessionAsync(SessionCreateModel model, CancellationToken cancellationToken = default)
    {
        var session = new Session
        {
            AuthProperties = model.UserTokenModel,
            UserId = model.User.Id,
            FirstName = model.User.FirstName,
            LastName = model.User.LastName,
            Agent = model.Agent,
            ClientId = model.UserTokenModel.ClientId,
            Email = model.User.Email!,
            NormalizedEmail = model.User.NormalizedEmail!,
            UserRoles = model.UserTokenViewModel.Roles,
            CreatedAt = DateTime.UtcNow,
            ChangedAt = DateTime.UtcNow
        };

        try
        {
            await mongoDbContext.Session.InsertOneAsync(session, cancellationToken: cancellationToken);
            await SetAsync(model.UserTokenModel.ClientId, session, cancellationToken);
        }
        catch(Exception e)
        {
            //throw new ImplikaDatabaseException(e);
        }
    }

    public async Task<List<Session>> GetUserSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await mongoDbContext.Session.FindAsync(x => x.UserId, userId, cancellationToken: cancellationToken);
    }

    public async Task<List<string>> GetUserConnectionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var sessions = await GetUserSessionsAsync(userId, cancellationToken);
        
        return sessions.Where(x => x.UserId == userId && x.SignalRConnectionId != null)
            .Select(y => y.SignalRConnectionId!).ToList();
    }
        
    
    public async Task<Session?> GetAsync(string email, Guid clientId, CancellationToken cancellationToken = default)
    { 
        return await mongoDbContext.Session
            .WithPartitionKey(email)
            .FindOneAsync(x=>x.AuthProperties.ClientId, clientId, cancellationToken: cancellationToken);
    }
    
    public async Task<Session?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await mongoDbContext.Session
            .FindOneAsync(x => x.AuthProperties.Token, token, cancellationToken: cancellationToken);
    }

    public async Task UpdateSessionAsync(Session session, CancellationToken cancellationToken = default)
    {
        try
        {
            var userIdFilter = Builders<Session>.Filter
                .Eq(x => x.UserId, session.UserId);
            
            var clientIdFilter = Builders<Session>.Filter
                .Eq(x => x.ClientId, session.ClientId);

            var filter = Builders<Session>.Filter.And(userIdFilter, clientIdFilter);

            var update = Builders<Session>.Update
                .Set(x => x, session);

            await mongoDbContext.Session.UpdateOneAsync(filter, update, default,  cancellationToken);
        }
        catch (Exception e)
        {
            //throw new ImplikaDatabaseException(e);
        }
    }

    public async Task DeleteSessionAsync(string email, Guid clientId, CancellationToken cancellationToken = default)
    {
        var session = await GetAsync(email, clientId, cancellationToken);

        if (session is null)
            return;

        try
        {
            var userIdFilter = Builders<Session>.Filter
                .Eq(x => x.UserId, session.UserId);
            
            var clientIdFilter = Builders<Session>.Filter
                .Eq(x => x.ClientId, session.ClientId);

            var filter = Builders<Session>.Filter.And(userIdFilter, clientIdFilter);
            
            await mongoDbContext.Session.DeleteOneAsync(filter, cancellationToken);
        }
        catch (Exception e)
        {
            //throw new ImplikaDatabaseException(e);
        }
    }

    public async Task DeleteSessionsAsync([Optional]Guid userId, [Optional]string email, CancellationToken cancellationToken = default)
    {
        var sessions = new List<Session>();
        
        if(!string.IsNullOrEmpty(email))
            sessions = await mongoDbContext.Session.WithPartitionKey(email).FindAsync(cancellationToken);
        
        if(userId != Guid.Empty)
            sessions = await mongoDbContext.Session.FindAsync(x => x.UserId, userId, cancellationToken);

        try
        {
            await mongoDbContext.Session.DeleteManyAsync(x => sessions.Contains(x), cancellationToken);
        }
        catch (Exception e)
        {
            //throw new ImplikaDatabaseException(e);
        }
    }

    public async Task SetAsync(Guid clientId, Session session, CancellationToken cancellationToken = default)
    {
        var existingSession = await cacheManager.GetAsync<Session>(clientId.ToString(), cancellationToken);
        
        if (existingSession is null)
            cacheManager.Set(clientId.ToString(), session);
        else
            cacheManager.Update(clientId.ToString(), session);
    }
}
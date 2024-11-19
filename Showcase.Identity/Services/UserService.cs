using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Showcase.Identity.Data.Context;
using Showcase.Identity.Data.Entities;
using Showcase.Identity.Data.Mappers;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Exceptions;
using Showcase.Identity.InMemoryCache;
using Showcase.Identity.Utils;

namespace Showcase.Identity.Services;

public class UserService(
    UserCredentials credentials,
    MssqlContext context,
    IUserStore<User> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<User> passwordHasher,
    IEnumerable<IUserValidator<User>> userValidators,
    IEnumerable<IPasswordValidator<User>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<User>> logger,
    ICacheManager cacheManager)
    : UserManager<User>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer,
        errors, services, logger)
{
    private DbSet<User> GetEntityDbSet()
    {
        return context.Users;
    }
    
    public IQueryable<User> GetQueryable() => GetEntityDbSet().Where(x => !x.IsDeleted)
        .AsQueryable();
    
    public async Task<UserViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetQueryable()
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new UserViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email ?? string.Empty,
                PhoneNumber = x.PhoneNumber ?? string.Empty,
                FullName = x.FullName,
            }).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<UserProfileViewModel?> GetUserProfileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetQueryable()
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new UserProfileViewModel()
            {
                FirstName = x.FirstName,
            }).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<AccountModel?> GetAccountVariablesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users.Where(x => x.Id == credentials.UserId).Select(x => new AccountModel
        {
            UserId = x.Id,
            FullName = x.FullName,
        }).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<(List<UserViewModel>, PaginationModel)> GetAllAsync(UserQueryFilterModel filter, CancellationToken cancellationToken = default)
    {
        return await GetQueryable()
            .Where(x => !x.IsDeleted)
            .Select(x => new UserViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email ?? string.Empty,
                PhoneNumber = x.PhoneNumber ?? string.Empty,
                FullName = x.FullName,
                
            }).ToPaginatedResultAsync(filter, cancellationToken);
    }
    
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await GetEntityDbSet().FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted, cancellationToken);
    }
    
    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await GetEntityDbSet().FirstOrDefaultAsync(x => x.UserName == userName && !x.IsDeleted, cancellationToken);
    }
    
    public async Task<User?> CreateAsync(UserCreateModel model, CancellationToken cancellationToken = default)
    {
        var entity = model.ToCreatedEntity();
        //await _userServiceGuard.CheckCreateDataAndThrowAsync(model, cancellationToken);
            
        try
        {
            var result = await CreateAsync(entity, model.Password);
            
            //await userRoleService.AddUserRolesAsync(userRoleModel, cancellationToken);
            if (!result.Succeeded)
                throw new ShowcaseDatabaseException();
            var flagsModel = new UserFlagsModel
            {
                IsEmailConfirmed = entity.EmailConfirmed,
                IsTermsAndConditionsAccepted = entity.IsTermsAndConditionsAccepted,
            };
            
            cacheManager.Set(entity.Id.ToString(), flagsModel);

        }
        catch (Exception)
        {
            //throw new ImplikaDatabaseException();
        }
        
        return entity;
    }
    
    public async Task<User> UpdateAsync(Guid id, UserUpdateModel model, CancellationToken cancellationToken = default)
    {
        var entity = await context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        
        // if (entity is null)
        //     throw new ImplikaBusinessException(ExceptionConstants.Business.UserNotFoundError);
        
        //await _userServiceGuard.CheckUpdateDataAndThrowAsync(model, cancellationToken);
        
        model.ToUpdatedEntity(entity, credentials);
        
        try
        {
            await UpdateAsync(entity);
        }
        catch (Exception)
        {
            //throw new ImplikaDatabaseException();
        }
        
        return entity;
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        
        // if (entity is null)
        //     throw new ImplikaBusinessException(ExceptionConstants.Business.UserNotFoundError);
        
        await DeleteAsync(entity);
    }
    
    public async Task<UserFlagsModel?> GetUserFlags(Guid id,CancellationToken cancellationToken = default)
    {
      
           var cacheModel = await cacheManager.GetAsync<UserFlagsModel>(credentials.UserId.ToString(), cancellationToken);

           if (cacheModel is null)
           {
               return await context.Users
                   .Where(x => x.Id == id)
                   .Select(x => new UserFlagsModel
                   {
                       IsEmailConfirmed = x.EmailConfirmed,
                       IsTermsAndConditionsAccepted = x.IsTermsAndConditionsAccepted,
                   })
                   .FirstOrDefaultAsync(cancellationToken) ?? throw new ShowcaseDatabaseException();
           }

           return cacheModel;
    }

    public async Task UpdateTermsAndConditionsAcceptedAsync(bool isAccepted, CancellationToken cancellationToken = default)
    {
        var user = await FindByIdAsync(credentials.UserId.ToString()) ??
                   throw new ShowcaseNotFoundException(ExceptionConstants.Business.NotFoundError);
        
        var currentModel = await GetUserFlags(credentials.UserId, cancellationToken) ?? throw new ShowcaseNullException();
        currentModel.IsTermsAndConditionsAccepted = isAccepted;
        
        HasAllFlagsAreChecked(currentModel);

        user.IsTermsAndConditionsAccepted = isAccepted;
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateEmailConfirmedAsync(bool isConfirmed, CancellationToken cancellationToken = default)
    {
        var user = await FindByIdAsync(credentials.UserId.ToString()) ??
                   throw new ShowcaseNotFoundException(ExceptionConstants.Business.NotFoundError);
        
        var currentModel = await GetUserFlags(credentials.UserId, cancellationToken) ?? throw new ShowcaseNullException();
        currentModel.IsEmailConfirmed = isConfirmed;
        
        HasAllFlagsAreChecked(currentModel);
        
        user.EmailConfirmed = isConfirmed;
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private void HasAllFlagsAreChecked(UserFlagsModel currentModel)
    {
        if (!currentModel.IsEmailConfirmed || 
            !currentModel.IsTermsAndConditionsAccepted)
        {
            cacheManager.Update(credentials.UserId.ToString(), currentModel); 
        }
        else
        {
            cacheManager.Remove(credentials.UserId.ToString());
        }
    }
}
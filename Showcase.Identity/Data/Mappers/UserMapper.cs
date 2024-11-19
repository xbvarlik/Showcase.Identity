using System.Globalization;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Entities;
using Showcase.Identity.Data.Models;

namespace Showcase.Identity.Data.Mappers;

public static class UserMapper
{
    public static UserCreateModel ToCreateModel(this RegisterModel model)
    {
        var lastName = model.FullName.Split(" ").Last();
        var firstName = model.FullName.Replace(lastName, string.Empty);
        
        return new UserCreateModel
        {
            FirstName = firstName,
            LastName = lastName,
            Email = model.Email,
            Password = model.Password,
            UniversityEmail = model.Email,
            PhoneNumber = model.PhoneNumber,
            Birthdate = model.Birthdate,
            CampusId = model.CampusId,
            UniversityStartYear = model.UniversityStartYear,
            GraduationYear = model.GraduationYear,
            DepartmentId = model.DepartmentId,
            GenderId = model.GenderId,
            VerificationDocument = model.VerificationDocument
        };
    }
    
    public static User ToCreatedEntity(this UserCreateModel model)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserName =  model.Email!.Split("@")[0],
            NormalizedUserName = model.FirstName.ToUpper(CultureInfo.InvariantCulture) + model.LastName.ToUpper(CultureInfo.InvariantCulture),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.PhoneNumber,
            Birthdate = new DateOnly(model.Birthdate,1,1),
            Email = model.Email,
            GenderId = model.GenderId,
            NormalizedEmail = model.Email.ToUpper(),
            EmailConfirmed = false,
            IsTermsAndConditionsAccepted = true,
            PhoneNumber = model.PhoneNumber,
            PhoneNumberConfirmed = true,
            CreatedAt = DateTime.Now,
            ChangedAt = DateTime.Now,
            CreatedBy = BootstrapperConstant.SystemUserId,
            ChangedBy = BootstrapperConstant.SystemUserId,
            IsDeleted = false,
        };
    }
    
    public static User ToUpdatedEntity(this UserUpdateModel model, User entity, UserCredentials credentials)
    {
        entity.FirstName = model.FirstName ?? entity.FirstName;
        entity.LastName = model.LastName ?? entity.LastName;
        entity.Phone = model.PhoneNumber ?? entity.Phone;
        entity.Birthdate = model.Birthdate ?? entity.Birthdate;
        entity.ChangedAt = DateTime.Now;
        entity.ChangedBy = credentials.UserId;
        
        return entity;
    }
    
    public static User ToDeletedEntity(this User entity, UserCredentials credentials)
    {
        entity.IsDeleted = true;
        entity.ChangedAt = DateTime.Now;
        entity.ChangedBy = credentials.UserId;
        
        return entity;
    }
}
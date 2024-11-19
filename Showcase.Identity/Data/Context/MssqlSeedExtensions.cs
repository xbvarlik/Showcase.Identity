using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Entities;

namespace Showcase.Identity.Data.Context;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "MemberCanBeMadeReadOnly.Global")]
public static class MssqlSeedExtensions
{
    private static Guid consumerId1 = Guid.Parse("9d65a509-3859-479f-8e38-2fe64cc35f21");
    private static Guid userId1 = Guid.Parse("19f26154-ebcb-4fd0-950b-28ba36c386bd");
    private static Guid userId2 = Guid.Parse("a21a0414-5bf6-42a4-91f7-9c7767f493f4");
    private static Guid systemUserId = Guid.Parse("2eec7d17-bbcd-4abf-898c-b4a5f073a14e");
    private static Guid superAdminId = Guid.Parse("fcd9dd06-1cd1-4543-9560-64e660fffbff");

    private static Guid consumerRoleId = Guid.Parse("838b8386-1ccf-4af3-9aed-2130a7bdac7e");
    private static Guid superAdminRoleId = Guid.Parse("6f4b76bd-d780-4636-af28-2e9c1638bdbc");
    private static Guid sellerRoleId = Guid.Parse("f85655fb-7239-48a3-a205-5e6a8048fd0c");
    private static Guid userRoleId = Guid.Parse("f3ab6ba5-7a9b-4bb4-b952-d7cb450fde92");
    
    public static void Seed(this ModelBuilder builder)
    {
        builder.SeedGender();
        builder.SeedIdentityTables();
    }

    
    private static void SeedGender(this ModelBuilder builder)
    {
        builder.Entity<Gender>().HasData(new List<Gender>
        {
            new()
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false
            },
            new()
            {
                Id = 2,
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false
            },
            new()
            {
                Id = 3,
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false
            },
        });
        
        builder.Entity<GenderText>().HasData(new List<GenderText>
        {
            new()
            {
                Id = 1,
                Text = "Erkek",
                LanguageId = "TR",
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false,
                GenderId = 01,
            },
            new()
            {
                Id = 2,
                Text = "Male",
                LanguageId = "EN",
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false,
                GenderId = 1,
            },
            new()
            {
                Id = 3,
                Text = "Kadın",
                LanguageId = "TR",
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false,
                GenderId = 2,
            },
            new()
            {
                Id = 4,
                Text = "Female",
                LanguageId = "EN",
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false,
                GenderId = 2,
            },
            new()
            {
                Id = 5,
                Text = "Diğer",
                LanguageId = "TR",
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false,
                GenderId = 3,
            },
            new()
            {
                Id = 6,
                Text = "Other",
                LanguageId = "EN",
                CreatedAt = DateTime.Now,
                CreatedBy = systemUserId,
                IsDeleted = false,
                GenderId = 3,
            },
        });
    }
    
    private static void SeedIdentityTables(this ModelBuilder builder)
    {
        
        builder.Entity<Role>().HasData(new List<Role>
        {
            new() {Id  = consumerRoleId, NormalizedName = AuthConstants.UserRoles.Consumer.ToUpper(), Name = AuthConstants.UserRoles.Consumer},
            new() {Id  = userRoleId, NormalizedName = AuthConstants.UserRoles.User.ToUpper(), Name = AuthConstants.UserRoles.User},
            new() {Id  = superAdminRoleId, NormalizedName = AuthConstants.UserRoles.SuperAdmin.ToUpper(), Name = AuthConstants.UserRoles.SuperAdmin},
            new() {Id  = sellerRoleId, NormalizedName = AuthConstants.UserRoles.Seller.ToUpper(), Name = AuthConstants.UserRoles.Seller}
        });

        User consumer = new()
        {
            Id = consumerId1,
            UserName = "consumer",
            NormalizedUserName = "consumer",
            Email = "consumer@vesta.com",
            NormalizedEmail = "consumer@VESTA.COM",
            EmailConfirmed = true,
            AccessFailedCount = 0,
            FirstName = "RealEstate",
            LastName = "Admin1",
            SecurityStamp = Guid.NewGuid()
                .ToString(),
            //StudiedProgram = "Real Estate Management",
            Phone = "123456789012",
            PhoneNumberConfirmed = true,
            Birthdate = new DateOnly(1999,
                10,
                24),
            GenderId = 1,
            IsTermsAndConditionsAccepted = true,
            ConcurrencyStamp = Guid.NewGuid()
                .ToString(),
            PhoneNumber = "",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = systemUserId,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = systemUserId,
            IsDeleted = false,

        };

        consumer.CreatePasswordHash("123vesta-_-");
        
        User superAdmin = new()
        {
            Id = superAdminId,
            UserName = "SuperAdmin",
            NormalizedUserName = "SUPERADMIN",
            Email = "SuperAdmin@vesta.com",
            NormalizedEmail = "SUPERADMIN@VESTA.COM",
            EmailConfirmed = true,
            AccessFailedCount = 0,
            FirstName = "Super",
            LastName = "Admin",
            SecurityStamp = Guid.NewGuid().ToString(),
            //StudiedProgram = "Everything",
            Phone = "123456789012",
            PhoneNumberConfirmed = true,
            Birthdate = new DateOnly(1999,
                10,
                24),
            GenderId = 1,
            IsTermsAndConditionsAccepted = true,
            ConcurrencyStamp = Guid.NewGuid()
                .ToString(),
            PhoneNumber = "",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = systemUserId,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = systemUserId,
            IsDeleted = false,
        };

        superAdmin.CreatePasswordHash("123vesta-_-");

        User studentUser1 = new()
        {
            Id = userId1,
            UserName = "StudentUser1",
            NormalizedUserName = "STUDENTUSER1",
            Email = "StudentUser1@vesta.com",
            NormalizedEmail = "STUDENTUSER1@VESTA.COM",
            EmailConfirmed = true,
            AccessFailedCount = 0,
            FirstName = "Student",
            LastName = "User",
            SecurityStamp = Guid.NewGuid().ToString(),
            //StudiedProgram = "Default Program",
            Phone = "123456789012",
            PhoneNumberConfirmed = true,
            Birthdate = new DateOnly(1999,
                10,
                24),
            GenderId = 1,
            IsTermsAndConditionsAccepted = true,
            ConcurrencyStamp = Guid.NewGuid()
                .ToString(),
            PhoneNumber = "",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = systemUserId,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = systemUserId,
            IsDeleted = false,
        };

        studentUser1.CreatePasswordHash("123vesta-_-");
        
        User studentUser2 = new()
        {
            Id = userId2,
            UserName = "StudentUser2",
            NormalizedUserName = "STUDENTUSER2",
            Email = "StudentUser2@vesta.com",
            NormalizedEmail = "STUDENTUSER2@VESTA.COM",
            EmailConfirmed = true,
            AccessFailedCount = 0,
            FirstName = "Student",
            LastName = "User 2",
            SecurityStamp = Guid.NewGuid().ToString(),
            Phone = "123456789012",
            PhoneNumberConfirmed = true,
            Birthdate = new DateOnly(1999,
                10,
                24),
            GenderId = 1,
            IsTermsAndConditionsAccepted = true,
            ConcurrencyStamp = Guid.NewGuid()
                .ToString(),
            PhoneNumber = "",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = systemUserId,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = systemUserId,
            IsDeleted = false,
        };
        
        studentUser2.CreatePasswordHash("123vesta-_-");
        
        builder.Entity<User>().HasData(consumer, studentUser1, studentUser2, superAdmin);

        builder.Entity<UserRole>().HasData(new List<UserRole>
        {
            new()
            {
                RoleId = consumerRoleId,
                UserId = consumerId1
            },
            new()
            {
                RoleId = userRoleId,
                UserId = userId1
            },
            new()
            {
                RoleId = userRoleId,
                UserId = userId2
            },
            new()
            {
                RoleId = superAdminRoleId,
                UserId = superAdminId
            }
        });
    }
    
    private static void CreatePasswordHash(this User user, string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, password);
    }
}
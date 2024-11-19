using Showcase.Identity.Data.Entities;

namespace Showcase.Identity.Data.Models;

public record SessionCreateModel(UserTokenModel UserTokenModel, UserTokenViewModel UserTokenViewModel, string Agent, User User);

public record SessionSignalRConnectionUpdateModel(string SignalRConnectionId);
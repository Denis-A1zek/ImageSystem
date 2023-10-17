namespace ImageSystem.Web.Common.Models;

public sealed record CreatedImageInfo(Guid UserId, string FileName, string RelativePath);

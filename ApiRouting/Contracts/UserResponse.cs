namespace UserManagementAPI.Contracts;

/// <summary>Represents a user returned by the API.</summary>
public sealed record UserResponse(int Id, string Name, string Email, string Department);

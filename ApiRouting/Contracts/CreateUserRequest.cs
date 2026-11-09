using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Contracts;

/// <summary>Payload for creating a user.</summary>
public sealed record CreateUserRequest
{
    /// <summary>The user's full name.</summary>
    [Required, StringLength(100, MinimumLength = 1)]
    public required string Name { get; init; }

    /// <summary>The user's unique email address.</summary>
    [Required, EmailAddress, StringLength(254)]
    public required string Email { get; init; }

    /// <summary>The department the user belongs to.</summary>
    [Required, StringLength(100, MinimumLength = 1)]
    public required string Department { get; init; }
}

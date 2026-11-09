using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Contracts;

/// <summary>Payload for replacing a user's details.</summary>
public sealed record UpdateUserRequest
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

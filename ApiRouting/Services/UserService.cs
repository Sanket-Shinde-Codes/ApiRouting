using UserManagementAPI.Contracts;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services;

internal sealed class UserService : IUserService
{
    private readonly object _lock = new();
    private readonly SortedDictionary<int, User> _users = [];
    private readonly HashSet<string> _emails = new(StringComparer.OrdinalIgnoreCase);
    private int _nextId = 1;

    public IReadOnlyList<UserResponse> GetAll()
    {
        lock (_lock)
        {
            return _users.Values
                .Select(ToResponse)
                .ToArray();
        }
    }

    public UserResponse? GetById(int id)
    {
        lock (_lock)
        {
            return _users.TryGetValue(id, out var user) ? ToResponse(user) : null;
        }
    }

    public UserResponse? TryCreate(CreateUserRequest request)
    {
        lock (_lock)
        {
            var email = request.Email.Trim();
            if (_emails.Contains(email))
            {
                return null;
            }

            var user = new User(
                _nextId++,
                request.Name.Trim(),
                email,
                request.Department.Trim());
            _users.Add(user.Id, user);
            _emails.Add(user.Email);
            return ToResponse(user);
        }
    }

    public UserResponse? Update(int id, UpdateUserRequest request, out bool emailConflict)
    {
        lock (_lock)
        {
            emailConflict = false;
            if (!_users.TryGetValue(id, out var existingUser))
            {
                return null;
            }

            var email = request.Email.Trim();
            if (!string.Equals(existingUser.Email, email, StringComparison.OrdinalIgnoreCase) &&
                _emails.Contains(email))
            {
                emailConflict = true;
                return null;
            }

            var user = new User(
                id,
                request.Name.Trim(),
                email,
                request.Department.Trim());
            _emails.Remove(existingUser.Email);
            _emails.Add(user.Email);
            _users[id] = user;
            return ToResponse(user);
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            if (!_users.Remove(id, out var user))
            {
                return false;
            }

            _emails.Remove(user.Email);
            return true;
        }
    }

    private static UserResponse ToResponse(User user) =>
        new(user.Id, user.Name, user.Email, user.Department);
}

using UserManagementAPI.Contracts;

namespace UserManagementAPI.Services;

internal interface IUserService
{
    IReadOnlyList<UserResponse> GetAll();
    UserResponse? GetById(int id);
    UserResponse? TryCreate(CreateUserRequest request);
    UserResponse? Update(int id, UpdateUserRequest request, out bool emailConflict);
    bool Delete(int id);
}

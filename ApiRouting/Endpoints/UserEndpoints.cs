using UserManagementAPI.Contracts;
using UserManagementAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Endpoints;

internal static class UserEndpoints
{
    public static WebApplication MapUsers(this WebApplication app)
    {
        var users = app.MapGroup("/api/users")
            .WithTags("Users");

        users.MapGet("/", (IUserService service, CancellationToken cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return TypedResults.Ok(service.GetAll());
            })
            .WithName("GetUsers")
            .WithSummary("Get all users")
            .WithDescription("Returns all users ordered by their ID.")
            .Produces<IReadOnlyList<UserResponse>>(StatusCodes.Status200OK);

        users.MapGet("/{id:int}", Results<Ok<UserResponse>, NotFound> (
                int id, IUserService service, CancellationToken cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var user = service.GetById(id);
                return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
            })
            .WithName("GetUserById")
            .WithSummary("Get a user by ID")
            .WithDescription("Returns one user, or 404 when the ID does not exist.")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        users.MapPost("/", Results<Created<UserResponse>, Conflict<ProblemDetails>> (
                CreateUserRequest request,
                IUserService service,
                CancellationToken cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var user = service.TryCreate(request);
                if (user is null)
                {
                    return TypedResults.Conflict(new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Email address already exists",
                        Detail = "A user with this email address already exists."
                    });
                }

                return TypedResults.Created($"/api/users/{user.Id}", user);
            })
            .WithName("CreateUser")
            .WithSummary("Create a user")
            .WithDescription("Creates a user and returns its resource URL.")
            .Produces<UserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict);

        users.MapPut("/{id:int}", Results<Ok<UserResponse>, NotFound, Conflict<ProblemDetails>> (
                int id,
                UpdateUserRequest request,
                IUserService service,
                CancellationToken cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var user = service.Update(id, request, out var emailConflict);
                if (emailConflict)
                {
                    return TypedResults.Conflict(new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Email address already exists",
                        Detail = "A user with this email address already exists."
                    });
                }

                return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
            })
            .WithName("UpdateUser")
            .WithSummary("Replace a user's details")
            .WithDescription("Updates all user fields, or returns 404 when the ID does not exist.")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        users.MapDelete("/{id:int}", Results<NoContent, NotFound> (
                int id, IUserService service, CancellationToken cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return service.Delete(id) ? TypedResults.NoContent() : TypedResults.NotFound();
            })
            .WithName("DeleteUser")
            .WithSummary("Delete a user")
            .WithDescription("Deletes one user, or returns 404 when the ID does not exist.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}

using System.Text.Json.Serialization;
using UserManagementAPI.Endpoints;
using UserManagementAPI.Middleware;
using UserManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});
builder.Services.AddValidation();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseMiddleware<BearerTokenAuthenticationMiddleware>();
app.UseStatusCodePages();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/api/_diagnostics/throw", (HttpContext _) =>
        throw new InvalidOperationException("Development-only exception test."));
}

app.MapUsers();

if (string.IsNullOrWhiteSpace(builder.Configuration["Authentication:BearerToken"]))
{
    app.Logger.LogWarning(
        "Authentication:BearerToken is not configured. API requests will be rejected until a token is configured.");
}

app.Run();

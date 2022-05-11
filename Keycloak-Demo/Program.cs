using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(options =>
{
    // Store the session to cookies
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // OpenId authentication
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie("Cookies")
.AddOpenIdConnect(options =>
{
    // URL of the Keycloak server
    options.Authority = Environment.GetEnvironmentVariable("KEYCLOAK_ENDPOINT");
    // Client configured in the Keycloak
    options.ClientId = Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_ID");

    // For testing we disable https (should be true for production)
    options.RequireHttpsMetadata = false;
    options.SaveTokens = true;

    // Client secret shared with Keycloak
    options.ClientSecret = Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_SECRET");
    options.GetClaimsFromUserInfoEndpoint = true;

    // OpenID flow to use
    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        NameClaimType = "email",
        RoleClaimType = "user_roles",
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("user_roles", "Admin"));
});

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

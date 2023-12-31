using JwtAspNet.Services;
using JwtAspNet.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JwtAspNet;
using System.Security.Claims;
using JwtAspNet.Extension;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<TokenService>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(x =>
x.AddPolicy("Admin", p => p.RequireRole("admin"))
);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", (TokenService service) =>

{
    var user = new User(
    Id: 1,
    Name: "Dai",
    Email: "dai@gmail.com",
    Image: "imagem do usuário",
    Password: "qualquer",
    Roles: new[] { "student", "premium" });
    return service.Create(user);
});

app.MapGet("/restrito", (ClaimsPrincipal user) => new
{
    id = user.Id(),
    name = user.Name(),
    email = user.Email(),
    giveName = user.GivenName(),
    image = user.Image(),
})
    .RequireAuthorization();
app.MapGet("/admin", () => "Você tem acesso")
    .RequireAuthorization("Admin");

app.Run();

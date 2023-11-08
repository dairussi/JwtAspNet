using JwtAspNet.Services;
using JwtAspNet.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<TokenService>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", (TokenService service) =>

{
    var user = new User(
    Id: 1,
    Name: "Dai",
    Email: "dai.russi@gmail.com",
    Image: "qualquer",
    Password: "qqq",
    Roles: new[] { "student", "premium" });
    return service.Create(user);
}).RequireAuthorization();

app.Run();

using System.Text;
using Api.Cloudinary;
using Api.Context;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(op =>
{
    op.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
builder.Services.AddScoped<ITokenServices, TokenServices>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokenkey"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddCors(op => op.AddDefaultPolicy(policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); })
);
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddLogging();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.MapControllers();

app.Run();
